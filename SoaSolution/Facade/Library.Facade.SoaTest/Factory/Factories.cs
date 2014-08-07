using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using Library.StringItemDict;
using Library.Facade.SoaTest.Interfaces;
using Library.Facade.SoaTest.Classes;  

namespace Library.Facade.SoaTest
{
    /// <summary>
    /// 工厂实例化
    /// </summary>
    public class Factories
    {
        /// <summary>
        /// 实例化
        /// </summary>
        /// <returns>IFacadeTestUser</returns>
        public IFacadeTestUser InstanceTestUser(string factoryModel = null)
        { 
            switch (factoryModel)
            { 
                default:
                    return new FacadeTestUser();
            }
        }
         
    }
}
