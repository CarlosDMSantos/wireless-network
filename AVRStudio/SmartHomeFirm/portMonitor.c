/*
* portMonitor.c
*
* Created: 25/01/2013 17:27:24
*  Author: Victor
*/


#include "EEPROM.h"
#include "DIGITAL.h"
#include "ANALOG.h"
#include "globals.h"
#include "config.h"
#include "command.h"

uint8_t lastValuesD[] = {0,0,0,0,0};//Ha de inicializarse con los valores al iniciar
uint8_t lastValuesA[] = {0,0,0,0,0,0,0,0};
//uint8_t lastChangeSign = 0; //0 -> positive  1 -> negative	

void CheckInputs_task()
{
	for(uint8_t port=0; port<5; port++)
	{
		uint8_t val;
		PORT_CONFIG_t config;
		_Bool changeOcurred = 0;
	
		switch(port)
		{
			case 0: val = HAL_GPIO_PORTB_read(); config = runningConfiguration.topConfiguration.portConfig_PB; break;
			case 1: val = HAL_GPIO_PORTD_read(); config = runningConfiguration.topConfiguration.portConfig_PD; break;
			case 2: val = HAL_GPIO_PORTE_read(); config = runningConfiguration.topConfiguration.portConfig_PE; break;
			case 3: val = HAL_GPIO_PORTF_read(); config = runningConfiguration.topConfiguration.portConfig_PF; break;
			case 4: val = HAL_GPIO_PORTG_read(); config = runningConfiguration.topConfiguration.portConfig_PG; break;
		}
	
		for(uint8_t pin=0; pin<8; pin++)
		{
			if(((~config.maskIO)>>pin) & 0x01) //Input
			{
				if((config.maskAD>>pin) & 0x01) //Digital
				{
					//Check for valid changes
					switch((config.changeTypeD>>(pin<<1)))
					{
						case FALLING_EDGE: changeOcurred = ( ((lastValuesD[port]>>pin) & 0x01) & (~((val>>pin) & 0x01)) ); break;
						case RISIN_EDGE: changeOcurred = ( (~((lastValuesD[port]>>pin) & 0x01)) & ((val>>pin) & 0x01) ); break;
						case BOTH_EDGE: changeOcurred = ( ((lastValuesD[port]>>pin) & 0x01) != ((val>>pin) & 0x01) ); break;
					}
				}else //Analog
				{
					uint8_t analog_val = ADC_Read(pin);
					ANALOG_EVENT_CONFIG_t analog_config;
					
					switch(pin)
					{
						case ADC0: analog_config = runningConfiguration.topConfiguration.analogConfig_ADC0; break;
						case ADC1: analog_config = runningConfiguration.topConfiguration.analogConfig_ADC1; break;
						case ADC2: analog_config = runningConfiguration.topConfiguration.analogConfig_ADC2; break;
						case ADC3: analog_config = runningConfiguration.topConfiguration.analogConfig_ADC3; break;
						case ADC4: analog_config = runningConfiguration.topConfiguration.analogConfig_ADC4; break;
						case ADC5: analog_config = runningConfiguration.topConfiguration.analogConfig_ADC5; break;
						case ADC6: analog_config = runningConfiguration.topConfiguration.analogConfig_ADC6; break;
						case ADC7: analog_config = runningConfiguration.topConfiguration.analogConfig_ADC7; break;
					}
					
					//int16_t diff = lastValuesA[pin] - analog_val;
					//if(diff>=0) //positive
					//{
						//changeOcurred = diff > analog_config.increment;
						//lastChangeSign &= ~(1<<pin);
					//}else
					//{
						//lastChangeSign |= (1<<pin);						
					//}
					
					if(changeOcurred = (abs(lastValuesA[pin] - analog_val) >= analog_config.increment))
					{
						lastValuesA[pin] = analog_val;	
					}
				}
			}
			
			//Launch Events if exits
			if(changeOcurred)
			{
				launchEvents((port*8) + pin);
			}
			
		}
		
		lastValuesD[port] = val;
	}
}

void launchEvents(uint8_t pinAddress)
{
	uint8_t pin_event_list_addr = runningConfiguration.raw[EVENT_TABLE_ADDR + pinAddress]; //Event address relative to the end of the event table
	uint8_t pin_event_list_length = runningConfiguration.raw[EVENT_TABLE_ADDR + pinAddress + 1];
	
	uint8_t table_restriction_addr = runningConfiguration.raw[EVENT_TABLE_ADDR + NUM_PINS + 2];//Relative
	
	uint8_t table_enable_addr = runningConfiguration.raw[EVENT_TABLE_ADDR + NUM_PINS + 3];//Event flags address relative to the end of the event table
	uint8_t enable_pin_number = runningConfiguration.raw[EVENT_TABLE_END_ADDR + table_enable_addr + pinAddress];
	uint8_t enable_pin_addr_relative = (enable_pin_number / 8); //Relative to the end of the enable event table

	uint16_t enable_pin_addr_absolute = (EVENT_TABLE_END_ADDR + table_enable_addr + NUM_PINS + 2) + enable_pin_addr_relative;
	uint8_t pin_ptr = (enable_pin_number - enable_pin_addr_relative);
	
	uint8_t res_ptr;
	
	for(res_ptr = table_restriction_addr; res_ptr < table_enable_addr; res_ptr += sizeof(EVENT_RESTRICTION_t))
	{
		EVENT_RESTRICTION_t* restric = (EVENT_RESTRICTION_t*)&runningConfiguration.raw[res_ptr + EVENT_TABLE_END_ADDR];
			
		if(restric->eventAddress >= pin_event_list_addr) break;
	}
	
	for(uint8_t i=pin_event_list_addr; i < pin_event_list_length;)
	{
		EVENT_RESTRICTION_t* restric = (EVENT_RESTRICTION_t*)&runningConfiguration.raw[EVENT_TABLE_END_ADDR + res_ptr];
		EVENT_HEADER_t* event_header = (EVENT_HEADER_t*)&runningConfiguration.raw[EVENT_TABLE_END_ADDR + i];//DestAddress AND OP_CODE
		
		uint8_t args_length = getCommandArgsLenght(event_header->operation);
		
		_Bool restriction_passed = 1;
		if(restric->eventAddress == (EVENT_TABLE_END_ADDR + i)) //Restriction for the current event?
		{
			restriction_passed = (compareTimes(restric->start, currentTime) <= 0) && (compareTimes(restric->end, currentTime) > 0);
			res_ptr += sizeof(EVENT_RESTRICTION_t);
		}		 
		
		
		if( ((runningConfiguration.raw[enable_pin_addr_absolute] >> (8 - pin_ptr)) & 0x01) && //Is enabled 
			restriction_passed ) //if time restriction ok
		{
			RF_Send_Event(i);
		}
		
		pin_ptr++;
		if(pin_ptr == 8)
		{
			pin_ptr = 0;
			enable_pin_addr_absolute++;
		}
		
		i += args_length + sizeof(EVENT_HEADER_t);
	}
}