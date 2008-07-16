/*
  MonoReporter - Report tool for Mono
  Copyright (C) 2006,2007 Milton Pividori

  This file is part of MonoReporter.

  MonoReporter is free software; you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation; either version 3 of the License, or
  (at your option) any later version.

  MonoReporter is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using Gtk;

using SvgReader;
using SvgReader.Shapes;

namespace MonoReporter
{
	public class Report
	{
		private PrintOperation printOperation;
		private SvgDocument svgDocument;
		
		private PrintOperationAction action;
		
		private Dictionary<string,string> data;
		private Dictionary<string,DataTable> dataTables;
		
		// Sections
		Section pageHeader;
		Section pageDetail;
		Section pageFooter;
		
		public Report(string reportName, string svgFile)
		{
			// Load svg file
			this.svgDocument = new SvgDocument(svgFile);
			
			// Create PrintOperation object
			this.printOperation = new PrintOperation();
			this.printOperation.NPages = 1;
			this.printOperation.Unit = Unit.Mm;
			this.printOperation.JobName = reportName;
			this.printOperation.ExportFilename = "test.pdf";
			
			PrintSettings psettings = new PrintSettings();
			PaperSize paperSize = new PaperSize("customPaperSize", "cps",
			                                    this.svgDocument.PageWidth,
			                                    this.svgDocument.PageHeight,
			                                    Unit.Mm);
			psettings.PaperSize = paperSize;
			
			Console.WriteLine("PageHeight: " + this.svgDocument.PageHeight.ToString());
			Console.WriteLine("PageWidth: " + this.svgDocument.PageWidth.ToString());
			Console.WriteLine();
			this.printOperation.PrintSettings = psettings;
			
			printOperation.DrawPage += this.DrawPage;
			printOperation.BeginPrint += this.BeginPrint;
			
			// Set default action
			this.action = PrintOperationAction.Export;
			
			// Data
			this.data = new Dictionary<string, string>();
			this.dataTables = new Dictionary<string, DataTable>();
			
			this.pageHeader = this.svgDocument.PageHeaderSection;
			this.pageDetail = this.svgDocument.PageDetailSection;
			this.pageFooter = this.svgDocument.PageFooterSection;
		}
		
		public PrintOperationAction Action
		{
			get { return this.action; }
			set { this.action = value; }
		}
		
		public Dictionary<string, string> Data
		{
			get { return this.data; }
			set { this.data = value; }
		}
		
		public Dictionary<string, DataTable> DataTables
		{
			get { return this.dataTables; }
			set { this.dataTables = value; }
		}
		
		public void BeginPrint(object o, Gtk.BeginPrintArgs args)
		{
			// Print shapes in the PageHeader section
			Console.WriteLine("$$$$$$$$$$$$$$ Page Header");
			this.pageHeader.Draw(args.Context,
			                     this.data,
			                     this.dataTables,
			                     this.svgDocument.PageDetailSection.Y,
			                     false);
			
			// Print shapes in the PageDetail section
			Console.WriteLine("$$$$$$$$$$$$$$ Page Detail");
			this.pageDetail.Draw(args.Context,
			                     this.data,
			                     this.dataTables,
			                     this.svgDocument.PageFooterSection.Y,
			                     false);
			
			// Print shapes in the PageFooter section
			Console.WriteLine("$$$$$$$$$$$$$$ Page Footer");
			this.pageFooter.Draw(args.Context,
			                     this.data,
			                     this.dataTables,
			                     this.printOperation.PrintSettings.PaperSize.GetHeight(Unit.Mm),
			                     false);
			
			Console.WriteLine("PH: " + this.pageHeader.PagesToAdd);
			Console.WriteLine("PD: " + this.pageDetail.PagesToAdd);
			Console.WriteLine("PF: " + this.pageFooter.PagesToAdd);
			int numberOfPagesToAdd = this.pageHeader.PagesToAdd +
				this.pageDetail.PagesToAdd + this.pageFooter.PagesToAdd;
			
			PrintOperation op = o as PrintOperation;
			op.NPages = op.NPages + numberOfPagesToAdd;
			
			this.pageHeader.ResetDataTablesToDraw();
			this.pageDetail.ResetDataTablesToDraw();
			this.pageFooter.ResetDataTablesToDraw();
		}
		
		private void DrawPage(object o, DrawPageArgs args)
		{
			/* Print shapes in the ReporteHeader section if we are drawing
			 * the first page */
//			if (args.PageNr == 0) {
//				Console.WriteLine("$$$$$$$$$$$$$$ REPORT HEADER");
//				this.reportHeader.Draw(args.Context,
//				                       this.data,
//				                       this.dataTables,
//				                       Double.MaxValue);
//			}
			
			// Print shapes in the PageHeader section
			Console.WriteLine("$$$$$$$$$$$$$$ Page Header");
			this.pageHeader.Draw(args.Context,
			                     this.data,
			                     this.dataTables,
			                     this.svgDocument.PageDetailSection.Y,
			                     true);
			
			// Print shapes in the PageDetail section
			Console.WriteLine("$$$$$$$$$$$$$$ Page Detail");
			this.pageDetail.Draw(args.Context,
			                     this.data,
			                     this.dataTables,
			                     this.svgDocument.PageFooterSection.Y,
			                     true);
			
			// Print shapes in the PageFooter section
			Console.WriteLine("$$$$$$$$$$$$$$ Page Footer");
			this.pageFooter.Draw(args.Context,
			                     this.data,
			                     this.dataTables,
			                     this.printOperation.PrintSettings.PaperSize.GetHeight(Unit.Mm),
			                     true);
			
			/* Print shapes in the ReportFooter section if we are draing the
			 * last page (TODO) */
		}
		
		public void Run(Gtk.Window win)
		{
			// Calculate the number of pages needed
			double pageHeight =
				this.printOperation.PrintSettings.PaperSize.GetHeight(Unit.Mm);
			double sum = 0;
			
			printOperation.Run(this.action, win);
		}
	}
}
