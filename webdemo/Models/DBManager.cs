using System;

using webdemo.Models;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;

namespace webdemo.Models
{
    public class DBManager
    {
        private string csComm = "";
        private string csMain = "";

        public SqlConnection conComm;
        public SqlConnection conMain;
        public SqlCommand cmd = new SqlCommand();
        public DBManager(string conn = "csMain")
        {
            string HostType = class_Init.HostType;
            switch (HostType)
            {
                case "LOC_UAT":
                    csComm = "Server=MAK\\MSSQLSERVERNEW;Database=demodb;Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=True;";
                    csMain = "Server=MAK\\MSSQLSERVERNEW;Database=demodb;Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=True;";
                    break;
                case "ONL_UAT":
                    csComm = "Server=serverurl;Database=dbname;User Id=admin;password=password;Trusted_Connection=False;MultipleActiveResultSets=true;";
                    csMain = "Server=serverurl;Database=dbname;User Id=admin;password=password;Trusted_Connection=False;MultipleActiveResultSets=true;";
                    break;
                default:
                    break;
            }
            try {
                conComm = new SqlConnection(csComm);
                conMain = new SqlConnection(csMain);
                if (conn == "csComm")
                {
                    cmd.Connection = conComm;
                    conComm.Open();
                }
                else
                {
                    cmd.Connection = conMain;
                    conMain.Open(); 
                }
            }
            catch (Exception ex)
            {
                Class_common ObjComm = new Class_common();
                ObjComm.ErrorLog(ex.Message + "////" + ex.StackTrace, "DBManager New");
            }

          
        }
        public void Close()
        {
            conComm.Close();
            conMain.Close();
        }
    }
}




