using System;
using System.IO;
using Newtonsoft.Json;
using WebHostXam.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebHostXam.Managers
{
    public class WindowViewManager : ControllerBase
    {
        public Action<WindowViewModel> ActionChangeUpperView { get; set; }
        public Action<WindowViewModel> ActionChangeBottomView { get; set; }
        public Action ActionOpenShift { get; set; }
        public Action ActionCloseShift { get; set; }
        
        private static WindowViewManager _viewManager;

        private WindowViewManager()
        {
            
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

        public void ChangeUpperView(WindowViewModel model)
        {
            ActionChangeUpperView.Invoke(model);
        }
        
        public void ChangeBottomView(WindowViewModel model)
        {
            ActionChangeBottomView.Invoke(model);
        }

        public FileStreamResult GetVideo()
        {
            var path = "/storage/emulated/0/Data/videokuskus.mp4";
            FileStream stream = System.IO.File.Open(path, FileMode.OpenOrCreate);
            return File(stream, "video/mp4");
           
        }
        
        public WindowViewModel DeserializeWindowVewData(string data)
        {
            return JsonConvert.DeserializeObject<WindowViewModel>(data);
        }
        
    }
}