using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;

using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using System.IO;
using System.Security.Cryptography;
using SQLite;
using Android.Runtime;
using Android.Content.PM;

namespace App1
{
    [Activity(Label = "LoginPage", ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginPage : Activity
    {
        View customActionBarView;
        String email = "", userName = "", pass = "";
        EditText emailText, userNameText, passText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.LoginPage);

            customActionBarView = LayoutInflater.Inflate(Resource.Layout.custom_action_bar, null);

            //customFontRegular = Typeface.createFromAsset(this.getAssets(), "font/Lato_Regular.ttf");
            //customFontSemiBold = Typeface.createFromAsset(this.getAssets(), "font/Lato_Semibold.ttf");

            TextView titleText = (TextView)customActionBarView.FindViewById(Resource.Id.title);
            TextView nameText = (TextView)customActionBarView.FindViewById(Resource.Id.name);
            ImageButton notifyConfig = (ImageButton)customActionBarView.FindViewById(Resource.Id.notifyConfig);

            //nameText.setTypeface(customFontRegular);
            nameText.Text = "Users";
            //nameText.SetTextColor(getResources().getColor(R.color.white_color));

            //titleText.setTypeface(customFontSemiBold);
            titleText.Text = "";
            //titleText.SetTextColor(getResources().getColor(R.color.white_color));

            //notifyConfig.setVisibility(View.INVISIBLE);

            //Use notifyConfig image button for the plus button functionality for the configuration page
            //notifyConfig.SetBackgroundResource(Resource.Drawable.plus_button);

            notifyConfig.Visibility = ViewStates.Invisible;

            //Drawable leftArrowImage = getResources().getDrawable(Resource.Drawable.arrow_left);
            //leftArrowImage.setAlpha(255); //Make it visible
            ActionBar.SetHomeAsUpIndicator(Resource.Drawable.arrow_left);

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            //ActionBar.SetDisplayOptions(ActionBar.DISPLAY_SHOW_CUSTOM);
            ActionBar.SetDisplayShowHomeEnabled(false);
            ActionBar.SetDisplayShowTitleEnabled(false);

            ActionBar.SetDisplayShowCustomEnabled(true);
            ActionBar.SetCustomView(customActionBarView, new ActionBar.LayoutParams(WindowManagerLayoutParams.WrapContent, WindowManagerLayoutParams.WrapContent));

            nameText.Click += delegate
            {
                OnBackPressed();
            };

            //TableLayout configurationLayout = (TableLayout)findViewById(R.id.configurationLayout);


            //InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            //inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
            Button submit = (Button)FindViewById<Button>(Resource.Id.SubmitClicked);
            submit.Click += delegate {
                OnClickSubmit();
            };

            emailText = (EditText)FindViewById(Resource.Id.emailText);
            userNameText = (EditText)FindViewById(Resource.Id.userNameText);
            passText = (EditText)FindViewById(Resource.Id.passText);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(emailText.WindowToken, 0);
            imm.HideSoftInputFromWindow(userNameText.WindowToken, 0);
            imm.HideSoftInputFromWindow(passText.WindowToken, 0);
            return base.OnTouchEvent(e);
        }

        public void OnClickSubmit()
        {
            //validateUser(email, userName, pass);

            email = emailText.Text;
            userName = userNameText.Text;
            pass = passText.Text;

            if (email.Equals(""))
            {
                ShowAlertDialog("Email field is empty", "Please enter Email");
            }
            else if (userName.Equals(""))
            {
                ShowAlertDialog("User Name field is empty", "Please enter User Name");
            }
            else if (pass.Equals(""))
            {
                ShowAlertDialog("Password field is empty", "Please enter Password");
            }
            else
            {
                ValidateUser(email, userName, pass);
            }

        }

        public void ShowAlertDialog(String title, String message)
        {
            //set alert for executing the task
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle(title);
            alert.SetMessage(message);

            alert.SetNegativeButton("Close", (senderAlert, args) => {
                
            });

            Dialog dialog = alert.Create();
            dialog.Show();
        }

        public void InsertEntryIntoDB(String email, String userName, String pass)
        {
            try
            {
                string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3");
                var db = new SQLiteConnection(dpPath);
                db.CreateTable<UsersTable>();
                UsersTable tbl = new UsersTable
                {
                    Email = email,
                    Username = userName,
                    Password = pass
                };
                db.Insert(tbl);
                Toast.MakeText(this, "User Record Added Successfully...,", ToastLength.Short).Show();

                var activity2 = new Intent(this, typeof(MainActivity));
                //activity2.PutExtra("MyData", "Data from Activity1");
                StartActivity(activity2);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
            }
        }

        public bool IsValidPassword(string charSequence)
        {
            Console.WriteLine("charSequence:   {0}", charSequence);
            Console.WriteLine("charSequence:   {0}", charSequence.Length);
            if (charSequence == null || charSequence.Length < 5 || charSequence.Length > 12)
            {
                return false;
            }

            bool foundNumber = false;
            bool foundAlphaCharacter = false;

            for (int x = 0; x < charSequence.Length; x++)
            {
                char singleCharacter = charSequence[x];

                Console.WriteLine("singleCharacter:   {0}", singleCharacter);
                // return false if a character is found that is not alphanumeric
                if (!char.IsLetterOrDigit(singleCharacter))
                {
                    return false;
                }
                // return false if an alpha character is uppercase
                if (char.IsLetterOrDigit(singleCharacter) && char.IsUpper(singleCharacter))
                {
                    return false;
                }
                // allow pairs of consecutive characters
                //			// return false if consecutive characters are found
                //			if (x > 0 && singleCharacter == charSequence.charAt(x-1)){
                //				return false;
                //			} else 	
                // return false if three consecutive of the same character are found
                if (x > 1 && charSequence.Substring(x - 2, 2).Equals(charSequence.Substring(x - 1, 2)))
                {
                    return false;
                    // return false if consecutive pairs of characters are found
                }
                else if (x > 2 && charSequence.Substring(x - 3, 2).Equals(charSequence.Substring(x - 1, 2)))
                {
                    return false;
                    // return false if consecutive triplets of characters are found
                }
                else if (x > 4 && charSequence.Substring(x - 5, 3).Equals(charSequence.Substring(x - 2, 3)))
                {
                    return false;
                    // return false if consecutive quadruplets of characters are found
                }
                else if (x > 6 && charSequence.Substring(x - 7, 4).Equals(charSequence.Substring(x - 3, 4)))
                {
                    return false;
                    // return false if consecutive quintuplets of characters are found
                }
                else if (x > 8 && charSequence.Substring(x - 9, 5).Equals(charSequence.Substring(x - 4, 5)))
                {
                    return false;
                    // return false if consecutive sextuplets of characters are found
                }
                else if (x > 10 && charSequence.Substring(x - 11, 6).Equals(charSequence.Substring(x - 5, 6)))
                {
                    return false;
                }

                // confirm that the password has at least one lowercase character and one number
                if (char.IsLetter(singleCharacter))
                {
                    foundAlphaCharacter = true;
                }
                else if (char.IsDigit(singleCharacter))
                {
                    foundNumber = true;
                }

                Console.WriteLine("x:   {0}", x);
            }

            Console.WriteLine("foundNumber:   {0}", foundNumber);

            return foundNumber && foundAlphaCharacter;
        }

        public void ValidateUser(String email, String userName, String pass)
        {
            try
            {
                using (Aes myAes = Aes.Create())
                {

                    bool status = IsValidPassword(pass.Trim());

                    Console.WriteLine("status:   {0}", status);

                    if (!status)
                    {
                        ShowAlertDialog("Password is invalid", "Please enter valid password");
                        return;
                    }

                    //string original = "Here is some data to encrypt!";

                    // Encrypt the string to an array of bytes.
                    byte[] encrypted = EncryptStringToBytes_Aes(pass, myAes.Key, myAes.IV);
                    //string encryptedMsg = System.Text.Encoding.UTF8.GetString(encrypted);
                    string encryptedMsg = Convert.ToBase64String(encrypted);

                    InsertEntryIntoDB(email, userName, encryptedMsg);

                    byte[] decBytes = Convert.FromBase64String(encryptedMsg);

                    // Decrypt the bytes to a string.
                    string decryptedMsg = DecryptStringFromBytes_Aes(decBytes, myAes.Key, myAes.IV);

                    //Display the original data and the decrypted data.
                    Console.WriteLine("Original:   {0}", pass);
                    //Console.WriteLine("encrypted:   {0}", encrypted);
                    Console.WriteLine("encryptedMsg:   {0}", encryptedMsg);
                    Console.WriteLine("decryptedMsg: {0}", decryptedMsg);
                }

            }catch (Exception e){
                //handle error
            }
        }


        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        //Console.WriteLine("msEncrypt:   {0}", msEncrypt);
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }


        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }


        public override void OnBackPressed()
        {
            Finish();
        }

    }
}