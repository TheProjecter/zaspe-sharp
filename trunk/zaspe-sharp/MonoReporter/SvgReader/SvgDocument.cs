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
		
//		public Rectangle[] Rectangles
//		{
//			get {
//				List<Rectangle> rectangles = new List<Rectangle>();
//				XmlNode svgNode = this.xmlDocument.GetElementsByTagName("svg")[0];
//				
//				foreach (XmlNode xmlNode in svgNode.ChildNodes) {
//					if (xmlNode.LocalName.Equals("rect"))
//						rectangles.Add(new Rectangle(xmlNode));
//				}
//				
//				return rectangles.ToArray();
//			}
//		}
		
//		public Text[] Texts
//		{
//			get {
//				List<Text> texts = new List<Text>();
//				XmlNode svgNode = this.xmlDocument.GetElementsByTagName("svg")[0];
//				
//				foreach (XmlNode xmlNode in svgNode.ChildNodes) {
//					if (xmlNode.LocalName.Equals("text"))
//						texts.Add(new Text(xmlNode));
//				}
//				
//				return texts.ToArray();
//			}
//		}
		
//		public Line[] Lines
//		{
//			get {
//				List<Line> lines = new List<Line>();
//				XmlNode svgNode = this.xmlDocument.GetElementsByTagName("svg")[0];
//				
//				foreach (XmlNode xmlNode in svgNode.ChildNodes) {
//					if (xmlNode.LocalName.Equals("path"))
//						lines.Add(new Line(xmlNode));
//				}
//				
//				return lines.ToArray();
//			}
//		}
		
//		public Table[] Tables
//		{
//			get {
//				List<Table> tables = new List<Table>();
//				XmlNode svgNode = this.xmlDocument.GetElementsByTagName("svg")[0];
//				
//				foreach (XmlNode xmlNode in svgNode.ChildNodes) {
//					/// Check if node is a "g" node, and if only contains "text"
//					/// nodes, because a Table only consists of "text" nodes.
//					if (xmlNode.LocalName.Equals("g") && this.NodeContainsOnlyNode(xmlNode, "text"))
//						tables.Add(new Table(xmlNode));
//				}
//				
//				return tables.ToArray();
//			}
//		}
		
		private Section GetSection(string sectionName)
		{
			XmlNode svgNode =
				this.xmlDocument.GetElementsByTagName("svg")[0];
			
			foreach (XmlNode xmlNode in svgNode.ChildNodes) {
				string inkscapeLabel = "";
				try {
					inkscapeLabel =
						Utils.GetAttributeValueFromNode(xmlNode,
						                                "inkscape:label");
				}
				catch (AttributeNotFoundException)
				{
				}
				
				if (Section.IsSection(xmlNode) &&
				    inkscapeLabel.Equals(sectionName))
					return new Section(xmlNode);
			}
			
			throw new SectionNotFoundException("Section " + sectionName +
			                                   "not " + "found");
		}
		
		public Section ReportHeaderSection {
			get {
				return this.GetSection("ReportHeader");
			}
		}
		
		public Section PageDetailSection {
			get {
				return this.GetSection("PageDetail");
			}
		}
		
		public Section ReportFooterSection {
			get {
				return this.GetSection("ReportFooter");
			}
		}
	}
}
