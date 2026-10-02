using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.Localization
{
	// Token: 0x0200000B RID: 11
	public class VoiceObject
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000091 RID: 145 RVA: 0x000044BE File Offset: 0x000026BE
		public MBReadOnlyList<string> VoicePaths
		{
			get
			{
				return this._voicePaths;
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000044C6 File Offset: 0x000026C6
		private VoiceObject()
		{
			this._voicePaths = new MBList<string>();
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000044D9 File Offset: 0x000026D9
		private void AddVoicePath(string voicePath)
		{
			this._voicePaths.Add(voicePath);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000044E8 File Offset: 0x000026E8
		public void AddVoicePaths(XmlNode node, string modulePath)
		{
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "Voice")
				{
					string text = modulePath + "/" + xmlNode.Attributes["path"].InnerText;
					this.AddVoicePath(text);
				}
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004574 File Offset: 0x00002774
		public static VoiceObject Deserialize(XmlNode node, string modulePath)
		{
			VoiceObject voiceObject = new VoiceObject();
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "Voice")
				{
					string text = modulePath + "/" + xmlNode.Attributes["path"].InnerText;
					voiceObject.AddVoicePath(text);
				}
			}
			return voiceObject;
		}

		// Token: 0x0400002A RID: 42
		private readonly MBList<string> _voicePaths;
	}
}
