using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.ModuleManager
{
	// Token: 0x02000007 RID: 7
	public class ModuleInfo
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002E96 File Offset: 0x00001096
		// (set) Token: 0x06000032 RID: 50 RVA: 0x00002E9E File Offset: 0x0000109E
		public bool IsSelected { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002EA7 File Offset: 0x000010A7
		// (set) Token: 0x06000034 RID: 52 RVA: 0x00002EAF File Offset: 0x000010AF
		public string Id { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002EB8 File Offset: 0x000010B8
		// (set) Token: 0x06000036 RID: 54 RVA: 0x00002EC0 File Offset: 0x000010C0
		public string Name { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002EC9 File Offset: 0x000010C9
		public bool IsOfficial
		{
			get
			{
				return this.Type > ModuleType.Community;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002ED4 File Offset: 0x000010D4
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002EDC File Offset: 0x000010DC
		public bool IsDefault { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002EE5 File Offset: 0x000010E5
		public bool IsRequiredOfficial
		{
			get
			{
				return this.Type == ModuleType.Official;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002EF0 File Offset: 0x000010F0
		// (set) Token: 0x0600003C RID: 60 RVA: 0x00002EF8 File Offset: 0x000010F8
		public bool IsActive { get; private set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002F01 File Offset: 0x00001101
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00002F09 File Offset: 0x00001109
		public ApplicationVersion Version { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002F12 File Offset: 0x00001112
		// (set) Token: 0x06000040 RID: 64 RVA: 0x00002F1A File Offset: 0x0000111A
		public ApplicationVersion RequiredBaseVersion { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002F23 File Offset: 0x00001123
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00002F2B File Offset: 0x0000112B
		public ModuleCategory Category { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002F34 File Offset: 0x00001134
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00002F3C File Offset: 0x0000113C
		public string FolderPath { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002F45 File Offset: 0x00001145
		// (set) Token: 0x06000046 RID: 70 RVA: 0x00002F4D File Offset: 0x0000114D
		public ModuleType Type { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002F56 File Offset: 0x00001156
		public bool HasMultiplayerCategory
		{
			get
			{
				return this.Category == ModuleCategory.Multiplayer || this.Category == ModuleCategory.MultiplayerOptional;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002F6C File Offset: 0x0000116C
		public bool IsNative
		{
			get
			{
				return this.Id.Equals("Native", StringComparison.OrdinalIgnoreCase);
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002F7F File Offset: 0x0000117F
		public ModuleInfo()
		{
			this.DependedModules = new List<DependedModule>();
			this.SubModules = new List<SubModuleInfo>();
			this.ModulesToLoadAfterThis = new List<DependedModule>();
			this.IncompatibleModules = new List<DependedModule>();
			this.IsActive = true;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002FBC File Offset: 0x000011BC
		private static string GetRequiredValueAttribute(XmlNode moduleNode, string elementName, string subModulePath)
		{
			XmlNode xmlNode = moduleNode.SelectSingleNode(elementName);
			if (xmlNode == null)
			{
				throw new Exception(string.Concat(new string[] { "Required element <", elementName, "> not found under <Module> in '", subModulePath, "'." }));
			}
			XmlAttributeCollection attributes = xmlNode.Attributes;
			XmlAttribute xmlAttribute = ((attributes != null) ? attributes["value"] : null);
			if (xmlAttribute == null)
			{
				throw new Exception(string.Concat(new string[] { "Element <", elementName, "> in '", subModulePath, "' is missing the required 'value' attribute." }));
			}
			return xmlAttribute.InnerText;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003058 File Offset: 0x00001258
		private static string GetValueAttribute(XmlNode node, string elementName, string subModulePath)
		{
			XmlAttributeCollection attributes = node.Attributes;
			XmlAttribute xmlAttribute = ((attributes != null) ? attributes["value"] : null);
			if (xmlAttribute == null)
			{
				throw new Exception(string.Concat(new string[] { "Element <", elementName, "> in '", subModulePath, "' is missing the required 'value' attribute." }));
			}
			return xmlAttribute.InnerText;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000030B8 File Offset: 0x000012B8
		private static string GetRequiredAttribute(XmlNode node, string attributeName, string elementDescription, string subModulePath)
		{
			XmlAttributeCollection attributes = node.Attributes;
			XmlAttribute xmlAttribute = ((attributes != null) ? attributes[attributeName] : null);
			if (xmlAttribute == null)
			{
				throw new Exception(string.Concat(new string[] { elementDescription, " in '", subModulePath, "' is missing the required '", attributeName, "' attribute." }));
			}
			return xmlAttribute.InnerText;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003118 File Offset: 0x00001318
		public void LoadWithFullPath(string fullPath)
		{
			this.SubModules.Clear();
			this.DependedModules.Clear();
			this.ModulesToLoadAfterThis.Clear();
			this.IncompatibleModules.Clear();
			this.FolderPath = fullPath;
			string text = this.FolderPath + "/SubModule.xml";
			Debug.Print("LoadWithFullPath  subModulePath = " + text, 0, Debug.DebugColor.White, 17592186044416UL);
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				using (StreamReader streamReader = new StreamReader(text))
				{
					xmlDocument.Load(streamReader);
				}
			}
			catch (XmlException ex)
			{
				throw new Exception(string.Format("Malformed XML in '{0}' at line {1}, position {2}: {3}", new object[] { text, ex.LineNumber, ex.LinePosition, ex.Message }), ex);
			}
			XmlNode xmlNode = xmlDocument.SelectSingleNode("Module");
			if (xmlNode == null)
			{
				throw new Exception("Root element <Module> not found in '" + text + "'. The file may be empty or have an unexpected root element.");
			}
			this.Name = ModuleInfo.GetRequiredValueAttribute(xmlNode, "Name", text);
			this.Id = ModuleInfo.GetRequiredValueAttribute(xmlNode, "Id", text);
			if (!this.Id.Contains(';'.ToString()))
			{
				this.Id.Contains(':'.ToString());
			}
			this.Version = ApplicationVersion.FromString(ModuleInfo.GetRequiredValueAttribute(xmlNode, "Version", text), 0);
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("RequiredBaseVersion");
			if (xmlNode2 != null)
			{
				this.RequiredBaseVersion = ApplicationVersion.FromString(ModuleInfo.GetValueAttribute(xmlNode2, "RequiredBaseVersion", text), 0);
			}
			XmlNode xmlNode3 = xmlNode.SelectSingleNode("DefaultModule");
			this.IsDefault = xmlNode3 != null && ModuleInfo.GetValueAttribute(xmlNode3, "DefaultModule", text).Equals("true");
			XmlNode xmlNode4 = xmlNode.SelectSingleNode("ModuleType");
			ModuleType moduleType;
			if (xmlNode4 != null && Enum.TryParse<ModuleType>(ModuleInfo.GetValueAttribute(xmlNode4, "ModuleType", text), out moduleType))
			{
				this.Type = moduleType;
			}
			this.IsSelected = this.IsNative;
			this.Category = ModuleCategory.Singleplayer;
			XmlNode xmlNode5 = xmlNode.SelectSingleNode("ModuleCategory");
			ModuleCategory moduleCategory;
			if (xmlNode5 != null && Enum.TryParse<ModuleCategory>(ModuleInfo.GetValueAttribute(xmlNode5, "ModuleCategory", text), out moduleCategory))
			{
				this.Category = moduleCategory;
			}
			XmlNode xmlNode6 = xmlNode.SelectSingleNode("DependedModules");
			XmlNodeList xmlNodeList = ((xmlNode6 != null) ? xmlNode6.SelectNodes("DependedModule") : null);
			if (xmlNodeList != null)
			{
				for (int i = 0; i < xmlNodeList.Count; i++)
				{
					string requiredAttribute = ModuleInfo.GetRequiredAttribute(xmlNodeList[i], "Id", "A <DependedModule> element", text);
					ApplicationVersion applicationVersion = ApplicationVersion.Empty;
					bool flag = false;
					if (xmlNodeList[i].Attributes["DependentVersion"] != null)
					{
						try
						{
							applicationVersion = ApplicationVersion.FromString(xmlNodeList[i].Attributes["DependentVersion"].InnerText, 0);
						}
						catch
						{
							string.Concat(new string[] { "Couldn't parse dependent version of ", requiredAttribute, " for ", this.Id, ". Using default version." });
						}
					}
					XmlAttribute xmlAttribute = xmlNodeList[i].Attributes["Optional"];
					bool flag2;
					if (bool.TryParse((xmlAttribute != null) ? xmlAttribute.InnerText : null, out flag2))
					{
						flag = flag2;
					}
					this.DependedModules.Add(new DependedModule(requiredAttribute, applicationVersion, flag));
				}
			}
			XmlNode xmlNode7 = xmlNode.SelectSingleNode("ModulesToLoadAfterThis");
			XmlNodeList xmlNodeList2 = ((xmlNode7 != null) ? xmlNode7.SelectNodes("Module") : null);
			if (xmlNodeList2 != null)
			{
				for (int j = 0; j < xmlNodeList2.Count; j++)
				{
					string requiredAttribute2 = ModuleInfo.GetRequiredAttribute(xmlNodeList2[j], "Id", "A <ModulesToLoadAfterThis> <Module> element", text);
					this.ModulesToLoadAfterThis.Add(new DependedModule(requiredAttribute2, ApplicationVersion.Empty, false));
				}
			}
			XmlNode xmlNode8 = xmlNode.SelectSingleNode("IncompatibleModules");
			XmlNodeList xmlNodeList3 = ((xmlNode8 != null) ? xmlNode8.SelectNodes("Module") : null);
			if (xmlNodeList3 != null)
			{
				for (int k = 0; k < xmlNodeList3.Count; k++)
				{
					string requiredAttribute3 = ModuleInfo.GetRequiredAttribute(xmlNodeList3[k], "Id", "An <IncompatibleModules> <Module> element", text);
					this.IncompatibleModules.Add(new DependedModule(requiredAttribute3, ApplicationVersion.Empty, false));
				}
			}
			XmlNode xmlNode9 = xmlNode.SelectSingleNode("SubModules");
			XmlNodeList xmlNodeList4 = ((xmlNode9 != null) ? xmlNode9.SelectNodes("SubModule") : null);
			if (xmlNodeList4 != null)
			{
				for (int l = 0; l < xmlNodeList4.Count; l++)
				{
					SubModuleInfo subModuleInfo = new SubModuleInfo();
					try
					{
						subModuleInfo.LoadFrom(xmlNodeList4[l], this.FolderPath, this.IsOfficial);
					}
					catch
					{
						string.Format("Cannot load a submodule {0} under {1}", l, this.FolderPath);
					}
					this.SubModules.Add(subModuleInfo);
				}
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000035FC File Offset: 0x000017FC
		public void ActivateModule()
		{
			this.IsActive = true;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003605 File Offset: 0x00001805
		public void DeactivateModule()
		{
			this.IsActive = false;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003610 File Offset: 0x00001810
		public void UpdateVersionChangeSet()
		{
			this.Version = new ApplicationVersion(this.Version.ApplicationVersionType, this.Version.Major, this.Version.Minor, this.Version.Revision, 122374);
		}

		// Token: 0x04000011 RID: 17
		private const int ModuleDefaultChangeSet = 0;

		// Token: 0x0400001C RID: 28
		public readonly List<SubModuleInfo> SubModules;

		// Token: 0x0400001D RID: 29
		public readonly List<DependedModule> DependedModules;

		// Token: 0x0400001E RID: 30
		public readonly List<DependedModule> ModulesToLoadAfterThis;

		// Token: 0x0400001F RID: 31
		public readonly List<DependedModule> IncompatibleModules;
	}
}
