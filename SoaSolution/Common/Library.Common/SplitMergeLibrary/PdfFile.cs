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
using System.IO;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Library.Common.Pdf
{
	public class PdfFile
    {
		private void OnProgress(int part,int total){}
		internal event ProgressDelegate ProgressEvent;
		internal string trailer;
        internal Stream memory;
        internal Hashtable objects;
        public PdfFile(Stream InputStream)
        {
            this.memory = InputStream;
			this.ProgressEvent=new ProgressDelegate(this.OnProgress);
        }

               
        public void Load()
        {
            long startxref = this.GetStartxref();
            this.trailer = this.ParseTrailer(startxref);
            long[] adds=this.GetAddresses(startxref);
            this.LoadHash( adds);
        }
		private void LoadHash(long[] addresses)
		{
			this.objects = new Hashtable();
			int part=0;
			int total=addresses.Length;
			foreach (long add in addresses)
			{
				this.ProgressEvent(part,total);
				this.memory.Seek(add, SeekOrigin.Begin);
				StreamReader sr = new StreamReader(this.memory);
				string line = sr.ReadLine();
				if (line.Length<2)
					line=sr.ReadLine();
				Match m = Regex.Match(line, @"(?'id'\d+)( )+0 obj",RegexOptions.ExplicitCapture);
				if (m.Success)
				{
					int num = int.Parse(m.Groups["id"].Value);
					if (!objects.ContainsKey(num))
					{
						objects.Add(num, PdfFileObject.Create(this,num,add));
					}
				}
				part++;
			}
		}
        
        internal PdfFileObject LoadObject(string text,string key)
        {
            string pattern = @"/"+key+@" (?'id'\d+)";
            Match m = Regex.Match(text, pattern, RegexOptions.ExplicitCapture);
			if (m.Success)
			{
				return this.LoadObject(int.Parse(m.Groups["id"].Value));
			}
			return null;
        }
        internal PdfFileObject LoadObject(int number)
        {
            return this.objects[number] as PdfFileObject;
        }
        internal ArrayList PageList
        {
            get
            {
                PdfFileObject root = this.LoadObject(this.trailer, "Root");
                PdfFileObject pages = this.LoadObject(root.text, "Pages");
                return pages.GetKids();
            }
        }
        public int PageCount
        {
            get
            {
                return this.PageList.Count;
            }
        }
        private long[] GetAddresses(long xref)
        {
            this.memory.Seek(xref, SeekOrigin.Begin);
            ArrayList al = new ArrayList();
            StreamReader sr = new StreamReader(this.memory);
            string line="";
            string prevPattern = @"/Prev \d+";
            bool ok = true;
            while (ok)
            {
                if (Regex.IsMatch(line, @"\d{10} 00000 n\s*"))
                {
                    al.Add(long.Parse(line.Substring(0,10)));
                }
                
                line = sr.ReadLine();
                ok = !(line == null || Regex.IsMatch(line, ">>"));
                if (line != null)
                {
                    Match m = Regex.Match(line, prevPattern);
                    if (m.Success)
                    {
                        al.AddRange(this.GetAddresses(long.Parse(m.Value.Substring(6))));
                    }
                }
                              
            }
            return al.ToArray(typeof(long)) as long[];
        }   
        private long GetStartxref()
        {
            StreamReader sr = new StreamReader(this.memory);
            this.memory.Seek(this.memory.Length - 100, SeekOrigin.Begin);
            string line="";
            while (!line.StartsWith("startxref"))
            {
                line = sr.ReadLine();
            }
            long startxref = long.Parse(sr.ReadLine());
            if (startxref == -1)
                throw new Exception("Cannot find the startxref");
            return startxref;
        }
        private string ParseTrailer(long xref)
        {
            this.memory.Seek(xref, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(this.memory);
            string line;
            string trailer = "";
            bool istrailer = false;
            while ((line = sr.ReadLine()) != "startxref")
            {
                line = line.Trim();
                if (line.StartsWith("trailer"))
                {
                    trailer = "";
                    istrailer = true;
                }
                if (istrailer)
                {
                    trailer += line + "\r";
                }
            }
            if (trailer == "")
                throw new Exception("Cannot find trailer");
            return trailer;
        }

    }
}

