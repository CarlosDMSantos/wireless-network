﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.Plugins;
using SmartHome.Network.HomeDevices;
using SmartHome.Comunications;
using SmartHome.Memory;

namespace SmartHome.Network
{
    public class Operation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public HomeDevice DestionationHomeDevice { get; set; }
        public string OperationName { get; set; }
        public object[] Args { get; set; }

        public List<TimeRestriction> TimeRestrictions { get; set; }
        public List<ConditionalRestriction> ConditionalRestriction { get; set; }

        public void Execute() 
        { 
        }

        public byte[] ToBinaryOperation()
        {
            //get OperationMessage

            //convertir OperationMessage

            throw new NotImplementedException();
        }


    }

    public class TimeRestriction
    {
        public byte MaskWeekDays { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public DateTime HourStart { get; set; }
        public DateTime HourEnd { get; set; }

        public TimeRestriction(byte maskWeekDays, int fromHour, int fromMin, int FromSeg, int ToHour, int ToMin, int ToSeg)
        {
            MaskWeekDays = maskWeekDays;
            HourStart = new DateTime(1, 1, 1, fromHour, fromMin, FromSeg);
            HourEnd = new DateTime(1, 1, 1, ToHour, ToMin, ToSeg);
        }
    }

    public class ConditionalRestriction
    {
        public HomeDevice HomeDeviceValue;
        public dynamic Value;
        public Operations Operation;
        public string NamePropierty;


    }

    public static class Sheduler
    {
        public static SortedDictionary<DateTime, List<Operation>> TimeActions = new SortedDictionary<DateTime, List<Operation>>();

    }

    public enum Operations
    {

    }
}