/**************************************************************************//**
  \file  halTaskManager.c

  \brief Implemenattion of HAL task manager.

  \author
      Atmel Corporation: http://www.atmel.com \n
      Support email: avr@atmel.com

    Copyright (c) 2008-2012, Atmel Corporation. All rights reserved.
    Licensed under Atmel's Limited License Agreement (BitCloudTM).

  \internal
    History:
      10/12/07 A. Khromykh - Created
 ******************************************************************************/
/******************************************************************************
 *   WARNING: CHANGING THIS FILE MAY AFFECT CORE FUNCTIONALITY OF THE STACK.  *
 *   EXPERT USERS SHOULD PROCEED WITH CAUTION.                                *
 ******************************************************************************/

/******************************************************************************
                   Includes section
******************************************************************************/
#include <halTaskManager.h>
#include <atomic.h>

/******************************************************************************
                   Global variables section
******************************************************************************/
void (* extHandler)(void);

/******************************************************************************
                   Prototypes section
******************************************************************************/
/**************************************************************************//**
\brief Interrupt handler of appTimer clock.
******************************************************************************/
void halAppTimerHandler(void);

/**************************************************************************//**
\brief  HAL USART task. Exact ection depends on USART internal task.
******************************************************************************/
void halSigUsartHandler(void);

/**************************************************************************//**
\brief twi end of sending handler
******************************************************************************/
void halSig2WireSerialHandler(void);

/**************************************************************************//**
\brief Interrupt handler about write completion to EEPROM.
******************************************************************************/
void halSigEepromReadyHandler(void);

/**************************************************************************//**
\brief Interrupt handler about sleep interval was completed.
******************************************************************************/
void halAsyncTimerHandler(void);

/***************************************************************************//**
\brief  Shutdown system. \n
  NOTES:  \n
  the application should be sure the poweroff will not be \n
  interrupted after the execution of the sleep().
*******************************************************************************/
void halPowerOff(void);

/**************************************************************************//**
\brief ADC interrupt handler.
******************************************************************************/
void halSigAdcHandler(void);

/**************************************************************************//**
\brief Endpoint interrupt handler.
******************************************************************************/
void halEndPointHandler(void);

/**************************************************************************//**
\brief usb suspend interrupt handler.
******************************************************************************/
void halSuspendHandler(void);

/**************************************************************************//**
\brief usb resume interrupt handler.
******************************************************************************/
void halResumeHandler(void);

/**************************************************************************//**
\brief usb bus reset interrupt handler.
******************************************************************************/
void halBusResetHandler(void);

/**************************************************************************//**
\brief Handler for task manager. It is executed when system has waked up.
******************************************************************************/
void halWakeupHandler(void);

/**************************************************************************//**
\brief Security Module request handler.
******************************************************************************/
void halSmRequestHandler(void);

/**************************************************************************//**
\brief Synchronization system time which based on sleep timer.
******************************************************************************/
void halSleepSystemTimeSynchronize(void);

/**************************************************************************//**
\brief Slave spi reception complete interrupt handler.
******************************************************************************/
void halSpiRxByteComplete(void);

/******************************************************************************
                   Global variables section
******************************************************************************/
volatile uint8_t halTaskFlags0 = 0;
volatile uint8_t halTaskFlags1 = 0;
volatile uint8_t halTaskFlags2 = 0;
volatile uint8_t halTaskFlags3 = 0;
volatile uint8_t halTaskFlags4 = 0;

/******************************************************************************
                   Implementations section
******************************************************************************/
/******************************************************************************
HAL task handler.
******************************************************************************/
void HAL_TaskHandler()
{
  if (halTaskFlags4 & HAL_TIMER4_COMPA)
  {
    ATOMIC_SECTION_ENTER
      halTaskFlags4 &= (~HAL_TIMER4_COMPA);
    ATOMIC_SECTION_LEAVE
    halAppTimerHandler();
  }

#if !defined(AT90USB1287)
  else if (halTaskFlags0 & HAL_ASYNC_TIMER)
  {
    ATOMIC_SECTION_ENTER
      halTaskFlags0 &= (~HAL_ASYNC_TIMER);
    ATOMIC_SECTION_LEAVE
    halAsyncTimerHandler();
  }
  else if (halTaskFlags0 & HAL_SYNC_SLEEP_TIME)
  {
    ATOMIC_SECTION_ENTER
      halTaskFlags0 &= (~HAL_SYNC_SLEEP_TIME);
    ATOMIC_SECTION_LEAVE
    halSleepSystemTimeSynchronize();
  }
  else if (halTaskFlags4 & HAL_WAKEUP)
  {
    ATOMIC_SECTION_ENTER
      halTaskFlags4 &= (~HAL_WAKEUP);
    ATOMIC_SECTION_LEAVE
    halWakeupHandler();
  }
  else if (halTaskFlags4 & HAL_SLEEP)
  {
    ATOMIC_SECTION_ENTER
      halTaskFlags4 &= (~HAL_SLEEP);
    ATOMIC_SECTION_LEAVE
    halPowerOff();
  }
#endif // !defined(AT90USB1287)

#ifdef HAL_USE_SPI
  else if (halTaskFlags2 & HAL_TASK_SPI)
  {
    ATOMIC_SECTION_ENTER
      halTaskFlags2 &= (~HAL_TASK_SPI);
    ATOMIC_SECTION_LEAVE
    halSpiRxByteComplete();
  }
#endif // HAL_USE_SPI

#ifdef HAL_USE_USART
  else if (halTaskFlags2 & HAL_TASK_USART)
  {
    ATOMIC_SECTION_ENTER
      halTaskFlags2 &= (~HAL_TASK_USART);
    ATOMIC_SECTION_LEAVE
    halSigUsartHandler();
  }
#endif // HAL_USE_USART

#ifdef HAL_USE_ADC
  else if (halTaskFlags3 & HAL_ADC)
  {
    ATOMIC_SECTION_ENTER
      halTaskFlags3 &=  (~HAL_ADC);
    ATOMIC_SECTION_LEAVE
    halSigAdcHandler();
  }
#endif // HAL_USE_ADC

#ifdef HAL_USE_EE_READY
  else if (halTaskFlags3 & HAL_EE_READY)
  {
    ATOMIC_SECTION_ENTER
      halTaskFlags3 &= (~HAL_EE_READY);
    ATOMIC_SECTION_LEAVE
    halSigEepromReadyHandler();
  }
#endif // HAL_USE_EE_READY

#ifdef HAL_USE_USB
  else if (halTaskFlags3 & HAL_USB_ENDPOINTS)
  {
    ATOMIC_SECTION_ENTER
      halTaskFlags3 &= (~HAL_USB_ENDPOINTS);
    ATOMIC_SECTION_LEAVE
    halEndPointHandler();
  }
  else if (halTaskFlags3 & HAL_USB_SUSPEND)
  {
    ATOMIC_SECTION_ENTER
      halTaskFlags3 &= (~HAL_USB_SUSPEND);
    ATOMIC_SECTION_LEAVE
    halSuspendHandler();
  }
  else if (halTaskFlags3 & HAL_USB_RESUME)
  {
    ATOMIC_SECTION_ENTER
      halTaskFlags3 &= (~HAL_USB_RESUME);
    ATOMIC_SECTION_LEAVE
    halResumeHandler();
  }
  else if (halTaskFlags3 & HAL_USB_BUS_RESET)
  {
    ATOMIC_SECTION_ENTER
      halTaskFlags3 &= (~HAL_USB_BUS_RESET);
    ATOMIC_SECTION_LEAVE
    halBusResetHandler();
  }
#endif // HAL_USE_USB

#ifdef HAL_USE_HW_AES
  else if (halTaskFlags3 & HAL_SM_REQ)
  {
    ATOMIC_SECTION_ENTER
      halTaskFlags3 &= (~HAL_SM_REQ);
    ATOMIC_SECTION_LEAVE
    halSmRequestHandler();
  }
#endif // HAL_USE_HW_AES

#ifdef HAL_USE_TWI
  else if (halTaskFlags4 & HAL_TWI)
  {
    ATOMIC_SECTION_ENTER
      halTaskFlags4 &= (~HAL_TWI);
    ATOMIC_SECTION_LEAVE
    halSig2WireSerialHandler();
  }
#endif // HAL_USE_TWI

#ifdef HAL_USE_EXT_HANDLER
  else if (halTaskFlags4 & HAL_EXT_HANDLER)
  {
    ATOMIC_SECTION_ENTER
      halTaskFlags4 &= (~HAL_EXT_HANDLER);
    ATOMIC_SECTION_LEAVE
    if (extHandler)
      extHandler();
  }
#endif // HAL_USE_EXT_HANDLER

  if (halTaskFlags0 ||
      halTaskFlags1 ||
      halTaskFlags2 ||
      halTaskFlags3 ||
      halTaskFlags4)
  {
    SYS_PostTask(HAL_TASK_ID);
  }
}

// eof halTaskManager.c
