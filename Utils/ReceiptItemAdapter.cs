using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using WebHost.Models;

namespace WebHostXam.Utils
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
                
                TextView textReceiptItemName = view.FindViewById<TextView>(Resource.Id.receipt_item_name);
                textReceiptItemName.Text = items[position].Name;
                
                TextView textReceiptItemDescription = view.FindViewById<TextView>(Resource.Id.description);
                textReceiptItemDescription.Text = items[position].Description;
                
                TextView textReceiptItemPrice = view.FindViewById<TextView>(Resource.Id.price);
                textReceiptItemPrice.Text = items[position].Price.ToString();
                
               
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
        

        public override string this[int position] => items[position].Id.ToString();
    }
}