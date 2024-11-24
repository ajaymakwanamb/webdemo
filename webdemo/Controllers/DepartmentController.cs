using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using webdemo.Models;

namespace webdemo.Controllers {
    public class DepartmentController : Controller {

        #region Index
        public async Task<IActionResult> Index() {
            var listDepartment = await class_department.GetListAsync();
            return View(listDepartment);
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int id) {
            var objdepartment = await class_department.GetListAsync(id);
            if (objdepartment == null || objdepartment.Rows.Count == 0) {
                return NotFound();
            }
            var department = class_department.MapFromDataRow(objdepartment.Rows[0]);
            return View(department);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create() {
            class_department objdepartment = new class_department();
            return View(objdepartment);
        }

        [HttpPost]
        public async Task<IActionResult> Create(class_department objdepartment) {
            objdepartment.Validate(ModelState);
            if (!ModelState.IsValid) {
                return View(objdepartment);
            }

            var rowsAffected = await class_department.SaveAsync(objdepartment);
            if (rowsAffected > 0) {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Failed to create department.");
            return View();
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id) {
            var objdepartment = await class_department.GetListAsync(id);
            if (objdepartment == null || objdepartment.Rows.Count == 0) {
                return NotFound();
            }
            var department = class_department.MapFromDataRow(objdepartment.Rows[0]);
            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int DepartmentId, class_department objdepartment) {
            objdepartment.Validate(ModelState);
            if (!ModelState.IsValid) {
                return View(objdepartment);
            }
            var rowsAffected = await class_department.SaveAsync(objdepartment);
            if (rowsAffected > 0) {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Failed to update department.");
            return View(objdepartment);
        }
        #endregion

        #region LiveSearch
        [HttpGet]
        public async Task<IActionResult> LiveSearch(string keyword, int pageNumber = 1, int pageSize = 10) {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var listDepartment = await class_department.GetListAsync();
            var departments = listDepartment.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(keyword)) {
                departments = departments.Where(row => row.GetValue<string>("DepartmentName").Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            int totalCount = departments.Count();
            var paginatedDepartments = departments.Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .Select(row => new {
                    DepartmentID = row.Field<int>("DepartmentID"),
                    DepartmentName = row.Field<string>("DepartmentName")
                })
                .ToList();

            return Json(new {
                TotalCount = totalCount, 
                PageNumber = pageNumber, 
                PageSize = pageSize,    
                Data = paginatedDepartments 
            });
        }
        #endregion

    }
}
