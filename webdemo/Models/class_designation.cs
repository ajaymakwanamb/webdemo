using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection;
namespace webdemo.Models {
    public class class_designation {
        #region Property
        public int DesignationId { get; set; }
        public string? DesignationName { get; set; }
        public string? Description { get; set; }
        #endregion

        #region Validation
        public void Validate(ModelStateDictionary modelState) {
            if (string.IsNullOrWhiteSpace(DesignationName)) {
                modelState.AddModelError(nameof(DesignationName), "Designation Name is required.");
            }
            if (Description != null && Description.Length > 500) {
                modelState.AddModelError(nameof(Description), "Description cannot exceed 500 characters.");
            }
        }

        #endregion

        #region Functions
        public async static Task<DataTable> GetListAsync(int? designationId = null) {
            var parameters = new List<SqlParameter>();
            if (designationId.HasValue) {
                parameters.Add(new SqlParameter("@designationID", designationId.Value));
            }
            return await class_db.ExecuteStoredProcAsync("Listdesignations", parameters.ToArray());
        }
        public async static Task<int> SaveAsync(class_designation objdesignation) {
            string procName = objdesignation.DesignationId > 0 ? "UpdateDesignation" : "InsertDesignation";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@DesignationName", objdesignation.DesignationName),
                new SqlParameter("@Description", objdesignation.Description)
            };
            if (objdesignation.DesignationId > 0) {
                parameters.Add(new SqlParameter("@designationID", objdesignation.DesignationId));
            }
            return await class_db.ExecuteNonQueryAsync(procName, parameters.ToArray());
        }
        #endregion
    }
}
