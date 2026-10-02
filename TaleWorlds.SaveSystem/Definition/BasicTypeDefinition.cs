using System;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000042 RID: 66
	internal class BasicTypeDefinition : TypeDefinitionBase
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000D142 File Offset: 0x0000B342
		// (set) Token: 0x060002A6 RID: 678 RVA: 0x0000D14A File Offset: 0x0000B34A
		public IBasicTypeSerializer Serializer { get; private set; }

		// Token: 0x060002A7 RID: 679 RVA: 0x0000D153 File Offset: 0x0000B353
		public BasicTypeDefinition(Type type, int saveId, IBasicTypeSerializer serializer)
			: base(type, new TypeSaveId(saveId))
		{
			this.Serializer = serializer;
		}
	}
}
