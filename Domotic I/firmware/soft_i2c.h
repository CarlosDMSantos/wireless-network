#define DS1307	0xD0
#define TC74A0	0x90

void i2c_start(void);
void i2c_stop(void);
unsigned char i2c_rx(char ack);
bit i2c_tx(unsigned char d);
void write_register(char device, char addr, char cmd);
char read_register(char device, char addr);

