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
using System.Collections.Generic;
using Glade;
using Gtk;

using ZaspeSharp.Persons;
using ZaspeSharp.Events;
using ZaspeSharp.Attendances;

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
		
		private List<Person> selectedPersons;
		private List<Event> selectedEvents;
		private List<TreeIter> selectedTreeIters;
		
		private List<Event> lastEvents;
		
		public MainWindow()
		{
			PersonsManager.Instance.PersonsCount();
			
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
			
			this.selectedPersons = new List<Person>();
			this.selectedEvents = new List<Event>();
			this.selectedTreeIters = new List<Gtk.TreeIter>();
			this.lastEvents = new List<Event>();
			
			// Icon for the main window
			this.mainWindow.Icon = new Gdk.Pixbuf("blue_fea.gif");
			
			this.mtbAddPerson.Menu = this.menuPersonActions;
			this.mtbAddEvent.Menu = this.menuEventActions;
			
			// ### tvPersons ###
			this.tvPersons = new TreeView();
			this.tvPersons.HeadersVisible = true;
			this.tvPersons.Selection.Mode = SelectionMode.Multiple;
				
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
			this.tvPersons.Selection.Changed += new EventHandler(this.OnPersonsListSelectionChanged);
			
			// Handler when a row is double clicked
			this.tvPersons.RowActivated += new RowActivatedHandler(this.OnPersonsListRowActivated);
			
			// Handler to disable modify and remove person buttons
			this.tvPersons.Hidden += new EventHandler(this.OnPersonsListHidden);
			
			this.tvPersons.ButtonPressEvent += new ButtonPressEventHandler(this.OnPersonsListButtonPress);
			this.tvPersons.PopupMenu += new PopupMenuHandler(this.OnPersonsListPopupMenu);
			
			
			// ### tvEvents ###
			this.tvEvents = new TreeView();
			this.tvEvents.HeadersVisible = true;
			this.tvEvents.Selection.Mode = SelectionMode.Multiple;
			
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
			this.tvEvents.Selection.Changed += new EventHandler(this.OnEventsListSelectionChanged);
			
			// Handler when a row is double clicked
			this.tvEvents.RowActivated += new RowActivatedHandler(this.OnEventsListRowActivated);
			
			// Handler to disable modify and remove event buttons
			this.tvEvents.Hidden += new EventHandler(this.OnEventsListHidden);
			
			this.tvEvents.ButtonPressEvent += new ButtonPressEventHandler(this.OnEventsListButtonPress);
			this.tvEvents.PopupMenu += new PopupMenuHandler(this.OnEventsListPopupMenu);
			
			
			// ### tvAttendaces ###
			this.LoadLastEvents();
			
//			this.lastEvents.AddRange(EventsManager.Instance.RetrieveLast(3));
//			Person[] allPersons = PersonsManager.Instance.RetrieveAll();
//			
//			List<Type> columnTypes = new List<Type>();
//			// Types to create ListStore. First type is string (name of persons)
//			columnTypes.Add(typeof(string));
//			
//			if (this.lastEvents.Count > 0 && allPersons.Length > 0) {
//				TreeViewColumn persons = new TreeViewColumn();
//				persons.Title = "Personas";
//				
//				CellRendererText personCell = new CellRendererText();
//				persons.PackStart(personCell, true);
//				persons.AddAttribute(personCell, "text", 0);
//				
//				this.tvAttendances.AppendColumn(persons);
//				
//				// Retrieve latest events added, and add them as columns
//				TreeViewColumn eventColumn;
//				CustomCellRendererToggle eventCellRenderer;
//				
//				int k = 1;
//				
//				foreach (Event anEvent in lastEvents) {
//					columnTypes.Add(typeof(bool));
//					
//					// Create cell renderer
//					eventCellRenderer = new CustomCellRendererToggle(k, anEvent);
//					eventCellRenderer.Activatable = true;
//					eventCellRenderer.Toggled += new ToggledHandler(this.OnCellRendererColumnsToggleEvent);
//					
//					// Create column
//					eventColumn = new TreeViewColumn();
//					eventColumn.Title = anEvent.Name + "\n(" + this.FormatEventDateTime(anEvent.Date) + ")";
//					eventColumn.Alignment = 0.5f;
//					eventColumn.PackStart(eventCellRenderer, true);
//					eventColumn.AddAttribute(eventCellRenderer, "active", k++);
//					
//					this.tvAttendances.AppendColumn(eventColumn);
//				}
//				
//				// Add an empty column to avoid last one to expand
//				CellRendererText eventCellRendererText = new CellRendererText();
//				eventCellRendererText.Sensitive = false;
//				
//				// Create column
//				eventColumn = new TreeViewColumn();
//				eventColumn.Title = "";
//				eventColumn.PackStart(eventCellRendererText, false);
//				
//				this.tvAttendances.AppendColumn(eventColumn);
//				
//				// Last type is Person
//				columnTypes.Add(typeof(Person));
//			}
//			
//			this.attendances = new ListStore(columnTypes.ToArray());
//			this.tvAttendances.Model = this.attendances;
			
			// ### Persons and events loading ###
			// Read persons from database
			foreach (Person p in PersonsManager.Instance.RetrieveAll())
				this.AddPersonToList(p);
			
			// Read events from database
			foreach (Event e in EventsManager.Instance.RetrieveAll())
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
			new ModifyEvent(this.mainWindow, this.selectedEvents[0]);
		}
		
		public void OnPersonsListRowActivated(object o, EventArgs args)
		{
			new ModifyPerson(this.mainWindow, this.selectedPersons[0]);
		}
		
		public void OnEventsListSelectionChanged(object o, EventArgs args)
		{
			this.imiModifyEvent.Sensitive = true;
			this.imiRemoveEvent.Sensitive = true;
			
			TreePath[] selection = this.tvEvents.Selection.GetSelectedRows();
			
			// if more than one row was selected, disable modify button
			if (selection.Length > 1)
				this.imiModifyEvent.Sensitive = false;
			else
				this.imiModifyEvent.Sensitive = true;
			
			// Clear selected events
			this.selectedEvents.Clear();
			this.selectedTreeIters.Clear();
			
			// Add selected events to list
			TreeIter iter;
			for (int i=0; i<selection.Length; i++) {
				this.events.GetIter(out iter, selection[i]);
				
				this.selectedTreeIters.Add(iter);
				this.selectedEvents.Add((Event)this.events.GetValue(iter, 2));
			}
		}
		
		public void OnPersonsListSelectionChanged(object o, EventArgs args)
		{
			this.imiModifyPerson.Sensitive = true;
			this.imiRemovePerson.Sensitive = true;
			
			TreePath[] selection = this.tvPersons.Selection.GetSelectedRows();
			
			// if more than one row was selected, disable modify button
			if (selection.Length > 1)
				this.imiModifyPerson.Sensitive = false;
			else
				this.imiModifyPerson.Sensitive = true;
			
			// Clear selected persons
			this.selectedPersons.Clear();
			this.selectedTreeIters.Clear();
			
			// Add selected persons to list
			TreeIter iter;
			for (int i=0; i<selection.Length; i++) {
				this.persons.GetIter(out iter, selection[i]);
				
				this.selectedTreeIters.Add(iter);
				this.selectedPersons.Add((Person)this.persons.GetValue(iter, 4));
			}
		}
		
		public void OnMenuItemModifyPersonActivate(object o, EventArgs args)
		{
			new ModifyPerson(this.mainWindow, this.selectedPersons[0]);
		}
		
		public void OnMenuItemRemovePersonActivate(object o, EventArgs args)
		{
			string msg;
			if (this.selectedPersons.Count > 1)
				msg = "¿Seguro que desea eliminar las personas seleccionadas?";
			else
				msg = "¿Seguro que desea eliminar la persona seleccionada?";
			
			MessageDialog md = new MessageDialog(this.mainWindow, DialogFlags.Modal,
			                                     MessageType.Question, ButtonsType.YesNo,
			                                     msg);
			md.Title = "Confirmación de eliminación";
			
			ResponseType response = (ResponseType)md.Run();
			
			TreeIter iter;
			if (response == ResponseType.Yes) {
				Person[] personsToRemove = this.selectedPersons.ToArray();
				TreeIter[] itersToRemove = this.selectedTreeIters.ToArray();
				
				for (int i=0; i<personsToRemove.Length; i++) {
					personsToRemove[i].Remove();
					
					iter = itersToRemove[i];
					this.persons.Remove(ref iter);
				}
			}
			
			md.Destroy();
		}
		
		public void OnMenuItemModifyEventActivate(object o, EventArgs args)
		{
			new ModifyEvent(this.mainWindow, this.selectedEvents[0]);
		}
		
		public void OnMenuItemRemoveEventActivate(object o, EventArgs args)
		{
			string msg;
			if (this.selectedEvents.Count > 1)
				msg = "¿Seguro que desea eliminar los eventos seleccionados?";
			else
				msg = "¿Seguro que desea eliminar el evento seleccionado?";
			
			MessageDialog md = new MessageDialog(this.mainWindow, DialogFlags.Modal,
			                                     MessageType.Question, ButtonsType.YesNo,
			                                     msg);
			md.Title = "Confirmación de eliminación";
			
			ResponseType response = (ResponseType)md.Run();
			
			TreeIter iter;
			if (response == ResponseType.Yes) {
				Event[] eventsToRemove = this.selectedEvents.ToArray();
				TreeIter[] itersToRemove = this.selectedTreeIters.ToArray();
				
				for (int i=0; i<eventsToRemove.Length; i++) {
					eventsToRemove[i].Remove();
					
					iter = itersToRemove[i];
					this.events.Remove(ref iter);
				}
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
		
		public void OnCellRendererColumnsToggleEvent(object o, ToggledArgs args)
		{
			TreeIter iter;
			
			if (!this.attendances.GetIter(out iter, new TreePath(args.Path)))
				return;
			
			CustomCellRendererToggle cellRendererEvent = (CustomCellRendererToggle)o;
			
			// Invert values
			bool oldValue = (bool)this.attendances.GetValue(iter, cellRendererEvent.ColumnNumber);
			bool newValue = !oldValue;
			this.attendances.SetValue(iter, cellRendererEvent.ColumnNumber, !oldValue);
			
			// Depending on newValue, we add or remove the attendance
			Person person = this.GetPerson(iter);
			
			if (newValue == true)
				AttendancesManager.Instance.AddAttendance(person, cellRendererEvent.Event);
			else
				AttendancesManager.Instance.RemoveAttendance(person, cellRendererEvent.Event);
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
		// Returns true if update is necessary. False otherwise.
		private bool LoadLastEvents()
		{
			// Add to attendances list.
			// First we check if there are persons added.
			if (PersonsManager.Instance.PersonsCount() == 0)
				return false;
			
			/* We get the last 3 events before the actual date. If they do not exists, then
			 * we quit. */
			Event[] lastEventsAgain = EventsManager.Instance.RetrieveLast(3);
			
			if (lastEventsAgain.Length == 0)
				return false;
			
			/* If they have changed, we continue, if not we stop: it's not necessary
			 * to update anything. */
			if (lastEventsAgain.Equals(this.lastEvents.ToArray()))
				return false;
			
			// Add as lastEvents the really last events :)
			this.lastEvents.Clear();
			this.lastEvents.AddRange(lastEventsAgain);
			
			// Remove old events columns
			foreach (TreeViewColumn tvc in this.tvAttendances.Columns)
				this.tvAttendances.RemoveColumn(tvc);
			
			// We'll need to update the model, so we save the new types
			List<Type> columnTypes = new List<Type>();
			// Types to create ListStore. First type is string (name of persons)
			columnTypes.Add(typeof(string));
			
			// Add column of persons
			TreeViewColumn persons = new TreeViewColumn();
			persons.Title = "Personas";
			
			CellRendererText personCell = new CellRendererText();
			persons.PackStart(personCell, true);
			persons.AddAttribute(personCell, "text", 0);
			
			this.tvAttendances.AppendColumn(persons);
			
			// Add new ones
			TreeViewColumn eventColumn;
			CustomCellRendererToggle eventCellRenderer;
			
			int k = 1;
			
			foreach(Event anEvent in this.lastEvents) {
				columnTypes.Add(typeof(bool));
				
				// Create cell renderer
				eventCellRenderer = new CustomCellRendererToggle(k, anEvent);
				eventCellRenderer.Activatable = true;
				eventCellRenderer.Toggled += new ToggledHandler(this.OnCellRendererColumnsToggleEvent);
				
				// Create column
				eventColumn = new TreeViewColumn();
				eventColumn.Title = anEvent.Name + "\n(" + this.FormatEventDateTime(anEvent.Date) + ")";
				eventColumn.Alignment = 0.5f;
				eventColumn.PackStart(eventCellRenderer, true);
				eventColumn.AddAttribute(eventCellRenderer, "active", k++);
				
				this.tvAttendances.AppendColumn(eventColumn);
			}
			
			// Add an empty column to avoid last one to expand
			CellRendererText eventCellRendererText = new CellRendererText();
			eventCellRendererText.Sensitive = false;
			
			// Create column
			eventColumn = new TreeViewColumn();
			eventColumn.Title = "";
			eventColumn.PackStart(eventCellRendererText, false);
			
			this.tvAttendances.AppendColumn(eventColumn);
			
			// Last type is Person
			columnTypes.Add(typeof(Person));
			
			this.attendances = new ListStore(columnTypes.ToArray());
			this.tvAttendances.Model = this.attendances;
			
			// We add all persons again
			AttendancesManager am = AttendancesManager.Instance;
			ArrayList data = new ArrayList();
			
			foreach (Person p in PersonsManager.Instance.RetrieveAll()) {
				data.Clear();
				
				// Person's data
				data.Add(p.Name + " " + p.Surname);
				
				// Events
				foreach (Event anEvent in this.lastEvents)
					data.Add(am.Attended(p, anEvent));
				
				// ... and the person object itself. We'll need it.
				data.Add(p);
				
				this.attendances.AppendValues(data.ToArray());
			}
			
			return true;
		}
		
		public void EventChanged()
		{
			// Update events list
			this.events.SetValue(this.selectedTreeIters[0], 0, this.selectedEvents[0].Name);
			this.events.SetValue(this.selectedTreeIters[0], 1, this.FormatEventDateTime(this.selectedEvents[0].Date));
			
			// TODO: Update attendances list
		}
		
		public void PersonChanged()
		{
			// Update persons list
			this.persons.SetValue(this.selectedTreeIters[0], 0, this.selectedPersons[0].Surname);
			this.persons.SetValue(this.selectedTreeIters[0], 1, this.selectedPersons[0].Name);
			this.persons.SetValue(this.selectedTreeIters[0], 2, this.selectedPersons[0].EMail);
			
			// This is to avoid print birthday if it was not set
			if (!this.selectedPersons[0].BirthdayDate.Equals(DateTime.MinValue))
				this.persons.SetValue(this.selectedTreeIters[0], 3, this.FormatBirthdayDateTime(this.selectedPersons[0].BirthdayDate));
			
			// TODO: Update attendances list
		}
		
		private string FormatEventDateTime(DateTime dt)
		{
			return (dt.ToString("d") + " " + dt.ToString("t"));
		}
		
		private string FormatBirthdayDateTime(DateTime dt)
		{
			return (dt.ToString("dd") + " de " + dt.ToString("MMMM"));
		}
		
		// Only valid if tvAttendances is visible
		public Person GetPerson(TreeIter iter)
		{
			return ((Person)this.attendances.GetValue(iter, 4));
		}
		
		public void AddPersonToList(Person p)
		{
			string birthday = this.FormatBirthdayDateTime(p.BirthdayDate);
			
			if (p.BirthdayDate.Equals(DateTime.MinValue))
			    birthday = "";
			
			// Add to persons list
			this.persons.AppendValues(p.Surname, p.Name, p.EMail, birthday, p);
			
			// Add to attendances list
			
			/* First load last events. If this fuction returns true, it means that an update
			 * to the last events was necessary, and the persons have been added, so it's not
			 * necessary to add this person again. */
			if (this.LoadLastEvents())
				return;
			
			// If there are no last events, we quit.
			if (this.lastEvents.Count == 0)
				return;
			
			AttendancesManager am = AttendancesManager.Instance;
			ArrayList data = new ArrayList();
			
			// Person's data
			data.Add(p.Name + " " + p.Surname);
			
			// Events
			foreach (Event anEvent in this.lastEvents)
				data.Add(am.Attended(p, anEvent));
			
			// ... and the person object itself. We'll need it.
			data.Add(p);
			
			this.attendances.AppendValues(data.ToArray());
		}
		
		public void AddEventToList(Event e)
		{
			// Add to events list
			this.events.AppendValues(e.Name, this.FormatEventDateTime(e.Date), e);
			
			// Mange attendances treeview
			this.LoadLastEvents();
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
