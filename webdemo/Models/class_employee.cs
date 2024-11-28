using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection;
namespace webdemo.Models {
    public class class_employee {
        #region Property
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }
        public decimal? Salary { get; set; }
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        #endregion

        #region Validation
        public void Validate2(ModelStateDictionary modelState) {
            if (string.IsNullOrWhiteSpace(FirstName)) {
                modelState.AddModelError(nameof(FirstName), "First Name is required.");
            }
            if (string.IsNullOrWhiteSpace(LastName)) {
                modelState.AddModelError(nameof(LastName), "Last Name is required.");
            }

        }

        public void Validate(ModelStateDictionary modelState) {
            // Validate FirstName
            if (string.IsNullOrWhiteSpace(FirstName)) {
                modelState.AddModelError(nameof(FirstName), "First Name is required.");
            } else if (FirstName.Length > 100) {
                modelState.AddModelError(nameof(FirstName), "First Name cannot exceed 100 characters.");
            }

            // Validate LastName
            if (string.IsNullOrWhiteSpace(LastName)) {
                modelState.AddModelError(nameof(LastName), "Last Name is required.");
            } else if (LastName.Length > 100) {
                modelState.AddModelError(nameof(LastName), "Last Name cannot exceed 100 characters.");
            }

            // Validate Email
            if (string.IsNullOrWhiteSpace(Email)) {
                modelState.AddModelError(nameof(Email), "Email is required.");
            } else if (Email.Length > 100) {
                modelState.AddModelError(nameof(Email), "Email cannot exceed 100 characters.");
            }

            // Validate PhoneNumber
            if (PhoneNumber != null && PhoneNumber.Length > 15) {
                modelState.AddModelError(nameof(PhoneNumber), "Phone Number cannot exceed 15 characters.");
            }

            // Validate DateOfBirth (optional)
            if (DateOfBirth.HasValue && DateOfBirth.Value > DateTime.Now) {
                modelState.AddModelError(nameof(DateOfBirth), "Date of Birth cannot be in the future.");
            }

            // Validate HireDate
            if (HireDate == default) {
                modelState.AddModelError(nameof(HireDate), "Hire Date is required.");
            } else if (HireDate > DateTime.Now) {
                modelState.AddModelError(nameof(HireDate), "Hire Date cannot be in the future.");
            }

            // Validate Salary
            if (Salary.HasValue && Salary.Value <= 0) {
                modelState.AddModelError(nameof(Salary), "Salary must be greater than zero.");
            }

            // Validate DepartmentID
            //if (DepartmentID <= 0) {
            //    modelState.AddModelError(nameof(DepartmentID), "Department ID must be a valid positive integer.");
            //}

            //// Validate DesignationID
            //if (DesignationID <= 0) {
            //    modelState.AddModelError(nameof(DesignationID), "Designation ID must be a valid positive integer.");
            //}

            //// Validate Address
            //if (Address != null && Address.Length > 255) {
            //    modelState.AddModelError(nameof(Address), "Address cannot exceed 255 characters.");
            //}

            //// Validate City
            //if (City != null && City.Length > 100) {
            //    modelState.AddModelError(nameof(City), "City cannot exceed 100 characters.");
            //}

            //// Validate State
            //if (State != null && State.Length > 100) {
            //    modelState.AddModelError(nameof(State), "State cannot exceed 100 characters.");
            //}

            //// Validate Country
            //if (Country != null && Country.Length > 100) {
            //    modelState.AddModelError(nameof(Country), "Country cannot exceed 100 characters.");
            //}

            //// Validate PostalCode
            //if (PostalCode != null && PostalCode.Length > 10) {
            //    modelState.AddModelError(nameof(PostalCode), "Postal Code cannot exceed 10 characters.");
            //}

            //// Validate Status
            //if (string.IsNullOrWhiteSpace(Status)) {
            //    modelState.AddModelError(nameof(Status), "Status is required.");
            //} else if (Status != "Active" && Status != "Inactive") {
            //    modelState.AddModelError(nameof(Status), "Status must be either 'Active' or 'Inactive'.");
            //}
        }

        #endregion

        #region Functions
        public async static Task<DataTable> GetListAsync(int? employeeId = null) {
            var parameters = new List<SqlParameter>();
            if (employeeId.HasValue) {
                parameters.Add(new SqlParameter("@EmployeeID", employeeId.Value));
            }
            return await class_db.ExecuteStoredProcAsync("ListEmployees", parameters.ToArray());
        }

        public async static Task<JArray> GetJsonAsync() {
            JArray rArray = new JArray();

            var listEmployee = await class_employee.GetListAsync();
            var departments = await class_department.GetListAsync();
            var designations = await class_designation.GetListAsync();

            var departmentLookup = departments.AsEnumerable().ToDictionary(row => row.GetValue<int>("DepartmentID"), row => row.GetValue<string>("DepartmentName"));
            var designationLookup = designations.AsEnumerable().ToDictionary(row => row.GetValue<int>("DesignationID"), row => row.GetValue<string>("DesignationName"));

            JObject jObjhead = new JObject {
                ["row_type"] = "head",
                ["row_class"] = "",
                ["col1"] = "Employee Id",
                ["col2"] = "First Name",
                ["col3"] = "Last Name",
                ["col4"] = "Email",
                ["col5"] = "Phone Number",
                ["col6"] = "Date Of Birth",
                ["col7"] = "Department",
                ["col8"] = "Designation",
                ["col9"] = "Action",
                ["col9_type"] = "html",
            };
            rArray.Add(jObjhead);

            foreach (DataRow row in listEmployee.Rows) {
                var employeeId = row.GetValue<int>("EmployeeID").ToString();
                int departmentId = row.GetValue<int>("DepartmentID");
                int designationId = row.GetValue<int>("DesignationID");

                var departmentName = departmentLookup.ContainsKey(departmentId) ? departmentLookup[departmentId] : departmentId.ToString();
                var designationName = designationLookup.ContainsKey(designationId) ? designationLookup[designationId] : designationId.ToString();

                string actionbtn = $@"<a class=""btn btn-primary btn-sm mx-1"" href=""/Designation/Details/{employeeId}"">View</a><a class=""btn btn-warning btn-sm mx-1"" href=""/Designation/Edit/{employeeId}"">Edit</a><a class=""btn btn-danger btn-sm mx-1"" href=""/Designation/Delete/{employeeId}"">Delete</a>";

                JObject jObjBody = new JObject {
                    ["row_type"] = "body",
                    ["row_class"] = "",
                    ["col1"] = row.GetValue<int>("EmployeeID").ToString(),
                    ["col2"] = row.GetValue<string>("FirstName"),
                    ["col3"] = row.GetValue<string>("LastName"),
                    ["col4"] = row.GetValue<string>("Email"),
                    ["col5"] = row.GetValue<string>("PhoneNumber"),
                    ["col6"] = row.GetValue<DateTime?>("DateOfBirth")?.ToString("yyyy-MM-dd") ?? "",
                    ["col7"] = departmentName,
                    ["col8"] = designationName,
                    ["col9"] = actionbtn,
                    ["col9_type"] = "html",
                    ["col9_class"] = $@"myactionrow{employeeId}"
                };
                rArray.Add(jObjBody);
            }
            return rArray;
        }

        public async static Task<int> SaveAsync(class_employee employee) {
            string procName = employee.EmployeeID > 0 ? "UpdateEmployee" : "InsertEmployee";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@FirstName", employee.FirstName),
                new SqlParameter("@LastName", employee.LastName),
                new SqlParameter("@Email", employee.Email),
                new SqlParameter("@PhoneNumber", employee.PhoneNumber ?? (object)DBNull.Value),
                new SqlParameter("@DateOfBirth", employee.DateOfBirth ?? (object)DBNull.Value),
                new SqlParameter("@HireDate", employee.HireDate),
                new SqlParameter("@Salary", employee.Salary ?? (object)DBNull.Value),
                new SqlParameter("@DepartmentID", employee.DepartmentID),
                new SqlParameter("@DesignationID", employee.DesignationID),
                new SqlParameter("@Address", employee.Address ?? (object)DBNull.Value),
                new SqlParameter("@City", employee.City ?? (object)DBNull.Value),
                new SqlParameter("@State", employee.State ?? (object)DBNull.Value),
                new SqlParameter("@Country", employee.Country ?? (object)DBNull.Value),
                new SqlParameter("@PostalCode", employee.PostalCode ?? (object)DBNull.Value),
                new SqlParameter("@Status", employee.Status),
            };
            if (employee.EmployeeID > 0) {
                parameters.Add(new SqlParameter("@EmployeeID", employee.EmployeeID));
            }
            return await class_db.ExecuteNonQueryAsync(procName, parameters.ToArray());
        }
        #endregion 

        #region MapFromDataRow
        public static class_employee MapFromDataRow(DataRow row) {
            return new class_employee {
                EmployeeID = row.GetValue<int>("EmployeeID"),
                FirstName = row.GetValue<string>("FirstName"),
                LastName = row.GetValue<string>("LastName"),
                Email = row.GetValue<string>("Email"),
                PhoneNumber = row.GetValue<string>("PhoneNumber"),
                DateOfBirth = row.GetValue<DateTime?>("DateOfBirth"),
                HireDate = row.GetValue<DateTime>("HireDate"),
                Salary = row.GetValue<decimal?>("Salary"),
                DepartmentID = row.GetValue<int>("DepartmentID"),
                DesignationID = row.GetValue<int>("DesignationID"),
                Address = row.GetValue<string>("Address"),
                City = row.GetValue<string>("City"),
                State = row.GetValue<string>("State"),
                Country = row.GetValue<string>("Country"),
                PostalCode = row.GetValue<string>("PostalCode"),
                Status = row.GetValue<string>("Status"),
                CreatedAt = row.GetValue<DateTime>("CreatedAt"),
                UpdatedAt = row.GetValue<DateTime>("UpdatedAt")
            };
        }
        #endregion
    }
}
