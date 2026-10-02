using System;
using System.Collections.Generic;

namespace TaleWorlds.Library.CodeGeneration
{
	// Token: 0x020000C5 RID: 197
	public class NamespaceCode
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x000184CC File Offset: 0x000166CC
		// (set) Token: 0x06000743 RID: 1859 RVA: 0x000184D4 File Offset: 0x000166D4
		public string Name { get; set; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x000184DD File Offset: 0x000166DD
		// (set) Token: 0x06000745 RID: 1861 RVA: 0x000184E5 File Offset: 0x000166E5
		public List<ClassCode> Classes { get; private set; }

		// Token: 0x06000746 RID: 1862 RVA: 0x000184EE File Offset: 0x000166EE
		public NamespaceCode()
		{
			this.Classes = new List<ClassCode>();
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00018504 File Offset: 0x00016704
		public void GenerateInto(CodeGenerationFile codeGenerationFile)
		{
			codeGenerationFile.AddLine("namespace " + this.Name);
			codeGenerationFile.AddLine("{");
			foreach (ClassCode classCode in this.Classes)
			{
				classCode.GenerateInto(codeGenerationFile);
				codeGenerationFile.AddLine("");
			}
			codeGenerationFile.AddLine("}");
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0001858C File Offset: 0x0001678C
		public void AddClass(ClassCode clasCode)
		{
			this.Classes.Add(clasCode);
		}
	}
}
