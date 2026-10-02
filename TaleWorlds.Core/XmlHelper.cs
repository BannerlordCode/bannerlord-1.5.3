using System;
using System.Globalization;
using System.Xml;

namespace TaleWorlds.Core
{
	// Token: 0x020000E2 RID: 226
	public static class XmlHelper
	{
		// Token: 0x06000B88 RID: 2952 RVA: 0x0002555C File Offset: 0x0002375C
		public static int ReadInt(XmlNode node, string str)
		{
			XmlAttribute xmlAttribute = node.Attributes[str];
			if (xmlAttribute == null)
			{
				return 0;
			}
			return int.Parse(xmlAttribute.Value);
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x00025588 File Offset: 0x00023788
		public static void ReadInt(ref int val, XmlNode node, string str)
		{
			XmlAttribute xmlAttribute = node.Attributes[str];
			if (xmlAttribute != null)
			{
				val = int.Parse(xmlAttribute.Value);
			}
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x000255B4 File Offset: 0x000237B4
		public static float ReadFloat(XmlNode node, string str, float defaultValue = 0f)
		{
			XmlAttribute xmlAttribute = node.Attributes[str];
			if (xmlAttribute == null)
			{
				return defaultValue;
			}
			return float.Parse(xmlAttribute.Value);
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x000255E0 File Offset: 0x000237E0
		public static string ReadString(XmlNode node, string str)
		{
			XmlAttribute xmlAttribute = node.Attributes[str];
			if (xmlAttribute == null)
			{
				return "";
			}
			return xmlAttribute.Value;
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x0002560C File Offset: 0x0002380C
		public static void ReadHexCode(ref uint val, XmlNode node, string str)
		{
			XmlAttribute xmlAttribute = node.Attributes[str];
			if (xmlAttribute != null)
			{
				string text = xmlAttribute.Value;
				text = text.Substring(2);
				val = uint.Parse(text, NumberStyles.HexNumber);
			}
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x00025648 File Offset: 0x00023848
		public static bool ReadBool(XmlNode node, string str)
		{
			XmlAttribute xmlAttribute = node.Attributes[str];
			return xmlAttribute != null && Convert.ToBoolean(xmlAttribute.InnerText);
		}
	}
}
