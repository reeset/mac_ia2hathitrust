using System;


using Foundation;
using AppKit;

namespace ia2hathitrust
{
	public partial class frmMainController : NSWindowController
	{
		private System.Collections.Hashtable objHash = new System.Collections.Hashtable();
		public frmMainController(IntPtr handle) : base(handle)
		{
		}

		[Export("initWithCoder:")]
		public frmMainController(NSCoder coder) : base(coder)
		{
		}

		public frmMainController() : base("frmMain")
		{
		}

		public override void LoadWindow()
		{
			NSBundle[] b = NSBundle._AllBundles;
			foreach (NSBundle bi in b)
			{
				System.Diagnostics.Debug.WriteLine(bi.BuiltinPluginsPath);

			}
			System.Diagnostics.Debug.WriteLine(NSBundle.MainBundle.BundlePath);
			System.Diagnostics.Debug.WriteLine(NSBundle.MainBundle.GetUrlForResource("frmMain", "nib").ToString());
			Foundation.NSArray topObjs = null;
			NSBundle.MainBundle.LoadNibNamed("frmMain", this, out topObjs);
			//base.LoadWindow();
		}
		public override void AwakeFromNib()
		{
			
			System.Diagnostics.Debug.WriteLine("Entered the awake function");
			base.AwakeFromNib();
			cmdClose.Activated += CmdClose_Activated;
			cmdHelp.Activated += CmdHelp_Activated;
			cmdOpen.Activated += CmdOpen_Activated;
			cmdDebugURL.Activated += CmdDebugURL_Activated;
			cmdProcess.Activated += CmdProcess_Activated;

			System.Diagnostics.Debug.WriteLine("All buttons setup");
			variables objV = new variables();
			try
			{
				objV.ReadConfig(ref objHash);

				txt_inst.StringValue = (string)objHash["txt_inst"];
				txt_start.StringValue = (string)objHash["txt_start"];
				txt_end.StringValue = (string)objHash["txt_end"];
				txt_save.StringValue = (string)objHash["txt_save"];

				if ((string)objHash["search_type"] == "0")
				{
					rdCollection.State = NSCellStateValue.On;
				}
				else {
					rdContributor.State = NSCellStateValue.On;
				}
			}
			catch { }

		}

		void CmdClose_Activated(object sender, EventArgs e)
		{
			this.Close();
		}

		void CmdHelp_Activated(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("http://marcedit.reeset.net/internet-archivehathitrust-data-packager");
		}

		void CmdOpen_Activated(object sender, EventArgs e)
		{
			variables obV = new variables();
			string sfile = obV.SaveDialog(this.Window, "Save Path", "", new string[] { "mrk", "txt" });
			if (!string.IsNullOrEmpty(sfile))
			{
				txt_save.StringValue = sfile;
			}

		}

		public new frmMain Window
		{
			get { return (frmMain)base.Window; }
		}

		void CmdDebugURL_Activated(object sender, EventArgs e)
		{
			string url = "";

			if (rdContributor.State == NSCellStateValue.On)
			{
				url = "http://archive.org/advancedsearch.php?q=mediatype%3Atexts%20updatedate%3A%5B" + 
					txt_start.StringValue + "%20TO%20" + txt_end.StringValue + "%5D%20contributor%3A\"" + 
				             txt_inst.StringValue + "\"&fl[]=identifier&rows=500&indent=yes&fmt=xml&xmlsearch=Search#raw";
			}
			else if (rdCollection.State == NSCellStateValue.On)
			{
				url = "http://archive.org/advancedsearch.php?q=mediatype%3Atexts%20updatedate%3A%5B" + 
					txt_start.StringValue + "%20TO%20" + 
				             txt_end.StringValue + "%5D%20collection%3A\"" + 
				             txt_inst.StringValue + "\"&fl[]=identifier&rows=500&indent=yes&fmt=xml&xmlsearch=Search#raw";
			}


			System.Diagnostics.Process.Start(url);
		}

		void CmdProcess_Activated(object sender, EventArgs e)
		{
			variables objV = new variables();
			objHash["txt_inst"] = txt_inst.StringValue;
			if (rdCollection.State == NSCellStateValue.On)
			{
				objHash["search_type"] = "0";
			}
			else {
				objHash["search_type"] = "1";
			}

			objHash["txt_start"] = txt_start.StringValue;
			objHash["txt_end"] = txt_end.StringValue;
			objHash["txt_save"] = txt_save.StringValue;

			objV.SaveConfig(ref objHash);


			//string url = "http://archive.org/advancedsearch.php?q=mediatype%3Atexts%20updatedate%3A%5B" + txt_start.Text + "%20TO%20" + txt_end.Text + "%5D%20collection%3A\"OhioStateUniversityLibrary\"&fl[]=identifier&rows=5000&indent=yes&fmt=xml&xmlsearch=Search#raw";
			string url = "";

			if (rdContributor.State == NSCellStateValue.On)
			{
				url = "http://archive.org/advancedsearch.php?q=mediatype%3Atexts%20updatedate%3A%5B" + 
					txt_start.StringValue + "%20TO%20" + 
				             txt_end.StringValue + "%5D%20contributor%3A\"" + 
				             txt_inst.StringValue + "\"&fl[]=identifier&rows=500&indent=yes&fmt=xml&xmlsearch=Search#raw";
			}
			else if (rdCollection.State == NSCellStateValue.On)
			{
				url = "http://archive.org/advancedsearch.php?q=mediatype%3Atexts%20updatedate%3A%5B" + 
					txt_start.StringValue + "%20TO%20" + 
				             txt_end.StringValue + "%5D%20collection%3A\"" + 
				             txt_inst.StringValue + "\"&fl[]=identifier&rows=500&indent=yes&fmt=xml&xmlsearch=Search#raw";
			}
			string xml_data = GetURL(url);
			string xml_buffer = "";
			if (!string.IsNullOrEmpty(xml_data))
			{
				System.Collections.ArrayList identifiers = new System.Collections.ArrayList();
				ProcessResults(url, ref identifiers);
				//now we iterate through the data file and process data
				string ia_stem = "http://www.archive.org/download/";

				int count = 0;
				foreach (string bookname in identifiers)
				{
					string ia_meta_stem = ia_stem + bookname + "/" + bookname;
					string ia_struct = GetURL(ia_meta_stem + "_meta.xml");
					string ia_marc = GetURL(ia_meta_stem + "_marc.xml");

					//if (string.IsNullOrEmpty(ia_marc))
					//{
					//    ia_marc = GetURL(ia_meta_stem + "_meta.mrc");

					//}

					//System.Windows.Forms.MessageBox.Show(ia_struct);
					//System.Windows.Forms.MessageBox.Show(ia_marc);

					lbStatus.StringValue = "Processing " + (count + 1).ToString() + " of " + identifiers.Count.ToString();

					if (!String.IsNullOrEmpty(ia_struct) && !string.IsNullOrEmpty(ia_marc))
					{
						string ark = GetArk(ia_struct);
						string ia_record = JoinRecords(ark, ia_marc);
						xml_buffer += ia_record;
						//System.Windows.Forms.MessageBox.Show(ia_record);

					}
					count++;

				}
			}

			string collection_header = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" + System.Environment.NewLine +
									   "<collection xmlns=\"http://www.loc.gov/MARC21/slim\"" + System.Environment.NewLine +
									   "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"" + System.Environment.NewLine +
									   "xsi:schemaLocation=\"http://www.loc.gov/MARC21/slim http://www.loc.gov/standards/marcxml/schema/MARC21slim.xsd\">" + System.Environment.NewLine;

			string collection_footer = "</collection>";
			System.IO.StreamWriter writer = new System.IO.StreamWriter(txt_save.StringValue, false, new System.Text.UTF8Encoding(false));
			writer.Write(collection_header + xml_buffer + collection_footer);
			writer.Close();
			//System.Windows.Forms.MessageBox.Show("finished");
			lbStatus.StringValue = "Process has completed.";
		}

		private string GetArk(string ia_struct)
		{
			string marc_field = "<datafield tag=\"955\" ind1=\" \" ind2=\" \">";

			System.Xml.XmlTextReader rd;
			rd = new System.Xml.XmlTextReader(new System.IO.StringReader(ia_struct));
			try
			{
				//System.Windows.Forms.MessageBox.Show("here");
				while (rd.Read())
				{
					//This is where we find the head of the record,
					//then process the values within the record.
					//We also need to do character encoding here if necessary.

					if (rd.NodeType == System.Xml.XmlNodeType.Element)
					{
						//System.Windows.Forms.MessageBox.Show(rd.LocalName);
						if (rd.LocalName == "identifier-ark")
						{
							marc_field += "<subfield code=\"b\">" + rd.ReadString() + "</subfield>";
						}
						else if (rd.LocalName == "identifier")
						{
							marc_field += "<subfield code=\"q\">" + rd.ReadString() + "</subfield>";
						}
					}
				}

				marc_field += "</datafield>";
			}
			catch { }
			rd.Close();
			return marc_field;
		}

		private string JoinRecords(string ark, string ia_marc)
		{

			int start_seek = ia_marc.IndexOf("<record");
			ia_marc = ia_marc.Substring(start_seek);
			int seek = ia_marc.IndexOf("</record>");
			string marc_string = "";
			if (seek > -1)
			{
				marc_string = ia_marc.Insert(seek - 1, ark);
				marc_string = marc_string.Substring(0, marc_string.IndexOf("</record>") + "</record>".Length);
				marc_string = marc_string.Replace("<record xmlns=\"http://www.loc.gov/MARC21/slim\">", "<record>");
			}

			return marc_string;
		}

		private void ProcessResults(string xml_data, ref System.Collections.ArrayList identifiers)
		{
			System.Xml.XmlTextReader rd;
			rd = new System.Xml.XmlTextReader(xml_data);
			try
			{
				//System.Windows.Forms.MessageBox.Show("here");
				while (rd.Read())
				{
					//This is where we find the head of the record,
					//then process the values within the record.
					//We also need to do character encoding here if necessary.

					if (rd.NodeType == System.Xml.XmlNodeType.Element)
					{
						//System.Windows.Forms.MessageBox.Show(rd.LocalName);
						if (rd.LocalName == "str" && rd.GetAttribute("name") == "identifier")
						{
							identifiers.Add(rd.ReadString());
						}
					}
				}
			}
			catch { }
		}

		public string GetURL(string url)
		{
			try
			{

				System.Net.WebRequest.DefaultWebProxy = null;
				System.Uri uri = new Uri(url);
				System.Net.HttpWebRequest objRequest =
				(System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
				//System.Net.HttpWebRequest objRequest =
				//(System.Net.HttpWebRequest)System.Net.WebRequest.Create(MyUri(uri));
				objRequest.Proxy = null;

				//if (cglobal.PublicProxy != null)
				//{
				//    objRequest.Proxy = cglobal.PublicProxy;
				//}
				objRequest.UserAgent = "MarcEdit Mac WebRequester";
				objRequest.Proxy = null;
				objRequest.Accept = "*/*";

				//Changing the default timeout from 100 seconds to 30 seconds.
				objRequest.Timeout = 30000;

				System.Net.WebResponse objResponse = objRequest.GetResponse();

				System.IO.StreamReader reader = new System.IO.StreamReader(objResponse.GetResponseStream(), System.Text.Encoding.UTF8);
				string tmpVal = reader.ReadToEnd().Trim();
				//System.Windows.Forms.MessageBox.Show(uri + "\n" + tmpVal);
				reader.Close();
				objResponse.Close();

				return tmpVal;
			}
			catch //(System.Exception xx)
			{
				//System.Windows.Forms.MessageBox.Show(uri + "\n" + xx.ToString());
				return "";
			}
		}
	}
}
