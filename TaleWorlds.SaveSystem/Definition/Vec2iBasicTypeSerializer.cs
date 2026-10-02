using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200004F RID: 79
	internal class Vec2iBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002D7 RID: 727 RVA: 0x0000D31C File Offset: 0x0000B51C
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			Vec2i vec2i = (Vec2i)value;
			writer.WriteFloat((float)vec2i.Item1);
			writer.WriteFloat((float)vec2i.Item2);
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000D34C File Offset: 0x0000B54C
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			int num = reader.ReadInt();
			int num2 = reader.ReadInt();
			return new Vec2i(num, num2);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000D371 File Offset: 0x0000B571
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 8;
		}
	}
}
