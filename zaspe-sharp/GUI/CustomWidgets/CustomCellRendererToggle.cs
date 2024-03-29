/*
    Zaspe# - Attendance management
    Copyright (C) 2006, 2007 Milton Pividori

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using Gtk;

using ZaspeSharp.Events;

namespace ZaspeSharp.GUI
{
	public class CustomCellRendererToggle : CellRendererToggle
	{
		private int columnNumber;
		private Event anEvent;
		
		public CustomCellRendererToggle(int columnNumber, Event anEvent) : base()
		{
			this.columnNumber = columnNumber;
			this.anEvent = anEvent;
		}
		
		public int ColumnNumber
		{
			get { return this.columnNumber; }
			set { this.columnNumber = value; }
		}
		
		public Event Event
		{
			get { return this.anEvent; }
			set { this.anEvent = value; }
		}
	}
}
