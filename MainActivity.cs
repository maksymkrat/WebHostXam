﻿using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views.Animations;
using Android.Widget;
using WebHostXam.Android.Utils;
using WebHostXam.Managers;
using WebHostXam.Models;


namespace WebHostXam.Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity //, global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
       public bool onShowReceiptWindow = false;
       public LinearLayout receiptLayout;
       public  ListView receiptItems;
       public float amount = 1224.41f;

        private List<ReceiptItemModel> items;

        public ReceiptManager receiptManager;
        
        
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            var host = new App();
            receiptManager = ReceiptManager.GetInstance();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            Button button = FindViewById<Button>(Resource.Id.button1);
            button.Click += ShowAndHideWindow;

           Initialize();

           receiptItems = FindViewById<ListView>(Resource.Id.list_items);
            
            ReceiptItemAdapter adapter = new ReceiptItemAdapter(this, items);
            receiptItems.Adapter = adapter;

            TextView textReceiptItemAmount = FindViewById<TextView>(Resource.Id.receipt_amount);
            textReceiptItemAmount.Text = "Amount: " + amount;
            
           
            
              string ip = App.WebHostParameters.ServerIpEndpoint.Address.ToString();
              string url = $"http://{ip}:{App.WebHostParameters.ServerIpEndpoint.Port}";
              TextView urlText = FindViewById<TextView>(Resource.Id.url_text);
              urlText.Text = url;

        }

        public void ShowAndHideWindow(object sender, EventArgs e)
        {
             receiptLayout = FindViewById<LinearLayout>(Resource.Id.receipt_window);

             receiptLayout.Animate()!
                 .TranslationY(onShowReceiptWindow ? 0 : -600)
                 .SetInterpolator(new OvershootInterpolator(0.5f))!
                 .Start();
             onShowReceiptWindow = !onShowReceiptWindow;
        }

        public void HideReceiptWindow()
        {
            ShowReceiptWindow(false);
            onShowReceiptWindow = true;

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
            ShowReceiptWindow(true);
            onShowReceiptWindow = false;
        }

        
        
        

        public void Initialize()
        {
            items = new List<ReceiptItemModel>();
            var item1 = new ReceiptItemModel();
            item1.Name = "Some product1";
            item1.Description = "Lorem ipsum dolor sit amet. Qui dolore adipisci est itaque eligendi et molestiae";
            item1.Id = 11;  
            item1.Price = 25.32f;
        
            var item2 = new ReceiptItemModel();
            item2.Name = "Some product2";
            item2.Description = "Non voluptatibus assumenda cum facere sint eos";
            item2.Id = 22;
            item2.Price = 40.50f;
            var item4 = new ReceiptItemModel();
            item4.Name = "Some product2";
            item4.Description = "Non voluptatibus assumenda cum facere sint eos";
            item4.Id = 44;
            item4.Price = 40.50f;
            
            var item3 = new ReceiptItemModel();
            item3.Name = "Some product3";
            item3.Description = "Ad veritatis voluptatem sed aspernatur voluptatibus cum laboriosam pariatur. Sed sapiente consectetur et officiis fugiat sit praesentium";
            item3.Id = 22;
            item3.Price = 40;
            items.Add(item1);
            items.Add(item2);
            items.Add(item3);
            items.Add(item4);
            
            receiptManager.ActionFinishReceipt += HideReceiptWindow;
            receiptManager.ActionStartReceipt += model => StartReceipt(model);

        }
        
       
    }
}