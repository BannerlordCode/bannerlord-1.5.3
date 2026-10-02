using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000031 RID: 49
	public class SpriteData
	{
		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600022D RID: 557 RVA: 0x000089BF File Offset: 0x00006BBF
		// (set) Token: 0x0600022E RID: 558 RVA: 0x000089C7 File Offset: 0x00006BC7
		public Dictionary<string, SpritePart> SpriteParts { get; private set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600022F RID: 559 RVA: 0x000089D0 File Offset: 0x00006BD0
		// (set) Token: 0x06000230 RID: 560 RVA: 0x000089D8 File Offset: 0x00006BD8
		public Dictionary<string, Sprite> Sprites { get; private set; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000231 RID: 561 RVA: 0x000089E1 File Offset: 0x00006BE1
		// (set) Token: 0x06000232 RID: 562 RVA: 0x000089E9 File Offset: 0x00006BE9
		public Dictionary<string, SpriteCategory> SpriteCategories { get; private set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000233 RID: 563 RVA: 0x000089F2 File Offset: 0x00006BF2
		// (set) Token: 0x06000234 RID: 564 RVA: 0x000089FA File Offset: 0x00006BFA
		public string Name { get; private set; }

		// Token: 0x06000235 RID: 565 RVA: 0x00008A03 File Offset: 0x00006C03
		public SpriteData(string name)
		{
			this.Name = name;
			this.SpriteParts = new Dictionary<string, SpritePart>();
			this.Sprites = new Dictionary<string, Sprite>();
			this.SpriteCategories = new Dictionary<string, SpriteCategory>();
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00008A34 File Offset: 0x00006C34
		public Sprite GetSprite(string name)
		{
			Sprite sprite;
			if (this.Sprites.TryGetValue(name, out sprite))
			{
				return sprite;
			}
			return null;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00008A54 File Offset: 0x00006C54
		public bool SpriteExists(string spriteName)
		{
			return this.GetSprite(spriteName) != null;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00008A60 File Offset: 0x00006C60
		private static SpriteData.SpriteDataLoadResult LoadFromDepot(ResourceDepot resourceDepot, string name)
		{
			XmlDocument xmlDocument = new XmlDocument();
			SpriteData.SpriteDataLoadResult spriteDataLoadResult = new SpriteData.SpriteDataLoadResult
			{
				SpriteCategories = new Dictionary<string, SpriteCategory>(),
				SpriteNames = new Dictionary<string, Sprite>(),
				SpritePartNames = new Dictionary<string, SpritePart>()
			};
			foreach (string text in resourceDepot.GetFilesEndingWith(name + ".xml"))
			{
				try
				{
					SpriteData.LoadSpriteDataFromFile(xmlDocument, text, ref spriteDataLoadResult);
				}
				catch (Exception)
				{
					Debug.FailedAssert("Failed to load sprite data from file: " + text, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.TwoDimension\\SpriteData.cs", "LoadFromDepot", 72);
				}
			}
			return spriteDataLoadResult;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00008B20 File Offset: 0x00006D20
		private static SpriteData.SpriteDataLoadResult LoadSpriteDataFromFile(XmlDocument spriteData, string filePath, ref SpriteData.SpriteDataLoadResult loadResult)
		{
			StreamReader streamReader = new StreamReader(filePath);
			spriteData.Load(streamReader);
			XmlElement xmlElement = spriteData["SpriteData"];
			XmlNode xmlNode = xmlElement["SpriteCategories"];
			XmlNode xmlNode2 = xmlElement["SpriteParts"];
			XmlNode xmlNode3 = xmlElement["Sprites"];
			foreach (object obj in xmlNode)
			{
				XmlNode xmlNode4 = (XmlNode)obj;
				string innerText = xmlNode4["Name"].InnerText;
				int num = Convert.ToInt32(xmlNode4["SpriteSheetCount"].InnerText);
				bool flag = false;
				Vec2i[] array = new Vec2i[num];
				foreach (object obj2 in xmlNode4.ChildNodes)
				{
					XmlNode xmlNode5 = (XmlNode)obj2;
					if (xmlNode5.Name == "SpriteSheetSize")
					{
						int num2 = Convert.ToInt32(xmlNode5.Attributes["ID"].InnerText);
						int num3 = Convert.ToInt32(xmlNode5.Attributes["Width"].InnerText);
						int num4 = Convert.ToInt32(xmlNode5.Attributes["Height"].InnerText);
						array[num2 - 1] = new Vec2i(num3, num4);
					}
					else if (xmlNode5.Name == "AlwaysLoad")
					{
						flag = true;
					}
				}
				SpriteCategory spriteCategory = new SpriteCategory(innerText, num, flag)
				{
					SheetSizes = array
				};
				loadResult.SpriteCategories[spriteCategory.Name] = spriteCategory;
			}
			foreach (object obj3 in xmlNode2)
			{
				XmlNode xmlNode6 = (XmlNode)obj3;
				string innerText2 = xmlNode6["Name"].InnerText;
				int num5 = Convert.ToInt32(xmlNode6["Width"].InnerText);
				int num6 = Convert.ToInt32(xmlNode6["Height"].InnerText);
				string innerText3 = xmlNode6["CategoryName"].InnerText;
				SpriteCategory spriteCategory2 = loadResult.SpriteCategories[innerText3];
				SpritePart spritePart = new SpritePart(innerText2, spriteCategory2, num5, num6)
				{
					SheetID = Convert.ToInt32(xmlNode6["SheetID"].InnerText),
					SheetX = Convert.ToInt32(xmlNode6["SheetX"].InnerText),
					SheetY = Convert.ToInt32(xmlNode6["SheetY"].InnerText)
				};
				loadResult.SpritePartNames[spritePart.Name] = spritePart;
				spritePart.UpdateInitValues();
			}
			foreach (object obj4 in xmlNode3)
			{
				XmlNode xmlNode7 = (XmlNode)obj4;
				Sprite sprite = null;
				if (xmlNode7.Name == "GenericSprite")
				{
					string innerText4 = xmlNode7["Name"].InnerText;
					string innerText5 = xmlNode7["SpritePartName"].InnerText;
					SpritePart spritePart2 = loadResult.SpritePartNames[innerText5];
					sprite = new SpriteGeneric(innerText4, spritePart2, in SpriteNinePatchParameters.Empty);
				}
				else if (xmlNode7.Name == "NineRegionSprite")
				{
					string innerText6 = xmlNode7["Name"].InnerText;
					string innerText7 = xmlNode7["SpritePartName"].InnerText;
					int num7 = Convert.ToInt32(xmlNode7["LeftWidth"].InnerText);
					int num8 = Convert.ToInt32(xmlNode7["RightWidth"].InnerText);
					int num9 = Convert.ToInt32(xmlNode7["TopHeight"].InnerText);
					int num10 = Convert.ToInt32(xmlNode7["BottomHeight"].InnerText);
					SpriteNinePatchParameters spriteNinePatchParameters = new SpriteNinePatchParameters(num7, num8, num9, num10);
					sprite = new SpriteGeneric(innerText6, loadResult.SpritePartNames[innerText7], in spriteNinePatchParameters);
				}
				loadResult.SpriteNames[sprite.Name] = sprite;
			}
			return loadResult;
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00008FD0 File Offset: 0x000071D0
		public void Load(ResourceDepot resourceDepot)
		{
			SpriteData.SpriteDataLoadResult spriteDataLoadResult = SpriteData.LoadFromDepot(resourceDepot, this.Name);
			this.SpriteCategories = spriteDataLoadResult.SpriteCategories;
			this.Sprites = spriteDataLoadResult.SpriteNames;
			this.SpriteParts = spriteDataLoadResult.SpritePartNames;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00009010 File Offset: 0x00007210
		public void Reload(ResourceDepot resourceDepot, ITwoDimensionResourceContext resourceContext)
		{
			SpriteData.SpriteDataLoadResult spriteDataLoadResult = SpriteData.LoadFromDepot(resourceDepot, this.Name);
			this.Sprites = spriteDataLoadResult.SpriteNames;
			this.SpriteParts = spriteDataLoadResult.SpritePartNames;
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			foreach (KeyValuePair<string, SpriteCategory> keyValuePair in this.SpriteCategories)
			{
				bool flag = false;
				foreach (KeyValuePair<string, SpriteCategory> keyValuePair2 in spriteDataLoadResult.SpriteCategories)
				{
					if (keyValuePair2.Key == keyValuePair.Key)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (KeyValuePair<string, SpriteCategory> keyValuePair3 in spriteDataLoadResult.SpriteCategories)
			{
				bool flag2 = false;
				foreach (KeyValuePair<string, SpriteCategory> keyValuePair4 in this.SpriteCategories)
				{
					if (keyValuePair3.Key == keyValuePair4.Key)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					list2.Add(keyValuePair3.Key);
				}
			}
			foreach (string text in list)
			{
				this.SpriteCategories[text].Unload();
				this.SpriteCategories.Remove(text);
			}
			foreach (string text2 in list2)
			{
				SpriteCategory spriteCategory = spriteDataLoadResult.SpriteCategories[text2];
				this.SpriteCategories.Add(text2, spriteCategory);
				if (spriteCategory.AlwaysLoad)
				{
					spriteCategory.Load(resourceContext, resourceDepot);
				}
			}
			foreach (KeyValuePair<string, SpriteCategory> keyValuePair5 in this.SpriteCategories)
			{
				SpriteCategory spriteCategory2;
				if (spriteDataLoadResult.SpriteCategories.TryGetValue(keyValuePair5.Key, out spriteCategory2))
				{
					keyValuePair5.Value.Reload(resourceContext, resourceDepot, spriteCategory2);
				}
			}
			foreach (KeyValuePair<string, SpritePart> keyValuePair6 in this.SpriteParts)
			{
				SpriteCategory spriteCategory3;
				if (this.SpriteCategories.TryGetValue(keyValuePair6.Value.Category.Name, out spriteCategory3))
				{
					keyValuePair6.Value.Category = spriteCategory3;
				}
			}
		}

		// Token: 0x02000046 RID: 70
		private struct SpriteDataLoadResult
		{
			// Token: 0x04000164 RID: 356
			public Dictionary<string, SpritePart> SpritePartNames;

			// Token: 0x04000165 RID: 357
			public Dictionary<string, Sprite> SpriteNames;

			// Token: 0x04000166 RID: 358
			public Dictionary<string, SpriteCategory> SpriteCategories;
		}
	}
}
