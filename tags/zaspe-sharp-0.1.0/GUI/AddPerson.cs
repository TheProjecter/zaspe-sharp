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
		
		protected ListStore personsModel;
		
		// Constructor for subclasses of this class
		public AddPerson(Window parent, ListStore personsModel)
		{
			Glade.XML gxml = new Glade.XML ("add_person.glade", "dlgAddPerson",
			                                null);
			gxml.Autoconnect(this);
			
			this.dlgAddPerson.TransientFor = parent;
			this.cmbSex.Active = 0;
			
			this.personsModel = personsModel;
		}
		
		// Constructor for users of this class
//		public AddPerson(Gtk.Window parent, ListStore personsModel)
//			: this(parent, personsModel)
//		{
//		}
		
		public void Run()
		{
			while (this.dlgAddPerson.Run() != (int)ResponseType.Close);
			
			this.dlgAddPerson.Destroy();
		}
		
		protected virtual void OnResponse (object o, Gtk.ResponseArgs args)
		{
			if (args.ResponseId.Equals(Gtk.ResponseType.DeleteEvent))
				this.dlgAddPerson.Respond(ResponseType.Close);
		}
		
		public void OnCancelClicked(object o, EventArgs args)
		{
			this.dlgAddPerson.Respond(ResponseType.Close);
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
			
			this.dlgAddPerson.Respond(ResponseType.Close);
		}
		
		private void Add()
		{
			PersonsManager pm = PersonsManager.Instance;
			Person aPerson = null;
			
			int dni = this.GetDNI();
			
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
		
		protected int GetDNI()
		{
			if (this.entryDNI.Text.Trim().Equals("0"))
				throw new Exception("El DNI no puede ser cero.");
			
			if (!this.entryDNI.Text.Equals("")) {
				try {
					return Convert.ToInt32(this.entryDNI.Text.Trim());
				}
				catch (Exception) {
					throw new Exception("El DNI no está en un formato correcto. El mismo debe formarse " +
					                    "únicamente con números, y no otros caracteres como los puntos.");
				}
			}
			else
				return 0;
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
