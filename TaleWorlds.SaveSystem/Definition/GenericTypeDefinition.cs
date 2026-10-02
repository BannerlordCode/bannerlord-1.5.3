using System;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000063 RID: 99
	internal class GenericTypeDefinition : TypeDefinition
	{
		// Token: 0x06000359 RID: 857 RVA: 0x0000ED9D File Offset: 0x0000CF9D
		public GenericTypeDefinition(Type type, GenericSaveId saveId)
			: base(type, saveId, null)
		{
		}
	}
}
