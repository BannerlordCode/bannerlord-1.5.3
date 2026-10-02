using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000049 RID: 73
	internal class SbyteBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002BF RID: 703 RVA: 0x0000D227 File Offset: 0x0000B427
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			writer.WriteSByte((sbyte)value);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000D235 File Offset: 0x0000B435
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadSByte();
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000D242 File Offset: 0x0000B442
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 1;
		}
	}
}
