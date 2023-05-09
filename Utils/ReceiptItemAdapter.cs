using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using WebHostXam.Models;

namespace WebHostXam.Android.Utils
{
    public class ReceiptItemAdapter : BaseAdapter<string>
    {
        private List<ReceiptItemModel> items;
        

        private Context _context;
        

        public ReceiptItemAdapter(Context context, List<ReceiptItemModel> inputItems)
        {
            this.items = inputItems;
            _context = context;

        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            if (view == null)
            {
                view = LayoutInflater.From(_context).Inflate(Resource.Layout.receipt_item, null, false);
                if(position == 0)
                    view.SetBackgroundColor(Color.ParseColor("#d9d9d9"));
                
                
                TextView textReceiptItemName = view.FindViewById<TextView>(Resource.Id.receipt_item_name);
                textReceiptItemName.Text = items[position].Name;
                
                TextView textReceiptItemDescription = view.FindViewById<TextView>(Resource.Id.description);
                textReceiptItemDescription.Text = items[position].Description;
                
                TextView textReceiptItemPrice = view.FindViewById<TextView>(Resource.Id.price);
                textReceiptItemPrice.Text = $"{items[position].Price.ToString("N2")} грн";
                
               
            }

            return view;
        }

        public override int Count
        {
            get
            {
                return items.Count;
            }
        }
        

        public override string this[int position] => items[position].Name;
    }
}