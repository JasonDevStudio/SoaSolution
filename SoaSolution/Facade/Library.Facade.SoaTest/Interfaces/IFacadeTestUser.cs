using Library.Criterias.SoaTest;
using Library.Models.SoaTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Facade.SoaTest
{
    public interface IFacadeTestUser
    {
        /// <summary>
        /// 数据查询
        /// </summary>
        /// <param name="resultMsg">执行结果</param> 
        /// <param name="criteria">查询条件</param>
        /// <returns>泛型</returns>
        IList<ModelTestuser> QueryTestuserList(out string resultMsg, object criteria);
    }
}
