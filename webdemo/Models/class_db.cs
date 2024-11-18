using Microsoft.Data.SqlClient;
using System.Data;
using static Azure.Core.HttpHeader;

namespace webdemo.Models {
    public class class_db {
        public async static Task<DataTable> ExecuteStoredProcAsync(string procName, SqlParameter[] parameters = null) {
            DBManager objDbHelper = new DBManager();
            var dataTable = new DataTable();
            try {
                objDbHelper.cmd.CommandText = procName;
                objDbHelper.cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null) {
                    objDbHelper.cmd.Parameters.AddRange(parameters);
                }
                using (var dataAdapter = new SqlDataAdapter(objDbHelper.cmd)) {
                    await Task.Run(() => dataAdapter.Fill(dataTable));
                }
            } catch (Exception ex) {
                Class_common ObjComm = new Class_common();
                ObjComm.ErrorLog(ex.Message + "////" + ex.StackTrace, "class_db ExecuteStoredProcAsync");
            } finally {
                objDbHelper.Close();
            }
            return dataTable;
        }
        public async static Task<int> ExecuteNonQueryAsync(string procName, SqlParameter[] parameters = null) {
            DBManager objDbHelper = new DBManager();
            int rowsAffected = 0;
            try {
                objDbHelper.cmd.CommandText = procName;
                objDbHelper.cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null) {
                    objDbHelper.cmd.Parameters.AddRange(parameters);
                }
                rowsAffected = await Task.Run(() => objDbHelper.cmd.ExecuteNonQuery());
            } catch (Exception ex) {
                Class_common ObjComm = new Class_common();
                ObjComm.ErrorLog(ex.Message + "////" + ex.StackTrace, "class_db ExecuteNonQueryAsync");
            } finally {
                objDbHelper.Close();
            }
            return rowsAffected;
        }

      
    }
}
