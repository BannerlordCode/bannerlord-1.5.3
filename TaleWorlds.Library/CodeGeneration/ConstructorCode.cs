using System;
using System.Collections.Generic;

namespace TaleWorlds.Library.CodeGeneration
{
	// Token: 0x020000C0 RID: 192
	public class ConstructorCode
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x00018024 File Offset: 0x00016224
		// (set) Token: 0x0600071F RID: 1823 RVA: 0x0001802C File Offset: 0x0001622C
		public string Name { get; set; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x00018035 File Offset: 0x00016235
		// (set) Token: 0x06000721 RID: 1825 RVA: 0x0001803D File Offset: 0x0001623D
		public string MethodSignature { get; set; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x00018046 File Offset: 0x00016246
		// (set) Token: 0x06000723 RID: 1827 RVA: 0x0001804E File Offset: 0x0001624E
		public string BaseCall { get; set; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x00018057 File Offset: 0x00016257
		// (set) Token: 0x06000725 RID: 1829 RVA: 0x0001805F File Offset: 0x0001625F
		public bool IsStatic { get; set; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x00018068 File Offset: 0x00016268
		// (set) Token: 0x06000727 RID: 1831 RVA: 0x00018070 File Offset: 0x00016270
		public MethodCodeAccessModifier AccessModifier { get; set; }

		// Token: 0x06000728 RID: 1832 RVA: 0x00018079 File Offset: 0x00016279
		public ConstructorCode()
		{
			this.Name = "UnassignedConstructorName";
			this.MethodSignature = "()";
			this.BaseCall = "";
			this._lines = new List<string>();
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x000180B0 File Offset: 0x000162B0
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
			text = text + this.Name + this.MethodSignature;
			if (!string.IsNullOrEmpty(this.BaseCall))
			{
				text = text + " : base" + this.BaseCall;
			}
			codeGenerationFile.AddLine(text);
			codeGenerationFile.AddLine("{");
			foreach (string text2 in this._lines)
			{
				codeGenerationFile.AddLine(text2);
			}
			codeGenerationFile.AddLine("}");
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x000181C8 File Offset: 0x000163C8
		public void AddLine(string line)
		{
			this._lines.Add(line);
		}

		// Token: 0x0400023A RID: 570
		private List<string> _lines;
	}
}
