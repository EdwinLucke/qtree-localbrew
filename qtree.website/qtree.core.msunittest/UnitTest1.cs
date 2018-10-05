using Microsoft.VisualStudio.TestTools.UnitTesting;
using qtree.core.website;
using System;
using System.ComponentModel;

namespace qtree.core.msunittest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var dataObj = ApiResponse.DataResponse("test");
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(dataObj))
            {
                Console.WriteLine($"{prop.Name}={prop.GetValue(dataObj)} - {prop.Attributes.ToString()}");
            }

            var theT = dataObj.GetType();
            //CustomDataContractResolver.Instance < dataObj.GetType() >.CreateProperty()
            //CustomDataContractResolver<dataObj>.Instance.CreateProperty(typeof(dataObj))
            Console.WriteLine(ApiResponse.DataResponse("test"));
        }
    }
}
