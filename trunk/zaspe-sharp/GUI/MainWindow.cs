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
	public class MainWindow
	{
		public static MainWindow mainWindowInstance;
		
		[Widget]
		private Gtk.Window mainWindow;
		
		[Widget]
		private ToolButton tbAddPerson;
		
		[Widget]
		private ToolButton tbAddEvent;
		
		[Widget]
		private ToolButton tbQuit;
		
		[Widget]
		private VBox vbox1;
		
		[Widget]
		private TreeView tvAttendances;
		
		[Widget]
		private TreeView tvPersons;
		
		[Widget]
		private TreeView tvEvents;
		
		[Widget]
		private ListStore attendances;
		
		[Widget]
		private ListStore persons;
		
		[Widget]
		private ListStore events;
		
		public MainWindow()
		{
			Glade.XML gxml = new Glade.XML ("main_window.glade", "mainWindow", null);
			gxml.Autoconnect(this);
			
			// Icon for the main window
			this.mainWindow.Icon = new Gdk.Pixbuf("blue_fea.gif");
			
			// tvPersons
			this.tvPersons = new TreeView();
			this.tvPersons.HeadersVisible = true;
			
			TreeViewColumn surname = new TreeViewColumn();
			surname.Title = "Apellido";
			CellRendererText surnameText = new CellRendererText();
			surname.PackStart(surnameText, true);
			
			TreeViewColumn name = new TreeViewColumn();
			name.Title = "Nombre";
			CellRendererText nameText = new CellRendererText();
			name.PackStart(nameText, true);
			
			TreeViewColumn email = new TreeViewColumn();
			email.Title = "E-Mail";
			CellRendererText emailText = new CellRendererText();
			email.PackStart(emailText, true);
			
			TreeViewColumn birthday = new TreeViewColumn();
			email.Title = "Cumpleaños";
			CellRendererText birthdayText = new CellRendererText();
			birthday.PackStart(birthdayText, true);
			
			this.tvPersons.AppendColumn(surname);
			this.tvPersons.AppendColumn(name);
			this.tvPersons.AppendColumn(email);
			this.tvPersons.AppendColumn(birthday);
			
			surname.AddAttribute(surnameText, "text", 0);
			name.AddAttribute(nameText, "text", 1);
			email.AddAttribute(emailText, "text", 2);
			birthday.AddAttribute(birthdayText, "text", 3);
			
			this.persons = new ListStore(typeof(string), typeof(string),
			                             typeof(string), typeof(string));
			
			// Example item
			//this.persons.AppendValues("Prueba", "DesdeCodigo", "A ver que pasa");
			
			this.tvPersons.Model = this.persons;
			
			// tvEvents
			this.tvEvents = new TreeView();
			
			TreeViewColumn eventName = new TreeViewColumn();
			eventName.Title = "Nombre";
			CellRendererText eventNameText = new CellRendererText();
			eventName.PackStart(eventNameText, true);
			
			TreeViewColumn eventDate = new TreeViewColumn();
			eventDate.Title = "Fecha";
			CellRendererText eventDateText = new CellRendererText();
			eventDate.PackStart(eventDateText, true);
			
			this.tvEvents.AppendColumn(eventName);
			this.tvEvents.AppendColumn(eventDate);
			
			eventName.AddAttribute(eventNameText, "text", 0);
			eventDate.AddAttribute(eventDateText, "text", 1);
			
			this.events = new ListStore(typeof(string), typeof(string));
			
			// Example item
			//this.events.AppendValues("Evento de", "Prueba");
			
			this.tvEvents.Model = this.events;
			
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
			
			this.attendances.AppendValues("Arnoldo Braida", false, false, false);
			this.attendances.AppendValues("Pepe Biondi", true, false, true);
			this.attendances.AppendValues("Damián Paduán", false, true, false);
			this.attendances.AppendValues("Fito Páez", false, false, false);
			
			this.tvAttendances.Model = attendances;
			
			// Read from persons database
			PersonsManager pm = PersonsManager.Instance;
			foreach (Person p in pm.RetrieveAll())
				this.AddPersonToList(p);
			
			this.mainWindow.ShowAll();
		}
		
#region Event handlers
		public void OnPersonsListToggle(object o, EventArgs args)
		{
			this.AddTreeViewInVBox(this.tvPersons);
		}
		
		public void OnEventsListToggle(object o, EventArgs args)
		{
			this.AddTreeViewInVBox(this.tvEvents);
		}
		
		public void OnAttendancesListToggle(object o, EventArgs args)
		{
			this.AddTreeViewInVBox(this.tvAttendances);
		}
		
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
		
#region Other methods
		public void AddPersonToList(Person p)
		{
			this.persons.AppendValues(p.Surname, p.Name, p.EMail, p.BirthdayDate.ToString("dd/MMMM"));
		}
		
		private void AddTreeViewInVBox(TreeView tv)
		{
			// I remove the actual treeview
			Widget widgetToRemove = null;
			
			foreach (Widget w in this.vbox1.AllChildren) {
				if (w is TreeView) {
					widgetToRemove = w;
					break;
				}
			}
			
			this.vbox1.Remove(widgetToRemove);
			
			// Add and reorder the treeview
			this.vbox1.PackStart(tv, true, true, 0);
			this.vbox1.ReorderChild(tv, 2);
			
			tv.Show();
		}
		
		private void Quit()
		{
			Application.Quit();
		}
		
		public static void Main(String[] args)
		{
			Application.Init();
			mainWindowInstance = new MainWindow();
			Application.Run();
		}
#endregion
	}
}
