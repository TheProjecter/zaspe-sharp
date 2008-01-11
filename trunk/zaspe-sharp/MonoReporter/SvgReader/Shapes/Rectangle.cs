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

namespace SvgReader.Shapes
{
	public class Rectangle : IShape
	{
		private string id;
		private double width;
		private double height;
		private double x;
		private double y;
		private double lineWidth;
		
		private double[] fillColor;
		private double fillOpacity;
		
		private double[] strokeColor;
		private double strokeOpacity;
		
		public Rectangle(XmlNode rectNode)
		{
			this.id = Utils.GetAttributeValueFromNode(rectNode, "id");
			
			string width = Utils.GetAttributeValueFromNode(rectNode, "width");
			string height = Utils.GetAttributeValueFromNode(rectNode, "height");
			string x = Utils.GetAttributeValueFromNode(rectNode, "x");
			string y = Utils.GetAttributeValueFromNode(rectNode, "y");
			
			this.width = Utils.DoubleParseAndPixelToMm(width);
			this.height = Utils.DoubleParseAndPixelToMm(height);
			this.x = Utils.PixelToMm(Utils.NormalizeShapeX(x));
			this.y = Utils.PixelToMm(Utils.NormalizeShapeY(y));
			
			// Read style, and get interesting properties from it
			try {
				string lineWidth = Utils.GetValueFromMultiValuatedAttribute(rectNode, "style", "stroke-width");
				this.lineWidth = Utils.DoubleParseAndPixelToMm(lineWidth);
			}
			catch (SubOptionNotFoundException) {
				this.lineWidth = Utils.PixelToMm(1.0);
			}
			
			try {
				string fillColor = Utils.GetValueFromMultiValuatedAttribute(rectNode, "style", "fill");
				this.fillColor = Utils.FromHexColorToRGBA(fillColor);
			}
			catch (SubOptionNotFoundException) {
				this.fillColor = new double[] {0, 0, 0};
			}
			
			try {
				string fillOpacity = Utils.GetValueFromMultiValuatedAttribute(rectNode, "style", "fill-opacity");
				this.fillOpacity = Utils.DoubleParse(fillOpacity);
			}
			catch (SubOptionNotFoundException) {
				this.fillOpacity = 1.0;
			}
			
			try {
				string strokeColor = Utils.GetValueFromMultiValuatedAttribute(rectNode, "style", "stroke");
				this.strokeColor = Utils.FromHexColorToRGBA(strokeColor);
			}
			catch (SubOptionNotFoundException) {
				this.strokeColor = new double[] {0, 0, 0};
			}
			
			try {
				string strokeOpacity = Utils.GetValueFromMultiValuatedAttribute(rectNode, "style", "stroke-opacity");
				this.strokeOpacity = Utils.DoubleParse(strokeOpacity);
			}
			catch (SubOptionNotFoundException) {
				this.strokeOpacity = 1.0;
			}
		}
		
		public string Id
		{
			get { return this.id; }
			set { this.id = value; }
		}
		
//		public double Width
//		{
//			get { return this.width; }
//			set { this.width = value; }
//		}
//		
//		public double Height
//		{
//			get { return this.height; }
//			set { this.height = value; }
//		}
//		
//		public double X
//		{
//			get { return this.x; }
//			set { this.x = value; }
//		}
//		
//		public double Y
//		{
//			get { return this.y; }
//			set { this.y = value; }
//		}
//		
//		public double LineWidth
//		{
//			get { return this.lineWidth; }
//			set { this.lineWidth = value; }
//		}
//		
//		public double[] FillColor
//		{
//			get { return this.fillColor; }
//			set { this.fillColor = value; }
//		}
//		
//		public double FillOpacity
//		{
//			get { return this.fillOpacity; }
//			set { this.fillOpacity = value; }
//		}
//		
//		public double[] StrokeColor
//		{
//			get { return this.strokeColor; }
//			set { this.strokeColor = value; }
//		}
//		
//		public double StrokeOpacity
//		{
//			get { return this.strokeOpacity; }
//			set { this.strokeOpacity = value; }
//		}
		
		public void Draw (Gtk.PrintContext context)
		{
			Cairo.Context con = context.CairoContext;
			
			Cairo.Rectangle cairoRectangle =
				new Cairo.Rectangle(new Cairo.Point((int)this.x,
				                                    (int)this.y),
				                    this.width,
				                    this.height);
			
			con.Rectangle(cairoRectangle);
			con.Color = new Cairo.Color(this.fillColor[0],
			                            this.fillColor[1],
			                            this.fillColor[2],
			                            this.fillOpacity);
			con.FillPreserve();
			
			con.LineWidth = this.lineWidth;
			con.Color = new Cairo.Color(this.strokeColor[0],
			                            this.strokeColor[1],
			                            this.strokeColor[2],
			                            this.strokeOpacity);
			con.Stroke();
		}
		
		public static bool IsRectangle(XmlNode node)
		{
			return node.LocalName.Equals("rect");
		}
	}
}
