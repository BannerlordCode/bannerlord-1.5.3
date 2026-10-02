using System;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.ObjectSystem
{
	// Token: 0x02000012 RID: 18
	internal class MBGUIDBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x06000095 RID: 149 RVA: 0x00004FB8 File Offset: 0x000031B8
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			writer.WriteUInt(((MBGUID)value).InternalValue);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00004FD9 File Offset: 0x000031D9
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return new MBGUID(reader.ReadUInt());
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004FEB File Offset: 0x000031EB
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 4;
		}
	}
}
