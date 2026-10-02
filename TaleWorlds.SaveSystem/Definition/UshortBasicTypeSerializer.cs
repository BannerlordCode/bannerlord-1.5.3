using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000047 RID: 71
	internal class UshortBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002B7 RID: 695 RVA: 0x0000D1DB File Offset: 0x0000B3DB
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			writer.WriteUShort((ushort)value);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000D1E9 File Offset: 0x0000B3E9
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadUShort();
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000D1F6 File Offset: 0x0000B3F6
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 2;
		}
	}
}
