using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Api.Controllers;

[Authorize]
public class GradeApiController : BaseApiController
{
    private readonly IGradeService _gradeService;
    private readonly ILogger<GradeApiController> _logger;

    public GradeApiController(IGradeService gradeService, ILogger<GradeApiController> logger)
    {
        _gradeService = gradeService;
        _logger = logger;
    }

    // POST api/GradeApi
    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public IActionResult AddGrade([FromBody] AddGradeToStudentVm addGradeVm)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var grade = _gradeService.AddGradeToStudent(addGradeVm);
            return Ok(grade);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, "Wewnętrzny błąd serwera.");
        }
    }

    // GET api/GradeApi/student/{studentId}/{getterId}
    [HttpGet("student/{studentId}/{getterId}")]
    [Authorize(Roles = "Teacher,Student,Parent")]
    public IActionResult GetGradesForStudent(int studentId, int getterId)
    {
        try
        {
            var grades = _gradeService.GetGradesReportForStudent(new GetGradesReportVm
            {
                StudentId = studentId,
                GetterUserId = getterId
            });
            return Ok(grades);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, "Wewnętrzny błąd serwera.");
        }
    }
}