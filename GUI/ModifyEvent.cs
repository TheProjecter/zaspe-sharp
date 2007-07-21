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
	public class ModifyEvent : AddEvent
	{
		[Widget]
		Button btnOkClose;
		
		[Widget]
		Button btnOkAdd;
		
		[Widget]
		Gtk.HButtonBox dialogButtons;
		
		private Event anEvent;
		
		protected string errorMessageTitle = "Error al modificar el evento";
		
		public ModifyEvent(Gtk.Window parent, Event anEvent) : base(parent, false)
		{
			this.dlgAddEvent.Title = "Modificar evento";
			
			this.dialogButtons.Remove(this.btnOkClose);
			this.btnOkAdd.Label = "Guardar";
			
			// Event data
			this.spbtnDay.Value = anEvent.Date.Day;
			this.cmbMonth.Active = anEvent.Date.Month - 1;
			this.spbtnYear.Value = anEvent.Date.Year;
			this.spbtnHour.Value = anEvent.Date.Hour;
			this.spbtnMinute.Value = anEvent.Date.Minute;
			
			this.cmbEventTypes.Active = anEvent.IdEventType - 1;
			
			this.entryName.Text = anEvent.Name;
			this.textviewGoals.Buffer.Text = anEvent.Goals;
			this.textviewObservations.Buffer.Text = anEvent.Observations;
			
			this.anEvent = anEvent;
			
			this.dlgAddEvent.ShowAll();
		}
		
		// This is, in fact, the Save button
		public new void OnOkAddClicked(object o, EventArgs args)
		{
			int hour = this.spbtnHour.ValueAsInt;
			int minute = this.spbtnMinute.ValueAsInt;
			int day = this.spbtnDay.ValueAsInt;
			int month = this.cmbMonth.Active + 1;
			int year = this.spbtnYear.ValueAsInt;
			
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
	}
}
