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
using Pango;
using Cairo;

namespace MonoReporter
{
	public class Page
	{
		private const double HEADER_HEIGHT = (10*72/25.4);
		PrintOperation printOperation;
		
		public Page()
		{
			// All this is only a test.
			
			Console.WriteLine("uno");
			this.printOperation = new PrintOperation();
			
			Console.WriteLine("dos");
			printOperation.BeginPrint += this.BeginPrint;
			printOperation.DrawPage += this.DrawPage;
			printOperation.EndPrint += this.EndPrint;
		}
		
		public void BeginPrint(object o, BeginPrintArgs args)
		{
			Console.WriteLine("BeginPrint");
			
			PrintOperation printOperation = (PrintOperation)o;
			printOperation.NPages = 1;
			printOperation.Unit = Unit.Mm;
		}
		
		public void DrawPage(object o, DrawPageArgs args)
		{
			Console.WriteLine("DrawPage");
			
			Pango.Layout layout;
			int textWidth, textHeight;
			double width;
			FontDescription desc;
			
			Cairo.Context con = args.Context.CairoContext;
			width = args.Context.Width;
			
			// Rectangle
			con.Rectangle(0, 0, width, HEADER_HEIGHT);
			con.Color = new Cairo.Color(0.8, 0.8, 0.8);
			con.FillPreserve();
			
			// Draw another thing
			con.Color = new Cairo.Color(0, 0, 0);
			con.LineWidth = 1;
			con.Stroke();
			
			con.Color = new Cairo.Color(0, 0, 0);
			con.MoveTo(20, 200);
			con.CurveTo(40, 270, 120, 165, 70, 60);
			con.LineJoin = LineJoin.Bevel;
			con.Stroke();
			
			con.MoveTo(30, 275);
			con.LineTo(60, 80);
			con.Stroke();
			
			con.Color = new Cairo.Color(0, 0, 0);
			layout = args.Context.CreatePangoLayout();
			layout.SetText("Prueba con Pango");
			desc = FontDescription.FromString("sans 14");
			layout.FontDescription = desc;
			
			layout.GetPixelSize(out textWidth, out textHeight);
			
			if (textWidth > width) {
				layout.Width = (int)width;
				layout.Ellipsize = EllipsizeMode.Start;
				layout.GetPixelSize(out textWidth, out textHeight);
			}
			
			con.MoveTo((width - textWidth)/2, (HEADER_HEIGHT - textHeight)/2);
			Pango.CairoHelper.ShowLayout(con, layout);
		}
		
		public void EndPrint(object o, EndPrintArgs args)
		{
			Console.WriteLine("EndPrint");
		}
		
		public void Run(Gtk.Window win)
		{
			Console.WriteLine("tres");
			printOperation.Run(PrintOperationAction.PrintDialog, win);
			
//			if (res == PrintOperationResult.Apply) {
//				Console.WriteLine("Se apretó Apply");
//			}
//			else
//				Console.WriteLine("ajajajajaj");
		}
	}
}
