using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
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
using Xamarin.Essentials;
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
        TextView textDiscountInUAN;
        TextView textReceiptAmount;
        ReceiptManager receiptManager;
        WindowViewManager viewManager;
        ReceiptItemAdapter adapter;
        Dialog receiptWindow = null;
        
        //eaxta offers
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
        //card info
        private TextView fourLastPhoneDigits;
        private TextView cardNumber;
        private TextView wasUAN;
        private TextView accruedUAN;
        private TextView withdrawnUAN;
        private TextView remainderUAN;


        private WebView upperWebView;
        private WebView bottomWebView;

        private View decorView;

        private RelativeLayout _layout;
        private LinearLayout _receipt_plcace;
        private VideoView _videoView;

        private ReceiptModel _receipt;
        private LinearLayout _black_layout;
        private string html;


        private const string bodyStyle = "<style>body {margin: 0; padding: 0;}</style>";
        private readonly string ServerURL = "http://193.193.222.87:5600/GetHTMLWindowView";
        private readonly HttpClient _httpClient = new HttpClient();
        private const string IsOpenShift = "IsOpenShift";
        private readonly string AccessData = "Hilgrup1289";


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            string[] perm = new string[]
                {Manifest.Permission.WriteExternalStorage, Manifest.Permission.WriteExternalStorage};
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
            upperWebView.Settings.JavaScriptEnabled = true;
            upperWebView.ClearCache(true);


            _receipt_plcace = FindViewById<LinearLayout>(Resource.Id.receipt_place);
            _receipt_plcace.Clickable = false;
            _receipt_plcace.Click += (sender, args) => { return; };

            // bottomWebView = FindViewById<WebView>(Resource.Id.bottomWebView);
            _black_layout = FindViewById<LinearLayout>(Resource.Id.black_layout);
            _videoView = FindViewById<VideoView>(Resource.Id.video);
            _videoView.SetBackgroundColor(Color.Red);

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

            //html =  String.Empty;//GetHTMLForView();
            //html = "<video autoplay muted loop ><source src=\"http://127.0.0.1:3555/files/vid2.mp4\" type=\"video/mp4\"></video>";
            //html = "<img src=\"http://127.0.0.1:3555/files/bl.png\" width=\"100%\" height=\"100%\">";

           // html = viewManager.GetHTMLForView();
           html = "";
        }

        // protected override void OnStart()
        // {
        //     base.OnStart();
        //     RunOnUiThread((() =>
        //     {
        //         upperWebView.LoadData(bodyStyle + html, "text/html", null);
        //     }));
        // }


        protected async override void OnResume()
        {
            base.OnResume();
            upperWebView.ClearCache(true);
            upperWebView.ClearHistory();
            upperWebView.Reload();
            await Task.Delay(3000);
            upperWebView.LoadData(bodyStyle + html, "text/html", null);
        }

        public void ChangeUpperView()
        {
            //html =  GetHTMLForView();
            RunOnUiThread((() =>
            {
                upperWebView.LoadData(bodyStyle + html, "text/html", null);

                SetUiFlags();
            }));
        }

        public string GetHTMLForView()
        {
            var data = new StringContent($"\"{AccessData}\"", Encoding.UTF8, "application/json");
            var response = _httpClient.PostAsync(ServerURL, data).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result;
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

        public void OpenShift()
        {
            Preferences.Set(IsOpenShift, "true");
            RunOnUiThread((() =>
            {
                WindowManagerLayoutParams param = Window.Attributes;
                param.ScreenBrightness = 1;
                Window.Attributes = param;

                _black_layout.Visibility = ViewStates.Invisible;
                SetUiFlags();
            }));
        }

        public void CloseShift()
        {
            Preferences.Set(IsOpenShift, "false");
            RunOnUiThread((() =>
            {
                WindowManagerLayoutParams param = Window.Attributes;
                param.ScreenBrightness = 0;
                Window.Attributes = param;

                _black_layout.Visibility = ViewStates.Visible;
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
                    if (String.IsNullOrEmpty(receipt.Offer1.Name) || String.IsNullOrEmpty(receipt.Offer2.Name))
                    {
                        CreateReceipt(receipt);
                    }
                    else
                    {
                        CreatReceiptAndExtraOffers(receipt);
                    }


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

        public void CreateReceipt(ReceiptModel receipt)
        {
            receiptWindow.SetContentView(Resource.Layout.activity_receipt);
            _layout = receiptWindow.FindViewById<RelativeLayout>(Resource.Id.id_receipt_activity);
            viewReceiptItems = receiptWindow.FindViewById<ListView>(Resource.Id.id_list_items);
            textReceiptAmount = receiptWindow.FindViewById<TextView>(Resource.Id.id_receipt_amount);
            adapter = new ReceiptItemAdapter(this, receipt.items);
            viewReceiptItems.Adapter = adapter;
            textReceiptAmount.Text = $"{receipt.Amount.ToString("N2")} грн";
            //card info

            fourLastPhoneDigits = receiptWindow.FindViewById<TextView>(Resource.Id.id_4_phone_digits);
            cardNumber = receiptWindow.FindViewById<TextView>(Resource.Id.id_card_number);
            wasUAN = receiptWindow.FindViewById<TextView>(Resource.Id.id_was_uan);
            accruedUAN = receiptWindow.FindViewById<TextView>(Resource.Id.id_accrued_uan);
            withdrawnUAN = receiptWindow.FindViewById<TextView>(Resource.Id.id_withdrawn_uan);
            remainderUAN = receiptWindow.FindViewById<TextView>(Resource.Id.id_remainder_uan);
            
            fourLastPhoneDigits.Text = $"(***) *** ** **";
            var phoneDigitsArr = receipt.CardInfo.FourLastPhoneDigits.ToCharArray();
            if(phoneDigitsArr.Length >3)
                fourLastPhoneDigits.Text = $"(***) *** {phoneDigitsArr[0]}{phoneDigitsArr[1]} {phoneDigitsArr[2]}{phoneDigitsArr[3]}";
            cardNumber.Text = $"по карті № {receipt.CardInfo.CardNumber}";
            wasUAN.Text =       $" {receipt.CardInfo.WasUAN.ToString("N2")} грн";
            accruedUAN.Text =   $" {receipt.CardInfo.AccruedUAN.ToString("N2")} грн";
            withdrawnUAN.Text = $" {receipt.CardInfo.WithdrawnUAN.ToString("N2")} грн";
            remainderUAN.Text = $" {receipt.CardInfo.RemainderUAN.ToString("N2")} грн";
            
        }

        public void CreatReceiptAndExtraOffers(ReceiptModel receipt)
        {
            receiptWindow.SetContentView(Resource.Layout.activity_receipt_offers);
            _layout = receiptWindow.FindViewById<RelativeLayout>(Resource.Id.id_receipt_activity);
            viewReceiptItems = receiptWindow.FindViewById<ListView>(Resource.Id.id_list_items);
            textReceiptAmount = receiptWindow.FindViewById<TextView>(Resource.Id.id_receipt_amount);
            adapter = new ReceiptItemAdapter(this, receipt.items);
            viewReceiptItems.Adapter = adapter;
            textReceiptAmount.Text = $"Сума: {receipt.Amount.ToString("N2")}";
            
            //card info
            fourLastPhoneDigits = receiptWindow.FindViewById<TextView>(Resource.Id.id_4_phone_digits);
            cardNumber = receiptWindow.FindViewById<TextView>(Resource.Id.id_card_number);
            wasUAN = receiptWindow.FindViewById<TextView>(Resource.Id.id_was_uan);
            accruedUAN = receiptWindow.FindViewById<TextView>(Resource.Id.id_accrued_uan);
            withdrawnUAN = receiptWindow.FindViewById<TextView>(Resource.Id.id_withdrawn_uan);
            remainderUAN = receiptWindow.FindViewById<TextView>(Resource.Id.id_remainder_uan);

            fourLastPhoneDigits.Text = $"(***) *** ** **";
            var phoneDigitsArr = receipt.CardInfo.FourLastPhoneDigits.ToCharArray();
            if(phoneDigitsArr.Length >3)
                fourLastPhoneDigits.Text = $"(***) *** {phoneDigitsArr[0]}{phoneDigitsArr[1]} {phoneDigitsArr[2]}{phoneDigitsArr[3]}";
            cardNumber.Text = $"по карті № {receipt.CardInfo.CardNumber}";
            wasUAN.Text =       $" {receipt.CardInfo.WasUAN.ToString("N2")} грн";
            accruedUAN.Text =   $" {receipt.CardInfo.AccruedUAN.ToString("N2")} грн";
            withdrawnUAN.Text = $" {receipt.CardInfo.WithdrawnUAN.ToString("N2")} грн";
            remainderUAN.Text = $" {receipt.CardInfo.RemainderUAN.ToString("N2")} грн";

            //extra offer

            offerImg1 = receiptWindow.FindViewById<ImageView>(Resource.Id.offer_img_1);
            offerName1 = receiptWindow.FindViewById<TextView>(Resource.Id.offer_name_1);
            offerDescr1 = receiptWindow.FindViewById<TextView>(Resource.Id.offer_descr_1);
            offerOldPrice1 = receiptWindow.FindViewById<TextView>(Resource.Id.offer_old_price_1);
            offerNewPrice1 = receiptWindow.FindViewById<TextView>(Resource.Id.offer_new_price_1);
            offerName1.Text = receipt.Offer1.Name;
            offerDescr1.Text = receipt.Offer1.Description;
            
            offerOldPrice1.Text = "";
            if (receipt.Offer1.OldPrice != 0)
                offerOldPrice1.Text = $"{receipt.Offer1.OldPrice.ToString("N2")} грн";
            
                

            offerOldPrice1.PaintFlags = PaintFlags.StrikeThruText;
            offerNewPrice1.Text = $"{receipt.Offer1.NewPrice.ToString("N2")} грн";
            var bm1 = ConvertStringBase64ToBitmap(receipt.Offer1.ImgBase64Str);
            if(bm1 != null)
                offerImg1.SetImageBitmap(bm1); 

            offerImg2 = receiptWindow.FindViewById<ImageView>(Resource.Id.offer_img_2);
            offerName2 = receiptWindow.FindViewById<TextView>(Resource.Id.offer_name_2);
            offerDescr2 = receiptWindow.FindViewById<TextView>(Resource.Id.offer_descr_2);
            offerOldPrice2 = receiptWindow.FindViewById<TextView>(Resource.Id.offer_old_price_2);
            offerNewPrice2 = receiptWindow.FindViewById<TextView>(Resource.Id.offer_new_price_2);
            offerName2.Text = receipt.Offer2.Name;
            offerDescr2.Text = receipt.Offer2.Description;
            
            offerOldPrice2.Text = "";
            if(receipt.Offer2.OldPrice !=0)
                offerOldPrice2.Text = $"{receipt.Offer2.OldPrice.ToString("N2")} грн";
            
            offerOldPrice2.PaintFlags = PaintFlags.StrikeThruText;
            offerNewPrice2.Text = $"{receipt.Offer2.NewPrice.ToString("N2")} грн";
            var bm2 = ConvertStringBase64ToBitmap(receipt.Offer2.ImgBase64Str);
            if(bm2 != null)
                offerImg2.SetImageBitmap(bm2);
        }


        public void Initialize()
        {
            receiptManager.ActionFinishReceipt += HideReceiptWindow;
            receiptManager.ActionStartReceipt += model => StartReceipt(model);
            viewManager.ActionChangeUpperView += ChangeUpperView;
            viewManager.ActionChangeBottomView += view => ChangeBottomView(view);
            viewManager.ActionOpenShift += OpenShift;
            viewManager.ActionCloseShift += CloseShift;
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
                return null;
            }
        }

        protected void SetUiFlags()
        {
            decorView = Window.DecorView;
            decorView.SystemUiVisibility = (StatusBarVisibility) (SystemUiFlags.HideNavigation
                                                                  | SystemUiFlags.LayoutFullscreen
                                                                  | SystemUiFlags.LayoutHideNavigation
                                                                  | SystemUiFlags.Fullscreen
                                                                  | SystemUiFlags.LowProfile
                                                                  | SystemUiFlags.Immersive);
        }
    }
}