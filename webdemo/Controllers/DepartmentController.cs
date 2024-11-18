using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using webdemo.Models;

namespace webdemo.Controllers {
    public class DepartmentController : Controller {

        public async Task<IActionResult> Index() {
            var listDepartment = await class_department.GetListAsync();
            return View(listDepartment);
        }

        [HttpGet]
        public IActionResult Create() {
            return View();
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id) {
            var department = await class_department.GetListAsync(id);
            if (department == null) {
                return NotFound();
            }
            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, class_department objdepartment) {
            objdepartment.Validate(ModelState);
            if (!ModelState.IsValid) {
                return View(objdepartment);
            }

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@DepartmentId", objdepartment.DepartmentId),
                new SqlParameter("@DepartmentName", objdepartment.DepartmentName),
                new SqlParameter("@Description", objdepartment.Description)
            };

            var rowsAffected = await class_department.SaveAsync(objdepartment);
            if (rowsAffected > 0) {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Failed to update department.");
            return View(objdepartment);
        }
    }
}
