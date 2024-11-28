using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using System.Data;
using webdemo.Models;
namespace webdemo.Controllers {
    public class EmployeeController : Controller {
        #region Index
        public async Task<IActionResult> Index() {
            var listEmployee = await class_employee.GetListAsync();
            return View(listEmployee);
        }

        public async Task<IActionResult> IndexData() {
            JArray rArray = await class_employee.GetJsonAsync();
            return Content(rArray.ToString(), "application/json");
        }
        #endregion

        #region Export

        public async Task<IActionResult> ExportData(string exporttype) {
            JArray rArray = await class_employee.GetJsonAsync();
            string cdate = DateTime.Now.ToString("yyyyMMdd");
            string myfilename = "employee_" + cdate + "." + exporttype;

            byte[] fileBytes = null;

            if (exporttype == "pdf") {
                string myhtml = class_export.CreateTable(rArray, "employeetable");
                fileBytes = class_export.generatepdf(myhtml); 
            } else if (exporttype == "xlsx") {
                fileBytes = class_export.generateexcel(rArray); 
            }

            return File(fileBytes, "application/octet-stream", myfilename);
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
