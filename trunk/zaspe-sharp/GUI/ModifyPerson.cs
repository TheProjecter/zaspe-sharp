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
using Glade;
using Gtk;

using ZaspeSharp.Persons;

namespace ZaspeSharp.GUI
{
	public class ModifyPerson
	{
		[Widget]
		Dialog dlgAddPerson;
		
#region Basic data
		[Widget]
		Entry entryName;
		
		[Widget]
		Entry entrySurname;
		
		[Widget]
		Entry entryDNI;
		
		[Widget]
		ComboBox cmbSex;
#endregion
		
#region Contact data
		[Widget]
		Entry entryAddress;
		
		[Widget]
		Entry entryCity;
		
		[Widget]
		Entry entryLandPhone;
		
		[Widget]
		Entry entryMobilePhone;
		
		[Widget]
		Entry entryEMail;
#endregion
		
#region Other data
		[Widget]
		Entry entryCommunity;
		
		[Widget]
		CheckButton chkIsActive;
		
		[Widget]
		CheckButton chkIsDataComplete;
		
		[Widget]
		SpinButton spbtnDay;
		
		[Widget]
		ComboBox cmbMonth;
#endregion
		
#region Buttons
		[Widget]
		Button btnOkClose;
		
		[Widget]
		Button btnOkAdd;
		
		[Widget]
		Button btnCancel;
#endregion
		
		[Widget]
		Gtk.HButtonBox dialogButtons;
		
		public ModifyPerson(Gtk.Window parent)
		{
			Glade.XML gxml = new Glade.XML ("add_person.glade", "dlgAddPerson", null);
			gxml.Autoconnect(this);
			
			this.dlgAddPerson.TransientFor = parent;
			
			this.dlgAddPerson.Title = "Modificar persona";
			
			this.dialogButtons.Remove(this.btnOkClose);
			this.btnOkAdd.Label = "Guardar";
			
			this.dlgAddPerson.Show();
		}
		
		public void OnCancelClicked(object o, EventArgs args)
		{
			this.dlgAddPerson.Destroy();
		}
		
		// This is, in fact, the Save button
		public void OnOkAddClicked(object o, EventArgs args)
		{
		
		}
		
		public void OnOkCloseClicked(object o, EventArgs args)
		{
		}
		
		private void Add()
		{
			PersonsManager pm = PersonsManager.Instance;
			Person aPerson = null;
			
			int dni;
			try {
				dni = Convert.ToInt32(this.entryDNI.Text);
			}
			catch (Exception) {
				throw new Exception("El DNI no está en un formato correcto. El mismo debe formarse " +
				                    "únicamente con números, y no otros caracteres como los puntos.");
			}
			
			int day = Convert.ToInt32(this.spbtnDay.Text);
			int month = this.cmbMonth.Active + 1;
			DateTime birthday = DateTime.MinValue;
			
			try {
				birthday = new DateTime(2000, month, day);
			}
			catch (Exception) {
			}
			
			aPerson = pm.AddPerson(dni, this.entrySurname.Text, this.entryName.Text,
			                       this.cmbSex.ActiveText.Equals("Hombre") ? true : false,
			                       birthday, this.entryAddress.Text,
			                       this.entryCity.Text, this.entryLandPhone.Text,
			                       this.entryMobilePhone.Text, this.entryEMail.Text,
			                       this.entryCommunity.Text, this.chkIsActive.Active,
			                       this.chkIsDataComplete.Active);
			
			// Update persons list in the main window
			MainWindow.mainWindowInstance.AddPersonToList(aPerson);
		}
	}
}
