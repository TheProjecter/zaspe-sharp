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
using Glade;
using Gtk;

using ZaspeSharp.Events;

namespace ZaspeSharp.GUI
{
	public class ModifyEvent
	{
		[Widget]
		Dialog dlgAddEvent;
		
		[Widget]
		SpinButton spbtnDay;
		
		[Widget]
		ComboBox cmbMonth;
		
		[Widget]
		SpinButton spbtnHour;
		
		[Widget]
		SpinButton spbtnMinute;
		
		[Widget]
		ComboBox cmbEventTypes;
		
		[Widget]
		Entry entryName;
		
		[Widget]
		TextView textviewGoals;
		
		[Widget]
		TextView textviewObservations;
		
#region Buttons
		[Widget]
		Button btnOkClose;
		
		[Widget]
		Button btnOkAdd;
#endregion
		
		[Widget]
		Gtk.HButtonBox dialogButtons;
		
		private Event anEvent;
		
		private string lastGeneratedEventName = "";
		
		public ModifyEvent(Gtk.Window parent, Event anEvent)
		{
			Glade.XML gxml = new Glade.XML ("add_event.glade", "dlgAddEvent", null);
			gxml.Autoconnect(this);
			
			this.dlgAddEvent.TransientFor = parent;
			this.dlgAddEvent.Title = "Modificar evento";
			
			this.dialogButtons.Remove(this.btnOkClose);
			this.btnOkAdd.Label = "Guardar";
			
			// Event data
			this.spbtnDay.Value = anEvent.Date.Day;
			this.cmbMonth.Active = anEvent.Date.Month - 1;
			this.spbtnHour.Value = anEvent.Date.Hour;
			this.spbtnMinute.Value = anEvent.Date.Minute;
			
			// TODO
			// Load event types
			EventTypesManager etm = EventTypesManager.Instance;
			
			this.cmbEventTypes.RemoveText(0);
			
			foreach (EventType anEventType in etm.RetrieveAll()) {
				this.cmbEventTypes.AppendText(anEventType.Name);
			}
			
			this.cmbEventTypes.Active = anEvent.IdEventType - 1;
			
			this.entryName.Text = anEvent.Name;
			this.textviewGoals.Buffer.Text = anEvent.Goals;
			this.textviewObservations.Buffer.Text = anEvent.Observations;
			
			this.anEvent = anEvent;
			
			this.dlgAddEvent.ShowAll();
		}
		
		private bool EventNameCanBeChanged()
		{
			if (this.entryName.Text.Equals(this.lastGeneratedEventName) ||
			    this.entryName.Text.Equals(""))
				return true;
			
			return false;
		}
		
		private string GenerateEventName()
		{
			return (this.cmbEventTypes.ActiveText + " " +
			        this.spbtnDay.Value.ToString() +
			        " de " + this.cmbMonth.ActiveText);
		}
		
		// This is, in fact, the Save button
		public void OnOkAddClicked(object o, EventArgs args)
		{
			int hour = (int)this.spbtnHour.Value;
			int minute = (int)this.spbtnMinute.Value;
			int day = (int)this.spbtnDay.Value;
			int month = this.cmbMonth.Active + 1;
			int year = DateTime.Now.Year + (month < DateTime.Now.Month ? 1 : 0);
			DateTime date = DateTime.MinValue;
			
			try {
				date = new DateTime(year, month, day, hour, minute, 0);
			}
			catch (Exception) {
				this.ShowErrorMessage("La fecha para el evento no es válida.");
			}
			
			try {
				this.anEvent.Date = date;
				this.anEvent.IdEventType = this.cmbEventTypes.Active + 1;
				this.anEvent.Name = this.entryName.Text;
				this.anEvent.Goals = this.textviewGoals.Buffer.Text;
				this.anEvent.Observations = this.textviewObservations.Buffer.Text;
			}
			catch (Exception ex) {
				this.ShowErrorMessage(ex.Message);
				return;
			}
					
			// Save changes to database
			this.anEvent.Persist();
			
			MainWindow.mainWindowInstance.EventChanged();
			
			this.dlgAddEvent.Destroy();
		}
		
		public void OnOkCloseClicked(object o, EventArgs args)
		{
		}
		
		public void OnEventTypesChanged(object o, EventArgs args)
		{
			if (this.cmbEventTypes.Active < 0 || this.cmbMonth.Active < 0)
				return;
			
			if (this.cmbEventTypes.ActiveText.Equals("Otro") ||
			     !this.EventNameCanBeChanged())
				return;
			
			this.entryName.Text =
				this.GenerateEventName();
			
			this.lastGeneratedEventName = this.entryName.Text;
		}
		
		public void OnDayValueChanged(object o, EventArgs args)
		{
			this.OnEventTypesChanged(null, null);
		}
		
		public void OnCancelClicked(object o, EventArgs args)
		{
			this.dlgAddEvent.Destroy();
		}
			
		private void ShowErrorMessage(string errorMsg)
		{
			MessageDialog md = new MessageDialog(this.dlgAddEvent, DialogFlags.Modal,
			                                     MessageType.Error, ButtonsType.Ok,
			                                     errorMsg);
			md.Title = "Error al modificar datos del evento";
			
			md.Run();
			md.Destroy();
		}
		
		private void ShowErrorMessage(Exception ex)
		{
			this.ShowErrorMessage(ex.Message);
		}
	}
}
