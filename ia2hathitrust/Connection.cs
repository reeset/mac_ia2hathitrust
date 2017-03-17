using System;
namespace ia2hathitrust
{
	public class Connection
	{
		private AppKit.NSWindowController p_parent_window = null;


		public void Startup()
		{
			MainWindowController objMain = new MainWindowController();
			objMain.Window.Center();
			objMain.ShowWindow(parent_window);
		}

		public AppKit.NSWindowController parent_window
		{
			get { return p_parent_window; }
			set { p_parent_window = (AppKit.NSWindowController)value; }
		}
	}
}
