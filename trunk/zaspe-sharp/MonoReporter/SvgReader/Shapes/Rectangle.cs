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
using System.Xml;
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
		private double lineWidth;
		
		public Rectangle(XmlNode rectNode)
		{
			string width = Utils.GetAttributeValueFromNode(rectNode, "width");
			string height = Utils.GetAttributeValueFromNode(rectNode, "height");
			string x = Utils.GetAttributeValueFromNode(rectNode, "x");
			string y = Utils.GetAttributeValueFromNode(rectNode, "y");
			
			// Read style, and get interesting properties from it
			try {
				string lineWidth = Utils.GetValueFromMultiValuatedAttribute(rectNode, "style", "stroke-width");
				this.lineWidth = Utils.DoubleParseAndPixelToMm(lineWidth);
			}
			catch (SubOptionNotFoundException) {
				this.lineWidth = Utils.PixelToMm(1.0);
			}
			
			this.id = Utils.GetAttributeValueFromNode(rectNode, "id");
			this.width = Utils.DoubleParseAndPixelToMm(width);
			this.height = Utils.DoubleParseAndPixelToMm(height);
			this.x = Utils.DoubleParseAndPixelToMm(x);
			this.y = Utils.DoubleParseAndPixelToMm(y);
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
		
		public double LineWidth
		{
			get { return this.lineWidth; }
			set { this.lineWidth = value; }
		}
	}
}