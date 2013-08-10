﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SmartHome.HomeModel
{
    public class View
    {
        [Key]
        public int Id { get; set; }

        public string NameView { get; set; }

        public Image ImageMap { get; set; }
    }
}