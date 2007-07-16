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
	public class AddPerson
	{
		[Widget]
		protected Dialog dlgAddPerson;
		
		#region Basic data
		[Widget]
		protected Entry entryName;
		
		[Widget]
		protected Entry entrySurname;
		
		[Widget]
		protected Entry entryDNI;
		
		[Widget]
		protected ComboBox cmbSex;
		#endregion
		
		#region Contact data
		[Widget]
		protected Entry entryAddress;
		
		[Widget]
		protected Entry entryCity;
		
		[Widget]
		protected Entry entryLandPhone;
		
		[Widget]
		protected Entry entryMobilePhone;
		
		[Widget]
		protected Entry entryEMail;
		#endregion
		
		#region Other data
		[Widget]
		protected Entry entryCommunity;
		
		[Widget]
		protected CheckButton chkIsActive;
		
		[Widget]
		protected CheckButton chkIsDataComplete;
		
		[Widget]
		protected SpinButton spbtnDay;
		
		[Widget]
		protected ComboBox cmbMonth;
		#endregion
		
		// Constructor for subclasses of this class
		protected AddPerson(Gtk.Window parent, bool showDialog)
		{
			Glade.XML gxml = new Glade.XML ("add_person.glade", "dlgAddPerson", null);
			gxml.Autoconnect(this);
			
			this.dlgAddPerson.TransientFor = parent;
			this.cmbSex.Active = 0;
			
			if (showDialog)
				this.dlgAddPerson.ShowAll();
		}
		
		// Constructor for users of this class
		public AddPerson(Gtk.Window parent) : this(parent, true) {}
		
		public void OnCancelClicked(object o, EventArgs args)
		{
			this.dlgAddPerson.Destroy();
		}
		
		public void OnOkAddClicked(object o, EventArgs args)
		{
			try {
				this.Add();
			}
			catch(Exception ex)
			{
				this.ShowErrorMessage(ex);
				
				return;
			}
			
			// Show message saying person was successfully added
			MessageDialog md = new MessageDialog(this.dlgAddPerson, DialogFlags.Modal,
			                                     MessageType.Info, ButtonsType.Ok,
			                                     "La persona fue ingresada correctamente.");
			md.Title = "Ingreso de persona correcto";
			
			md.Run();
			md.Destroy();
			
			// Clean controls to add a new person
			this.entryName.Text = "";
			this.entrySurname.Text = "";
			this.entryDNI.Text = "";
			this.cmbSex.Active = 0;
			
			this.entryAddress.Text = "";
			this.entryCity.Text = "";
			this.entryLandPhone.Text = "";
			this.entryMobilePhone.Text = "";
			this.entryEMail.Text = "";
			
			this.entryCommunity.Text = "";
			this.chkIsActive.Active = false;
			this.spbtnDay.Value = 1;
			this.cmbMonth.Active = -1;
			this.chkIsDataComplete.Active = false;
			
			// entryName has focus to quick adding without using the mouse
			this.entryName.HasFocus = true;
		}
		
		public void OnOkCloseClicked(object o, EventArgs args)
		{
			try {
				this.Add();
			}
			catch (Exception ex)
			{
				this.ShowErrorMessage(ex);
				return;
			}
			
			this.dlgAddPerson.Destroy();
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
			
			int day = (int)this.spbtnDay.Value;
			int month = this.cmbMonth.Active + 1;
			DateTime birthday = DateTime.MinValue;
			
			try {
				birthday = new DateTime(2000, month, day);
			}
			catch (Exception) {
			}
			
			aPerson = pm.AddPerson(dni, this.entrySurname.Text.Trim(), this.entryName.Text.Trim(),
			                       this.cmbSex.ActiveText.Equals("Hombre") ? true : false,
			                       birthday, this.entryAddress.Text.Trim(),
			                       this.entryCity.Text.Trim(), this.entryLandPhone.Text.Trim(),
			                       this.entryMobilePhone.Text.Trim(), this.entryEMail.Text.Trim(),
			                       this.entryCommunity.Text.Trim(), this.chkIsActive.Active,
			                       this.chkIsDataComplete.Active);
			
			// Update persons list in the main window
			MainWindow.mainWindowInstance.AddPersonToList(aPerson);
		}
		
		protected void ShowErrorMessage(string errorMsg)
		{
			MessageDialog md = new MessageDialog(this.dlgAddPerson, DialogFlags.Modal,
			                                     MessageType.Error, ButtonsType.Ok,
			                                     errorMsg);
			md.Title = "Error con la persona";
			
			md.Run();
			md.Destroy();
		}
		
		protected void ShowErrorMessage(Exception ex)
		{
			this.ShowErrorMessage(ex.Message);
		}
	}
}
