/*
 * EEPROM.h
 *
 * Created: 12/10/2012 17:42:55
 *  Author: Victor
 */ 


#ifndef EEPROM_H_
#define EEPROM_H_

#include "RTC.h"
#include <avr/eeprom.h>
#include "config.h"


#define TIME_OPERATION_LIST_START_ADDRESS				runningConfiguration.raw[OPERATION_TABLE_ADDR + (NUM_PINS * 2)]		//Time OPERATION list address relative to the end of the OPERATION table
#define OPERATION_RESTRIC_LIST_START_ADDRESS			runningConfiguration.raw[OPERATION_TABLE_ADDR + (NUM_PINS * 2) + 2]
#define FREE_REGION_START_ADDRESS					runningConfiguration.raw[OPERATION_TABLE_ADDR + (NUM_PINS * 2) + 4]

#define TIME_OPERATION_LIST_END_ADDRESS					OPERATION_RESTRIC_LIST_START_ADDRESS		//Renowned for greater understanding
#define OPERATION_RESTRIC_LIST_END_ADDRESS				FREE_REGION_START_ADDRESS

#define NO_EDGE			0
#define FALLING_EDGE	1
#define RISIN_EDGE		2
#define BOTH_EDGE		3

typedef struct{
	uint8_t shieldModel;  
	uint16_t length; //Size of the current config
	uint16_t checkSum; //crc16
	DATE_t updateDate;
	TIME_t updateTime;
}DEVICE_INFO_t;

typedef struct{
	uint16_t deviceAddress;
	uint8_t channel;
	uint16_t panId;
	uint8_t securityKey[16];
}NETWORK_CONFIG_t;

typedef struct{
	unsigned pin0_ChangeType : 2;	//Two for pin	10-> risingEdge		01-> fallingEdge	11->both
	unsigned pin1_ChangeType : 2;
	unsigned pin2_ChangeType : 2;
	unsigned pin3_ChangeType : 2;
	unsigned pin4_ChangeType : 2;
	unsigned pin5_ChangeType : 2;
	unsigned pin6_ChangeType : 2;
	unsigned pin7_ChangeType : 2;
}DIGITAL_PORT_CHANGE_TYPE_t;

typedef struct{
	uint8_t maskIO;   //0-> inputs		1-> outputs
	uint8_t maskAD;   //0-> analog		1-> digital
	uint8_t defaultValuesD;		 //Initial values DIGITAL
	uint16_t changeTypeD; //Two for pin	10-> risingEdge		01-> fallingEdge	11->both
}PORT_CONFIG_t;

typedef struct{
	uint8_t defaultValueA;   //Initial value ANALOG output (PWM)
}PWM_CONFIG_t;

typedef struct{
	uint8_t increment;   //Difference between the last and current value
	uint8_t threshold;	 //hysteresis value to exceed
}ANALOG_EVENT_CONFIG_t;

typedef struct{
	uint16_t destinationAddress;
	uint8_t opCode;
}OPERATION_HEADER_t;

typedef struct{
	TIME_t activationTime;		  //Activation time
	OPERATION_HEADER_t operationHeader;   //Header of the operation to send on activation
}TIME_OPERATION_HEADER_t;

typedef struct
{
	uint16_t operationAddress; //Relative to the end of operation table
	TIME_t start;
	TIME_t end;
}OPERATION_RESTRICTION_t;


typedef struct {
	DEVICE_INFO_t deviceInfo;
	NETWORK_CONFIG_t networkConfig;
	PORT_CONFIG_t portConfig_PA;
	PORT_CONFIG_t portConfig_PB;
	PORT_CONFIG_t portConfig_PC;
	PORT_CONFIG_t portConfig_PD;
	PORT_CONFIG_t portConfig_PE;
	PORT_CONFIG_t portConfig_PF;
	PORT_CONFIG_t portConfig_PG;
	PWM_CONFIG_t pwmConfig_PWM0;
	PWM_CONFIG_t pwmConfig_PWM1;
	PWM_CONFIG_t pwmConfig_PWM2;
	PWM_CONFIG_t pwmConfig_PWM3;
	PWM_CONFIG_t pwmConfig_PWM4;
	PWM_CONFIG_t pwmConfig_PWM5;
	PWM_CONFIG_t pwmConfig_PWM6;
	PWM_CONFIG_t pwmConfig_PWM7;
	ANALOG_EVENT_CONFIG_t analogConfig_ADC0;
	ANALOG_EVENT_CONFIG_t analogConfig_ADC1;
	ANALOG_EVENT_CONFIG_t analogConfig_ADC2;
	ANALOG_EVENT_CONFIG_t analogConfig_ADC3;
	ANALOG_EVENT_CONFIG_t analogConfig_ADC4;
	ANALOG_EVENT_CONFIG_t analogConfig_ADC5;
	ANALOG_EVENT_CONFIG_t analogConfig_ADC6;
	ANALOG_EVENT_CONFIG_t analogConfig_ADC7;
}TOP_CONFIGURATION;

typedef union
{
	TOP_CONFIGURATION  topConfiguration;
	uint8_t raw[EEPROM_SIZE];
}RUNNING_CONFIGURATION_t;

void EEPROM_Init(void);

extern inline uint8_t EEPROM_Read_Byte(int address);
extern inline void EEPROM_Read_Block(void * buffer, const void * address, size_t length);
extern inline void EEPROM_Write_Byte(int address, uint8_t value);
extern inline void EEPROM_Write_Block(void * buffer, const void * address, size_t length);

#endif /* EEPROM_H_ */