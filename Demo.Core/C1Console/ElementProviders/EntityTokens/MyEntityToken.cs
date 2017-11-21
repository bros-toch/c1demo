using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Composite.C1Console.Security;
using Composite.Data;

namespace Demo.Core.C1Console.ElementProviders.EntityTokens
{
    [SecurityAncestorProvider(typeof(MySecurityAncestorProvider))]
    public sealed class MyEntityToken : EntityToken
    {
        public MyEntityToken(string publicationStatus)
        {
            this.PublicationStatus = publicationStatus;
        }
        public string PublicationStatus
        {
            get;
            set;
        }
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
            get { return this.PublicationStatus; }
        }
        public override string Serialize()
        {
            return DoSerialize();
        }
        public static EntityToken Deserialize(string serializedEntityToken)
        {
            string type, source, id;
            DoDeserialize(serializedEntityToken, out type, out source, out id);
            return new MyEntityToken(id);
        }
    }
}
