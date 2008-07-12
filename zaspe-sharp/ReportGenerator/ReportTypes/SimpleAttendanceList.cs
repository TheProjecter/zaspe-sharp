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

using iTextSharp.text;
using iTextSharp.text.pdf;

using ZaspeSharp.Persons;
using ZaspeSharp.Attendances;

namespace ZaspeSharp.ReportGenerator
{
	internal class SimpleAttendanceList : ReportType
	{
		private static SimpleAttendanceList instance;
		
		private SimpleAttendanceList(Selection s) : base(s, "lista_asistencias.pdf", "Asistencias")
		{
		}
		
		public static SimpleAttendanceList GetInstance(Selection selection) {
			if (instance == null)
				instance = new SimpleAttendanceList(selection);
			
			instance.selection = selection;
			
			return instance;
		}
		
		public override void MakeReport ()
		{
			// Subtitle
			Chunk c = new Chunk(this.selection.Events[0].Name,
			                    FontFactory.GetFont(FontFactory.HELVETICA, 14, Font.BOLD));
			Paragraph par = new Paragraph(c);
			par.Alignment = Rectangle.ALIGN_CENTER;
			
			this.doc.Add(par);
			
			// List
			Table t = new Table(2);
			t.Border = 0;
			t.DefaultCellBorder = 0;
			
			Cell cell = new Cell();
			cell.HorizontalAlignment = Element.ALIGN_CENTER;
			t.DefaultCell = cell; // Default cell
			
			Font fuenteTitulo = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 14, Font.UNDERLINE);

			cell = new Cell();
			Chunk texto = new Chunk("Nombre y Apellido", fuenteTitulo);
			cell.Add(texto);
			t.AddCell(cell);
			
			cell = new Cell();
			texto = new Chunk("¿Asistió?", fuenteTitulo);
			cell.Add(texto);
			t.AddCell(cell);
			
			Font fuenteDatos = FontFactory.GetFont(FontFactory.HELVETICA, 10);
			
			Font fuenteSi = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
			fuenteSi.Color = Color.GREEN;
			
			Font fuenteNo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
			fuenteNo.Color = Color.RED;
			
			foreach (Person p in this.selection.Persons) {
				cell = new Cell();
				texto = new Chunk(p.Name + " " + p.Surname, fuenteDatos);
				cell.Add(texto);
				t.AddCell(cell);
				
				cell = new Cell();
				
				if (AttendancesManager.Instance.Attended(p, this.selection.Events[0]))
					texto = new Chunk("Si", fuenteSi);
				else
					texto = new Chunk("No", fuenteNo);
				
				cell.Add(texto);
				t.AddCell(cell);
			}
			
			this.doc.Add(t);
			
			this.doc.Close();
		}

	}
}
