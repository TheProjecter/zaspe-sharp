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
				if (attributeValue.EndsWith("mm"))
					widthValue = attributeValue.Substring(0, attributeValue.Length - 2);
				else {
					double tmp = Double.Parse(attributeValue);
					tmp = tmp * 0.282258065;
					
					widthValue = tmp.ToString();
				}
				
				this.pageWidth = Double.Parse(widthValue, NumberStyles.AllowDecimalPoint);
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
				if (attributeValue.EndsWith("mm"))
				    heightValue = attributeValue.Substring(0, attributeValue.Length - 2);
				else {
					double tmp = Double.Parse(attributeValue);
					tmp = tmp * 0.282258065;
					
					heightValue = tmp.ToString();
				}
				    
				Console.WriteLine(heightValue);
				this.pageHeight = Double.Parse(heightValue, NumberStyles.AllowDecimalPoint);
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
					aRectangle = new Rectangle(this.GetAttributeValueFromNode(rectNode, "id"),
					                           this.GetAttributeValueFromNode(rectNode, "width"),
					                           this.GetAttributeValueFromNode(rectNode, "height"),
					                           this.GetAttributeValueFromNode(rectNode, "x"),
					                           this.GetAttributeValueFromNode(rectNode, "y"));
					
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
	}
}
