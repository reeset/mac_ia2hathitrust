using System;
using Foundation;
using AppKit;
using CoreGraphics;

namespace ia2hathitrust
{
	public class MainWindow : NSWindow
	{
		
		private System.Collections.Hashtable objHash = new System.Collections.Hashtable();

		#region Computed Properties
		public NSTextField Label_Description_title { get; set; }
		public NSTextField Label_Description { get; set;}
		public NSTextField Label_Group_By { get; set;}
		public NSTextField txt_status { get; set;}
		public NSTextField lb_start { get; set;}
		public NSTextField lb_end { get; set;}
		public NSTextField lb_save { get; set;}



		public NSTextField txt_inst { get; set; }
		public NSTextField txt_start { get; set; }
		public NSTextField txt_end { get; set;}
		public NSTextField txt_save { get; set; }
		public NSTextField lb_custom { get; set; }




		public NSButton help_button { get; set;}
		public NSButton rd_Collection { get; set;}
		public NSButton rd_Contributor { get; set;}
		public NSButton cmd_debug_url { get; set;}
		public NSButton cmd_save { get; set;}
		public NSButton cmd_process { get; set;}
		public NSButton cmd_close { get; set;}
		public NSButton cmd_open_custom { get; set; }

		public NSBox vline { get; set;}



		#endregion


		private enum Button_Styles {def_style = 0, image };


		#region Constructors

		public MainWindow(CGRect contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool deferCreation) : base(contentRect, aStyle, bufferingType, deferCreation)
		{
			// Define the User Interface of the Window here
			Title = "Internet Archives to HathiTrust Packager";

			// Create the content view for the window and make it fill the window
			ContentView = new NSView(Frame);

			// Add UI Elements to window
			Label_Description_title = CreateLabel(new CGRect(18, 398, 151, 23), false, NSLineBreakMode.Clipping, true, "Description:");
			ContentView.AddSubview(Label_Description_title);

			string sdesc = "This plugin provides an automated method for harvesting data via the Internet Archive and packaging it for HathiTrust ingest.  The plugin makes one assumption, that institutions have provided Internet Archive with copies of their MARC metadata.";
			Label_Description = CreateLabel(new CGRect(18, 319, 420, 79), false, NSLineBreakMode.ByWordWrapping, false, sdesc);
			ContentView.AddSubview(Label_Description);

			Label_Group_By = CreateLabel(new CGRect(54, 288, 78, 25), false, "Group By:");
			ContentView.AddSubview(Label_Group_By);

			lb_start = CreateLabel(new CGRect(48, 186, 90, 24), false, "Start Date:");
			ContentView.AddSubview(lb_start);

			lb_end = CreateLabel(new CGRect(270, 186, 76, 24), false, "End Date:");
			ContentView.AddSubview(lb_end);

			lb_save = CreateLabel(new CGRect(48, 142, 68, 24), false, "Save File:");
			ContentView.AddSubview(lb_save);

			txt_status = CreateLabel(new CGRect(48, 33, 394, 17), false, "Status:");
			ContentView.AddSubview(txt_status);

			lb_custom = CreateLabel(new CGRect(82, 78, 368, 17), false, "No custom rules file defined.");
			ContentView.AddSubview(lb_custom);

			help_button = CreateButton(new CGRect(20, 288, 28, 28), NSButtonType.MomentaryPushIn, NSBezelStyle.RegularSquare, "");
			help_button.Image = NSImage.ImageNamed(NSImageName.BookmarksTemplate);
			ContentView.AddSubview(help_button);

			cmd_save = CreateButton(new CGRect(412, 142, 28, 28), NSButtonType.MomentaryPushIn, NSBezelStyle.RegularSquare, "");
			cmd_save.Image = NSImage.ImageNamed(NSImageName.Folder);
			ContentView.AddSubview(cmd_save);

			cmd_open_custom = CreateButton(new CGRect(48, 73, 28, 28), NSButtonType.MomentaryPushIn, NSBezelStyle.RegularSquare, "");
			cmd_open_custom.Image = NSImage.ImageNamed(NSImageName.Folder);
			ContentView.AddSubview(cmd_open_custom);

			cmd_process = CreateButton(new CGRect(464, 377, 104, 32), NSButtonType.MomentaryPushIn, NSBezelStyle.Rounded, "Process");
			ContentView.AddSubview(cmd_process);

			cmd_close = CreateButton(new CGRect(464, 344, 104, 32), NSButtonType.MomentaryPushIn, NSBezelStyle.Rounded, "Close");
			ContentView.AddSubview(cmd_close);

			cmd_debug_url = CreateButton(new CGRect(42, 108, 131, 32), NSButtonType.MomentaryPushIn, NSBezelStyle.Rounded, "Debug URL");
			ContentView.AddSubview(cmd_debug_url);

			rd_Collection = CreateButton(new CGRect(49, 271, 152, 18), NSButtonType.Radio, NSBezelStyle.Circular, "Collection");
			rd_Collection.State = NSCellStateValue.Off;
			ContentView.AddSubview(rd_Collection);

			rd_Contributor = CreateButton(new CGRect(49, 249, 152, 18), NSButtonType.Radio, NSBezelStyle.Circular, "Contributor");
			rd_Contributor.State = NSCellStateValue.On;
			ContentView.AddSubview(rd_Contributor);

			txt_inst = CreateLabel(new CGRect(50, 220, 390, 22), true, "");
			ContentView.AddSubview(txt_inst);

			txt_start = CreateLabel(new CGRect(123, 188, 96, 22), true, "");
			ContentView.AddSubview(txt_start);

			txt_end = CreateLabel(new CGRect(344, 188, 96, 22), true, "");
			ContentView.AddSubview(txt_end);

			txt_save = CreateLabel(new CGRect(113, 144, 288, 22), true, "");
			ContentView.AddSubview(txt_save);

			vline = new NSBox(new CGRect(459, 63, 5, 358))
			{
				BoxType = NSBoxType.NSBoxSeparator
			};
			ContentView.AddSubview(vline);

	
		}
		#endregion

		#region Override Methods
		public override void AwakeFromNib()
		{
			base.AwakeFromNib();

			// Wireup events

			help_button.Activated += Help_Button_Activated;
			cmd_save.Activated += Cmd_Save_Activated;
			cmd_debug_url.Activated += Cmd_Debug_Url_Activated;
			cmd_close.Activated += Cmd_Close_Activated;
			cmd_process.Activated += Cmd_Process_Activated;
			rd_Collection.Activated += Rd_Collection_Activated;
			rd_Contributor.Activated += Rd_Collection_Activated;
			cmd_open_custom.Activated += Cmd_Open_Custom_Activated;

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
					rd_Collection.State = NSCellStateValue.On;
					rd_Contributor.State = NSCellStateValue.Off;
				}
				else {
					rd_Contributor.State = NSCellStateValue.On;
					rd_Collection.State = NSCellStateValue.Off;
				}
			}
			catch { }


		}
		#endregion

		void Help_Button_Activated(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("http://marcedit.reeset.net/internet-archivehathitrust-data-packager");
		}

		void Cmd_Open_Custom_Activated(object sender, EventArgs e)
		{
			variables obV = new variables();
			string sfile = obV.OpenDialog(this, "Select Rules File", obV.UserDataPath(), new string[] { "xsl", "*.*" });
			if (!string.IsNullOrEmpty(sfile))
			{
				lb_custom.StringValue = sfile;
			}
			else
			{
				lb_custom.StringValue = "No custom rules file is defined.";
			}
		}

		void Cmd_Save_Activated(object sender, EventArgs e)
		{
			variables obV = new variables();
			string sfile = obV.SaveDialog(this, "Save Path", "", new string[] { "xml", "*.*" });
			if (!string.IsNullOrEmpty(sfile))
			{
				txt_save.StringValue = sfile;
			}
		}

		void Cmd_Debug_Url_Activated(object sender, EventArgs e)
		{
			string url = "";

			if (rd_Contributor.State == NSCellStateValue.On)
			{
				url = "http://archive.org/advancedsearch.php?q=mediatype%3Atexts%20updatedate%3A%5B" +
					txt_start.StringValue + "%20TO%20" + txt_end.StringValue + "%5D%20contributor%3A\"" +
							 txt_inst.StringValue + "\"&fl[]=identifier&rows=500&indent=yes&fmt=xml&xmlsearch=Search#raw";
			}
			else 
			{
				url = "http://archive.org/advancedsearch.php?q=mediatype%3Atexts%20updatedate%3A%5B" +
					txt_start.StringValue + "%20TO%20" +
							 txt_end.StringValue + "%5D%20collection%3A\"" +
							 txt_inst.StringValue + "\"&fl[]=identifier&rows=500&indent=yes&fmt=xml&xmlsearch=Search#raw";
			}

			variables objv = new variables();
			objv.MessageBox(this, url);
			//System.Diagnostics.Process.Start(url);
		}

		void Cmd_Close_Activated(object sender, EventArgs e)
		{
			this.Close();
		}

		void Rd_Collection_Activated(object sender, EventArgs e)
		{
			if ((NSButton)sender == rd_Collection)
			{
				if (rd_Collection.State == NSCellStateValue.Off)
				{
					rd_Collection.State = NSCellStateValue.On;
					rd_Contributor.State = NSCellStateValue.Off;
				}
			}
			else {
				if (rd_Contributor.State == NSCellStateValue.Off)
				{
					rd_Collection.State = NSCellStateValue.Off;
					rd_Contributor.State = NSCellStateValue.On;
				}
			}
		}

		void Cmd_Process_Activated(object sender, EventArgs e)
		{
			variables objV = new variables();
			objHash["txt_inst"] = txt_inst.StringValue;
			if (rd_Collection.State == NSCellStateValue.On)
			{
				objHash["search_type"] = "0";
			}
			else {
				objHash["search_type"] = "1";
			}

			objHash["txt_start"] = txt_start.StringValue;
			objHash["txt_end"] = txt_end.StringValue;
			objHash["txt_save"] = txt_save.StringValue;
			objHash["custom"] = lb_custom.StringValue;

			objV.SaveConfig(ref objHash);


			//string url = "http://archive.org/advancedsearch.php?q=mediatype%3Atexts%20updatedate%3A%5B" + txt_start.Text + "%20TO%20" + txt_end.Text + "%5D%20collection%3A\"OhioStateUniversityLibrary\"&fl[]=identifier&rows=5000&indent=yes&fmt=xml&xmlsearch=Search#raw";
			string url = "";

			if (rd_Contributor.State == NSCellStateValue.On)
			{
				url = "http://archive.org/advancedsearch.php?q=mediatype%3Atexts%20updatedate%3A%5B" +
					txt_start.StringValue + "%20TO%20" +
							 txt_end.StringValue + "%5D%20contributor%3A\"" +
							 txt_inst.StringValue + "\"&fl[]=identifier&rows=500&indent=yes&fmt=xml&xmlsearch=Search#raw";
			}
			else if (rd_Collection.State == NSCellStateValue.On)
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

					txt_status.StringValue = "Processing " + (count + 1).ToString() + " of " + identifiers.Count.ToString();
					objV.DoEvents();
					if (!String.IsNullOrEmpty(ia_struct) && !string.IsNullOrEmpty(ia_marc))
					{
						string ark = GetArk(ia_struct);
						string ia_record = JoinRecords(ark, ia_marc);
						if (lb_custom.StringValue.Trim().Length > 0 && 
						    System.IO.File.Exists(lb_custom.StringValue)) {
							ia_record = TransformRecord(ia_record, lb_custom.StringValue);
							if (ia_record.Trim().Length == 0) {
								break;
							}
						}
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
			txt_status.StringValue = "Process has completed.";
		}

		private string TransformRecord(string sxml, string sXSLT)
		{
			//use Saxon; which will be installed with MarcEdit; to allow xslt 1, 2, and 3

			System.IO.StringReader sreader = new System.IO.StringReader(sxml);
			System.Xml.XmlReader xreader = System.Xml.XmlReader.Create(sreader);
			Saxon.Api.Processor processor = new Saxon.Api.Processor();
			Saxon.Api.XsltCompiler xsltCompiler = processor.NewXsltCompiler();
			Saxon.Api.XdmNode input = processor.NewDocumentBuilder().Build(xreader);


			// Create a transformer for the stylesheet.
			Saxon.Api.XsltTransformer transformer = null;
			if (System.IO.File.Exists(sXSLT))
			{
				System.IO.FileStream xstream = new System.IO.FileStream(sXSLT, System.IO.FileMode.Open);
				xsltCompiler.BaseUri = new Uri(System.IO.Path.GetDirectoryName(sXSLT) + System.IO.Path.DirectorySeparatorChar.ToString(), UriKind.Absolute);
				transformer = xsltCompiler.Compile(xstream).Load();
			}
			else
			{
				txt_status.StringValue = "ERROR applying the cutom rules file";
				return "";
			}



			// Set the root node of the source document to be the initial context node
			transformer.InitialContextNode = input;

			// Create a serializer
			Saxon.Api.Serializer serializer = new Saxon.Api.Serializer();
			System.IO.TextWriter stringWriter = new System.IO.StringWriter();
			serializer.SetOutputWriter(stringWriter);
			transformer.Run(serializer);

			string tmp = stringWriter.ToString();

			//System.Windows.Forms.MessageBox.Show(tmp);
			//****Remove these lines because all the white space was being removed on translation***
			//System.Text.RegularExpressions.Regex objRegEx = new System.Text.RegularExpressions.Regex(@"\n +=LDR ", System.Text.RegularExpressions.RegexOptions.None);
			//tmp = objRegEx.Replace(tmp, "\n" + @"=LDR ");

			return tmp;
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


		private NSTextField CreateLabel(CGRect bounds,
								  bool bEditable = false,
										 string sText = "")
		{
			return CreateLabel(bounds, bEditable, NSLineBreakMode.Clipping, false, sText);
		}


		private NSTextField CreateLabel(CGRect bounds,
								  bool bEditable = false,
										NSLineBreakMode lineBreak = NSLineBreakMode.Clipping,
										bool isBold = false,
		                                string sText = "")
		{
			NSTextField mylabel = new NSTextField(bounds)
			{
				LineBreakMode = lineBreak,
				BackgroundColor = NSColor.Clear,
				TextColor = NSColor.Black,
				Editable = bEditable,
				Bezeled = false,
				AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.MinYMargin,
				StringValue = sText
			};

			if (bEditable == true)
			{
				mylabel.Bezeled = true;
				mylabel.BezelStyle = NSTextFieldBezelStyle.Square;
			}

			if (isBold == true)
			{
				NSFontManager manager = new NSFontManager();
				mylabel.Font = manager.ConvertWeight(true, mylabel.Font);
			}
			return mylabel;
		}

		private NSButton CreateButton(CGRect bounds,
									  NSButtonType button_type = NSButtonType.MomentaryPushIn,
		                              NSBezelStyle style = NSBezelStyle.Rounded,
									  string sText = "")
		{
			NSButton mybutton = new NSButton(bounds)
			{
				BezelStyle = style
			};

			mybutton.SetButtonType(button_type);
			mybutton.Title = sText;
			return mybutton;
		}

		private NSTextView CreateTextBox(CGRect bounds)
		{
			NSTextView myTextBox = new NSTextView(bounds)
			{
				Editable = true
			};
			return myTextBox;
		}

	}
}
