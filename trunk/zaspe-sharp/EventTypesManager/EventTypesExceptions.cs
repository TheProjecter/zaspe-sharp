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
	public class EventTypeException : System.Exception
	{
		public EventTypeException() : base("Hubo un error con alg�n tipo de evento") {}
		public EventTypeException(string strErr) : base(strErr) {}
	}
	
	public class EventTypeExistsException : EventTypeException
	{
		public EventTypeExistsException() : base("Ya existe un tipo de evento con ese id") {}
		public EventTypeExistsException(string strErr) : base(strErr) {}
	}
	
	public class EventTypeDoesNotExistException : EventTypeException
	{
		public EventTypeDoesNotExistException() : base("No existe un tipo evento con ese id") {}
		public EventTypeDoesNotExistException(string strErr) : base(strErr) {}
	}
}