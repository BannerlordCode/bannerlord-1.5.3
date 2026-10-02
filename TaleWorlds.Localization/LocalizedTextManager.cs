using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization
{
	// Token: 0x02000006 RID: 6
	public static class LocalizedTextManager
	{
		// Token: 0x0600004C RID: 76 RVA: 0x00002DB4 File Offset: 0x00000FB4
		public static string GetTranslatedText(string languageId, string id)
		{
			string text;
			if (LocalizedTextManager._gameTextDictionary.TryGetValue(id, out text))
			{
				return text;
			}
			return null;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002DD4 File Offset: 0x00000FD4
		public static List<string> GetLanguageIds(bool developmentMode)
		{
			List<string> list = new List<string>();
			foreach (LanguageData languageData in LanguageData.All)
			{
				bool flag = developmentMode || !languageData.IsUnderDevelopment;
				if (languageData != null && languageData.IsValid && flag)
				{
					list.Add(languageData.StringId);
				}
			}
			return list;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002E54 File Offset: 0x00001054
		public static string GetLanguageTitle(string id)
		{
			LanguageData languageData = LanguageData.GetLanguageData(id);
			if (languageData != null)
			{
				return languageData.Title;
			}
			return LanguageData.GetLanguageData("English").Title;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002E84 File Offset: 0x00001084
		public static LanguageSpecificTextProcessor CreateTextProcessorForLanguage(string id)
		{
			LanguageData languageData = LanguageData.GetLanguageData(id);
			if (languageData == null || languageData.TextProcessor == null)
			{
				return new DefaultTextProcessor();
			}
			Type type = Type.GetType(languageData.TextProcessor);
			if (type == null)
			{
				Debug.FailedAssert("Can't find the type: " + languageData.TextProcessor, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Localization\\LocalizedTextManager.cs", "CreateTextProcessorForLanguage", 71);
				return new DefaultTextProcessor();
			}
			return (LanguageSpecificTextProcessor)Activator.CreateInstance(type);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002EF0 File Offset: 0x000010F0
		public static void AddLanguageTest(string id, string processor)
		{
			LanguageData languageData = new LanguageData(id);
			languageData.InitializeDefault(id, new string[] { id }, id, processor, false);
			LanguageData.LoadTestData(languageData);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002F20 File Offset: 0x00001120
		public static int GetLanguageIndex(string id)
		{
			int num = LanguageData.GetLanguageDataIndex(id);
			if (num == -1)
			{
				num = LanguageData.GetLanguageDataIndex("English");
			}
			return num;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002F44 File Offset: 0x00001144
		public static void LoadLocalizationXmls(string[] loadedModules)
		{
			LanguageData.Clear();
			for (int i = 0; i < loadedModules.Length; i++)
			{
				string text = loadedModules[i] + "/ModuleData/Languages";
				if (Directory.Exists(text))
				{
					string[] array;
					try
					{
						array = Directory.GetFiles(text, "language_data.xml", SearchOption.AllDirectories);
					}
					catch (Exception ex)
					{
						Debug.FailedAssert("Exception occurred in LoadLocalizationXmls: " + ex.Message, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Localization\\LocalizedTextManager.cs", "LoadLocalizationXmls", 119);
						array = new string[0];
					}
					string[] array2 = array;
					for (int j = 0; j < array2.Length; j++)
					{
						XmlDocument xmlDocument = LocalizedTextManager.LoadXmlFile(array2[j]);
						if (xmlDocument != null)
						{
							LanguageData.LoadFromXml(xmlDocument, text);
						}
					}
				}
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002FFC File Offset: 0x000011FC
		public static void AddLocalizationXml(string newModule)
		{
			string text = newModule + "/ModuleData/Languages";
			if (Directory.Exists(text))
			{
				string[] array;
				try
				{
					array = Directory.GetFiles(text, "language_data.xml", SearchOption.AllDirectories);
				}
				catch (Exception ex)
				{
					Debug.FailedAssert("Exception occurred in LoadLocalizationXmls: " + ex.Message, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Localization\\LocalizedTextManager.cs", "AddLocalizationXml", 147);
					array = new string[0];
				}
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					XmlDocument xmlDocument = LocalizedTextManager.LoadXmlFile(array2[i]);
					if (xmlDocument != null)
					{
						LanguageData.LoadFromXml(xmlDocument, text);
					}
				}
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003098 File Offset: 0x00001298
		public static string GetDateFormattedByLanguage(string languageCode, DateTime dateTime)
		{
			string shortDatePattern = LocalizedTextManager.GetCultureInfo(languageCode).DateTimeFormat.ShortDatePattern;
			return dateTime.ToString(shortDatePattern);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000030C0 File Offset: 0x000012C0
		public static string GetTimeFormattedByLanguage(string languageCode, DateTime dateTime)
		{
			string shortTimePattern = LocalizedTextManager.GetCultureInfo(languageCode).DateTimeFormat.ShortTimePattern;
			return dateTime.ToString(shortTimePattern);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000030E6 File Offset: 0x000012E6
		public static string GetSubtitleExtensionOfLanguage(string languageId)
		{
			return LocalizedTextManager.GetLanguageData(languageId).SubtitleExtension;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000030F4 File Offset: 0x000012F4
		public static string GetLocalizationCodeOfISOLanguageCode(string isoLanguageCode)
		{
			foreach (LanguageData languageData in LanguageData.All)
			{
				string[] supportedIsoCodes = languageData.SupportedIsoCodes;
				for (int i = 0; i < supportedIsoCodes.Length; i++)
				{
					if (string.Equals(supportedIsoCodes[i], isoLanguageCode, StringComparison.InvariantCultureIgnoreCase))
					{
						return languageData.StringId;
					}
				}
			}
			Debug.FailedAssert("Undefined language code " + isoLanguageCode, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Localization\\LocalizedTextManager.cs", "GetLocalizationCodeOfISOLanguageCode", 202);
			return "English";
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003194 File Offset: 0x00001394
		private static CultureInfo GetCultureInfo(string languageId)
		{
			LanguageData languageData = LocalizedTextManager.GetLanguageData(languageId);
			CultureInfo cultureInfo = CultureInfo.InvariantCulture;
			if (languageData.SupportedIsoCodes != null && languageData.SupportedIsoCodes.Length != 0)
			{
				cultureInfo = new CultureInfo(languageData.SupportedIsoCodes[0]);
			}
			return cultureInfo;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000031D0 File Offset: 0x000013D0
		private static LanguageData GetLanguageData(string languageId)
		{
			LanguageData languageData = LanguageData.GetLanguageData(languageId);
			if (languageData == null || !languageData.IsValid)
			{
				languageData = LanguageData.GetLanguageData("English");
				Debug.FailedAssert("Undefined language code: " + languageId, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Localization\\LocalizedTextManager.cs", "GetLanguageData", 225);
			}
			return languageData;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000321C File Offset: 0x0000141C
		private static XmlDocument LoadXmlFile(string path)
		{
			try
			{
				Debug.Print("opening " + path, 0, Debug.DebugColor.White, 17592186044416UL);
				XmlDocument xmlDocument = new XmlDocument();
				StreamReader streamReader = new StreamReader(path);
				string text = streamReader.ReadToEnd();
				xmlDocument.LoadXml(text);
				streamReader.Close();
				return xmlDocument;
			}
			catch
			{
				Debug.FailedAssert("Could not parse: " + path, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Localization\\LocalizedTextManager.cs", "LoadXmlFile", 245);
			}
			return null;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000032A0 File Offset: 0x000014A0
		internal static void LoadLanguage(string languageId)
		{
			LocalizedTextManager._gameTextDictionary.Clear();
			LanguageData languageData = LanguageData.GetLanguageData(languageId);
			if (languageData != null)
			{
				LocalizedTextManager.LoadLanguage(languageData);
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000032C8 File Offset: 0x000014C8
		private static void LoadLanguage(LanguageData language)
		{
			MBTextManager.ResetFunctions();
			string stringId = language.StringId;
			bool flag = stringId != "English";
			foreach (string text in language.XmlPaths)
			{
				XmlDocument xmlDocument = LocalizedTextManager.LoadXmlFile(text);
				if (xmlDocument != null)
				{
					for (XmlNode xmlNode = xmlDocument.ChildNodes[1].FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
					{
						if (xmlNode.Name == "strings" && xmlNode.HasChildNodes)
						{
							if (flag)
							{
								for (XmlNode xmlNode2 = xmlNode.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
								{
									if (xmlNode2.Name == "string" && xmlNode2.NodeType != XmlNodeType.Comment)
									{
										LocalizedTextManager.DeserializeStrings(xmlNode2, stringId);
									}
								}
							}
						}
						else if (xmlNode.Name == "functions" && xmlNode.HasChildNodes)
						{
							for (XmlNode xmlNode3 = xmlNode.FirstChild; xmlNode3 != null; xmlNode3 = xmlNode3.NextSibling)
							{
								if (xmlNode3.Name == "function" && xmlNode3.NodeType != XmlNodeType.Comment)
								{
									string value = xmlNode3.Attributes["functionName"].Value;
									string value2 = xmlNode3.Attributes["functionBody"].Value;
									MBTextManager.SetFunction(value, value2);
								}
							}
						}
					}
				}
			}
			Debug.Print("Loading localized text xml.", 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003474 File Offset: 0x00001674
		private static void DeserializeStrings(XmlNode node, string languageId)
		{
			if (node.Attributes == null)
			{
				throw new TWXmlLoadException("Node attributes are null!");
			}
			string value = node.Attributes["id"].Value;
			string value2 = node.Attributes["text"].Value;
			LocalizedTextManager._gameTextDictionary[value] = value2;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000034CC File Offset: 0x000016CC
		[CommandLineFunctionality.CommandLineArgumentFunction("change_language", "localization")]
		public static string ChangeLanguage(List<string> strings)
		{
			if (strings.Count != 1)
			{
				return "Format is \"localization.change_language [LanguageCode/LanguageName/ISOCode]\".";
			}
			string text = strings[0];
			int activeTextLanguageIndex = MBTextManager.GetActiveTextLanguageIndex();
			string text2 = null;
			foreach (string text3 in LocalizedTextManager.GetLanguageIds(true))
			{
				if (LocalizedTextManager.GetLanguageTitle(text3).Equals(text, StringComparison.OrdinalIgnoreCase) || LocalizedTextManager.GetSubtitleExtensionOfLanguage(text3).Contains(text))
				{
					text2 = text3;
					break;
				}
			}
			if (string.IsNullOrEmpty(text2))
			{
				return "cant find the language in current configuration.";
			}
			if (LocalizedTextManager.GetLanguageIndex(text2) == activeTextLanguageIndex)
			{
				return "Same language";
			}
			MBTextManager.ChangeLanguage(text2);
			return "New language is " + text2;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000358C File Offset: 0x0000178C
		[CommandLineFunctionality.CommandLineArgumentFunction("reload_texts", "localization")]
		public static string ReloadTexts(List<string> strings)
		{
			LocalizedTextManager.LoadLanguage(MBTextManager.ActiveTextLanguage);
			return "OK";
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000035A0 File Offset: 0x000017A0
		[CommandLineFunctionality.CommandLineArgumentFunction("check_for_errors", "localization")]
		public static string CheckValidity(List<string> strings)
		{
			if (File.Exists("faulty_translation_lines.txt"))
			{
				File.Delete("faulty_translation_lines.txt");
			}
			bool flag = false;
			foreach (string text in LocalizedTextManager.GetLanguageIds(false))
			{
				MBTextManager.ChangeLanguage(text);
				LocalizedTextManager.<CheckValidity>g__Write|23_0("Testing Language: " + MBTextManager.ActiveTextLanguage + "\n\n");
				foreach (KeyValuePair<string, string> keyValuePair in LocalizedTextManager._gameTextDictionary)
				{
					string key = keyValuePair.Key;
					string value = keyValuePair.Value;
					string text2;
					bool flag2 = LocalizedTextManager.CheckValidity(key, value, out text2);
					if (flag2)
					{
						LocalizedTextManager.<CheckValidity>g__Write|23_0(text2);
					}
					flag = flag2 || flag;
				}
				LocalizedTextManager.<CheckValidity>g__Write|23_0("\nTesting Language: " + MBTextManager.ActiveTextLanguage + "\n\n");
			}
			if (!flag)
			{
				return "No errors are found.";
			}
			return "Errors are written into 'faulty_translation_lines.txt' file in the binary folder.";
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000036B0 File Offset: 0x000018B0
		public static bool CheckValidity(string id, string text, out string errorLine)
		{
			errorLine = null;
			bool flag = false;
			int num = 0;
			int num2 = 0;
			foreach (char c in text)
			{
				if (c == '{')
				{
					num++;
				}
				else if (c == '}')
				{
					num2++;
				}
			}
			int num3 = 0;
			int num4 = 0;
			string text2 = text;
			for (;;)
			{
				int num5 = text2.IndexOf("{?");
				if (num5 == -1)
				{
					break;
				}
				num5 = MathF.Min(num5 + 1, text2.Length - 1);
				text2 = text2.Substring(num5);
				if (text2.Length > 2 && text2[1] != '}')
				{
					num3++;
				}
			}
			string text3 = text;
			for (;;)
			{
				int num6 = text3.IndexOf("{\\?}");
				if (num6 == -1)
				{
					break;
				}
				num4++;
				num6 = MathF.Min(num6 + 1, text.Length - 1);
				text3 = text3.Substring(num6);
			}
			if (num != num2)
			{
				errorLine = string.Format("{0} | {1}\n", id, text);
				flag = true;
			}
			else if (num3 != num4)
			{
				errorLine = string.Format("{0} | {1}\n", id, text);
				flag = true;
			}
			else if (!flag)
			{
				try
				{
					MBTextManager.ProcessTextToString(new TextObject("{=" + id + "}" + LocalizedTextManager.GetTranslatedText(MBTextManager.ActiveTextLanguage, id), null), true);
				}
				catch
				{
					errorLine = string.Format("{0} | {1}\n", id, text);
				}
			}
			return flag;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003820 File Offset: 0x00001A20
		[CompilerGenerated]
		internal static void <CheckValidity>g__Write|23_0(string s)
		{
			File.AppendAllText("faulty_translation_lines.txt", s, Encoding.Unicode);
		}

		// Token: 0x04000011 RID: 17
		public const string LanguageDataFileName = "language_data";

		// Token: 0x04000012 RID: 18
		public const string DefaultEnglishLanguageId = "English";

		// Token: 0x04000013 RID: 19
		private static readonly Dictionary<string, string> _gameTextDictionary = new Dictionary<string, string>();
	}
}
