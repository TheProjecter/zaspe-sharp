/*
  MonoReporter - Report tool for Mono
  Copyright (C) 2006,2007 Milton Pividori

  This file is part of MonoReporter.

  MonoReporter is free software; you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation; either version 3 of the License, or
  (at your option) any later version.

  MonoReporter is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Text.RegularExpressions;
using System.Xml;
using System.Globalization;

namespace SvgReader
{
	public static class Utils
	{
		public static NumberFormatInfo numberFormat;
		
		public const double pxToMmConversion = 0.282258065;
		private const char subOptionsSeparator = ';';
		private const char subOptionFromValueSeparator = ':';
		
		static Utils()
		{
			numberFormat = new NumberFormatInfo();
			numberFormat.CurrencyDecimalSeparator = ".";
		}
		
		public static double PixelToMm(double pixels)
		{
			return (pixels * pxToMmConversion);
		}
		
		public static double DoubleParse(string doubleString)
		{
			return Double.Parse(doubleString, numberFormat);
		}
		
		public static double DoubleParseAndPixelToMm(string doubleString)
		{
			double d = Utils.DoubleParse(doubleString);
			return Utils.PixelToMm(d);
		}
		
		public static string GetAttributeValueFromNode(XmlNode node, string attributeName)
		{
			foreach (XmlAttribute anAttribute in node.Attributes) {
				if (anAttribute.Name.Equals(attributeName))
					return anAttribute.Value;
			}
			
			throw new Exception("'" + attributeName + "'" + " attribute not found.");
		}
		
		public static string GetValueFromMultiValuatedAttribute(XmlNode node, string attributeName, string subOptionName)
		{
			string attributeValue = Utils.GetAttributeValueFromNode(node, attributeName);
			
			foreach (string aSubOption in attributeValue.Split(subOptionsSeparator)) {
				string[] subOptionItems = aSubOption.Split(subOptionFromValueSeparator);
				
				if (subOptionItems[0].Equals(subOptionName))
					return subOptionItems[1];
			}
			
			throw new SubOptionNotFoundException("'" + subOptionName + "'" + " sub option not found.");
		}
	}
}
