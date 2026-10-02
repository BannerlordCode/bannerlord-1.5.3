using System;
using System.Reflection;
using TaleWorlds.SaveSystem.Resolvers;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000060 RID: 96
	internal class EnumDefinition : TypeDefinitionBase
	{
		// Token: 0x06000343 RID: 835 RVA: 0x0000EAF4 File Offset: 0x0000CCF4
		public EnumDefinition(Type type, SaveId saveId, IEnumResolver resolver)
			: base(type, saveId)
		{
			this.Resolver = resolver;
			this.HasFlags = type.GetCustomAttribute<FlagsAttribute>() != null;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000EB14 File Offset: 0x0000CD14
		public EnumDefinition(Type type, int saveId, IEnumResolver resolver)
			: this(type, new TypeSaveId(saveId), resolver)
		{
		}

		// Token: 0x040000F8 RID: 248
		public readonly IEnumResolver Resolver;

		// Token: 0x040000F9 RID: 249
		public readonly bool HasFlags;
	}
}
