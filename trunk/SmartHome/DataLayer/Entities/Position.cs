﻿#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel.DataAnnotations; 
#endregion

namespace DataLayer.Entities
{
    public class Position
    {
        [Key]
        public int Id { get; set; }

        public virtual Zone Zone { get; set; }

        public PointF ZoneCoordenates { get; set; }

        public Dictionary<View, PointF> Views { get; set; }

        public bool Movil { get; set; }

        public void CalculatePosition()
        {
            
        }
    }
}
