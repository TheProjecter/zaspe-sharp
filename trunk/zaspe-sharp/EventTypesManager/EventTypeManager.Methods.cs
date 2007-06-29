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
using System.Collections;
using System.Diagnostics;

using Gentle.Framework;

namespace ZaspeSharp.Events
{
	/// <summary>
	/// Description of ControladorTiposEventoMetodos.
	/// </summary>
	public partial class EventTypesManager
	{
		#region Retrieve methods
		public EventType Retrieve(int id)
		{
			Key key = new Key(typeof(EventType), true);
			key.Add("id", id);
			
			EventType anEventType = Broker.SessionBroker.TryRetrieveInstance(
				typeof(EventType), key) as EventType;
			
			if (anEventType != null)
				return anEventType;
			
			throw new EventTypeDoesNotExistException();
		}
		
		public EventType Retrieve(string name)
		{
			SqlBuilder sb = new SqlBuilder(StatementType.Select, typeof(EventType));
			sb.AddConstraint(Operator.Equals, "name", name);
			SqlStatement stmt = sb.GetStatement();
			
			EventType anEventType = null;
			try {
				anEventType =
					(EventType)ObjectFactory.GetInstance(typeof(EventType), stmt.Execute());
			}
			catch (Gentle.Common.GentleException) {
				throw new EventTypeDoesNotExistException();
			}
			
			return anEventType;
		}
		
		public EventType[] RetrieveAll()
		{
			SqlBuilder sb = new SqlBuilder(StatementType.Select, typeof(EventType));
			SqlStatement stmt = sb.GetStatement(true);
			
			EventType[] eventTypes = (EventType[])ObjectFactory.GetCollection(typeof(EventType), stmt.Execute());
			return eventTypes;
		}
		#endregion
		
		#region Entry methods
		public EventType AddEventType(string name)
		{
			try {
				this.Retrieve(name);
			}
			catch (EventTypeDoesNotExistException) {
				EventType newEventType = new EventType(name);
				newEventType.Persist();
				
				return newEventType;
			}
			
			throw new EventTypeExistsException("Se está intentando ingresar un tipo de evento que" +
					" ya existe en la base de datos.");
		}
		#endregion
		
		#region Deletion methods
		public void DeleteEventType(string name)
		{
			EventType anEventType;
			
			try {
				anEventType = Retrieve(name);
			}
			catch (EventTypeDoesNotExistException) {
				throw new EventTypeDoesNotExistException("Se está intentando eliminar un tipo de evento que" + 
					" no existe en la base de datos.");
			}
			
			anEventType.Remove();
		}
		#endregion
		
		#region Other methods
		[Conditional("DEBUG")]
		public void CleanCache()
		{
			Gentle.Common.CacheManager.Clear();
		}
		#endregion
	}
}
