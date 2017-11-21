using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Composite.C1Console.Actions;
using Composite.C1Console.Security;

namespace Demo.Core.C1Console.ElementProviders.Actions
{
    [ActionExecutor(typeof(MyUrlActionExecutor))]
    public sealed class MyUrlActionToken : ActionToken
    {
        private static readonly PermissionType[] _permissionTypes = new PermissionType[] { PermissionType.Administrate };
        public override IEnumerable<PermissionType> PermissionTypes => _permissionTypes;

        public override string Serialize()
        {
            return "MyUrlAction";
        }
        public static ActionToken Deserialize(string serializedData)
        {
            return new MyUrlActionToken();
        }
    }
}
