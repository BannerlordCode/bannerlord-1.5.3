using System;
using System.IO;
using System.Xml;
using TaleWorlds.Library;

namespace Helpers
{
	// Token: 0x02000023 RID: 35
	public static class MiscHelper
	{
		// Token: 0x0600011E RID: 286 RVA: 0x0000E4AC File Offset: 0x0000C6AC
		public static XmlDocument LoadXmlFile(string path)
		{
			Debug.Print("opening " + path, 0, Debug.DebugColor.White, 17592186044416UL);
			XmlDocument xmlDocument = new XmlDocument();
			StreamReader streamReader = new StreamReader(path);
			string text = streamReader.ReadToEnd();
			xmlDocument.LoadXml(text);
			streamReader.Close();
			return xmlDocument;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000E4F8 File Offset: 0x0000C6F8
		public static string GenerateCampaignId(int length)
		{
			Random random = new Random((int)(DateTime.Now.Ticks & 65535L));
			char[] array = new char[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"[random.Next("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".Length)];
			}
			string text = new string(array);
			Debug.Print("Campaign id: " + text, 1, Debug.DebugColor.Green, 17592186044416UL);
			return text;
		}
	}
}
