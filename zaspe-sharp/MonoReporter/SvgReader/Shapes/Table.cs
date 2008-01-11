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
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace SvgReader.Shapes
{
	public class Table : IShape
	{
		private string id;
		private int numberOfColumns = 0;
		private List<Dictionary<string, Text>> rows;
		
		private Dictionary<string, Text> lastRowAdded;
		private bool firstTextAlreadyAdded = false;
		
		// tableNode is the "g" node in the svg file
		public Table(XmlNode tableNode)
		{
			if (!tableNode.LocalName.Equals("g"))
				throw new Exception("tableNode is not a 'g' node");
			
			this.id = Utils.GetAttributeValueFromNode(tableNode, "id");
			
			this.rows = new List<Dictionary<string, Text>>();
			this.lastRowAdded = new Dictionary<string, Text>();
			this.rows.Add(this.lastRowAdded);
			
			foreach (XmlNode childNode in tableNode.ChildNodes) {
				if (childNode.LocalName.Equals("text")) {
					Text aText = new Text(childNode);
					this.lastRowAdded[aText.Id] = aText;
					
					this.numberOfColumns++;
				}
			}
		}
		
		public string Id
		{
			get { return this.id; }
		}
		
		public int NumberOfColumns
		{
			get { return this.numberOfColumns; }
		}
		
//		public ArrayList Rows
//		{
//			get { return this.rows; }
//		}
		
		public void AddRow(List<Text> texts)
		{
			if (texts.Count != this.numberOfColumns)
				throw new Exception("Number of columns wrong");
			
			/// If this is the first row added, we replace the actual one, because
			/// it's only the definition of the colums
			if (!this.firstTextAlreadyAdded) {
				foreach (Text t in texts) {
					t.Y = this.rows[0][t.Id].Y;
				}
				
				this.rows.Clear();
				this.firstTextAlreadyAdded = true;
			}
			
			Dictionary<string, Text> newRow = new Dictionary<string, Text>();
			
			foreach (Text aText in texts)
				newRow[aText.Id] = aText;
			
			this.rows.Add(newRow);
		}
		
//		public void AddRow(List<Text> texts)
//		{
//			Text[] array = (Text[])texts.ToArray(typeof(Text));
//			this.AddRow(array);
//		}
		
		public IList<Text> LastRowAdded
		{
			get {
				Dictionary<string, Text> lastRow =
					this.rows[this.rows.Count - 1];
				List<Text> result = new List<Text>();
				
				foreach (Text aText in lastRow.Values)
					result.Add(aText);
				
				return result;
			}
		}
		
		public void Draw (Gtk.PrintContext context)
		{
			foreach (Dictionary<string, Text> aRow in this.rows) {
				foreach (Text aField in aRow.Values)
					aField.Draw(context);
			}
		}
		
		public static bool IsTable(XmlNode node)
		{
			return (node.LocalName.Equals("g") &&
			        Utils.NodeContainsOnlyNode(node, "text"));
		}
	}
}
