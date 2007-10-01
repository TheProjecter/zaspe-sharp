// /home/miltondp/Projects/zaspe-sharp/GUI/AttendancesReport.cs created with MonoDevelop
// User: miltondp at 20:25 04/08/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;
using Glade;

namespace ZaspeSharp.GUI
{
	public class AttendancesReport
	{
		Assistant attendancesReportAssistant;
		
		public AttendancesReport(Gtk.Window parent)
		{
			this.attendancesReportAssistant = new Assistant();
			
			this.attendancesReportAssistant.Parent = parent;
			
			this.attendancesReportAssistant.ShowAll();
		}
	}
}
