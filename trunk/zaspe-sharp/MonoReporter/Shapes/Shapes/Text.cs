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

namespace Shapes
{
	public class Text : ICloneable, IShape
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
		
		private Text()
		{
		}
		
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
//		
//		public double Size
//		{
//			get { return this.size; }
//			set { this.size = value; }
//		}
		
//		public string Style
//		{
//			get { return this.style; }
//			set { this.style = value; }
//		}
//		
//		public string Family
//		{
//			get { return this.family; }
//			set { this.family = value; }
//		}
//		
//		public string Alignment
//		{
//			get { return this.alignment; }
//			set { this.alignment = value; }
//		}
		
		internal string FontDescription
		{
			get {
				return (this.family + " " + Convert.ToInt32(this.size * 0.8));
			}
		}
		
//		public double[] Color
//		{
//			get { return this.color; }
//			set { this.color = value; }
//		}
//		
//		public double Opacity
//		{
//			get { return this.opacity; }
//			set { this.opacity = value; }
//		}
		
		internal double X
		{
			get { return this.x; }
			set { this.x = value; }
		}
		
		internal double Y
		{
			get { return this.y; }
			set { this.y = value; }
		}
		
		public object Clone ()
		{
			Text newText = new Text();
			
			newText.id = this.id;
			newText.textValue = this.textValue;
			newText.size = this.size;
			newText.style = this.style;
			newText.family = this.family;
			newText.alignment = this.alignment;
			
			newText.x = this.x;
			newText.y = this.y;
			
			newText.color = (double[])this.color.Clone();
			newText.opacity = this.opacity;
			
			return newText;
		}
		
		public void Draw (Gtk.PrintContext context)
		{
			Cairo.Context con = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout();
			
			layout.SetText(this.textValue);
			layout.FontDescription = Pango.FontDescription.FromString(this.FontDescription);
			
			int pixelSizeWidth, pixelSizeHeight;
			layout.GetPixelSize(out pixelSizeWidth, out pixelSizeHeight);
			
			Console.WriteLine("   Text value: " + this.textValue);
			Console.WriteLine("   Pixel size width: " + pixelSizeWidth);
			Console.WriteLine("   Pixel size height: " + pixelSizeHeight);
			Console.WriteLine("   Text X position: " + this.x);
			Console.WriteLine("   Text Y position: " + this.y);
			
			double x_offset = pixelSizeWidth*0; // Disabled
			double y_offset = pixelSizeHeight * 0.83;
			
			Console.WriteLine("width: " + x_offset);
			Console.WriteLine("height: " + y_offset);
			
			/// Alignment
			/// If text.Alignment == "center", then x coordinate is indicating
			/// the center, not the left side. That's way we add pixelSizeWidth/2
			/// to x_offset
			if (this.alignment.Equals("center")) {
				layout.Alignment = Pango.Alignment.Center;
				x_offset += pixelSizeWidth/2;
			}
			
			con.MoveTo(this.x - x_offset, this.y - y_offset);
			Pango.CairoHelper.ShowLayout(con, layout);
			
//			con.MoveTo(text.X, text.Y - y_offset);
//			con.LineTo(text.X + 100, text.Y - y_offset);
//			con.Stroke();
//			                  
//			con.MoveTo(text.X, 38.4376957919563);
//			con.LineTo(text.X + 100, 38.4376957919563);
//			con.Stroke();
		}
		
		public static bool IsText(XmlNode node)
		{
			return node.LocalName.Equals("text");
		}
	}
}
