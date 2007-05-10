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
using ZaspeSharp.Instruments;
using NUnit.Framework;

namespace ZaspeSharp.Tests
{
	[TestFixture]
	public class ControladorInstrumentosTest
	{
		InstrumentsManager controladorInstrumentos = InstrumentsManager.Instance;
		
		Instrument guitarra, batería, bajo;
		
		[SetUp]
		public void Init()
		{
			guitarra = controladorInstrumentos.AddInstrument("Guitarra");
			batería = controladorInstrumentos.AddInstrument("Batería");
			bajo = controladorInstrumentos.AddInstrument("Bajo");
		}
		
		[TearDown]
		public void Dispose()
		{
			foreach (Instrument instrumento in controladorInstrumentos.RetrieveAll())
				controladorInstrumentos.DeleteInstrument(instrumento.Name);
		}
		
		[Test]
		public void ObtenerControlador()
		{
			InstrumentsManager c1 = InstrumentsManager.Instance;
			InstrumentsManager c2 = InstrumentsManager.Instance;
			InstrumentsManager c3 = InstrumentsManager.Instance;
			
			Assert.AreSame(c1, c2);
			Assert.AreSame(c2, c3);
		}
		
		[Test]
		public void RecuperarInstrumento()
		{
			Instrument mismaGuitarra = controladorInstrumentos.Retrieve("Guitarra");
			
			Assert.AreEqual(guitarra.Name, mismaGuitarra.Name);
		}
		
		[Test]
		public void RecuperarInstrumentoCacheLimpio()
		{
			// Primero limpiamos la caché
			controladorInstrumentos.CleanCache();
			
			Instrument mismaGuitarra = controladorInstrumentos.Retrieve("Guitarra");
			
			Assert.AreEqual(guitarra.Name, mismaGuitarra.Name);
		}
		
		[Test]
		[ExpectedException(typeof(InstrumentDoesNotExistException))]
		public void RecuperarInstrumentoNoExistente()
		{
			Instrument i = controladorInstrumentos.Retrieve("Trompeta");
		}
		
		[Test]
		public void RecuperarTodosLosInstrumentos()
		{
			// Limpiamos el caché
			controladorInstrumentos.CleanCache();
			
			Instrument[] todosInstrumentos = controladorInstrumentos.RetrieveAll();
			
			// Verificamos que sean tres (milton, gisela y daiana)
			Assert.AreEqual(todosInstrumentos.Length, 3);
			
			Instrument mGuitarra = null;
			Instrument mBatería = null;
			Instrument mBajo = null;
			
			foreach (Instrument unInstrumento in todosInstrumentos)
			{
				if (unInstrumento.Name == "Guitarra")
					mGuitarra = unInstrumento;
				else if (unInstrumento.Name == "Batería")
					mBatería = unInstrumento;
				else if (unInstrumento.Name == "Bajo")
					mBajo = unInstrumento;
			}
			
			// Guitarra
			Assert.AreEqual(guitarra.Name, mGuitarra.Name, "Nombre");
			
			// Batería
			Assert.AreEqual(batería.Name, mBatería.Name, "Nombre");
			
			// Trompeta
			Assert.AreEqual(bajo.Name, mBajo.Name, "Nombre");
		}
		
		[Test]
		public void IngresarInstrumento()
		{
			Instrument trompeta = controladorInstrumentos.AddInstrument("Trompeta");
			
			// Limpiamos el caché antes de comprobar los datos, así obligamos a
			// Gentle a buscar el objeto en la base de datos.
			controladorInstrumentos.CleanCache();
			
			trompeta = controladorInstrumentos.Retrieve("Trompeta");
			
			// Chequemos los datos
			Assert.AreEqual("Trompeta", trompeta.Name, "Nombre");
		}
		
		[Test]
		[ExpectedException(typeof(InstrumentExistsException))]
		public void IngresarInstrumentoDuplicado()
		{
			Instrument guitarraOtraVez = controladorInstrumentos.AddInstrument("Guitarra");
		}
		
		[Test]
		public void EliminarInstrumento1()
		{
			controladorInstrumentos.DeleteInstrument(guitarra.Name);
		}
		
		[Test]
		[ExpectedException(typeof(InstrumentDoesNotExistException))]
		public void EliminarInstrumento2()
		{
			controladorInstrumentos.DeleteInstrument(guitarra.Name);

			Instrument mismaGuitarra = controladorInstrumentos.Retrieve(guitarra.Name);
		}
		
		[Test]
		public void EliminarInstrumentoCacheLimpio1()
		{
			controladorInstrumentos.CleanCache();
			
			controladorInstrumentos.DeleteInstrument(guitarra.Name);
		}
		
		[Test]
		[ExpectedException(typeof(InstrumentDoesNotExistException))]
		public void EliminarInstrumentoCacheLimpio2()
		{
			controladorInstrumentos.CleanCache();
			
			controladorInstrumentos.DeleteInstrument(guitarra.Name);

			Instrument mismaGuitarra = controladorInstrumentos.Retrieve(guitarra.Name);
		}
		
		[Test]
		[ExpectedException(typeof(InstrumentDoesNotExistException))]
		public void EliminarInstrumentoNoExistente()
		{
			controladorInstrumentos.DeleteInstrument("NombreQueNuncaVoyAusarParaUnInstrumento");
		}
	}
}
