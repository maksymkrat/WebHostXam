using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V7.App;
using Android.Views.Animations;
using Android.Widget;
using WebHostXam.Android.Utils;
using WebHostXam.Managers;
using WebHostXam.Models;
using Android.Views;
using Android.Webkit;
using ActionBar = Android.App.ActionBar;
using Bitmap = System.Drawing.Bitmap;
using Color = Android.Graphics.Color;
using Path = System.IO.Path;
using Uri = Android.Net.Uri;


namespace WebHostXam.Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        LinearLayout receiptLayout;
        ListView viewReceiptItems;
        TextView textDiscount;
        TextView textReceiptAmount;
        ReceiptManager receiptManager;
         WindowViewManager viewManager;
        ReceiptItemAdapter adapter;
        Dialog receiptWindow = null;

        ExtraOffer Offer1 = null;
        ExtraOffer Offer2 = null;
        ImageView offerImg1;
        TextView offerName1;
        TextView offerDescr1;
        TextView offerOldPrice1;
        TextView offerNewPrice1;
        ImageView offerImg2;
        TextView offerName2;
        TextView offerDescr2;
        TextView offerOldPrice2;
        TextView offerNewPrice2;

        private WebView upperWebView;
        private WebView bottomWebView;

        private View decorView;

        private RelativeLayout _layout;

        private ReceiptModel _receipt;
        private string html;
        
        
        private readonly string ServerURL = "http://172.19.100.133:5555/GetHTMLWindowView";
        private readonly   HttpClient _httpClient= new HttpClient();




        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            string[] perm = new string[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.WriteExternalStorage };
            RequestPermissions(perm, 325);

           SetUiFlags();
            
            Window.AddFlags(WindowManagerFlags.Fullscreen);
            RequestedOrientation = ScreenOrientation.Landscape;


            var host = new App();
            receiptManager = ReceiptManager.GetInstance();
            viewManager = WindowViewManager.GetInstance();
            
            SetContentView(Resource.Layout.activity_main);

            Initialize();
            
            upperWebView = FindViewById<WebView>(Resource.Id.upperWebView);
            bottomWebView = FindViewById<WebView>(Resource.Id.bottomWebView);
            
            //reae byte and create video
            //  var b64Str = System.IO.File.ReadAllText(@"/storage/emulated/0/Data/textFile.txt");
            //  Byte[] bytes = Convert.FromBase64String(b64Str);
            //
            //  
            // if (!File.Exists("/storage/emulated/0/Data/kuskus.mp4"))
            // {
            //     FileInfo file = new FileInfo("/storage/emulated/0/Data/kuskus.mp4");
            //     using (Stream stream = file.OpenWrite())
            //     {
            //         stream.Write(bytes,0 ,bytes.Length);
            //         stream.Close();
            //     }
            // }
            
            //html = System.IO.File.ReadAllText(@"/storage/emulated/0/Data/test2.html");

           

        }

        public async Task<string> GetHTMLForView()
        {
            var response = await _httpClient.GetAsync(ServerURL);

            if(response.IsSuccessStatusCode)
            {
                return  await response.Content.ReadAsStringAsync();
            }
            return string.Empty;
        }
        

        public void HideReceiptWindow()
        {
            // ShowReceiptWindow(false);
            // onShowReceiptWindow = true;

            RunOnUiThread((() =>
                    {
                        receiptWindow.Dismiss();
                        receiptWindow.Hide();
                        receiptWindow = null;
                        Offer1 = null;
                        Offer2 = null;
                    }
                ));
        }

        public async void ChangeUpperView(WindowViewModel view)
        {
            html = await GetHTMLForView();
            RunOnUiThread((() =>
            {
               //upperWebView.LoadData(view.HTML, "text/html", null);
              upperWebView.LoadData(html, "text/html", null);

              SetUiFlags();
            }));
        }
        
        public void ChangeBottomView(WindowViewModel view)
        {
            RunOnUiThread((() =>
            {
               
               bottomWebView.LoadData(view.HTML, "text/html", null);
               
                SetUiFlags();
            }));
        }

        public void ShowReceiptWindow(bool show)
        {
            // RunOnUiThread((() =>
            // {
            //     receiptLayout = FindViewById<LinearLayout>(Resource.Id.receipt_window);
            //     receiptLayout.Animate()!
            //         .TranslationY(show ? 0 : -600)
            //         .SetInterpolator(new OvershootInterpolator(0.5f))!
            //         .Start();
            // }));
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
                _receipt = receipt;
                RunOnUiThread((() =>
                {
                    // //receipt setup
                    if (receiptWindow == null)
                        receiptWindow = new Dialog(this, Resource.Style.Theme_Transparent);

                    receiptWindow.SetContentView(Resource.Layout.activity_receipt);

                    _layout = receiptWindow.FindViewById<RelativeLayout>(Resource.Id.id_receipt_activity);

                    viewReceiptItems = receiptWindow.FindViewById<ListView>(Resource.Id.id_list_items);
                    textDiscount = receiptWindow.FindViewById<TextView>(Resource.Id.id_discount);
                    textReceiptAmount = receiptWindow.FindViewById<TextView>(Resource.Id.id_receipt_amount);

                    adapter = new ReceiptItemAdapter(this, receipt.items);
                    viewReceiptItems.Adapter = adapter;
                    textDiscount.Text = $"Знижка по карті: {receipt.Discount}%";
                    textReceiptAmount.Text = $"Сума: {receipt.Amount.ToString("N2")}";

                    //extra offer

                    offerImg1 = receiptWindow.FindViewById<ImageView>(Resource.Id.Offer_img_1);
                    offerName1 = receiptWindow.FindViewById<TextView>(Resource.Id.offer_name_1);
                    offerDescr1 = receiptWindow.FindViewById<TextView>(Resource.Id.offer_descr_1);
                    offerOldPrice1 = receiptWindow.FindViewById<TextView>(Resource.Id.offer_old_price_1);
                    offerNewPrice1 = receiptWindow.FindViewById<TextView>(Resource.Id.offer_new_price_1);
                    offerName1.Text = receipt.Offer1.Name;
                    offerDescr1.Text = receipt.Offer1.Description;
                    offerOldPrice1.Text = receipt.Offer1.OldPrice.ToString("N2");
                    offerNewPrice1.Text = receipt.Offer1.NewPrice.ToString("N2");
                    offerImg1.SetImageBitmap(ConvertStringBase64ToBitmap(receipt.Offer1.ImgBase64Str)); // if null TODO

                    offerImg2 = receiptWindow.FindViewById<ImageView>(Resource.Id.offer_img_2);
                    offerName2 = receiptWindow.FindViewById<TextView>(Resource.Id.offer_name_2);
                    offerDescr2 = receiptWindow.FindViewById<TextView>(Resource.Id.offer_descr_2);
                    offerOldPrice2 = receiptWindow.FindViewById<TextView>(Resource.Id.offer_old_price_2);
                    offerNewPrice2 = receiptWindow.FindViewById<TextView>(Resource.Id.offer_new_price_2);
                    offerName2.Text = receipt.Offer2.Name;
                    offerDescr2.Text = receipt.Offer2.Description;
                    offerOldPrice2.Text = receipt.Offer2.OldPrice.ToString("N2");
                    offerNewPrice2.Text = receipt.Offer2.NewPrice.ToString("N2");
                    offerImg2.SetImageBitmap(ConvertStringBase64ToBitmap(receipt.Offer2.ImgBase64Str)); // if null TODO


                    //window seutup
                    receiptWindow.Window.SetLayout(ActionBar.LayoutParams.MatchParent,
                        ActionBar.LayoutParams.WrapContent);
                    receiptWindow.Window.SetGravity(GravityFlags.Top | GravityFlags.Bottom);
                    receiptWindow.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
                    ;
                    receiptWindow.Window.DecorView.SystemUiVisibility =
                        (StatusBarVisibility) (
                                              
                                                SystemUiFlags.HideNavigation
                                               | SystemUiFlags.LayoutFullscreen
                                               | SystemUiFlags.LayoutHideNavigation
                                               | SystemUiFlags.Fullscreen
                                                | SystemUiFlags.LowProfile
                                                | SystemUiFlags.Immersive
                                               );
                       
                    receiptWindow.Show();
                }));
            }
        }

  


        public void Initialize()
        {
            receiptManager.ActionFinishReceipt += HideReceiptWindow;
            receiptManager.ActionStartReceipt += model => StartReceipt(model);
            viewManager.ActionChangeUpperView += view => ChangeUpperView(view);
            viewManager.ActionChangeBottomView += view => ChangeBottomView(view);
        }

        protected global::Android.Graphics.Bitmap ConvertStringBase64ToBitmap(string str)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(str);
                return BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        protected void SetUiFlags()
        {
            decorView = Window.DecorView;
            decorView.SystemUiVisibility = (StatusBarVisibility) (  SystemUiFlags.HideNavigation
                                                                    | SystemUiFlags.LayoutFullscreen
                                                                    | SystemUiFlags.LayoutHideNavigation
                                                                    | SystemUiFlags.Fullscreen
                                                                    | SystemUiFlags.LowProfile
                                                                    | SystemUiFlags.Immersive);
        }
    }
}