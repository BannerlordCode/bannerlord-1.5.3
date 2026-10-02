using System;
using System.Collections.Generic;

namespace TaleWorlds.Library.CodeGeneration
{
	// Token: 0x020000BD RID: 189
	public class CodeGenerationContext
	{
		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x00017D4E File Offset: 0x00015F4E
		// (set) Token: 0x06000714 RID: 1812 RVA: 0x00017D56 File Offset: 0x00015F56
		public List<NamespaceCode> Namespaces { get; private set; }

		// Token: 0x06000715 RID: 1813 RVA: 0x00017D5F File Offset: 0x00015F5F
		public CodeGenerationContext()
		{
			this.Namespaces = new List<NamespaceCode>();
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x00017D74 File Offset: 0x00015F74
		public NamespaceCode FindOrCreateNamespace(string name)
		{
			foreach (NamespaceCode namespaceCode in this.Namespaces)
			{
				if (namespaceCode.Name == name)
				{
					return namespaceCode;
				}
			}
			NamespaceCode namespaceCode2 = new NamespaceCode();
			namespaceCode2.Name = name;
			this.Namespaces.Add(namespaceCode2);
			return namespaceCode2;
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x00017DF0 File Offset: 0x00015FF0
		public void GenerateInto(CodeGenerationFile codeGenerationFile)
		{
			foreach (NamespaceCode namespaceCode in this.Namespaces)
			{
				namespaceCode.GenerateInto(codeGenerationFile);
				codeGenerationFile.AddLine("");
			}
		}
	}
}
