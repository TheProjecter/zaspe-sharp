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

namespace ZaspeSharp.Instruments
{
	public class InstrumentException : System.Exception
	{
		public InstrumentException() : base("Hubo un error con algún instrumento") {}
		public InstrumentException(string strErr) : base(strErr) {}
	}
	
	public class InstrumentExistsException : InstrumentException
	{
		public InstrumentExistsException() : base("Ya existe un instrumento con ese nombre") {}
		public InstrumentExistsException(string strErr) : base(strErr) {}
	}
	
	public class InstrumentDoesNotExistException : InstrumentException
	{
		public InstrumentDoesNotExistException() : base("No existe un instrumento con ese nombre") {}
		public InstrumentDoesNotExistException(string strErr) : base(strErr) {}
	}
}
