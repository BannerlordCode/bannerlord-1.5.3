using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x02000021 RID: 33
	public class FontFactory
	{
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x0000E056 File Offset: 0x0000C256
		// (set) Token: 0x060002C7 RID: 711 RVA: 0x0000E05E File Offset: 0x0000C25E
		public Language DefaultLanguage { get; private set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x0000E068 File Offset: 0x0000C268
		// (set) Token: 0x060002C9 RID: 713 RVA: 0x0000E226 File Offset: 0x0000C426
		public Language CurrentLanguage
		{
			get
			{
				if (this._currentLangugage != null)
				{
					return this._currentLangugage;
				}
				if (this.DefaultLanguage != null)
				{
					string text = "Couldn't find language in language map: ";
					Language currentLangugage = this._currentLangugage;
					Debug.Print(text + ((currentLangugage != null) ? currentLangugage.LanguageID : null), 0, Debug.DebugColor.White, 17592186044416UL);
					string text2 = "Couldn't find language in language map: ";
					Language currentLangugage2 = this._currentLangugage;
					Debug.FailedAssert(text2 + ((currentLangugage2 != null) ? currentLangugage2.LanguageID : null), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\FontFactory.cs", "CurrentLanguage", 26);
					this._currentLangugage = this.DefaultLanguage;
					return this._currentLangugage;
				}
				Language language;
				if (this._fontLanguageMap.TryGetValue("English", out language))
				{
					string text3 = "Couldn't find default language(";
					Language defaultLanguage = this.DefaultLanguage;
					Debug.Print(text3 + (((defaultLanguage != null) ? defaultLanguage.LanguageID : null) ?? "INVALID") + ") in language map.", 0, Debug.DebugColor.White, 17592186044416UL);
					string text4 = "Couldn't find default language(";
					Language defaultLanguage2 = this.DefaultLanguage;
					Debug.FailedAssert(text4 + (((defaultLanguage2 != null) ? defaultLanguage2.LanguageID : null) ?? "INVALID") + ") in language map.", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\FontFactory.cs", "CurrentLanguage", 35);
					this.DefaultLanguage = language;
					this._currentLangugage = language;
					return this._currentLangugage;
				}
				Debug.Print("Couldn't find English language in language map.", 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.FailedAssert("Couldn't find English language in language map.", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\FontFactory.cs", "CurrentLanguage", 45);
				this.DefaultLanguage = this._fontLanguageMap.FirstOrDefault<KeyValuePair<string, Language>>().Value;
				this._currentLangugage = this.DefaultLanguage;
				if (this._currentLangugage == null)
				{
					Debug.Print("There are no languages in language map", 0, Debug.DebugColor.White, 17592186044416UL);
					Debug.FailedAssert("There are no languages in language map", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\FontFactory.cs", "CurrentLanguage", 54);
				}
				return this._currentLangugage;
			}
			private set
			{
				if (value != this._currentLangugage)
				{
					this._currentLangugage = value;
				}
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060002CA RID: 714 RVA: 0x0000E238 File Offset: 0x0000C438
		public Font DefaultFont
		{
			get
			{
				return this.CurrentLanguage.DefaultFont;
			}
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000E245 File Offset: 0x0000C445
		public FontFactory(ResourceDepot resourceDepot)
		{
			this._resourceDepot = resourceDepot;
			this._bitmapFonts = new Dictionary<string, Font>();
			this._fontLanguageMap = new Dictionary<string, Language>();
			this._resourceDepot.OnResourceChange += this.OnResourceChange;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000E281 File Offset: 0x0000C481
		private void OnResourceChange()
		{
			this.CheckForUpdates();
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000E28C File Offset: 0x0000C48C
		public void LoadAllFonts(SpriteData spriteData)
		{
			foreach (string text in this._resourceDepot.GetFiles("Fonts", ".fnt", false))
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
				this.TryAddFontDefinition(Path.GetDirectoryName(text) + "/", fileNameWithoutExtension, spriteData);
			}
			foreach (string text2 in this._resourceDepot.GetFiles("Fonts", ".xml", false))
			{
				if (Path.GetFileNameWithoutExtension(text2).EndsWith("Languages"))
				{
					try
					{
						this.LoadLocalizationValues(text2);
					}
					catch (Exception)
					{
						Debug.FailedAssert("Failed to load language at path: " + text2, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\FontFactory.cs", "LoadAllFonts", 128);
					}
				}
			}
			Language language;
			if (this.DefaultLanguage == null && this._fontLanguageMap.TryGetValue("English", out language))
			{
				this.DefaultLanguage = language;
				this.CurrentLanguage = this.DefaultLanguage;
			}
			this._latestSpriteData = spriteData;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000E394 File Offset: 0x0000C594
		public bool TryAddFontDefinition(string fontPath, string fontName, SpriteData spriteData)
		{
			Font font = new Font(fontName);
			string text = fontPath + fontName + ".fnt";
			bool flag = font.TryLoadFontFromPath(text, spriteData);
			if (flag)
			{
				this._bitmapFonts.Add(fontName, font);
			}
			return flag;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000E3D0 File Offset: 0x0000C5D0
		public void LoadLocalizationValues(string sourceXMLPath)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(sourceXMLPath);
			XmlElement xmlElement = xmlDocument["Languages"];
			XmlAttribute xmlAttribute = xmlElement.Attributes["DefaultLanguage"];
			if (xmlAttribute != null)
			{
				string innerText = xmlAttribute.InnerText;
			}
			foreach (object obj in xmlElement)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name == "Language")
				{
					Language language = Language.CreateFrom(xmlNode, this);
					Language language2;
					if (this._fontLanguageMap.TryGetValue(language.LanguageID, out language2))
					{
						this._fontLanguageMap[language.LanguageID] = language;
					}
					else
					{
						this._fontLanguageMap.Add(language.LanguageID, language);
					}
				}
			}
			XmlAttribute xmlAttribute2 = xmlElement.Attributes["DefaultLanguage"];
			string text = ((xmlAttribute2 != null) ? xmlAttribute2.InnerText : null);
			Language language3;
			if (!string.IsNullOrEmpty(text) && this._fontLanguageMap.TryGetValue(text, out language3))
			{
				this.DefaultLanguage = language3;
				this.CurrentLanguage = this.DefaultLanguage;
				return;
			}
			Debug.FailedAssert("DefaultLanguage cannot be found in the dictionary.", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\FontFactory.cs", "LoadLocalizationValues", 200);
			if (this._fontLanguageMap.TryGetValue("English", out language3))
			{
				this.DefaultLanguage = language3;
				this.CurrentLanguage = this.DefaultLanguage;
				return;
			}
			Debug.FailedAssert("English cannot be found in the dictionary.", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\FontFactory.cs", "LoadLocalizationValues", 209);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000E560 File Offset: 0x0000C760
		public Font GetFont(string fontName)
		{
			if (this._bitmapFonts.ContainsKey(fontName))
			{
				return this._bitmapFonts[fontName];
			}
			return this.DefaultFont;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000E583 File Offset: 0x0000C783
		public IEnumerable<Font> GetFonts()
		{
			return this._bitmapFonts.Values;
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000E590 File Offset: 0x0000C790
		public string GetFontName(Font font)
		{
			return this._bitmapFonts.FirstOrDefault<KeyValuePair<string, Font>>((KeyValuePair<string, Font> f) => f.Value == font).Key;
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000E5CC File Offset: 0x0000C7CC
		public Font GetMappedFontForLocalization(string englishFontName)
		{
			if (string.IsNullOrEmpty(englishFontName))
			{
				return this.DefaultFont;
			}
			if (this.DefaultLanguage != this.CurrentLanguage && this.CurrentLanguage != null && this.CurrentLanguage.FontMapHasKey(englishFontName))
			{
				return this.CurrentLanguage.GetMappedFont(englishFontName);
			}
			return this.GetFont(englishFontName);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000E620 File Offset: 0x0000C820
		public void OnLanguageChange(string newLanguageCode)
		{
			Language currentLanguage = this.CurrentLanguage;
			if (((currentLanguage != null) ? currentLanguage.LanguageID : null) != newLanguageCode)
			{
				Language language;
				if (!string.IsNullOrEmpty(newLanguageCode) && this._fontLanguageMap.TryGetValue(newLanguageCode, out language))
				{
					this.CurrentLanguage = language;
					return;
				}
				Debug.FailedAssert(newLanguageCode + " doesn't exist in the dictionary!", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\FontFactory.cs", "OnLanguageChange", 260);
			}
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000E688 File Offset: 0x0000C888
		public Font GetUsableFontForCharacter(int characterCode)
		{
			for (int i = 0; i < this._fontLanguageMap.Values.Count; i++)
			{
				if (this._fontLanguageMap.ElementAt<KeyValuePair<string, Language>>(i).Value.DefaultFont.Characters.ContainsKey(characterCode))
				{
					return this._fontLanguageMap.ElementAt<KeyValuePair<string, Language>>(i).Value.DefaultFont;
				}
			}
			return null;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000E6F4 File Offset: 0x0000C8F4
		public void CheckForUpdates()
		{
			Language currentLanguage = this.CurrentLanguage;
			if (currentLanguage != null)
			{
				string languageID = currentLanguage.LanguageID;
			}
			this.DefaultLanguage = null;
			this.CurrentLanguage = null;
			this._bitmapFonts.Clear();
			this._fontLanguageMap.Clear();
			this.LoadAllFonts(this._latestSpriteData);
			Language language = null;
			if (language != null)
			{
				this.CurrentLanguage = language;
			}
		}

		// Token: 0x0400016C RID: 364
		private Language _currentLangugage;

		// Token: 0x0400016D RID: 365
		private readonly Dictionary<string, Font> _bitmapFonts;

		// Token: 0x0400016E RID: 366
		private readonly ResourceDepot _resourceDepot;

		// Token: 0x0400016F RID: 367
		private readonly Dictionary<string, Language> _fontLanguageMap;

		// Token: 0x04000170 RID: 368
		private SpriteData _latestSpriteData;
	}
}
