using System;
using System.Collections.Generic;

namespace TaleWorlds.Library.CodeGeneration
{
	// Token: 0x020000BF RID: 191
	public class CommentSection
	{
		// Token: 0x0600071B RID: 1819 RVA: 0x00017FA0 File Offset: 0x000161A0
		public CommentSection()
		{
			this._lines = new List<string>();
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00017FB3 File Offset: 0x000161B3
		public void AddCommentLine(string line)
		{
			this._lines.Add(line);
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x00017FC4 File Offset: 0x000161C4
		public void GenerateInto(CodeGenerationFile codeGenerationFile)
		{
			foreach (string text in this._lines)
			{
				codeGenerationFile.AddLine("//" + text);
			}
		}

		// Token: 0x04000234 RID: 564
		private List<string> _lines;
	}
}
