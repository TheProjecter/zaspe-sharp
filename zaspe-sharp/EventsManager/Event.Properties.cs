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
	public partial class Event 
	{
		[TableColumn("id", NotNull=true), PrimaryKey(AutoGenerated=true)]
		public int Id
		{
			get { return this.id; }
			set { this.id = value; }
		}
		
		[TableColumn("date", NotNull=true)]//, PrimaryKey(AutoGenerated=false)]
		public DateTime Date
		{
			get { return this.date; }
			set {
				
				try {
					Event anEvent = EventsManager.Instance.Retrieve(value);
					
					if (!this.Equals(anEvent))
						throw new Exception("Otro evento ya tiene esa fecha. Las fechas son " +
						                    "únicas para los eventos.");
				}
				catch (EventDoesNotExistException) {
				}
				
				this.date = value;
			}
		}
		
		[TableColumn("name", NotNull=true)]
		public string Name
		{
			get { return this.name; }
			set	{
				if (value.Equals(""))
					throw new Exception("El nombre del evento no puede ser nulo.");
				
				this.name = value;
			}
		}
		
		[TableColumn("id_event_type", NotNull=true)]
		public int IdEventType {
			get { return this.eventType.Id; }
			set {
				EventTypesManager etm = EventTypesManager.Instance;
				
				this.eventType = etm.Retrieve(value);
			}
		}
		
		public EventType EventType {
			get { return this.eventType; }
			set { this.eventType = value; }
		}
		
		[TableColumn("goals", NotNull=false)]
		public string Goals
		{
			get { return this.goals; }
			set { this.goals = value; }
		}
		
		[TableColumn("observations", NotNull=false)]
		public string Observations
		{
			get { return this.observations; }
			set { this.observations = value; }
		}
	}
}
