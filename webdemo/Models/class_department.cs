using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection;
namespace webdemo.Models {
    public class class_department {
        #region Property
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public string? Description { get; set; }
        #endregion

        #region Validation
        public void Validate(ModelStateDictionary modelState) {
            if (string.IsNullOrWhiteSpace(DepartmentName)) {
                modelState.AddModelError(nameof(DepartmentName), "Department Name is required.");
            }
            if (Description != null && Description.Length > 500) {
                modelState.AddModelError(nameof(Description), "Description cannot exceed 500 characters.");
            }
        }

        #endregion

        #region Functions
        public async static Task<DataTable> GetListAsync(int? departmentId = null) {
            var parameters = new List<SqlParameter>();
            if (departmentId.HasValue) {
                parameters.Add(new SqlParameter("@DepartmentID", departmentId.Value));
            }
            return await class_db.ExecuteStoredProcAsync("ListDepartments", parameters.ToArray());
        }


        public async static Task<int> SaveAsync(class_department objdepartment) {
            string procName = objdepartment.DepartmentId > 0 ? "UpdateDepartment" : "InsertDepartment";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@DepartmentName", objdepartment.DepartmentName),
                new SqlParameter("@Description", objdepartment.Description)
            };
            if (objdepartment.DepartmentId > 0) {
                parameters.Add(new SqlParameter("@DepartmentID", objdepartment.DepartmentId));
            }

            return await class_db.ExecuteNonQueryAsync(procName, parameters.ToArray());
        }
        #endregion

        #region MapFromDataRow
        public static class_department MapFromDataRow(DataRow row) {
            return new class_department {
                DepartmentId = Convert.ToInt32(row["DepartmentId"]),
                DepartmentName = row["DepartmentName"].ToString(),
                Description = row["Description"].ToString()
            };
        }
        #endregion

        #region SelectList
        public async static Task<List<object>> GetList() {
            DataTable dt = await GetListAsync();
            List<object> departments = new List<object>();
            foreach (DataRow row in dt.Rows) {
                departments.Add(new {
                    id = Convert.ToInt32(row["DepartmentID"]),
                    name = row["DepartmentName"].ToString()
                });
            }
            return departments;
        }
        #endregion
    }
}
