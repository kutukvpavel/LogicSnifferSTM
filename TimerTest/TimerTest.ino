/*
 Name:		TimerTest.ino
 Created:	29.07.2018 0:35:38
 Author:	Павел
*/

USBSerial usb;
HardwareTimer milli(2);
HardwareTimer micro(3);

void USBPrintln(char const* text)
{
	if (usb) usb.println(text);
}

// the setup function runs once when you press reset or power the board
void setup() {
	pinMode(LED_BUILTIN, OUTPUT_OPEN_DRAIN);
	digitalWrite(LED_BUILTIN, LOW);
	for (uint16_t i = 0; i < 10000; i++)
	{
		if (usb) break;
		delay(1);
	}
	delay(1000);
	digitalWrite(LED_BUILTIN, HIGH);
	USBPrintln("Goodnight moon!");
	rcc_clk_enable(RCC_TIMER2);
	rcc_clk_enable(RCC_TIMER3);
	micro.setPrescaleFactor(72U); //1uS exactly
	micro.setOverflow(1000);
	milli.setPrescaleFactor(1); //Somehow default prescaler turned out to be 2
	TIMER3->regs.bas->CR2 |= TIMER_CR2_MMS_UPDATE; //Setup reload trigger TRGO3 (TRGO3 <--> ITR2, see RM0008)
	TIMER2->regs.adv->SMCR |= (TIMER_SMCR_TS_ITR2 | TIMER_SMCR_SMS_EXTERNAL); //Enable internal trigger tied to TIM3 and external clock mode
}

// the loop function runs over and over again until power down or reset
void loop() {
	micro.pause();
	micro.refresh();
	milli.refresh();
	while (!usb);
	while (usb.read() != 'D');
	micro.resume();
	for (int i = 0; i < 40; i++)
	{
		uint16 mic = micro.getCount();
		uint16 mil = milli.getCount();
		if (mic > micro.getCount())	mil = milli.getCount(); //Update if rollover has just happened
		usb.print(mil);
		usb.write(':');
		usb.println(mic);
		delay_us(100);
	}
	
}
