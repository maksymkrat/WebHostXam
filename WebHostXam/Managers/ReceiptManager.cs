using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using WebHostXam.Models;

namespace WebHostXam.Managers
{
    public sealed class ReceiptManager 
    {
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


        public void SendReceipt(ReceiptModel model)
        {
            ActionStartReceipt.Invoke(model);
        }
        public void FinishReceipt()
        {
            ActionFinishReceipt.Invoke();
        }

        public ReceiptModel DeserializeReceiptData(string data)
        {
            return JsonConvert.DeserializeObject<ReceiptModel>(data);
        }
    }
}