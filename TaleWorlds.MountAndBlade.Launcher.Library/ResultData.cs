using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace TaleWorlds.MountAndBlade.Launcher.Library
{
	// Token: 0x02000012 RID: 18
	public class ResultData
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600009D RID: 157 RVA: 0x000045EE File Offset: 0x000027EE
		// (set) Token: 0x0600009E RID: 158 RVA: 0x000045F6 File Offset: 0x000027F6
		public string Errors { get; set; } = "";

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600009F RID: 159 RVA: 0x000045FF File Offset: 0x000027FF
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x00004607 File Offset: 0x00002807
		public List<DLLResult> DLLs { get; set; } = new List<DLLResult>();

		// Token: 0x060000A1 RID: 161 RVA: 0x00004610 File Offset: 0x00002810
		public void AddDLLResult(string dllName, bool isSafe, string information)
		{
			this.DLLs.Add(new DLLResult(dllName, isSafe, information));
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004628 File Offset: 0x00002828
		public override string ToString()
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ResultData));
			string text;
			using (StringWriter stringWriter = new StringWriter())
			{
				xmlSerializer.Serialize(stringWriter, this);
				text = stringWriter.ToString();
			}
			return text;
		}
	}
}
