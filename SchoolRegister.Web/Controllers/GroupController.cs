using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using AutoMapper;
using SchoolRegister.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using SchoolRegister.ViewModels.VM;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace SchoolRegister.Web.Controllers;

[Authorize(Roles = "Admin")]
public class GroupController : BaseController
{
    private readonly IGroupService _groupService;
    private readonly IStudentService _studentService;

    public GroupController(ILogger<GroupController> logger, IMapper mapper,
                           IStringLocalizer<BaseController> localizer,
                           IGroupService groupService,
                           IStudentService studentService)
                           : base(logger, mapper, localizer)
    {
        _groupService = groupService;
        _studentService = studentService;
    }

    public IActionResult Index()
    {
        var groups = _groupService.GetGroups(g => g.Id > 0);
        return View(groups);
    }

    public IActionResult Details(int id)
    {
        var group = _groupService.GetGroup(g => g.Id == id);
        if (group == null) return NotFound();

        // Pobieramy studentów którzy NIE są jeszcze w żadnej grupie lub są w tej grupie
        var allStudents = _studentService.GetStudents(s => s.Id > 0);
        ViewBag.StudentsSelectList = new SelectList(allStudents, "Id", "UserName");

        return View(group);
    }

    [HttpGet]
    public IActionResult AddOrEditGroup(int? id)
    {
        if (id == null)
        {
            ViewBag.ActionType = "Add";
            return View(new AddOrUpdateGroupVm());
        }
        else
        {
            ViewBag.ActionType = "Edit";
            var group = _groupService.GetGroup(g => g.Id == id);
            if (group == null) return NotFound();
            var model = _mapper.Map<AddOrUpdateGroupVm>(group);
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddOrEditGroup(AddOrUpdateGroupVm model)
    {
        if (ModelState.IsValid)
        {
            _groupService.AddOrUpdateGroup(model);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.ActionType = model.Id > 0 ? "Edit" : "Add";
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddStudentToGroup(int studentId, int groupId)
    {
        var model = new AttachDetachStudentToGroupVm
        {
            StudentId = studentId,
            GroupId = groupId
        };
        _groupService.AttachStudentToGroup(model);
        return RedirectToAction(nameof(Details), new { id = groupId });
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveStudentFromGroup(int studentId, int groupId)
    {
        var model = new AttachDetachStudentToGroupVm
        {
            StudentId = studentId,
            GroupId = groupId
        };
        _groupService.DetachStudentFromGroup(model);
        return RedirectToAction(nameof(Details), new { id = groupId });
    }
}
