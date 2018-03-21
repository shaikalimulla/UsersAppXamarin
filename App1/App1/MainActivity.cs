using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Views;
using Android.Util;
using System;
using SQLite;
using System.IO;
using Android.Content.PM;

namespace App1
{
    [Activity(Label = "UserApp", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {

        View customActionBarView;
        private ListView usersListView;
        UsersListAdapter usersList;
        TextView emptyText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            
            //LayoutInflater inflater = (LayoutInflater) getActionBar().getThemedContext().getSystemService(LAYOUT_INFLATER_SERVICE);

            customActionBarView = LayoutInflater.Inflate(Resource.Layout.custom_action_bar, null);

            //customFontRegular = Typeface.createFromAsset(this.getAssets(), "font/Lato_Regular.ttf");
            //customFontSemiBold = Typeface.createFromAsset(this.getAssets(), "font/Lato_Semibold.ttf");

            TextView titleText = (TextView)customActionBarView.FindViewById(Resource.Id.title);
            TextView nameText = (TextView)customActionBarView.FindViewById(Resource.Id.name);
            ImageButton notifyConfig = (ImageButton)customActionBarView.FindViewById(Resource.Id.notifyConfig);

            //nameText.setTypeface(customFontRegular);
            nameText.Text = "";
            //nameText.SetTextColor(getResources().getColor(R.color.white_color));

            //titleText.setTypeface(customFontSemiBold);
            titleText.Text = "Users List";
            //titleText.SetTextColor(getResources().getColor(R.color.white_color));

            //notifyConfig.setVisibility(View.INVISIBLE);

            //Use notifyConfig image button for the plus button functionality for the configuration page
            notifyConfig.SetBackgroundResource(Resource.Drawable.plus_button);

            //ActionBar.SetDisplayOptions(ActionBar.DISPLAY_SHOW_CUSTOM);
            ActionBar.SetDisplayShowHomeEnabled(false);
            ActionBar.SetDisplayShowTitleEnabled(false);

            ActionBar.SetDisplayShowCustomEnabled(true);
            ActionBar.SetCustomView(customActionBarView, new ActionBar.LayoutParams(WindowManagerLayoutParams.WrapContent, WindowManagerLayoutParams.WrapContent));

            notifyConfig.Click += delegate {

                //Call Your Method When User Clicks The Button
                OnClickNotifyConfig();
            };

            usersListView = (ListView) FindViewById(Resource.Id.UsersListView);
            usersList = new UsersListAdapter(this);
            
            usersListView.SetAdapter(usersList);

            usersListView.ItemLongClick += listView_ItemLongClick;

            emptyText = (TextView) FindViewById(Resource.Id.Empty);
            /*emptyText.Text = "There are currently no users. Please add users by clicking + button.";
            //emptyText.setTypeface(customFontRegular);
            usersListView.EmptyView = emptyText;
            */
 

        }

        public void HandleDeleteUserEntry(int position)
        {
            string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3");
            var db = new SQLiteConnection(dpPath);

            //db.DropTable<UsersTable>();
            var selectedUserName = usersList.GetUserName(position);
  
            Console.WriteLine("selectedUserName:   {0}", selectedUserName);
            Console.WriteLine("position:   {0}", position);
            //Console.WriteLine("data1:   {0}", data1);

            var table = db.Table<UsersTable>();
            foreach (var tbl in table)
            {
                if(tbl.Username == selectedUserName)
                {
                    db.Delete<UsersTable>(tbl.Id);

                    usersList.RemoveUser(position);
                    usersList.NotifyDataSetChanged();
                }
                
            }

        }

        public void ShowAlertDialog(String title, String message, int position)
        {
            //set alert for executing the task
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle(title);
            alert.SetMessage(message);

            alert.SetPositiveButton("Yes", (senderAlert, args) => {
                HandleDeleteUserEntry(position);
            });

            alert.SetNegativeButton("Close", (senderAlert, args) => {

            });

            Dialog dialog = alert.Create();
            dialog.Show();
        }

        private void listView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            var selected = e.Position;

            ShowAlertDialog("Delete User", "Are you sure you want to proceed?", selected);
        }

        protected override void OnResume()
        {
            base.OnResume();
            ReadUsersDatabase();
        }

        public static bool TableExists<T>(SQLiteConnection connection)
        {
            const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
            var cmd = connection.CreateCommand(cmdText, typeof(T).Name);
            return cmd.ExecuteScalar<string>() != null;
        }

        public void ReadUsersDatabase()
        {
            Console.WriteLine("Reading data");
            string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3");
            var db = new SQLiteConnection(dpPath);

            if (emptyText != null)
                emptyText.Visibility = ViewStates.Visible;

            if (TableExists<UsersTable>(db))
            {
                //db.DropTable<UsersTable>();

                var table = db.Table<UsersTable>();
                foreach (var tbl in table)
                {
                    if (emptyText != null)
                        emptyText.Visibility = ViewStates.Invisible;

                    Console.WriteLine(tbl.Id + " " + tbl.Username + " " + tbl.Password);
                    usersList.AddUser(Resource.Drawable.vessel_image, tbl.Email, tbl.Username);
                }
            }
        }

        //Use notifyConfig image button for the plus button functionality for the configuration page
        public void OnClickNotifyConfig()
        {      
            var activity2 = new Intent(this, typeof(LoginPage));
            //activity2.PutExtra("MyData", "Data from Activity1");
            StartActivity(activity2);
            
        }
}
}

