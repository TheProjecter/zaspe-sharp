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

namespace SvgReader.Shapes
{
	public class Section
	{
		private string id;
		private List<IShape> shapesInSection;
		private List<DataRow> datarowsToDraw;
		private int originalNumberOfDataRows = 0;
		
		internal Section()
		{
			this.shapesInSection = new List<IShape>();
		}
		
		public Section(XmlNode sectionNode) : this()
		{
			if (!sectionNode.LocalName.Equals("g"))
				throw new Exception("sectionNode is not a 'g' node");
			
			this.id = Utils.GetAttributeValueFromNode(sectionNode, "id");
			
			//this.shapesInSection = new List<IShape>();
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
		                  Dictionary<string, DataTable> dataTables,
		                  double maxYToDraw,
		                  bool reallyDraw)
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
					
					/* If the table has no data, don't draw it */
					if (!dataTables.ContainsKey(aTable.Id))
						continue;
					
					// DataTable corresponding to the Table object
					DataTable dataTable = dataTables[aTable.Id];
					
					if (this.datarowsToDraw == null) {
						this.datarowsToDraw = new List<DataRow>();
						
						// Add all DataRows
						foreach (DataRow dr in dataTable.Rows)
							this.datarowsToDraw.Add(dr);
						
						this.originalNumberOfDataRows = this.datarowsToDraw.Count;
					}
					
					Console.WriteLine("Procesando un DataTable...");
					
					Text textTemp = null;
					List<Text> textRow = new List<Text>(aTable.NumberOfColumns);
					List<DataRow> datarowsDrawn = new List<DataRow>();
					
					foreach (DataRow dataRow in this.datarowsToDraw) {
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
						
						/* We stop drawing rows if we have reached the end
						 * of the section (the start of the next one) */
						double heightOfLastText =
							textTemp.Y + Utils.GetPixelHeightSize(textTemp,
							                                      context.CreatePangoLayout());
						if (heightOfLastText > maxYToDraw)
							break;
						
						aTable.AddRow(textRow);
						textRow.Clear();
						
						/* I add the DataRow to remove it later.
						 * This is for multipage reports, where there is a lot
						 * of data. Next call to this function we will draw
						 * remaning shapes */
						datarowsDrawn.Add(dataRow);
					}
					
					// Remove already drawn datarows
					foreach (DataRow dr in datarowsDrawn)
						this.datarowsToDraw.Remove(dr);
				}
				
				// Draw the Shape
				if (reallyDraw)
					aShape.Draw(context);
				
				// Let's reset all tables for next iteration
				if (aShape is Table) (aShape as Table).Reset();
			}
		}
		
		/// <value>
		/// Where the section starts. This is, the shape with the min value
		/// in its "y" property
		/// </value>
		public double Y {
			get {
				double mixY = Double.MaxValue;
				
				foreach (IShape shape in this.shapesInSection)
					if (shape.Y < mixY)
						mixY = shape.Y;
				
				return mixY;
			}
		}
		
		public double Height {
			get {
				double maxY = Double.MinValue;
				
				foreach (IShape shape in this.shapesInSection)
					if (shape.Y > maxY)
						maxY = shape.Y;
				
				return (maxY - this.Y);
			}
		}
		
		public int PagesToAdd {
			get {
				if (this.datarowsToDraw == null || this.datarowsToDraw.Count == 0)
					return 0;
				
				Console.WriteLine("ONODR: " + this.originalNumberOfDataRows);
				Console.WriteLine("DRTDC: " + this.datarowsToDraw.Count);
				return (this.originalNumberOfDataRows / (this.originalNumberOfDataRows - this.datarowsToDraw.Count));
			}
		}
		
		public void ResetDataTablesToDraw()
		{
			this.datarowsToDraw = null;
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
