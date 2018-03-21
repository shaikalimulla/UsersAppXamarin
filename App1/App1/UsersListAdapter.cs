using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using static Android.Resource;

namespace App1
{
    class UsersListAdapter : BaseAdapter
    {
        List<int> imagesList;
        List<string> emailList;
        List<string> userNameList;
        private LayoutInflater inflater;
        private Activity context;

        int updateImagePosition = -1;
        //Bitmap updateImageBitMap;
        public UsersListAdapter(Activity parent)
        {
            context = parent;

            imagesList = new List<int>();
            emailList = new List<string>();
            userNameList = new List<string>();

            inflater = parent.LayoutInflater;
        }

        public bool IsUserPresent(string userName)
        {
            if (userNameList.Contains(userName) == true)
                return true;

            return false;
        }

        public void AddUser(int imageId, string email, string userName)
        {
            if (IsUserPresent(userName))
            {
                return;
            }

            imagesList.Add(imageId);
            emailList.Add(email);
            userNameList.Add(userName);
        }

        public void RemoveUser(int position)
        {
            if (emailList.Count <= 0)
            {
                return;
            }

            imagesList.RemoveAt(position);
            emailList.RemoveAt(position);
            userNameList.RemoveAt(position);
        }

        public string GetEmail(int index)
        {
            return emailList[index];
        }

        public string GetUserName(int index)
        {
            return userNameList[index];
        }

        /*public string getVesselName(int index)
        {
            return vesselNameList.get(index);
        }

        public int getCurrIndex(string gateway)
        {
            return emailList.indexOf(gateway);
        }

        public void updateGatewayImage(int positionIndex, Bitmap currentImage)
        {
            //int deviceIndex = getCurrIndex(gateway);
            if (positionIndex < 0)
                return;

            updateImagePosition = positionIndex;
            updateImageBitMap = currentImage;
            //imagesList.set(positionIndex, imageId);
            //imagesList.setImage(currentImage);
        }*/

        public void clearList()
        {
            imagesList.Clear();
            emailList.Clear();
            userNameList.Clear();

        }

        public override int Count
        {
            get { return userNameList.Count; }
        }

        
        public override Java.Lang.Object GetItem(int position)
        {
            return GetUserName(position);
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            // get already available view or create new if necessary
            FieldReferences fields;
            //Typeface customFontRegular, customFontSemiBold;
            int imageId;
            //string vesselName, marinaName = "", slipNo = "";

            if (convertView == null)
            {
                //convertView = context.LayoutInflater.Inflate(Resource.Layout.list_row, parent, false);
                convertView = inflater.Inflate(Resource.Layout.list_row, null);
                fields = new FieldReferences();
                fields.userName = (TextView)convertView.FindViewById(Resource.Id.userName);
                fields.email = (TextView)convertView.FindViewById(Resource.Id.email);
                fields.img = (ImageView)convertView.FindViewById(Resource.Id.device_img);
                convertView.Tag = fields;
            }
            else
            {
                fields = (FieldReferences) convertView.Tag;
            }

            //customFontRegular = Typeface.createFromAsset(context.getAssets(), "font/Lato_Regular.ttf");
            //customFontSemiBold = Typeface.createFromAsset(context.getAssets(), "font/Lato_Semibold.ttf");

            imageId = imagesList[position];

            fields.img.SetImageResource(imageId);
            fields.userName.Text = userNameList[position];
            fields.email.Text = emailList[position];

            return convertView;
        }

        public class FieldReferences : Java.Lang.Object
        {
            public TextView userName;
            public TextView email;
            public ImageView img;
        }

    }
}