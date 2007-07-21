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
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using Gentle.Framework;

namespace ZaspeSharp.Events
{
	public partial class EventsManager
	{
		#region Retrieve methods
		public Event Retrieve(int id)
		{
			Key key = new Key(typeof(Event), true);
			key.Add("id", id);
			
			Event anEvent = Broker.SessionBroker.TryRetrieveInstance(
				typeof(Event), key) as Event;
			
			if (anEvent != null)
				return anEvent;
			
			throw new EventDoesNotExistException();
		}
		
		public Event Retrieve(DateTime date)
		{
			// Seconds are not important
			DateTime dateWithoutSeconds = new DateTime(date.Year, date.Month, date.Day,
			                                         date.Hour, date.Minute, 0);
			
			Key key = new Key(typeof(Event), true);
			key.Add("date", dateWithoutSeconds);
			
			Event anEvent = Broker.SessionBroker.TryRetrieveInstance(
				typeof(Event), key) as Event;
			
			if (anEvent != null)
				return anEvent;
			
			throw new EventDoesNotExistException();
		}
		
		public Event[] RetrieveAll()
		{
			SqlBuilder sb = new SqlBuilder(StatementType.Select, typeof(Event));
			SqlStatement stmt = sb.GetStatement(true);
			
			IList events = ObjectFactory.GetCollection(typeof(Event), stmt.Execute());
			
			List<Event> events_result = new List<Event>();
			foreach (Event p in events)
				events_result.Add(p);
			
			return events_result.ToArray();
		}
		
		public Event[] RetrieveLast(int numberOfEvents) {			
			SqlBuilder sb = new SqlBuilder(StatementType.Select, typeof(Event));
			sb.SetRowLimit(numberOfEvents);
			sb.AddOrderByField(false, "date");
			sb.AddConstraint(Operator.LessThanOrEquals, "date", DateTime.Now);
			
			SqlStatement stmt = sb.GetStatement(true);
			
			IList events = ObjectFactory.GetCollection(typeof(Event), stmt.Execute());
			
			List<Event> events_result = new List<Event>();
			foreach (Event e in events)
				events_result.Add(e);
			
			return events_result.ToArray();
		}
		#endregion
		
		#region Entry methods
		public Event AddEvent(DateTime date, string name, EventType eventType,
			string goals, string observations) {
			
			try {
				Retrieve(date);
			}
			catch (EventDoesNotExistException) {
				Event newEvent = new Event(date, name, eventType,
				                                goals, observations);
				newEvent.Persist();
				
				return newEvent;
			}
			
			throw new EventExistsException("Se está intentando ingresar un evento que " +
			                               "ya existe en la base de datos. Cada evento " +
			                               "tiene una fecha y hora únicos.");
		}
		#endregion
		
		#region Deletion methods
		public void EliminarEvento(DateTime fecha)
		{
			Event unEvento;
			
			try {
				unEvento = Retrieve(fecha);
			}
			catch (EventDoesNotExistException) {
				throw new EventDoesNotExistException("Se está intentando eliminar un evento que" + 
					" no existe en la base de datos.");
			}
			
			unEvento.Remove();
		}
		#endregion
		
		#region Otros métodos
		[Conditional("DEBUG")]
		public void LimpiarCache()
		{
			Gentle.Common.CacheManager.Clear();
		}
		#endregion
	}
}
