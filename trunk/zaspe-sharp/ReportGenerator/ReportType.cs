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
using System.Collections.Generic;
using System.IO;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ZaspeSharp.ReportGenerator
{
	internal abstract class ReportType
	{
		protected Selection selection;
		protected Rectangle pageSize;
		protected string reportFile;
		protected string reportTitle;
		
		protected Document doc;
		
		private static Dictionary<Type,ReportType> instance;
		
		static ReportType()
		{
			instance = new Dictionary<Type,ReportType>();
		}
		
//		private ReportType(Selection selection, Rectangle pageSize, string reportFile, string reportTitle)
//		{
//			this.selection = selection;
//			this.pageSize = pageSize;
//			this.reportFile = reportFile;
//			this.reportTitle = reportTitle;
//		}
		
		public static T GetInstance<T>(Selection selection,
		                               Rectangle pageSize,
		                               string reportFile) where T : ReportType, new()
		{
			// Create instance if it does not exist
			ReportType reportType;
			instance.TryGetValue(typeof(T), out reportType);
			
			if (reportType == null) {
				instance[typeof(T)] = new T();
				reportType = instance[typeof(T)];
			}

			// Set parameters
			reportType.selection = selection;
			reportType.pageSize = pageSize;
			reportType.reportFile = reportFile;
			
			// Open the documment
			reportType.doc = new Document(pageSize);
			
			return (T)reportType;
		}
		
		public void InitializeDocumment()
		{
			PdfWriter.GetInstance(this.doc, new FileStream(this.reportFile, FileMode.Create));
			this.doc.Open();
			
			// Title
			Chunk c = new Chunk(this.reportTitle, FontFactory.GetFont(FontFactory.HELVETICA, 20, Font.BOLD));
			Paragraph par = new Paragraph(c);
			par.Alignment = Rectangle.ALIGN_CENTER;
			
			this.doc.Add(par);
		}
		
		public void Run()
		{
			// First, initilize documment.
			this.InitializeDocumment();
			
			// Then, make the report
			this.MakeReport();
			
			// Close the documment
			this.doc.Close();
		}
		
		public abstract void MakeReport();
	}
}
