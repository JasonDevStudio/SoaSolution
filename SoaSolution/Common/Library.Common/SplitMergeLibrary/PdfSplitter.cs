//============================================================================
//Gios PDF SPlitter And Merger - A library for splitting and merging Pdf Documents in C#
//Copyright (C) 2005  Paolo Gios - www.paologios.com
//
//This library is free software; you can redistribute it and/or
//modify it under the terms of the GNU Lesser General Public
//License as published by the Free Software Foundation; either
//version 2.1 of the License, or (at your option) any later version.
//
//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//Lesser General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public
//License along with this library; if not, write to the Free Software
//Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//=============================================================================
using System;
using System.Text;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace Library.Common.Pdf
{
	internal class PdfSplitter
    {
        internal Hashtable sObjects;
        internal ArrayList pageNumbers;
        internal Hashtable  transHash;
        internal PdfFile PdfFile;
		private void OnProgress(int part, int total){}		
		public event ProgressDelegate ProgressEvent;
		internal PdfSplitter()
		{
           this.ProgressEvent=new ProgressDelegate(this.OnProgress);
		}
		internal void Load(PdfFile PdfFile,int[] PageNumbers, int startNumber)
		{
			this.PdfFile = PdfFile;
			this.pageNumbers = new ArrayList();
			this.sObjects = new Hashtable();
			int part=0;
			int total=PageNumbers.Length;
			foreach (int PageNumber in PageNumbers)
			{
				this.ProgressEvent(part,total);
				PdfFileObject page = PdfFile.PageList[PageNumber] as PdfFileObject;
				page.PopulateRelatedObjects(PdfFile, this.sObjects);
				this.pageNumbers.Add(page.number);
				part++;
			}
			this.transHash = this.CalcTransHash(startNumber);
			foreach (PdfFileObject pfo in this.sObjects.Values)
			{
				pfo.Transform(transHash);
			}
		}     
        private Hashtable CalcTransHash(int startNumber)
        {
            Hashtable ht = new Hashtable();
            ArrayList al = new ArrayList();
            foreach (PdfFileObject pfo in this.sObjects.Values)
            {
                al.Add(pfo);
            }
            al.Sort(new PdfFileObjectNumberComparer());
            int number = startNumber;
            foreach (PdfFileObject pfo in al)
            {
                ht.Add(pfo.number, number);
                number++;
            }
            return ht;
        }       
        
    }
}

