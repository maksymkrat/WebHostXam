using System;
using System.Collections.Generic;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V7.App;
using Android.Views.Animations;
using Android.Widget;
using WebHostXam.Android.Utils;
using WebHostXam.Managers;
using WebHostXam.Models;
using Android.Net;
using Android.Support.V4.Widget;
using Android.Views;
using ActionBar = Android.App.ActionBar;
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
        ReceiptItemAdapter adapter ;

        private Dialog popupDialog = null;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            var host = new App();
            receiptManager = ReceiptManager.GetInstance();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            


            //need delete
            Button button = FindViewById<Button>(Resource.Id.button1);
            button.Click += ShowAndHideWindow;

            Initialize();


            //need delete
            string ip = App.WebHostParameters.ServerIpEndpoint.Address.ToString();
            string url = $"http://{ip}:{App.WebHostParameters.ServerIpEndpoint.Port}";
            TextView urlText = FindViewById<TextView>(Resource.Id.url_text);
            urlText.Text = url;

            VideoView video = FindViewById<VideoView>(Resource.Id.video);
            var uri = Uri.Parse("android.resource://" + Application.PackageName + "/" + Resource.Drawable.video2);
            video.SetVideoURI(uri);
            ;
            video.Start();
        }

        public void ShowAndHideWindow(object sender, EventArgs e)
        {
            // ShowReceiptWindow(onShowReceiptWindow);
            // onShowReceiptWindow = !onShowReceiptWindow;
            //----------------------
        }

        public void HideReceiptWindow()
        {
            // ShowReceiptWindow(false);
            // onShowReceiptWindow = true;

            RunOnUiThread((() =>
                    {
                        popupDialog.Dismiss();
                        popupDialog.Hide();
                        popupDialog = null;
                    }
                ));
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

            //----------------------

            //StartActivity(typeof(ReceiptActivity));

            //----------------------
            if (receipt != null)
            {
                RunOnUiThread((() =>
                {
                    if(popupDialog == null)
                        popupDialog = new Dialog(this, Resource.Style.Theme_Transparent);
                    
                    popupDialog.SetContentView(Resource.Layout.activity_receipt);

                    viewReceiptItems = popupDialog.FindViewById<ListView>(Resource.Id.id_list_items);
                    textDiscount = popupDialog.FindViewById<TextView>(Resource.Id.id_discount);
                    textReceiptAmount = popupDialog.FindViewById<TextView>(Resource.Id.id_receipt_amount);

                    adapter = new ReceiptItemAdapter(this, receipt.items);
                    viewReceiptItems.Adapter = adapter;
                    textDiscount.Text = $"Знижка по карті: {receipt.Discount}%";
                    textReceiptAmount.Text = $"Сума: {receipt.Amount}";

                    popupDialog.Window.SetLayout(ActionBar.LayoutParams.MatchParent,
                        ActionBar.LayoutParams.WrapContent);
                    popupDialog.Window.SetGravity(GravityFlags.Top);
                    popupDialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
                    popupDialog.Show();
                }));
            }
        }


        public void Initialize()
        {
            receiptManager.ActionFinishReceipt += HideReceiptWindow;
            receiptManager.ActionStartReceipt += model => StartReceipt(model);
        }
    }
}