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
using Cairo;

namespace MonoReporter
{
	public class Page
	{
		public Page()
		{
			PrintOperation printOperation = new PrintOperation();
			printOperation.NPages = 1;
			printOperation.Unit = Unit.Mm;
			
			printOperation.BeginPrint += this.BeginPrint;
			printOperation.DrawPage += this.DrawPage;
			printOperation.EndPrint += this.EndPrint;
			
			PrintOperationResult res = printOperation.Run(PrintOperationAction.PrintDialog, null);
			PrintSettings settings;
			
			if (res == PrintOperationResult.Apply) {
				Console.WriteLine("Se apretó Apply");
				settings = printOperation.PrintSettings;
				
			}
		}
		
		public void BeginPrint(object o, BeginPrintArgs args)
		{
			Console.WriteLine("BeginPrint");
		}
		
		public void DrawPage(object o, DrawPageArgs args)
		{
			Console.WriteLine("DrawPage");
			Context con = args.Context.CairoContext;
			
			// Rectangle
			con.SetSourceRGB(1, 0, 0);
			con.Rectangle(0, 0, args.Context.Width, 50);
			con.Fill();
			
			// Draw another thing
			con.SetSourceRGB(0, 0, 0);
			con.MoveTo(20, 20);
			con.LineTo(50, 50);
			con.MoveTo(90, 75);
			con.LineTo(60, 80);
			con.CurveTo(40, 270, 120, 165, 70, 60);
			con.LineJoin = LineJoin.Miter;
			con.LineWidth = 5;
			
			con.Stroke();
		}
		
		public void EndPrint(object o, EndPrintArgs args)
		{
			Console.WriteLine("EndPrint");
		}
	}
}
