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
		
		public SimplePersonsList(Selection s) : base(s, "lista_personas.pdf", "Lista de personas")
		{
		}
		
		public override void MakeReport ()
		{
			Table t = new Table(3);
			t.Border = 1;
			t.BorderColor = Color.BLACK;
			t.Alignment = 1;
			t.DefaultHorizontalAlignment = 0;
			t.DefaultVerticalAlignment = 2;
			
			Font fuenteTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, Font.BOLD);

			Cell cell = new Cell();
			Paragraph texto = new Paragraph(new Chunk("Apellido", fuenteTitulo));
			cell.Add(texto);
			cell.Header = true;
			cell.HorizontalAlignment = 1;
			cell.VerticalAlignment = 1;
			t.AddCell(cell);
			
			cell = new Cell();
			texto = new Paragraph(new Chunk("Nombre", fuenteTitulo));
			cell.Add(texto);
			cell.Header = true;
			cell.HorizontalAlignment = 1;
			cell.VerticalAlignment = 50;
			t.AddCell(cell);
			
			cell = new Cell();
			texto = new Paragraph(new Chunk("E-Mail", fuenteTitulo));
			cell.Add(texto);
			cell.Header = true;
			cell.HorizontalAlignment = 1;
			cell.VerticalAlignment = 1;
			t.AddCell(cell);
			
			foreach (Person p in this.selection.Persons) {
				t.AddCell(p.Surname);
				t.AddCell(p.Name);
				t.AddCell(p.EMail);
			}
			
			this.doc.Add(t);
			
			this.doc.Close();
		}

	}
}
