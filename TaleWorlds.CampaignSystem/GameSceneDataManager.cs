using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000092 RID: 146
	public class GameSceneDataManager
	{
		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x060012B9 RID: 4793 RVA: 0x000567B5 File Offset: 0x000549B5
		// (set) Token: 0x060012BA RID: 4794 RVA: 0x000567BC File Offset: 0x000549BC
		public static GameSceneDataManager Instance { get; private set; }

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x060012BB RID: 4795 RVA: 0x000567C4 File Offset: 0x000549C4
		public MBReadOnlyList<SingleplayerBattleSceneData> SingleplayerBattleScenes
		{
			get
			{
				return this._singleplayerBattleScenes;
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x060012BC RID: 4796 RVA: 0x000567CC File Offset: 0x000549CC
		public MBReadOnlyList<ConversationSceneData> ConversationScenes
		{
			get
			{
				return this._conversationScenes;
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x060012BD RID: 4797 RVA: 0x000567D4 File Offset: 0x000549D4
		public MBReadOnlyList<MeetingSceneData> MeetingScenes
		{
			get
			{
				return this._meetingScenes;
			}
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x000567DC File Offset: 0x000549DC
		public GameSceneDataManager()
		{
			this._singleplayerBattleScenes = new MBList<SingleplayerBattleSceneData>();
			this._conversationScenes = new MBList<ConversationSceneData>();
			this._meetingScenes = new MBList<MeetingSceneData>();
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x00056805 File Offset: 0x00054A05
		internal static void Initialize()
		{
			GameSceneDataManager.Instance = new GameSceneDataManager();
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x00056811 File Offset: 0x00054A11
		internal static void Destroy()
		{
			GameSceneDataManager.Instance = null;
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x0005681C File Offset: 0x00054A1C
		public void LoadSPBattleScenes(string path)
		{
			XmlDocument xmlDocument = this.LoadXmlFile(path);
			this.LoadSPBattleScenes(xmlDocument);
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x00056838 File Offset: 0x00054A38
		public void LoadConversationScenes(string path)
		{
			XmlDocument xmlDocument = this.LoadXmlFile(path);
			this.LoadConversationScenes(xmlDocument);
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x00056854 File Offset: 0x00054A54
		public void LoadMeetingScenes(string path)
		{
			XmlDocument xmlDocument = this.LoadXmlFile(path);
			this.LoadMeetingScenes(xmlDocument);
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x00056870 File Offset: 0x00054A70
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

		// Token: 0x060012C5 RID: 4805 RVA: 0x000568BC File Offset: 0x00054ABC
		private void LoadSPBattleScenes(XmlDocument doc)
		{
			Debug.Print("loading sp_battles.xml", 0, Debug.DebugColor.White, 17592186044416UL);
			if (doc.ChildNodes.Count <= 1)
			{
				throw new TWXmlLoadException("Incorrect XML document format. XML document must have at least 2 child nodes.");
			}
			XmlNode xmlNode = doc.ChildNodes[1];
			if (xmlNode.Name != "SPBattleScenes")
			{
				throw new TWXmlLoadException("Incorrect XML document format. Root node's name must be SPBattleScenes.");
			}
			if (xmlNode.Name == "SPBattleScenes")
			{
				foreach (object obj in xmlNode.ChildNodes)
				{
					XmlNode xmlNode2 = (XmlNode)obj;
					if (xmlNode2.NodeType != XmlNodeType.Comment)
					{
						string text = null;
						List<int> list = new List<int>();
						TerrainType terrainType = TerrainType.Plain;
						ForestDensity forestDensity = ForestDensity.None;
						bool flag = false;
						for (int i = 0; i < xmlNode2.Attributes.Count; i++)
						{
							if (xmlNode2.Attributes[i].Name == "id")
							{
								text = xmlNode2.Attributes[i].InnerText;
							}
							else if (xmlNode2.Attributes[i].Name == "map_indices")
							{
								foreach (string text2 in xmlNode2.Attributes[i].InnerText.Replace(" ", "").Split(new char[] { ',' }))
								{
									list.Add(int.Parse(text2));
								}
							}
							else if (xmlNode2.Attributes[i].Name == "terrain")
							{
								if (!Enum.TryParse<TerrainType>(xmlNode2.Attributes[i].InnerText, out terrainType))
								{
									terrainType = TerrainType.Plain;
								}
							}
							else if (xmlNode2.Attributes[i].Name == "forest_density")
							{
								char[] array2 = xmlNode2.Attributes[i].InnerText.ToLower().ToCharArray();
								array2[0] = char.ToUpper(array2[0]);
								if (!Enum.TryParse<ForestDensity>(new string(array2), out forestDensity))
								{
									forestDensity = ForestDensity.None;
								}
							}
							else if (xmlNode2.Attributes[i].Name == "is_naval")
							{
								bool.TryParse(xmlNode2.Attributes[i].Value, out flag);
							}
						}
						XmlNodeList childNodes = xmlNode2.ChildNodes;
						List<TerrainType> list2 = new List<TerrainType>();
						foreach (object obj2 in childNodes)
						{
							XmlNode xmlNode3 = (XmlNode)obj2;
							if (xmlNode3.NodeType != XmlNodeType.Comment && xmlNode3.Name == "TerrainTypes")
							{
								foreach (object obj3 in xmlNode3.ChildNodes)
								{
									XmlNode xmlNode4 = (XmlNode)obj3;
									TerrainType terrainType2;
									if (xmlNode4.Name == "TerrainType" && Enum.TryParse<TerrainType>(xmlNode4.Attributes["name"].InnerText, out terrainType2) && !list2.Contains(terrainType2))
									{
										list2.Add(terrainType2);
									}
								}
							}
						}
						this._singleplayerBattleScenes.Add(new SingleplayerBattleSceneData(text, terrainType, list2, forestDensity, list, flag));
					}
				}
			}
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x00056C9C File Offset: 0x00054E9C
		private void LoadConversationScenes(XmlDocument doc)
		{
			Debug.Print("loading conversation_scenes.xml", 0, Debug.DebugColor.White, 17592186044416UL);
			if (doc.ChildNodes.Count <= 1)
			{
				throw new TWXmlLoadException("Incorrect XML document format. XML document must have at least 2 child nodes.");
			}
			XmlNode xmlNode = doc.ChildNodes[1];
			if (xmlNode.Name != "ConversationScenes")
			{
				throw new TWXmlLoadException("Incorrect XML document format. Root node's name must be ConversationScenes.");
			}
			if (xmlNode.Name == "ConversationScenes")
			{
				foreach (object obj in xmlNode.ChildNodes)
				{
					XmlNode xmlNode2 = (XmlNode)obj;
					if (xmlNode2.NodeType != XmlNodeType.Comment)
					{
						string text = null;
						TerrainType terrainType = TerrainType.Plain;
						ForestDensity forestDensity = ForestDensity.None;
						for (int i = 0; i < xmlNode2.Attributes.Count; i++)
						{
							if (xmlNode2.Attributes[i].Name == "id")
							{
								text = xmlNode2.Attributes[i].InnerText;
							}
							else if (xmlNode2.Attributes[i].Name == "terrain")
							{
								if (!Enum.TryParse<TerrainType>(xmlNode2.Attributes[i].InnerText, out terrainType))
								{
									terrainType = TerrainType.Plain;
								}
							}
							else if (xmlNode2.Attributes[i].Name == "forest_density")
							{
								char[] array = xmlNode2.Attributes[i].InnerText.ToLower().ToCharArray();
								array[0] = char.ToUpper(array[0]);
								if (!Enum.TryParse<ForestDensity>(new string(array), out forestDensity))
								{
									forestDensity = ForestDensity.None;
								}
							}
						}
						XmlNodeList childNodes = xmlNode2.ChildNodes;
						List<TerrainType> list = new List<TerrainType>();
						foreach (object obj2 in childNodes)
						{
							XmlNode xmlNode3 = (XmlNode)obj2;
							if (xmlNode3.NodeType != XmlNodeType.Comment && xmlNode3.Name == "flags")
							{
								foreach (object obj3 in xmlNode3.ChildNodes)
								{
									XmlNode xmlNode4 = (XmlNode)obj3;
									TerrainType terrainType2;
									if (xmlNode4.NodeType != XmlNodeType.Comment && xmlNode4.Attributes["name"].InnerText == "TerrainType" && Enum.TryParse<TerrainType>(xmlNode4.Attributes["value"].InnerText, out terrainType2) && !list.Contains(terrainType2))
									{
										list.Add(terrainType2);
									}
								}
							}
						}
						this._conversationScenes.Add(new ConversationSceneData(text, terrainType, list, forestDensity));
					}
				}
			}
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x00056FC8 File Offset: 0x000551C8
		private void LoadMeetingScenes(XmlDocument doc)
		{
			Debug.Print("loading meeting_scenes.xml", 0, Debug.DebugColor.White, 17592186044416UL);
			if (doc.ChildNodes.Count <= 1)
			{
				throw new TWXmlLoadException("Incorrect XML document format. XML document must have at least 2 child nodes.");
			}
			XmlNode xmlNode = doc.ChildNodes[1];
			if (xmlNode.Name != "MeetingScenes")
			{
				throw new TWXmlLoadException("Incorrect XML document format. Root node's name must be MeetingScenes.");
			}
			if (xmlNode.Name == "MeetingScenes")
			{
				foreach (object obj in xmlNode.ChildNodes)
				{
					XmlNode xmlNode2 = (XmlNode)obj;
					if (xmlNode2.NodeType != XmlNodeType.Comment)
					{
						string text = null;
						string text2 = null;
						for (int i = 0; i < xmlNode2.Attributes.Count; i++)
						{
							if (xmlNode2.Attributes[i].Name == "id")
							{
								text = xmlNode2.Attributes[i].InnerText;
							}
							if (xmlNode2.Attributes[i].Name == "culture")
							{
								text2 = xmlNode2.Attributes[i].InnerText.Split(new char[] { '.' })[1];
							}
						}
						this._meetingScenes.Add(new MeetingSceneData(text, text2));
					}
				}
			}
		}

		// Token: 0x04000623 RID: 1571
		private MBList<SingleplayerBattleSceneData> _singleplayerBattleScenes;

		// Token: 0x04000624 RID: 1572
		private MBList<ConversationSceneData> _conversationScenes;

		// Token: 0x04000625 RID: 1573
		private MBList<MeetingSceneData> _meetingScenes;

		// Token: 0x04000626 RID: 1574
		private const TerrainType DefaultTerrain = TerrainType.Plain;

		// Token: 0x04000627 RID: 1575
		private const ForestDensity DefaultForestDensity = ForestDensity.None;
	}
}
