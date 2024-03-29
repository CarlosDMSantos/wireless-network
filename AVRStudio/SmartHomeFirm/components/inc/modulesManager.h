/*
 * modulesManger.h
 *
 * Created: 27/01/2013 19:37:33
 *  Author: Victor
 */ 


#ifndef MODULESMANAGER_H_
#define MODULESMANAGER_H_

#include <stdint.h>
#include <stdbool.h>
#include "configManager.h"
#include "radioManager.h"

#include "logic_module.h"
#include "network_module.h"
#include "config_module.h"
#include "time_module.h"
#include "temhum_module.h"
#include "presence_module.h"
#include "dimmer_module.h"

#define EXTENSION_OPCODE 0xFF

#define MODULES_TABLE	  \
LOGIC_MODULE_DEFINITION   \
NETWORK_MODULE_DEFINITION \
CONFIG_MODULE_DEFINITION  \
TIME_MODULE_DEFINITION	  \
TEMHUM_MODULE_DEFINITION  \
PRESENCE_MODULE_DEFINITION  \
DIMMER_MODULE_DEFINITION  \

#define X(a, b, c) a,
typedef enum  {
	MODULES_TABLE
}MODULES_ID;
#undef X


#define COMMANDS_TABLE  \
COMMANDS_TABLE_LOGIC    \
COMMANDS_TABLE_NETWORK  \
COMMANDS_TABLE_CONFIG   \
COMMANDS_TABLE_TIME		\
COMMANDS_TABLE_TEMHUM   \
COMMANDS_TABLE_PRESENCE \
COMMANDS_TABLE_DIMMER	\

#define X(a, b, c, d, e, f) a = b,
typedef enum  {
	COMMANDS_TABLE
}COMMAND_OPCODES;
#undef X

void MODULES_Init(void);
void MODULES_Notify(uint8_t moduleId, OPERATION_HEADER_t* header);
extern inline void* MODULES_DataConf(uint8_t opcode);
uint8_t MODULES_GetCommandArgsLength(uint8_t* opcode);
extern inline void MODULES_HandleCommand(OPERATION_HEADER_t* header);
inline _Bool MODULES_HandledByFirmware(uint8_t opcode);
inline _Bool MODULES_HandledByBroadcast(uint8_t opcode);


#endif /* MODULESMANAGER_H_ */