using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using AutoMapper;

namespace SchoolRegister.Web.Controllers;

public abstract class BaseController : Controller
{
    protected readonly ILogger _logger;
    protected readonly IMapper _mapper;
    protected readonly IStringLocalizer<BaseController> _localizer;

    public BaseController(ILogger logger, IMapper mapper, IStringLocalizer<BaseController> localizer)
    {
        _logger = logger;
        _mapper = mapper;
        _localizer = localizer;
    }
}