using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Api.Controllers;

[Authorize]
public class GroupApiController : BaseApiController
{
    private readonly IGroupService _groupService;
    private readonly ILogger<GroupApiController> _logger;

    public GroupApiController(IGroupService groupService, ILogger<GroupApiController> logger)
    {
        _groupService = groupService;
        _logger = logger;
    }

    // GET api/GroupApi
    [HttpGet]
    public IActionResult GetGroups()
    {
        try
        {
            var groups = _groupService.GetGroups();
            return Ok(groups);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, "Wewnętrzny błąd serwera.");
        }
    }

    // GET api/GroupApi/{id}
    [HttpGet("{id}")]
    public IActionResult GetGroup(int id)
    {
        try
        {
            var group = _groupService.GetGroup(g => g.Id == id);
            if (group == null)
                return NotFound($"Grupa o id={id} nie istnieje.");
            return Ok(group);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, "Wewnętrzny błąd serwera.");
        }
    }

    // POST api/GroupApi
    [HttpPost]
    public IActionResult AddGroup([FromBody] AddOrUpdateGroupVm addGroupVm)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var group = _groupService.AddOrUpdateGroup(addGroupVm);
            return CreatedAtAction(nameof(GetGroup), new { id = group.Id }, group);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, "Wewnętrzny błąd serwera.");
        }
    }

    // PUT api/GroupApi
    [HttpPut]
    public IActionResult UpdateGroup([FromBody] AddOrUpdateGroupVm updateGroupVm)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var group = _groupService.AddOrUpdateGroup(updateGroupVm);
            return Ok(group);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, "Wewnętrzny błąd serwera.");
        }
    }

    // POST api/GroupApi/addStudent
    [HttpPost("addStudent")]
    public IActionResult AddStudentToGroup([FromBody] AttachDetachStudentToGroupVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var student = _groupService.AttachStudentToGroup(vm);
            return Ok(student);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, "Wewnętrzny błąd serwera.");
        }
    }

    // DELETE api/GroupApi/removeStudent
    [HttpDelete("removeStudent")]
    public IActionResult RemoveStudentFromGroup([FromBody] AttachDetachStudentToGroupVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var student = _groupService.DetachStudentFromGroup(vm);
            return Ok(student);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, "Wewnętrzny błąd serwera.");
        }
    }
}