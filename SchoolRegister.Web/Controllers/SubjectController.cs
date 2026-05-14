using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using AutoMapper;
using SchoolRegister.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using SchoolRegister.ViewModels.VM;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace SchoolRegister.Web.Controllers;

[Authorize(Roles = "Admin")]
public class SubjectController : BaseController
{
    private readonly ISubjectService _subjectService;
    private readonly ITeacherService _teacherService;
    private readonly IGroupService _groupService;

    public SubjectController(ILogger<SubjectController> logger, IMapper mapper, 
                             IStringLocalizer<BaseController> localizer, 
                             ISubjectService subjectService, 
                             ITeacherService teacherService,
                             IGroupService groupService) 
                             : base(logger, mapper, localizer)
    {
        _subjectService = subjectService;
        _teacherService = teacherService;
        _groupService = groupService;
    }

    public IActionResult Index()
    {
        var subjects = _subjectService.GetSubjects(s => s.Id > 0);
        return View(subjects);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var subject = _subjectService.GetSubject(s => s.Id == id);
        if (subject == null) return NotFound();
        return View(subject);
    }

    [HttpGet]
    public IActionResult AttachSubjectToGroup(int subjectId)
    {
        var subject = _subjectService.GetSubject(s => s.Id == subjectId);
        if (subject == null) return NotFound();

        var groups = _groupService.GetGroups(g => g.Id > 0);
        ViewBag.GroupsSelectList = new SelectList(groups, "Id", "Name");
        ViewBag.SubjectName = subject.Name;

        return View(new AttachDetachSubjectGroupVm { SubjectId = subjectId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AttachSubjectToGroup(AttachDetachSubjectGroupVm model)
    {
        if (ModelState.IsValid)
        {
            _groupService.AttachSubjectToGroup(model);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult DetachSubjectFromGroup(int subjectId)
    {
        var subject = _subjectService.GetSubject(s => s.Id == subjectId);
        if (subject == null) return NotFound();

        var groups = subject.Groups; 
        ViewBag.GroupsSelectList = new SelectList(groups, "Id", "Name");
        ViewBag.SubjectName = subject.Name;

        return View(new AttachDetachSubjectGroupVm { SubjectId = subjectId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DetachSubjectFromGroup(AttachDetachSubjectGroupVm model)
    {
        _groupService.DetachSubjectFromGroup(model);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult AddOrEditSubject(int? id)
    {
        var teachers = _teacherService.GetTeachers(t => t.Id > 0);
        ViewBag.TeachersSelectList = new SelectList(teachers, "Id", "UserName");

        if (id == null)
        {
            ViewBag.ActionType = "Add";
            return View(new AddOrUpdateSubjectVm());
        }
        else
        {
            ViewBag.ActionType = "Edit";
            var subject = _subjectService.GetSubject(s => s.Id == id);
            if (subject == null) return NotFound();
            var model = _mapper.Map<AddOrUpdateSubjectVm>(subject);
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddOrEditSubject(AddOrUpdateSubjectVm model)
    {
        if (ModelState.IsValid)
        {
            _subjectService.AddOrUpdateSubject(model);
            return RedirectToAction(nameof(Index));
        }

        var teachers = _teacherService.GetTeachers(t => t.Id > 0);
        ViewBag.TeachersSelectList = new SelectList(teachers, "Id", "UserName");
        ViewBag.ActionType = model.Id == null ? "Add" : "Edit";
        
        return View(model);
    }
}