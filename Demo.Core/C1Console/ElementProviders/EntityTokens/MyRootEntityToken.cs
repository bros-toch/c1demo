using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Composite.C1Console.Security;
using Composite.C1Console.Security.SecurityAncestorProviders;

namespace Demo.Core.C1Console.ElementProviders.EntityTokens
{
    [SecurityAncestorProvider(typeof(NoAncestorSecurityAncestorProvider))]
    public sealed class MyRootEntityToken : EntityToken
    {
        public override string Type
        {
            get { return ""; }
        }
        public override string Source
        {
            get { return ""; }
        }
        public override string Id
        {
            get { return "MyRootEntityToken"; }
        }
        public override string Serialize()
        {
            return DoSerialize();
        }
        public static EntityToken Deserialize(string serializedEntityToken)
        {
            return new MyRootEntityToken();
        }
    }


    [SecurityAncestorProvider(typeof(NoAncestorSecurityAncestorProvider))]
    public sealed class UsersRootEntityToken : EntityToken
    {
        public override string Type
        {
            get { return ""; }
        }
        public override string Source
        {
            get { return ""; }
        }
        public override string Id
        {
            get { return "UsersRootToken"; }
        }
        public override string Serialize()
        {
            return DoSerialize();
        }
        public static EntityToken Deserialize(string serializedEntityToken)
        {
            return new MyRootEntityToken();
        }
    }
}
