using System;

namespace TaleWorlds.Library.CodeGeneration
{
	// Token: 0x020000C6 RID: 198
	public class VariableCode
	{
		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x0001859A File Offset: 0x0001679A
		// (set) Token: 0x0600074A RID: 1866 RVA: 0x000185A2 File Offset: 0x000167A2
		public string Name { get; set; }

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600074B RID: 1867 RVA: 0x000185AB File Offset: 0x000167AB
		// (set) Token: 0x0600074C RID: 1868 RVA: 0x000185B3 File Offset: 0x000167B3
		public string Type { get; set; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600074D RID: 1869 RVA: 0x000185BC File Offset: 0x000167BC
		// (set) Token: 0x0600074E RID: 1870 RVA: 0x000185C4 File Offset: 0x000167C4
		public bool IsStatic { get; set; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600074F RID: 1871 RVA: 0x000185CD File Offset: 0x000167CD
		// (set) Token: 0x06000750 RID: 1872 RVA: 0x000185D5 File Offset: 0x000167D5
		public VariableCodeAccessModifier AccessModifier { get; set; }

		// Token: 0x06000751 RID: 1873 RVA: 0x000185DE File Offset: 0x000167DE
		public VariableCode()
		{
			this.Type = "System.Object";
			this.Name = "Unnamed variable";
			this.IsStatic = false;
			this.AccessModifier = VariableCodeAccessModifier.Private;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0001860C File Offset: 0x0001680C
		public string GenerateLine()
		{
			string text = "";
			if (this.AccessModifier == VariableCodeAccessModifier.Public)
			{
				text += "public ";
			}
			else if (this.AccessModifier == VariableCodeAccessModifier.Protected)
			{
				text += "protected ";
			}
			else if (this.AccessModifier == VariableCodeAccessModifier.Private)
			{
				text += "private ";
			}
			else if (this.AccessModifier == VariableCodeAccessModifier.Internal)
			{
				text += "internal ";
			}
			if (this.IsStatic)
			{
				text += "static ";
			}
			return string.Concat(new string[] { text, this.Type, " ", this.Name, ";" });
		}
	}
}
