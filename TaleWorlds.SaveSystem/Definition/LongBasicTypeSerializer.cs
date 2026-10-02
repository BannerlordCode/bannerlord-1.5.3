using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200004C RID: 76
	internal class LongBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002CB RID: 715 RVA: 0x0000D299 File Offset: 0x0000B499
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			writer.WriteLong((long)value);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000D2A7 File Offset: 0x0000B4A7
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadLong();
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000D2B4 File Offset: 0x0000B4B4
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 8;
		}
	}
}
