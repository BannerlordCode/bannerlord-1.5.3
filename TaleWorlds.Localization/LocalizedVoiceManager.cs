using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.Localization
{
	// Token: 0x02000007 RID: 7
	public static class LocalizedVoiceManager
	{
		// Token: 0x06000064 RID: 100 RVA: 0x00003834 File Offset: 0x00001A34
		public static VoiceObject GetLocalizedVoice(string id)
		{
			VoiceObject voiceObject;
			if (LocalizedVoiceManager._voiceObjectDictionary.TryGetValue(id, out voiceObject))
			{
				return voiceObject;
			}
			Debug.Print("Voice object for text id is not found: " + id, 0, Debug.DebugColor.Green, 17592186044416UL);
			return null;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003870 File Offset: 0x00001A70
		public static List<string> GetVoiceLanguageIds()
		{
			List<string> list = new List<string>();
			foreach (LanguageData languageData in LanguageData.All)
			{
				if (languageData != null && languageData.IsValid && languageData.VoiceXmlPathsAndModulePaths.Count > 0)
				{
					list.Add(languageData.StringId);
				}
			}
			return list;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000038E8 File Offset: 0x00001AE8
		internal static void LoadLanguage(string languageId)
		{
			LocalizedVoiceManager._voiceObjectDictionary.Clear();
			LanguageData languageData = LanguageData.GetLanguageData(languageId);
			if (languageData != null)
			{
				LocalizedVoiceManager.LoadLanguage(languageData);
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003910 File Offset: 0x00001B10
		private static XmlDocument LoadXmlFile(string xmlPath)
		{
			try
			{
				Debug.Print("opening " + xmlPath, 0, Debug.DebugColor.White, 17592186044416UL);
				XmlDocument xmlDocument = new XmlDocument();
				StreamReader streamReader = new StreamReader(xmlPath);
				string text = streamReader.ReadToEnd();
				xmlDocument.LoadXml(text);
				streamReader.Close();
				return xmlDocument;
			}
			catch
			{
				Debug.FailedAssert("Could not parse: " + xmlPath, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Localization\\LocalizedVoiceManager.cs", "LoadXmlFile", 70);
			}
			return null;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003990 File Offset: 0x00001B90
		private static void LoadLanguage(LanguageData language)
		{
			foreach (KeyValuePair<string, string> keyValuePair in language.VoiceXmlPathsAndModulePaths)
			{
				XmlDocument xmlDocument = LocalizedVoiceManager.LoadXmlFile(keyValuePair.Key);
				if (xmlDocument != null)
				{
					XmlNode xmlNode = null;
					foreach (object obj in xmlDocument.DocumentElement.ChildNodes)
					{
						XmlNode xmlNode2 = (XmlNode)obj;
						if (xmlNode2.Name == "VoiceOvers")
						{
							xmlNode = xmlNode2;
							break;
						}
					}
					foreach (object obj2 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode3 = (XmlNode)obj2;
						if (xmlNode3.Name == "VoiceOver")
						{
							string innerText = xmlNode3.Attributes["id"].InnerText;
							if (LocalizedVoiceManager._voiceObjectDictionary.ContainsKey(innerText))
							{
								LocalizedVoiceManager._voiceObjectDictionary[innerText].AddVoicePaths(xmlNode3, keyValuePair.Value);
							}
							else
							{
								VoiceObject voiceObject = VoiceObject.Deserialize(xmlNode3, keyValuePair.Value);
								LocalizedVoiceManager._voiceObjectDictionary.Add(innerText, voiceObject);
							}
						}
					}
				}
			}
		}

		// Token: 0x04000014 RID: 20
		private static readonly Dictionary<string, VoiceObject> _voiceObjectDictionary = new Dictionary<string, VoiceObject>();
	}
}
