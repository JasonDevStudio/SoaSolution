using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.Facade.SoaTest.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Facade.SoaTest.Interfaces;
using Library.Criterias.SoaTest;
namespace Library.Facade.SoaTest.Classes.Tests
{
    [TestClass()]
    public class FacadeTestUserTests
    {
        [TestMethod()]
        public void QueryTestuserListTest()
        {
            IFacadeTestUser facade = new Factories().InstanceTestUser();
            var resultMsg =string.Empty;
            var criteria = new CriteriaTestuser();
            var list = facade.QueryTestuserList(out resultMsg, criteria);
            if (list.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }
    }
}
