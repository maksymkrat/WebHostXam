using System;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using WebHostXam.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebHostXam.Managers
{
    public class WindowViewManager : ControllerBase
    {
        public Action ActionChangeUpperView { get; set; }
        public Action<WindowViewModel> ActionChangeBottomView { get; set; }
        public Action ActionOpenShift { get; set; }
        public Action ActionCloseShift { get; set; }
        
        private const string IS_OPEN_SHIFT_KEY = "IsOpenShift";
        private const string AccessData = "Hilgrup1289";
        private const string ServerURL = "http://193.193.222.87:5600/GetHTMLWindowView";
        private  HttpClient httpClient;
        
       
        private static WindowViewManager _viewManager;

        private WindowViewManager()
        {
            httpClient = new HttpClient(); 
        }

        public static WindowViewManager GetInstance()
        {
            if (_viewManager == null)
            {
                _viewManager = new WindowViewManager();
            }

            return _viewManager;
        }
        public void OpenShift()
        {
            ActionOpenShift.Invoke();
        }

        public void CloseShift()
        {
            ActionCloseShift.Invoke();
        }

        public void ChangeUpperView()
        {
            ActionChangeUpperView.Invoke();
        }
        

        // public FileStreamResult GetMedia( string mediaType)
        // {
        //     if (mediaType.Equals(GetVideo))
        //     {
        //         var pathV = "/storage/emulated/0/Download/video.mp4";
        //         FileStream stream = System.IO.File.Open(pathV, FileMode.OpenOrCreate);
        //         return File(stream, "video/mp4");
        //     }
        //     
        //     if (mediaType.Equals(GetImg))
        //     {
        //         var pathV = "/storage/emulated/0/Download/img.png";
        //         FileStream stream = System.IO.File.Open(pathV, FileMode.OpenOrCreate);
        //         return File(stream, "image/png");
        //     }
        //
        //     return null;
        //
        // }
        
        public WindowViewModel DeserializeWindowVewData(string data)
        {
            return JsonConvert.DeserializeObject<WindowViewModel>(data);
        }
        
        public  string GetHTMLForView()
        {
            try
            {
                var data = new StringContent($"\"{AccessData}\"", System.Text.Encoding.UTF8, "application/json");
                var response =  httpClient.PostAsync(ServerURL,  data).Result;

                if(response.IsSuccessStatusCode)
                {
                    return   response.Content.ReadAsStringAsync().Result;
                }
                return string.Empty;
            }
            catch (Exception e)
            {
                return String.Empty;
            }
           
        }
        
    }
}