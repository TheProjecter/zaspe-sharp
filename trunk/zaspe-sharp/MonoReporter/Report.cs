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
using Gtk;

using SvgReader;
using SvgReader.Shapes;

namespace MonoReporter
{
	public class Report
	{
		PrintOperation printOperation;
		SvgDocument svgDocument;
		
		public Report(string reportName, string svgFile)
		{
			this.svgDocument = new SvgDocument(svgFile);
			
			this.printOperation = new PrintOperation();
			this.printOperation.NPages = 1;
			this.printOperation.Unit = Unit.Mm;
			this.printOperation.JobName = reportName;
			printOperation.ExportFilename = "test.pdf";
			
			PrintSettings psettings = new PrintSettings();
			PaperSize paperSize = new PaperSize("customPaperSize", "cps", this.svgDocument.PageWidth, this.svgDocument.PageHeight, Unit.Mm);
			psettings.PaperSize = paperSize;
			Console.WriteLine("PageHeight: " + this.svgDocument.PageHeight.ToString());
			Console.WriteLine("PageWidth: " + this.svgDocument.PageWidth.ToString());
			Console.WriteLine();
			this.printOperation.PrintSettings = psettings;
			
			printOperation.DrawPage += this.DrawPage;
		}
		
		public void DrawPage(object o, DrawPageArgs args)
		{
			Cairo.Context con = args.Context.CairoContext;
			con.LineWidth = 1;
			
			Rectangle[] rectangles = this.svgDocument.Rectangles;
			
			foreach (Rectangle r in rectangles) {
				Console.WriteLine("Rect ID: " + r.Id);
				Console.WriteLine("  X: " + r.X + "  Y: " + r.Y + "  Width: " + r.Width + "  Height: " + r.Height);
				Console.WriteLine("  FillColor -> R: " + r.FillColor[0] + " - G: " + r.FillColor[1] + " - B: " + r.FillColor[2]);
				Console.WriteLine("  FillOpacity: " + r.FillOpacity);
				Console.WriteLine("  StrokeColor -> R: " + r.StrokeColor[0] + " - G: " + r.StrokeColor[1] + " - B: " + r.StrokeColor[2]);
				Console.WriteLine("  StrokeOpacity: " + r.StrokeOpacity);
				CairoDrawingFunctions.Draw(con, r);
			}
		}
		
		public void Run(Gtk.Window win)
		{
			printOperation.Run(PrintOperationAction.Export, win);
			
//			if (res == PrintOperationResult.Apply) {
//				Console.WriteLine("Se apretó Apply");
//			}
//			else
//				Console.WriteLine("ajajajajaj");
		}
	}
}
