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
using Glade;
using Gtk;

using ZaspeSharp.Persons;

namespace ZaspeSharp.GUI
{
	public class ModifyPerson : AddPerson
	{
		[Widget]
		Button btnOkClose;
		
		[Widget]
		Button btnOkAdd;
		
		[Widget]
		Gtk.HButtonBox dialogButtons;
		
		private Person person;
		private TreeIter personIter;
		
		public ModifyPerson(Gtk.Window parent, ListStore personsModel,
		                    Person aPerson, TreeIter personIter)
			: base (parent, personsModel)
		{
			this.personIter = personIter;
			
			this.dlgAddPerson.Title = "Modificar persona";
			
			this.dialogButtons.Remove(this.btnOkClose);
			this.btnOkAdd.Label = "Guardar";
			
			// Person data
			this.entryName.Text = aPerson.Name;
			this.entrySurname.Text = aPerson.Surname;
			this.entryDNI.Text = aPerson.Dni == 0 ? "" : aPerson.Dni.ToString();
			
			this.cmbSex.Active = aPerson.IsMan ? 0 : 1;
			
			this.entryAddress.Text = aPerson.Address;
			this.entryCity.Text = aPerson.City;
			this.entryLandPhone.Text = aPerson.LandPhoneNumber;
			this.entryMobilePhone.Text = aPerson.MobileNumber;
			this.entryEMail.Text = aPerson.EMail;
			
			this.entryCommunity.Text = aPerson.Community;
			this.chkIsActive.Active = aPerson.IsActive;
			
			if (!aPerson.BirthdayDate.Equals(DateTime.MinValue)) {
				this.spbtnDay.Value = aPerson.BirthdayDate.Day;
				this.cmbMonth.Active = aPerson.BirthdayDate.Month - 1;
			}
			
			this.chkIsDataComplete.Active = aPerson.IsDataComplete;
			
			this.person = aPerson;
		}
		
		// This is, in fact, the Save button
		public new void OnOkAddClicked(object o, EventArgs args)
		{
			int dni = this.GetDNI();
			
			int day = (int)this.spbtnDay.Value;
			int month = this.cmbMonth.Active + 1;
			DateTime birthday;
			
			try {
				birthday = new DateTime(2000, month, day);
			}
			catch (Exception) {
				birthday = DateTime.MinValue;
			}
			
			try {
				// Basic data
				this.person.Name = this.entryName.Text.Trim();
				this.person.Surname = this.entrySurname.Text.Trim();
				this.person.Dni = dni;
				this.person.IsMan = this.cmbSex.Active == 0 ? true : false;
				
				// Contact data
				this.person.Address = this.entryAddress.Text.Trim();
				this.person.City = this.entryCity.Text.Trim();
				this.person.LandPhoneNumber = this.entryLandPhone.Text.Trim();
				this.person.MobileNumber = this.entryMobilePhone.Text.Trim();
				this.person.EMail = this.entryEMail.Text.Trim();
				
				// Other data
				this.person.Community = this.entryCommunity.Text.Trim();
				this.person.IsActive = this.chkIsActive.Active;
				this.person.BirthdayDate = birthday;
				this.person.IsDataComplete = this.chkIsDataComplete.Active;
			}
			catch (Exception ex) {
				this.ShowErrorMessage(ex);
				return;
			}
			
			// Save data to database
			this.person.Persist();
			
//			MainWindow.mainWindowInstance.PersonChanged();
			
			this.dlgAddPerson.Respond(ResponseType.Close);
		}
	}
}
