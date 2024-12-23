﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        #region MapFromDataRow
        public static class_designation MapFromDataRow(DataRow row) {
            return new class_designation {
                DesignationId = Convert.ToInt32(row["DesignationId"]),
                DesignationName = row["DesignationName"].ToString(),
                Description = row["Description"].ToString()
            };
        }
        #endregion

        #region SelectList
        public async static Task<List<object>> GetList() {
            DataTable dt = await GetListAsync();
            List<object> designations = new List<object>();
            foreach (DataRow row in dt.Rows) {
                designations.Add(new {
                    id = Convert.ToInt32(row["DesignationId"]),
                    name = row["DesignationName"].ToString()
                });
            }
            return designations;
        }
        #endregion
    }
}
