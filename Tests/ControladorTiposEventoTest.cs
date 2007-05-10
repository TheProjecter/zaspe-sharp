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
using System.Collections.Generic;

using NUnit.Framework;
using ZaspeSharp.Events;

namespace ZaspeSharp.Tests
{
	[TestFixture]
	public class ControladorTiposEventoTest
	{
		EventTypesManager controladorTiposEvento = EventTypesManager.Instance;

		// Tipos de evento para realizar las pruebas
		EventType ensayo, misa, otro;
		
		[TestFixtureSetUp]
		public void Inicio() {
			ensayo = controladorTiposEvento.Retrieve("Ensayo");
			misa = controladorTiposEvento.Retrieve("Misa");
			otro = controladorTiposEvento.Retrieve("Otro");
		}
		
		[Test]
		public void ValidarTiposEventoIngresados() {
			// Valido que los tipos de evento se hayan leído correctamente
			Assert.IsNotNull(ensayo);
			Assert.AreEqual("Ensayo", ensayo.Name);
			
			Assert.IsNotNull(misa);
			Assert.AreEqual("Misa", misa.Name);
			
			Assert.IsNotNull(otro);
			Assert.AreEqual("Otro", otro.Name);
			
			//Assert.AreNotEqual(ensayo.Id, misa.Id);
			//Assert.AreNotEqual(ensayo.Id, otro.Id);
			//Assert.AreNotEqual(misa.Id, otro.Id);
		}
		
		[Test]
		public void ObtenerControlador()
		{
			EventTypesManager c1 = EventTypesManager.Instance;
			EventTypesManager c2 = EventTypesManager.Instance;
			EventTypesManager c3 = EventTypesManager.Instance;
			
			Assert.AreSame(c1, c2);
			Assert.AreSame(c2, c3);
		}
		
		[Test]
		public void RecuperarTipoEvento()
		{
			EventType unTipoEvento;
			
			unTipoEvento = controladorTiposEvento.Retrieve(ensayo.Name);
			Assert.AreEqual(ensayo.Id, unTipoEvento.Id);
			Assert.AreEqual(ensayo.Name, unTipoEvento.Name);
			
			unTipoEvento = controladorTiposEvento.Retrieve(misa.Name);
			Assert.AreEqual(misa.Id, unTipoEvento.Id);
			Assert.AreEqual(misa.Name, unTipoEvento.Name);
		}
		
		[Test]
		public void RecuperarTipoEventoCacheLimpio()
		{
			controladorTiposEvento.CleanCache();
			
			EventType unTipoEvento;
			
			unTipoEvento = controladorTiposEvento.Retrieve(ensayo.Name);
			Assert.AreEqual(ensayo.Id, unTipoEvento.Id);
			Assert.AreEqual(ensayo.Name, unTipoEvento.Name);
			
			unTipoEvento = controladorTiposEvento.Retrieve(misa.Name);
			Assert.AreEqual(misa.Id, unTipoEvento.Id);
			Assert.AreEqual(misa.Name, unTipoEvento.Name);
		}
		
		[Test]
		[ExpectedException(typeof(EventTypeDoesNotExistException))]
		public void RecuperarTipoEventoNoExistente()
		{
			EventType e = controladorTiposEvento.Retrieve("unTipoEventoQueNoExiste234");
		}
		
		[Test]
		public void RecuperarTodosLosEventos()
		{
			// Limpiamos el caché
			controladorTiposEvento.CleanCache();
			
			EventType[] todosLosTiposEvento = controladorTiposEvento.RetrieveAll();
			
			// Verificamos que sean tres (milton, gisela y daiana)
			Assert.AreEqual(3, todosLosTiposEvento.Length);
			
			EventType mEnsayo = null;
			EventType mMisa = null;
			EventType mOtro = null;
			
			foreach (EventType unTipoEvento in todosLosTiposEvento)
			{
				if (unTipoEvento.Name == "Ensayo")
					mEnsayo = unTipoEvento;
				else if (unTipoEvento.Name == "Misa")
					mMisa = unTipoEvento;
				else if (unTipoEvento.Name == "Otro")
					mOtro = unTipoEvento;
			}
			
			Assert.AreEqual(ensayo.Id, mEnsayo.Id);
			Assert.AreEqual(ensayo.Name, mEnsayo.Name);
			
			Assert.AreEqual(misa.Id, mMisa.Id);
			Assert.AreEqual(misa.Name, mMisa.Name);
			
			Assert.AreEqual(otro.Id, mOtro.Id);
			Assert.AreEqual(otro.Name, mOtro.Name);
		}
	}
}

