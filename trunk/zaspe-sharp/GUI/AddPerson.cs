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
		
		public AddPerson(Gtk.Window parent)
		{
			Glade.XML gxml = new Glade.XML ("add_person.glade", "dlgAddPerson", null);
			gxml.Autoconnect(this);
			
			this.dlgAddPerson.TransientFor = parent;
			this.cmbSex.Active = 0;
			//this.cmbMonth.Active = 0;
			
			this.dlgAddPerson.Show();
		}
		
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
			
			// entryName has focus to quick adding without the mouse
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
		
		private void ShowErrorMessage(Exception ex)
		{
			MessageDialog md = new MessageDialog(this.dlgAddPerson, DialogFlags.Modal,
			                                     MessageType.Error, ButtonsType.Ok,
			                                     ex.Message);
			md.Title = "Error al ingresar la persona";
			
			md.Run();
			md.Destroy();
		}
	}
}
