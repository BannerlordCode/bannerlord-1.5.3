using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000057 RID: 87
	internal class BoolBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002F7 RID: 759 RVA: 0x0000D634 File Offset: 0x0000B834
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			writer.WriteBool((bool)value);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000D642 File Offset: 0x0000B842
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadBool();
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000D64F File Offset: 0x0000B84F
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 1;
		}
	}
}
