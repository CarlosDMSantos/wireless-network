﻿using SmartHome.Comunications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Network.HomeDevices
{
    public class Light: HomeDevice
    {
        public bool IsOn { get; set; }

        public Light(string name)
            : base(name)
        {
            base.Operations = new List<Operation>();
        }

        public OperationMessage On()
        {
            return OperationMessage.LogicWrite(Id, LogicWriteValues.Set, 0);
        }

        public OperationMessage Off()
        {
            return OperationMessage.LogicWrite(Id, LogicWriteValues.Clear, 0);
        }

        public OperationMessage OnTime(byte seconds)
        {
            return OperationMessage.LogicWrite(Id, LogicWriteValues.Set, seconds);
        }

        public OperationMessage Switch()
        {
            return OperationMessage.LogicSwitch(Id, 0);
        }

        public override void RefreshState()
        {
            base.RefreshState();
        }
        
    }
}
