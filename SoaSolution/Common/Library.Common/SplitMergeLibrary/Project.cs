using System;
using System.Collections;

namespace Library.Common.Pdf
{
	public class Project
	{
		public string Name;
		public ArrayList Parts;
		public int TotalPages
		{
			get
			{
				int tot=0;
				foreach (ProjectPart pp in this.Parts)
				{
					if (pp.Pages==null)
					{
						tot+=pp.MaxPage;
					}
					else
					{
						tot+=pp.Pages.Length;
					}
				}
				return tot;
			}
		}
		public Project()
		{
			this.Parts=new ArrayList(); 
		}
	}
}
