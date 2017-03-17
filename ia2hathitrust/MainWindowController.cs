using System;
using Foundation;
using AppKit;
using CoreGraphics;

namespace ia2hathitrust
{
	public class MainWindowController : NSWindowController
	{
		public MainWindowController() : base()
		{
			// Construct the window from code here
			CGRect contentRect = new CGRect(0, 0, 582, 441);
			base.Window = new MainWindow(contentRect, (NSWindowStyle.Titled | NSWindowStyle.Closable | 
			                                           NSWindowStyle.Miniaturizable | NSWindowStyle.Resizable), NSBackingStore.Buffered, false);


			// Simulate Awaking from Nib
			Window.AwakeFromNib();
		}

		public new MainWindow Window
		{
			get { return (MainWindow)base.Window; }
		}
	}
}