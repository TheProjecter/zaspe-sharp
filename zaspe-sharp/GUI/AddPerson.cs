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
using Glade;
using Gtk;

using ZaspeSharp.Persons;

namespace ZaspeSharp.GUI
{
	public class AddPerson
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
		Entry entryComunity;
		
		[Widget]
		CheckButton chkIsActive;
		
		[Widget]
		CheckButton chkIsDataComplete;
		
		[Widget]
		SpinButton spbtnDay;
		
		[Widget]
		ComboBox cmbMonth;
		#endregion
		
		public AddPerson(Gtk.Window parent)
		{
			Glade.XML gxml = new Glade.XML ("add_person.glade", "dlgAddPerson", null);
			gxml.Autoconnect(this);
			
			this.dlgAddPerson.TransientFor = parent;
			this.cmbSex.Active = 0;
			this.cmbMonth.Active = 0;
			
			this.dlgAddPerson.Show();
		}
		
		public void OnCancelClicked(object o, EventArgs args)
		{
			this.dlgAddPerson.Destroy();
		}
		
		public void OnOkAddClicked(object o, EventArgs args)
		{
		
		}
		
		public void OnOkCloseClicked(object o, EventArgs args)
		{
			try {
				this.Add();
			}
			
			catch (PersonExistsException ex)
			{
				MessageDialog md = new MessageDialog(this.dlgAddPerson, DialogFlags.Modal,
					                                     MessageType.Error, ButtonsType.Ok,
					                                     ex.Message);
				md.Run();
				md.Destroy();
				
				return;
			}
			
			catch (Exception)
			{
				MessageDialog md = new MessageDialog(this.dlgAddPerson, DialogFlags.Modal,
					                                     MessageType.Error, ButtonsType.Ok,
					                                     "Hubo un error desconocido al ingresar la persona");
				md.Run();
				md.Destroy();
				
				return;
			}
			
			this.dlgAddPerson.Destroy();
		}
		
		private void Add()
		{
			PersonsManager pm = PersonsManager.Instance;
			Person aPerson = null;
			
			aPerson = pm.AddPerson(Convert.ToInt32(this.entryDNI.Text),
				this.entrySurname.Text, this.entryName.Text,
				this.cmbSex.ActiveText.Equals("Hombre") ? true : false,
				this.chkIsDataComplete.Active);
			
			// Add more data to aPerson
			aPerson.Address = this.entryAddress.Text;
			aPerson.City = this.entryCity.Text;
			aPerson.LandPhoneNumber = this.entryLandPhone.Text;
			aPerson.MobileNumber = this.entryMobilePhone.Text;
			aPerson.EMail = this.entryEMail.Text;
			aPerson.Community = this.entryComunity.Text;
			aPerson.IsActive = this.chkIsActive.Active;
			
			// Birthday date
			int day = Convert.ToInt32(this.spbtnDay.Text);
			int month = this.cmbMonth.Active + 1;
			aPerson.BirthdayDate = new DateTime(2000, month, day);
			
			// Persist the new data added above
			aPerson.Persist();
			
			// Update persons list in the main window
			MainWindow.mainWindowInstance.AddPersonToList(aPerson);
		}
	}
}
