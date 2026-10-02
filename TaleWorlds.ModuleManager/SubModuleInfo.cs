using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.ModuleManager
{
	// Token: 0x02000009 RID: 9
	public class SubModuleInfo
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00003665 File Offset: 0x00001865
		// (set) Token: 0x06000052 RID: 82 RVA: 0x0000366D File Offset: 0x0000186D
		public string Name { get; private set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00003676 File Offset: 0x00001876
		// (set) Token: 0x06000054 RID: 84 RVA: 0x0000367E File Offset: 0x0000187E
		public string DLLName { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00003687 File Offset: 0x00001887
		// (set) Token: 0x06000056 RID: 86 RVA: 0x0000368F File Offset: 0x0000188F
		public string DLLPath { get; private set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00003698 File Offset: 0x00001898
		// (set) Token: 0x06000058 RID: 88 RVA: 0x000036A0 File Offset: 0x000018A0
		public bool IsTWCertifiedDLL { get; private set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000059 RID: 89 RVA: 0x000036A9 File Offset: 0x000018A9
		// (set) Token: 0x0600005A RID: 90 RVA: 0x000036B1 File Offset: 0x000018B1
		public bool DLLExists { get; private set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600005B RID: 91 RVA: 0x000036BA File Offset: 0x000018BA
		// (set) Token: 0x0600005C RID: 92 RVA: 0x000036C2 File Offset: 0x000018C2
		public List<string> Assemblies { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600005D RID: 93 RVA: 0x000036CB File Offset: 0x000018CB
		// (set) Token: 0x0600005E RID: 94 RVA: 0x000036D3 File Offset: 0x000018D3
		public string SubModuleClassTypeName { get; private set; }

		// Token: 0x0600005F RID: 95 RVA: 0x000036DC File Offset: 0x000018DC
		public SubModuleInfo()
		{
			this.Tags = new List<Tuple<SubModuleInfo.SubModuleTags, string>>();
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000036F0 File Offset: 0x000018F0
		public void LoadFrom(XmlNode subModuleNode, string path, bool isOfficial)
		{
			this.Tags.Clear();
			this.Name = subModuleNode.SelectSingleNode("Name").Attributes["value"].InnerText;
			this.DLLName = subModuleNode.SelectSingleNode("DLLName").Attributes["value"].InnerText;
			string text = this.DLLName;
			text = Path.Combine(path, "bin\\Win64_Shipping_Client", this.DLLName);
			if (!string.IsNullOrEmpty(this.DLLName))
			{
				this.DLLExists = File.Exists(text);
				this.DLLPath = text;
				if (!this.DLLExists)
				{
					Debug.Print("Couldn't find .dll: " + this.DLLPath, 0, Debug.DebugColor.White, 17592186044416UL);
				}
				this.IsTWCertifiedDLL = this.DLLExists && this.GetIsTWCertified(text, isOfficial);
			}
			this.SubModuleClassTypeName = subModuleNode.SelectSingleNode("SubModuleClassType").Attributes["value"].InnerText;
			this.Assemblies = new List<string>();
			if (subModuleNode.SelectSingleNode("Assemblies") != null)
			{
				XmlNodeList xmlNodeList = subModuleNode.SelectSingleNode("Assemblies").SelectNodes("Assembly");
				for (int i = 0; i < xmlNodeList.Count; i++)
				{
					this.Assemblies.Add(xmlNodeList[i].Attributes["value"].InnerText);
				}
			}
			XmlNode xmlNode = subModuleNode.SelectSingleNode("Tags");
			if (xmlNode != null)
			{
				XmlNodeList xmlNodeList2 = xmlNode.SelectNodes("Tag");
				for (int j = 0; j < xmlNodeList2.Count; j++)
				{
					SubModuleInfo.SubModuleTags subModuleTags;
					if (Enum.TryParse<SubModuleInfo.SubModuleTags>(xmlNodeList2[j].Attributes["key"].InnerText, out subModuleTags))
					{
						string innerText = xmlNodeList2[j].Attributes["value"].InnerText;
						this.Tags.Add(new Tuple<SubModuleInfo.SubModuleTags, string>(subModuleTags, innerText));
						if (subModuleTags == SubModuleInfo.SubModuleTags.DedicatedServerType && innerText != "none")
						{
							this.IsTWCertifiedDLL = true;
						}
					}
				}
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003904 File Offset: 0x00001B04
		private bool GetIsTWCertified(string fileName, bool isOfficial)
		{
			bool flag;
			try
			{
				X509Certificate2 x509Certificate = new X509Certificate2(fileName);
				X509Chain x509Chain = X509Chain.Create();
				x509Chain.Build(x509Certificate);
				foreach (X509ChainElement x509ChainElement in x509Chain.ChainElements)
				{
					if (x509ChainElement.Certificate.GetCertHashString() == "64A1AC8AF5E01DAE9F1AC49DDDA4688DCC17BB2C" && x509ChainElement.Certificate.GetSerialNumberString() == "4C4242B31A43B99BAB36C723")
					{
						return true;
					}
				}
				flag = false;
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x04000024 RID: 36
		private const string CertHashString = "64A1AC8AF5E01DAE9F1AC49DDDA4688DCC17BB2C";

		// Token: 0x04000025 RID: 37
		private const string CertSerialNum = "4C4242B31A43B99BAB36C723";

		// Token: 0x0400002D RID: 45
		public readonly List<Tuple<SubModuleInfo.SubModuleTags, string>> Tags;

		// Token: 0x02000013 RID: 19
		public enum SubModuleTags
		{
			// Token: 0x04000043 RID: 67
			RejectedPlatform,
			// Token: 0x04000044 RID: 68
			ExclusivePlatform,
			// Token: 0x04000045 RID: 69
			DedicatedServerType,
			// Token: 0x04000046 RID: 70
			IsNoRenderModeElement,
			// Token: 0x04000047 RID: 71
			DependantRuntimeLibrary,
			// Token: 0x04000048 RID: 72
			PlayerHostedDedicatedServer,
			// Token: 0x04000049 RID: 73
			EngineType
		}
	}
}
