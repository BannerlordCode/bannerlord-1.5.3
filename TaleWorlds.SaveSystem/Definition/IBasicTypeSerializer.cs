using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000043 RID: 67
	public interface IBasicTypeSerializer
	{
		// Token: 0x060002A8 RID: 680
		void Serialize(IWriter writer, object value);

		// Token: 0x060002A9 RID: 681
		object Deserialize(IReader reader);

		// Token: 0x060002AA RID: 682
		int GetSizeInBytes();
	}
}
