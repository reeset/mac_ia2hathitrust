// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace ia2hathitrust
{
	[Register ("frmMainController")]
	partial class frmMainController
	{
		[Outlet]
		AppKit.NSButton cmdClose { get; set; }

		[Outlet]
		AppKit.NSButton cmdDebugURL { get; set; }

		[Outlet]
		AppKit.NSButton cmdHelp { get; set; }

		[Outlet]
		AppKit.NSButton cmdOpen { get; set; }

		[Outlet]
		AppKit.NSButton cmdProcess { get; set; }

		[Outlet]
		AppKit.NSTextField lbStatus { get; set; }

		[Outlet]
		AppKit.NSButton rdCollection { get; set; }

		[Outlet]
		AppKit.NSButton rdContributor { get; set; }

		[Outlet]
		AppKit.NSTextField txt_end { get; set; }

		[Outlet]
		AppKit.NSTextField txt_inst { get; set; }

		[Outlet]
		AppKit.NSTextField txt_save { get; set; }

		[Outlet]
		AppKit.NSTextField txt_start { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (cmdHelp != null) {
				cmdHelp.Dispose ();
				cmdHelp = null;
			}

			if (cmdOpen != null) {
				cmdOpen.Dispose ();
				cmdOpen = null;
			}

			if (cmdProcess != null) {
				cmdProcess.Dispose ();
				cmdProcess = null;
			}

			if (cmdClose != null) {
				cmdClose.Dispose ();
				cmdClose = null;
			}

			if (cmdDebugURL != null) {
				cmdDebugURL.Dispose ();
				cmdDebugURL = null;
			}

			if (rdCollection != null) {
				rdCollection.Dispose ();
				rdCollection = null;
			}

			if (rdContributor != null) {
				rdContributor.Dispose ();
				rdContributor = null;
			}

			if (txt_inst != null) {
				txt_inst.Dispose ();
				txt_inst = null;
			}

			if (txt_start != null) {
				txt_start.Dispose ();
				txt_start = null;
			}

			if (txt_end != null) {
				txt_end.Dispose ();
				txt_end = null;
			}

			if (txt_save != null) {
				txt_save.Dispose ();
				txt_save = null;
			}

			if (lbStatus != null) {
				lbStatus.Dispose ();
				lbStatus = null;
			}
		}
	}
}
