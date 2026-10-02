using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000055 RID: 85
	internal class QuaternionBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002EF RID: 751 RVA: 0x0000D578 File Offset: 0x0000B778
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			Quaternion quaternion = (Quaternion)value;
			writer.WriteFloat(quaternion.X);
			writer.WriteFloat(quaternion.Y);
			writer.WriteFloat(quaternion.Z);
			writer.WriteFloat(quaternion.W);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000D5BC File Offset: 0x0000B7BC
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			float num = reader.ReadFloat();
			float num2 = reader.ReadFloat();
			float num3 = reader.ReadFloat();
			float num4 = reader.ReadFloat();
			return new Quaternion(num, num2, num3, num4);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000D5F1 File Offset: 0x0000B7F1
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 16;
		}
	}
}
