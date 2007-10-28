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

namespace ZaspeSharp.GUI
{
	public class AttendancesReport
	{
		private Assistant attendancesReportAssistant;
		
		public AttendancesReport(Gtk.Window parent)
		{
			this.attendancesReportAssistant = new Assistant();
			
			this.attendancesReportAssistant.TransientFor = parent;
			this.attendancesReportAssistant.WindowPosition = WindowPosition.CenterOnParent;
			this.attendancesReportAssistant.Modal = true;
			this.attendancesReportAssistant.SkipTaskbarHint = true;
			this.attendancesReportAssistant.Title = "Asistente para informe sobre asistencias";
			this.attendancesReportAssistant.SetSizeRequest(450, 300);
			
			// ####### Pages
			
			// First page - Introduction
			Label label1_page1 = new Label();
			label1_page1.Wrap = true;
			label1_page1.Text = "Este asistente lo guiará para generar un reporte sobre las asistencias " +
				"de las personas.";
			
			this.attendancesReportAssistant.AppendPage(label1_page1);
			this.attendancesReportAssistant.SetPageTitle(label1_page1, "Introducción");
			this.attendancesReportAssistant.SetPageType(label1_page1, AssistantPageType.Confirm);
			this.attendancesReportAssistant.SetPageComplete(label1_page1, true);
			
			this.attendancesReportAssistant.Cancel += this.OnAttendancesReportAssistantCancel;
			this.attendancesReportAssistant.Close += this.OnAttendancesReportAssistantClose;
			
			this.attendancesReportAssistant.ShowAll();
		}
		
		private void OnAttendancesReportAssistantCancel(object o, EventArgs args)
		{
			this.attendancesReportAssistant.Destroy();
		}
		
		private void OnAttendancesReportAssistantClose(object o, EventArgs args)
		{
			// TODO: Add code to generate and show report.
			this.attendancesReportAssistant.Destroy();
		}
	}
}
