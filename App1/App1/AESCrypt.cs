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

namespace App1
{
    class AESCrypt
    {
     
        /*
        private static String TAG = "AESCrypt";

        //AESCrypt-ObjC uses CBC and PKCS7Padding
        private static String AES_MODE = "AES/CBC/PKCS7Padding";
        private static String CHARSET = "UTF-8";

        //AESCrypt-ObjC uses SHA-256 (and so a 256-bit key)
        private static String HASH_ALGORITHM = "SHA-256";

        //AESCrypt-ObjC uses blank IV (not the best security, but the aim here is compatibility)
        //private static final byte[] ivBytes = {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};

        static String iv = "fedcba9876543210";
        private static byte[] ivBytes = Encoding.ASCII.GetBytes(iv);

        //togglable log option (please turn off in live!)
        public static bool DEBUG_LOG_ENABLED = false;

        private static SecretKeySpec generateKey(String password) 
        {
            MessageDigest digest = MessageDigest.getInstance(HASH_ALGORITHM);
            byte[] bytes = Encoding.ASCII.GetBytes(password);
            digest.update(bytes, 0, bytes.length);
            byte[] key = digest.digest();

            SecretKeySpec secretKeySpec = new SecretKeySpec(key, "AES");
            return secretKeySpec;
        }



        public static String encrypt(String message)       
        {

            try {
                //final SecretKeySpec key = generateKey(password);
                

                //Change Key and iv according to iOS
                String keyStr = "0123456789abcdef";
                SecretKeySpec key = new SecretKeySpec(Encoding.ASCII.GetBytes(keyStr), "AES");

                //String iv = "fedcba9876543210";
                byte[] cipherText = encrypt(key, ivBytes, Encoding.ASCII.GetBytes(message));

                //NO_WRAP is important as was getting \n at the end
                String encoded = Base64.encodeToString(cipherText, Base64.NO_WRAP);
                
                return encoded;
            } catch (Exception e) {

            }
        }


        public static byte[] encrypt(SecretKeySpec key, byte[] iv, byte[] message)
        {
            Cipher cipher = Cipher.getInstance(AES_MODE);
            IvParameterSpec ivSpec = new IvParameterSpec(iv);
            cipher.init(Cipher.ENCRYPT_MODE, key, ivSpec);
            byte[] cipherText = cipher.doFinal(message);

            return cipherText;
        }



        public static String decrypt(String password, String base64EncodedCipherText)
        {
                try {
                    SecretKeySpec key = generateKey(password);
                    byte[] decodedCipherText = Base64.decode(base64EncodedCipherText, Base64.NO_WRAP);
                    byte[] decryptedBytes = decrypt(key, ivBytes, decodedCipherText);
                    String message = new String(decryptedBytes, CHARSET);
                
                    return message;
                } catch (Exception e) {

                }
        }



        public static byte[] decrypt(SecretKeySpec key, final byte[] iv, byte[] decodedCipherText)
        {
            Cipher cipher = Cipher.getInstance(AES_MODE);
            IvParameterSpec ivSpec = new IvParameterSpec(iv);
            cipher.init(Cipher.DECRYPT_MODE, key, ivSpec);
            byte[] decryptedBytes = cipher.doFinal(decodedCipherText);

            return decryptedBytes;
        }
        


        private static String bytesToHex(byte[] bytes)
        {
            char[] hexArray = {'0', '1', '2', '3', '4', '5', '6', '7', '8',
                        '9', 'A', 'B', 'C', 'D', 'E', 'F'};
            char[] hexChars = new char[bytes.length * 2];
            int v;
            for (int j = 0; j < bytes.length; j++)
            {
                v = bytes[j] & 0xFF;
                hexChars[j * 2] = hexArray[v >>> 4];
                hexChars[j * 2 + 1] = hexArray[v & 0x0F];
            }
            return new String(hexChars);
        }

        private AESCrypt()
        {
        }


        */
    }
}