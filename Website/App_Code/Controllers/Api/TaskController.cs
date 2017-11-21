using System.Web.Http;

namespace Controllers.Api
{
    /// <summary>
    /// Summary description for UserController
    /// </summary>
    [RoutePrefix("task")]
    public class TaskController : ApiController
    {
        public TaskController()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        [HttpGet, Route("add")]
        public dynamic Add()
        {
            return new
            {
                success = true
            };
        }
    }
}