using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000068 RID: 104
	public class SaveCodeGenerationContext
	{
		// Token: 0x06000379 RID: 889 RVA: 0x0000F1A1 File Offset: 0x0000D3A1
		public SaveCodeGenerationContext(DefinitionContext definitionContext)
		{
			this._definitionContext = definitionContext;
			this._assemblies = new Dictionary<Assembly, SaveCodeGenerationContextAssembly>();
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000F1BC File Offset: 0x0000D3BC
		public void AddAssembly(Assembly assembly, string defaultNamespace, string location, string fileName)
		{
			SaveCodeGenerationContextAssembly saveCodeGenerationContextAssembly = new SaveCodeGenerationContextAssembly(this._definitionContext, assembly, defaultNamespace, location, fileName);
			this._assemblies.Add(assembly, saveCodeGenerationContextAssembly);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000F1E8 File Offset: 0x0000D3E8
		internal SaveCodeGenerationContextAssembly FindAssemblyInformation(Assembly assembly)
		{
			SaveCodeGenerationContextAssembly saveCodeGenerationContextAssembly;
			this._assemblies.TryGetValue(assembly, out saveCodeGenerationContextAssembly);
			return saveCodeGenerationContextAssembly;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000F208 File Offset: 0x0000D408
		internal void FillFiles()
		{
			List<Tuple<string, string>> list = new List<Tuple<string, string>>();
			foreach (SaveCodeGenerationContextAssembly saveCodeGenerationContextAssembly in this._assemblies.Values)
			{
				saveCodeGenerationContextAssembly.Generate();
				string text = saveCodeGenerationContextAssembly.GenerateText();
				list.Add(new Tuple<string, string>(saveCodeGenerationContextAssembly.Location + saveCodeGenerationContextAssembly.FileName, text));
			}
			foreach (Tuple<string, string> tuple in list)
			{
				File.WriteAllText(tuple.Item1, tuple.Item2, Encoding.UTF8);
			}
		}

		// Token: 0x04000109 RID: 265
		private Dictionary<Assembly, SaveCodeGenerationContextAssembly> _assemblies;

		// Token: 0x0400010A RID: 266
		private DefinitionContext _definitionContext;
	}
}
