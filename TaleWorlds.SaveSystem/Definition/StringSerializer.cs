using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000058 RID: 88
	internal class StringSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002FB RID: 763 RVA: 0x0000D65A File Offset: 0x0000B85A
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000D65C File Offset: 0x0000B85C
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return null;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000D65F File Offset: 0x0000B85F
		public int GetSizeInBytes()
		{
			return 0;
		}
	}
}
