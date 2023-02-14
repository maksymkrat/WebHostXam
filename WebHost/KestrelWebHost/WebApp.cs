using System.Text;
using Microsoft.AspNetCore.Http;
using WebHost.Managers;
using WebHost.Models;

namespace WebHost.KestrelWebHost;

public class WebApp
{
    

        private readonly IReceiptManager _receiptManager;
        
        private const string StartReceipt = "/StartReceipt";
        private const string AddItemToReceipt = "/AddItemToReceipt";
        private const string RemoveItemInReceipt = "/RemoveItemInReceipt";
        private const string FinishReceipt = "/FinishReceipt";
        
        
        private static readonly byte[] _helloWorldBytes = Encoding.UTF8.GetBytes(
            "Hello from Kestrel server");
        
        private static WebApp instance = new WebApp();
        private static WebApp Instance
        {
            get { return instance; }
        }

        private ReceiptModel _receipt;
        private ReceiptItemModel _newItem;
        

        public WebApp()
        {
            _receiptManager = new ReceiptManager();
            
            //Create Receipt

            var item1 = new ReceiptItemModel();
            item1.Name = "Ssome product1";
            item1.Description = "Ssome description1";
            item1.Id = 11;
            item1.Price = 25;
        
            var item2 = new ReceiptItemModel();
            item2.Name = "Ssome product2";
            item2.Description = "Ssome description2";
            item2.Id = 22;
            item2.Price = 40;
        
            var receipt = new ReceiptModel();
            receipt.products = new List<ReceiptItemModel>();
            receipt.products.Add(item1);
            receipt.products.Add(item2);
            receipt.DateTime = DateTime.Now;
            receipt.Suma = 115;
            _receipt = receipt;
            
            var newItem = new ReceiptItemModel();
            newItem.Name = "Ssome newItem product3";
            newItem.Description = "Ssome description3";

            newItem.Id = 33;
            newItem.Price = 5;
            
            _newItem = newItem;

        }

     

        public static Task OnHttpRequest(HttpContext httpContext)
        {
            
            try
            {
                Instance.RecognizeMethod(httpContext);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            var response = httpContext.Response;
            response.StatusCode = 200;
            response.ContentType = "text/plain";
            
            var helloWorld = _helloWorldBytes;
            response.ContentLength = helloWorld.Length;
             return response.Body.WriteAsync(helloWorld, 0, helloWorld.Length);
        }
        
        

        public   void RecognizeMethod(HttpContext context)
        {
        
            switch (context.Request.Path)
            {
                case StartReceipt:
                    var value = _receipt;
                    _receiptManager.StartReceipt(value);
                    break;
                case AddItemToReceipt:
                    var value1 = _newItem;
                    _receiptManager.AddItemToReceipt(value1);
                    break;
                case RemoveItemInReceipt:
                    var value2 = 33;
                    _receiptManager.RemoveItemInReceipt(value2);
                    break;
                case FinishReceipt:
                    var value3 = true;
                    _receiptManager.FinishReceipt(value3);
                    break;
                
            }
            
        }
        
        public static object DeserializeBody(HttpContext context)
        {
            return new object(); // TODO
        }
    
}