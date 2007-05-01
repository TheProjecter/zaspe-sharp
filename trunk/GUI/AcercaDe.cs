/* 
   Zaspe# - Attendance management
   Copyright (C) 2006, 2007 Milton Pividori

   Zaspe# is free software; you can redistribute it and/or
   modify it under the terms of the GNU General Public License
   as published by the Free Software Foundation; either version 2
   of the License, or (at your option) any later version.

   Zaspe# is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with Zaspe#; if not, write to the Free Software
   Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

using System;
using Glade;
using Gtk;

namespace ZaspeSharp.GUI
{
	public class AcercaDe
	{
		[Widget]
		AboutDialog adAcercaDe;
		
		public AcercaDe(Window parent)
		{
			Glade.XML gxml = new Glade.XML (null, "main_window.glade", "adAcercaDe", null);
			gxml.Autoconnect(this);
			
			Gdk.Pixbuf pf = new Gdk.Pixbuf(System.Reflection.Assembly.GetExecutingAssembly(), "blue_fea.gif");
			this.adAcercaDe.Logo = pf;
			
			this.adAcercaDe.TransientFor = parent;
			
			this.adAcercaDe.Show();
		}
		
		public void OnClose (object o, EventArgs args)
		{
			System.Console.WriteLine("aver av er");
		}
	}
}
