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
using System.Text.RegularExpressions;
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
		string community;
		ArrayList instruments;
		bool is_active;
		
		// If person's data is complete
		bool is_data_complete;
		
		// To check email syntax. See static constructor.
		private static Regex email_regex;
		#endregion
		
		#region Constructors
		public Person(int dni, string surname, string name, bool is_man,
		               DateTime birthday_date, string address, string city,
		               string land_phone_number, string mobile_number, string email,
		               string community, bool is_active, bool is_data_complete)
		{
			this.instruments = new ArrayList();
			
			this.Dni = dni;
			this.Surname = surname;
			this.Name = name;
			this.IsMan = is_man;
			this.BirthdayDate = birthday_date;
			this.Address = address;
			this.City = city;
			this.LandPhoneNumber = land_phone_number;
			this.MobileNumber = mobile_number;
			this.EMail = email;
			this.Community = community;
			this.IsActive = is_active;
			this.IsDataComplete = is_data_complete;
		}
		
		static Person() {
			email_regex = new Regex(@"\w+\@\w+\.\w+");
		}
		#endregion
		
		public override bool Equals (object o)
		{
			if (!(o is Person))
				return false;
			
			Person person = (Person)o;
			
			if (this.id == person.id)
				return true;
			
			return false;
		}

	}
}
