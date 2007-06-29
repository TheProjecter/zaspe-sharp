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
using System.Collections;
using Gentle.Framework;

namespace ZaspeSharp.Persons
{
	/// <summary>
	/// Description of Persona.
	/// </summary>
	[TableName("persons")]
	public partial class Person : Persistent
	{
		#region User Variables
		[TableColumn("id", NotNull=true), PrimaryKey(AutoGenerated=true)]
		private int id;
		
		// Basic data
		private int dni;
		private string surname;
		private string name;
		private bool is_man;
		private DateTime birthday_date;
		
		// Address
		private string address;
		private string city;
		
		// Contact
		private string land_phone_number;
		private string mobile_number;
		private string email;
		
		// Chorus related
		string comunity;
		ArrayList instruments;
		bool is_active;
		
		// If person's data is complete
		bool is_data_complete;
		#endregion
		
		#region Constructores
		/// <summary>
		/// This constructor should be used commonly.
		/// </summary>
		public Person (int dni, string surname, string name, bool is_man,
		                bool data_is_complete) : this(dni, surname, name, is_man,
		                DateTime.MinValue, null, null, null, null, null, null, false,
		                data_is_complete) {}

		/// <summary>
		/// This constructor should be used by Gentle.NET
		/// </summary>
		public Person(int dni, string surname, string name, bool is_man,
		               DateTime birthday_date, string address, string city,
		               string land_phone_number, string mobile_number, string email,
		               string comunity, bool is_active, bool is_data_complete)
		{
			this.instruments = new ArrayList();
			
			this.dni = dni;
			this.surname = surname;
			this.Name = name;
			this.is_man = is_man;
			this.birthday_date = birthday_date;
			this.address = address;
			this.city = city;
			this.land_phone_number = land_phone_number;
			this.mobile_number = mobile_number;
			this.email = email;
			this.comunity = comunity;
			this.is_active = is_active;
			this.is_data_complete = is_data_complete;
		}
		#endregion
	}
}
