//  Zaspe# - Attendance management
//  Copyright (C) 2006, 2007 Milton Pividori
//
//  This file is part of Zaspe#.
//
//  Zaspe# is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 3 of the License, or
//  (at your option) any later version.
//
//  Zaspe# is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with Zaspe#.  If not, see <http://www.gnu.org/licenses/>.

using System;
using Gentle.Framework;

namespace ZaspeSharp.Events
{
	/// <summary>
	/// Description of Evento.
	/// </summary>
	[TableName("events")]
	public partial class Event : Persistent
	{
		#region User variables
		int id;
		DateTime date;
		string name;
		
		EventType eventType;
		
		string goals;
		string observations;
		#endregion
		
		#region Constructors
		public Event(DateTime date, string name, int id_event_type,
		             string goals, string observations)
			: this(date, name, EventTypesManager.Instance.Retrieve(id_event_type),
			       goals, observations) {}
		
		public Event(DateTime date, string name, EventType eventType,
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
