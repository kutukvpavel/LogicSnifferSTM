/*
 Name:		LogicSnifferSTM.ino
 Created:	18.07.2018 19:19:22
 Author:	Павел


*/

#define ACQUISITION_PARTS 1 //Divides DEPTH into parts, better be even
#define ACQUISITION_DEPTH 2560 //Edges, DEPTH * (2<uint16 milliseconds> + 2<uint16 microsecond> + 1<bool data>) < 15000 <3/4 of available RAM>
#define MICROSECOND_TIMER_OVERFLOW 1000U
#define ACQUISITION_DELAY 5 //Empiric
//Acquisition pin properties
#define PIN_NUM PB12
#define PIN_REG GPIOB_BASE
#define PIN_BIT 12
//Capture button pin properties
#define BUTTON_PIN_NUM PA1
#define BUTTON_PIN_REG GPIOA_BASE
#define BUTTON_PIN_BIT 1
#define SOURCE_PIN_NUM PA0 //Pulled down (ground) pin for button (button had a 2-pin header so it had to be right next to button pin)

#define NOP __asm__ __volatile__("nop\n\t")

USBSerial usb;
HardwareTimer milli(2); //Chained 16-bit timers can provide up to 32-bit resolution (depending on micro overflow value)  
HardwareTimer micro(3);

void setup() {
	//Wait for PC for 10 seconds maximum
	pinMode(LED_BUILTIN, OUTPUT_OPEN_DRAIN);
	digitalWrite(LED_BUILTIN, LOW);
	for (uint16_t i = 0; i < 10000; i++)
	{
		if (usb) break;
		delay(1);
	}
	delay(1000);
	digitalWrite(LED_BUILTIN, HIGH);
	if (usb) usb.println("Goodnight moon!");
	//Pull-down configuration for the whole acquisition port (this way is prettier than digitalWrite with cycles, isn't it)
	PIN_REG->ODR = 0U; //Pull-down = 0, pull-up = 1
	PIN_REG->CRL = 0x88888888U; //Control register bits for each pulled input pin are 1000 (8 in HEX)
	PIN_REG->CRH = 0x88888888U;
	//Button-related pins
	pinMode(BUTTON_PIN_NUM, INPUT_PULLUP);
	pinMode(SOURCE_PIN_NUM, OUTPUT);
	digitalWrite(SOURCE_PIN_NUM, LOW);
	//Initialize millisecond and microsecond chained timers
	rcc_clk_enable(RCC_TIMER2); //Not sure if needed inside arduino environment (reset value of RCC regs is 0 though)
	rcc_clk_enable(RCC_TIMER3);
	micro.pause(); //Stop fast timer (it's useless until capturing starts)
	micro.setPrescaleFactor(72U); //1uS exactly
	micro.setOverflow(MICROSECOND_TIMER_OVERFLOW);
	milli.setPrescaleFactor(1); //Somehow default prescaler turned out to be 2
	milli.setOverflow(UINT16_MAX); //Maximum possible (rollover will happen after almost a minute of capturing)
	//Core libraries provide no functions to setup master-slave synchronization
	//Setup master mode (edge upon reload) for TRGO3 (TRGO3 <--> ITR2, see RM0008)
	TIMER3->regs.bas->CR2 |= TIMER_CR2_MMS_UPDATE; 
	TIMER2->regs.adv->SMCR |= (TIMER_SMCR_TS_ITR2 | TIMER_SMCR_SMS_EXTERNAL); //Setup slave mode (ITR2 clocks timer)
	milli.resume(); //Ensure externally clocked timer is enabled (not sure if needed for arduino environment)
}

void loop() {
	//Setup data arrays
	uint16 timestamp[ACQUISITION_DEPTH * 2];
	bool data[ACQUISITION_DEPTH];
	bool* stop[ACQUISITION_PARTS];
	bool* dat;
	uint16* tim;
	bool* lim;
	bool** stp;
	for (uint8 i = 0; i < ACQUISITION_PARTS; i++)
	{
		micro.refresh(); //Not only resets the values but also ensures that settings are applied (from setup)
		milli.refresh();
		dat = data + (ACQUISITION_DEPTH / ACQUISITION_PARTS) * i; //Setup pointers to allocated data buffers (to allow pointer arithmetic array access)
		tim = timestamp + (ACQUISITION_DEPTH / ACQUISITION_PARTS) * 2 * i;
		*tim++ = 0; *tim = 0;
		lim = dat + (ACQUISITION_DEPTH / ACQUISITION_PARTS); //Index of the last element
		stp = stop + i;
		//Wait until user enables capturing
		while (digitalRead(BUTTON_PIN_NUM));
		digitalWrite(LED_BUILTIN, LOW); //Led is powered by low-side switch [ON=LOW=CAPTURING]
		delay(400); //Debouncing
		noInterrupts(); //Obviously, disable all interrupts
		Acquisition(dat, tim, stp, lim);
		interrupts(); //Return to normal operation
		digitalWrite(LED_BUILTIN, HIGH);
		delay(400);
	}
	//Wait for PC to dump data.
	while (!usb);
	while (usb.read() != 'D');
	usb.println("Dumping data...");
	for (uint8 i = 0; i < ACQUISITION_PARTS; i++)
	{
		dat = data + (ACQUISITION_DEPTH / ACQUISITION_PARTS) * i;
		tim = timestamp + (ACQUISITION_DEPTH / ACQUISITION_PARTS) * 2 * i + 2;
		lim = stop[i];
		usb.print(0, DEC); //First element has to be dumped outside of the loop due to rollover check mechanics
		usb.write(':');
		usb.println(*dat++);
		while (dat <= lim)
		{
			if ((tim[1] < tim[-1]) && (tim[0] == tim[-2])) //If we've got a rollover between timer polls
			{
				tim[1] = MICROSECOND_TIMER_OVERFLOW - 1; //Then this is definitely more accurate then incrementing mS
			}
			usb.print(MICROSECOND_TIMER_OVERFLOW * static_cast<uint32>(tim[0]) + static_cast<uint32>(tim[1]), DEC); //1000*[mS](goes first) + [uS](second)
			usb.write(':');
			usb.println(*dat++);
			tim += 2;
		}
		usb.println("EOF");
	}
}

void Acquisition(bool* dat, uint16* tim, bool** stp, const bool* lim) //This signature is crucial, return statement ruins normal operation (within 1uS intervals)
{
	//Time-critical operations start here
	micro.resume(); //Start the timers and go
	*dat = PIN_REG->IDR & (1U << PIN_BIT); //Read first state now to use pre-increment later
	for (uint8 i = 0; i < ACQUISITION_DELAY; i++) NOP; //sometimes something weird happens like two consecutive zeros at the beginning
	while ((BUTTON_PIN_REG->IDR & (1U << BUTTON_PIN_BIT)) && (dat < lim)) //Loop until full or user abort
	{
		bool sample = PIN_REG->IDR; //All the pins are pulled to ground, change on any of them will trigger writing
		if (sample != *dat) //Takes only several opcodes, corresponding <0.1uS error is negligible 
		{
			*(++tim) = static_cast<uint16>(TIMER2_BASE->CNT); //mS goes first
			*(++tim) = static_cast<uint16>(TIMER3_BASE->CNT); //uS goes after, therefore we are able to fix rollovers during post-processing
			*(++dat) = sample; //Data goes last in order to reduce latency between sampling and timestamp reading
		}
	}
	micro.pause(); //Stop the master timer
	//Time-critical section ends here
	*stp = dat;
}