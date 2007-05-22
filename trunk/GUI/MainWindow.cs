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

namespace ZaspeSharp.GUI
{
	public class MainWindow
	{
		[Widget]
		private Gtk.Window mainWindow;
		
		[Widget]
		private ToolButton tbAddPerson;
		
		[Widget]
		private ToolButton tbAddEvent;
		
		[Widget]
		private ToolButton tbQuit;
		
		[Widget]
		private TreeView tvAttendances;
		
		private ListStore attendances;
		
		public MainWindow()
		{
			Glade.XML gxml = new Glade.XML ("main_window.glade", "mainWindow", null);
			gxml.Autoconnect(this);
			
			// Icon for the main window
			this.mainWindow.Icon = new Gdk.Pixbuf(null, "blue_fea.gif");
			
			// TreeView example
			TreeViewColumn persons = new TreeViewColumn();
			persons.Title = "Personas";
			
			CellRendererText personCell = new CellRendererText();
			persons.PackStart(personCell, true);
			
			TreeViewColumn event1 = new TreeViewColumn();
			event1.Title = "Misa 27/05 - 19 hs";
			
			TreeViewColumn event2 = new TreeViewColumn();
			event2.Title = "Ensayo 26/05 - 19 hs";
			
			TreeViewColumn event3 = new TreeViewColumn();
			event3.Title = "Misa 3/06 - 19 hs";
			
			CellRendererToggle event1Cell = new CellRendererToggle();
			CellRendererToggle event2Cell = new CellRendererToggle();
			CellRendererToggle event3Cell = new CellRendererToggle();
			
			event1Cell.Activatable = true;
			event2Cell.Activatable = true;
			event3Cell.Activatable = true;
			
			event1Cell.Toggled += this.OnCellRendererToggleEvent1Toggled;
			event2Cell.Toggled += this.OnCellRendererToggleEvent2Toggled;
			event3Cell.Toggled += this.OnCellRendererToggleEvent3Toggled;
			
			event1.PackStart(event1Cell, true);
			event2.PackStart(event2Cell, true);
			event3.PackStart(event3Cell, true);
			
			this.tvAttendances.AppendColumn(persons);
			this.tvAttendances.AppendColumn(event1);
			this.tvAttendances.AppendColumn(event2);
			this.tvAttendances.AppendColumn(event3);
			
			persons.AddAttribute(personCell, "text", 0);
			event1.AddAttribute(event1Cell, "active", 1);
			event2.AddAttribute(event2Cell, "active", 2);
			event3.AddAttribute(event3Cell, "active", 3);
			
			this.attendances = new ListStore(typeof(string), typeof(bool),
				typeof(bool), typeof(bool));
			
			attendances.AppendValues("Arnoldo Braida", false, false, false);
			attendances.AppendValues("Pepe Biondi", true, false, true);
			attendances.AppendValues("Damián Paduán", false, true, false);
			attendances.AppendValues("Fito Páez", false, false, false);
			
			this.tvAttendances.Model = attendances;
			
			this.mainWindow.ShowAll();
		}
		
		#region Event handlers
		public void OnCellRendererToggleEvent1Toggled(object o, ToggledArgs args)
		{
			TreeIter iter;
			
			if (this.attendances.GetIter(out iter, new TreePath(args.Path))) {
				bool old = (bool)this.attendances.GetValue(iter, 1);
				this.attendances.SetValue(iter, 1, !old);
			}
		}
		
		public void OnCellRendererToggleEvent2Toggled(object o, ToggledArgs args)
		{
			TreeIter iter;
			
			if (this.attendances.GetIter(out iter, new TreePath(args.Path))) {
				bool old = (bool)this.attendances.GetValue(iter, 2);
				this.attendances.SetValue(iter, 2, !old);
			}
		}
		
		public void OnCellRendererToggleEvent3Toggled(object o, ToggledArgs args)
		{
			TreeIter iter;
			
			if (this.attendances.GetIter(out iter, new TreePath(args.Path))) {
				bool old = (bool)this.attendances.GetValue(iter, 3);
				this.attendances.SetValue(iter, 3, !old);
			}
		}
		
		public void OnAddPersonClicked(object o, EventArgs args)
		{
			new AddPerson(this.mainWindow);
		}
		
		public void OnAddEventClicked(object o, EventArgs args)
		{
			new AddEvent(this.mainWindow);
		}
		
		public void OnWindowDeleteEvent(object o, DeleteEventArgs args)
		{
			this.Quit();
			args.RetVal = true;
		}
		
		public void OnToolButtonQuitClicked(object o, EventArgs args)
		{
			this.Quit();
		}
		
		public void OnMenuItemQuitActivate(object o, EventArgs args)
		{
			this.Quit();
		}
		
		public void OnImageMenuItemAboutActivate(object o, EventArgs args)
		{
			About ad = new About(this.mainWindow);
		}
		#endregion
		
		private void Quit()
		{
			Application.Quit();
		}
		
		public static void Main(String[] args)
		{
			Application.Init();
			new MainWindow();
			Application.Run();
		}
	}
}
