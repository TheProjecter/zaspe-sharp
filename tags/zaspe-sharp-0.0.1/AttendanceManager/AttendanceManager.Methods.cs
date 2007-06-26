/* 
   Zaspe# - Attendance management
   Copyright (C) 2006, 2007 Milton Pividori

   Zaspe# is free software; you can redistribute it and/or
   modify it under the terms of the GNU General Public License
   as published by the Free Software Foundation; either version 2
   of the License, or (at your option) any later version.

   Zaspe# is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with Zaspe#; if not, write to the Free Software
   Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

using System;
using System.Collections.Generic;

using ZaspeSharp.Persons;
using ZaspeSharp.Events;

using Gentle.Framework;

namespace ZaspeSharp.Attendances
{
	public partial class AttendanceManager
	{
		#region Retrieve methods
		public Attendance Retrieve(Person aPerson, Event anEvent) {
			// I look for the person through DNI
			Key key = new Key(typeof(Person), true);
			key.Add("idPerson", aPerson.Id);
			key.Add("eventDate", anEvent.Date);
			
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
			try {
				Retrieve(aPerson, anEvent);
			}
			catch (AttendanceDoesNotExistException) {
				Attendance newAttendance = new Attendance(aPerson.Id, anEvent.Date);
				newAttendance.Persist();
				
				return newAttendance;
			}
			
			throw new AttendanceExistsException("Se est� intentando ingresar una asistencia que" +
					" ya existe en la base de datos.");
		}
		#endregion
	}
}
