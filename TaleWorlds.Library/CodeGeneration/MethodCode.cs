using System;
using System.Collections.Generic;

namespace TaleWorlds.Library.CodeGeneration
{
	// Token: 0x020000C1 RID: 193
	public class MethodCode
	{
		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x000181D6 File Offset: 0x000163D6
		// (set) Token: 0x0600072C RID: 1836 RVA: 0x000181DE File Offset: 0x000163DE
		public string Comment { get; set; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x000181E7 File Offset: 0x000163E7
		// (set) Token: 0x0600072E RID: 1838 RVA: 0x000181EF File Offset: 0x000163EF
		public string Name { get; set; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x000181F8 File Offset: 0x000163F8
		// (set) Token: 0x06000730 RID: 1840 RVA: 0x00018200 File Offset: 0x00016400
		public string MethodSignature { get; set; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x00018209 File Offset: 0x00016409
		// (set) Token: 0x06000732 RID: 1842 RVA: 0x00018211 File Offset: 0x00016411
		public string ReturnParameter { get; set; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x0001821A File Offset: 0x0001641A
		// (set) Token: 0x06000734 RID: 1844 RVA: 0x00018222 File Offset: 0x00016422
		public bool IsStatic { get; set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x0001822B File Offset: 0x0001642B
		// (set) Token: 0x06000736 RID: 1846 RVA: 0x00018233 File Offset: 0x00016433
		public MethodCodeAccessModifier AccessModifier { get; set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x0001823C File Offset: 0x0001643C
		// (set) Token: 0x06000738 RID: 1848 RVA: 0x00018244 File Offset: 0x00016444
		public MethodCodePolymorphismInfo PolymorphismInfo { get; set; }

		// Token: 0x06000739 RID: 1849 RVA: 0x0001824D File Offset: 0x0001644D
		public MethodCode()
		{
			this.Name = "UnnamedMethod";
			this.MethodSignature = "()";
			this.PolymorphismInfo = MethodCodePolymorphismInfo.None;
			this.ReturnParameter = "void";
			this._lines = new List<string>();
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00018288 File Offset: 0x00016488
		public void GenerateInto(CodeGenerationFile codeGenerationFile)
		{
			string text = "";
			if (this.AccessModifier == MethodCodeAccessModifier.Public)
			{
				text += "public ";
			}
			else if (this.AccessModifier == MethodCodeAccessModifier.Protected)
			{
				text += "protected ";
			}
			else if (this.AccessModifier == MethodCodeAccessModifier.Private)
			{
				text += "private ";
			}
			else if (this.AccessModifier == MethodCodeAccessModifier.Internal)
			{
				text += "internal ";
			}
			if (this.IsStatic)
			{
				text += "static ";
			}
			if (this.PolymorphismInfo == MethodCodePolymorphismInfo.Virtual)
			{
				text += "virtual ";
			}
			else if (this.PolymorphismInfo == MethodCodePolymorphismInfo.Override)
			{
				text += "override ";
			}
			text = string.Concat(new string[] { text, this.ReturnParameter, " ", this.Name, this.MethodSignature });
			if (!string.IsNullOrEmpty(this.Comment))
			{
				codeGenerationFile.AddLine(this.Comment);
			}
			codeGenerationFile.AddLine(text);
			codeGenerationFile.AddLine("{");
			foreach (string text2 in this._lines)
			{
				codeGenerationFile.AddLine(text2);
			}
			codeGenerationFile.AddLine("}");
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x000183E4 File Offset: 0x000165E4
		public void AddLine(string line)
		{
			this._lines.Add(line);
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x000183F4 File Offset: 0x000165F4
		public void AddLines(IEnumerable<string> lines)
		{
			foreach (string text in lines)
			{
				this._lines.Add(text);
			}
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x00018444 File Offset: 0x00016644
		public void AddCodeBlock(CodeBlock codeBlock)
		{
			this.AddLines(codeBlock.Lines);
		}

		// Token: 0x04000242 RID: 578
		private List<string> _lines;
	}
}
