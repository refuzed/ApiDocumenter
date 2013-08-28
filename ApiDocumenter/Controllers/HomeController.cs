using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.Runtime.Serialization;

namespace ApiDocumenter.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var result = Activator.CreateInstance(typeof(MockData)).GetType().GetProperties();
            return View(result);
        }

        public ActionResult GetTypeView(string payload)
        {
            var type = Type.GetType(payload);

            if (type.IsGenericType)
            {
                var @interfaces = type.GetInterfaces();

                if (@interfaces.Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
                {
                    return PartialView("_dictionary", type);
                }

                if (@interfaces.Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICollection<>)))
                {
                    return PartialView("_list", type.GetGenericArguments().First());
                }

                return PartialView("_generic", type.GetGenericArguments());
            }

            if (type.IsEnum)
                return PartialView("_enum", type);

            if (type.IsArray)
                return PartialView("_list", type.GetElementType());

            return PartialView("_properties", type.GetProperties());
        }

        [DataContract]
        public class MockData
        {
            [DataMember]
            public string StringTest { get; set; }

            [DataMember]
            public int IntTest { get; set; }

            [DataMember]
            public MockEnum EnumTest { get; set; }

            [DataMember]
            public Dictionary<int, MockData> DictionaryTest { get; set; }

            [DataMember]
            public List<MockData>[] ListArray { get; set; }

            [DataMember]
            public HashSet<MockData> HashSet { get; set; }

            [DataMember]
            public MockData[][] ArrayArray { get; set; }

            [DataMember]
            public List<MockData>[] ListIntTest { get; set; }

            [DataMember]
            public MockData SelfTest { get; set; }
        }

        [DataContract]
        public enum MockEnum 
        {
            [EnumMember]
            Unknown = 1,

            [EnumMember]
            Beyond = 15
        }
    }
}
