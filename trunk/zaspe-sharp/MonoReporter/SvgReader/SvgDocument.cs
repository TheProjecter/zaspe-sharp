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
using System.Collections.Generic;
using System.Xml;

namespace SvgReader
{
	public class SvgDocument
	{
		private string xmlDocumentFileName;
		private XmlDocument xmlDocument;
		
		private double pageWidth;
		private double pageHeight;
		
		public SvgDocument(string xmlDocumentFileName)
		{
			this.xmlDocumentFileName = xmlDocumentFileName;
			
			this.xmlDocument = new XmlDocument();
			this.xmlDocument.Load(this.xmlDocumentFileName);
			
			this.pageWidth = -1;
			this.pageHeight = -1;
		}
		
		public double PageWidth
		{
			get {
				if (this.pageWidth > 0)
					return this.pageWidth;
				
				XmlNode svgNode = this.xmlDocument.GetElementsByTagName("svg")[0];
				string attributeValue = this.GetAttributeValueFromNode(svgNode, "width");
				
				string widthValue;
				if (attributeValue.EndsWith("mm")) {
					widthValue = attributeValue.Substring(0, attributeValue.Length - 2);
					this.pageWidth = Utils.DoubleParse(widthValue);
				}
				else {
					double tmp = Utils.DoubleParse(attributeValue);
					tmp = Utils.PixelToMm(tmp);
					
					this.pageWidth = tmp;
				}
				
				return this.pageWidth;
			}
		}
		
		public double PageHeight
		{
			get {
				if (this.pageHeight > 0)
					return this.pageHeight;
				
				XmlNode svgNode = this.xmlDocument.GetElementsByTagName("svg")[0];
				string attributeValue = this.GetAttributeValueFromNode(svgNode, "height");
				
				string heightValue;
				if (attributeValue.EndsWith("mm")) {
				    heightValue = attributeValue.Substring(0, attributeValue.Length - 2);
					this.pageHeight = Utils.DoubleParse(heightValue);
				}
				else {
					double tmp = Utils.DoubleParse(attributeValue);
					tmp = Utils.PixelToMm(tmp);
					
					this.pageHeight = tmp;
				}
				
				return this.pageHeight;
			}
		}
		
		public Rectangle[] Rectangles
		{
			get {
				List<Rectangle> rectangles = new List<Rectangle>();
				XmlNodeList rectanglesList = this.xmlDocument.GetElementsByTagName("rect");
				
				Rectangle aRectangle;
				foreach (XmlNode rectNode in rectanglesList)
				{
					string id = this.GetAttributeValueFromNode(rectNode, "id");
					string width = this.GetAttributeValueFromNode(rectNode, "width");
					string height = this.GetAttributeValueFromNode(rectNode, "height");
					string x = this.GetAttributeValueFromNode(rectNode, "x");
					string y = this.GetAttributeValueFromNode(rectNode, "y");
					string lineWidth = this.GetAttributeValueFromNode(rectNode, "style");
					
					aRectangle = new Rectangle(id,
					                           Utils.PixelToMm(Utils.DoubleParse(width)),
					                           Utils.PixelToMm(Utils.DoubleParse(height)),
					                           Utils.PixelToMm(Utils.DoubleParse(x)),
					                           Utils.PixelToMm(Utils.DoubleParse(y)));
					
					rectangles.Add(aRectangle);
				}
				
				return rectangles.ToArray();
			}
		}
		
		private string GetAttributeValueFromNode(XmlNode node, string attributeName)
		{
			foreach (XmlAttribute anAttribute in node.Attributes) {
				if (!anAttribute.Name.Equals(attributeName))
					continue;
				
				return anAttribute.Value;
			}
			
			throw new Exception("'" + attributeName + "'" + " attribute not found.");
		}
		
		private string GetValueFromMultiValuatedAttribute(XmlNode node, string attributeName, string subOption)
		{
			return null;
		}
	}
}
