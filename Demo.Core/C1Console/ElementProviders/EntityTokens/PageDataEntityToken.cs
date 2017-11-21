using System;
using System.Linq;
using Composite.C1Console.Security;
using Composite.C1Console.Security.SecurityAncestorProviders;
using Composite.Core.Types;
using Composite.Data;
using Composite.Data.Types;

namespace Demo.Core.C1Console.ElementProviders.EntityTokens
{
    [SecurityAncestorProvider(typeof(NoAncestorSecurityAncestorProvider))]
    public class PageDataEntityToken : EntityToken
    {
        private readonly string _type;
        public override string Type
        {
            get { return _type; }
        }

        private readonly string _source;
        public override string Source
        {
            get { return _source; }
        }

        private readonly string _id;
        public override string Id
        {
            get { return _id; }
        }

        public IPage Data { get; set; }
        public PageDataEntityToken(IPage data)
        {
            Data = data;


            _source = data.Id.ToString();
            _type = TypeManager.SerializeType(data.DataSourceId.InterfaceType);
            _id = data.Id.ToString();
        }
        public override string Serialize()
        {
            return DoSerialize();
        }

        public static EntityToken Deserialize(string serializedEntityToken)
        {
            string type;
            string source;
            string id;

            DoDeserialize(serializedEntityToken, out type, out source, out id);

            var page = PageManager.GetPageById(new Guid(source));

            return new PageDataEntityToken(page);
        }
    }
}