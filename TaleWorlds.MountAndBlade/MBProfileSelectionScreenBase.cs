using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.PlatformService;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000394 RID: 916
	public class MBProfileSelectionScreenBase : ScreenBase, IGameStateListener
	{
		// Token: 0x060034CD RID: 13517 RVA: 0x000DA696 File Offset: 0x000D8896
		public MBProfileSelectionScreenBase(ProfileSelectionState state)
		{
			this._state = state;
		}

		// Token: 0x060034CE RID: 13518 RVA: 0x000DA6A5 File Offset: 0x000D88A5
		protected sealed override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			if (ScreenManager.TopScreen == this)
			{
				this.OnProfileSelectionTick(dt);
			}
		}

		// Token: 0x060034CF RID: 13519 RVA: 0x000DA6BD File Offset: 0x000D88BD
		protected virtual void OnProfileSelectionTick(float dt)
		{
		}

		// Token: 0x060034D0 RID: 13520 RVA: 0x000DA6BF File Offset: 0x000D88BF
		protected void OnActivateProfileSelection()
		{
			PlatformServices.Instance.LoginUser();
		}

		// Token: 0x060034D1 RID: 13521 RVA: 0x000DA6CB File Offset: 0x000D88CB
		void IGameStateListener.OnActivate()
		{
		}

		// Token: 0x060034D2 RID: 13522 RVA: 0x000DA6CD File Offset: 0x000D88CD
		void IGameStateListener.OnDeactivate()
		{
		}

		// Token: 0x060034D3 RID: 13523 RVA: 0x000DA6CF File Offset: 0x000D88CF
		void IGameStateListener.OnFinalize()
		{
		}

		// Token: 0x060034D4 RID: 13524 RVA: 0x000DA6D1 File Offset: 0x000D88D1
		void IGameStateListener.OnInitialize()
		{
			Utilities.DisableGlobalLoadingWindow();
			LoadingWindow.DisableGlobalLoadingWindow();
		}

		// Token: 0x04001662 RID: 5730
		private ProfileSelectionState _state;
	}
}
