/*
    Zaspe# - Attendance management
    Copyright (C) 2006, 2007 Milton Pividori

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Diagnostics;
using System.Collections;
using Gentle.Framework;

namespace ZaspeSharp.Events
{
	public partial class EventsManager
	{
		#region Retrieve methods
		public Event Retrieve(DateTime date)
		{
			// Los segundos no cuentan
			DateTime dateWithoutSeconds = new DateTime(date.Year, date.Month, date.Day,
			                                         date.Hour, date.Minute, 0);
			
			Key key = new Key(typeof(Event), true);
			key.Add("date", dateWithoutSeconds);
			
			Event anEvent = Broker.SessionBroker.TryRetrieveInstance(
				typeof(Event), key) as Event;
			
			if (anEvent != null)
				return anEvent;
			
			throw new EventDoesNotExistException();
		}
		
		public Event[] RetrieveAll()
		{
			SqlBuilder sb = new SqlBuilder(StatementType.Select, typeof(Event));
			SqlStatement stmt = sb.GetStatement(true);
			
			Event[] events = (Event[])ObjectFactory.GetCollection(typeof(Event), stmt.Execute());
			return events;
		}
		
		public Event[] RetrieveLast(int nroEventosRecuperar) {			
			SqlBuilder sb = new SqlBuilder(StatementType.Select, typeof(Event));
			sb.SetRowLimit(nroEventosRecuperar);
			sb.AddOrderByField(false, "fecha");
			
			SqlStatement stmt = sb.GetStatement(true);
			
			Event[] events = (Event[])ObjectFactory.GetCollection(typeof(Event), stmt.Execute());
			return events;
		}
		#endregion
		
		#region Métodos de ingreso
		public Event IngresarEvento(DateTime fecha, string nombre, Types tipoEvento)
		{
			// Quitamos los segundos de la fecha
			DateTime fechaSinSegundos = new DateTime(fecha.Year, fecha.Month, fecha.Day,
			                                         fecha.Hour, fecha.Minute, 0);
			
			return IngresarEvento(fechaSinSegundos, nombre, (int)tipoEvento, null, null);
		}
		
		public Event IngresarEvento(DateTime fecha, string nombre, int tipoEvento,
			string objetivos, string observaciones) {
			
			try {
				Retrieve(fecha);
			}
			catch (EventDoesNotExistException) {
				Event nuevoEvento = new Event(fecha, nombre, tipoEvento,
				                                objetivos, observaciones);
				nuevoEvento.Persist();
				
				return nuevoEvento;
			}
			
			throw new EventExistsException("Se está intentando ingresar un evento que" +
					" ya existe en la base de datos.");
		}
		#endregion
		
		#region Métodos de eliminación
		public void EliminarEvento(DateTime fecha)
		{
			Event unEvento;
			
			try {
				unEvento = Retrieve(fecha);
			}
			catch (EventDoesNotExistException) {
				throw new EventDoesNotExistException("Se está intentando eliminar un evento que" + 
					" no existe en la base de datos.");
			}
			
			unEvento.Remove();
		}
		#endregion
		
		#region Otros métodos
		[Conditional("DEBUG")]
		public void LimpiarCache()
		{
			Gentle.Common.CacheManager.Clear();
		}
		#endregion
	}
}
