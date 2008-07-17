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
using System.IO;

using iTextSharp.text;
using iTextSharp.text.pdf;

using ZaspeSharp.Persons;

namespace ZaspeSharp.ReportGenerator
{
	internal class SimplePersonsList : ReportType
	{
//		private static SimplePersonsList instance;
						
//		private SimplePersonsList(Selection s, Rectangle pageSize, string reportFile)
//			: base(s, pageSize, reportFile, "Lista de personas")
//		{
//		}
		
		public SimplePersonsList()
		{
			this.reportTitle = "Lista de personas";
		}
		
//		
//		public static SimplePersonsList GetInstance(Selection selection,
//		                                            Rectangle pageSize,
//		                                            string reportFile) {
//			
//			if (instance == null)
//				instance = new SimplePersonsList(selection, pageSize, reportFile);
//			else {
//				instance.selection = selection;
//				instance.reportFile = reportFile;
//			}
//			
//			if (!this.doc.IsOpen())
//				this.doc.Open();
//			
//			return instance;
//		}
		
		public override void MakeReport ()
		{
			Table t = new Table(3);
			t.Border = 0;
			t.DefaultCellBorder = 0;
			
			Cell cell = new Cell();
			cell.HorizontalAlignment = Element.ALIGN_CENTER;
			t.DefaultCell = cell; // Default cell
			
			Font fuenteTitulo = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 14, Font.UNDERLINE);

			cell = new Cell();
			Chunk texto = new Chunk("Apellido", fuenteTitulo);
			cell.Add(texto);
			t.AddCell(cell);
			
			cell = new Cell();
			texto = new Chunk("Nombre", fuenteTitulo);
			cell.Add(texto);
			t.AddCell(cell);
			
			cell = new Cell();
			texto = new Chunk("E-Mail", fuenteTitulo);
			cell.Add(texto);
			t.AddCell(cell);
			
			Font fuenteDatos = FontFactory.GetFont(FontFactory.HELVETICA, 10);
			
			foreach (Person p in this.selection.Persons) {
				cell = new Cell();
				texto = new Chunk(p.Surname, fuenteDatos);
				cell.Add(texto);
				t.AddCell(cell);
				
				cell = new Cell();
				texto = new Chunk(p.Name, fuenteDatos);
				cell.Add(texto);
				t.AddCell(cell);
				
				cell = new Cell();
				texto = new Chunk(p.EMail, fuenteDatos);
				cell.Add(texto);
				t.AddCell(cell);
			}
			
			this.doc.Add(t);
		}

	}
}
