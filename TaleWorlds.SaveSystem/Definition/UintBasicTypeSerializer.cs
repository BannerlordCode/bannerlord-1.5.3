using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000045 RID: 69
	internal class UintBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002AF RID: 687 RVA: 0x0000D18F File Offset: 0x0000B38F
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			writer.WriteUInt((uint)value);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000D19D File Offset: 0x0000B39D
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadUInt();
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000D1AA File Offset: 0x0000B3AA
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 4;
		}
	}
}
