﻿using System;
using System.Collections.Generic;

namespace WebHostXam.Models
{
    public class ReceiptModel
    {
        public DateTime DateTime { get; set; }
        public List<ReceiptItemModel> products { get; set; }
        public int Discount { get; set;}
        public float Amount { get; set;}
    }
}