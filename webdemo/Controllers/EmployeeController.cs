using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using System.Data;
using webdemo.Models;
namespace webdemo.Controllers {
    public class EmployeeController : Controller {
        #region Index
        public async Task<IActionResult> Index() {
            var listDepartment = await class_employee.GetListAsync();
            return View(listDepartment);
        }

        public async Task<IActionResult> IndexData() {
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
                ["col9"] = "Action"
            };
            rArray.Add(jObjhead);

            // Add body rows
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
            return Content(rArray.ToString(), "application/json");
        }
        #endregion

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create() {
            class_employee objemployee = new class_employee();
            AddDepartment(0);
            AddDesignation(0);
            return View(objemployee);
        }

        [HttpPost]
        public async Task<IActionResult> Create(class_employee objemployee) {
            objemployee.Validate(ModelState);
            if (!ModelState.IsValid) {
                AddDepartment(objemployee.DepartmentID);
                AddDesignation(objemployee.DesignationID);
                return View(objemployee);
            }

            var rowsAffected = await class_employee.SaveAsync(objemployee);
            if (rowsAffected > 0) {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Failed to create employee.");
            return View();
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id) {
            var myemployee = await class_employee.GetListAsync(id);
            if (myemployee == null || myemployee.Rows.Count == 0) {
                return NotFound();
            }
           
            class_employee objemployee = class_employee.MapFromDataRow(myemployee.Rows[0]);
            AddDepartment(objemployee.DepartmentID);
            AddDesignation(objemployee.DesignationID);
            return View(objemployee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int DepartmentId, class_employee objemployee) {
            objemployee.Validate(ModelState);
            if (!ModelState.IsValid) {
                AddDepartment(objemployee.DepartmentID);
                AddDesignation(objemployee.DesignationID);
                return View(objemployee);
            }
            var rowsAffected = await class_employee.SaveAsync(objemployee);
            if (rowsAffected > 0) {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Failed to update employee.");
            return View(objemployee);
        }
        #endregion

        #region Dropdown

        public async void AddDepartment(int DepartmentId) {
            var listDepartments = await class_department.GetList();
            if (listDepartments == null || !listDepartments.Any()) {
                ModelState.AddModelError("", "No departments found.");
            }
            ViewBag.Departments = new SelectList(listDepartments, "id", "name", DepartmentId);
        }
        public async void AddDesignation(int DesignationId) {
            var listDesignations = await class_designation.GetList();
            if (listDesignations == null || !listDesignations.Any()) {
                ModelState.AddModelError("", "No designation found.");
            }
            ViewBag.Designations = new SelectList(listDesignations, "id", "name", DesignationId);
        }
        #endregion

    }
}
