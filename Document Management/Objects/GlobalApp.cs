using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement
{
    public static class GlobalApp
    {
       public static GlobalOptions Options { get; set; }
       public static Encryption Encryption { get; set; }
    }

    public class GlobalOptions
    {
        private Dictionary<string, string> options = new Dictionary<string, string>();

        public GlobalOptions()
        {
            options.Add("app_path", System.Windows.Forms.Application.StartupPath + @"\");

            dynamic jsonArray = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(options["app_path"] + @"options.json"));

            if (jsonArray != null) {
                foreach (dynamic element in jsonArray)
                {
                    string key = element.Name;
                    string value = element.Value;
                    options.Add(key, value);
                }
            }
        }

        public string getOption (string key)
        {
            if (options.ContainsKey(key))
            {
                return options[key];
            } else
            {
                return "";
            }
        }

        public void editOption (string key, string value)
        {
            if (options.ContainsKey(key))
            {
                options[key] = value;
            } else
            {
                options.Add(key, value);
            }
        }

        public void saveOptions ()
        {
            // Runtime determined options may not be saved to the file!
            string app_path = options["app_path"];
            options.Remove("app_path");

            File.WriteAllText(app_path + @"options.json", JsonConvert.SerializeObject(options));

            // Add the runtime options back to the Dictionary
            options.Add("app_path", app_path);
        }
    }

    public class Encryption
    {
        private const int SaltSize = 8;

        public Encryption() { }

        public void encrypt(FileInfo targetFile, string password, List<string> lines)
        {
            var keyGenerator = new Rfc2898DeriveBytes(password, SaltSize);
            var rijndael = Rijndael.Create();

            Console.WriteLine(rijndael.BlockSize.ToString());
            Console.WriteLine(rijndael.KeySize.ToString());

            // BlockSize, KeySize in bit --> divide by 8
            rijndael.IV = keyGenerator.GetBytes(rijndael.BlockSize / 8);
            rijndael.Key = keyGenerator.GetBytes(rijndael.KeySize / 8);

            using (var fileStream = targetFile.Create())
            {
                // write random salt
                fileStream.Write(keyGenerator.Salt, 0, SaltSize);

                using (var cryptoStream = new CryptoStream(fileStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    bformatter.Serialize(cryptoStream, lines);
                    cryptoStream.FlushFinalBlock();
                }
            }
        }


        public string decrypt(FileInfo sourceFile, string password)
        {
            string output = "";

            // read salt
            var fileStream = sourceFile.OpenRead();
            var salt = new byte[SaltSize];
            fileStream.Read(salt, 0, SaltSize);

            // initialize algorithm with salt
            var keyGenerator = new Rfc2898DeriveBytes(password, salt);
            var rijndael = Rijndael.Create();
            rijndael.IV = keyGenerator.GetBytes(rijndael.BlockSize / 8);
            rijndael.Key = keyGenerator.GetBytes(rijndael.KeySize / 8);

            // decrypt
            using (var cryptoStream = new CryptoStream(fileStream, rijndael.CreateDecryptor(), CryptoStreamMode.Read))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                List<string> lines = (List<string>)bformatter.Deserialize(cryptoStream);
                foreach (string line in lines)
                {
                    output += line;
                }
            }

            return output;
        }
    }
}
