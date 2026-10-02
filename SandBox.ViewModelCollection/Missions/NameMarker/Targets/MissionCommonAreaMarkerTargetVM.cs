using System;
using SandBox.Objects.AreaMarkers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Engine;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Missions.NameMarker.Targets
{
	// Token: 0x02000039 RID: 57
	public class MissionCommonAreaMarkerTargetVM : MissionNameMarkerTargetVM<CommonAreaMarker>
	{
		// Token: 0x06000421 RID: 1057 RVA: 0x000117BC File Offset: 0x0000F9BC
		public MissionCommonAreaMarkerTargetVM(CommonAreaMarker target)
			: base(target)
		{
			base.NameType = "Passage";
			base.IconType = "common_area";
			this.TargetAlley = Hero.MainHero.CurrentSettlement.Alleys[target.AreaIndex - 1];
			this.UpdateAlleyStatus();
			CampaignEvents.AlleyOwnerChanged.AddNonSerializedListener(this, new Action<Alley, Hero, Hero>(this.OnAlleyOwnerChanged));
			this.RefreshValues();
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0001182B File Offset: 0x0000FA2B
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEventDispatcher.Instance.RemoveListeners(this);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0001183E File Offset: 0x0000FA3E
		private void OnAlleyOwnerChanged(Alley alley, Hero newOwner, Hero oldOwner)
		{
			if (this.TargetAlley == alley && (newOwner == Hero.MainHero || oldOwner == Hero.MainHero))
			{
				this.UpdateAlleyStatus();
			}
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0001185F File Offset: 0x0000FA5F
		public override void UpdatePosition(Camera missionCamera)
		{
			base.UpdatePositionWith(missionCamera, base.Target.GetPosition() + MissionNameMarkerHelper.DefaultHeightOffset);
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0001187D File Offset: 0x0000FA7D
		protected override TextObject GetName()
		{
			return base.Target.GetName();
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0001188C File Offset: 0x0000FA8C
		private void UpdateAlleyStatus()
		{
			if (this.TargetAlley != null)
			{
				Hero owner = this.TargetAlley.Owner;
				if (owner != null)
				{
					if (owner == Hero.MainHero)
					{
						base.NameType = "Friendly";
						base.IsFriendly = true;
						base.IsEnemy = false;
						return;
					}
					base.NameType = "Passage";
					base.IsFriendly = false;
					base.IsEnemy = true;
					return;
				}
				else
				{
					base.NameType = "Normal";
					base.IsFriendly = false;
					base.IsEnemy = false;
				}
			}
		}

		// Token: 0x04000225 RID: 549
		public readonly Alley TargetAlley;
	}
}
