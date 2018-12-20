using Microsoft.VisualStudio.TestTools.UnitTesting;
using qtree.core.common;
using qtree.core.website;
using System;
using System.ComponentModel;

namespace qtree.core.msunittest
{
    [TestClass]
    public class UnitTest_PropertyDescriptor
    {
        [TestMethod]
        public void UnitTest_PropertyDescriptor_test()
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
