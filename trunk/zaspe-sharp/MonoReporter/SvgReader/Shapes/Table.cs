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
using System.Collections.Generic;
using System.Xml;

namespace SvgReader.Shapes
{
	public class Table
	{
		private string id;
		private int numberOfColumns = 0;
		private List<Dictionary<string, Text>> rows;
		
		// tableNode is the "g" node in the svg file
		public Table(XmlNode tableNode)
		{
			if (!tableNode.LocalName.Equals("g"))
				throw new Exception("tableNode is not a 'g' node");
			
			this.id = Utils.GetAttributeValueFromNode(tableNode, "id");
			
			this.rows = new List<Dictionary<string, Text>>();
			Dictionary<string, Text> firstRow = new Dictionary<string,Text>();
			this.rows.Add(firstRow);
			
			foreach (XmlNode childNode in tableNode.ChildNodes) {
				if (childNode.LocalName.Equals("text")) {
					Text aText = new Text(childNode);
					firstRow[aText.Id] = aText;
					
					this.numberOfColumns++;
				}
			}
		}
		
		public string Id
		{
			get { return this.id; }
		}
		
//		public Text[] Rows
//		{
//			get { return this.rows.ToArray(); }
//			
//			set {
//				this.rows.Clear();
//				//this.rows.AddRange(value);
//			}
//		}
		
		public void AddRow(params Text[] texts)
		{
			if (texts.Length != this.numberOfColumns)
				throw new Exception("Number of columns wrong");
			
			Dictionary<string, Text> newRow = new Dictionary<string,Text>();
			
			foreach (Text aText in texts)
				newRow[aText.Id] = aText;
			
			this.rows.Add(newRow);
		}
	}
}
