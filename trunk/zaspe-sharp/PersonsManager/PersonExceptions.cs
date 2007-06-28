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

namespace ZaspeSharp.Persons
{
	public class PersonException : SystemException
	{
		public PersonException() : base("Hubo un error con alguna Persona") {}
		public PersonException(string strErr) : base(strErr) {}
	}
	
	public class PersonExistsException : PersonException
	{
		public PersonExistsException() : base("Ya existe una persona con ese DNI") {}
		public PersonExistsException(string strErr) : base(strErr) {}
	}
	
	public class PersonDoesNotExistException : PersonException
	{
		public PersonDoesNotExistException() : base("No existe una persona con ese DNI") {}
		public PersonDoesNotExistException(string strErr) : base(strErr) {}
	}
}
