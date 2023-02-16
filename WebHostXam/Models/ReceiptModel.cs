using System;
using System.Collections.Generic;

namespace WebHostXam.Models
{
    public class ReceiptModel
    {
        public List<ReceiptItemModel> items { get; set; }
        public int Discount { get; set;}
        public float Amount { get; set;}
    }
}