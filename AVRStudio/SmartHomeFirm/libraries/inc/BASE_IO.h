/*
 * BASE_IO.h
 *
 * Created: 29/07/2013 19:52:41
 *  Author: Victor
 */ 


#ifndef BASE_IO_H_
#define BASE_IO_H_

#include <avr/io.h>

typedef enum 
{
	ATMega128RFA1_V1,
	ATMega128RFA1_V2,
} BASE_MODELS;

typedef enum
{
	DEBUG_SHIELD,
} SHIELD_MODELS;

void BASE_Init();

void BASE_LedOn();
void BASE_LedOff();

_Bool BASE_ButtonPressed();

uint8_t BASE_PinAddressToINT(uint8_t pinAddress);

#endif /* BASE_IO_H_ */