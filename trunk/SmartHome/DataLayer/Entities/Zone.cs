﻿#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations; 
#endregion

namespace SmartHome.DataLayer
{
    public class Zone
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public byte[] ImageMap { get; set; }

        [Required]
        public virtual Home Home { get; set; }

        public virtual ICollection<View> Views { get; set; }

        public Zone()
        {
            this.Views = new List<View>();
        }
    }
}