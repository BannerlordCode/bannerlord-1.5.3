using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000050 RID: 80
	internal class Vec3BasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002DB RID: 731 RVA: 0x0000D37C File Offset: 0x0000B57C
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			Vec3 vec = (Vec3)value;
			writer.WriteVec3(vec);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000D397 File Offset: 0x0000B597
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadVec3();
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000D3A4 File Offset: 0x0000B5A4
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 16;
		}
	}
}
