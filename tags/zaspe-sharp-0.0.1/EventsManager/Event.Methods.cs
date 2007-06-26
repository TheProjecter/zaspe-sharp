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

namespace ZaspeSharp.Events
{
	public partial class Event
	{
		#region Other methods
		public override string ToString()
		{
			return (name + " (" + date.ToString("dd/mm/yyyy H:mm") + ")");
		}
		
		public override bool Equals(object obj)
		{
			if (! (obj is Event))
				return false;
			
			Event theOtherEvent = (Event)obj;
			
			if (this.date.Equals(theOtherEvent.date)
			    && this.name.Equals(theOtherEvent.name)
			    && this.goals.Equals(theOtherEvent.goals)
			    && this.observations.Equals(theOtherEvent.observations)
			    && this.eventType.Equals(theOtherEvent.eventType))
				
				return true;

			return false;
		}
		#endregion
	}
}
