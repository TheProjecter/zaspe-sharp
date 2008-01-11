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
using System.Data;
using System.Collections.Generic;
using System.Xml;

namespace Shapes
{
	public class Section
	{
		private string id;
		public List<IShape> shapesInSection;
		
		public Section(XmlNode sectionNode)
		{
			if (!sectionNode.LocalName.Equals("g"))
				throw new Exception("sectionNode is not a 'g' node");
			
			this.id = Utils.GetAttributeValueFromNode(sectionNode, "id");
			
			this.shapesInSection = new List<IShape>();
			IShape shape = null;
			
			foreach(XmlNode childNode in sectionNode.ChildNodes) {
				// Check the type of the node
				if (Table.IsTable(childNode))
					shape = new Table(childNode);
				
				else if (Line.IsLine(childNode))
					shape = new Line(childNode);
				
				else if (Rectangle.IsRectangle(childNode))
					shape = new Rectangle(childNode);
				
				else if (Text.IsText(childNode))
					shape = new Text(childNode);
				
				else
					throw new Exception("No other type of shape available");
				
				this.shapesInSection.Add(shape);
			}
		}
		
		public void Draw (Gtk.PrintContext context,
		                  Dictionary<string, string> data,
		                  Dictionary<string, DataTable> dataTables)
		{
			foreach (IShape aShape in this.shapesInSection) {
				if (aShape is Text) {
					Text aText = (Text)aShape;
					Console.WriteLine(aText.TextValue);
					
					if (data.ContainsKey(aText.Id))
						aText.TextValue = data[aText.Id];
				}
				
				else if (aShape is Table) {
					Table aTable = (Table)aShape;
					
					if (!dataTables.ContainsKey(aTable.Id))
						continue;
					
					Console.WriteLine("Procesando un DataTable...");
					
					DataTable dataTable = dataTables[aTable.Id];
					Text textTemp;
					List<Text> textRow = new List<Text>(aTable.NumberOfColumns);
					
					foreach (DataRow dataRow in dataTable.Rows) {
						foreach (Text aText in aTable.LastRowAdded) {
							Console.WriteLine("   Text: " + aText.TextValue);
							
							textTemp = (Text)aText.Clone();
							textTemp.TextValue =
								dataRow[textTemp.Id].ToString();
							textTemp.Y = textTemp.Y +
								Utils.GetPixelHeightSize(textTemp,
								                         context.CreatePangoLayout());
							
							textRow.Add(textTemp);
						}
						
						aTable.AddRow(textRow);
						textRow.Clear();
					}
				}
				
				aShape.Draw(context);
			}
		}
		
		/// <summary>
		/// And XmlNode is a Section if it's a 'g' node, and it has children
		/// </summary>
		public static bool IsSection(XmlNode node)
		{
			return (node.LocalName.Equals("g") &&
			        node.ChildNodes.Count > 0);
		}
	}
}
