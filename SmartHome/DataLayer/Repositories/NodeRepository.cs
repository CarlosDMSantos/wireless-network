﻿#region Using Statements
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
#endregion

namespace DataLayer
{
    public class NodeRepository: Repository<Node>
    {
        public NodeRepository(SmartHomeDBContext context) : base(context) { }

        public Node GetByMacAddress(string macAddress)
        {
            return this._Collection.FirstOrDefault(n => n.Mac == macAddress);
        }

        public Node GetByNetworkAddress(int nodeAddress)
        {
            return this._Collection.FirstOrDefault(n => n.Address == nodeAddress);
        }

        public int GetNewAddress()
        {
            var existingAddress = this._Collection.Select(n => n.Address);

            for (int i = 2; i < 0xFFFF; i++)
            {
                if (!existingAddress.Contains(i))
                    return i;
            }

            throw new IndexOutOfRangeException("A valid address can't be found");
        }
    }
}
