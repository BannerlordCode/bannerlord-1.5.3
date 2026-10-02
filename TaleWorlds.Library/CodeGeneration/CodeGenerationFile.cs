using System;
using System.Collections.Generic;
using System.Text;

namespace TaleWorlds.Library.CodeGeneration
{
	// Token: 0x020000BE RID: 190
	public class CodeGenerationFile
	{
		// Token: 0x06000718 RID: 1816 RVA: 0x00017E4C File Offset: 0x0001604C
		public CodeGenerationFile(List<string> usingDefinitions = null)
		{
			this._lines = new List<string>();
			if (usingDefinitions != null && usingDefinitions.Count > 0)
			{
				foreach (string text in usingDefinitions)
				{
					this.AddLine("using " + text + ";");
				}
			}
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x00017EC8 File Offset: 0x000160C8
		public void AddLine(string line)
		{
			this._lines.Add(line);
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00017ED8 File Offset: 0x000160D8
		public string GenerateText()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string text in this._lines)
			{
				if (text == "}" || text == "};")
				{
					num--;
				}
				string text2 = "";
				for (int i = 0; i < num; i++)
				{
					text2 += "\t";
				}
				text2 = text2 + text + "\n";
				if (text == "{")
				{
					num++;
				}
				stringBuilder.Append(text2);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000233 RID: 563
		private List<string> _lines;
	}
}
