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
			this.tvPersons.Selection.Mode = SelectionMode.Multiple;
			
			
			
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

			// Single selection
			this.tvEvents.Selection.Mode = SelectionMode.Single;
			
			
			// ######### Add data ###########
			// Add all persons
			ListStore personsModel = (ListStore)this.tvPersons.Model;
			Person[] allPersons = PersonsManager.Instance.RetrieveAll();
			
			foreach (Person p in allPersons)
				personsModel.AppendValues(p);
			
			// Select all persons
			this.tvPersons.Selection.SelectAll();
			
			
			// Add past events
			ListStore eventsModel = (ListStore)this.tvEvents.Model;
			Event[] lastEvents = EventsManager.Instance.RetrieveLast(10);
			
			if (lastEvents.Length > 0) {
				TreeIter firstIter = eventsModel.AppendValues(lastEvents[0]); 
				
				for (int i=1; i<lastEvents.Length; i++) {
					eventsModel.AppendValues(lastEvents[i]);
				}
				
				// Select first event by default
				this.tvEvents.Selection.SelectIter(firstIter);
			}
			
			// Disable persons report if there is no person.
			this.CanMakePersonsReport(allPersons.Length > 0);
			
			// Disable events report if there is no event.
			// TODO: Events report disable for now.
			//this.CanMakeEventsReport(lastEvents.Length == 0);
				
			
			// Disable attendances report if there is no person or no event.
			this.CanMakeAttendancesReport(allPersons.Length > 0 && lastEvents.Length > 0);
			
			// If no report type is available, disable Accept button, and report page size options
			this.CanMakeAnyReport(this.rbAttendancesList.Sensitive ||
			    this.rbEventsList.Sensitive ||
			    this.rbPersonsList.Sensitive);
		}
		
		public ReportGUI(Gtk.Window parent) : this()
		{
			this.TransientFor = parent;
			this.Modal = true;
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
		
		private void CanMakeAnyReport(bool val) {
			this.btnAccept.Sensitive = val;
			this.fraReportOptions.Sensitive = val;
		}
		
		private void CanMakePersonsReport(bool val) {
			this.rbPersonsList.Sensitive = val;
			this.fraPersons.Sensitive = val;
		}
		
		private void CanMakeEventsReport(bool val) {
			this.rbEventsList.Sensitive = val;
			this.fraEvents.Sensitive = val;
		}
		
		private void CanMakeAttendancesReport(bool val) {
			this.rbAttendancesList.Sensitive = val;
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
			
			// Add "pdf" extension if it's not present
			string filename = fc.Filename;
			if (!fc.Filename.ToLower().EndsWith(".pdf"))
				filename += ".pdf";
			
			// Run report according to the type selected
			try {
				ReportType rt = null;
				if (this.rbPersonsList.Active)
					rt = ReportType.GetInstance<SimplePersonsList>(this.selection,
					                                               this.PageSizeSelected,
					                                               filename);
				else if (this.rbAttendancesList.Active)
					rt = ReportType.GetInstance<SimpleAttendanceList>(this.selection,
					                                                  this.PageSizeSelected,
					                                                  filename);
				
				rt.Run();
			}
			catch(Exception ex) {
				MessageDialog errorMessage = new MessageDialog(this,
				                                     DialogFlags.Modal,
				                                     MessageType.Error,
				                                     ButtonsType.Ok,
				                                     true,
				                                     "Hubo un error al generar el reporte:\n'" + ex.Message + "'");
				
				errorMessage.Title = "Error";
				
				errorMessage.Run();
				errorMessage.Destroy();
			}
			
			fc.Destroy();
			
			// Success message
			MessageDialog successMessage = new MessageDialog(this,
			                                     DialogFlags.Modal,
			                                     MessageType.Info,
			                                     ButtonsType.Ok,
			                                     true,
			                                     "Reporte generado correctamente.");
				
			successMessage.Title = "Reporte generado";
			
			successMessage.Run();
			successMessage.Destroy();
		}

		protected virtual void OnResponse (object o, Gtk.ResponseArgs args)
		{
			if (args.ResponseId.Equals(Gtk.ResponseType.DeleteEvent) ||
				    args.ResponseId.Equals(Gtk.ResponseType.Close))
				this.Destroy();
		}

		protected virtual void OnPersonsListToggled (object sender, System.EventArgs e)
		{
			this.fraPersons.Sensitive = true;
			this.fraEvents.Sensitive = false;
		}

		protected virtual void OnEventsListToggled (object sender, System.EventArgs e)
		{
			this.fraPersons.Sensitive = false;
			this.fraEvents.Sensitive = true;
		}

		protected virtual void OnAttendancesListToggled (object sender, System.EventArgs e)
		{
			this.fraPersons.Sensitive = true;
			this.fraEvents.Sensitive = true;
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
