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
using ZaspeSharp.Events;
using NUnit.Framework;

namespace ZaspeSharp.Tests
{
	[TestFixture]
	public class ControladorEventosTest
	{
		EventsManager controladorEventos = EventsManager.Instance;
		
		Event ensayoSabado, misaDomingo, otroEvento;
		
		[SetUp]
		public void Init()
		{
			ensayoSabado = controladorEventos.IngresarEvento(new DateTime(DateTime.Now.Year, 8, 11),
				"Ensayo", Types.ENSAYO);
			misaDomingo = controladorEventos.IngresarEvento(new DateTime(DateTime.Now.Year, 8, 12),
				"Misa", Types.MISA);
			otroEvento = controladorEventos.IngresarEvento(new DateTime(DateTime.Now.Year, 8, 13),
				"Otro evento", Types.OTRO);
		}
		
		[TearDown]
		public void Dispose()
		{
			foreach (Event unEvento in controladorEventos.RetrieveAll())
				controladorEventos.EliminarEvento(unEvento.Date);
		}
		
		[Test]
		public void ValidarEventosIngresados()
		{
			Assert.IsNotNull(ensayoSabado);
			Assert.AreEqual(new DateTime(DateTime.Now.Year, 8, 11), ensayoSabado.Date);
			Assert.AreEqual("Ensayo", ensayoSabado.Name);
			Assert.AreEqual((int)Types.ENSAYO, ensayoSabado.EventType);
			Assert.IsNull(ensayoSabado.Goals);
			Assert.IsNull(ensayoSabado.Observations);
			
			Assert.IsNotNull(misaDomingo);
			Assert.AreEqual(new DateTime(DateTime.Now.Year, 8, 12), misaDomingo.Date);
			Assert.AreEqual("Misa", misaDomingo.Name);
			Assert.AreEqual((int)Types.MISA, misaDomingo.EventType);
			Assert.IsNull(misaDomingo.Goals);
			Assert.IsNull(misaDomingo.Observations);
			
			Assert.IsNotNull(otroEvento);
			Assert.AreEqual(new DateTime(DateTime.Now.Year, 8, 13), otroEvento.Date);
			Assert.AreEqual("Otro evento", otroEvento.Name);
			Assert.AreEqual((int)Types.OTRO, otroEvento.EventType);
			Assert.IsNull(otroEvento.Goals);
			Assert.IsNull(otroEvento.Observations);
		}
		
		[Test]
		public void ObtenerControlador()
		{
			EventsManager c1 = EventsManager.Instance;
			EventsManager c2 = EventsManager.Instance;
			EventsManager c3 = EventsManager.Instance;
			
			Assert.AreSame(c1, c2);
			Assert.AreSame(c2, c3);
		}
		
		[Test]
		public void RecuperarEvento()
		{
			Event es = controladorEventos.Retrieve(new DateTime(DateTime.Now.Year, 8, 11));
			
			Assert.AreEqual(ensayoSabado.Date, es.Date);
			Assert.AreEqual(ensayoSabado.Name, es.Name);
			Assert.AreEqual(ensayoSabado.EventType, es.EventType);
			Assert.AreEqual(ensayoSabado.Goals, es.Goals);
			Assert.AreEqual(ensayoSabado.Observations, es.Observations);
		}
		
		[Test]
		public void RecuperarEventoCacheLimpio()
		{
			// Primero limpiamos la caché
			controladorEventos.LimpiarCache();
			
			Event md = controladorEventos.Retrieve(new DateTime(DateTime.Now.Year, 8, 12));
			
			Assert.AreEqual(misaDomingo.Date, md.Date);
			Assert.AreEqual(misaDomingo.Name, md.Name);
			Assert.AreEqual(misaDomingo.EventType, md.EventType);
			Assert.AreEqual(misaDomingo.Goals, md.Goals);
			Assert.AreEqual(misaDomingo.Observations, md.Observations);
		}
		
		[Test]
		[ExpectedException(typeof(EventDoesNotExistException))]
		public void RecuperarEventoNoExistente()
		{
			Event e = controladorEventos.Retrieve(new DateTime(DateTime.Now.Year, 1, 1));
		}
		
		[Test]
		public void RecuperarTodosLosEventos()
		{
			// Limpiamos el caché
			controladorEventos.LimpiarCache();
			
			Event[] todosEventos = controladorEventos.RetrieveAll();
			
			// Verificamos que sean tres (milton, gisela y daiana)
			Assert.AreEqual(3, todosEventos.Length);
			
			Event mEnsayoSabado = null;
			Event mMisaDomingo = null;
			Event mOtroEvento = null;
			
			foreach (Event unEvento in todosEventos)
			{
				if (unEvento.Name == "Ensayo")
					mEnsayoSabado = unEvento;
				else if (unEvento.Name == "Misa")
					mMisaDomingo = unEvento;
				else if (unEvento.Name == "Otro evento")
					mOtroEvento = unEvento;
			}
			
			// Ensayo del sábado
			Assert.AreEqual(ensayoSabado.Date, mEnsayoSabado.Date);
			Assert.AreEqual(ensayoSabado.Name, mEnsayoSabado.Name);
			Assert.AreEqual(ensayoSabado.EventType, mEnsayoSabado.EventType);
			Assert.AreEqual(ensayoSabado.Goals, mEnsayoSabado.Goals);
			Assert.AreEqual(ensayoSabado.Observations, mEnsayoSabado.Observations);
			
			// Misa del domingo
			Assert.AreEqual(misaDomingo.Date, mMisaDomingo.Date);
			Assert.AreEqual(misaDomingo.Name, mMisaDomingo.Name);
			Assert.AreEqual(misaDomingo.EventType, mMisaDomingo.EventType);
			Assert.AreEqual(misaDomingo.Goals, mMisaDomingo.Goals);
			Assert.AreEqual(misaDomingo.Observations, mMisaDomingo.Observations);
			
			// Otro evento
			Assert.AreEqual(otroEvento.Date, mOtroEvento.Date);
			Assert.AreEqual(otroEvento.Name, mOtroEvento.Name);
			Assert.AreEqual(otroEvento.EventType, mOtroEvento.EventType);
			Assert.AreEqual(otroEvento.Goals, mOtroEvento.Goals);
			Assert.AreEqual(otroEvento.Observations, mOtroEvento.Observations);
		}
		
		[Test]
		public void IngresarEvento()
		{
			DateTime fechaEvento = new DateTime(DateTime.Now.Year, 10, 11, 18, 30, 0);
			
			Event ensayo = controladorEventos.IngresarEvento(fechaEvento, "Ensayo de algún día", Types.OTRO);
			
			// Limpiamos el caché antes de comprobar los datos, así obligamos a
			// Gentle a buscar el objeto en la base de datos.
			controladorEventos.LimpiarCache();
			
			Event ens = controladorEventos.Retrieve(fechaEvento);
			
			// Chequemos los datos
			// La fecha y hora
			Assert.AreEqual(ens.Date.Year, fechaEvento.Year);
			Assert.AreEqual(ens.Date.Month, fechaEvento.Month);
			Assert.AreEqual(ens.Date.Day, fechaEvento.Day);
			Assert.AreEqual(ens.Date.Hour, fechaEvento.Hour);
			Assert.AreEqual(ens.Date.Minute, fechaEvento.Minute);
			Assert.AreEqual(ens.Date.Second, fechaEvento.Second);
			
			Assert.AreEqual("Ensayo de algún día", ens.Name);
			Assert.AreEqual((int)Types.OTRO, ens.EventType);
			Assert.IsNull(ens.Goals);
			Assert.IsNull(ens.Observations);
		}
		
		[Test]
		[ExpectedException(typeof(EventExistsException))]
		public void IngresarEventoDuplicado()
		{
			Event eventoDuplicado = controladorEventos.IngresarEvento(
				new DateTime(DateTime.Now.Year, 8, 11), "Ensayo", Types.ENSAYO);
		}
		
		[Test]
		public void EliminarEvento1()
		{
			controladorEventos.EliminarEvento(ensayoSabado.Date);
		}
		
		[Test]
		[ExpectedException(typeof(EventDoesNotExistException))]
		public void EliminarEvento2()
		{
			controladorEventos.EliminarEvento(ensayoSabado.Date);

			Event es = controladorEventos.Retrieve(ensayoSabado.Date);
		}
		
		[Test]
		public void EliminarEventoCacheLimpio1()
		{
			controladorEventos.LimpiarCache();
			
			controladorEventos.EliminarEvento(ensayoSabado.Date);
		}
		
		[Test]
		[ExpectedException(typeof(EventDoesNotExistException))]
		public void EliminarEventoCacheLimpio2()
		{
			controladorEventos.LimpiarCache();
			
			controladorEventos.EliminarEvento(ensayoSabado.Date);

			Event es = controladorEventos.Retrieve(ensayoSabado.Date);
		}
		
		[Test]
		[ExpectedException(typeof(EventDoesNotExistException))]
		public void EliminarEventoNoExistente()
		{
			controladorEventos.EliminarEvento(new DateTime(1950, 2, 2));
		}
	}
}
