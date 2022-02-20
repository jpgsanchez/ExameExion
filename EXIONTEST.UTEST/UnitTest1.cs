using NUnit.Framework;

using System.Configuration;

namespace EXIONTEST.UTEST
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var a = ConfigurationManager.ConnectionStrings["conn"];
            
            Assert.Pass();
        }
    }
}