﻿#region Using Statements
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
#endregion

namespace DataLayer.Entities
{
    public class TimeOperation : IComparable<TimeOperation>
    {
        [Key]
        public int Id { get; set; }

        public TimeSpan Time { get; set; }

        public byte MaskWeekDays { get; set; }

        public virtual Operation Operation { get; set; }


        public int CompareTo(TimeOperation other)
        {
            return TimeSpan.Compare(this.Time, other.Time);
        }
    }
}
