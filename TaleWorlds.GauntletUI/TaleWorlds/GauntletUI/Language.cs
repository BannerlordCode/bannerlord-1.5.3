using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x02000022 RID: 34
	public class Language : ILanguage
	{
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x0000E74F File Offset: 0x0000C94F
		// (set) Token: 0x060002D8 RID: 728 RVA: 0x0000E757 File Offset: 0x0000C957
		public char[] ForbiddenStartOfLineCharacters { get; private set; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0000E760 File Offset: 0x0000C960
		// (set) Token: 0x060002DA RID: 730 RVA: 0x0000E768 File Offset: 0x0000C968
		public char[] ForbiddenEndOfLineCharacters { get; private set; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060002DB RID: 731 RVA: 0x0000E771 File Offset: 0x0000C971
		// (set) Token: 0x060002DC RID: 732 RVA: 0x0000E779 File Offset: 0x0000C979
		public string LanguageID { get; private set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060002DD RID: 733 RVA: 0x0000E782 File Offset: 0x0000C982
		// (set) Token: 0x060002DE RID: 734 RVA: 0x0000E78A File Offset: 0x0000C98A
		public string DefaultFontName { get; private set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060002DF RID: 735 RVA: 0x0000E793 File Offset: 0x0000C993
		// (set) Token: 0x060002E0 RID: 736 RVA: 0x0000E79B File Offset: 0x0000C99B
		public bool DoesFontRequireSpaceForNewline { get; private set; } = true;

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x0000E7A4 File Offset: 0x0000C9A4
		// (set) Token: 0x060002E2 RID: 738 RVA: 0x0000E7AC File Offset: 0x0000C9AC
		public Font DefaultFont { get; private set; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x0000E7B5 File Offset: 0x0000C9B5
		// (set) Token: 0x060002E4 RID: 740 RVA: 0x0000E7BD File Offset: 0x0000C9BD
		public char LineSeperatorChar { get; private set; }

		// Token: 0x060002E5 RID: 741 RVA: 0x0000E7C6 File Offset: 0x0000C9C6
		public bool FontMapHasKey(string keyFontName)
		{
			return this._fontMap.ContainsKey(keyFontName);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000E7D4 File Offset: 0x0000C9D4
		public Font GetMappedFont(string keyFontName)
		{
			return this._fontMap[keyFontName];
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000E7E2 File Offset: 0x0000C9E2
		private Language()
		{
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000E7FC File Offset: 0x0000C9FC
		public static Language CreateFrom(XmlNode languageNode, FontFactory fontFactory)
		{
			Language language = new Language
			{
				LanguageID = languageNode.Attributes["id"].InnerText
			};
			Language language2 = language;
			XmlAttribute xmlAttribute = languageNode.Attributes["DefaultFont"];
			language2.DefaultFontName = ((xmlAttribute != null) ? xmlAttribute.InnerText : null) ?? "Galahad";
			Language language3 = language;
			XmlAttribute xmlAttribute2 = languageNode.Attributes["LineSeperatorChar"];
			language3.LineSeperatorChar = ((xmlAttribute2 != null) ? xmlAttribute2.InnerText[0] : '-');
			language.DefaultFont = fontFactory.GetFont(language.DefaultFontName);
			foreach (object obj in languageNode.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					if (xmlNode.Name == "Map")
					{
						string innerText = xmlNode.Attributes["From"].InnerText;
						string innerText2 = xmlNode.Attributes["To"].InnerText;
						language._fontMap.Add(innerText, fontFactory.GetFont(innerText2));
					}
					else if (xmlNode.Name == "NewlineDoesntRequireSpace")
					{
						language.DoesFontRequireSpaceForNewline = false;
					}
					else if (xmlNode.Name == "ForbiddenStartOfLineCharacters")
					{
						Language language4 = language;
						XmlAttribute xmlAttribute3 = xmlNode.Attributes["Characters"];
						language4.ForbiddenStartOfLineCharacters = ((xmlAttribute3 != null) ? xmlAttribute3.InnerText.ToCharArray() : null);
					}
					else if (xmlNode.Name == "ForbiddenEndOfLineCharacters")
					{
						Language language5 = language;
						XmlAttribute xmlAttribute4 = xmlNode.Attributes["Characters"];
						language5.ForbiddenEndOfLineCharacters = ((xmlAttribute4 != null) ? xmlAttribute4.InnerText.ToCharArray() : null);
					}
				}
			}
			return language;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000E9E0 File Offset: 0x0000CBE0
		IEnumerable<char> ILanguage.GetForbiddenStartOfLineCharacters()
		{
			return this.ForbiddenStartOfLineCharacters;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000E9E8 File Offset: 0x0000CBE8
		IEnumerable<char> ILanguage.GetForbiddenEndOfLineCharacters()
		{
			return this.ForbiddenEndOfLineCharacters;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000E9F0 File Offset: 0x0000CBF0
		bool ILanguage.IsCharacterForbiddenAtStartOfLine(char character)
		{
			if (this.ForbiddenStartOfLineCharacters == null || this.ForbiddenStartOfLineCharacters.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < this.ForbiddenStartOfLineCharacters.Length; i++)
			{
				if (this.ForbiddenStartOfLineCharacters[i] == character)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000EA34 File Offset: 0x0000CC34
		bool ILanguage.IsCharacterForbiddenAtEndOfLine(char character)
		{
			if (this.ForbiddenEndOfLineCharacters == null || this.ForbiddenEndOfLineCharacters.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < this.ForbiddenEndOfLineCharacters.Length; i++)
			{
				if (this.ForbiddenEndOfLineCharacters[i] == character)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000EA75 File Offset: 0x0000CC75
		string ILanguage.GetLanguageID()
		{
			return this.LanguageID;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000EA7D File Offset: 0x0000CC7D
		string ILanguage.GetDefaultFontName()
		{
			return this.DefaultFontName;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000EA85 File Offset: 0x0000CC85
		Font ILanguage.GetDefaultFont()
		{
			return this.DefaultFont;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000EA8D File Offset: 0x0000CC8D
		char ILanguage.GetLineSeperatorChar()
		{
			return this.LineSeperatorChar;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000EA95 File Offset: 0x0000CC95
		bool ILanguage.DoesLanguageRequireSpaceForNewline()
		{
			return this.DoesFontRequireSpaceForNewline;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000EA9D File Offset: 0x0000CC9D
		bool ILanguage.FontMapHasKey(string keyFontName)
		{
			return this._fontMap.ContainsKey(keyFontName);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000EAAB File Offset: 0x0000CCAB
		Font ILanguage.GetMappedFont(string keyFontName)
		{
			return this._fontMap[keyFontName];
		}

		// Token: 0x04000178 RID: 376
		private readonly Dictionary<string, Font> _fontMap = new Dictionary<string, Font>();
	}
}
