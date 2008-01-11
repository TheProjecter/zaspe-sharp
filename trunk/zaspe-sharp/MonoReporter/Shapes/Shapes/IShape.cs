using System;

namespace Shapes
{
	public interface IShape
	{
		void Draw(Gtk.PrintContext context);
		//static bool IsThisType(XmlNode node);
	}
}
