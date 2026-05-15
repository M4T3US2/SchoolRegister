using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Api.Controllers;

[Authorize]
public class SubjectApiController : BaseApiController
{
    private readonly ISubjectService _subjectService;
    private readonly ILogger<SubjectApiController> _logger;

    public SubjectApiController(ISubjectService subjectService, ILogger<SubjectApiController> logger)
    {
        _subjectService = subjectService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetSubjects()
    {
        try
        {
            var subjects = _subjectService.GetSubjects();
            return Ok(subjects);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, "Wewnętrzny błąd serwera.");
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetSubject(int id)
    {
        try
        {
            var subject = _subjectService.GetSubject(s => s.Id == id);
            if (subject == null)
                return NotFound($"Przedmiot o id={id} nie istnieje.");
            return Ok(subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, "Wewnętrzny błąd serwera.");
        }
    }

    [HttpPost]
    public IActionResult AddSubject([FromBody] AddOrUpdateSubjectVm addSubjectVm)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var subject = _subjectService.AddOrUpdateSubject(addSubjectVm);
            return CreatedAtAction(nameof(GetSubject), new { id = subject.Id }, subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, "Wewnętrzny błąd serwera.");
        }
    }

    [HttpPut]
    public IActionResult UpdateSubject([FromBody] AddOrUpdateSubjectVm updateSubjectVm)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!updateSubjectVm.Id.HasValue || updateSubjectVm.Id == 0)
                return BadRequest("Id przedmiotu jest wymagane do aktualizacji.");

            var subject = _subjectService.AddOrUpdateSubject(updateSubjectVm);
            return Ok(subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, "Wewnętrzny błąd serwera.");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteSubject(int id)
    {
        try
        {
            var result = _subjectService.RemoveSubject(s => s.Id == id);
            if (!result)
                return NotFound($"Przedmiot o id={id} nie istnieje.");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, "Wewnętrzny błąd serwera.");
        }
    }
}