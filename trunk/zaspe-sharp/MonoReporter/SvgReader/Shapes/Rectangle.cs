//    MensajeroRemoting - Mensajero instantáneo hecho con .NET Remoting
//    y otras tecnologías de .NET.
//    Copyright (C) 2007  Luis Ignacio Larrateguy, Milton Pividori y César Sandrigo
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Globalization;

namespace SvgReader
{
	public class Rectangle
	{
		private string id;
		private double width;
		private double height;
		private double x;
		private double y;
		
		public Rectangle(string id, double width, double height, double x, double y)
		{
			this.id = id;
			this.width = width;
			this.height = height;
			this.x = x;
			this.y = y;
		}
		
		public Rectangle(string id, string width, string height, string x, string y)
			: this(id,
			       Double.Parse(width, NumberStyles.AllowDecimalPoint),
			       Double.Parse(height, NumberStyles.AllowDecimalPoint),
			       Double.Parse(x, NumberStyles.AllowDecimalPoint),
			       Double.Parse(y, NumberStyles.AllowDecimalPoint))
		{
		}
		
		public string Id
		{
			get { return this.id; }
			set { this.id = value; }
		}
		
		public double Width
		{
			get { return this.width; }
			set { this.width = value; }
		}
		
		public double Height
		{
			get { return this.height; }
			set { this.height = value; }
		}
		
		public double X
		{
			get { return this.x; }
			set { this.x = value; }
		}
		
		public double Y
		{
			get { return this.y; }
			set { this.y = value; }
		}
	}
}
