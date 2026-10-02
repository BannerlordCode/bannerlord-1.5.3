using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000053 RID: 83
	internal class Mat3BasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002E7 RID: 743 RVA: 0x0000D45C File Offset: 0x0000B65C
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			Mat3 mat = (Mat3)value;
			writer.WriteVec3(mat.s);
			writer.WriteVec3(mat.f);
			writer.WriteVec3(mat.u);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000D494 File Offset: 0x0000B694
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			Vec3 vec = reader.ReadVec3();
			Vec3 vec2 = reader.ReadVec3();
			Vec3 vec3 = reader.ReadVec3();
			return new Mat3(in vec, in vec2, in vec3);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000D4C6 File Offset: 0x0000B6C6
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 48;
		}
	}
}
