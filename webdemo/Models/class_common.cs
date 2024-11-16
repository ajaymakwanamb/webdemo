using System.Security.Cryptography;
using System.Text.RegularExpressions;
namespace webdemo.Models {
    public class Class_common {
        public void ErrorLog(string LogText, string LogTitle = "ErrorLog") {
            string fileName = LogTitle + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "//LOG", fileName);
            File.WriteAllText(filePath, LogText);
        }
        public static string Encrypt(string plainText, string key, string iv) {
            using (Aes aesAlg = Aes.Create()) {
                aesAlg.Key = Convert.FromBase64String(key);
                aesAlg.IV = Convert.FromBase64String(iv);
                using (MemoryStream msEncrypt = new MemoryStream()) {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(), CryptoStreamMode.Write)) {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt)) {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText, string key, string iv) {
            using (Aes aesAlg = Aes.Create()) {
                aesAlg.Key = Convert.FromBase64String(key);
                aesAlg.IV = Convert.FromBase64String(iv);
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText))) {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(), CryptoStreamMode.Read)) {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt)) {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
        public static void GenerateKey(ref string Key, ref string IV) {
            using (Aes aesAlg = Aes.Create()) {
                aesAlg.GenerateKey(); 
                aesAlg.GenerateIV();  
                Key = Convert.ToBase64String(aesAlg.Key);
                IV = Convert.ToBase64String(aesAlg.IV);
            }
        }
      
        public bool ValidateMobileNumber(string MobileNo) {
            string pattern = @"^\d{10}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(MobileNo);
        }
    }
}
