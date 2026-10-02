using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200006A RID: 106
	public abstract class SaveId
	{
		// Token: 0x0600039B RID: 923
		public abstract string GetStringId();

		// Token: 0x0600039C RID: 924 RVA: 0x00010F60 File Offset: 0x0000F160
		public override int GetHashCode()
		{
			return this.GetStringId().GetHashCode();
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00010F6D File Offset: 0x0000F16D
		public override bool Equals(object obj)
		{
			return obj != null && !(obj.GetType() != base.GetType()) && this.GetStringId() == ((SaveId)obj).GetStringId();
		}

		// Token: 0x0600039E RID: 926
		public abstract void WriteTo(IWriter writer);

		// Token: 0x0600039F RID: 927 RVA: 0x00010FA0 File Offset: 0x0000F1A0
		public static SaveId ReadSaveIdFrom(IReader reader)
		{
			byte b = reader.ReadByte();
			SaveId saveId = null;
			if (b == 0)
			{
				saveId = TypeSaveId.ReadFrom(reader);
			}
			else if (b == 1)
			{
				saveId = GenericSaveId.ReadFrom(reader);
			}
			else if (b == 2)
			{
				saveId = ContainerSaveId.ReadFrom(reader);
			}
			return saveId;
		}

		// Token: 0x060003A0 RID: 928
		public abstract int GetSizeInBytes();
	}
}
