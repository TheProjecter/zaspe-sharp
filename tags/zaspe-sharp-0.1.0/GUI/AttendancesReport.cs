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
using Glade;
using Gtk;

namespace ZaspeSharp.GUI
{
	public class AttendancesReport
	{
		[Widget]
		private Dialog dialogShowStats;
		
		[Widget]
		private ComboBox cmbPageSize;
		
		public AttendancesReport(Gtk.Window parent)
		{
			Glade.XML gxml = new Glade.XML ("show_stats.glade", "dialogShowStats",
			                                null);
			gxml.Autoconnect(this);
			
			this.cmbPageSize.Active = 0;
			
			this.dialogShowStats.ShowAll();
		}
		
		private void OnCancelClicked(object o, EventArgs args)
		{
			this.dialogShowStats.Destroy();
		}
		
		private void OnAcceptClicked(object o, EventArgs args)
		{
			this.dialogShowStats.Destroy();
		}
	}
}
