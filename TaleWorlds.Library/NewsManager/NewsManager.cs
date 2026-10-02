using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;

namespace TaleWorlds.Library.NewsManager
{
	// Token: 0x020000AA RID: 170
	public class NewsManager
	{
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x00016926 File Offset: 0x00014B26
		public MBReadOnlyList<NewsItem> NewsItems
		{
			get
			{
				return this._newsItems;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000674 RID: 1652 RVA: 0x0001692E File Offset: 0x00014B2E
		// (set) Token: 0x06000675 RID: 1653 RVA: 0x00016936 File Offset: 0x00014B36
		public bool IsInPreviewMode { get; private set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000676 RID: 1654 RVA: 0x0001693F File Offset: 0x00014B3F
		// (set) Token: 0x06000677 RID: 1655 RVA: 0x00016947 File Offset: 0x00014B47
		public string LocalizationID { get; private set; }

		// Token: 0x06000678 RID: 1656 RVA: 0x00016950 File Offset: 0x00014B50
		public NewsManager()
		{
			this._newsItems = new MBList<NewsItem>();
			this.UpdateConfigSettings();
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00016970 File Offset: 0x00014B70
		public async Task<MBReadOnlyList<NewsItem>> GetNewsItems(bool forceRefresh)
		{
			await this.UpdateNewsItems(forceRefresh);
			return this.NewsItems;
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x000169BD File Offset: 0x00014BBD
		public void SetNewsSourceURL(string url)
		{
			this._newsSourceURL = url;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x000169C8 File Offset: 0x00014BC8
		public async Task UpdateNewsItems(bool forceRefresh)
		{
			if (ApplicationPlatform.CurrentPlatform != Platform.Durango && ApplicationPlatform.CurrentPlatform != Platform.GDKDesktop)
			{
				if (this._isNewsItemCacheDirty || forceRefresh)
				{
					try
					{
						if (Uri.IsWellFormedUriString(this._newsSourceURL, UriKind.Absolute))
						{
							this._newsItems = await NewsManager.DeserializeObjectAsync<MBList<NewsItem>>(await HttpHelper.DownloadStringTaskAsync(this._newsSourceURL));
						}
						else
						{
							Debug.FailedAssert("News file doesn't exist", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\NewsSystem\\NewsManager.cs", "UpdateNewsItems", 73);
						}
					}
					catch (Exception)
					{
					}
					this._isNewsItemCacheDirty = false;
				}
			}
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00016A18 File Offset: 0x00014C18
		public static Task<T> DeserializeObjectAsync<T>(string json)
		{
			Task<T> task;
			try
			{
				using (new StringReader(json))
				{
					task = Task.FromResult<T>(JsonConvert.DeserializeObject<T>(json));
				}
			}
			catch (Exception ex)
			{
				Debug.Print(ex.Message, 0, Debug.DebugColor.White, 17592186044416UL);
				task = Task.FromResult<T>(default(T));
			}
			return task;
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x00016A88 File Offset: 0x00014C88
		private void UpdateConfigSettings()
		{
			this._configPath = this.GetConfigXMLPath();
			this.IsInPreviewMode = false;
			this.LocalizationID = "en";
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(this._configPath);
				this.IsInPreviewMode = this.GetIsInPreviewMode(xmlDocument);
				this.LocalizationID = this.GetLocalizationCode(xmlDocument);
			}
			catch (Exception ex)
			{
				Debug.Print(ex.Message, 0, Debug.DebugColor.White, 17592186044416UL);
			}
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00016B0C File Offset: 0x00014D0C
		private bool GetIsInPreviewMode(XmlDocument configDocument)
		{
			return configDocument != null && configDocument.HasChildNodes && bool.Parse(configDocument.ChildNodes[0].SelectSingleNode("UsePreviewLink").Attributes["Value"].InnerText);
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00016B4A File Offset: 0x00014D4A
		private string GetLocalizationCode(XmlDocument configDocument)
		{
			if (configDocument != null && configDocument.HasChildNodes)
			{
				return configDocument.ChildNodes[0].SelectSingleNode("LocalizationID").Attributes["Value"].InnerText;
			}
			return "en";
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00016B88 File Offset: 0x00014D88
		public void UpdateLocalizationID(string localizationID)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(this._configPath);
			if (xmlDocument.HasChildNodes)
			{
				xmlDocument.ChildNodes[0].SelectSingleNode("LocalizationID").Attributes["Value"].Value = localizationID;
			}
			xmlDocument.Save(this._configPath);
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00016BE8 File Offset: 0x00014DE8
		private PlatformFilePath GetConfigXMLPath()
		{
			PlatformDirectoryPath platformDirectoryPath = new PlatformDirectoryPath(PlatformFileType.User, "Configs");
			PlatformFilePath platformFilePath = new PlatformFilePath(platformDirectoryPath, "NewsFeedConfig.xml");
			bool flag = FileHelper.FileExists(platformFilePath);
			bool flag2 = true;
			if (flag)
			{
				XmlDocument xmlDocument = new XmlDocument();
				try
				{
					xmlDocument.Load(platformFilePath);
					flag2 = xmlDocument.HasChildNodes && xmlDocument.FirstChild.HasChildNodes;
				}
				catch (Exception ex)
				{
					Debug.Print(ex.Message, 0, Debug.DebugColor.White, 17592186044416UL);
					flag2 = false;
				}
			}
			if (!flag || !flag2)
			{
				try
				{
					XmlDocument xmlDocument2 = new XmlDocument();
					XmlNode xmlNode = xmlDocument2.CreateElement("Root");
					xmlDocument2.AppendChild(xmlNode);
					((XmlElement)xmlNode.AppendChild(xmlDocument2.CreateElement("LocalizationID"))).SetAttribute("Value", "en");
					((XmlElement)xmlNode.AppendChild(xmlDocument2.CreateElement("UsePreviewLink"))).SetAttribute("Value", "False");
					xmlDocument2.Save(platformFilePath);
				}
				catch (Exception ex2)
				{
					Debug.Print(ex2.Message, 0, Debug.DebugColor.White, 17592186044416UL);
				}
			}
			return platformFilePath;
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00016D18 File Offset: 0x00014F18
		public void OnFinalize()
		{
			MBList<NewsItem> newsItems = this._newsItems;
			if (newsItems != null)
			{
				newsItems.Clear();
			}
			this._newsItems = null;
			this.LocalizationID = null;
		}

		// Token: 0x040001EA RID: 490
		private string _newsSourceURL;

		// Token: 0x040001EB RID: 491
		private MBList<NewsItem> _newsItems;

		// Token: 0x040001EC RID: 492
		private bool _isNewsItemCacheDirty = true;

		// Token: 0x040001EF RID: 495
		private PlatformFilePath _configPath;

		// Token: 0x040001F0 RID: 496
		private const string DataFolder = "Configs";

		// Token: 0x040001F1 RID: 497
		private const string FileName = "NewsFeedConfig.xml";
	}
}
