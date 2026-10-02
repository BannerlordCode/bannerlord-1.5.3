using System;
using SandBox.Objects.Usables;
using TaleWorlds.Library;

namespace SandBox.Objects
{
	// Token: 0x02000038 RID: 56
	public class GroupSpawnPoint : UsablePlace
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x0000CDB3 File Offset: 0x0000AFB3
		public bool IsInstant
		{
			get
			{
				return this.Delay < 0f || this.Delay.ApproximatelyEqualsTo(0f, 1E-05f);
			}
		}

		// Token: 0x040000BA RID: 186
		public float Delay = -1f;

		// Token: 0x040000BB RID: 187
		public int SpawnCount = 1;
	}
}
