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
using System.Collections.Generic;

using ZaspeSharp.Persons;
using ZaspeSharp.Events;

using Gentle.Framework;

namespace ZaspeSharp.Attendances
{
	public partial class AttendancesManager
	{
#region Retrieve methods
		private Attendance Retrieve(Person aPerson, Event anEvent) {
			Key key = new Key(true);
			key.Add("id_person", aPerson.Id);
			key.Add("id_event", anEvent.Id);
			
			Attendance anAttendance =
				Broker.SessionBroker.TryRetrieveInstance(typeof(Attendance), key) as Attendance;
			
			// If found, return it
			if (anAttendance != null)
				return anAttendance;
			
			return null;
		}
		
		public bool Attended(Person aPerson, Event anEvent) {
			if (this.Retrieve(aPerson, anEvent) != null)
				return true;
			else
				return false;
		}
		
		public IList<Attendance> RetrieveLastEventsAttended(Person aPerson, int numberOfEvents) {
			SqlBuilder sb = new SqlBuilder(StatementType.Select, typeof(Attendance));
			sb.SetRowLimit(numberOfEvents);
			sb.AddOrderByField(false, "date");
			
			SqlStatement stmt = sb.GetStatement(true);
			
			Attendance[] attendances =
				(Attendance[])ObjectFactory.GetCollection(typeof(Attendance), stmt.Execute());
			
			return attendances;
		}
		
		public IList<Attendance> RetrieveAll() {
			SqlBuilder sb = new SqlBuilder(StatementType.Select, typeof(Attendance));
			SqlStatement stmt = sb.GetStatement(true);
			
			Attendance[] attendances =
				(Attendance[])ObjectFactory.GetCollection(typeof(Attendance), stmt.Execute());
			
			return attendances;
		}
#endregion
		
#region Entry methods
		public Attendance AddAttendance(Person aPerson, Event anEvent) {
			if (Retrieve(aPerson, anEvent) == null) {
				Attendance newAttendance = new Attendance(aPerson.Id, anEvent.Id);
				newAttendance.Persist();
				
				return newAttendance;
			}
			
			throw new AttendanceExistsException("Se está intentando ingresar una asistencia que" +
					" ya existe en la base de datos.");
		}
#endregion
		
#region Deletion methods
		public void RemoveAttendance(Person aPerson, Event anEvent) {
			Attendance at = Retrieve(aPerson, anEvent);
			
			if (at == null)
				throw new Exception("La asistencia que se intenta eliminar no existe en la base de datos.");
			
			at.Remove();
		}
#endregion
	}
}
