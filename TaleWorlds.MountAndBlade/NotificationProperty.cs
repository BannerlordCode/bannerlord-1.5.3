using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000328 RID: 808
	[AttributeUsage(AttributeTargets.Field)]
	public class NotificationProperty : Attribute
	{
		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x06002E35 RID: 11829 RVA: 0x000B31BB File Offset: 0x000B13BB
		// (set) Token: 0x06002E36 RID: 11830 RVA: 0x000B31C3 File Offset: 0x000B13C3
		public string StringId { get; private set; }

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06002E37 RID: 11831 RVA: 0x000B31CC File Offset: 0x000B13CC
		// (set) Token: 0x06002E38 RID: 11832 RVA: 0x000B31D4 File Offset: 0x000B13D4
		public string SoundIdOne { get; private set; }

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06002E39 RID: 11833 RVA: 0x000B31DD File Offset: 0x000B13DD
		// (set) Token: 0x06002E3A RID: 11834 RVA: 0x000B31E5 File Offset: 0x000B13E5
		public string SoundIdTwo { get; private set; }

		// Token: 0x06002E3B RID: 11835 RVA: 0x000B31EE File Offset: 0x000B13EE
		public NotificationProperty(string stringId, string soundIdOne, string soundIdTwo = "")
		{
			this.StringId = stringId;
			this.SoundIdOne = soundIdOne;
			this.SoundIdTwo = soundIdTwo;
		}
	}
}
