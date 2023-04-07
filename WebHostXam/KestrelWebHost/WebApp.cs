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
    public class WebApp : ControllerBase
    {
        private readonly ReceiptManager _receiptManager;
        private readonly WindowViewManager _viewManager;

        private const string SendReceipt = "/sr"; //"/SendReceipt";
        private const string FinishReceipt = "/f"; // "/FinishReceipt";
        private const string OpenShift = "/os";
        private const string CloseShift = "/cs";

        private static FileStreamResult mediaStream = null;


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
            var page = httpContext.Request.Path.ToString();
            try
            {
                if (page.Contains("/files"))  
                {
                    var contentType = Instance.GetContentType($".{page.Split('.')[1]}");
                    response.ContentType = contentType;
                    mediaStream = Instance.GetMedia(page.Split('/')[2], contentType);
                    Byte[] bytes = new Byte[0];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        mediaStream.FileStream.CopyTo(ms);
                        bytes = ms.ToArray();
                    }
                    mediaStream = null;
                    return response.Body.WriteAsync(bytes, 0, bytes.Length);
                   
                }
                else
                {
                    Instance.RecognizeMethod(httpContext);
                    response.StatusCode = 200;
                    _serverStatus = Encoding.UTF8.GetBytes("server running");
                }
                
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
                    
                    case OpenShift:
                        _viewManager.OpenShift();
                        break;
                    
                    case CloseShift:
                        _viewManager.CloseShift();
                        break;

                   
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        
        public FileStreamResult GetMedia( string fileName, string mediaType)
        {
           
            var pathV = $"/storage/emulated/0/Download/{fileName}";
            FileStream stream = System.IO.File.Open(pathV, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
            return File(stream, mediaType);
         

        }

        public string GetContentType(string extension)
        {
            switch (extension)
            {
                case ".png":
                    return "image/png";
                case ".mp4":
                    return "video/mp4";
                case ".avi":
                    return "video/avi";
                case "jpeg":
                case "jpg":
                    return "image/jpeg";
                case ".gif":
                    return "image/gif";
            } 
            return string.Empty;

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