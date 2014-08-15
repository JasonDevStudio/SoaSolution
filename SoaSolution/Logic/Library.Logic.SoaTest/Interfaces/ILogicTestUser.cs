using Library.Criterias.SoaTest;
using Library.Models.SoaTest;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Library.Logic.SoaTest.Interfaces
{
    public interface ILogicTestUser
    {
        /// <summary>
        /// 数据查询
        /// </summary>
        /// <param name="resultMsg">执行结果</param> 
        /// <param name="criteria">查询条件</param>
        /// <returns>泛型</returns>
        IList<ModelTestuser> QueryTestuserList(out string resultMsg, CriteriaTestuser criteria);

        DataSet QueryTable(out string resultMsg, int count = 10);
         
    }
}
