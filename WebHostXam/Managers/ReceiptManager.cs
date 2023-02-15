using System;
using WebHostXam.Models;

namespace WebHostXam.Managers
{
    public sealed class ReceiptManager //: IReceiptManager
    {
        public Action<bool> HideReceiptWindow { get; set; }
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
        }

        public void AddItemToReceipt(ReceiptItemModel item)
        {
        }

        public void RemoveItemInReceipt(int itemId)
        {
        }

        public void FinishReceipt(bool finish)
        {
            HideReceiptWindow.Invoke(finish);
        }
    }
}