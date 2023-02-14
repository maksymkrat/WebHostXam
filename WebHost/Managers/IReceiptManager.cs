using WebHost.Models;

namespace WebHost.Managers;

public interface IReceiptManager
{
    void StartReceipt(ReceiptModel model);
    void AddItemToReceipt(ReceiptItemModel item);
    void RemoveItemInReceipt(int itemId);
    void FinishReceipt(bool finish);
}