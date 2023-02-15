using System;
using WebHostXam.Models;

namespace WebHostXam.Managers
{
    public interface IReceiptManager
    {
        Action<bool> HideReceiptWindow { get; set; }
        void StartReceipt(ReceiptModel model);
        void AddItemToReceipt(ReceiptItemModel item);
        void RemoveItemInReceipt(int itemId);
        void FinishReceipt(bool finish);
    }
}