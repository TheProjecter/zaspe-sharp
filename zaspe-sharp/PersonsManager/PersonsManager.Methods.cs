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
using System.Collections.Generic;
using Gentle.Framework;

namespace ZaspeSharp.Persons
{
	public partial class PersonsManager
	{
#region Retrieve methods
		/// <summary>
		/// Recupera una persona por su id o DNI
		/// </summary>
		/// <param name="dni_id">Puede ser el id o DNI. Si falla con uno trata con el otro.</param>
		/// <returns></returns>
		public Person Retrieve(int dni_id)
		{
			// I look for a person by his DNI
			Key key = new Key(typeof(Person), true);
			key.Add("dni", dni_id);
			
			Person aPerson = Broker.SessionBroker.TryRetrieveInstance(typeof(Person), key) as Person;
			
			// If found, it's returned
			if (aPerson != null)
				return aPerson;

			// Now i look for by id
			//key = new Key(typeof(Person), true);
			//key.Add("id", dni_id);
			
			//aPerson = Broker.SessionBroker.TryRetrieveInstance(typeof(Person), key) as Person;
			
			//if (aPerson != null)
			//	return aPerson;
			
			// the person who is looked for does not exist
			throw new PersonDoesNotExistException("No existe una persona con ese DNI");
		}
		
		public Person[] RetrieveAll()
		{
			// Recuperamos todas las personas de la base de datos
			SqlBuilder sb = new SqlBuilder(StatementType.Select, typeof(Person));
			SqlStatement stmt = sb.GetStatement(true);

			IList persons = ObjectFactory.GetCollection(typeof(Person), stmt.Execute());
			
			List<Person> persons_result = new List<Person>();
			foreach (Person p in persons)
				persons_result.Add(p);
			
			return persons_result.ToArray();
		}
#endregion
		
#region Entry methods
		public Person AddPerson(int dni, string surname, string name, bool is_man,
		                               bool data_is_complete)
		{
			// Chequemos que no exista una persona con el mismo DNI
			try {
				Retrieve(dni);
			}
			catch (PersonDoesNotExistException) {
				Person newPerson = new Person(dni, surname, name, is_man, data_is_complete);
				newPerson.Persist();
				
				return newPerson;
			}
			
			throw new PersonExistsException("Se está intentando ingresar una persona que " +
			                                "ya existe en la base de datos: El DNI ingresado " +
			                                "pertenece a una persona previamente ingresada.");
		}
#endregion
		
#region Deletion methods
		public void DeletePerson(int dni)
		{
			Person aPerson;
			
			try {
				aPerson = Retrieve(dni);
			}
			catch (PersonDoesNotExistException) {
				throw new PersonDoesNotExistException("Se está intentando eliminar una persona que" + 
					" no existe en la base de datos.");
			}
			
			aPerson.Remove();
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
