using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebHostXam.Managers;
using WebHostXam.Models;


namespace WebHostXam.KestrelWebHost
{
    public class WebApp
    {
        private readonly ReceiptManager _receiptManager;

        private const string SendReceipt = "/SendReceipt";
        private const string FinishReceipt = "/FinishReceipt";


        private static byte[] _serverStatus = Encoding.UTF8.GetBytes(
            "server run");

        private static WebApp instance = new WebApp();

        private static WebApp Instance
        {
            get { return instance; }
        }

        private ReceiptModel _receipt;
        private ReceiptItemModel _newItem;


        public WebApp()
        {
            _receiptManager = ReceiptManager.GetInstance();
        }


        public static Task OnHttpRequest(HttpContext httpContext)
        {
            var response = httpContext.Response;

            try
            {
                Instance.RecognizeMethod(httpContext);
                response.StatusCode = 200;
                _serverStatus = Encoding.UTF8.GetBytes("server runing");
            }
            catch (Exception e)
            {
                response.StatusCode = 400;
                _serverStatus = Encoding.UTF8.GetBytes(
                    e.ToString());
            }


            response.ContentType = "text/plain";
            response.ContentLength = _serverStatus.Length;
            return response.Body.WriteAsync(_serverStatus, 0, _serverStatus.Length);
        }


        public void RecognizeMethod(HttpContext context)
        {
            try
            {
                switch (context.Request.Path)
                {
                    case SendReceipt:
                        var strData = ReadBodyFromRequest(context);
                        var newReceipt = _receiptManager.DeserializeReceiptData(strData);
                        _receiptManager.SendReceipt(newReceipt);
                        break;
                    case FinishReceipt:
                        _receiptManager.FinishReceipt();
                        break;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public string ReadBodyFromRequest(HttpContext context)
        {
            
                var str = "";
                using (StreamReader reader = new StreamReader(context.Request.Body))
                {
                    str = reader.ReadToEnd();
                    if (String.IsNullOrEmpty(str))
                    {
                        throw new Exception("body is empty");
                    }
                }
                return str;
                

        }
    }
}