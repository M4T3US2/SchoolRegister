using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.Web.Controllers;

[Authorize]
public class GradeController : BaseController
{
    private readonly IGradeService _gradeService;
    private readonly ISubjectService _subjectService;
    private readonly UserManager<User> _userManager;

    public GradeController(ILogger<GradeController> logger, IMapper mapper,
                           IStringLocalizer<BaseController> localizer,
                           IGradeService gradeService,
                           ISubjectService subjectService,
                           UserManager<User> userManager)
                           : base(logger, mapper, localizer)
    {
        _gradeService = gradeService;
        _subjectService = subjectService;
        _userManager = userManager;
    }

    [Authorize(Roles = "Student,Parent,Admin,Teacher")]
    public IActionResult Index(int? studentId)
    {
        
        var currentUserIdStr = _userManager.GetUserId(User);
        var currentUserId = int.Parse(currentUserIdStr ?? "0");

        
        if (!studentId.HasValue)
            studentId = currentUserId;

        var reportRequest = new GetGradesReportVm
        {
            StudentId = studentId ?? 0,
            
            GetterUserId = currentUserId
        };

        var grades = _gradeService.GetGradesReportForStudent(reportRequest);

        // POPRAWKA #2: zabezpieczenie przed null - widok dostaje pustą listę zamiast crash
        return View(grades ?? Enumerable.Empty<GradeVm>());
    }

    [Authorize(Roles = "Teacher,Admin")]
    [HttpGet]
    public IActionResult AddGrade()
    {
        
        var students = _userManager.Users.OfType<Student>().ToList();
        var subjects = _subjectService.GetSubjects(s => s.Id > 0).ToList();

        ViewBag.StudentId = new SelectList(students, "Id", "UserName");
        ViewBag.SubjectId = new SelectList(subjects, "Id", "Name");

        var gradeProperty = typeof(AddGradeToStudentVm).GetProperty("GradeValue");
        ViewBag.Grades = new SelectList(Enum.GetValues(gradeProperty!.PropertyType));

        return View(new AddGradeToStudentVm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Teacher,Admin")]
    public IActionResult AddGrade(AddGradeToStudentVm model)
    {
       
        var currentUserIdStr = _userManager.GetUserId(User);
        if (currentUserIdStr != null)
            model.TeacherId = int.Parse(currentUserIdStr);

        if (ModelState.IsValid)
        {
            _gradeService.AddGradeToStudent(model);
            return RedirectToAction(nameof(Index), new { studentId = model.StudentId });
        }

        
        var students = _userManager.Users.OfType<Student>().ToList();
        var subjects = _subjectService.GetSubjects(s => s.Id > 0).ToList();
        ViewBag.StudentId = new SelectList(students, "Id", "UserName");
        ViewBag.SubjectId = new SelectList(subjects, "Id", "Name");
        var gradeProperty = typeof(AddGradeToStudentVm).GetProperty("GradeValue");
        ViewBag.Grades = new SelectList(Enum.GetValues(gradeProperty!.PropertyType));

        return View(model);
    }
}