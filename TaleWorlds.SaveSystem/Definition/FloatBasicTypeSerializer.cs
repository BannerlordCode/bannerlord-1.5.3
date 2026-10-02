using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200004A RID: 74
	internal class FloatBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002C3 RID: 707 RVA: 0x0000D24D File Offset: 0x0000B44D
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			writer.WriteFloat((float)value);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000D25B File Offset: 0x0000B45B
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadFloat();
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000D268 File Offset: 0x0000B468
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 4;
		}
	}
}
