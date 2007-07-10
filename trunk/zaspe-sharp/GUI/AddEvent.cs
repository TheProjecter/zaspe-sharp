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
		Dialog dlgAddEvent;
		
		[Widget]
		ComboBox cmbEventTypes;
		
		[Widget]
		Entry entryName;
		
		[Widget]
		SpinButton spbtnDay;
		
		[Widget]
		ComboBox cmbMonth;
		
		[Widget]
		SpinButton spbtnHour;
		
		[Widget]
		SpinButton spbtnMinute;
		
		[Widget]
		TextView textviewGoals;
		
		[Widget]
		TextView textviewObservations;
		
		private string lastGeneratedEventName = "";
		
		public AddEvent(Window parent)
		{
			Glade.XML gxml = new Glade.XML ("add_event.glade", "dlgAddEvent", null);
			gxml.Autoconnect(this);
			
			this.dlgAddEvent.TransientFor = parent;
			
			// Set actual day and month
			this.spbtnDay.Value = DateTime.Now.Day;
			this.cmbMonth.Active = DateTime.Now.Month - 1;
			
			// Load event types
			EventTypesManager etm = EventTypesManager.Instance;
			
			this.cmbEventTypes.RemoveText(0);
			
			foreach (EventType anEventType in etm.RetrieveAll()) {
				this.cmbEventTypes.AppendText(anEventType.Name);
			}
			
			this.dlgAddEvent.ShowAll();
		}
		
		private bool EventNameCanBeChanged()
		{
			if (this.entryName.Text.Equals(this.lastGeneratedEventName) ||
			    this.entryName.Text.Equals(""))
				return true;
			
			return false;
			
//			if (this.entryName.Text.Equals(""))
//				return true;
//			
//			EventTypesManager etm = EventTypesManager.Instance;
//			
//			/* If user has written his own name for the event, do not
//			 * replace it with an automatic generated one */
//			foreach (EventType anEventType in etm.RetrieveAll()) {
//				if (this.entryName.Text.Equals(this.GenerateEventName(anEventType.Name)))
//					return true;
//			}
//			
//			return false;
		}
		
		private string GenerateEventName()
		{
			return (this.cmbEventTypes.ActiveText + " " +
			        this.spbtnDay.Value.ToString() +
			        " de " + this.cmbMonth.ActiveText);
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
			System.Console.WriteLine("Day: {0}", this.spbtnDay.Value);
			this.OnEventTypesChanged(null, null);
		}
		
		public void OnCancelClicked(object o, EventArgs args)
		{
			this.dlgAddEvent.Destroy();
		}
		
		public void OnOkCloseClicked(object o, EventArgs args)
		{
			this.Add();
			try {
				
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
		
		}
		
		private void Add()
		{
			EventsManager em = EventsManager.Instance;
			Event anEvent = null;
			
			int day = (int)this.spbtnDay.Value;
			int month = this.cmbMonth.Active + 1;
			int year = DateTime.Now.Year + (month < DateTime.Now.Month ? 1 : 0);
			DateTime date;
			
			try {
				date = new DateTime(year, month, day);
			}
			catch (Exception ex) {
				throw new Exception("La fecha para el evento no es válida.");
			}
			
			if (this.cmbEventTypes.Active < 0)
				throw new Exception("Se debe escoger el tipo del evento.");
			
			EventType eventTypeSelected = EventTypesManager.Instance.Retrieve(this.cmbEventTypes.ActiveText);
			
			anEvent = em.AddEvent(date, this.entryName.Text, eventTypeSelected,
			                      this.textviewGoals.Buffer.Text,
			                      this.textviewObservations.Buffer.Text);
			
			// Update persons list in the main window
			MainWindow.mainWindowInstance.AddEventToList(anEvent);
		}
		
		private void ShowErrorMessage(Exception ex)
		{
			MessageDialog md = new MessageDialog(this.dlgAddEvent, DialogFlags.Modal,
			                                     MessageType.Error, ButtonsType.Ok,
			                                     ex.Message);
			md.Title = "Error al ingresar el evento";
			
			md.Run();
			md.Destroy();
		}
	}
}