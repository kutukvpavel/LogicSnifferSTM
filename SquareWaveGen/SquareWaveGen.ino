/*
 Name:		SquareWaveGen.ino
 Created:	30.07.2018 17:32:03
 Author:	Павел

 Unfortunately, timer approach can hardly provide ~200kHz, PWM can provide up to 8MHz but is too complex for this purpose.
 Better stick to arduino-based NOP bit-banger (see below).

*/

#define USE_TIMER_BASED 0

#if USE_TIMER_BASED

#include <avr\interrupt.h>

ISR(TIMER1_COMPA_vect)
{
	TCNT1H = 0x00;
	TCNT1L = 0x00;
	PORTB ^= 0x01;
}

// the setup function runs once when you press reset or power the board
void setup() {
	TIMSK0 &= ~_BV(TOIE0); //Disable millis interrupt
	pinMode(8, OUTPUT);
	// Timer/Counter 1 initialization
	// Clock source: System Clock
	// Clock value: 16000,000 kHz
	// Mode: Normal top=0xFFFF
	// OC1A output: Discon.
	// OC1B output: Discon.
	// Noise Canceler: Off
	// Input Capture on Falling Edge
	// Timer1 Overflow Interrupt: Off
	// Input Capture Interrupt: Off
	// Compare A Match Interrupt: On
	// Compare B Match Interrupt: Off
	TCCR1A = 0x00;
	TCCR1B = 0x01;
	TCNT1H = 0x00;
	TCNT1L = 0x00;
	ICR1H = 0x00;
	ICR1L = 0x00;
	OCR1AH = 0x00;
	OCR1AL = 0x0A;
	OCR1BH = 0x00;
	OCR1BL = 0x00;
	// Timer/Counter 1 Interrupt(s) initialization
	TIMSK1 = 0x02;
	sei();
}

// the loop function runs over and over again until power down or reset
void loop() {
	while (true);
}

#else

//Bit-banger

#define NOP __asm__ __volatile__ ("nop\n\t")

#define MICROSECOND_DELAY NOP;NOP;NOP;NOP;NOP;NOP;NOP;NOP;NOP; //Almost exactly 1uS half-period on Arduino Nano
#define ADDITIONAL_DELAY NOP;NOP;NOP;

void setup() {
  // put your setup code here, to run once:
  pinMode(8, OUTPUT);
  //TIMSK0 &= ~_BV(TOIE0); //Disable millis interrupt
  noInterrupts(); //Alternative
}

void loop() {
  // put your main code here, to run repeatedly:
  //digitalWrite(8, !digitalRead(8)); //About 5-7uS between transitions
  PORTB^=0x01;
  MICROSECOND_DELAY
  //MICROSECOND_DELAY //1uS, including PORT and jumps
  //ADDITIONAL_DELAY //Compensates time required for PORT instructions and jumps when half-period is more than 1uS
}

#endif // USE_TIMER_BASED