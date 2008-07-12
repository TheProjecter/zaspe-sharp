//  Zaspe# - Attendance management
//  Copyright (C) 2006, 2007 Milton Pividori
//
//  This file is part of Zaspe#.
//
//  Zaspe# is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 3 of the License, or
//  (at your option) any later version.
//
//  Zaspe# is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with Zaspe#.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.IO;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ZaspeSharp.ReportGenerator
{
	internal abstract class ReportType
	{
		protected Selection selection;
		protected Document doc;
		
//		public ReportType(string reportFile, string reportTitle) : this(null, reportFile, reportTitle)
//		{
//		}
		
		public ReportType(Selection sel, string reportFile, string reportTitle)
		{
			this.selection = sel;
			this.doc = new Document(PageSize.A4);
			
			PdfWriter.GetInstance(doc, new FileStream(reportFile, FileMode.Create));
			this.doc.Open();
			
			// Title
			Chunk c = new Chunk(reportTitle, FontFactory.GetFont(FontFactory.HELVETICA, 20, Font.BOLD));
			Paragraph par = new Paragraph(c);
			par.Alignment = Rectangle.ALIGN_CENTER;

			this.doc.Add(par);
		}
		
		public abstract void MakeReport();
	}
}
