//  MonoReporter - Report tool for Mono
//  Copyright (C) 2006,2007 Milton Pividori
//
//  This file is part of MonoReporter.
//
//  MonoReporter is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 3 of the License, or
//  (at your option) any later version.
//
//  MonoReporter is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using Gtk;

using MonoReporter;

public class MainWindow {
	static Gtk.Window myWindow;
	static void Main ()
	{
		Application.Init ();
		myWindow = new Window ("This is a window");
		myWindow.DeleteEvent += OnDelete;
		myWindow.SetDefaultSize (200, 200);

		//Put a button in the Window
		Button button = new Button ("Imprimir");
		button.Clicked += OnButtonClicked;
		myWindow.Add (button);
		myWindow.ShowAll ();
		Application.Run ();
	}
	
	static void OnButtonClicked(object o, EventArgs args)
	{
		Page page = new Page();
		page.Run(myWindow);
	}

     static void OnDelete (object o, DeleteEventArgs e)
     {
		Application.Quit();
     }     
}