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

using NUnit.Framework;

using ZaspeSharp.Attendances;
using ZaspeSharp.Persons;
using ZaspeSharp.Events;

namespace ZaspeSharp.Tests
{
	/// <summary>
	/// Description of ControladorAsistenciasTest.
	/// </summary>
	[TestFixture]
	public class AttendaceManagerTest
	{
		private Person person1, person2;
		private Event event1, event2, event3, event4;
		
		[TestFixtureSetUp]
		public void Init() {
			this.person1 = PersonsManager.Instance.AddPerson(1, "Pividori", "Milton", true, false);
			this.person2 = PersonsManager.Instance.AddPerson(2, "Paduán", "Doli", false, false);
			
			this.event1 = EventsManager.Instance.IngresarEvento(new DateTime(DateTime.Now.Year, 1, 1), "Evento1", Types.ENSAYO);
			this.event2 = EventsManager.Instance.IngresarEvento(new DateTime(DateTime.Now.Year, 1, 2), "Evento2", Types.MISA);
			this.event3 = EventsManager.Instance.IngresarEvento(new DateTime(DateTime.Now.Year, 1, 3), "Evento3", Types.ENSAYO);
			this.event4 = EventsManager.Instance.IngresarEvento(new DateTime(DateTime.Now.Year, 1, 4), "Evento4", Types.OTRO);
		}
		
		[TestFixtureTearDown]
		public void Dispose() {
			if (this.person1.IsPersisted) this.person1.Remove();
			if (this.person2.IsPersisted) this.person2.Remove();
			
			if (this.event1.IsPersisted) this.event1.Remove();
			if (this.event2.IsPersisted) this.event2.Remove();
			if (this.event3.IsPersisted) this.event3.Remove();
			if (this.event4.IsPersisted) this.event4.Remove();
			
			foreach (Attendance anAttendance in AttendanceManager.Instance.RetrieveAll())
				anAttendance.Remove();
		}
		
		[Test]
		public void ValidarDatosIngresados() {
			Assert.IsNotNull(this.person1);
			Assert.AreEqual(1, this.person1.Dni);
			Assert.AreEqual("Pividori", this.person1.Surname);
			Assert.AreEqual("Milton", this.person1.Name);
			Assert.AreEqual(true, this.person1.IsMan);
			Assert.AreEqual(false, this.person1.IsDataComplete);
			
			Assert.IsNotNull(this.person2);
			Assert.AreEqual(2, this.person2.Dni);
			Assert.AreEqual("Paduán", this.person2.Surname);
			Assert.AreEqual("Doli", this.person2.Name);
			Assert.AreEqual(false, this.person2.IsMan);
			Assert.AreEqual(false, this.person2.IsDataComplete);
			
			Assert.IsNotNull(this.event1);
			Assert.AreEqual(new DateTime(DateTime.Now.Year, 1, 1), this.event1.Date);
			Assert.AreEqual("Evento1", this.event1.Name);
			Assert.AreEqual((int)Types.ENSAYO, this.event1.EventType);
			
			Assert.IsNotNull(this.event2);
			Assert.AreEqual(new DateTime(DateTime.Now.Year, 1, 2), this.event2.Date);
			Assert.AreEqual("Evento2", this.event2.Name);
			Assert.AreEqual((int)Types.MISA, this.event2.EventType);
			
			Assert.IsNotNull(this.event3);
			Assert.AreEqual(new DateTime(DateTime.Now.Year, 1, 3), this.event3.Date);
			Assert.AreEqual("Evento3", this.event3.Name);
			Assert.AreEqual((int)Types.ENSAYO, this.event3.EventType);
			
			Assert.IsNotNull(this.event4);
			Assert.AreEqual(new DateTime(DateTime.Now.Year, 1, 4), this.event4.Date);
			Assert.AreEqual("Evento4", this.event4.Name);
			Assert.AreEqual((int)Types.OTRO, this.event4.EventType);
		}
		
		[Test]
		public void ObtenerControlador() {
			AttendanceManager ca1 = AttendanceManager.Instance;
			AttendanceManager ca2 = AttendanceManager.Instance;
			AttendanceManager ca3 = AttendanceManager.Instance;
			
			Assert.AreSame(ca1, ca2);
			Assert.AreSame(ca2, ca3);
		}
		
		[Test]
		public void IngresarAsistencia() {
			Attendance a1 = AttendanceManager.Instance.AddAttendance(this.person1, this.event1);
			
			Attendance a1_recovered = AttendanceManager.Instance.Retrieve(this.person1, this.event1);
			
			Assert.IsNotNull(a1);
			Assert.IsNotNull(a1_recovered);
			Assert.AreSame(a1, a1_recovered);
		}
	}
}
