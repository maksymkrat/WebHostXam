using System;
using System.Collections.Generic;
using WebHostXam.Models;

namespace WebHostXam.Managers
{
    public sealed class ReceiptManager 
    {
        public Action<ReceiptItemModel> ActionAddItemToReceipt { get; set; } // ?
        public Action<int> ActionRemoveItemInReceipt { get; set; } // ?
        public Action<float> ActionAddDiscount { get; set; } // ?
        public Action<ReceiptModel> ActionStartReceipt { get; set; }
        public Action ActionFinishReceipt { get; set; }
        
        private static ReceiptManager _receiptManager;
        private ReceiptManager()
        {
        }

        public static ReceiptManager GetInstance()
        {
            if (_receiptManager == null)
            {
                _receiptManager = new ReceiptManager();
            }

            return _receiptManager;
        }
        

       

        public void StartReceipt(ReceiptModel model)
        {
            ActionStartReceipt.Invoke(model);
        }

        public void AddItemToReceipt(ReceiptItemModel item)
        {
        }

        public void RemoveItemInReceipt(int itemId)
        {
        }

        public void FinishReceipt()
        {
            ActionFinishReceipt.Invoke();
        }
        
        
        public void CalculateReceiptAmount(List<ReceiptItemModel> items, float discount)
        {
            float amount = 0;
            foreach (var item in items)
            {
                amount += item.Price;
            }

            if (discount != 1)
            {
                var discountValue = amount * discount;
                amount = amount - (float) Math.Round((Decimal) discountValue, 2, MidpointRounding.AwayFromZero);
            }
               
        }
    }
}