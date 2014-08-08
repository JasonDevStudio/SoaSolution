using Library.Criterias.SoaTest;  
using Library.Models.SoaTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.Logic.SoaTest.Interfaces;
using Library.Logic.SoaTest.Factory;

namespace Library.Facade.SoaTest
{
    public class FacadeTestUser : IFacadeTestUser
    {
        /// <summary>
        /// 数据查询
        /// </summary>
        /// <param name="resultMsg">执行结果</param> 
        /// <param name="criteria">查询条件</param>
        /// <returns>泛型</returns>
        public IList<ModelTestuser> QueryTestuserList(out string resultMsg, object criteria)
        { 
            ILogicTestUser logic = new FactorySoaTest().InstanceSoaTest();
            return logic.QueryTestuserList(out resultMsg, (CriteriaTestuser)criteria);
        }
    }
}
