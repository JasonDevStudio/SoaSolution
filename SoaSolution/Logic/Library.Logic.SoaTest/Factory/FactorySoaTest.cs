using Library.Kernel.DataBaseHelper;
using Library.Logic.SoaTest.Classes;
using Library.Logic.SoaTest.Interfaces;
using Library.StringItemDict;

namespace Library.Logic.SoaTest.Factory
{
    public class FactorySoaTest
    {
        /// <summary>
        /// 数据字典Logic 实例化
        /// </summary>
        /// <returns></returns>
        public ILogicTestUser InstanceSoaTest(string factoryModel = null)
        {
            factoryModel = string.IsNullOrWhiteSpace(factoryModel) ? DBHelper.DBNAME : factoryModel;
            switch (factoryModel)
            {
                case BaseDict.OracleData:
                    return null;
                case BaseDict.SqlServerData:
                default:
                    return new LogicTestUser();
            }
        }
    }
}
