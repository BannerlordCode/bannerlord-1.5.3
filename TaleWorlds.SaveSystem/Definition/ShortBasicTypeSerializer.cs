using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000046 RID: 70
	internal class ShortBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002B3 RID: 691 RVA: 0x0000D1B5 File Offset: 0x0000B3B5
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			writer.WriteShort((short)value);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000D1C3 File Offset: 0x0000B3C3
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadShort();
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000D1D0 File Offset: 0x0000B3D0
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 2;
		}
	}
}
