using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000054 RID: 84
	internal class MatrixFrameBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002EB RID: 747 RVA: 0x0000D4D4 File Offset: 0x0000B6D4
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			MatrixFrame matrixFrame = (MatrixFrame)value;
			writer.WriteVec3(matrixFrame.origin);
			writer.WriteVec3(matrixFrame.rotation.s);
			writer.WriteVec3(matrixFrame.rotation.f);
			writer.WriteVec3(matrixFrame.rotation.u);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000D528 File Offset: 0x0000B728
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			Vec3 vec = reader.ReadVec3();
			Vec3 vec2 = reader.ReadVec3();
			Vec3 vec3 = reader.ReadVec3();
			Vec3 vec4 = reader.ReadVec3();
			Mat3 mat = new Mat3(in vec3, in vec2, in vec4);
			return new MatrixFrame(in mat, in vec);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000D56C File Offset: 0x0000B76C
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 48;
		}
	}
}
