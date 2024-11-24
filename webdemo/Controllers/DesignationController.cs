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

        #region Details
        public async Task<IActionResult> Details(int id) {
            var objdesignation = await class_designation.GetListAsync(id);
            if (objdesignation == null || objdesignation.Rows.Count == 0) {
                return NotFound();
            }
            var designation = class_designation.MapFromDataRow(objdesignation.Rows[0]);
            return View(designation);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create() {
            class_designation objdesignation = new class_designation();
            return View(objdesignation);
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
            var objdesignation = await class_designation.GetListAsync(id);
            if (objdesignation == null || objdesignation.Rows.Count == 0) {
                return NotFound();
            }
            var designation = class_designation.MapFromDataRow(objdesignation.Rows[0]);
            return View(designation);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int DesignationId, class_designation objdesignation) {
            objdesignation.Validate(ModelState);
            if (!ModelState.IsValid) {
                return View(objdesignation);
            }
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
