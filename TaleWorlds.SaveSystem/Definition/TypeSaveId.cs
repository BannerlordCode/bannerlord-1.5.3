using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200006F RID: 111
	public class TypeSaveId : SaveId
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060003CA RID: 970 RVA: 0x00011646 File Offset: 0x0000F846
		// (set) Token: 0x060003CB RID: 971 RVA: 0x0001164E File Offset: 0x0000F84E
		public int Id { get; private set; }

		// Token: 0x060003CC RID: 972 RVA: 0x00011658 File Offset: 0x0000F858
		public TypeSaveId(int id)
		{
			this.Id = id;
			this._stringId = this.Id.ToString();
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00011686 File Offset: 0x0000F886
		public override string GetStringId()
		{
			return this._stringId;
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0001168E File Offset: 0x0000F88E
		public override void WriteTo(IWriter writer)
		{
			writer.WriteByte(0);
			writer.WriteInt(this.Id);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x000116A3 File Offset: 0x0000F8A3
		public static TypeSaveId ReadFrom(IReader reader)
		{
			return new TypeSaveId(reader.ReadInt());
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x000116B0 File Offset: 0x0000F8B0
		public override int GetSizeInBytes()
		{
			return 5;
		}

		// Token: 0x04000128 RID: 296
		private readonly string _stringId;
	}
}
