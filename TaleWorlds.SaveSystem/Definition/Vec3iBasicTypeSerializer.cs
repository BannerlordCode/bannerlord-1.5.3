using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000051 RID: 81
	internal class Vec3iBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002DF RID: 735 RVA: 0x0000D3B0 File Offset: 0x0000B5B0
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			Vec3i vec3i = (Vec3i)value;
			writer.WriteVec3Int(vec3i);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000D3CB File Offset: 0x0000B5CB
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadVec3Int();
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000D3D8 File Offset: 0x0000B5D8
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 12;
		}
	}
}
