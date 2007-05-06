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
		private ToolButton tbAgregarPersona;
		
		[Widget]
		private ToolButton tbAgregarEvento;
		
		[Widget]
		private ToolButton tbSalir;
		
		public MainWindow()
		{
			Gtk.AboutDialog ad;
			Glade.XML gxml = new Glade.XML ("main_window.glade", "mainWindow", null);
			gxml.Autoconnect(this);
			
//			Gdk.Pixbuf pf = new Gdk.Pixbuf(System.Reflection.Assembly.GetExecutingAssembly(), "add.png");
//			Image im = new Image();
//			im.Pixbuf = pf;
//			
//			this.tbAgregarPersona.IconName = null;
//			this.tbAgregarPersona.IconWidget = im;
			
			this.mainWindow.Show();
		}
		
		public void OnWindowDeleteEvent(object o, DeleteEventArgs args)
		{
			this.Salir();
			args.RetVal = true;
		}
		
		public void OnToolButtonSalirClicked(object o, EventArgs args)
		{
			this.Salir();
		}
		
		public void OnMenuItemSalirActivate(object o, EventArgs args)
		{
			System.Console.WriteLine("aja");
			this.Salir();
		}
		
		public void OnMenuItemSalirActivate(object o, ButtonPressEventArgs args)
		{
			System.Console.WriteLine("aja2");
			this.Salir();
		}
		
		public void OnImageMenuItemAcercaDeActivate(object o, EventArgs args)
		{
			AcercaDe ad = new AcercaDe(this.mainWindow);
		}
		
		private void Salir()
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
