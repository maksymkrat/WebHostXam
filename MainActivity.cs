using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views.Animations;
using Android.Widget;
using WebHostXam.Android.Utils;
using WebHostXam.Managers;
using WebHostXam.Models;
using Android.Net;
using Uri = Android.Net.Uri;


namespace WebHostXam.Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity 
    {
       public bool onShowReceiptWindow = true; // need delete

        LinearLayout receiptLayout;
        ListView viewReceiptItems;
        TextView textDiscount;
        TextView textReceiptAmount;
        ReceiptManager receiptManager;
        ReceiptItemAdapter adapter;
       
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            var host = new App();
            receiptManager = ReceiptManager.GetInstance();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            
            receiptLayout = FindViewById<LinearLayout>(Resource.Id.receipt_window);
            viewReceiptItems = FindViewById<ListView>(Resource.Id.list_items);
            textDiscount = FindViewById<TextView>(Resource.Id.discount);
            textReceiptAmount = FindViewById<TextView>(Resource.Id.receipt_amount);
            
            //need delete
            Button button = FindViewById<Button>(Resource.Id.button1);
            button.Click += ShowAndHideWindow;

           Initialize();

           
           
                //need delete
              string ip = App.WebHostParameters.ServerIpEndpoint.Address.ToString();
              string url = $"http://{ip}:{App.WebHostParameters.ServerIpEndpoint.Port}";
              TextView urlText = FindViewById<TextView>(Resource.Id.url_text);
              urlText.Text = url;

              VideoView video =  FindViewById<VideoView>(Resource.Id.video);
              var uri= Uri.Parse("android.resource://" + Application.PackageName + "/" + Resource.Drawable.video2);
              video.SetVideoURI(uri);
              ;
              video.Start();


        }

        public void ShowAndHideWindow(object sender, EventArgs e)
        {
             ShowReceiptWindow(onShowReceiptWindow);
             onShowReceiptWindow = !onShowReceiptWindow;
        }

        public void HideReceiptWindow()
        {
            // ShowReceiptWindow(false);
            // onShowReceiptWindow = true;

        }

        public void ShowReceiptWindow(bool show)
        {
            RunOnUiThread((() =>
            {
                receiptLayout = FindViewById<LinearLayout>(Resource.Id.receipt_window);
                receiptLayout.Animate()!
                    .TranslationY(show ? 0 : -600)
                    .SetInterpolator(new OvershootInterpolator(0.5f))!
                    .Start();
            }));
        }

        public void StartReceipt(ReceiptModel receipt)
        {
            // if (receipt != null)
            // {
            //     RunOnUiThread((() =>
            //     { 
            //         adapter = new ReceiptItemAdapter(this, receipt.items);
            //         viewReceiptItems.Adapter = adapter;
            //         textDiscount.Text = $"Знижка по карті: {receipt.Discount}%";
            //         textReceiptAmount.Text = $"Сума: {receipt.Amount}";
            //
            //     }));
            //     ShowReceiptWindow(true);
            //     onShowReceiptWindow = false; //need delete
            // }
           
            StartActivity(typeof(ReceiptActivity));
        }

        
        
        

        public void Initialize()
        {
            receiptManager.ActionFinishReceipt += HideReceiptWindow;
            receiptManager.ActionStartReceipt += model => StartReceipt(model);
        }
        
       
    }
}