using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.Diamond.ClientApplication
{
	// Token: 0x02000041 RID: 65
	public class ClientApplicationConfiguration
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000555A File Offset: 0x0000375A
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x00005562 File Offset: 0x00003762
		public string Name { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x0000556B File Offset: 0x0000376B
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x00005573 File Offset: 0x00003773
		public string InheritFrom { get; set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x0000557C File Offset: 0x0000377C
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x00005584 File Offset: 0x00003784
		public string[] Clients { get; set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000558D File Offset: 0x0000378D
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x00005595 File Offset: 0x00003795
		public SessionProviderType SessionProviderType { get; set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x0000559E File Offset: 0x0000379E
		// (set) Token: 0x060001BA RID: 442 RVA: 0x000055A6 File Offset: 0x000037A6
		public ParameterContainer Parameters { get; set; }

		// Token: 0x060001BB RID: 443 RVA: 0x000055AF File Offset: 0x000037AF
		public ClientApplicationConfiguration()
		{
			this.Name = "NewlyCreated";
			this.InheritFrom = "";
			this.Clients = new string[0];
			this.Parameters = new ParameterContainer();
		}

		// Token: 0x060001BC RID: 444 RVA: 0x000055E4 File Offset: 0x000037E4
		private void FillFromBase(ClientApplicationConfiguration baseConfiguration)
		{
			this.SessionProviderType = baseConfiguration.SessionProviderType;
			this.Parameters = baseConfiguration.Parameters.Clone();
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00005604 File Offset: 0x00003804
		public static string GetDefaultConfigurationFromFile()
		{
			XmlDocument xmlDocument = new XmlDocument();
			string fileContent = VirtualFolders.GetFileContent(BasePath.Name + "Parameters/ClientProfile.xml", null);
			if (fileContent == "")
			{
				return "";
			}
			xmlDocument.LoadXml(fileContent);
			return xmlDocument.ChildNodes[0].Attributes["Value"].InnerText;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00005667 File Offset: 0x00003867
		public static void SetDefaultConfigurationCategory(string category)
		{
			ClientApplicationConfiguration._defaultConfigurationCategory = category;
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001BF RID: 447 RVA: 0x0000566F File Offset: 0x0000386F
		public static string DefaultConfigurationCategory
		{
			get
			{
				return ClientApplicationConfiguration._defaultConfigurationCategory;
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00005676 File Offset: 0x00003876
		public void FillFrom(string configurationName)
		{
			if (string.IsNullOrEmpty(ClientApplicationConfiguration._defaultConfigurationCategory))
			{
				ClientApplicationConfiguration._defaultConfigurationCategory = ClientApplicationConfiguration.GetDefaultConfigurationFromFile();
			}
			this.FillFrom(ClientApplicationConfiguration._defaultConfigurationCategory, configurationName);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000569C File Offset: 0x0000389C
		public void FillFrom(string configurationCategory, string configurationName)
		{
			XmlDocument xmlDocument = new XmlDocument();
			if (configurationCategory == "")
			{
				return;
			}
			string fileContent = VirtualFolders.GetFileContent(string.Concat(new string[]
			{
				BasePath.Name,
				"Parameters/ClientProfiles/",
				configurationCategory,
				"/",
				configurationName,
				".xml"
			}), null);
			if (fileContent == "")
			{
				return;
			}
			xmlDocument.LoadXml(fileContent);
			this.Name = Path.GetFileNameWithoutExtension(configurationName);
			XmlNode firstChild = xmlDocument.FirstChild;
			if (firstChild.Attributes != null && firstChild.Attributes["InheritFrom"] != null)
			{
				this.InheritFrom = firstChild.Attributes["InheritFrom"].InnerText;
				ClientApplicationConfiguration clientApplicationConfiguration = new ClientApplicationConfiguration();
				clientApplicationConfiguration.FillFrom(configurationCategory, this.InheritFrom);
				this.FillFromBase(clientApplicationConfiguration);
			}
			ParameterLoader.LoadParametersInto(string.Concat(new string[] { "ClientProfiles/", configurationCategory, "/", configurationName, ".xml" }), this.Parameters);
			foreach (object obj in firstChild.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "SessionProvider")
				{
					string innerText = xmlNode.Attributes["Type"].InnerText;
					this.SessionProviderType = (SessionProviderType)Enum.Parse(typeof(SessionProviderType), innerText);
				}
				else if (xmlNode.Name == "Clients")
				{
					List<string> list = new List<string>();
					foreach (object obj2 in xmlNode.ChildNodes)
					{
						string innerText2 = ((XmlNode)obj2).Attributes["Type"].InnerText;
						list.Add(innerText2);
					}
					this.Clients = list.ToArray();
				}
				else
				{
					xmlNode.Name == "Parameters";
				}
			}
		}

		// Token: 0x040000A3 RID: 163
		private static string _defaultConfigurationCategory = "";
	}
}
