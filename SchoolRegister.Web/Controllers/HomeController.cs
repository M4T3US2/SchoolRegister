using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.Web.Controllers;
using AutoMapper;
using Microsoft.Extensions.Localization;

namespace SchoolRegister.Web.Controllers;

public class HomeController : BaseController
{
    private readonly ISubjectService _subjectService;

    // Wstrzykujemy serwis przedmiotów do konstruktora
    public HomeController(ILogger<HomeController> logger, IMapper mapper, 
                          IStringLocalizer<BaseController> localizer, ISubjectService subjectService) 
                          : base(logger, mapper, localizer)
    {
        _subjectService = subjectService;
    }

    public IActionResult Index()
    {
        // Pobieramy listę przedmiotów, żeby Model nie był nullem!
        var subjects = _subjectService.GetSubjects(s => s.Id > 0);
        return View(subjects);
    }
}