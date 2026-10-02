using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000052 RID: 82
	internal class Mat2BasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002E3 RID: 739 RVA: 0x0000D3E4 File Offset: 0x0000B5E4
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			Mat2 mat = (Mat2)value;
			writer.WriteVec2(mat.s);
			writer.WriteVec2(mat.f);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000D410 File Offset: 0x0000B610
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			Vec2 vec = reader.ReadVec2();
			Vec2 vec2 = reader.ReadVec2();
			return new Mat2(vec.x, vec.y, vec2.x, vec2.y);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000D44D File Offset: 0x0000B64D
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 16;
		}
	}
}
