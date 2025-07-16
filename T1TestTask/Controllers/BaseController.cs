using Microsoft.AspNetCore.Mvc;
using T1TestTask.Filters;

namespace T1TestTask.Controllers
{
    [TypeFilter<ApiExceptionFilter>]
    public class BaseController : ControllerBase
    {
    }
}
