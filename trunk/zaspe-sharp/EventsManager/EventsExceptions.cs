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
	public class EventException : System.Exception
	{
		public EventException() : base("Hubo un error con algún evento") {}
		public EventException(string strErr) : base(strErr) {}
	}
	
	public class EventExistsException : EventException
	{
		public EventExistsException() : base("Ya existe un evento con esa fecha y ese nombre") {}
		public EventExistsException(string strErr) : base(strErr) {}
	}
	
	public class EventDoesNotExistException : EventException
	{
		public EventDoesNotExistException() : base("No existe un evento con esa fecha y ese nombre") {}
		public EventDoesNotExistException(string strErr) : base(strErr) {}
	}
}
