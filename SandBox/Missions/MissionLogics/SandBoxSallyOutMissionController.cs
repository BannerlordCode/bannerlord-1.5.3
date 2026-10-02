using System;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000084 RID: 132
	public class SandBoxSallyOutMissionController : SallyOutMissionController
	{
		// Token: 0x06000535 RID: 1333 RVA: 0x00023028 File Offset: 0x00021228
		public SandBoxSallyOutMissionController(bool isSallyOutAmbush)
			: base(isSallyOutAmbush)
		{
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00023031 File Offset: 0x00021231
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._mapEvent = MapEvent.PlayerMapEvent;
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00023044 File Offset: 0x00021244
		protected override void GetInitialTroopCounts(out int besiegedTotalTroopCount, out int besiegerTotalTroopCount)
		{
			besiegedTotalTroopCount = this._mapEvent.GetNumberOfInvolvedMen(BattleSideEnum.Defender);
			besiegerTotalTroopCount = this._mapEvent.GetNumberOfInvolvedMen(BattleSideEnum.Attacker);
		}

		// Token: 0x040002C6 RID: 710
		private MapEvent _mapEvent;
	}
}
