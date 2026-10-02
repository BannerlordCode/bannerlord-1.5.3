using System;
using System.IO;
using System.Xml;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Credits
{
	// Token: 0x02000083 RID: 131
	public class CreditsVM : ViewModel
	{
		// Token: 0x06000AD6 RID: 2774 RVA: 0x000269D1 File Offset: 0x00024BD1
		public CreditsVM()
		{
			this.ExitKey = InputKeyItemVM.CreateFromHotKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"), false);
			this.ExitText = new TextObject("{=exitMenuOption}Exit", null).ToString();
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x00026A10 File Offset: 0x00024C10
		private static CreditsItemVM CreateFromFile(string path)
		{
			CreditsItemVM creditsItemVM = null;
			try
			{
				if (File.Exists(path))
				{
					XmlDocument xmlDocument = new XmlDocument();
					XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
					xmlReaderSettings.IgnoreComments = true;
					using (XmlReader xmlReader = XmlReader.Create(new StreamReader(path), xmlReaderSettings))
					{
						xmlDocument.Load(xmlReader);
					}
					XmlNode xmlNode = null;
					for (int i = 0; i < xmlDocument.ChildNodes.Count; i++)
					{
						XmlNode xmlNode2 = xmlDocument.ChildNodes.Item(i);
						if (xmlNode2.NodeType == XmlNodeType.Element && xmlNode2.Name == "Credits")
						{
							xmlNode = xmlNode2;
							break;
						}
					}
					if (xmlNode != null)
					{
						creditsItemVM = CreditsVM.CreateItem(xmlNode);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Print("Could not load Credits xml from " + path + ". Exception: " + ex.Message, 0, Debug.DebugColor.White, 17592186044416UL);
				creditsItemVM = null;
			}
			return creditsItemVM;
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x00026B08 File Offset: 0x00024D08
		public void FillFromFile(string path)
		{
			try
			{
				if (File.Exists(path))
				{
					XmlDocument xmlDocument = new XmlDocument();
					XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
					xmlReaderSettings.IgnoreComments = true;
					using (XmlReader xmlReader = XmlReader.Create(new StreamReader(path), xmlReaderSettings))
					{
						xmlDocument.Load(xmlReader);
					}
					XmlNode xmlNode = null;
					for (int i = 0; i < xmlDocument.ChildNodes.Count; i++)
					{
						XmlNode xmlNode2 = xmlDocument.ChildNodes.Item(i);
						if (xmlNode2.NodeType == XmlNodeType.Element && xmlNode2.Name == "Credits")
						{
							xmlNode = xmlNode2;
							break;
						}
					}
					if (xmlNode != null)
					{
						CreditsItemVM creditsItemVM = CreditsVM.CreateItem(xmlNode);
						this._rootItem = creditsItemVM;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Print("Could not load Credits xml. Exception: " + ex.Message, 0, Debug.DebugColor.White, 17592186044416UL);
			}
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x00026BF8 File Offset: 0x00024DF8
		private static CreditsItemVM CreateItem(XmlNode node)
		{
			CreditsItemVM creditsItemVM = null;
			if (node.Name.ToLower() == "LoadFromFile".ToLower())
			{
				string value = node.Attributes["Name"].Value;
				string text = "";
				if (node.Attributes["PlatformSpecific"] != null && node.Attributes["PlatformSpecific"].Value.ToLower() == "true")
				{
					if (ApplicationPlatform.IsPlatformConsole())
					{
						text = "Console";
					}
					else
					{
						text = "PC";
					}
				}
				if (node.Attributes["ConsoleSpecific"] != null && node.Attributes["ConsoleSpecific"].Value.ToLower() == "true")
				{
					if (ApplicationPlatform.CurrentPlatform == Platform.Durango)
					{
						text = "XBox";
					}
					else if (ApplicationPlatform.CurrentPlatform == Platform.Orbis)
					{
						text = "PlayStation";
					}
					else
					{
						text = "PC";
					}
				}
				creditsItemVM = CreditsVM.CreateFromFile(ModuleHelper.GetModuleFullPath("Native") + "ModuleData/" + value + text + ".xml");
			}
			else
			{
				creditsItemVM = new CreditsItemVM();
				creditsItemVM.Type = node.Name;
				if (node.Attributes["Text"] != null)
				{
					creditsItemVM.Text = new TextObject(node.Attributes["Text"].Value, null).ToString();
				}
				else
				{
					creditsItemVM.Text = "";
				}
				foreach (object obj in node.ChildNodes)
				{
					CreditsItemVM creditsItemVM2 = CreditsVM.CreateItem((XmlNode)obj);
					creditsItemVM.Items.Add(creditsItemVM2);
				}
			}
			return creditsItemVM;
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x00026DCC File Offset: 0x00024FCC
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.ExitKey.OnFinalize();
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000ADB RID: 2779 RVA: 0x00026DDF File Offset: 0x00024FDF
		// (set) Token: 0x06000ADC RID: 2780 RVA: 0x00026DE7 File Offset: 0x00024FE7
		[DataSourceProperty]
		public CreditsItemVM RootItem
		{
			get
			{
				return this._rootItem;
			}
			set
			{
				if (value != this._rootItem)
				{
					this._rootItem = value;
					base.OnPropertyChangedWithValue<CreditsItemVM>(value, "RootItem");
				}
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000ADD RID: 2781 RVA: 0x00026E05 File Offset: 0x00025005
		// (set) Token: 0x06000ADE RID: 2782 RVA: 0x00026E0D File Offset: 0x0002500D
		[DataSourceProperty]
		public InputKeyItemVM ExitKey
		{
			get
			{
				return this._exitKey;
			}
			set
			{
				if (value != this._exitKey)
				{
					this._exitKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ExitKey");
				}
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000ADF RID: 2783 RVA: 0x00026E2B File Offset: 0x0002502B
		// (set) Token: 0x06000AE0 RID: 2784 RVA: 0x00026E33 File Offset: 0x00025033
		[DataSourceProperty]
		public string ExitText
		{
			get
			{
				return this._exitText;
			}
			set
			{
				if (value != this._exitText)
				{
					this._exitText = value;
					base.OnPropertyChangedWithValue<string>(value, "ExitText");
				}
			}
		}

		// Token: 0x04000503 RID: 1283
		public CreditsItemVM _rootItem;

		// Token: 0x04000504 RID: 1284
		private InputKeyItemVM _exitKey;

		// Token: 0x04000505 RID: 1285
		private string _exitText;
	}
}
