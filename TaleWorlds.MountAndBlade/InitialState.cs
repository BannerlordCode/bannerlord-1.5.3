using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000246 RID: 582
	public class InitialState : GameState
	{
		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x060021B8 RID: 8632 RVA: 0x00076FF4 File Offset: 0x000751F4
		public override bool IsMusicMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x060021B9 RID: 8633 RVA: 0x00076FF8 File Offset: 0x000751F8
		// (remove) Token: 0x060021BA RID: 8634 RVA: 0x00077030 File Offset: 0x00075230
		public event OnInitialMenuOptionInvokedDelegate OnInitialMenuOptionInvoked;

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x060021BB RID: 8635 RVA: 0x00077068 File Offset: 0x00075268
		// (remove) Token: 0x060021BC RID: 8636 RVA: 0x000770A0 File Offset: 0x000752A0
		public event OnGameContentUpdatedDelegate OnGameContentUpdated;

		// Token: 0x060021BD RID: 8637 RVA: 0x000770D5 File Offset: 0x000752D5
		protected override void OnActivate()
		{
			base.OnActivate();
			MBMusicManager mbmusicManager = MBMusicManager.Current;
			if (mbmusicManager == null)
			{
				return;
			}
			mbmusicManager.UnpauseMusicManagerSystem();
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x000770EC File Offset: 0x000752EC
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x000770F5 File Offset: 0x000752F5
		public void OnExecutedInitialStateOption(InitialStateOption target)
		{
			OnInitialMenuOptionInvokedDelegate onInitialMenuOptionInvoked = this.OnInitialMenuOptionInvoked;
			if (onInitialMenuOptionInvoked == null)
			{
				return;
			}
			onInitialMenuOptionInvoked(target);
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x00077108 File Offset: 0x00075308
		public void RefreshContentState()
		{
			OnGameContentUpdatedDelegate onGameContentUpdated = this.OnGameContentUpdated;
			if (onGameContentUpdated == null)
			{
				return;
			}
			onGameContentUpdated();
		}
	}
}
