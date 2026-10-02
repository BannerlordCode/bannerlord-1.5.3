using System;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x0200006C RID: 108
	public abstract class MissionBattleUIBaseView : MissionView
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x0001F5B0 File Offset: 0x0001D7B0
		// (set) Token: 0x06000439 RID: 1081 RVA: 0x0001F5B8 File Offset: 0x0001D7B8
		public bool IsViewCreated { get; private set; }

		// Token: 0x0600043A RID: 1082
		protected abstract void OnCreateView();

		// Token: 0x0600043B RID: 1083
		protected abstract void OnDestroyView();

		// Token: 0x0600043C RID: 1084 RVA: 0x0001F5C1 File Offset: 0x0001D7C1
		private void OnEnableView()
		{
			this.OnCreateView();
			this.IsViewCreated = true;
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0001F5D0 File Offset: 0x0001D7D0
		private void OnDisableView()
		{
			this.OnDestroyView();
			this.IsViewCreated = false;
		}

		// Token: 0x0600043E RID: 1086
		protected abstract override void OnSuspendView();

		// Token: 0x0600043F RID: 1087
		protected abstract override void OnResumeView();

		// Token: 0x06000440 RID: 1088 RVA: 0x0001F5DF File Offset: 0x0001D7DF
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			if (GameNetwork.IsMultiplayer)
			{
				this.OnEnableView();
			}
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0001F5F4 File Offset: 0x0001D7F4
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (!GameNetwork.IsMultiplayer && !MBCommon.IsPaused)
			{
				if (!this.IsViewCreated && !BannerlordConfig.HideBattleUI)
				{
					this.OnEnableView();
					return;
				}
				if (this.IsViewCreated && BannerlordConfig.HideBattleUI)
				{
					this.OnDisableView();
				}
			}
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0001F641 File Offset: 0x0001D841
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			if (this.IsViewCreated)
			{
				this.OnDisableView();
			}
		}
	}
}
