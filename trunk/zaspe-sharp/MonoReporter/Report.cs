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

namespace MonoReporter
{
	public class Report
	{
		PrintOperation printOperation;
		
		public Report()
		{
			// All this is only a test.
			
			Console.WriteLine("uno");
			this.printOperation = new PrintOperation();
			
			this.printOperation.ShowProgress = true;
			printOperation.NPages = 1;
			printOperation.Unit = Unit.Mm;
			printOperation.JobName = "ImagenesEnGtkPrint";
			
//			PrintSettings settings = new PrintSettings();
//			settings.UseColor = true;
//			printOperation.PrintSettings = settings;
			
			Console.WriteLine("dos");
			printOperation.DrawPage += this.DrawPage;
		}
		
		public void DrawPage(object o, DrawPageArgs args)
		{
			Cairo.Context con = args.Context.CairoContext;
			con.Antialias = Cairo.Antialias.Subpixel;
			
			// Cargo una imagen
			Console.WriteLine("cuatro");
			Gdk.Pixbuf imagen = Rsvg.Pixbuf.FromFileAtSize("prueba.svg", (int)args.Context.Width, (int)args.Context.Height);
			
			Gdk.CairoHelper.SetSourcePixbuf(con, imagen, 0, 0);
			con.Paint();
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
