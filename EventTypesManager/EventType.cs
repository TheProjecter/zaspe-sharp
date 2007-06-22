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
using Gentle.Framework;

namespace ZaspeSharp.Events
{
	[TableName("event_types")]
	public partial class EventType : Persistent
	{
		#region User variables
		[TableColumn("id", NotNull=true), PrimaryKey(AutoGenerated=true)]
		private int id;
		
		private string name;
		#endregion
		
		#region Constructors
		public EventType(string name) {
			this.name = name;
		}
		#endregion
	}
}
