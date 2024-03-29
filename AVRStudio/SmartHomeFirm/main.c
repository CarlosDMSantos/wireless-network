#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <util/delay.h>

#include "config.h"
#include "hal.h"
#include "phy.h"
#include "sys.h"
#include "nwk.h"
#include "halUart.h"
#include "halSleep.h"
#include "sysTimer.h"

//LIBRARIES
#include "RTC.h"
#include "ANALOG.h"
#include "DIGITAL.h"
#include "DS2401.h"
#include "DHT11.h"
#include "DISPLAY.h"
#include "BASE_IO.h"

#define DECLARING_GLOBALS
#include "globals.h"

//COMPONENTS
#include "configManager.h"
#include "timeManager.h"
#include "uartManager.h"

#ifdef APP_ENABLE_OTA_SERVER
#include "otaServer.h"
#endif

#ifdef APP_ENABLE_OTA_CLIENT
#include "otaClient.h"
#endif

/*****************************************************************************
*****************************************************************************/
#if APP_ADDR == 0
#define APP_CAPTION     "Coordinator"
#define APP_NODE_TYPE   0
#define APP_COORDINATOR 1
#elif APP_ADDR < 0x8000
#define APP_CAPTION     "Router"
#define APP_NODE_TYPE   1
#define APP_ROUTER      1
#else
#define APP_CAPTION     "End Device"
#define APP_NODE_TYPE   2
#define APP_ENDDEVICE   1
#endif

#define APP_CAPTION_SIZE  (sizeof(APP_CAPTION) - 1)

static SYS_Timer_t appNetworkStatusTimer;

_Bool initialized = false;
_Bool buttonLastPressed = false;
_Bool sendFlag = false;

#if APP_ROUTER || APP_ENDDEVICE
/*****************************************************************************
*****************************************************************************/
static void appNetworkStatusTimerHandler(SYS_Timer_t *timer)
{
	/*if(!IS_COORDINATOR)
		sendFlag = true;*/
	
	(void)timer;
}
#endif

/*****************************************************************************
*****************************************************************************/
uint16_t adcVal;
uint8_t temp;
static void appSendData(void)
{/*
	HAL_UartWriteNumberDEC(currentDate.day,2);
	HAL_UartWriteByte('/');
	HAL_UartWriteNumberDEC(currentDate.month,2);
	HAL_UartWriteByte('/');
	HAL_UartWriteNumberDEC(currentDate.year,4);
	HAL_UartPrint("\t");
	
	HAL_UartWriteNumberDEC(currentTime.hour,2);
	HAL_UartWriteByte(':');
	HAL_UartWriteNumberDEC(currentTime.minute,2);
	HAL_UartWriteByte(':');
	HAL_UartWriteNumberDEC(currentTime.second,2);
	HAL_UartPrint("\r\n");*/
	
	if(VALID_DATETIME)
	{
		DISPLAY_Clear();
		DISPLAY_WriteString("DATE: ");
		DISPLAY_WriteNumberDEC(currentDate.day,2);
		DISPLAY_WriteByte('/');
		DISPLAY_WriteNumberDEC(currentDate.month,2);
		DISPLAY_WriteByte('/');
		DISPLAY_WriteNumberDEC(currentDate.year,4);
		DISPLAY_SetCursor(0,1);
		DISPLAY_WriteString("TIME: ");
		DISPLAY_WriteNumberDEC(currentTime.hour,2);
		DISPLAY_WriteByte(':');
		DISPLAY_WriteNumberDEC(currentTime.minute,2);
		DISPLAY_WriteByte(':');
		DISPLAY_WriteNumberDEC(currentTime.second,2);
	}
	/*
	ADC_Reference(REF_DEFAULT);
	adcVal = ADC_Read(ADC0);
	//ADC_Reference(REF_INTERNAL_16);
	//adcVal = ADC_Read(INTERNAL_TEMP);
	//adcVal = 1.13 * adcVal - 272.8;
	HAL_UartPrint("ADC0: ");
	HAL_UartWriteNumberDEC(adcVal,4);
	HAL_UartPrint("\r\n");*/
	
	//sendData(serialNumber, SERIAL_NUMBER_SIZE);
	
	/*
	temp = DHT11_ReadTemperature(PINADDRESS('D', 0));
	if(temp != DHT11_ERROR)
	{
		HAL_UartPrint("TEMP: ");
		HAL_UartWriteNumberDEC(temp);
		HAL_UartPrint("�C\t");
		
		temp = DHT11_ReadHumidity(PINADDRESS('D', 0));//D0
		HAL_UartPrint("HUM: ");
		HAL_UartWriteNumberDEC(temp);
		HAL_UartPrint("%\r\n");
	}else
	{
		HAL_UartPrint("DHT11 not detected\r\n");
	}*/
	
	sendFlag = false;
}

/*****************************************************************************
*****************************************************************************/
static void appInit(void)
{
	#ifdef APP_ENABLE_OTA_SERVER
	OTA_ServerInit();
	#endif
	#ifdef APP_ENABLE_OTA_CLIENT
	OTA_ClientInit();
	#endif
	
	DISPLAY_Init(PINADDRESS('D',1), PINADDRESS('D',0), PINADDRESS('B',1), PINADDRESS('B',3), PINADDRESS('B',5), PINADDRESS('B',7));
	DISPLAY_WriteString("HELLO!");
	
	#if APP_ROUTER || APP_ENDDEVICE
	appNetworkStatusTimer.interval = 1000;//500
	appNetworkStatusTimer.mode = SYS_TIMER_PERIODIC_MODE;
	appNetworkStatusTimer.handler = appNetworkStatusTimerHandler;
	SYS_TimerStart(&appNetworkStatusTimer);
	
	//HAL_UartPrint("ROUTER\r\n");
	#else
	HAL_UartPrint("COORDINATOR\r\n");
	#endif
	
	buttonLastPressed = BASE_ButtonPressed();
	uint8_t joinCounter = 0;
	while(buttonLastPressed)
	{
		BASE_LedToggle();
		_delay_ms(200);
		
		if(joinCounter++ == (3 * (1000 / 200))) //3 Seconds
		{
			RADIO_StartNetworkJoin();
			break;
		}
		
		buttonLastPressed = BASE_ButtonPressed();
	};
	
	TIME_Init();
	OM_Init();
	RADIO_Init(false);
	MODULES_Init();
	
	BASE_LedOn();
	
	initialized = true;
}

/*****************************************************************************
*****************************************************************************/
static void APP_TaskHandler(void)
{
	if(!initialized)
	{
		appInit();
	}else
	{
		if(buttonLastPressed && BASE_ButtonPressed())
		{
			softReset();
		}
		
		buttonLastPressed = BASE_ButtonPressed();
		
		if(sendFlag)
			appSendData();
	}
}

/*****************************************************************************
*****************************************************************************/
int main(void)
{
	SYS_Init();
	
	USART_Init();
	
	BASE_Init();
	
	CONFIG_Init();
	
	/*
	TIME_t debugTime;
	debugTime.hour = 21;
	debugTime.minute = 59;
	debugTime.second = 00;
	TIME_ValidateTime(&debugTime);
	
	WEEKDAY_t debugWeek;
	DATE_t debugDate;
	debugWeek.flags.Friday = 1;
	debugDate.day = 31;
	debugDate.month = 7;
	debugDate.year = 2013;
	TIME_ValidateDate(&debugDate, &debugWeek);*/
	
	/*if(DS2401_Init())
	{
		if(IS_COORDINATOR)
		{
			//sendData(serialNumber, SERIAL_NUMBER_SIZE);
		}
		else
		{
			HAL_UartPrint("SERIAL NUMBER: ");
			for(int i = 0; i < SERIAL_NUMBER_SIZE - 1; i++)
			{
				HAL_UartWriteNumberHEX(serialNumber[i]);
				HAL_UartWriteByte('-');
			}
			HAL_UartWriteNumberHEX(serialNumber[SERIAL_NUMBER_SIZE - 1]);
			HAL_UartPrint("\r\n\r\n");
		}
	}else
	{
		HAL_UartPrint("SERIAL NUMBER: NOT DETECTED!\r\n");
	}*/

	while (1)
	{
		SYS_TaskHandler();
		HAL_UartTaskHandler();
		#ifdef APP_ENABLE_OTA_SERVER
		OTA_ServerTaskHandler();
		#endif
		#ifdef APP_ENABLE_OTA_CLIENT
		OTA_ClientTaskHandler();
		#endif
		APP_TaskHandler();
	}
}
