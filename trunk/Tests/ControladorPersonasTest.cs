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
using ZaspeSharp.Persons;
using NUnit.Framework;

namespace ZaspeSharp.Tests
{
	/// <summary>
	/// Description of ControladorPersonasTest.
	/// </summary>
	[TestFixture]
	public class ControladorPersonasTest
	{
		PersonsManager personsManager = PersonsManager.Instance;
		
		Person milton, gisela, daiana;
		
		/// <summary>
		/// Inicializa la base de datos, justo antes de ejecutar cada test.
		/// </summary>
		[SetUp]
		public void Init()
		{
			// Ingresamos algunos datos genéricos para uso global
			this.milton = personsManager.AddPerson(31486846, "Pividori", "Milton", true, false);
			this.gisela = personsManager.AddPerson(32222222, "Pividori", "Gisela", false, false);
			this.daiana = personsManager.AddPerson(33333333, "Pividori", "Daiana", false, false);
		}
		
		/// <summary>
		/// Éste método se encarga de eliminar todas las entradas en la tabla "personas".
		/// De ésta forma podemos correr varias veces los tests sin preocuparnos por
		/// reinicializar la base de datos.
		/// Se ejecuta justo después de correr un test.
		/// </summary>
		[TearDown]
		public void Dispose()
		{
			// Recuperamos todas las personas
			Person[] personas = personsManager.RetrieveAll();
			
			// Eliminamos, una a una, todas las personas (suena a matanza esto)
			foreach (Person unaPersona in personas)
				personsManager.DeletePerson(unaPersona.Dni);
		}
		
		/// <summary>
		/// Chequeamos que el método de instancia "ControladorPersonas.Instancia"
		/// retorne siempre el mismo objeto, como debe ser.
		/// </summary>
		[Test]
		public void ObtenerControlador()
		{
			PersonsManager c1 = PersonsManager.Instance;
			PersonsManager c2 = PersonsManager.Instance;
			PersonsManager c3 = PersonsManager.Instance;
			
			Assert.AreSame(c1, c2);
			Assert.AreSame(c2, c3);
		}
		
		/// <summary>
		/// Recuperamos una persona, posiblemente desde el mismo caché de Gentle.
		/// </summary>
		[Test]
		public void RecuperarPersona()
		{
			Person mismoMilton = personsManager.Retrieve(31486846);
			
			Assert.AreEqual(milton.Dni, mismoMilton.Dni, "DNI");
			Assert.AreEqual(milton.Surname, mismoMilton.Surname, "Surname");
			Assert.AreEqual(milton.Name, mismoMilton.Name, "Nombre");
			Assert.AreEqual(milton.IsMan, mismoMilton.IsMan, "EsHombre");
			Assert.AreEqual(milton.IsDataComplete, mismoMilton.IsDataComplete, "DatosCompletos");
			Assert.AreEqual(milton.BirthdayDate, mismoMilton.BirthdayDate, "FechaCumpleaños");
			Assert.AreEqual(milton.Address, mismoMilton.Address, "Dirección");
			Assert.AreEqual(milton.City, mismoMilton.City, "Ciudad");
			Assert.AreEqual(milton.LandPhoneNumber, mismoMilton.LandPhoneNumber, "TeléfonoFijo");
			Assert.AreEqual(milton.MobileNumber, mismoMilton.MobileNumber, "TeléfonoCelular");
			Assert.AreEqual(milton.EMail, mismoMilton.EMail, "Mail");
			Assert.AreEqual(milton.Community, mismoMilton.Community, "Comunidad");
			Assert.AreEqual(milton.IsActive, mismoMilton.IsActive, "Activo");
			Assert.IsNotNull(milton.Instruments, "Instrumentos es null");
		}
		
		/// <summary>
		/// Recuperamos una persona, pero antes borramos el caché, para asegurarnos
		/// de que Gentle la busque en la base de datos.
		/// </summary>
		[Test]
		public void RecuperarPersonaCacheLimpio()
		{
			// Primero limpiamos la caché
			personsManager.CleanCache();
			
			Person mismoMilton = personsManager.Retrieve(31486846);
			
			Assert.AreEqual(milton.Dni, mismoMilton.Dni, "DNI");
			Assert.AreEqual(milton.Surname, mismoMilton.Surname, "Apellido");
			Assert.AreEqual(milton.Name, mismoMilton.Name, "Nombre");
			Assert.AreEqual(milton.IsMan, mismoMilton.IsMan, "EsHombre");
			Assert.AreEqual(milton.IsDataComplete, mismoMilton.IsDataComplete, "DatosCompletos");
			Assert.AreEqual(milton.BirthdayDate, mismoMilton.BirthdayDate, "FechaCumpleaños");
			Assert.AreEqual(milton.Address, mismoMilton.Address, "Dirección");
			Assert.AreEqual(milton.City, mismoMilton.City, "Ciudad");
			Assert.AreEqual(milton.LandPhoneNumber, mismoMilton.LandPhoneNumber, "TeléfonoFijo");
			Assert.AreEqual(milton.MobileNumber, mismoMilton.MobileNumber, "TeléfonoCelular");
			Assert.AreEqual(milton.EMail, mismoMilton.EMail, "Mail");
			Assert.AreEqual(milton.Community, mismoMilton.Community, "Comunidad");
			Assert.AreEqual(milton.IsActive, mismoMilton.IsActive, "Activo");
			Assert.IsNotNull(milton.Instruments, "Instrumentos es null");
		}
		
		/// <summary>
		/// Recuperamos una persona.
		/// </summary>
		[Test]
		[ExpectedException(typeof(PersonDoesNotExistException))]
		public void RecuperarPersonaNoExistente()
		{
			Person p = personsManager.Retrieve(11);
		}
		
		/// <summary>
		/// Recupera todas las personas de la base de datos.
		/// </summary>
		[Test]
		public void RecuperarTodasLasPersonas()
		{
			// Limpiamos el caché
			personsManager.CleanCache();
			
			// Recuperamos todas las personas
			Person[] todasPersonas = personsManager.RetrieveAll();
			
			// Verificamos que sean tres (milton, gisela y daiana)
			Assert.AreEqual(todasPersonas.Length, 3);
			
			Person mMilton = null;
			Person mGise = null;
			Person mDaia = null;
			
			foreach (Person unaPersona in todasPersonas)
			{
				if (unaPersona.Name == "Milton")
					mMilton = unaPersona;
				else if (unaPersona.Name == "Gisela")
					mGise = unaPersona;
				else if (unaPersona.Name == "Daiana")
					mDaia = unaPersona;
			}
			
			// Milton
			Assert.AreEqual(milton.Dni, mMilton.Dni, "DNI");
			Assert.AreEqual(milton.Surname, mMilton.Surname, "Apellido");
			Assert.AreEqual(milton.Name, mMilton.Name, "Nombre");
			Assert.AreEqual(milton.IsMan, mMilton.IsMan, "EsHombre");
			Assert.AreEqual(milton.IsDataComplete, mMilton.IsDataComplete, "DatosCompletos");
			Assert.AreEqual(milton.BirthdayDate, mMilton.BirthdayDate, "FechaCumpleaños");
			Assert.AreEqual(milton.Address, mMilton.Address, "Dirección");
			Assert.AreEqual(milton.City, mMilton.City, "Ciudad");
			Assert.AreEqual(milton.LandPhoneNumber, mMilton.LandPhoneNumber, "TeléfonoFijo");
			Assert.AreEqual(milton.MobileNumber, mMilton.MobileNumber, "TeléfonoCelular");
			Assert.AreEqual(milton.EMail, mMilton.EMail, "Mail");
			Assert.AreEqual(milton.Community, mMilton.Community, "Comunidad");
			Assert.AreEqual(milton.IsActive, mMilton.IsActive, "Activo");
			Assert.IsNotNull(mMilton.Instruments, "Instrumentos es null");
			
			// Gisela
			Assert.AreEqual(gisela.Dni, mGise.Dni, "DNI");
			Assert.AreEqual(gisela.Surname, mGise.Surname, "Apellido");
			Assert.AreEqual(gisela.Name, mGise.Name, "Nombre");
			Assert.AreEqual(gisela.IsMan, mGise.IsMan, "EsHombre");
			Assert.AreEqual(gisela.IsDataComplete, mGise.IsDataComplete, "DatosCompletos");
			Assert.AreEqual(gisela.BirthdayDate, mGise.BirthdayDate, "FechaCumpleaños");
			Assert.AreEqual(gisela.Address, mGise.Address, "Dirección");
			Assert.AreEqual(gisela.City, mGise.City, "Ciudad");
			Assert.AreEqual(gisela.LandPhoneNumber, mGise.LandPhoneNumber, "TeléfonoFijo");
			Assert.AreEqual(gisela.MobileNumber, mGise.MobileNumber, "TeléfonoCelular");
			Assert.AreEqual(gisela.EMail, mGise.EMail, "Mail");
			Assert.AreEqual(gisela.Community, mGise.Community, "Comunidad");
			Assert.AreEqual(gisela.IsActive, mGise.IsActive, "Activo");
			Assert.IsNotNull(mGise.Instruments, "Instrumentos es null");
			
			// Daiana
			Assert.AreEqual(daiana.Dni, mDaia.Dni, "DNI");
			Assert.AreEqual(daiana.Surname, mDaia.Surname, "Apellido");
			Assert.AreEqual(daiana.Name, mDaia.Name, "Nombre");
			Assert.AreEqual(daiana.IsMan, mDaia.IsMan, "EsHombre");
			Assert.AreEqual(daiana.IsDataComplete, mDaia.IsDataComplete, "DatosCompletos");
			Assert.AreEqual(daiana.BirthdayDate, mDaia.BirthdayDate, "FechaCumpleaños");
			Assert.AreEqual(daiana.Address, mDaia.Address, "Dirección");
			Assert.AreEqual(daiana.City, mDaia.City, "Ciudad");
			Assert.AreEqual(daiana.LandPhoneNumber, mDaia.LandPhoneNumber, "TeléfonoFijo");
			Assert.AreEqual(daiana.MobileNumber, mDaia.MobileNumber, "TeléfonoCelular");
			Assert.AreEqual(daiana.EMail, mDaia.EMail, "Mail");
			Assert.AreEqual(daiana.Community, mDaia.Community, "Comunidad");
			Assert.AreEqual(daiana.IsActive, mDaia.IsActive, "Activo");
			Assert.IsNotNull(mDaia.Instruments, "Instrumentos es null");
		}
		
		/// <summary>
		/// Ingreso común de una persona. Luego vamos haciendo modificaciones a la misma,
		/// y vamos verificando que se plasmen en la base de datos.
		/// </summary>
		[Test]
		public void IngresarPersona()
		{
			Person doli = personsManager.AddPerson(11111111, "Paduán", "Doli", false, false);
			
			// Seteamos el cumpleaños
			DateTime cumple = new DateTime(DateTime.Now.Year, 8, 11);
			doli.BirthdayDate = cumple;
			
			// Los demás datos
			doli.Address = "Calle 16 Nro 145";
			doli.City = "Avellaneda";
			doli.LandPhoneNumber = "12345";
			doli.MobileNumber = "54321";
			doli.EMail = "dolipaduan@gmail.com";
			doli.Community = "Damasco";
			doli.IsDataComplete = true;
			
			doli.Persist();
			
			// Limpiamos el caché antes de comprobar los datos, así obligamos a
			// Gentle a buscar el objeto en la base de datos.
			personsManager.CleanCache();
			
			doli = personsManager.Retrieve(11111111);
			
			// Chequemos los datos de doli
			Assert.AreEqual(11111111, doli.Dni, "DNI");
			Assert.AreEqual("Paduán", doli.Surname, "Apellido");
			Assert.AreEqual("Doli", doli.Name, "Nombre");
			Assert.IsFalse(doli.IsMan, "EsHombre");
			Assert.IsTrue(doli.IsDataComplete, "DatosCompletos");
			Assert.AreEqual(cumple, doli.BirthdayDate, "FechaCumpleaños");
			Assert.AreEqual("Calle 16 Nro 145", doli.Address, "Dirección");
			Assert.AreEqual("Avellaneda", doli.City, "Ciudad");
			Assert.AreEqual("12345", doli.LandPhoneNumber, "TeléfonoFijo");
			Assert.AreEqual("54321", doli.MobileNumber, "TeléfonoCelular");
			Assert.AreEqual("dolipaduan@gmail.com", doli.EMail, "Mail");
			Assert.AreEqual("Damasco", doli.Community, "Comunidad");
			Assert.IsFalse(doli.IsActive, "Activo");
			Assert.IsNotNull(doli.Instruments, "Instrumentos es null");
		}
		
		/// <summary>
		/// Ingreso duplicado de una persona.
		/// </summary>
		[Test]
		[ExpectedException(typeof(PersonExistsException))]
		public void IngresarPersonaDuplicada()
		{
			Person miltonOtraVez = personsManager.AddPerson(31486846, "Pividori",
				"Milton", true, false);
		}
		
		/// <summary>
		/// Ingreso común de una persona. Luego vamos haciendo modificaciones a la misma,
		/// y vamos verificando que se plasmen en la base de datos.
		/// </summary>
		[Test]
		public void ModificarPersona()
		{
			Person doli = personsManager.AddPerson(11111111, "Paduán", "Doli", false, false);
			
			// Seteamos el cumpleaños
			DateTime cumple = new DateTime(DateTime.Now.Year, 8, 11);
			doli.BirthdayDate = cumple;
			
			// Los demás datos
			doli.Address = "Calle 16 Nro 145";
			doli.City = "Avellaneda";
			doli.LandPhoneNumber = "12345";
			doli.MobileNumber = "54321";
			doli.EMail = "dolipaduan@gmail.com";
			doli.Community = "Damasco";
			doli.IsDataComplete = true;
			
			doli.Persist();
			
			// Limpiamos el caché antes de comprobar los datos, así obligamos a
			// Gentle a buscar el objeto en la base de datos.
			personsManager.CleanCache();
			
			doli = personsManager.Retrieve(11111111);
			
			// Chequemos los datos de doli
			Assert.AreEqual(11111111, doli.Dni, "DNI");
			Assert.AreEqual("Paduán", doli.Surname, "Apellido");
			Assert.AreEqual("Doli", doli.Name, "Nombre");
			Assert.IsFalse(doli.IsMan, "EsHombre");
			Assert.IsTrue(doli.IsDataComplete, "DatosCompletos");
			Assert.AreEqual(cumple, doli.BirthdayDate, "BirthdayDate");
			Assert.AreEqual("Calle 16 Nro 145", doli.Address, "Dirección");
			Assert.AreEqual("Avellaneda", doli.City, "Ciudad");
			Assert.AreEqual("12345", doli.LandPhoneNumber, "TeléfonoFijo");
			Assert.AreEqual("54321", doli.MobileNumber, "TeléfonoCelular");
			Assert.AreEqual("dolipaduan@gmail.com", doli.EMail, "Mail");
			Assert.AreEqual("Damasco", doli.Community, "Comunidad");
			Assert.IsFalse(doli.IsActive, "Activo");
			Assert.IsNotNull(doli.Instruments, "Instrumentos es null");
			
			// Modificamos las propiedades de doli, la persistimos, limpiamos
			// el caché, la recuperamos de la base de datos, y verificamos
			// los cambios.
			doli.BirthdayDate = DateTime.MinValue;
			doli.IsDataComplete = false;
			doli.Persist();
			
			personsManager.CleanCache();
			doli = personsManager.Retrieve(11111111);
			
			Assert.AreEqual(DateTime.MinValue, doli.BirthdayDate, "FechaCumpleaños después de modificar");
			Assert.IsFalse(doli.IsDataComplete, "DatosCompletos después de modificar");
		}
		
		[Test]
		public void EliminarPersona1()
		{
			// Intenamos eliminar una persona que existe en la base de datos
			personsManager.DeletePerson(milton.Dni);
		}
		
		[Test]
		[ExpectedException(typeof(PersonDoesNotExistException))]
		public void EliminarPersona2()
		{
			// Intenamos eliminar una persona que existe en la base de datos
			personsManager.DeletePerson(milton.Dni);
			
			Person mismoMilton = personsManager.Retrieve(milton.Dni);
		}
		
		[Test]
		public void EliminarPersonaCacheLimpio1()
		{
			personsManager.CleanCache();
			
			// Intenamos eliminar una persona que existe en la base de datos
			personsManager.DeletePerson(milton.Dni);
		}
		
		[Test]
		[ExpectedException(typeof(PersonDoesNotExistException))]
		public void EliminarPersonaCacheLimpio2()
		{
			personsManager.CleanCache();
			
			// Intenamos eliminar una persona que existe en la base de datos
			personsManager.DeletePerson(milton.Dni);

			Person mismoMilton = personsManager.Retrieve(milton.Dni);
		}
		
		/// <summary>
		/// Eliminación de una persona que no existe
		/// </summary>
		[Test]
		[ExpectedException(typeof(PersonDoesNotExistException))]
		public void EliminarPersonaNoExistente()
		{
			// Intenamos eliminar a milton
			personsManager.DeletePerson(12);
		}
	}
}
