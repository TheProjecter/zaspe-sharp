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
using System.Xml;

namespace SvgReader.Shapes
{
	public class Table
	{
		private string id;
		private int numberOfColumns = 0;
		private ArrayList rows;
		
		private Hashtable lastRowAdded;
		private bool firstTextAlreadyAdded = false;
		
		// tableNode is the "g" node in the svg file
		public Table(XmlNode tableNode)
		{
			if (!tableNode.LocalName.Equals("g"))
				throw new Exception("tableNode is not a 'g' node");
			
			this.id = Utils.GetAttributeValueFromNode(tableNode, "id");
			
			this.rows = new ArrayList();
			this.lastRowAdded = new Hashtable();
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
		
		public ArrayList Rows
		{
			get { return this.rows; }
		}
		
		public void AddRow(Text[] texts)
		{
			if (texts.Length != this.numberOfColumns)
				throw new Exception("Number of columns wrong");
			
			/// If this is the first row added, we replace the actual one, because
			/// it's only the definition of the colums
			if (!this.firstTextAlreadyAdded) {
				foreach (Text t in texts) {
					t.Y = ((Text)((Hashtable)this.rows[0])[t.Id]).Y;
				}
				
				this.rows.Clear();
				this.firstTextAlreadyAdded = true;
			}
			
			Hashtable newRow = new Hashtable();
			
			foreach (Text aText in texts)
				newRow[aText.Id] = aText;
			
			this.rows.Add(newRow);
		}
		
		public void AddRow(ArrayList texts)
		{
			Text[] array = (Text[])texts.ToArray(typeof(Text));
			this.AddRow(array);
		}
		
		public Text[] LastRowAdded
		{
			get {
				Hashtable lastRow = (Hashtable)this.rows[this.rows.Count - 1];
				ArrayList result = new ArrayList();
				
				foreach (Text aText in lastRow.Values)
					result.Add(aText);
				
				return (Text[])result.ToArray(typeof(Text));
			}
		}
	}
}
