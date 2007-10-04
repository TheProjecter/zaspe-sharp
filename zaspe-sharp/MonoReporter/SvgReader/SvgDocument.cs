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
using System.Globalization;
using System.Collections.Generic;
using System.Xml;

using SvgReader.Shapes;

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
				string attributeValue = Utils.GetAttributeValueFromNode(svgNode, "width");
				
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
				string attributeValue = Utils.GetAttributeValueFromNode(svgNode, "height");
				
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
				
				foreach (XmlNode rectNode in rectanglesList)
					rectangles.Add(new Rectangle(rectNode));
				
				return rectangles.ToArray();
			}
		}
		
		public Text[] Texts
		{
			get {
				List<Text> texts = new List<Text>();
				XmlNodeList textsList = this.xmlDocument.GetElementsByTagName("text");
				
				foreach (XmlNode textNode in textsList)
					texts.Add(new Text(textNode));
				
				return texts.ToArray();
			}
		}
	}
}
