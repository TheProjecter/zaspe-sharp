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
using Pango;
using SvgReader.Shapes;

namespace MonoReporter
{
	public static class CairoDrawingFunctions
	{
		public static void Draw(Cairo.Context con, SvgReader.Shapes.Rectangle rectangle)
		{
			Cairo.Rectangle cairoRectangle = new Cairo.Rectangle(new Cairo.Point((int)rectangle.X, (int)rectangle.Y),
			                                         rectangle.Width,
			                                         rectangle.Height);
			
			con.Rectangle(cairoRectangle);
			con.Color = new Cairo.Color(rectangle.FillColor[0], rectangle.FillColor[1], rectangle.FillColor[2], rectangle.FillOpacity);
			con.FillPreserve();
			
			con.LineWidth = rectangle.LineWidth;
			con.Color = new Cairo.Color(rectangle.StrokeColor[0], rectangle.StrokeColor[1], rectangle.StrokeColor[2], rectangle.StrokeOpacity);
			con.Stroke();
		}
		
		public static void Draw(Cairo.Context con, Pango.Layout layout, Text text)
		{
			layout.SetText(text.TextValue);
			layout.FontDescription = FontDescription.FromString(text.FontDescription);
			
			int pixel_size_width, pixel_size_height;
			layout.GetPixelSize(out pixel_size_width, out pixel_size_height);
			
			Console.WriteLine("   Pixel size width: " + pixel_size_width);
			Console.WriteLine("   Pixel size height: " + pixel_size_height);
			Console.WriteLine("   Text X position: " + text.X);
			Console.WriteLine("   Text Y position: " + text.Y);
			
			double x_offset = pixel_size_width*0; // Disabled
			double y_offset = pixel_size_height * 1.15;
			
			Console.WriteLine("width: " + x_offset);
			Console.WriteLine("height: " + y_offset);
			
			con.MoveTo(text.X - x_offset, text.Y - y_offset);
			Pango.CairoHelper.ShowLayout(con, layout);
			
//			con.MoveTo(text.X, text.Y - y_offset);
//			con.LineTo(text.X + 100, text.Y - y_offset);
//			con.Stroke();
//			                  
//			con.MoveTo(text.X, 38.4376957919563);
//			con.LineTo(text.X + 100, 38.4376957919563);
//			con.Stroke();
		}
	}
}
