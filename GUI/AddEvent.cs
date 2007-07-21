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

using ZaspeSharp.Events;

namespace ZaspeSharp.GUI
{
	public class AddEvent
	{
		[Widget]
		protected Dialog dlgAddEvent;
		
		[Widget]
		protected ComboBox cmbEventTypes;
		
		[Widget]
		protected Entry entryName;
		
		[Widget]
		protected SpinButton spbtnDay;
		
		[Widget]
		protected ComboBox cmbMonth;
		
		[Widget]
		protected SpinButton spbtnYear;
		
		[Widget]
		protected SpinButton spbtnHour;
		
		[Widget]
		protected SpinButton spbtnMinute;
		
		[Widget]
		protected TextView textviewGoals;
		
		[Widget]
		protected TextView textviewObservations;
		
		protected string lastGeneratedEventName = "";
		
		// Constructor for subclasses of this class
		protected AddEvent(Window parent, bool showDialog)
		{
			Glade.XML gxml = new Glade.XML ("add_event.glade", "dlgAddEvent", null);
			gxml.Autoconnect(this);
			
			this.dlgAddEvent.TransientFor = parent;
			
			// Set actual day and month
			this.spbtnDay.Value = DateTime.Now.Day;
			this.cmbMonth.Active = DateTime.Now.Month - 1;
			
			this.spbtnYear.Adjustment.Lower = DateTime.Now.Year;
			this.spbtnYear.Adjustment.Upper = 9999;
			this.spbtnYear.Value = DateTime.Now.Year;
			
			// Load event types
			EventTypesManager etm = EventTypesManager.Instance;
			
			this.cmbEventTypes.RemoveText(0);
			
			foreach (EventType anEventType in etm.RetrieveAll()) {
				this.cmbEventTypes.AppendText(anEventType.Name);
			}
			
			if (showDialog)
				this.dlgAddEvent.ShowAll();
		}
		
		// Constructor for users of this class
		public AddEvent(Window parent) : this(parent, true) {}
		
		protected bool EventNameCanBeChanged()
		{
			if (this.entryName.Text.Equals(this.lastGeneratedEventName) ||
			    this.entryName.Text.Equals(""))
				return true;
			
			return false;
		}
		
		protected string GenerateEventName()
		{
			return (this.cmbEventTypes.ActiveText + " " +
			        this.spbtnDay.Value.ToString() +
			        " de " + this.cmbMonth.ActiveText);
		}
		
		public void OnMonthChanged(object o, EventArgs args)
		{
			/* If we are on November or Dicember, and the event's month is January
			 * or February, then the year is the next, not the actual. */
			if (DateTime.Now.Month > 10 && (this.cmbMonth.Active + 1) < 3)
				this.spbtnYear.Value = DateTime.Now.Year + 1;
			
			// Call because of event's name generation issues
			this.OnEventTypesChanged(null, null);
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
			
			this.dlgAddEvent.Destroy();
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
			
			// Show message saying event was successfully added
			MessageDialog md = new MessageDialog(this.dlgAddEvent, DialogFlags.Modal,
			                                     MessageType.Info, ButtonsType.Ok,
			                                     "El evento fue ingresado correctamente.");
			md.Title = "Ingreso de evento correcto";
			
			md.Run();
			md.Destroy();
			
			// Clean controls to add a new event
			// Set actual day and month
			this.spbtnDay.Value = DateTime.Now.Day;
			this.cmbMonth.Active = DateTime.Now.Month - 1;
			this.spbtnHour.Value = 12;
			this.spbtnMinute.Value = 0;
			
			this.cmbEventTypes.Active = -1;
			this.entryName.Text = "";
			this.textviewGoals.Buffer.Text = "";
			this.textviewObservations.Buffer.Text = "";
			
			// spbtnDay has focus to quick adding without using the mouse
			this.spbtnDay.HasFocus = true;
		}
		
		private void Add()
		{
			EventsManager em = EventsManager.Instance;
			Event anEvent = null;
			
			int hour = this.spbtnHour.ValueAsInt;
			int minute = this.spbtnMinute.ValueAsInt;
			int day = this.spbtnDay.ValueAsInt;
			int month = this.cmbMonth.Active + 1;
			int year = this.spbtnYear.ValueAsInt;
			
			DateTime date;
			
			try {
				date = new DateTime(year, month, day, hour, minute, 0);
			}
			catch (Exception) {
				throw new Exception("La fecha para el evento no es válida.");
			}
			
			if (this.cmbEventTypes.Active < 0)
				throw new Exception("Se debe escoger el tipo del evento.");
			
			EventType eventTypeSelected = EventTypesManager.Instance.Retrieve(this.cmbEventTypes.ActiveText);
			
			anEvent = em.AddEvent(date, this.entryName.Text.Trim(), eventTypeSelected,
			                      this.textviewGoals.Buffer.Text.Trim(),
			                      this.textviewObservations.Buffer.Text.Trim());
			
			// Update persons list in the main window
			MainWindow.mainWindowInstance.AddEventToList(anEvent);
		}
		
		protected void ShowErrorMessage(string errorMsg)
		{
			MessageDialog md = new MessageDialog(this.dlgAddEvent, DialogFlags.Modal,
			                                     MessageType.Error, ButtonsType.Ok,
			                                     errorMsg);
			md.Title = "Error con el evento";
			
			md.Run();
			md.Destroy();
		}
		
		protected void ShowErrorMessage(Exception ex)
		{
			this.ShowErrorMessage(ex.Message);
		}
	}
}