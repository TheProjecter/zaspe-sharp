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
using ZaspeSharp.Events;

namespace ZaspeSharp.GUI
{
	public class MainWindow
	{
		public static MainWindow mainWindowInstance;
		
		[Widget]
		private Gtk.Window mainWindow;
		
		[Widget]
		private MenuToolButton mtbAddPerson;
		
		[Widget]
		private MenuToolButton mtbAddEvent;
		
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
		
		private Menu menuPersonActions;
		private Menu menuEventActions;
		
		private ImageMenuItem imiModifyPerson;
		private ImageMenuItem imiRemovePerson;
		private ImageMenuItem imiModifyEvent;
		private ImageMenuItem imiRemoveEvent;
		
		private Person selectedPerson;
		private Event selectedEvent;
		private TreeIter selectedTreeIter;
		
		public MainWindow()
		{
			Glade.XML gxml_person = new Glade.XML("main_window.glade", "menuPersonActions", null);
			gxml_person.Autoconnect(this);
			this.imiModifyPerson = (ImageMenuItem)gxml_person.GetWidget("imiModifyPerson");
			this.imiRemovePerson = (ImageMenuItem)gxml_person.GetWidget("imiRemovePerson");
			
			Glade.XML gxml_event = new Glade.XML("main_window.glade", "menuEventActions", null);
			gxml_event.Autoconnect(this);
			this.imiModifyEvent = (ImageMenuItem)gxml_event.GetWidget("imiModifyEvent");
			this.imiRemoveEvent = (ImageMenuItem)gxml_event.GetWidget("imiRemoveEvent");
			
			Glade.XML gxml = new Glade.XML ("main_window.glade", "mainWindow", null);
			gxml.Autoconnect(this);
			
			this.menuPersonActions = (Menu)gxml_person.GetWidget("menuPersonActions");
			this.menuEventActions = (Menu)gxml_event.GetWidget("menuEventActions");
			
			// Icon for the main window
			this.mainWindow.Icon = new Gdk.Pixbuf("blue_fea.gif");
			
			this.mtbAddPerson.Menu = this.menuPersonActions;
			this.mtbAddEvent.Menu = this.menuEventActions;
			
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
			birthday.Title = "Cumpleaños";
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
			                             typeof(string), typeof(string),
			                             typeof(Person));
			
			this.persons.RowDeleted += new RowDeletedHandler(this.OnPersonsListRowDeleted);
			
			this.tvPersons.Model = this.persons;
			
			// Handler when a row is selected, to enable person modify button
			this.tvPersons.CursorChanged += new EventHandler(this.OnPersonsListCursorChanged);
			
			// Handler when a row is double clicked
			this.tvPersons.RowActivated += new RowActivatedHandler(this.OnPersonsListRowActivated);
			
			// Handler to disable modify and remove person buttons
			this.tvPersons.Hidden += new EventHandler(this.OnPersonsListHidden);
			
			this.tvPersons.ButtonPressEvent += new ButtonPressEventHandler(this.OnPersonsListButtonPress);
			this.tvPersons.PopupMenu += new PopupMenuHandler(this.OnPersonsListPopupMenu);
			
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
			
			this.events = new ListStore(typeof(string), typeof(string), typeof(Event));
			
			this.events.RowDeleted += new RowDeletedHandler(this.OnEventsListRowDeleted);
			
			this.tvEvents.Model = this.events;
			
			// Handler when a row is selected, to enable event modify button
			this.tvEvents.CursorChanged += new EventHandler(this.OnEventsListCursorChanged);
			
			// Handler when a row is double clicked
			this.tvEvents.RowActivated += new RowActivatedHandler(this.OnEventsListRowActivated);
			
			// Handler to disable modify and remove event buttons
			this.tvEvents.Hidden += new EventHandler(this.OnEventsListHidden);
			
			this.tvEvents.ButtonPressEvent += new ButtonPressEventHandler(this.OnEventsListButtonPress);
			this.tvEvents.PopupMenu += new PopupMenuHandler(this.OnEventsListPopupMenu);
			
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
			
			this.tvAttendances.Model = this.attendances;
			
			// Read persons from database
			PersonsManager pm = PersonsManager.Instance;
			foreach (Person p in pm.RetrieveAll())
				this.AddPersonToList(p);
			
			// Read events from database
			EventsManager em = EventsManager.Instance;
			foreach (Event e in em.RetrieveAll())
				this.AddEventToList(e);
			
			this.mainWindow.ShowAll();
		}
		
#region Event handlers
		private void DisableEventActionsButtons()
		{
			this.imiModifyEvent.Sensitive = false;
			this.imiRemoveEvent.Sensitive = false;
		}
		
		private void DisablePersonActionsButtons()
		{
			this.imiModifyPerson.Sensitive = false;
			this.imiRemovePerson.Sensitive = false;
		}
		
		public void OnEventsListRowDeleted(object o, EventArgs args)
		{
			/* TODO: I don't know if this is the best way to know if a
			 * TreeView is empty */
			TreeIter iter = TreeIter.Zero;
			this.events.GetIterFirst(out iter);
			
			if (iter.Equals(TreeIter.Zero))
				this.DisableEventActionsButtons();
		}
		
		public void OnPersonsListRowDeleted(object o, EventArgs args)
		{
			/* TODO: I don't know if this is the best way to know if a
			 * TreeView is empty */
			TreeIter iter = TreeIter.Zero;
			this.persons.GetIterFirst(out iter);
			
			if (iter.Equals(TreeIter.Zero))
				this.DisablePersonActionsButtons();
		}
		
		[GLib.ConnectBefore]
		public void OnEventsListButtonPress(object o, ButtonPressEventArgs args)
		{
			// Check if pressed button is the the right one
			if (args.Event.Button == 3)
				this.menuEventActions.Popup();
		}
		
		[GLib.ConnectBefore]
		public void OnPersonsListButtonPress(object o, ButtonPressEventArgs args)
		{
			// Check if pressed button is the the right one
			if (args.Event.Button == 3)
				this.menuPersonActions.Popup();
		}
		
		public void OnEventsListPopupMenu(object o, PopupMenuArgs args)
		{
			this.menuEventActions.ShowAll();
			this.menuEventActions.Popup();
		}
		
		public void OnPersonsListPopupMenu(object o, PopupMenuArgs args)
		{
			this.menuPersonActions.ShowAll();
			this.menuEventActions.Popup();
			//this.menuPersonActions.Popup(null, null, null, 0, 0);
		}
		
		public void OnEventsListHidden(object o, EventArgs args)
		{
			this.DisableEventActionsButtons();
		}
		
		public void OnPersonsListHidden(object o, EventArgs args)
		{
			this.DisablePersonActionsButtons();
		}
		
		public void OnEventsListRowActivated(object o, EventArgs args)
		{
			new ModifyEvent(this.mainWindow, this.selectedEvent);
		}
		
		public void OnPersonsListRowActivated(object o, EventArgs args)
		{
			new ModifyPerson(this.mainWindow, this.selectedPerson);
		}
		
		public void OnEventsListCursorChanged(object o, EventArgs args)
		{
			this.imiModifyEvent.Sensitive = true;
			this.imiRemoveEvent.Sensitive = true;
			
			this.tvEvents.Selection.GetSelected(out this.selectedTreeIter);
			this.selectedEvent = (Event)this.events.GetValue(this.selectedTreeIter, 2);
		}
		
		public void OnPersonsListCursorChanged(object o, EventArgs args)
		{
			this.imiModifyPerson.Sensitive = true;
			this.imiRemovePerson.Sensitive = true;
			
			this.tvPersons.Selection.GetSelected(out this.selectedTreeIter);
			this.selectedPerson = (Person)this.persons.GetValue(this.selectedTreeIter, 4);
		}
		
		public void OnMenuItemModifyPersonActivate(object o, EventArgs args)
		{
			new ModifyPerson(this.mainWindow, this.selectedPerson);
		}
		
		public void OnMenuItemRemovePersonActivate(object o, EventArgs args)
		{
			MessageDialog md = new MessageDialog(this.mainWindow, DialogFlags.Modal,
			                                     MessageType.Question, ButtonsType.YesNo,
			                                     "¿Seguro que desea eliminar la persona?");
			md.Title = "Confirmación de eliminación";
			
			ResponseType response = (ResponseType)md.Run();
			
			if (response == ResponseType.Yes) {
				this.selectedPerson.Remove();
				this.persons.Remove(ref this.selectedTreeIter);
			}
			
			md.Destroy();
		}
		
		public void OnMenuItemModifyEventActivate(object o, EventArgs args)
		{
			new ModifyEvent(this.mainWindow, this.selectedEvent);
		}
		
		public void OnMenuItemRemoveEventActivate(object o, EventArgs args)
		{
			MessageDialog md = new MessageDialog(this.mainWindow, DialogFlags.Modal,
			                                     MessageType.Question, ButtonsType.YesNo,
			                                     "¿Seguro que desea eliminar el evento?");
			md.Title = "Confirmación de eliminación";
			
			ResponseType response = (ResponseType)md.Run();
			
			if (response == ResponseType.Yes) {
				this.selectedEvent.Remove();
				this.events.Remove(ref this.selectedTreeIter);
			}
			
			md.Destroy();
		}
		
		
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
		
		public void OnImageMenuItemAboutActivate(object o, EventArgs args)
		{
			new About(this.mainWindow);
		}
#endregion
		
#region Other methods
		public void EventChanged()
		{
			this.events.SetValue(this.selectedTreeIter, 0, this.selectedEvent.Name);
			this.events.SetValue(this.selectedTreeIter, 1, this.selectedEvent.Date.ToString());
		}
		
		public void PersonChanged()
		{
			this.persons.SetValue(this.selectedTreeIter, 0, this.selectedPerson.Name);
			this.persons.SetValue(this.selectedTreeIter, 1, this.selectedPerson.Surname);
			this.persons.SetValue(this.selectedTreeIter, 2, this.selectedPerson.EMail);
			
			// This is to avoid print birthday if it was not set
			if (!this.selectedPerson.BirthdayDate.Equals(DateTime.MinValue))
				this.persons.SetValue(this.selectedTreeIter, 3, this.FormatDateTime(this.selectedPerson.BirthdayDate));
		}
		
		private string FormatDateTime(DateTime dt)
		{
			return (dt.ToString("dd") + " de " + dt.ToString("MMMM"));
		}
		
		public void AddPersonToList(Person p)
		{
			string birthday = this.FormatDateTime(p.BirthdayDate);
			
			if (p.BirthdayDate.Equals(DateTime.MinValue))
			    birthday = "";
			
			this.persons.AppendValues(p.Surname, p.Name, p.EMail, birthday, p);
		}
		
		public void AddEventToList(Event e)
		{
//			string date = this.FormatDateTime(p.BirthdayDate);
//			
//			if (p.BirthdayDate.Equals(DateTime.MinValue))
//			    birthday = "";
			
			this.events.AppendValues(e.Name, e.Date.ToString(), e);
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
			
			widgetToRemove.Hide();
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
