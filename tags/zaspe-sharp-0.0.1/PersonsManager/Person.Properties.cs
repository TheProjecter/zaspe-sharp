﻿/* 
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
		
		[TableColumn("dni", NotNull=true), PrimaryKey(AutoGenerated=false)]
		public int Dni
		{
			get { return this.dni; }
			set { this.dni = value; }
		}
		
		[TableColumn("surname", NotNull=true)]
		public string Surname
		{
			get { return this.surname; }
			set { this.surname = value; }
		}
		
		[TableColumn("name", NotNull=true)]
		public string Name
		{
			get { return this.name; }
			set { this.name = value; }
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
			set {this.email = value;}
		}
		#endregion
		
		#region Chorus related
		[TableColumn("comunity", NotNull=false)]
		public string Community
		{
			get {return this.comunity;}
			set {this.comunity = value;}
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