//  Zaspe# - Attendance management
//  Copyright (C) 2006, 2007 Milton Pividori
//
//  This file is part of Zaspe#.
//
//  Zaspe# is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 3 of the License, or
//  (at your option) any later version.
//
//  Zaspe# is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with Zaspe#.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Text;
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
		private ToolButton tbReload;
		
		private ListStore attendances;
		private ListStore persons;
		private ListStore events;
		
		private Menu menuPersonActions;
		private Menu menuEventActions;
		
		private ImageMenuItem imiWhoHadAttended;
		private ImageMenuItem imiModifyPerson;
		private ImageMenuItem imiRemovePerson;
		private ImageMenuItem imiModifyEvent;
		private ImageMenuItem imiRemoveEvent;
		
//		private List<Person> selectedPersons;
//		private List<Event> selectedEvents;
//		private List<TreeIter> selectedTreeIters;
		
		private Dictionary<Person, TreeIter> treeItersOnAttendancesList;
		private List<Event> lastEventsOnAttendancesList;
		
		public MainWindow()
		{
			PersonsManager.Instance.PersonsCount();
			
			Glade.XML gxml_person = new Glade.XML("main_window.glade", "menuPersonActions", null);
			gxml_person.Autoconnect(this);
			this.imiModifyPerson = (ImageMenuItem)gxml_person.GetWidget("imiModifyPerson");
			this.imiRemovePerson = (ImageMenuItem)gxml_person.GetWidget("imiRemovePerson");
			
			Glade.XML gxml_event = new Glade.XML("main_window.glade", "menuEventActions", null);
			gxml_event.Autoconnect(this);
			this.imiWhoHadAttended = (ImageMenuItem)gxml_event.GetWidget("imiWhoHadAttended");
			this.imiModifyEvent = (ImageMenuItem)gxml_event.GetWidget("imiModifyEvent");
			this.imiRemoveEvent = (ImageMenuItem)gxml_event.GetWidget("imiRemoveEvent");
			
			Glade.XML gxml = new Glade.XML ("main_window.glade", "mainWindow", null);
			gxml.Autoconnect(this);
			
			this.menuPersonActions = (Menu)gxml_person.GetWidget("menuPersonActions");
			this.menuEventActions = (Menu)gxml_event.GetWidget("menuEventActions");
			
//			this.selectedPersons = new List<Person>();
//			this.selectedEvents = new List<Event>();
//			this.selectedTreeIters = new List<Gtk.TreeIter>();
			this.lastEventsOnAttendancesList = new List<Event>();
			this.treeItersOnAttendancesList = new Dictionary<Person,TreeIter>();
			
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
			
			surname.SetCellDataFunc(surnameText,
			                        new TreeCellDataFunc(this.RenderSurname));
			name.SetCellDataFunc(nameText,
			                     new TreeCellDataFunc(this.RenderName));
			email.SetCellDataFunc(emailText,
			                      new TreeCellDataFunc(this.RenderEmail));
			birthday.SetCellDataFunc(birthdayText,
			                         new TreeCellDataFunc(this.RenderBirthday));
			
			this.persons = new ListStore(typeof(Person));
			
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
			
			eventName
				.SetCellDataFunc(eventNameText,
				                 new TreeCellDataFunc(this.RenderEventName));
			eventDate
				.SetCellDataFunc(eventDateText,
			                     new TreeCellDataFunc(this.RenderEventDate));
			
			this.events = new ListStore(typeof(Event));
			
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
			this.tvAttendances.Hidden += new EventHandler(this.OnAttendancesListHidden);
			
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
		public void OnMenuItemShowStats(object o, EventArgs args)
		{
			new AttendancesReport(this.mainWindow);
		}
		
		public void OnToolButtonReloadClicked(object o, EventArgs args)
		{
			this.LoadAttendancesListData();
		}
		
		private void DisableEventActionsButtons()
		{
			this.imiWhoHadAttended.Sensitive = false;
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
			
			if (iter.Equals(TreeIter.Zero)) {
				// Disable buttons to modify and remove events
				this.DisableEventActionsButtons();
				
				this.CleanAttendancesList();
			}
		}
		
		public void OnPersonsListRowDeleted(object o, EventArgs args)
		{
			/* TODO: I don't know if this is the best way to know if a
			 * TreeView is empty */
			TreeIter iter = TreeIter.Zero;
			this.persons.GetIterFirst(out iter);
			
			if (iter.Equals(TreeIter.Zero)) {
				// Disable buttons to modify and remove persons
				this.DisablePersonActionsButtons();
				
				this.CleanAttendancesList();
			}
		}
		
		// This method erase all rows and columns in the attendances list
		private void CleanAttendancesList()
		{
			// Erase all rows
			if (this.attendances != null)
				this.attendances.Clear();
			
			// Erase all columns
			foreach (TreeViewColumn tvc in this.tvAttendances.Columns)
				this.tvAttendances.RemoveColumn(tvc);
			
			// Clean lastEvents list
			this.lastEventsOnAttendancesList.Clear();
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
			this.menuPersonActions.Popup();
			//this.menuPersonActions.Popup(null, null, null, 0, 0);
		}
		
		public void OnAttendancesListHidden(object o, EventArgs args)
		{
			this.tbReload.Sensitive = false;
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
			this.ModifyEvent();
		}
		
		public void OnPersonsListRowActivated(object o, EventArgs args)
		{
			this.ModifyPerson();
		}
		
		private void ModifyPerson()
		{
			TreeIter iter = this.SelectedItersFromPersons[0];
			Person p = this.GetPersonFromIter(iter);
			
			ModifyPerson mp =
				new ModifyPerson(this.mainWindow, this.persons, p, iter);
			mp.Run();
			
			this.LoadAttendancesListData();
		}
		
		private void ModifyEvent()
		{
			TreeIter iter = this.SelectedItersFromEvents[0];
			Event e = this.GetEventFromIter(iter);
			
			ModifyEvent me =
				new ModifyEvent(this.mainWindow, this.events, e, iter);
			me.Run();
			
			this.LoadAttendancesListData();
		}
		
		public void OnEventsListSelectionChanged(object o, EventArgs args)
		{
			this.imiWhoHadAttended.Sensitive = true;
			this.imiModifyEvent.Sensitive = true;
			this.imiRemoveEvent.Sensitive = true;
			
			TreePath[] selection = this.tvEvents.Selection.GetSelectedRows();
			
			// if more than one row was selected, disable modify button
			if (selection.Length > 1) {
				this.imiWhoHadAttended.Sensitive = false;
				this.imiModifyEvent.Sensitive = false;
			}
			else {
				this.imiWhoHadAttended.Sensitive = true;
				this.imiModifyEvent.Sensitive = true;
			}
			
//			// Clear selected events
//			this.selectedEvents.Clear();
//			this.selectedTreeIters.Clear();
//			
//			// Add selected events to list
//			TreeIter iter;
//			for (int i=0; i<selection.Length; i++) {
//				this.events.GetIter(out iter, selection[i]);
//				
//				this.selectedTreeIters.Add(iter);
//				this.selectedEvents.Add((Event)this.events.GetValue(iter, 2));
//			}
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
			
//			// Clear selected persons
//			this.selectedPersons.Clear();
//			this.selectedTreeIters.Clear();
//			
//			// Add selected persons to list
//			TreeIter iter;
//			for (int i=0; i<selection.Length; i++) {
//				this.persons.GetIter(out iter, selection[i]);
//				
//				this.selectedTreeIters.Add(iter);
//				this.selectedPersons.Add((Person)this.persons.GetValue(iter, 4));
//			}
		}
		
		public void OnMenuItemModifyPersonActivate(object o, EventArgs args)
		{
			this.ModifyPerson();
		}
		
		public void OnMenuItemRemovePersonActivate(object o, EventArgs args)
		{
			TreeIter iter;
			TreeModel model;
			TreePath[] selection = this.tvPersons
				.Selection.GetSelectedRows(out model);
			
			List<Person> personList = new List<Person>();
			List<TreeIter> iterList = new List<TreeIter>();
			
			foreach(TreePath tp in selection) {
				model.GetIter(out iter, tp);
				Person p = (Person)model.GetValue(iter, 0);
				
				iterList.Add(iter);
				personList.Add(p);
			}
			
			string msg;
			if (selection.Length > 1)
				msg = "¿Seguro que desea eliminar las personas seleccionadas?";
			else
				msg = "¿Seguro que desea eliminar la persona seleccionada?";
			
			MessageDialog md = new MessageDialog(this.mainWindow, DialogFlags.Modal,
			                                     MessageType.Question, ButtonsType.YesNo,
			                                     msg);
			md.Title = "Confirmación de eliminación";
			
			ResponseType response = (ResponseType)md.Run();
			
			if (response == ResponseType.Yes) {
//				Person[] personsToRemove = this.selectedPersons.ToArray();
//				TreeIter[] itersToRemove = this.selectedTreeIters.ToArray();
				
				for (int i=0; i<personList.Count; i++) {
					// Remove all attendances of the person and the person itself
					AttendancesManager
						.Instance.RemoveAllAttendancesOfPerson(personList[i]);
					personList[i].Remove();
					
					// Remove treeiters from persons list
					iter = iterList[i];
					this.persons.Remove(ref iter);
					
					/* Remove treeiters from attendances list, but only if that list
					 * is enabled (if there are events added as columns, it's enabled) */
					if (this.lastEventsOnAttendancesList.Count > 0) {
						iter = this.treeItersOnAttendancesList[personList[i]];
						this.attendances.Remove(ref iter);
						this.treeItersOnAttendancesList.Remove(personList[i]);
					}
				}
			}
			
			md.Destroy();
		}
		
		private List<Event> SelectedEvents
		{
			get {
				TreeIter iter;
				TreeModel model;
				
				// I get the selected events
				TreePath[] selection = this.tvEvents.Selection
					.GetSelectedRows(out model);
				
				List<Event> eventList = new List<Event>();
				
				// Get every selected event and add it to the list
				foreach (TreePath tp in selection) {
					model.GetIter(out iter, tp);
					
					Event e = (Event)model.GetValue(iter, 0);
					
					eventList.Add(e);
				}
				
				return eventList;
			}
		}
		
		private List<Person> SelectedPersons
		{
			get {
				TreeIter iter;
				TreeModel model;
				
				// I get the selected events
				TreePath[] selection = this.tvPersons.Selection
					.GetSelectedRows(out model);
				
				List<Person> personList = new List<Person>();
				
				// Get every selected event and add it to the list
				foreach (TreePath tp in selection) {
					model.GetIter(out iter, tp);
					
					Person p = (Person)model.GetValue(iter, 0);
					
					personList.Add(p);
				}
				
				return personList;
			}
		}
		
		private List<TreeIter> SelectedItersFromPersons
		{
			get {
				TreeIter iter;
				TreeModel model;
				
				// I get the selected events
				TreePath[] selection = this.tvPersons.Selection
					.GetSelectedRows(out model);
				
				List<TreeIter> itersList = new List<TreeIter>();
				
				// Get every selected event and add it to the list
				foreach (TreePath tp in selection) {
					model.GetIter(out iter, tp);
					
					itersList.Add(iter);
				}
				
				return itersList;
			}
		}
		
		private List<TreeIter> SelectedItersFromEvents
		{
			get {
				TreeIter iter;
				TreeModel model;
				
				// I get the selected events
				TreePath[] selection = this.tvEvents.Selection
					.GetSelectedRows(out model);
				
				List<TreeIter> itersList = new List<TreeIter>();
				
				// Get every selected event and add it to the list
				foreach (TreePath tp in selection) {
					model.GetIter(out iter, tp);
					
					itersList.Add(iter);
				}
				
				return itersList;
			}
		}
		
		private Event GetEventFromIter(TreeIter iter)
		{
			Event e = (Event)this.tvEvents.Model.GetValue(iter, 0);
			
			return e;
		}
		
		private Person GetPersonFromIter(TreeIter iter)
		{
			Person p = (Person)this.tvPersons.Model.GetValue(iter, 0);
			
			return p;
		}
		
		public void OnMenuItemWhoHadAttendedActivate(object o, EventArgs args)
		{
			StringBuilder sb = new StringBuilder();
			
			Event selectedEvent = this.SelectedEvents[0];
			
			List<Person> personsThatAttended =
				AttendancesManager
					.Instance.WhoHadAttended(selectedEvent);
			
			if (personsThatAttended.Count == 0)
				sb.Append("Ninguna persona asistió al evento '" +
				          selectedEvent.Name + "'");
			else {
				sb.Append("Asistieron al evento '" + selectedEvent.Name + "':\n\n");
				foreach (Person p in personsThatAttended)
					sb.Append(p.Name + " " + p.Surname + "\n");
			}
			
			MessageDialog md = new MessageDialog(this.mainWindow, DialogFlags.Modal,
			                                     MessageType.Info, ButtonsType.Ok,
			                                     sb.ToString());
			md.Title = "Personas que asistieron al evento";
			
			md.Run();
			md.Destroy();
		}
		
		public void OnMenuItemModifyEventActivate(object o, EventArgs args)
		{
			this.ModifyEvent();
		}
		
		public void OnMenuItemRemoveEventActivate(object o, EventArgs args)
		{
			string msg;
			if (this.SelectedEvents.Count > 1)
				msg = "¿Seguro que desea eliminar los eventos seleccionados?";
			else
				msg = "¿Seguro que desea eliminar el evento seleccionado?";
			
			MessageDialog md = new MessageDialog(this.mainWindow, DialogFlags.Modal,
			                                     MessageType.Question, ButtonsType.YesNo,
			                                     msg);
			md.Title = "Confirmación de eliminación";
			
			ResponseType response = (ResponseType)md.Run();
			
			TreeIter iter;
			CustomTreeViewColumn tvc;
			if (response == ResponseType.Yes) {
				foreach(TreeIter iterToRemove in this.SelectedItersFromEvents) {
					/* I get the event corresponding to the iter to remove */
					Event eventToRemove = this.GetEventFromIter(iterToRemove);
					
					/* Remove all attendances with that event, and the event
					 * itself. */
					AttendancesManager.Instance
						.RemoveAllAttendancesOfEvent(eventToRemove);
					eventToRemove.Remove();
					
					// Remove row in the events list
					TreeIter iterCopied =
						iterToRemove.Copy();// I need to copy the iter to remove
					this.events.Remove(ref iterCopied);
					
					// Remove column in the attendances list
					tvc = this.GetAttendancesListColumnByEvent(eventToRemove);
					this.tvAttendances.RemoveColumn(tvc);
				}
			}
			
			this.LoadAttendancesListData();
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
			this.tbReload.Sensitive = true;
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
			Person person = this.GetPersonFromAttendancesIter(iter);
			
			if (newValue == true)
				AttendancesManager.Instance.AddAttendance(person, cellRendererEvent.Event);
			else
				AttendancesManager.Instance.RemoveAttendance(person, cellRendererEvent.Event);
		}
		
		public void OnAddPersonClicked(object o, EventArgs args)
		{
			AddPerson ap = new AddPerson(this.mainWindow, this.persons);
			ap.Run();
		}
		
		public void OnAddEventClicked(object o, EventArgs args)
		{
			AddEvent ae = new AddEvent(this.mainWindow, this.events);
			ae.Run();
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
		
#region Renderer Methods
		private void RenderSurname(TreeViewColumn column, CellRenderer cell,
		                           TreeModel model, TreeIter iter)
		{
			Person p = (Person)model.GetValue(iter, 0);
			(cell as CellRendererText).Text = p.Surname;
		}
		
		private void RenderName(TreeViewColumn column, CellRenderer cell,
		                        TreeModel model, TreeIter iter)
		{
			Person p = (Person)model.GetValue(iter, 0);
			(cell as CellRendererText).Text = p.Name;
		}
		
		private void RenderNameAndSurname(TreeViewColumn column,
		                                  CellRenderer cell, TreeModel model,
		                                  TreeIter iter)
		{
			Person p = (Person)model.GetValue(iter, 0);
			(cell as CellRendererText).Text = p.Name + " " + p.Surname;
		}
		
		private void RenderEmail(TreeViewColumn column, CellRenderer cell,
		                         TreeModel model, TreeIter iter)
		{
			Person p = (Person)model.GetValue(iter, 0);
			(cell as CellRendererText).Text = p.EMail;
		}
		
		private void RenderBirthday(TreeViewColumn column, CellRenderer cell,
		                            TreeModel model, TreeIter iter)
		{
			Person p = (Person)model.GetValue(iter, 0);
			
			// If birthday was not set, show nothing
			if (!p.BirthdayDate.Equals(DateTime.MinValue))
				(cell as CellRendererText).Text =
					this.FormatBirthdayDateTime(p.BirthdayDate);
			else
				(cell as CellRendererText).Text = "";
		}
		
		private void RenderEventName(TreeViewColumn column, CellRenderer cell,
		                             TreeModel model, TreeIter iter)
		{
			Event e = (Event)model.GetValue(iter, 0);
			(cell as CellRendererText).Text = e.Name;
		}
		
		private void RenderEventDate(TreeViewColumn column, CellRenderer cell,
		                             TreeModel model, TreeIter iter)
		{
			Event e = (Event)model.GetValue(iter, 0);
			
			(cell as CellRendererText).Text =
				this.FormatEventDateTime(e.Date);
		}
#endregion
		
#region Other methods
//		public TreeIter GetAttendancesListIterByPerson(Person aPerson)
//		{
//			TreeIter iter;
//			
//			this.attendances.GetIterFirst(out iter);
//			
//			if (this.GetPerson(iter).Equals(aPerson))
//				return iter;
//			
//			while (this.attendances.IterNext(ref iter)) {
//				if (this.GetPerson(iter).Equals(aPerson))
//					return iter;
//			}
//			
//			return TreeIter.Zero;
//		}
		
		public CustomTreeViewColumn GetAttendancesListColumnByEvent(Event anEvent)
		{
			foreach (CustomTreeViewColumn tvc in this.tvAttendances.Columns) {
				if (tvc.Event != null && tvc.Event.Equals(anEvent))
					return tvc;
			}
			
			return null;
		}
		
		private string GenerateColumnTitle(Event anEvent)
		{
			return (anEvent.Name + "\n(" + this.FormatEventDateTime(anEvent.Date) + ")");
		}
		
		private void LoadAttendancesListData()
		{
			// Add to attendances list.
			// First we check if there are persons added.
			if (PersonsManager.Instance.PersonsCount() == 0)
				return;
			
			/* We get the last 3 events before the actual date. If they do not exists, then
			 * we quit. */
			Event[] lastEventsAgain = EventsManager.Instance.RetrieveLast(3);
			
			if (lastEventsAgain.Length == 0) {
				this.CleanAttendancesList();
				
				return;
			}
			
			/* If they have changed, we continue, if not we stop: it's not necessary
			 * to update anything. */
			if (lastEventsAgain.Length == this.lastEventsOnAttendancesList.Count) {
				int j;
				for (j=0; j<lastEventsAgain.Length; j++) {
					// Check if events (objects) are equals
					if (!lastEventsAgain[j].Equals(this.lastEventsOnAttendancesList[j]))
						break;
				}
				
				/* If events are the same, regenerate columns titles (maybe it's not necessary,
				 * but it the same that checking if they are equals or not to the correct one). */
				if (j == lastEventsAgain.Length) {
					
					foreach(CustomTreeViewColumn ctvc in this.tvAttendances.Columns) {
						if (ctvc.Event != null)
							ctvc.Title = this.GenerateColumnTitle(ctvc.Event);
					}
					
					return;
				}
			}
			
			// Clear attendances list
			this.CleanAttendancesList();
			
			// Add as lastEvents the really last events :)
			this.lastEventsOnAttendancesList.AddRange(lastEventsAgain);
			
			// We'll need to update the model, so we save the new types
			List<Type> columnTypes = new List<Type>();
			// Types to create ListStore. First type is string (name of persons)
			columnTypes.Add(typeof(Person));
			
			// Add column of persons
			CustomTreeViewColumn persons = new CustomTreeViewColumn();
			persons.Title = "Personas";
			
			CellRendererText personCell = new CellRendererText();
			persons.PackStart(personCell, true);
			persons
				.SetCellDataFunc(personCell,
				                 new TreeCellDataFunc(this.RenderNameAndSurname));
			
			this.tvAttendances.AppendColumn(persons);
			
			// Add new ones
			CustomTreeViewColumn eventColumn;
			CustomCellRendererToggle eventCellRenderer;
			
			int k = 1;
			
			foreach(Event anEvent in this.lastEventsOnAttendancesList) {
				columnTypes.Add(typeof(bool));
				
				// Create cell renderer
				eventCellRenderer = new CustomCellRendererToggle(k, anEvent);
				eventCellRenderer.Activatable = true;
				eventCellRenderer.Toggled += new ToggledHandler(this.OnCellRendererColumnsToggleEvent);
				
				// Create column
				eventColumn = new CustomTreeViewColumn(anEvent);
				eventColumn.Title = this.GenerateColumnTitle(anEvent);
				eventColumn.Alignment = 0.5f;
				eventColumn.PackStart(eventCellRenderer, true);
				eventColumn.AddAttribute(eventCellRenderer, "active", k++);
				
				this.tvAttendances.AppendColumn(eventColumn);
			}
			
			// Add an empty column to avoid last one to expand
			CellRendererText eventCellRendererText = new CellRendererText();
			eventCellRendererText.Sensitive = false;
			
			// Create column
			eventColumn = new CustomTreeViewColumn();
			eventColumn.Title = "";
			eventColumn.PackStart(eventCellRendererText, false);
			
			this.tvAttendances.AppendColumn(eventColumn);
			
			// Last type is Person
//			columnTypes.Add(typeof(Person));
			
			this.attendances = new ListStore(columnTypes.ToArray());
			this.tvAttendances.Model = this.attendances;
			
			// Clean dictionary with persons and TreeIter of attendances list
			this.treeItersOnAttendancesList.Clear();
			
			// We add all persons again
			AttendancesManager am = AttendancesManager.Instance;
			ArrayList data = new ArrayList();
			TreeIter iter;
			
			foreach (Person p in PersonsManager.Instance.RetrieveAll()) {
				data.Clear();
				
				// Person's data
				data.Add(p);
				Console.WriteLine("Agregado " + p.Name);
				// Events
				foreach (Event anEvent in this.lastEventsOnAttendancesList)
					data.Add(am.Attended(p, anEvent));
				
				// ... and the person object itself. We'll need it.
//				data.Add(p);
				
				iter = this.attendances.AppendValues(data.ToArray());
				
				if (!this.treeItersOnAttendancesList.ContainsKey(p))
					this.treeItersOnAttendancesList.Add(p, iter);
			}
			
			return;
		}
		
//		public void EventChanged()
//		{
//			// Update events list
//			this.events.SetValue(this.selectedTreeIters[0], 0, this.selectedEvents[0].Name);
//			this.events.SetValue(this.selectedTreeIters[0], 1, this.FormatEventDateTime(this.selectedEvents[0].Date));
//			
//			// Update attendances list
//			this.LoadAttendancesListData();
//		}
//		
//		public void PersonChanged()
//		{
//			// Update persons list
//			this.persons.SetValue(this.selectedTreeIters[0], 0, this.selectedPersons[0].Surname);
//			this.persons.SetValue(this.selectedTreeIters[0], 1, this.selectedPersons[0].Name);
//			this.persons.SetValue(this.selectedTreeIters[0], 2, this.selectedPersons[0].EMail);
//			
//			// This is to avoid print birthday if it was not set
//			if (!this.selectedPersons[0].BirthdayDate.Equals(DateTime.MinValue))
//				this.persons.SetValue(this.selectedTreeIters[0], 3, this.FormatBirthdayDateTime(this.selectedPersons[0].BirthdayDate));
//			
//			// Update attendances list
//			// First see if the attendances list is enabled (there are last events)
//			if (this.lastEventsOnAttendancesList.Count == 0)
//				return;
//			
//			this.attendances.SetValue(this.treeItersOnAttendancesList[this.selectedPersons[0]],
//			                          0,
//			                          this.selectedPersons[0].Name + " " + this.selectedPersons[0].Surname);
//		}
		
		private string FormatEventDateTime(DateTime dt)
		{
			return (dt.ToString("d") + " " + dt.ToString("t"));
		}
		
		private string FormatBirthdayDateTime(DateTime dt)
		{
			return (dt.ToString("dd") + " de " + dt.ToString("MMMM"));
		}
		
		// Only valid if tvAttendances is visible
		public Person GetPersonFromAttendancesIter(TreeIter iter)
		{
			Person p = (Person)this.attendances.GetValue(iter, 0);
			return p;
		}
		
		public void AddPersonToList(Person p)
		{
			// Add to persons list
			this.persons.AppendValues(p);
			
			/* First load last events. If this fuction returns true, it means
			 * that an update to the last events was necessary, and the persons
			 * have been added, so it's not necessary to add this person
			 * again. */
			this.LoadAttendancesListData();
			
			// If persons in attendances list is updated, quit
			if (this.treeItersOnAttendancesList.ContainsKey(p))
				return;
			
			// If there are no last events, we quit.
			if (this.lastEventsOnAttendancesList.Count == 0)
				return;
			
			AttendancesManager am = AttendancesManager.Instance;
			ArrayList data = new ArrayList();
			
			// Person's data
			data.Add(p);
			
			// Events
			foreach (Event anEvent in this.lastEventsOnAttendancesList)
				data.Add(am.Attended(p, anEvent));
			
			// ... and the person object itself. We'll need it.
//			data.Add(p);
			
			TreeIter iter = this.attendances.AppendValues(data.ToArray());
			
			if (!this.treeItersOnAttendancesList.ContainsKey(p))
					this.treeItersOnAttendancesList.Add(p, iter);
		}
		
		public void AddEventToList(Event e)
		{
			// Add to events list
			this.events.AppendValues(e);
			
			// Mange attendances treeview
			this.LoadAttendancesListData();
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
