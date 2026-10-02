using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000044 RID: 68
	internal class IntBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002AB RID: 683 RVA: 0x0000D169 File Offset: 0x0000B369
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000D177 File Offset: 0x0000B377
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadInt();
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000D184 File Offset: 0x0000B384
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 4;
		}
	}
}
