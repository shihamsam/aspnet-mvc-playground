using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Data;
using StudentPortal.Web.Models;
using StudentPortal.Web.Models.Entities;

namespace StudentPortal.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public StudentsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(StudentAddViewModel viewModel)
        {
            await _dbContext.Students.AddAsync(new Student
            {
                Name = viewModel.Name,
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                Subscribed = viewModel.Subscribed
            });

            await _dbContext.SaveChangesAsync();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var students = await _dbContext.Students.ToListAsync();

            return View(students);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var student = await _dbContext.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(new StudentEditViewModel
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Phone = student.Phone,
                Subscribed = student.Subscribed
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StudentEditViewModel viewModel)
        {
            var student = await _dbContext.Students.FindAsync(viewModel.Id);

            if (student == null)
            {
                return NotFound();
            }

            student.Name = viewModel.Name;
            student.Email = viewModel.Email;
            student.Phone = viewModel.Phone;
            student.Subscribed = viewModel.Subscribed;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("List", "Students");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Student viewModel)
        {
            var student = await _dbContext.Students.FindAsync(viewModel.Id);

            if (student == null)
            {
                return NotFound();
            }

            _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("List", "Students");
        }

    }
}