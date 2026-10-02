using System;
using System.Collections.Generic;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x0200000D RID: 13
	public class RichTextTag
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00004AEE File Offset: 0x00002CEE
		// (set) Token: 0x06000086 RID: 134 RVA: 0x00004AF6 File Offset: 0x00002CF6
		public string Name { get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00004AFF File Offset: 0x00002CFF
		// (set) Token: 0x06000088 RID: 136 RVA: 0x00004B07 File Offset: 0x00002D07
		public RichTextTagType Type { get; set; }

		// Token: 0x06000089 RID: 137 RVA: 0x00004B10 File Offset: 0x00002D10
		public RichTextTag(string name)
		{
			this.Name = name;
			this._attributes = new Dictionary<string, string>();
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004B2A File Offset: 0x00002D2A
		public void AddAtrribute(string key, string value)
		{
			this._attributes.Add(key, value);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00004B39 File Offset: 0x00002D39
		public string GetAttribute(string key)
		{
			if (this._attributes.ContainsKey(key))
			{
				return this._attributes[key];
			}
			return "";
		}

		// Token: 0x04000054 RID: 84
		private Dictionary<string, string> _attributes;
	}
}
