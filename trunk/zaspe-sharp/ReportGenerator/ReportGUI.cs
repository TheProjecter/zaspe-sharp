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
using Gtk;

using ZaspeSharp.Persons;
using ZaspeSharp.Events;

namespace ZaspeSharp.ReportGenerator {
	public partial class ReportGUI : Gtk.Dialog {
		
		private Selection selection;
		
		public ReportGUI()
		{
			this.Build();
			
			// Create Selection object
			this.selection = new Selection();
			
			// Setup person's treeview
			
			// Some events
			this.tvPersons.Selection.Changed += this.PersonsSelectionChanged;
			
			// Column
			TreeViewColumn personName = new TreeViewColumn();
			personName.Title = "Nombre y apellido";
			CellRendererText personNameText = new CellRendererText();
			personName.PackStart(personNameText, true);
			
			this.tvPersons.AppendColumn(personName);
			
			personName.SetCellDataFunc(personNameText,
			                        new TreeCellDataFunc(this.RenderPersonName));
			
			this.tvPersons.Model = new ListStore(typeof(Person));
			
			// Add all persons
			foreach (Person p in PersonsManager.Instance.RetrieveAll())
				(this.tvPersons.Model as ListStore).AppendValues(p);
			
			// Select all persons
			this.tvPersons.Selection.Mode = SelectionMode.Multiple;
			this.tvPersons.Selection.SelectAll();
			
			
			
			// Setup events's treeview
			
			// Some events
			this.tvEvents.Selection.Changed += this.EventsSelectionChanged;
			
			// Column
			TreeViewColumn eventName = new TreeViewColumn();
			eventName.Title = "Nombre y fecha";
			CellRendererText eventNameText = new CellRendererText();
			eventName.PackStart(eventNameText, true);
			
			this.tvEvents.AppendColumn(eventName);
			
			eventName.SetCellDataFunc(eventNameText,
			                          new TreeCellDataFunc(this.RenderEventName));
			
			this.tvEvents.Model = new ListStore(typeof(Event));
			
			// Add all events
			foreach (Event e in EventsManager.Instance.RetrieveAll())
				(this.tvEvents.Model as ListStore).AppendValues(e);
			
			// Select all events
			this.tvEvents.Selection.Mode = SelectionMode.Multiple;
			this.tvEvents.Selection.SelectAll();
		}
		
		public ReportGUI(Gtk.Window parent) : this()
		{
			this.TransientFor = parent;
		}
		
		protected virtual void OnCancelClicked(object sender, System.EventArgs e)
		{
			this.Respond(Gtk.ResponseType.Close);
		}
		
		protected virtual void OnAcceptClicked(object sender, System.EventArgs e)
		{
			ReportType rt = new SimplePersonsList(this.selection);
			rt.MakeReport();
		}

		protected virtual void OnResponse (object o, Gtk.ResponseArgs args)
		{
			if (args.ResponseId.Equals(Gtk.ResponseType.DeleteEvent) ||
				    args.ResponseId.Equals(Gtk.ResponseType.Close))
				this.Destroy();
		}

		protected virtual void OnPersonsListToggled (object sender, System.EventArgs e)
		{
			this.tvPersons.Sensitive = true;
			this.tvEvents.Sensitive = false;
		}

		protected virtual void OnEventsListToggled (object sender, System.EventArgs e)
		{
			this.tvPersons.Sensitive = false;
			this.tvEvents.Sensitive = true;
		}

		protected virtual void OnAttendancesListToggled (object sender, System.EventArgs e)
		{
			this.tvPersons.Sensitive = true;
			this.tvEvents.Sensitive = true;
		}
		
		private void RenderPersonName(TreeViewColumn column,
		                              CellRenderer cell, TreeModel model,
		                              TreeIter iter)
		{
			Person p = (Person)model.GetValue(iter, 0);
			(cell as CellRendererText).Text = p.Name + " " + p.Surname;
		}
		
		private void RenderEventName(TreeViewColumn column,
		                             CellRenderer cell, TreeModel model,
		                             TreeIter iter)
		{
			Event e = (Event)model.GetValue(iter, 0);
			(cell as CellRendererText).Text = e.Name + " (" + e.Date.ToShortDateString() + ")";
		}
		
		private void PersonsSelectionChanged(object o, EventArgs args)
		{
			this.selection.ClearPersons();
			
			TreeModel model;
			TreeIter iter;
			TreePath[] sels = this.tvPersons.Selection.GetSelectedRows(out model);
			
			Person p;
			foreach (TreePath tp in sels) {
				model.GetIter(out iter, tp);
				p = (Person)model.GetValue(iter, 0);
				
				this.selection.AddPersons(p);
			}
		}
		
		private void EventsSelectionChanged(object o, EventArgs args)
		{
			this.selection.ClearEvents();
			
			TreeModel model;
			TreeIter iter;
			TreePath[] sels = this.tvEvents.Selection.GetSelectedRows(out model);
			
			Event e;
			foreach (TreePath tp in sels) {
				model.GetIter(out iter, tp);
				e = (Event)model.GetValue(iter, 0);
				
				this.selection.AddEvents(e);
			}
		}
	}
}
