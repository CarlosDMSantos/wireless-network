﻿#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
#endregion

namespace SmartHome.DataLayer
{
    public class Security
    {
        public const byte CHANNEL = 0x00;
        public const ushort PANID = 0x00;

        public byte Channel { set; get; }
        public ushort PanId { set; get; }
        public string SecurityKey { set; get; }


        public byte[] GetSecurityKey()
        {
            return Encoding.ASCII.GetBytes("TestSecurityKey0");
        }

        public Security()
        {
            Channel = CHANNEL;
            PanId = PANID;
        }
    }
}