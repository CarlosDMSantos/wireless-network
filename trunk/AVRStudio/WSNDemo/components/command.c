/*
* command.c
*
* Created: 27/01/2013 19:37:23
*  Author: Victor
*/
#include "command.h"

#define X(a, b, c, d, e) [b] = c,
void (*command_handlers[]) (uint8_t*) = {
	COMMANDS_TABLE
};
#undef X

#define X(a, b, c, d, e) [b] = sizeof(d),
uint8_t command_lengths[] = {
	COMMANDS_TABLE
};
#undef X

#define X(a, b, c, d, e) [b] = e,
bool command_is_dinamic[] = {
	COMMANDS_TABLE
};
#undef X

uint8_t getCommandArgsLength(uint8_t* opcode)
{
	if(*opcode == EXTENSION_OPCODE)
		return 4;// JUST FOR TRIALS!
	
	//if(*opcode == EXTENSION_OPCODE)
	//	return getCommandArgsLength(opcode+1);
	
	if(command_is_dinamic[*opcode])
		return command_lengths[*opcode] + *(opcode+1); //CHECK!!!!!!!!! LENGTH READ
	else
		return command_lengths[*opcode];
}	

void handleCommand(uint8_t* opcode)
{
	(*command_handlers[*opcode]) (opcode);
}
