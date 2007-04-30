/* Zaspe# - Attendance management
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
	public class MainWindow
	{
		[Widget]
		Window mainWindow;
		
		[Widget]
		ToolButton tbAgregarPersona;
		
		[Widget]
		ToolButton tbAgregarEvento;
		
		[Widget]
		ToolButton tbSalir;
		
		public MainWindow()
		{
			Glade.XML gxml = new Glade.XML (null, "main_window.glade", "mainWindow", null);
			gxml.Autoconnect(this);
			
			// Cargo los íconos en la barra de menúes
			Gdk.Pixbuf pf = Gdk.Pixbuf.LoadFromResource("add.png");
			
			this.mainWindow.Show();
		}
		
		public void OnWindowDeleteEvent(object o, DeleteEventArgs args)
		{
			Application.Quit();
		}
		
		public static void Main(String[] args)
		{
			Application.Init();
			new MainWindow();
			Application.Run();
		}
	}
}
