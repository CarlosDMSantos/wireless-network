﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions;
using DomoticNetwork.NetworkModel;

namespace DomoticNetwork.EEPROM
{
    //firm version 1.0
    class Generator
    {
        public Shield ShieldNode { get; set; }
        public UInt16 NodeAddress { set; get; }
        public Byte Channel { set; get; }
        public UInt16 PanId { set; get; }
        public Byte[] SecurityKey { set; get; }

        public Generator(Network Network, UInt16 Address)
        {
            ShieldNode = Network.GetNode(Address).NodeShield;
            NodeAddress = Address;
            Channel = Network.Channel;
            PanId = Network.PanId;
            SecurityKey = Network.SecurityKey;
        }

        public Byte[] GenerateEEPROM()
        {
            List<Byte> result = new List<Byte>();

            //Generar toda la memoria (la memoria se genera con CRC16 a "00 00")

            result.AddRange(DeviceInfo());

            result.AddRange(NetworkConfig());

            result.AddRange(PortsIOConfig());

            result.AddRange(PWMConfig());

            result.AddRange(AnalogInputsConfig());






            Byte[] memory = result.ToArray();

            //Calculamos el tamaño del byte
            UInt16 sizeMemory = (UInt16)memory.Length;

            memory[1] = sizeMemory.Uint16ToByte(ShieldNode.ShieldBase.LittleEndian)[0];
            memory[2] = sizeMemory.Uint16ToByte(ShieldNode.ShieldBase.LittleEndian)[1];

            //Generar el CRC
            Byte[] crc = new Crc16().ComputeChecksumBytes(memory, ShieldNode.ShieldBase.LittleEndian);

            //sustituimos el valor de CRC que esta en la posicion de memoria 0x02 0x03, no hace falta contar con endianidad
            memory[3] = crc[0];
            memory[4] = crc[1];

            return memory;
        }

        private Byte[] DeviceInfo()
        {
            Byte[] result = new Byte[5];

            result[0] = (byte)ShieldNode.Type;

            //Default Lenght (unkown at the moment)
            result[1] = 0x00;
            result[2] = 0x00;

            //Default CRC16 to 00 00
            result[3] = 0x00;
            result[4] = 0x00;

            return result;
        }

        private Byte[] NetworkConfig()
        {
            List<Byte> result = new List<Byte>();
            result.AddRange(NodeAddress.Uint16ToByte(ShieldNode.ShieldBase.LittleEndian));

            result.Add(Channel);

            result.AddRange(PanId.Uint16ToByte(ShieldNode.ShieldBase.LittleEndian));

            result.AddRange(SecurityKey);

            return result.ToArray();
        }

        private Byte[] PortsIOConfig()
        {
            List<Byte> result = new List<Byte>();

            for (Byte i = 0; i < ShieldNode.ShieldBase.NumPorts; i++)
            {
                result.AddRange(PinIOConfig(i));
            }

            return result.ToArray();
        }

        private Byte[] PinIOConfig(Byte port)
        {
            //Si no esta definido suponemos Entrada digital
            Byte[] result = new Byte[5];
            Conector c = null;
            //Input:0 - Output:1, default=0
            result[0] = 0x00;
            for (byte i = 0; i < ShieldNode.ShieldBase.NumPins; i++)
            {
                c = ShieldNode.GetConector(port, i);
                if (c != null && c.Output == true)
                {
                    result[0] = (byte)(result[0] | (0x01 << i));
                }
            }

            //Analog:0 - Digital:1 default: 1
            result[1] = 0xFF;
            for (byte i = 0; i < ShieldNode.ShieldBase.NumPins; i++)
            {
                c = ShieldNode.GetConector(port, i);
                if (c != null && c.Digital == false)
                {
                    result[1] = (byte)(result[1] & ~(0x01 << i));
                }
            }

            //Input:0 - Output:1, default=0
            result[2] = 0x00;
            for (byte i = 0; i < ShieldNode.ShieldBase.NumPins; i++)
            {
                c = ShieldNode.GetConector(port, i);
                if (c != null && c.DefaultValueD == true)
                {
                    result[2] = (byte)(result[2] | (0x01 << i));
                }
            }

            //ChangetypeD None:00 Rising:10, Fall:01, Both:11
            UInt16 ctd = 0x00; //change type digital
            for (byte i = 0; i < ShieldNode.ShieldBase.NumPins; i++)
            {
                c = ShieldNode.GetConector(port, i);
                if (c != null) ctd = (byte)(ctd | ((byte)c.changeTypeD) << (i * 2));
            }

            result[3] = ctd.Uint16ToByte(ShieldNode.ShieldBase.LittleEndian)[0];
            result[4] = ctd.Uint16ToByte(ShieldNode.ShieldBase.LittleEndian)[1];

            return result;
        }

        private Byte[] PWMConfig()
        {
            List<Byte> result = new List<Byte>();

            foreach (String pin in ShieldNode.ShieldBase.PWMPorts)
            {
                if (ShieldNode.GetConector(pin[0], Convert.ToByte(pin[1])) != null)
                    result.Add(ShieldNode.GetConector(pin[0], Convert.ToByte(pin[1])).DefaultValueA);
                else
                {
                    result.Add(0x00);
                }
            }
            return result.ToArray();
        }

        private Byte[] AnalogInputsConfig()
        {
            List<Byte> result = new List<Byte>();
            Conector c = null;

            foreach (String pin in ShieldNode.ShieldBase.AnalogPorts)
            {
                c = ShieldNode.GetConector(pin[0], Convert.ToByte(pin[1]));
                if (c != null)
                {
                    //Analog Input To Binary
                    result.Add(c.Increment);
                    result.Add(c.Threshold);
                }
                else
                {
                    result.Add(0x00); //Increment
                    result.Add(0x00); //Threshold
                }
            }

            return result.ToArray();
        }

        //Este metodo genera toda la parte dinamica y parten de el todas las zonas de memorias restantes
        private Byte[] Dinamic()
        {
            List<Byte> result = new List<Byte>();

            result.AddRange(TableEventList());
            UInt16 TimeEventTablePointer = 


            return result.ToArray();
        }



        private Byte[] TableEventList()
        {
            List<Byte> result = new List<Byte>();
            UInt16 pointer = 0;

            //añadimos la primera direccion, que siempre será 0x00
            result.Add(0x00);

            for (int i = 0; i < ShieldNode.ShieldBase.NumPorts; i++)
            {
                for (int j = 0; j < ShieldNode.ShieldBase.NumPins; j++)
                {
                    if (Device.Ports[i].Pins[j] != null)
                    {
                        pointer += Device.Ports[i].Pins[j].SizePortEvents();
                    }
                    result.AddRange(pointer.Uint16ToByte(ShieldNode.ShieldBase.LittleEndian));
                }
            }

            //direccion lista de eventos de tiempo, ya esta introducida por que basicamente, es la ulitma iteracion del for

            //direccion restricciones temporales de los eventos de puertos OJO!!!
            result.Add(0x00);
            result.Add(0x00);

            //direccion banderas de habilitación de eventos
            result.Add(0x00);
            result.Add(0x00);

            return result.ToArray();
        }

        private Byte[] PortsEvents()
        {
            List<Byte> result = new List<Byte>();
            for (int i = 0; i < Device.DeviceInfo.NumPorts; i++)
            {
                for (int j = 0; j < Device.DeviceInfo.NumPins; j++)
                {
                    if (Device.Ports[i].Pins[j] != null)
                    {
                        foreach (PortEvent pe in Device.Ports[i].Pins[j].PortEvents)
                        {
                            result.AddRange(pe.Event.ToBinary(ShieldNode.ShieldBase.LittleEndian));
                        }
                    }
                }
            }

            return result.ToArray();
        }

        private Byte[] TimeEvents()
        {
            List<Byte> result = new List<Byte>();


            return result.ToArray();
        }

        private Byte[] PortEventTimeRestriction()
        {
            List<Byte> result = new List<Byte>();


            return result.ToArray();
        }

        private Byte[] TableEnableEventsFlags()
        {
            List<Byte> result = new List<Byte>();


            return result.ToArray();
        }

        private Byte[] TableEvensFlags()
        {
            List<Byte> result = new List<Byte>();


            return result.ToArray();
        }

    }



}
