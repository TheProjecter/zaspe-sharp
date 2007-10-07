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

namespace SvgReader.Shapes
{
	public class Text
	{
		public const double X_FIX = 0.25;
		public const double Y_FIX = 0.86;
		
		private string id;
		
		private string textValue;
		private double size;
		private string style;
		private string family;
		private string alignment;
		
		private double x;
		private double y;
		
		private double[] color;
		private double opacity;
		
		public Text(XmlNode textNode)
		{
			// textNode.FirstChild is tspan node
			
			this.id = Utils.GetAttributeValueFromNode(textNode, "id");
			this.textValue = textNode.FirstChild.FirstChild.Value;
			
			string x = Utils.GetAttributeValueFromNode(textNode, "x");
			this.x = Utils.PixelToMm(Utils.NormalizeShapeX(x));
			
			string y = Utils.GetAttributeValueFromNode(textNode, "y");
			this.y = Utils.PixelToMm(Utils.NormalizeShapeY(y));
			
			try {
				// Look for "font-size" in tspan
				string size = Utils.GetValueFromMultiValuatedAttribute(textNode.FirstChild, "style", "font-size");
				this.size = Utils.DoubleParse(size.Substring(0, size.Length - 2));
			}
			catch (AttributeNotFoundException) {
				// If it failed, look for it in text
				string size = Utils.GetValueFromMultiValuatedAttribute(textNode, "style", "font-size");
				this.size = Utils.DoubleParse(size.Substring(0, size.Length - 2));
			}
			catch (SubOptionNotFoundException) {
				this.size = 14.0;
			}
			
			try {
				this.style = Utils.GetValueFromMultiValuatedAttribute(textNode, "style", "font-style");
			}
			catch (SubOptionNotFoundException) {
				this.style = "normal";
			}
			
			try {
				this.family = Utils.GetValueFromMultiValuatedAttribute(textNode, "style", "font-family");
			}
			catch (SubOptionNotFoundException) {
				this.family = "Times New Roman";
			}
			
			try {
				this.alignment = Utils.GetValueFromMultiValuatedAttribute(textNode, "style", "text-align");
			}
			catch (SubOptionNotFoundException) {
				this.alignment = "start";
			}
			
			try {
				string color = Utils.GetValueFromMultiValuatedAttribute(textNode, "style", "fill");
				this.color = Utils.FromHexColorToRGBA(color);
			}
			catch (SubOptionNotFoundException) {
				this.color = new double[] {0, 0, 0};
			}
			
			try {
				string opacity = Utils.GetValueFromMultiValuatedAttribute(textNode, "style", "fill-opacity");
				this.opacity = Utils.DoubleParse(opacity);
			}
			catch (SubOptionNotFoundException) {
				this.opacity = 1.0;
			}
		}
		
		public string Id
		{
			get { return this.id; }
			set { this.id = value; }
		}
		
		public string TextValue
		{
			get { return this.textValue; }
			set { this.textValue = value; }
		}
		
		public double Size
		{
			get { return this.size; }
			set { this.size = value; }
		}
		
		public string Style
		{
			get { return this.style; }
			set { this.style = value; }
		}
		
		public string Family
		{
			get { return this.family; }
			set { this.family = value; }
		}
		
		public string Alignment
		{
			get { return this.alignment; }
			set { this.alignment = value; }
		}
		
		public string FontDescription
		{
			get {
				return (this.family + " " + Convert.ToInt32(this.size * 0.8));
			}
		}
		
		public double[] Color
		{
			get { return this.color; }
			set { this.color = value; }
		}
		
		public double Opacity
		{
			get { return this.opacity; }
			set { this.opacity = value; }
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
