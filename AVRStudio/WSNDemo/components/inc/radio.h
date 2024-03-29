/*
 * radio.h
 *
 * Created: 28/01/2013 14:27:58
 *  Author: Victor
 */ 


#ifndef RADIO_H_
#define RADIO_H_

#include "EEPROM.h"
#include <stdbool.h>

typedef enum RadioState_t
{
	RF_STATE_INITIAL,
	RF_STATE_READY_TO_SEND,
	RF_STATE_WAIT_CONF,
	RF_STATE_PREPARE_TO_SLEEP,
	RF_STATE_SLEEP,
	RF_STATE_WAKEUP,
} RadioState_t;

typedef enum BufferType_t
{
	RF_BUFFER_NONE,
	RF_BUFFER_REFERENCES,
	RF_BUFFER_COPIES
} BufferType_t;

_Bool Radio_AddMessageByCopy(OPERATION_HEADER_t* message);
_Bool Radio_AddMessageByReference(OPERATION_HEADER_t* message);

void Radio_Init(void);

#endif /* RADIO_H_ */