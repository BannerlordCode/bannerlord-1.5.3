using System;
using System.IO;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x020000A4 RID: 164
	public sealed class ManagedParameters : IManagedParametersInitializer
	{
		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x0600138A RID: 5002 RVA: 0x00057F0E File Offset: 0x0005610E
		public static ManagedParameters Instance { get; } = new ManagedParameters();

		// Token: 0x0600138B RID: 5003 RVA: 0x00057F18 File Offset: 0x00056118
		public void Initialize(string relativeXmlPath)
		{
			XmlDocument xmlDocument = ManagedParameters.LoadXmlFile(relativeXmlPath);
			this.LoadFromXml(xmlDocument);
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x00057F34 File Offset: 0x00056134
		private void LoadFromXml(XmlNode doc)
		{
			XmlNode xmlNode = null;
			if (doc.ChildNodes[1].ChildNodes[0].Name == "managed_campaign_parameters")
			{
				xmlNode = doc.ChildNodes[1].ChildNodes[0].ChildNodes[0];
			}
			while (xmlNode != null)
			{
				ManagedParametersEnum managedParametersEnum;
				if (xmlNode.Name == "managed_campaign_parameter" && xmlNode.NodeType != XmlNodeType.Comment && Enum.TryParse<ManagedParametersEnum>(xmlNode.Attributes["id"].Value, true, out managedParametersEnum))
				{
					this._managedParametersArray[(int)managedParametersEnum] = bool.Parse(xmlNode.Attributes["value"].Value);
				}
				xmlNode = xmlNode.NextSibling;
			}
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x00057FFC File Offset: 0x000561FC
		private static XmlDocument LoadXmlFile(string path)
		{
			Debug.Print("opening " + path, 0, Debug.DebugColor.White, 17592186044416UL);
			XmlDocument xmlDocument = new XmlDocument();
			StreamReader streamReader = new StreamReader(path);
			string text = streamReader.ReadToEnd();
			xmlDocument.LoadXml(text);
			streamReader.Close();
			return xmlDocument;
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x00058045 File Offset: 0x00056245
		public bool GetManagedParameter(ManagedParametersEnum _managedParametersEnum)
		{
			return this._managedParametersArray[(int)_managedParametersEnum];
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x00058050 File Offset: 0x00056250
		public bool SetManagedParameter(ManagedParametersEnum _managedParametersEnum, bool value)
		{
			this._managedParametersArray[(int)_managedParametersEnum] = value;
			return value;
		}

		// Token: 0x0400064B RID: 1611
		private readonly bool[] _managedParametersArray = new bool[2];
	}
}
