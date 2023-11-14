﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSQ003.ConsistentHash.Library.Tests.Models
{
    /// <summary>
    /// Key Item
    /// </summary>
    public class KeyItem
    {
        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Slot
        /// </summary>
        public int Slot { get; set; } = 0;
    }
}
