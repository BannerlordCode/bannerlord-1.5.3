using System;
using SandBox.Objects.Usables;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Missions.NameMarker.Targets.Hideout
{
	// Token: 0x0200003E RID: 62
	public class MissionStealthAreaUsePointNameMarkerTargetVM : MissionNameMarkerTargetBaseVM
	{
		// Token: 0x06000434 RID: 1076 RVA: 0x00011B04 File Offset: 0x0000FD04
		public MissionStealthAreaUsePointNameMarkerTargetVM(StealthAreaUsePoint usePoint)
		{
			this._usePoint = usePoint;
			base.IconType = "call_troops";
			base.NameType = "Normal";
			this.RefreshValues();
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00011B2F File Offset: 0x0000FD2F
		public override bool Equals(MissionNameMarkerTargetBaseVM other)
		{
			return false;
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00011B34 File Offset: 0x0000FD34
		public override void UpdatePosition(Camera missionCamera)
		{
			MatrixFrame globalFrame = this._usePoint.GameEntity.GetGlobalFrame();
			base.UpdatePositionWith(missionCamera, globalFrame.origin + Vec3.Up * 0.5f);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00011B76 File Offset: 0x0000FD76
		protected override TextObject GetName()
		{
			return new TextObject("{=GmjiZk9P}Call Troops", null);
		}

		// Token: 0x0400022B RID: 555
		private StealthAreaUsePoint _usePoint;
	}
}
