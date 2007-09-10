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

namespace MonoReporter
{
	public class Page
	{
		public Page()
		{
			PrintOperation printOperation = new PrintOperation();
			
			printOperation.BeginPrint += this.BeginPrint;
			printOperation.DrawPage += this.DrawPage;
			printOperation.EndPrint += this.EndPrint;
			
			printOperation.Run(PrintOperationAction.PrintDialog, null);
		}
		
		public void BeginPrint(object o, BeginPrintArgs args)
		{
			
		}
		
		public void DrawPage(object o, DrawPageArgs args)
		{
			
		}
		
		public void EndPrint(object o, EndPrintArgs args)
		{
			
		}
	}
}
