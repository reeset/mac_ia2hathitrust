using System;
using AppKit;

namespace ia2hathitrust
{
	public class variables
	{
		public Interfaces.IHost objEditor = null;
		/// <summary>
		/// Opens the dialog.
		/// </summary>
		/// <returns>The dialog.</returns>
		/// <param name="sender">Sender.</param>
		/// <param name="sTitle">S title.</param>
		/// <param name="sPath">S path.</param>
		/// <param name="sfilter">Sfilter.</param>
		/// <param name="bAllowAll">Allows all types (default)</param>
		public string OpenDialog(object sender, string sTitle, string sPath, string[] sfilter, bool bAllowAll = true)
		{
			

			string[] tfilter = new string[sfilter.Length];
			sfilter.CopyTo(tfilter, 0);
			tfilter[tfilter.Length - 1] = ".";

			string ret = String.Empty;

			try
			{
				Foundation.NSUrl sDir = new Foundation.NSUrl(sPath);
				AppKit.NSOpenPanel p = new NSOpenPanel();
				p.Title = sTitle;
				if (bAllowAll == false)
				{
					p.AllowedFileTypes = new string[] { ".*" };
				}
				p.CanChooseFiles = true;
				p.DirectoryUrl = sDir;
				p.RunModal();

				if (p.Url != null)
				{
					ret = p.Url.Path;
				}


			}
			catch
			{
				Foundation.NSUrl sDir = new Foundation.NSUrl(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
				AppKit.NSOpenPanel p = new NSOpenPanel();
				p.Title = "Open File";
				p.AllowedFileTypes = tfilter;
				p.CanChooseFiles = true;
				p.DirectoryUrl = sDir;
				p.RunModal();

				if (p.Url != null)
				{
					ret = p.Url.Path;
				}


			}

			return ret;
		}

		/// <summary>
		/// Saves the dialog.
		/// </summary>
		/// <returns>The dialog.</returns>
		/// <param name="sender">Sender.</param>
		/// <param name="sTitle">S title.</param>
		/// <param name="sPath">S path.</param>
		/// <param name="sfilter">Sfilter.</param>
		public string SaveDialog(NSWindow sender, string sTitle, string sPath, string[] sfilter)
		{
			

			AppKit.NSSavePanel p = new NSSavePanel();

			Foundation.NSUrl sDir = null;

			try
			{
				sDir = new Foundation.NSUrl(sPath);
			}
			catch
			{
				sDir = new Foundation.NSUrl(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
			}

			try
			{
				p.Title = sTitle;
				p.AllowedFileTypes = sfilter;
				p.DirectoryUrl = sDir;
				p.ExtensionHidden = false;
				p.NameFieldStringValue = "";
				p.RunModal();

				string ret = String.Empty;

				if (p.Url != null &&
					!String.IsNullOrEmpty(p.Url.Path))
				{
					ret = p.Url.Path;
				}

				//			p.Begin (result => {
				//				if (result == 1) {
				//					if (p.Url != null) {
				//						ret = p.Url.Path;
				//					}	
				//				}
				//			});




				return ret;
			}
			catch
			{
				return "";
			}
		}

		public void SaveConfig(ref System.Collections.Hashtable objH)
		{
			try
			{
				System.IO.StreamWriter writer = new System.IO.StreamWriter(UserDataPath() + "ia2hathitrust.txt", false, new System.Text.UTF8Encoding(false));
				writer.WriteLine((string)objH["txt_inst"]);
				writer.WriteLine((string)objH["search_type"]);
				writer.WriteLine((string)objH["txt_start"]);
				writer.WriteLine((string)objH["txt_end"]);
				writer.WriteLine((string)objH["txt_save"]);
				writer.WriteLine((string)objH["custom"]);
				writer.Close();

			}
			catch { }
		}

		public void ReadConfig(ref System.Collections.Hashtable objH)
		{
			objH.Add("txt_inst", "");
			objH.Add("search_type", "0");
			objH.Add("txt_start", "");
			objH.Add("txt_end", "");
			objH.Add("txt_save", "");
			objH.Add("custom", "");
			if (System.IO.File.Exists(UserDataPath() + "ia2hathitrust.txt"))
			{
				string[] lines = System.IO.File.ReadAllLines(UserDataPath() + "ia2hathitrust.txt");
				for (int x = 0; x < lines.Length; x++)
				{
					switch (x)
					{
						case 0:
							objH["txt_inst"] =  lines[0];
							break;
						case 1:
							objH["search_type"] = lines[1];
							break;
						case 2:
							objH["txt_start"] = lines[2];
							break;
						case 3:
							objH["txt_end"] = lines[3];
							break;
						case 4:
							objH["txt_save"] = lines[4];
							break;
						case 5:
							objH["custom"] = lines[5];
							break;
						default:
							break;
					}
				}
			}



		}

		public bool IsNumeric(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return false;
			}

			for (int x = 0; x < s.Length; x++)
			{
				if (Char.IsNumber(s[x]) == false)
				{
					return false;
				}
			}
			return true;
		}

		public void DoEvents()
		{
			Foundation.NSRunLoop.Current.RunUntil(new Foundation.NSDate().AddSeconds(0.02));
		}

		/// <summary>
		/// Users the data path.
		/// </summary>
		/// <returns>The data path.</returns>
		public string UserDataPath()
		{
			string s = String.Empty;
			s = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			if (s.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()) == false)
			{
				s += System.IO.Path.DirectorySeparatorChar.ToString() + @"marcedit" + System.IO.Path.DirectorySeparatorChar.ToString() + @"plugins" + System.IO.Path.DirectorySeparatorChar.ToString();
			}

			return s;
		}

		/// <summary>
		/// Messages the box.
		/// </summary>
		/// <returns><c>true</c>, if box was messaged, <c>false</c> otherwise.</returns>
		/// <param name="sender">Sender.</param>
		/// <param name="sMessage">S message.</param>
		/// <param name="Title">Title.</param>
		/// <param name="breturn">If set to <c>true</c> breturn.</param>
		public bool MessageBox(object sender, string sMessage, string Title = "", bool breturn = false, bool bYesNo = false)
		{



			var alert = new NSAlert();
			alert.AlertStyle = NSAlertStyle.Informational;
			alert.MessageText = sMessage;
			alert.Window.Title = Title;

			if (bYesNo == false)
			{
				alert.AddButton("OK");

				if (breturn == true)
				{
					alert.AddButton("Cancel");
				}
			}
			else {
				alert.AddButton("Yes");

				if (breturn == true)
				{
					alert.AddButton("No");
				}
			}

			var ret = alert.RunSheetModal(sender as NSWindow);
			return (ret == (int)NSAlertButtonReturn.First);

		}


	}
}
