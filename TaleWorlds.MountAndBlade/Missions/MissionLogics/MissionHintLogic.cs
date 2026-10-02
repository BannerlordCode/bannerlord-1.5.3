using System;
using TaleWorlds.MountAndBlade.Missions.Hints;

namespace TaleWorlds.MountAndBlade.Missions.MissionLogics
{
	// Token: 0x020003F4 RID: 1012
	public class MissionHintLogic : MissionLogic
	{
		// Token: 0x140000AF RID: 175
		// (add) Token: 0x060037E5 RID: 14309 RVA: 0x000E7D38 File Offset: 0x000E5F38
		// (remove) Token: 0x060037E6 RID: 14310 RVA: 0x000E7D70 File Offset: 0x000E5F70
		public event MissionHintLogic.MissionHintChangedDelegate OnActiveHintChanged;

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x060037E7 RID: 14311 RVA: 0x000E7DA5 File Offset: 0x000E5FA5
		// (set) Token: 0x060037E8 RID: 14312 RVA: 0x000E7DAD File Offset: 0x000E5FAD
		public MissionHint ActiveHint { get; private set; }

		// Token: 0x060037E9 RID: 14313 RVA: 0x000E7DB8 File Offset: 0x000E5FB8
		public void SetActiveHint(MissionHint hint)
		{
			MissionHint activeHint = this.ActiveHint;
			this.ActiveHint = hint;
			MissionHintLogic.MissionHintChangedDelegate onActiveHintChanged = this.OnActiveHintChanged;
			if (onActiveHintChanged == null)
			{
				return;
			}
			onActiveHintChanged(activeHint, this.ActiveHint);
		}

		// Token: 0x060037EA RID: 14314 RVA: 0x000E7DEA File Offset: 0x000E5FEA
		public void Clear()
		{
			this.SetActiveHint(null);
		}

		// Token: 0x020006A8 RID: 1704
		// (Invoke) Token: 0x0600429A RID: 17050
		public delegate void MissionHintChangedDelegate(MissionHint previousHint, MissionHint newHint);
	}
}
