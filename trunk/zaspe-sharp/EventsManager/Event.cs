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
using Gentle.Framework;

namespace ZaspeSharp.Events
{
	// This would be changed to be generic
	public enum Types {
		MISA = 1,
		ENSAYO = 2,
		OTRO = 3
	}
	
	/// <summary>
	/// Description of Evento.
	/// </summary>
	[TableName("events")]
	public partial class Event : Persistent
	{
		#region User variables
		DateTime date;
		string name;
		int eventType;
		string goals;
		string observations;
		#endregion
		
		#region Constructors
		public Event(DateTime date, string name, Types eventType)
			: this(date, name, (int)eventType, null, null) {}
		
		public Event(DateTime date, string name, int eventType,
			string goals, string observations) {
			
			this.date = date;
			this.name = name;
			this.eventType = eventType;
			this.goals = goals;
			this.observations = observations;
		}
		#endregion
	}
}
