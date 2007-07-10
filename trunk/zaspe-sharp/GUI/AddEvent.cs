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
		
		public AddEvent(Window parent)
		{
			Glade.XML gxml = new Glade.XML ("add_event.glade", "dlgAddEvent", null);
			gxml.Autoconnect(this);
			
			this.dlgAddEvent.TransientFor = parent;
			
			// Load event types
			EventTypesManager etp = EventTypesManager.Instance;
			
			this.cmbEventTypes.RemoveText(0);
			
			foreach (EventType anEventType in etp.RetrieveAll()) {
				this.cmbEventTypes.PrependText(anEventType.Name);
			}
			
			this.dlgAddEvent.ShowAll();
		}
		
		public void OnCancelClicked(object o, EventArgs args)
		{
			this.dlgAddEvent.Destroy();
		}
		
		public void OnOkAddClicked(object o, EventArgs args)
		{
		
		}
		
		public void OnOkCloseClicked(object o, EventArgs args)
		{
		
		}
	}
}