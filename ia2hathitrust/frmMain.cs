using System;

using Foundation;
using AppKit;

namespace ia2hathitrust
{
	public partial class frmMain : NSWindow
	{
		public frmMain(IntPtr handle) : base(handle)
		{
		}

		[Export("initWithCoder:")]
		public frmMain(NSCoder coder) : base(coder)
		{
		}


		public override void AwakeFromNib()
		{
			base.AwakeFromNib();
		}
	}
}
