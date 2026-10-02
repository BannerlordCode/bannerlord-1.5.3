using System;
using System.Collections.Generic;

namespace TaleWorlds.Library.CodeGeneration
{
	// Token: 0x020000C2 RID: 194
	public class CodeBlock
	{
		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x00018452 File Offset: 0x00016652
		public List<string> Lines
		{
			get
			{
				return this._lines;
			}
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0001845A File Offset: 0x0001665A
		public CodeBlock()
		{
			this._lines = new List<string>();
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x0001846D File Offset: 0x0001666D
		public void AddLine(string line)
		{
			this._lines.Add(line);
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x0001847C File Offset: 0x0001667C
		public void AddLines(IEnumerable<string> lines)
		{
			foreach (string text in lines)
			{
				this._lines.Add(text);
			}
		}

		// Token: 0x04000243 RID: 579
		private List<string> _lines;
	}
}
