using System;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000064 RID: 100
	internal class InterfaceDefinition : TypeDefinitionBase
	{
		// Token: 0x0600035A RID: 858 RVA: 0x0000EDA8 File Offset: 0x0000CFA8
		public InterfaceDefinition(Type type, SaveId saveId)
			: base(type, saveId)
		{
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000EDB2 File Offset: 0x0000CFB2
		public InterfaceDefinition(Type type, int saveId)
			: base(type, new TypeSaveId(saveId))
		{
		}
	}
}
