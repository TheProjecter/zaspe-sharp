
using System;
using Glade;
using Gtk;

namespace ZaspeSharp.GUI
{
	public class AddEvent
	{
		[Widget]
		Dialog dlgAddEvent;
		
		public AddEvent(Window parent)
		{
			Glade.XML gxml = new Glade.XML ("add_event.glade", "dlgAddEvent", null);
			gxml.Autoconnect(this);
			
			this.dlgAddEvent.TransientFor = parent;
			this.dlgAddEvent.Show();
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