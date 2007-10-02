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
using SvgReader.Shapes;

namespace MonoReporter
{
	public static class CairoDrawingFunctions
	{
		public static void Draw(Cairo.Context con, Rectangle rectangle)
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
	}
}
