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
using System.Text.RegularExpressions;
using Cairo;

namespace SvgReader.Shapes
{
	public class Line : IShape
	{
		private string id;
		
		// Start point
		private double x1;
		private double y1;
		
		// End point
		private double x2;
		private double y2;
		
		private double[] strokeColor;
		private double strokeOpacity;
		private double strokeWidth;
		
		public Line(XmlNode lineNode)
		{
			this.id = Utils.GetAttributeValueFromNode(lineNode, "id");
			
			string dAttribute = Utils.GetAttributeValueFromNode(lineNode, "d");
			
			Regex regex = new Regex(@"\d+\.\d");
			
//			Console.WriteLine("Matches:");
//			foreach (Match m in regex.Matches(dAttribute))
//				Console.WriteLine(m.Value);
			
			Match match = regex.Match(dAttribute);
			
			this.x1 = Utils.PixelToMm(Utils.NormalizeShapeX(match.Value));
			match = match.NextMatch();
			
			this.y1 = Utils.PixelToMm(Utils.NormalizeShapeY(match.Value));
			match = match.NextMatch();
			
			this.x2 = Utils.PixelToMm(Utils.NormalizeShapeX(match.Value));
			match = match.NextMatch();
			
			this.y2 = Utils.PixelToMm(Utils.NormalizeShapeY(match.Value));
			
			try {
				string strokeWidth = Utils.GetValueFromMultiValuatedAttribute(lineNode, "style", "stroke-width");
				this.strokeWidth = Utils.DoubleParseAndPixelToMm(strokeWidth);
			}
			catch (SubOptionNotFoundException) {
				this.strokeWidth = Utils.PixelToMm(1.0);
			}
			
			try {
				string strokeColor = Utils.GetValueFromMultiValuatedAttribute(lineNode, "style", "stroke");
				this.strokeColor = Utils.FromHexColorToRGBA(strokeColor);
			}
			catch (SubOptionNotFoundException) {
				this.strokeColor = new double[] {0, 0, 0};
			}
			
			try {
				string strokeOpacity = Utils.GetValueFromMultiValuatedAttribute(lineNode, "style", "stroke-opacity");
				this.strokeOpacity = Utils.DoubleParse(strokeOpacity);
			}
			catch (SubOptionNotFoundException) {
				this.strokeOpacity = 1.0;
			}
		}
		
		public string Id
		{
			get { return this.id; }
		}
		
//		public double X1
//		{
//			get { return this.x1; }
//			set { this.x1 = value; }
//		}
//		
//		public double Y1
//		{
//			get { return this.y1; }
//			set { this.y1 = value; }
//		}
//		
//		public double X2
//		{
//			get { return this.x2; }
//			set { this.x2 = value; }
//		}
//		
//		public double Y2
//		{
//			get { return this.y2; }
//			set { this.y2 = value; }
//		}
		
//		public double[] StrokeColor
//		{
//			get { return this.strokeColor; }
//			set { this.strokeColor = value; }
//		}
		
//		public double StrokeOpacity
//		{
//			get { return this.strokeOpacity; }
//			set { this.strokeOpacity = value; }
//		}
		
//		public double StrokeWidth
//		{
//			get { return this.strokeWidth; }
//			set { this.strokeWidth = value; }
//		}
		
		public void Draw (Gtk.PrintContext context)
		{
			Cairo.Context con = context.CairoContext; 
			
			con.LineWidth = this.strokeWidth;
			con.Color = new Cairo.Color(this.strokeColor[0],
			                            this.strokeColor[1],
			                            this.strokeColor[2],
			                            this.strokeOpacity);
			
			con.MoveTo(this.x1, this.y1);
			con.LineTo(this.x2, this.y2);
			
			con.Stroke();
		}
		
		public static bool IsLine(XmlNode node)
		{
			return node.LocalName.Equals("path");
		}
	}
}
