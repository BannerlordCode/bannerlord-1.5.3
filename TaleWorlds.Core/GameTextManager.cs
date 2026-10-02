using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x02000078 RID: 120
	public class GameTextManager
	{
		// Token: 0x06000838 RID: 2104 RVA: 0x0001B355 File Offset: 0x00019555
		public GameTextManager()
		{
			this._gameTexts = new Dictionary<string, GameText>();
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0001B368 File Offset: 0x00019568
		public GameText GetGameText(string id)
		{
			GameText gameText;
			if (this._gameTexts.TryGetValue(id, out gameText))
			{
				return gameText;
			}
			return null;
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0001B388 File Offset: 0x00019588
		public GameText AddGameText(string id)
		{
			GameText gameText;
			if (!this._gameTexts.TryGetValue(id, out gameText))
			{
				gameText = new GameText(id);
				this._gameTexts.Add(gameText.Id, gameText);
			}
			return gameText;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0001B3C0 File Offset: 0x000195C0
		public bool TryGetText(string id, string variation, out TextObject text)
		{
			text = null;
			GameText gameText;
			this._gameTexts.TryGetValue(id, out gameText);
			if (gameText != null)
			{
				if (variation == null)
				{
					text = gameText.DefaultText;
				}
				else
				{
					text = gameText.GetVariation(variation);
				}
				if (text != null)
				{
					text = text.CopyTextObject();
					text.AddIDToValue(id);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0001B418 File Offset: 0x00019618
		public TextObject FindText(string id, string variation = null)
		{
			TextObject textObject;
			if (this.TryGetText(id, variation, out textObject))
			{
				return textObject;
			}
			TextObject textObject2;
			if (variation == null)
			{
				textObject2 = new TextObject("{=!}ERROR: Text with id " + id + " doesn't exist!", null);
			}
			else
			{
				textObject2 = new TextObject("{=!}ERROR: Text with id " + id + " doesn't exist! Variation: " + variation, null);
			}
			return textObject2;
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0001B468 File Offset: 0x00019668
		public IEnumerable<TextObject> FindAllTextVariations(string id)
		{
			GameText gameText;
			this._gameTexts.TryGetValue(id, out gameText);
			if (gameText != null)
			{
				foreach (GameText.GameTextVariation gameTextVariation in gameText.Variations)
				{
					yield return gameTextVariation.Text;
				}
				IEnumerator<GameText.GameTextVariation> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0001B480 File Offset: 0x00019680
		public void LoadGameTexts()
		{
			Game game = Game.Current;
			bool flag = false;
			string text = "";
			if (game != null)
			{
				flag = game.GameType.IsDevelopment;
				text = game.GameType.GetType().Name;
			}
			XmlDocument mergedXmlForManaged = MBObjectManager.GetMergedXmlForManaged("GameText", false, flag, text);
			try
			{
				this.LoadFromXML(mergedXmlForManaged);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0001B4E8 File Offset: 0x000196E8
		public void LoadDefaultTexts()
		{
			try
			{
				List<string> list = new List<string>();
				foreach (ModuleInfo moduleInfo in ModuleHelper.GetModules(null))
				{
					string text = moduleInfo.FolderPath + "/ModuleData/global_strings.xml";
					if (File.Exists(text))
					{
						list.Add(text);
					}
				}
				string text2 = ModuleHelper.GetModuleFullPath("Native") + "ModuleData/consoles.xml";
				if (File.Exists(text2))
				{
					list.Add(text2);
				}
				else
				{
					Debug.FailedAssert("Cant find Native/consoles.xml", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\GameTextManager.cs", "LoadDefaultTexts", 177);
				}
				foreach (string text3 in list)
				{
					Debug.Print("opening " + text3, 0, Debug.DebugColor.White, 17592186044416UL);
					XmlDocument xmlDocument = new XmlDocument();
					StreamReader streamReader = new StreamReader(text3);
					string text4 = streamReader.ReadToEnd();
					xmlDocument.LoadXml(text4);
					streamReader.Close();
					this.LoadFromXML(xmlDocument);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0001B64C File Offset: 0x0001984C
		private void LoadFromXML(XmlDocument doc)
		{
			XmlNode xmlNode = null;
			for (int i = 0; i < doc.ChildNodes.Count; i++)
			{
				XmlNode xmlNode2 = doc.ChildNodes[i];
				if (xmlNode2.NodeType != XmlNodeType.Comment && xmlNode2.Name == "strings" && xmlNode2.ChildNodes.Count > 0)
				{
					xmlNode = xmlNode2.ChildNodes[0];
					IL_01FF:
					while (xmlNode != null)
					{
						try
						{
							if (xmlNode.Name == "string" && xmlNode.NodeType != XmlNodeType.Comment)
							{
								if (xmlNode.Attributes == null)
								{
									throw new TWXmlLoadException("Node attributes are null.");
								}
								string[] array = xmlNode.Attributes["id"].Value.Split(new char[] { '.' });
								string text = array[0];
								GameText gameText = this.AddGameText(text);
								string text2 = "";
								if (array.Length > 1)
								{
									text2 = array[1];
								}
								TextObject textObject = new TextObject(xmlNode.Attributes["text"].Value, null);
								List<GameTextManager.ChoiceTag> list = new List<GameTextManager.ChoiceTag>();
								foreach (object obj in xmlNode.ChildNodes)
								{
									XmlNode xmlNode3 = (XmlNode)obj;
									if (xmlNode3.Name == "tags")
									{
										XmlNodeList childNodes = xmlNode3.ChildNodes;
										for (int j = 0; j < childNodes.Count; j++)
										{
											XmlAttributeCollection attributes = childNodes[j].Attributes;
											if (attributes != null)
											{
												int num = 1;
												if (attributes["weight"] != null)
												{
													int.TryParse(attributes["weight"].Value, out num);
												}
												GameTextManager.ChoiceTag choiceTag = new GameTextManager.ChoiceTag(attributes["tag_name"].Value, num);
												list.Add(choiceTag);
											}
										}
									}
								}
								textObject.CacheTokens();
								gameText.AddVariationWithId(text2, textObject, list);
							}
						}
						catch (Exception)
						{
						}
						finally
						{
							xmlNode = xmlNode.NextSibling;
						}
					}
					return;
				}
			}
			goto IL_01FF;
		}

		// Token: 0x04000425 RID: 1061
		private readonly Dictionary<string, GameText> _gameTexts;

		// Token: 0x0200011B RID: 283
		public struct ChoiceTag
		{
			// Token: 0x17000401 RID: 1025
			// (get) Token: 0x06000C03 RID: 3075 RVA: 0x00026783 File Offset: 0x00024983
			// (set) Token: 0x06000C04 RID: 3076 RVA: 0x0002678B File Offset: 0x0002498B
			public string TagName { get; private set; }

			// Token: 0x17000402 RID: 1026
			// (get) Token: 0x06000C05 RID: 3077 RVA: 0x00026794 File Offset: 0x00024994
			// (set) Token: 0x06000C06 RID: 3078 RVA: 0x0002679C File Offset: 0x0002499C
			public uint Weight { get; private set; }

			// Token: 0x17000403 RID: 1027
			// (get) Token: 0x06000C07 RID: 3079 RVA: 0x000267A5 File Offset: 0x000249A5
			// (set) Token: 0x06000C08 RID: 3080 RVA: 0x000267AD File Offset: 0x000249AD
			public bool IsTagReversed { get; private set; }

			// Token: 0x06000C09 RID: 3081 RVA: 0x000267B6 File Offset: 0x000249B6
			public ChoiceTag(string tagName, int weight)
			{
				this = default(GameTextManager.ChoiceTag);
				this.TagName = tagName;
				this.Weight = (uint)MathF.Abs(weight);
				this.IsTagReversed = weight < 0;
			}
		}
	}
}
