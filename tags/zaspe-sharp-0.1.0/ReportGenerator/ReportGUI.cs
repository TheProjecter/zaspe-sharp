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

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ZaspeSharp.ReportGenerator {
	public partial class ReportGUI : Gtk.Dialog {
		
		private Selection selection;
		
		public ReportGUI()
		{
			this.Build();
			
			// Create Selection object and default report type
			this.selection = new Selection();
//			this.pageSize = PageSize.A4;
			//this.reportType = SimplePersonsList.GetInstance(this.selection, this.pageSize);
			
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
			
			// Add past events
			foreach (Event e in EventsManager.Instance.RetrieveLast(10)) {
				(this.tvEvents.Model as ListStore).AppendValues(e);
			}
			
			// Select all events
			this.tvEvents.Selection.Mode = SelectionMode.Single;
			this.tvEvents.Selection.SelectAll();
		}
		
		private Rectangle PageSizeSelected {
			get {
				Rectangle pageSize = iTextSharp.text.PageSize.A4;
				
				if (this.cmbPageSize.ActiveText.Equals("A4"))
					pageSize = iTextSharp.text.PageSize.A4;
				else if (this.cmbPageSize.ActiveText.Equals("Carta"))
					pageSize = iTextSharp.text.PageSize.LETTER;
				
				return pageSize;
			}
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
			// Ask for report file
			Gtk.FileChooserDialog fc =
				new Gtk.FileChooserDialog("Elija dónde guardar el archivo de reporte",
				                          this,
				                          FileChooserAction.Save,
				                          "Cancelar", ResponseType.Cancel,
				                          "Guardar", ResponseType.Accept);
			
			if (fc.Run() != (int)ResponseType.Accept) {
				fc.Destroy();
				return;
			}
			
			try {
				ReportType rt = null;
				if (this.rbPersonsList.Active)
					rt = ReportType.GetInstance<SimplePersonsList>(this.selection,
					                                               this.PageSizeSelected,
					                                               fc.Filename);
				else if (this.rbAttendancesList.Active)
					rt = ReportType.GetInstance<SimpleAttendanceList>(this.selection,
					                                                  this.PageSizeSelected,
					                                                  fc.Filename);
				
				rt.Run();
			}
			catch(Exception ex) {
				MessageDialog md = new MessageDialog(this,
				                                     DialogFlags.Modal,
				                                     MessageType.Error,
				                                     ButtonsType.Ok,
				                                     true,
				                                     "format?");
				
				md.Title = "Error";
				//md.Text = "Hubo un error al generar el reporte:\n'" + ex.Message + "'";
				
				md.Run();
				md.Destroy();
			}
			
			fc.Destroy();
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

		protected virtual void OnPageSizeChanged (object sender, System.EventArgs e)
		{
			Console.WriteLine(this.cmbPageSize.ActiveText);
		}
	}
}
