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
using System.Reflection;
using Glade;
using Gtk;

namespace ZaspeSharp.GUI
{
	public class About
	{
		[Widget]
		private AboutDialog adAbout;
		
		public About(Window parent)
		{
			Glade.XML gxml = new Glade.XML ("main_window.glade", "adAbout", null);
			gxml.Autoconnect(this);
			
			Gdk.Pixbuf pf = new Gdk.Pixbuf("blue_fea.gif");
			this.adAbout.Logo = pf;
			
			this.adAbout.TransientFor = parent;
			
			// Set version
			Version assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
			this.adAbout.Version = assemblyVersion.Major + "." +
				assemblyVersion.Minor + "." + assemblyVersion.Build;
			
			this.adAbout.ShowAll();
		}
	}
}
