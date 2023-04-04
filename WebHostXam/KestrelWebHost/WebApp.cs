using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebHostXam.Managers;
using WebHostXam.Models;


namespace WebHostXam.KestrelWebHost
{
    public class WebApp
    {
        private readonly ReceiptManager _receiptManager;
        private readonly WindowViewManager _viewManager;

        private const string SendReceipt = "/sr"; //"/SendReceipt";
        private const string FinishReceipt = "/f"; // "/FinishReceipt";
        private const string SendNewUpperWindowView = "/suv"; //"SendNewUpperWindowView";
        private const string SendNewBottomWindowView = "/sbv"; //"SendNewBottomWindowView";
        private const string GetVideo = "/v.mp4"; //GetVideo

        private static FileStreamResult videoStream = null;


        private static byte[] _serverStatus = Encoding.UTF8.GetBytes(
            "server running");

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
            _viewManager = WindowViewManager.GetInstance();
        }


        public  static Task OnHttpRequest(HttpContext httpContext)
        {
            var response = httpContext.Response;

            try
            {
                 Instance.RecognizeMethod(httpContext);
                response.StatusCode = 200;
                _serverStatus = Encoding.UTF8.GetBytes("server running");
                // if (videoStream != null)
                // {
                   //
                   //  var bytes = System.IO.File.ReadAllBytes(@"/storage/emulated/0/Data/videokuskus.mp4");
                   //  response.ContentType = "video/mp4";
                   // return response.Body.WriteAsync(bytes, 0, bytes.Length);
                //}
            }
            catch (Exception e)
            {
                response.StatusCode = 400;
                _serverStatus = Encoding.UTF8.GetBytes(
                    e.ToString());
            }


            response.ContentType = "text/plain";
            response.ContentLength = _serverStatus.Length;
            return  response.Body.WriteAsync(_serverStatus, 0, _serverStatus.Length);
        }


        public void RecognizeMethod(HttpContext context)
        {
            try
            {
                switch (context.Request.Path)
                {
                    case SendReceipt:
                        var strReceiptData = ReadBodyFromRequest(context);
                        var newReceipt = _receiptManager.DeserializeReceiptData(strReceiptData);
                        _receiptManager.SendReceipt(newReceipt);
                        break;
                    case FinishReceipt:
                        _receiptManager.FinishReceipt();
                        break;

                    case SendNewUpperWindowView:
                        var strViewUpperData = ReadBodyFromRequest(context);
                       var newUpperView =  _viewManager.DeserializeWindowVewData(strViewUpperData);
                        _viewManager.ChangeUpperView(newUpperView);
                        break;
                    
                    case SendNewBottomWindowView:
                        var strViewBottomData = ReadBodyFromRequest(context);
                        var newBottomView =  _viewManager.DeserializeWindowVewData(strViewBottomData);
                        _viewManager.ChangeBottomView(newBottomView);
                        break;
                    
                    case GetVideo:
                        //var videoStream = _viewManager.GetVideo();
 
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