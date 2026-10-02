using System;
using System.Threading.Tasks;
using System.Xml;

namespace TaleWorlds.Library
{
	// Token: 0x02000032 RID: 50
	public static class FileHelperExtensions
	{
		// Token: 0x060001B4 RID: 436 RVA: 0x00006F34 File Offset: 0x00005134
		public static void Load(this XmlDocument document, PlatformFilePath path)
		{
			string fileContentString = FileHelper.GetFileContentString(path);
			if (!string.IsNullOrEmpty(fileContentString))
			{
				document.LoadXml(fileContentString);
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00006F58 File Offset: 0x00005158
		public static async Task LoadAsync(this XmlDocument document, PlatformFilePath path)
		{
			string text = await FileHelper.GetFileContentStringAsync(path);
			if (!string.IsNullOrEmpty(text))
			{
				document.LoadXml(text);
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00006FA8 File Offset: 0x000051A8
		public static void Save(this XmlDocument document, PlatformFilePath path)
		{
			string outerXml = document.OuterXml;
			FileHelper.SaveFileString(path, outerXml);
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00006FC4 File Offset: 0x000051C4
		public static async Task SaveAsync(this XmlDocument document, PlatformFilePath path)
		{
			string outerXml = document.OuterXml;
			await FileHelper.SaveFileStringAsync(path, outerXml);
		}
	}
}
