using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Facade.SoaTest;
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
            var criteria = new CriteriaTestuser() { Count = 100 };
            var list = facade.QueryTestuserList(out resultMsg, criteria);
            var data = facade.QueryTable(out resultMsg, count: 20);

            if (list.Count > 0 && data.Tables[0].Rows.Count > 0)
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
