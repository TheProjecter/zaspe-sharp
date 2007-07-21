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

namespace ZaspeSharp.Instruments
{
	public partial class InstrumentsManager
	{
		#region Retrieve methods
		public Instrument Retrieve(string name)
		{
			Key key = new Key(typeof(Instrument), true);
			key.Add("name", name);
			
			Instrument anInstrument = Broker.SessionBroker.TryRetrieveInstance(
				typeof(Instrument), key) as Instrument;
			
			if (anInstrument != null)
				return anInstrument;
			
			throw new InstrumentDoesNotExistException("No existe un instrumento con ese nombre");
		}
		
		public Instrument[] RetrieveAll()
		{
			SqlBuilder sb = new SqlBuilder(StatementType.Select, typeof(Instrument));
			SqlStatement stmt = sb.GetStatement(true);

			Instrument[] instruments = (Instrument[])ObjectFactory.GetCollection(typeof(Instrument), stmt.Execute());
			return instruments;
		}
		#endregion
		
		#region Entry methods
		public Instrument AddInstrument(string name)
		{
			// Check if an other instrument with the same name already exists
			try {
				Retrieve(name);
			}
			catch (InstrumentDoesNotExistException) {
				Instrument newInstrument = new Instrument(name);
				newInstrument.Persist();
				
				return newInstrument;
			}
			
			throw new InstrumentExistsException("Se está intentando ingresar un instrumento que" +
					" ya existe en la base de datos.");
		}
		#endregion
		
		#region Deletion methods
		public void DeleteInstrument(string name)
		{
			Instrument anInstrument;
			
			try {
				anInstrument = Retrieve(name);
			}
			catch (InstrumentDoesNotExistException) {
				throw new InstrumentDoesNotExistException("Se está intentando eliminar un instrumento que" + 
					" no existe en la base de datos.");
			}
			
			anInstrument.Remove();
		}
		#endregion
		
		#region Other methods
		[Conditional("DEBUG")]
		public void CleanCache()
		{
			Gentle.Common.CacheManager.Clear();
		}
		#endregion
	}
}
