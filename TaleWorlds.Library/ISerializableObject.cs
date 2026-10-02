using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000043 RID: 67
	public interface ISerializableObject
	{
		// Token: 0x0600021E RID: 542
		void DeserializeFrom(IReader reader);

		// Token: 0x0600021F RID: 543
		void SerializeTo(IWriter writer);
	}
}
