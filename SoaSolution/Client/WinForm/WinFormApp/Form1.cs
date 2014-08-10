using Library.Client.Proxy;
using Library.Criterias.SoaTest;
using Library.Models.Common;
using Library.Models.SoaTest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var timeOld = DateTime.Now;
            var resultMsg = string.Empty;
            var criteria = new CriteriaTestuser();
            criteria.UName = null;
            criteria.Count = 100000;

            OperateClass ope = new OperateClass();
            ope.Assembly = "Library.Facade.SoaTest";
            ope.Class = string.Format("{0}.{1}", ope.Assembly, "FacadeTestUser");
            ope.Method = "QueryTestuserList";
            ope.Parameters = new object[] { resultMsg, criteria };

            ClientProxy proxy = new ClientProxy();
            var resultOpc = proxy.Operate(ope);
            var list = (IList<ModelTestuser>)resultOpc.ResultObj;

            var timeNew = DateTime.Now;
            var timeSpan = timeNew - timeOld;
            var msg = string.Format("返回数据:{0} 条,耗时 {1} 秒!", list.Count,timeSpan.TotalSeconds);
            MessageBox.Show(msg);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var timeOld = DateTime.Now;
            var resultMsg = string.Empty;
            var criteria = new CriteriaTestuser();
            criteria.UName = null;
            criteria.Count = 100000;

            OperateClass ope = new OperateClass();  
            ope.Method = "QueryTestuserList";
            ope.Parameters = new object[] { resultMsg, criteria };

            ClientProxy proxy = new ClientProxy();
            var resultOpc = proxy.FacadeTestUserOperate(ope);
            var list = (IList<ModelTestuser>)resultOpc.ResultObj;

            var timeNew = DateTime.Now;
            var timeSpan = timeNew - timeOld;
            var msg = string.Format("返回数据:{0} 条,耗时 {1} 秒!", list.Count, timeSpan.TotalSeconds);
            MessageBox.Show(msg);
        }
    }
}
