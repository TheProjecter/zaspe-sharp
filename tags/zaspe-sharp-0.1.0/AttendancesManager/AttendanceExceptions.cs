//  Zaspe# - Attendance management
//  Copyright (C) 2006, 2007 Milton Pividori
//
//  This file is part of Zaspe#.
//
//  Zaspe# is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 3 of the License, or
//  (at your option) any later version.
//
//  Zaspe# is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with Zaspe#.  If not, see <http://www.gnu.org/licenses/>.

using System;

namespace ZaspeSharp.Attendances
{
	public class AttendanceException : System.Exception
	{
		public AttendanceException() : base("Hubo un error con alguna asistencia") {}
		public AttendanceException(string strErr) : base(strErr) {}
	}
	
	public class AttendanceExistsException : AttendanceException
	{
		public AttendanceExistsException() : base("Ya existe una asistencia con esa persona y ese evento") {}
		public AttendanceExistsException(string strErr) : base(strErr) {}
	}
	
	public class AttendanceDoesNotExistException : AttendanceException
	{
		public AttendanceDoesNotExistException() : base("No existe una asistencia con esa persona y ese evento") {}
		public AttendanceDoesNotExistException(string strErr) : base(strErr) {}
	}
}
