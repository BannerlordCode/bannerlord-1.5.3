using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200004D RID: 77
	internal class UlongBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002CF RID: 719 RVA: 0x0000D2BF File Offset: 0x0000B4BF
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			writer.WriteULong((ulong)value);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000D2CD File Offset: 0x0000B4CD
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadULong();
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000D2DA File Offset: 0x0000B4DA
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 8;
		}
	}
}
