using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000048 RID: 72
	internal class ByteBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002BB RID: 699 RVA: 0x0000D201 File Offset: 0x0000B401
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			writer.WriteByte((byte)value);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000D20F File Offset: 0x0000B40F
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadByte();
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000D21C File Offset: 0x0000B41C
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 1;
		}
	}
}
