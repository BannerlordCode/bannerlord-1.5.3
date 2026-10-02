using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000322 RID: 802
	public static class MultiplayerGameTypes
	{
		// Token: 0x06002DE0 RID: 11744 RVA: 0x000B27DB File Offset: 0x000B09DB
		public static void Initialize()
		{
			MultiplayerGameTypes.CreateGameTypeInformations();
			MultiplayerGameTypes.LoadMultiplayerSceneInformations();
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x000B27E7 File Offset: 0x000B09E7
		public static bool CheckGameTypeInfoExists(string gameType)
		{
			return MultiplayerGameTypes._multiplayerGameTypeInfos.ContainsKey(gameType);
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x000B27F4 File Offset: 0x000B09F4
		public static MultiplayerGameTypeInfo GetGameTypeInfo(string gameType)
		{
			if (MultiplayerGameTypes._multiplayerGameTypeInfos.ContainsKey(gameType))
			{
				return MultiplayerGameTypes._multiplayerGameTypeInfos[gameType];
			}
			Debug.Print("Cannot find game type:" + gameType, 0, Debug.DebugColor.White, 17592186044416UL);
			return null;
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x000B282C File Offset: 0x000B0A2C
		private static void LoadMultiplayerSceneInformations()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(ModuleHelper.GetModuleFullPath("Native") + "ModuleData/Multiplayer/MultiplayerScenes.xml");
			foreach (object obj in xmlDocument.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name == "MultiplayerScenes")
				{
					using (IEnumerator enumerator2 = xmlNode.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							XmlNode xmlNode2 = (XmlNode)obj2;
							if (xmlNode2.NodeType != XmlNodeType.Comment)
							{
								string innerText = xmlNode2.Attributes["name"].InnerText;
								foreach (object obj3 in xmlNode2.ChildNodes)
								{
									XmlNode xmlNode3 = (XmlNode)obj3;
									if (xmlNode3.NodeType != XmlNodeType.Comment)
									{
										string innerText2 = xmlNode3.Attributes["name"].InnerText;
										if (MultiplayerGameTypes._multiplayerGameTypeInfos.ContainsKey(innerText2))
										{
											MultiplayerGameTypes._multiplayerGameTypeInfos[innerText2].Scenes.Add(innerText);
										}
									}
								}
							}
						}
						break;
					}
				}
			}
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x000B29E4 File Offset: 0x000B0BE4
		private static void CreateGameTypeInformations()
		{
			MultiplayerGameTypes._multiplayerGameTypeInfos = new Dictionary<string, MultiplayerGameTypeInfo>();
			foreach (MultiplayerGameTypeInfo multiplayerGameTypeInfo in Module.CurrentModule.GetMultiplayerGameTypes())
			{
				MultiplayerGameTypes._multiplayerGameTypeInfos.Add(multiplayerGameTypeInfo.GameType, multiplayerGameTypeInfo);
			}
		}

		// Token: 0x0400121C RID: 4636
		private static Dictionary<string, MultiplayerGameTypeInfo> _multiplayerGameTypeInfos;
	}
}
