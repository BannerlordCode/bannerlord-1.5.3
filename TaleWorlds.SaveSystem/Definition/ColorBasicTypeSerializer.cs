using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000056 RID: 86
	internal class ColorBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002F3 RID: 755 RVA: 0x0000D600 File Offset: 0x0000B800
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			Color color = (Color)value;
			writer.WriteColor(color);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000D61B File Offset: 0x0000B81B
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadColor();
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000D628 File Offset: 0x0000B828
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 16;
		}
	}
}
