using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using webdemo.Models;

namespace webdemo.Controllers {
    public class DesignationController : Controller {
        #region Index
        public async Task<IActionResult> Index() {
            var listDesignation = await class_designation.GetListAsync();
            return View(listDesignation);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(class_designation objdesignation) {
            objdesignation.Validate(ModelState);
            if (!ModelState.IsValid) {
                return View(objdesignation);
            }
           
            var rowsAffected = await class_designation.SaveAsync(objdesignation);
            if (rowsAffected > 0) {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Failed to create designation.");
            return View();
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id) {
            var designation = await class_designation.GetListAsync(id);
            if (designation == null) {
                return NotFound();
            }
            return View(designation);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, class_designation objdesignation) {
            objdesignation.Validate(ModelState);
            if (!ModelState.IsValid) {
                return View(objdesignation);
            }

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@DesignationId", objdesignation.DesignationId),
                new SqlParameter("@DesignationName", objdesignation.DesignationName),
                new SqlParameter("@Description", objdesignation.Description)
            };

            var rowsAffected = await class_designation.SaveAsync(objdesignation);
            if (rowsAffected > 0) {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Failed to update designation.");
            return View(objdesignation);
        }
        #endregion
    }
}
