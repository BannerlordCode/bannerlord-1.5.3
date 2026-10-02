using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200004B RID: 75
	internal class DoubleBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002C7 RID: 711 RVA: 0x0000D273 File Offset: 0x0000B473
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			writer.WriteDouble((double)value);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000D281 File Offset: 0x0000B481
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadDouble();
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000D28E File Offset: 0x0000B48E
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 8;
		}
	}
}
