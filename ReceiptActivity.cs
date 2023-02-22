
using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using WebHostXam.Android.Utils;
using WebHostXam.Managers;


namespace WebHostXam.Android
{
   /// [Activity (NoHistory = true, Theme = "@style/Theme.Transparent" )]
    [Activity (NoHistory = true )]
    public class ReceiptActivity : Activity
    {
        
        LinearLayout receiptLayout;
        ListView viewReceiptItems;
        TextView textDiscount;
        TextView textReceiptAmount;
        ReceiptManager receiptManager;
        ReceiptItemAdapter adapter;

        private LinearLayout _layout;
        
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            receiptManager = ReceiptManager.GetInstance();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_receipt);
            _layout = FindViewById<LinearLayout>(Resource.Id.id_receipt_activity);
            _layout.SetGravity(GravityFlags.Top);
            
            receiptLayout = FindViewById<LinearLayout>(Resource.Id.id_receipt_window);
            receiptLayout.SetGravity(GravityFlags.Top);
            viewReceiptItems = FindViewById<ListView>(Resource.Id.id_list_items);
            textDiscount = FindViewById<TextView>(Resource.Id.id_discount);
            textReceiptAmount = FindViewById<TextView>(Resource.Id.id_receipt_amount);

            // //need delete
            // Button button = FindViewById<Button>(Resource.Id.id_button);
            // button.Click += HideActivity;
            //
            
            Initialize();
            
            
            
        }

        protected void Initialize()
        {
            receiptManager.ActionFinishReceipt += Finish;
            
            RunOnUiThread((() =>
            { 
                adapter = new ReceiptItemAdapter(this, receiptManager.ReceiptModel.items);
                viewReceiptItems.Adapter = adapter;
                textDiscount.Text = $"Знижка по карті: {receiptManager.ReceiptModel.Discount}%";
                textReceiptAmount.Text = $"Сума: {receiptManager.ReceiptModel.Amount}";
            
            }));
            
            
        }

        private void HideActivity(object sender, EventArgs e)
        {
           Finish();
        }
    }
}