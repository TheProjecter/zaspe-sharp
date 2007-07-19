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

namespace ZaspeSharp.Events
{
	public partial class Event
	{
		#region Other methods
		public override string ToString()
		{
			return (name + " (" + date.ToString("dd/mm/yyyy H:mm") + ")");
		}
		
		public override bool Equals (object o)
		{
			if (! (o is Event))
				return false;
			
			Event theOtherEvent = (Event)o;
			
			if (this.id == theOtherEvent.id
			    && this.date.Equals(theOtherEvent.date)
			    && this.name.Equals(theOtherEvent.name)
			    && this.eventType.Equals(theOtherEvent.eventType)
			    && this.goals.Equals(theOtherEvent.goals)
			    && this.observations.Equals(theOtherEvent.observations)) {
			
				//Console.WriteLine(this.date.ToString() + " igual a " + theOtherEvent.date.ToString());
				
				return true;
			}
			
			//Console.WriteLine(this.date.ToString() + " distinto a " + theOtherEvent.date.ToString());
			
			return false;
		}
		#endregion
	}
}
