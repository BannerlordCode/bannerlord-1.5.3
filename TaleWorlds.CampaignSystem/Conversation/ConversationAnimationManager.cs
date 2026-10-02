using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.CampaignSystem.Conversation
{
	// Token: 0x02000241 RID: 577
	public class ConversationAnimationManager
	{
		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06002303 RID: 8963 RVA: 0x0009ABAE File Offset: 0x00098DAE
		// (set) Token: 0x06002304 RID: 8964 RVA: 0x0009ABB6 File Offset: 0x00098DB6
		public Dictionary<string, ConversationAnimData> ConversationAnims { get; private set; }

		// Token: 0x06002305 RID: 8965 RVA: 0x0009ABBF File Offset: 0x00098DBF
		public ConversationAnimationManager()
		{
			this.ConversationAnims = new Dictionary<string, ConversationAnimData>();
			this.LoadConversationAnimData(ModuleHelper.GetModuleFullPath("Sandbox") + "ModuleData/conversation_animations.xml");
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x0009ABEC File Offset: 0x00098DEC
		private void LoadConversationAnimData(string xmlPath)
		{
			XmlDocument xmlDocument = this.LoadXmlFile(xmlPath);
			this.LoadFromXml(xmlDocument);
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x0009AC08 File Offset: 0x00098E08
		private XmlDocument LoadXmlFile(string path)
		{
			Debug.Print("opening " + path, 0, Debug.DebugColor.White, 17592186044416UL);
			XmlDocument xmlDocument = new XmlDocument();
			StreamReader streamReader = new StreamReader(path);
			string text = streamReader.ReadToEnd();
			xmlDocument.LoadXml(text);
			streamReader.Close();
			return xmlDocument;
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x0009AC54 File Offset: 0x00098E54
		private void LoadFromXml(XmlDocument doc)
		{
			if (doc.ChildNodes.Count <= 1)
			{
				throw new TWXmlLoadException("Incorrect XML document format.");
			}
			if (doc.ChildNodes[1].Name != "ConversationAnimations")
			{
				throw new TWXmlLoadException("Incorrect XML document format.");
			}
			foreach (object obj in doc.DocumentElement.SelectNodes("IdleAnim"))
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Attributes != null)
				{
					KeyValuePair<string, ConversationAnimData> keyValuePair = new KeyValuePair<string, ConversationAnimData>(xmlNode.Attributes["id"].Value, new ConversationAnimData());
					keyValuePair.Value.IdleAnimStart = xmlNode.Attributes["action_id_1"].Value;
					keyValuePair.Value.IdleAnimLoop = xmlNode.Attributes["action_id_2"].Value;
					keyValuePair.Value.FamilyType = 0;
					XmlAttribute xmlAttribute = xmlNode.Attributes["family_type"];
					int num;
					if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value) && int.TryParse(xmlAttribute.Value, out num))
					{
						keyValuePair.Value.FamilyType = num;
					}
					keyValuePair.Value.MountFamilyType = 0;
					XmlAttribute xmlAttribute2 = xmlNode.Attributes["mount_family_type"];
					int num2;
					if (xmlAttribute2 != null && !string.IsNullOrEmpty(xmlAttribute2.Value) && int.TryParse(xmlAttribute2.Value, out num2))
					{
						keyValuePair.Value.MountFamilyType = num2;
					}
					foreach (object obj2 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode2 = (XmlNode)obj2;
						if (xmlNode2.Name == "Reactions")
						{
							foreach (object obj3 in xmlNode2.ChildNodes)
							{
								XmlNode xmlNode3 = (XmlNode)obj3;
								if (xmlNode3.Name == "Reaction" && xmlNode3.Attributes["id"] != null && xmlNode3.Attributes["action_id"] != null)
								{
									keyValuePair.Value.Reactions.Add(xmlNode3.Attributes["id"].Value, xmlNode3.Attributes["action_id"].Value);
								}
							}
						}
					}
					this.ConversationAnims.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}
	}
}
