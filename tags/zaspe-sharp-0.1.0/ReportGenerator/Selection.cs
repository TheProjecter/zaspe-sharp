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
using System.Collections.Generic;

using ZaspeSharp.Persons;
using ZaspeSharp.Events;

namespace ZaspeSharp.ReportGenerator
{
	internal class Selection
	{
		private List<Person> persons;
		private List<Event> events;
			
		public Selection()
		{
			this.persons = new List<Person>();
			this.events = new List<Event>();
		}
		
		internal void AddPersons(params Person[] personsToAdd)
		{
			foreach (Person p in personsToAdd)
				if (!this.persons.Contains(p))
					this.persons.Add(p);
		}
		
		internal void AddEvents(params Event[] eventsToAdd)
		{
			foreach (Event e in eventsToAdd)
				if (!this.events.Contains(e))
					this.events.Add(e);
		}
		
		internal void ClearPersons()
		{
			this.persons.Clear();
		}
		
		internal void ClearEvents()
		{
			this.events.Clear();
		}
		
		public List<Person> Persons {
			get {
				return persons;
			}
		}

		public List<Event> Events {
			get {
				return events;
			}
		}
	}
}
