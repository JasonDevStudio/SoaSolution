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
using System.IO;
using System.Text.RegularExpressions;

namespace Library.Common.Pdf
{
    internal class PdfFileStreamObject : PdfFileObject
    {
        private byte[] streamBuffer;
        private int streamStartOffset, streamLength;
        internal PdfFileStreamObject(PdfFileObject obj)
        {
            this.address = obj.address;
            this.length = obj.length;
            this.text = obj.text;
           this.number = obj.number;
            this.PdfFile = obj.PdfFile;
            this.LoadStreamBuffer();                        
        }
        
        private void LoadStreamBuffer()
        {
            Match m1 = Regex.Match(this.text, @"stream\s*");
            this.streamStartOffset = m1.Index + m1.Value.Length;
            this.streamLength = this.length - this.streamStartOffset;
            this.streamBuffer = new byte[this.streamLength];
            this.PdfFile.memory.Seek(this.address+this.streamStartOffset, SeekOrigin.Begin);
            this.PdfFile.memory.Read(this.streamBuffer, 0,this.streamLength);

            this.PdfFile.memory.Seek(this.address,SeekOrigin.Begin);
            StreamReader sr = new StreamReader(this.PdfFile.memory);
            char[] startChars = new char[this.streamStartOffset];
            sr.ReadBlock(startChars, 0, this.streamStartOffset);
            StringBuilder sb = new StringBuilder();
            sb.Append(startChars);
            this.text = sb.ToString();           
        }
        internal override void Transform(System.Collections.Hashtable TransformationHash)
        {
            base.Transform(TransformationHash);
        }
        internal override long WriteToStream(Stream Stream)
        {
            StreamWriter sw = new StreamWriter(Stream);
            sw.Write(this.text);
            sw.Flush();
            new MemoryStream(this.streamBuffer).WriteTo(Stream);
           sw.Flush();
            return this.streamLength+this.text.Length;
        }       
    }
}
