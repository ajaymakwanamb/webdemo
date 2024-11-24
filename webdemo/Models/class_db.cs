using Microsoft.Data.SqlClient;
using System.Data;
using static Azure.Core.HttpHeader;

namespace webdemo.Models {
    public class class_db {
        #region Execute Stored Procedure
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
        #endregion

        #region Execute Query
        public async static Task<int> ExecuteNonQueryAsync(string procName, SqlParameter[] parameters = null) {
            DBManager objDbHelper = new DBManager();
            int rowsAffected = 0;
            var outputParam = new SqlParameter();
            try {
                objDbHelper.cmd.CommandText = procName;
                objDbHelper.cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null) {
                    objDbHelper.cmd.Parameters.AddRange(parameters);
                }
                if (procName.ToLower().Contains("insert") || procName.ToLower().Contains("update")) {
                    outputParam = new SqlParameter("@RowsAffected", SqlDbType.Int) {
                        Direction = ParameterDirection.Output
                    };
                    objDbHelper.cmd.Parameters.Add(outputParam);
                }
                await Task.Run(() => objDbHelper.cmd.ExecuteNonQuery());
                rowsAffected = (int)outputParam.Value;
            } catch (Exception ex) {
                Class_common ObjComm = new Class_common();
                ObjComm.ErrorLog(ex.Message + "////" + ex.StackTrace, "class_db ExecuteNonQueryAsync");
            } finally {
                objDbHelper.Close();
            }
            return rowsAffected;
        }
        #endregion
    }
}
