using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x02000018 RID: 24
	public class BannerManager
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00004BDA File Offset: 0x00002DDA
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00004BE1 File Offset: 0x00002DE1
		public static BannerManager Instance { get; private set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00004BE9 File Offset: 0x00002DE9
		public MBReadOnlyList<BannerIconGroup> BannerIconGroups
		{
			get
			{
				return this._bannerIconGroups;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00004BF1 File Offset: 0x00002DF1
		// (set) Token: 0x060000FE RID: 254 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public int BaseBackgroundId { get; private set; }

		// Token: 0x060000FF RID: 255 RVA: 0x00004C02 File Offset: 0x00002E02
		private BannerManager()
		{
			this._bannerIconGroups = new MBList<BannerIconGroup>();
			this._colorPalette = new Dictionary<int, BannerColor>();
			this._cultureColorPalette = new Dictionary<BasicCultureObject, List<BannerColor>>();
			this.ReadOnlyColorPalette = this._colorPalette.GetReadOnlyDictionary<int, BannerColor>();
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00004C3C File Offset: 0x00002E3C
		public static void Initialize()
		{
			if (BannerManager.Instance == null)
			{
				BannerManager.Instance = new BannerManager();
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00004C4F File Offset: 0x00002E4F
		public static void ResetAndLoad()
		{
			BannerManager.Instance._bannerIconGroups = new MBList<BannerIconGroup>();
			BannerManager.Instance._colorPalette = new Dictionary<int, BannerColor>();
			BannerManager.Instance._cultureColorPalette = new Dictionary<BasicCultureObject, List<BannerColor>>();
			BannerManager.Instance.LoadBannerIcons();
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00004C88 File Offset: 0x00002E88
		private static MBReadOnlyDictionary<int, BannerColor> ColorPalette
		{
			get
			{
				return BannerManager.Instance.ReadOnlyColorPalette;
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00004C94 File Offset: 0x00002E94
		public static uint GetColor(int id)
		{
			BannerColor bannerColor;
			if (BannerManager.ColorPalette.TryGetValue(id, out bannerColor))
			{
				return bannerColor.Color;
			}
			return 3735928559U;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00004CC0 File Offset: 0x00002EC0
		public static int GetColorId(uint color)
		{
			foreach (KeyValuePair<int, BannerColor> keyValuePair in BannerManager.ColorPalette)
			{
				if (keyValuePair.Value.Color == color)
				{
					return keyValuePair.Key;
				}
			}
			return -1;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00004D2C File Offset: 0x00002F2C
		public int GetRandomColorId(MBFastRandom random)
		{
			return BannerManager.ColorPalette.ElementAt<KeyValuePair<int, BannerColor>>(random.Next(BannerManager.ColorPalette.Count<KeyValuePair<int, BannerColor>>())).Key;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00004D5C File Offset: 0x00002F5C
		public BannerIconData GetIconDataFromIconId(int id)
		{
			using (List<BannerIconGroup>.Enumerator enumerator = this._bannerIconGroups.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BannerIconData bannerIconData;
					if (enumerator.Current.AllIcons.TryGetValue(id, out bannerIconData))
					{
						return bannerIconData;
					}
				}
			}
			return default(BannerIconData);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00004DC8 File Offset: 0x00002FC8
		public int GetRandomBackgroundId(MBFastRandom random)
		{
			int num = random.Next(0, this._availablePatternCount);
			foreach (BannerIconGroup bannerIconGroup in this.BannerIconGroups)
			{
				if (bannerIconGroup.IsPattern)
				{
					if (num < bannerIconGroup.AllBackgrounds.Count)
					{
						return bannerIconGroup.AllBackgrounds.ElementAt<KeyValuePair<int, string>>(num).Key;
					}
					num -= bannerIconGroup.AllBackgrounds.Count;
				}
			}
			return -1;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00004E64 File Offset: 0x00003064
		public int GetRandomBannerIconId(MBFastRandom random)
		{
			int num = random.Next(0, this._availableIconCount);
			foreach (BannerIconGroup bannerIconGroup in this.BannerIconGroups)
			{
				if (!bannerIconGroup.IsPattern)
				{
					if (num < bannerIconGroup.AvailableIcons.Count)
					{
						return bannerIconGroup.AvailableIcons.ElementAt<KeyValuePair<int, BannerIconData>>(num).Key;
					}
					num -= bannerIconGroup.AvailableIcons.Count;
				}
			}
			return -1;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00004F00 File Offset: 0x00003100
		public string GetBackgroundMeshName(int id)
		{
			foreach (BannerIconGroup bannerIconGroup in this.BannerIconGroups)
			{
				if (bannerIconGroup.IsPattern && bannerIconGroup.AllBackgrounds.ContainsKey(id))
				{
					return bannerIconGroup.AllBackgrounds[id];
				}
			}
			return null;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00004F74 File Offset: 0x00003174
		public string GetIconSourceTextureName(int id)
		{
			foreach (BannerIconGroup bannerIconGroup in this.BannerIconGroups)
			{
				if (!bannerIconGroup.IsPattern && bannerIconGroup.AllBackgrounds.ContainsKey(id))
				{
					return bannerIconGroup.AllBackgrounds[id];
				}
			}
			return null;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00004FE8 File Offset: 0x000031E8
		public void SetBaseBackgroundId(int id)
		{
			this.BaseBackgroundId = id;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00004FF1 File Offset: 0x000031F1
		public void SetCultureColors(BasicCultureObject culture, List<BannerColor> color)
		{
			if (!this._cultureColorPalette.ContainsKey(culture))
			{
				this._cultureColorPalette[culture] = color;
				return;
			}
			Debug.FailedAssert("Culture colors already set", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\BannerManager.cs", "SetCultureColors", 200);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00005028 File Offset: 0x00003228
		public void LoadBannerIcons()
		{
			Game game = Game.Current;
			bool flag = false;
			string text = "";
			if (game != null)
			{
				flag = game.GameType.IsDevelopment;
				text = game.GameType.GetType().Name;
			}
			XmlDocument mergedXmlForManaged = MBObjectManager.GetMergedXmlForManaged("BannerIcons", false, flag, text);
			this.LoadBannerIconsFromXml(mergedXmlForManaged);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00005078 File Offset: 0x00003278
		public void LoadBannerIcons(string xmlPath)
		{
			XmlDocument xmlDocument = this.LoadXmlFile(xmlPath);
			this.LoadBannerIconsFromXml(xmlDocument);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005094 File Offset: 0x00003294
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

		// Token: 0x06000110 RID: 272 RVA: 0x000050E0 File Offset: 0x000032E0
		private void LoadBannerIconsFromXml(XmlDocument doc)
		{
			Debug.Print("loading banner_icons.xml:", 0, Debug.DebugColor.White, 17592186044416UL);
			XmlNodeList elementsByTagName = doc.GetElementsByTagName("base");
			if (elementsByTagName.Count != 1)
			{
				throw new TWXmlLoadException("Incorrect XML document format.");
			}
			XmlNode xmlNode = elementsByTagName[0].ChildNodes[0];
			if (xmlNode.Name != "BannerIconData")
			{
				throw new TWXmlLoadException("Incorrect XML document format.");
			}
			if (xmlNode.Name == "BannerIconData")
			{
				foreach (object obj in xmlNode.ChildNodes)
				{
					XmlNode xmlNode2 = (XmlNode)obj;
					if (xmlNode2.Name == "BannerIconGroup")
					{
						BannerIconGroup bannerIconGroup = new BannerIconGroup();
						bannerIconGroup.Deserialize(xmlNode2, this._bannerIconGroups);
						BannerIconGroup bannerIconGroup3 = this._bannerIconGroups.FirstOrDefault<BannerIconGroup>((BannerIconGroup x) => x.Id == bannerIconGroup.Id);
						if (bannerIconGroup3 == null)
						{
							this._bannerIconGroups.Add(bannerIconGroup);
						}
						else
						{
							bannerIconGroup3.Merge(bannerIconGroup);
						}
					}
					if (xmlNode2.Name == "BannerColors")
					{
						foreach (object obj2 in xmlNode2.ChildNodes)
						{
							XmlNode xmlNode3 = (XmlNode)obj2;
							if (xmlNode3.Name == "Color")
							{
								int num = Convert.ToInt32(xmlNode3.Attributes["id"].Value);
								if (!this._colorPalette.ContainsKey(num))
								{
									uint num2 = Convert.ToUInt32(xmlNode3.Attributes["hex"].Value, 16);
									XmlAttribute xmlAttribute = xmlNode3.Attributes["player_can_choose_for_sigil"];
									bool flag = Convert.ToBoolean(((xmlAttribute != null) ? xmlAttribute.Value : null) ?? "false");
									XmlAttribute xmlAttribute2 = xmlNode3.Attributes["player_can_choose_for_background"];
									bool flag2 = Convert.ToBoolean(((xmlAttribute2 != null) ? xmlAttribute2.Value : null) ?? "false");
									this._colorPalette.Add(num, new BannerColor(num2, flag, flag2));
								}
							}
						}
					}
				}
			}
			this._availablePatternCount = 0;
			this._availableIconCount = 0;
			foreach (BannerIconGroup bannerIconGroup2 in this._bannerIconGroups)
			{
				if (bannerIconGroup2.IsPattern)
				{
					this._availablePatternCount += bannerIconGroup2.AllBackgrounds.Count;
				}
				else
				{
					this._availableIconCount += bannerIconGroup2.AvailableIcons.Count;
				}
			}
		}

		// Token: 0x04000123 RID: 291
		public const int DarkRed = 1;

		// Token: 0x04000124 RID: 292
		public const int Green = 120;

		// Token: 0x04000125 RID: 293
		public const int Blue = 119;

		// Token: 0x04000126 RID: 294
		public const int Purple = 4;

		// Token: 0x04000127 RID: 295
		public const int DarkPurple = 6;

		// Token: 0x04000128 RID: 296
		public const int Orange = 9;

		// Token: 0x04000129 RID: 297
		public const int DarkBlue = 12;

		// Token: 0x0400012A RID: 298
		public const int Red = 118;

		// Token: 0x0400012B RID: 299
		public const int Yellow = 121;

		// Token: 0x0400012D RID: 301
		public MBReadOnlyDictionary<int, BannerColor> ReadOnlyColorPalette;

		// Token: 0x0400012E RID: 302
		private Dictionary<BasicCultureObject, List<BannerColor>> _cultureColorPalette;

		// Token: 0x0400012F RID: 303
		private Dictionary<int, BannerColor> _colorPalette;

		// Token: 0x04000130 RID: 304
		private MBList<BannerIconGroup> _bannerIconGroups;

		// Token: 0x04000132 RID: 306
		private int _availablePatternCount;

		// Token: 0x04000133 RID: 307
		private int _availableIconCount;
	}
}
