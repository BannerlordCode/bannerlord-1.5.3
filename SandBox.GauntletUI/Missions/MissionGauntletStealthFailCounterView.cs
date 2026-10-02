using System;
using SandBox.Missions;
using SandBox.View.Missions;
using SandBox.ViewModelCollection.Missions.NameMarker.Targets.Hideout;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.View;

namespace SandBox.GauntletUI.Missions
{
	// Token: 0x02000023 RID: 35
	[OverrideView(typeof(MissionStealthFailCounterView))]
	public class MissionGauntletStealthFailCounterView : MissionStealthFailCounterView
	{
		// Token: 0x060001D8 RID: 472 RVA: 0x0000C278 File Offset: 0x0000A478
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._countdownCounterVM = new MissionStealthFailCounterVM();
			this._countdownLayer = new GauntletLayer("MissionStealthFailCounter", 10, false);
			this._countdownLayer.LoadMovie("MissionStealthFailCounter", this._countdownCounterVM);
			base.MissionScreen.AddLayer(this._countdownLayer);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000C2D1 File Offset: 0x0000A4D1
		public override void AfterStart()
		{
			this._stealthFailCounterMissionLogic = base.Mission.GetMissionBehavior<StealthFailCounterMissionLogic>();
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000C2E4 File Offset: 0x0000A4E4
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			this._countdownCounterVM.OnFinalize();
			base.MissionScreen.RemoveLayer(this._countdownLayer);
			this._countdownLayer = null;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000C30F File Offset: 0x0000A50F
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this._stealthFailCounterMissionLogic != null)
			{
				this._countdownCounterVM.UpdateFailCounter(this._stealthFailCounterMissionLogic.FailCounterElapsedTime, this._stealthFailCounterMissionLogic.FailCounterSeconds, this._stealthFailCounterMissionLogic.IsActive);
			}
		}

		// Token: 0x04000098 RID: 152
		private GauntletLayer _countdownLayer;

		// Token: 0x04000099 RID: 153
		private MissionStealthFailCounterVM _countdownCounterVM;

		// Token: 0x0400009A RID: 154
		private StealthFailCounterMissionLogic _stealthFailCounterMissionLogic;
	}
}
