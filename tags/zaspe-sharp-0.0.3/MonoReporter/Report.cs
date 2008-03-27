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
			this.printOperation.NPages = 1; // FIXME: Correct number of pages must be calculated
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
			                     this.svgDocument.PageDetailSection.Y);
			
			// Print shapes in the PageDetail section
			Console.WriteLine("$$$$$$$$$$$$$$ Page Detail");
			this.pageDetail.Draw(args.Context,
			                     this.data,
			                     this.dataTables,
			                     this.svgDocument.PageFooterSection.Y);
			
			// Print shapes in the PageFooter section
			Console.WriteLine("$$$$$$$$$$$$$$ Page Footer");
			this.pageFooter.Draw(args.Context,
			                     this.data,
			                     this.dataTables,
			                     this.printOperation.PrintSettings.PaperSize.GetHeight(Unit.Mm));
			
			/* Print shapes in the ReportFooter section if we are draing the
			 * last page (TODO) */
			
//			Cairo.Context con = args.Context.CairoContext;
//			
//			// Draw rectangles
//			Rectangle[] rectangles = this.svgDocument.Rectangles;
//			foreach (Rectangle r in rectangles) {
//				Console.WriteLine("Rect ID: " + r.Id);
//				Console.WriteLine("  X: " + r.X + "  Y: " + r.Y + "  Width: " + r.Width + "  Height: " + r.Height);
//				Console.WriteLine("  FillColor -> R: " + r.FillColor[0] + " - G: " + r.FillColor[1] + " - B: " + r.FillColor[2]);
//				Console.WriteLine("  FillOpacity: " + r.FillOpacity);
//				Console.WriteLine("  StrokeColor -> R: " + r.StrokeColor[0] + " - G: " + r.StrokeColor[1] + " - B: " + r.StrokeColor[2]);
//				Console.WriteLine("  StrokeOpacity: " + r.StrokeOpacity);
//				CairoDrawingFunctions.Draw(con, r);
//			}
//			
//			// Draw text
//			Text[] texts = this.svgDocument.Texts;
//			foreach(Text t in texts) {
//				Console.WriteLine("Text ID: " + t.Id);
//				Console.WriteLine("  TextValue: " + t.TextValue);
//				Console.WriteLine("  FontDescription: " + t.FontDescription);
//				Console.WriteLine("  X: " + t.X + "  Y: " + t.Y);
//				Console.WriteLine("  Color -> R: " + t.Color[0] + " - G: " + t.Color[1] + " - B: " + t.Color[2]);
//				Console.WriteLine("  Opacity: " + t.Opacity);
////				Console.WriteLine("  StrokeColor -> R: " + r.StrokeColor[0] + " - G: " + r.StrokeColor[1] + " - B: " + r.StrokeColor[2]);
////				Console.WriteLine("  StrokeOpacity: " + r.StrokeOpacity);
//				
//				if (this.data.ContainsKey(t.Id)) {
//					t.TextValue = this.data[t.Id].ToString();
//					Console.WriteLine("Data setted. Text id: " + t.Id + ". Value: " + t.TextValue);
//				}
//				
//				CairoDrawingFunctions.Draw(con, args.Context.CreatePangoLayout(), t);
//			}
//			
//			// Draw lines
//			foreach(Line l in this.svgDocument.Lines) {
//				CairoDrawingFunctions.Draw(con, l);
//			}
//			
//			// Draw tables
//			foreach (SvgReader.Shapes.Table aTable in this.svgDocument.Tables) {
//				Console.WriteLine("Table id: " + aTable.Id);
//				
//				if (!this.dataTables.ContainsKey(aTable.Id))
//					continue;
//				
//				Console.WriteLine("Procesando un DataTable...");
//				
//				DataTable dataTable = (DataTable)this.dataTables[aTable.Id];
//				Text textTemp;
//				ArrayList textRow = new ArrayList(aTable.NumberOfColumns);
//				
//				foreach (DataRow dataRow in dataTable.Rows) {
//					foreach (Text aText in aTable.LastRowAdded) {
//						Console.WriteLine("   Text: " + aText.TextValue);
//						
//						textTemp = (Text)aText.Clone();
//						textTemp.TextValue = dataRow[textTemp.Id].ToString();
//						textTemp.Y = textTemp.Y +
//							CairoDrawingFunctions.GetPixelHeightSize(textTemp,
//							                                         args.Context.CreatePangoLayout());
//						
//						textRow.Add(textTemp);
//					}
//					
//					aTable.AddRow(textRow);
//					textRow.Clear();
//				}
//				
//				CairoDrawingFunctions.Draw(args.Context, aTable);
//			}
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
