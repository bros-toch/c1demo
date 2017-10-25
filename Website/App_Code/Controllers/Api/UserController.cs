using System.Collections.Generic;
using System.Web.Http;
using Composite.Core.Extensions;
using Composite.Data;
using Composite.Data.Types;

namespace Controllers.Api
{
    /// <summary>
    /// Summary description for UserController
    /// </summary>
    public class UserController : ApiController
    {
        public UserController()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public IEnumerable<IUser> Get()
        {
            using (var c = new DataConnection())
            {
                return c.Get<IUser>();
            }
        }
    }
}