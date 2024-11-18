using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
namespace webdemo.Models {
    public class Class_common {
        public void ErrorLog(string LogText, string LogTitle = "ErrorLog") {
            string fileName = LogTitle + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "//LOG", fileName);
            File.WriteAllText(filePath, LogText);
        }
    }
}
