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
	public partial class Person
	{
		#region Basic data
		public int Id {
			get { return this.id; }
			set { this.id = value; }
		}
		
		[TableColumn("dni", NotNull=true)]//, PrimaryKey(AutoGenerated=false)]
		public int Dni
		{
			get { return this.dni; }
			
			set {
				// DNI must be positive bigger than zero
				if (value <= 0)
					throw new Exception("El DNI debe ser mayor a cero.");
				
				this.dni = value;
			}
		}
		
		[TableColumn("surname", NotNull=true)]
		public string Surname
		{
			get { return this.surname; }
			
			set {
				// Surname must not be blank
				if (value.Trim().Equals(""))
					throw new Exception("El apellido no puede estar en blanco");
				
				this.surname = value;
			}
		}
		
		[TableColumn("name", NotNull=true)]
		public string Name
		{
			get { return this.name; }
			
			set {
				// Name must not be blank
				if (value.Trim().Equals(""))
					throw new Exception("El nombre no puede estar en blanco");
				
				this.name = value;
			}
		}
		
		[TableColumn("is_man", NotNull=true)]
		public bool IsMan
		{
			get { return this.is_man; }
			set { this.is_man = value; }
		}
		
		[TableColumn("birthday_date", NotNull=false)]
		public DateTime BirthdayDate
		{
			get { return this.birthday_date; }
			
			set {
				if (value.Year == 1800)
					this.birthday_date = DateTime.MinValue;
				else
					this.birthday_date = value;
			}
		}
		#endregion
		
		#region Address
		[TableColumn("address", NotNull=false)]
		public string Address
		{
			get { return this.address; }
			set { this.address = value; }
		}
		
		[TableColumn("city", NotNull=false)]
		public string City
		{
			get { return this.city; }
			set { this.city = value; }
		}
		#endregion
		
		#region Contact
		[TableColumn("land_phone_number", NotNull=false)]
		public string LandPhoneNumber
		{
			get { return this.land_phone_number; }
			set { this.land_phone_number = value; }
		}
		
		[TableColumn("mobile_number", NotNull=false)]
		public string MobileNumber
		{
			get	{ return this.mobile_number; }
			set { this.mobile_number = value; }
		}
		
		[TableColumn("email", NotNull=false)]
		public string EMail
		{
			get {return this.email;}
			
			set {
				if (value == null || value.Equals("") || email_regex.IsMatch(value))
				    this.email = value;
				else
					throw new Exception("El email es inválido.");
			}
		}
		#endregion
		
		#region Chorus related
		[TableColumn("comunity", NotNull=false)]
		public string Community
		{
			get {return this.community;}
			set {this.community = value;}
		}
		
		public ArrayList Instruments
		{
			get {return this.instruments;}
		}
		
		[TableColumn("is_active", NotNull=true)]
		public bool IsActive
		{
			get {return this.is_active;}
			set { this.is_active = value; }
		}
		#endregion
		
		#region Other data
		[TableColumn("is_data_complete", NotNull=true)]
		public bool IsDataComplete
		{
			get { return this.is_data_complete; }
			set { this.is_data_complete = value; }
		}
		#endregion
	}
}
