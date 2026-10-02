using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200004E RID: 78
	internal class Vec2BasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002D3 RID: 723 RVA: 0x0000D2E8 File Offset: 0x0000B4E8
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			Vec2 vec = (Vec2)value;
			writer.WriteVec2(vec);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000D303 File Offset: 0x0000B503
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadVec2();
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000D310 File Offset: 0x0000B510
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 8;
		}
	}
}
