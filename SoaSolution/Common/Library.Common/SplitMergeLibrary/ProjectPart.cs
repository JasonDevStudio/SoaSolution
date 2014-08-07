using System;
using System.IO;
using System.Collections; 
using System.Text.RegularExpressions;

namespace Library.Common.Pdf
{
    public class ProjectPart
	{
		private void OnProgress(int part, int total){}		
		internal event ProgressDelegate ProgressEvent;
		public string path;
		internal string pages;
        public int[] Pages
        {
            get
            {
                ArrayList ps = new ArrayList();
                if (this.pages == null || pages.Length == 0)
                {
                    for (int index = 0; index < this.MaxPage; index++)
                    {
                        ps.Add(index);
                    }
                }
                else
                {
                    string[] ss = this.pages.Split(new char[] { ',',' ',';' });
                    foreach (string s in ss)
                        if (Regex.IsMatch(s, @"\d+-\d+"))
                        {
                            int start = int.Parse(s.Split(new char[] { '-' })[0]);
                            int end = int.Parse(s.Split(new char[] { '-' })[1]);
                            if (start > end)
                                return new int[] {0 };
                            while (start <= end)
                            {
                                ps.Add(start-1);
                                start++;
                            }
                        }
                        else
                        {
                            ps.Add(int.Parse(s)-1);
                        }
                }
                return ps.ToArray(typeof(int)) as int[];
            }           
        }
		public int MaxPage;
       public void Load(string path)
		{
			this.path=path;
			PdfFile pf=new PdfFile(File.OpenRead(path));
			pf.ProgressEvent+=new ProgressDelegate(pf_ProgressEvent);
			pf.Load();
			this.MaxPage=pf.PageCount;
		}
		public ProjectPart()
		{
			this.ProgressEvent=new ProgressDelegate(this.OnProgress);			
		} 
		

		private void pf_ProgressEvent(int part, int total)
		{
			this.ProgressEvent(part,total);
		}
	}
}
