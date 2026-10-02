using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200005E RID: 94
	public readonly struct UndoRedoKey
	{
		// Token: 0x06000734 RID: 1844 RVA: 0x00018F0C File Offset: 0x0001710C
		public UndoRedoKey(int gender, int race, BodyProperties bodyProperties)
		{
			this.Gender = gender;
			this.Race = race;
			this.BodyProperties = bodyProperties;
		}

		// Token: 0x040003A2 RID: 930
		public readonly int Gender;

		// Token: 0x040003A3 RID: 931
		public readonly int Race;

		// Token: 0x040003A4 RID: 932
		public readonly BodyProperties BodyProperties;
	}
}
