//  MonoReporter - Report tool for Mono
//  Copyright (C) 2006,2007 Milton Pividori
//
//  This file is part of MonoReporter.
//
//  MonoReporter is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 3 of the License, or
//  (at your option) any later version.
//
//  MonoReporter is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using Gtk;
using SvgReader;

namespace MonoReporter
{
	public class Report
	{
		PrintOperation printOperation;
		SvgDocument svgDocument;
		
		public Report(string svgFile)
		{
			this.svgDocument = new SvgDocument(svgFile);
			
			this.printOperation = new PrintOperation();
			this.printOperation.NPages = 1;
			this.printOperation.Unit = Unit.Mm;
			this.printOperation.JobName = "ImagenesEnGtkPrint";
			
			PrintSettings psettings = new PrintSettings();
			Console.WriteLine("PageHeight: " + this.svgDocument.PageHeight.ToString());
			Console.WriteLine("PageWidth: " + this.svgDocument.PageWidth.ToString());
			Console.WriteLine();
			psettings.SetPaperHeight(this.svgDocument.PageHeight, Unit.Mm);
			psettings.SetPaperWidth(this.svgDocument.PageWidth, Unit.Mm);
			this.printOperation.PrintSettings = psettings;
			
			printOperation.DrawPage += this.DrawPage;
		}
		
		public void DrawPage(object o, DrawPageArgs args)
		{
			Cairo.Context con = args.Context.CairoContext;
			con.LineWidth = 1;
			
			Rectangle[] rectangles = this.svgDocument.Rectangles;
			
			foreach (Rectangle r in rectangles) {
				Console.WriteLine("X: " + r.X + "  Y: " + r.Y + "  Width: " + r.Width + "  Height: " + r.Height);
				Cairo.Rectangle cr = new Cairo.Rectangle(new Cairo.Point((int)r.X, (int)r.Y),
			                                      r.Width,
			                                      r.Height);
			
				con.Rectangle(cr);
				con.Stroke();
			}
		}
		
		public void Run(Gtk.Window win)
		{
			printOperation.Run(PrintOperationAction.PrintDialog, win);
			
//			if (res == PrintOperationResult.Apply) {
//				Console.WriteLine("Se apretó Apply");
//			}
//			else
//				Console.WriteLine("ajajajajaj");
		}
	}
}
