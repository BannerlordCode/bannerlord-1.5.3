using System;

namespace TaleWorlds.MountAndBlade.Source.Missions
{
	// Token: 0x020003E0 RID: 992
	public class MissionOptionsComponent : MissionLogic
	{
		// Token: 0x140000AD RID: 173
		// (add) Token: 0x0600374A RID: 14154 RVA: 0x000E5A3C File Offset: 0x000E3C3C
		// (remove) Token: 0x0600374B RID: 14155 RVA: 0x000E5A74 File Offset: 0x000E3C74
		public event OnMissionAddOptionsDelegate OnOptionsAdded;

		// Token: 0x0600374C RID: 14156 RVA: 0x000E5AA9 File Offset: 0x000E3CA9
		public void OnAddOptionsUIHandler()
		{
			if (this.OnOptionsAdded != null)
			{
				this.OnOptionsAdded();
			}
		}
	}
}
