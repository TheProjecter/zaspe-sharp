using System;

namespace SvgReader.Shapes
{
	public interface IShape
	{
		double Y { get; }
		
		void Draw(Gtk.PrintContext context);
	}
}
