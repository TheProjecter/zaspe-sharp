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
	public class MainWindow
	{
		[Widget]
		private Gtk.Window mainWindow;
		
		[Widget]
		private ToolButton tbAddPerson;
		
		[Widget]
		private ToolButton tbAddEvent;
		
		[Widget]
		private ToolButton tbQuit;
		
		public MainWindow()
		{
			Glade.XML gxml = new Glade.XML ("main_window.glade", "mainWindow", null);
			gxml.Autoconnect(this);
			
//			Gdk.Pixbuf pf = new Gdk.Pixbuf(System.Reflection.Assembly.GetExecutingAssembly(), "add.png");
//			Image im = new Image();
//			im.Pixbuf = pf;
//			
//			this.tbAgregarPersona.IconName = null;
//			this.tbAgregarPersona.IconWidget = im;

			this.mainWindow.Icon = new Gdk.Pixbuf(null, "blue_fea.gif");
			this.mainWindow.Show();
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
		
		public void OnMenuItemQuitActivate(object o, EventArgs args)
		{
			this.Quit();
		}
		
		public void OnImageMenuItemAboutActivate(object o, EventArgs args)
		{
			About ad = new About(this.mainWindow);
		}
		
		private void Quit()
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
