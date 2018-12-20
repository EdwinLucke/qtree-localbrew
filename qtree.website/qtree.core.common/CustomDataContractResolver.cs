using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;

namespace qtree.core.common
{
    public class CustomDataContractResolver : DefaultContractResolver
    {
        public static readonly CustomDataContractResolver Instance = new CustomDataContractResolver();

        public CustomDataContractResolver()
        {

        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var Tobject = ApiResponse.Empty<string>(HttpStatusCode.Accepted, "test");
            var property = base.CreateProperty(member, memberSerialization);
            if (property.DeclaringType == Tobject.GetType())
            {
                if (property.PropertyName.Equals("data", StringComparison.OrdinalIgnoreCase))
                {
                    property.PropertyName = Tobject.GetType().Name.ToLowerInvariant();
                }
            }
            return property;
        }
    }
}
