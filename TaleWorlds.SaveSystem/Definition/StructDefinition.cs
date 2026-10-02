using System;
using TaleWorlds.SaveSystem.Resolvers;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200006B RID: 107
	internal class StructDefinition : TypeDefinition
	{
		// Token: 0x060003A2 RID: 930 RVA: 0x00010FE3 File Offset: 0x0000F1E3
		public StructDefinition(Type type, int saveId)
			: this(type, saveId, null)
		{
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00010FEE File Offset: 0x0000F1EE
		public StructDefinition(Type type, int saveId, IObjectResolver objectResolver)
			: base(type, saveId, objectResolver)
		{
		}
	}
}
