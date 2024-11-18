using Microsoft.Data.SqlClient;
using System.Data;

namespace webdemo.Models {
    public class class_employee {
        public async static Task<DataTable> GetListAsync(int? employeeId = null) {
            var parameters = new List<SqlParameter>();
            if (employeeId.HasValue) {
                parameters.Add(new SqlParameter("@EmployeeID", employeeId.Value));
            }
            return await class_db.ExecuteStoredProcAsync("ListEmployees", parameters.ToArray());
        }
    }
}
