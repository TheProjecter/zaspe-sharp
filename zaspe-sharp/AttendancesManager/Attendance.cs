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

using ZaspeSharp.Persons;
using ZaspeSharp.Events;

using Gentle.Framework;

namespace ZaspeSharp.Attendances
{
	/// <summary>
	/// Description of Asistencia.
	/// </summary>
	[TableName("attendances")]
	public partial class Attendance : Persistent
	{
		#region User Variables
		private int person_id;
		private Person aPerson;
		
		private DateTime eventDate;
		private Event anEvent;
		#endregion
		
		#region Constructors
		public Attendance(int idPerson, DateTime eventDate)
		{
			this.person_id = person_id;
			this.eventDate = eventDate;
			
			this.aPerson = PersonsManager.Instance.Retrieve(this.person_id);
			this.anEvent = EventsManager.Instance.Retrieve(this.eventDate);
		}
		#endregion
	}
}