using Microsoft.AspNetCore.Mvc;
using webdemo.Models;

namespace webdemo.Controllers {
    public class EmployeeController : Controller {
        public async Task<IActionResult> Index() {
            var listEmployee = await class_employee.GetListAsync();
            return View(listEmployee); 
        }
    }
}
