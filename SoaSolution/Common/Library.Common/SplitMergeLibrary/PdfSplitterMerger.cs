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

namespace Library.Common.Pdf
{
	public delegate void ProgressDelegate(int part,int total);
	public class PdfSplitterMerger
    {
        Stream target;
        long pos = 15;
        private int number = 3;
        private ArrayList pageNumbers, xrefs;
	   private void OnProgress(int part,int total){}
	   public event ProgressDelegate ProgressEvent;		
      public PdfSplitterMerger(Stream OutputStream)
        {
		  this.ProgressEvent=new ProgressDelegate(this.OnProgress);
			this.xrefs = new ArrayList();
            this.pageNumbers = new ArrayList();
            this.target = OutputStream;
			StreamWriter sw = new StreamWriter(this.target);
            sw.Write("%PDF-1.4\r");
            sw.Flush();
            Byte[] buffer = new Byte[7];
            buffer[0] = 0x25;
            buffer[1] = 0xE2;
            buffer[2] = 0xE3;
            buffer[3] = 0xCF;
            buffer[4] = 0xD3;
            buffer[5] = 0x0D;
            buffer[6] = 0x0A;
            this.target.Write(buffer, 0, buffer.Length);
            this.target.Flush();
        }
       public void Add(Stream PdfInputStream, int[] PageNumbers)
       {
           PdfFile pf = new PdfFile(PdfInputStream);
		   pf.ProgressEvent+=new ProgressDelegate(pf_ProgressEvent);
		   pf.Load();
           PdfSplitter ps = new PdfSplitter();
		   ps.ProgressEvent+=new ProgressDelegate(pf_ProgressEvent);
		   ps.Load(pf, PageNumbers, this.number);
           this.Add(ps);
       }       
        private void Add(PdfSplitter PdfSplitter)
		{
			foreach (int pageNumber in PdfSplitter.pageNumbers)
			{
				this.pageNumbers.Add(PdfSplitter.transHash[pageNumber]);
			}
			ArrayList sortedObjects = new ArrayList();
            foreach (PdfFileObject pfo in PdfSplitter.sObjects.Values)
                sortedObjects.Add(pfo);
            sortedObjects.Sort(new PdfFileObjectNumberComparer());

            foreach (PdfFileObject pfo in sortedObjects)
            {
                this.xrefs.Add(pos);
                this.pos += pfo.WriteToStream(this.target);
                this.number++;
            }
		}       
        public void Finish()
        {
            StreamWriter sw = new StreamWriter(this.target);
            
            string root = "";
            root = "1 0 obj\r";
            root += "<< \r/Type /Catalog \r";
            root += "/Pages 2 0 R \r";
            root += ">> \r";
            root += "endobj\r";

            xrefs.Insert(0, pos);
            pos += root.Length;
            sw.Write(root);
           
            string pages = "";
            pages+= "2 0 obj \r";
            pages += "<< \r";
            pages += "/Type /Pages \r";
            pages += "/Count " + pageNumbers.Count + " \r";
            pages += "/Kids [ ";
            foreach (int pageIndex in pageNumbers)
            {
                pages += pageIndex + " 0 R ";
            }
            pages += "] \r";
            pages += ">> \r";
            pages += "endobj\r";

            xrefs.Insert(1, pos);
            pos += pages.Length;
            sw.Write(pages);
            
           
            sw.Write("xref\r");
            sw.Write("0 " + (this.number) + " \r");
            sw.Write("0000000000 65535 f \r");
            
            foreach (long xref in this.xrefs)
                sw.Write((xref+1).ToString("0000000000") + " 00000 n \r");
            sw.Write("trailer\r");
            sw.Write("<<\r");
            sw.Write("/Size " + (this.number) + "\r");
            sw.Write("/Root 1 0 R \r");
            sw.Write(">>\r");
            sw.Write("startxref\r");
            sw.Write((pos+1) + "\r");
            sw.Write("%%EOF\r");
            sw.Flush();
			sw.Close();
		}

	   private void pf_ProgressEvent(int part, int total)
	   {
			this.ProgressEvent(part,total);
	   }
   }
}
