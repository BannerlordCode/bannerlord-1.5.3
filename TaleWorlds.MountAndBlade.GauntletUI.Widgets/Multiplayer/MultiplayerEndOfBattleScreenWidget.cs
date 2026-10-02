using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer
{
	// Token: 0x02000088 RID: 136
	public class MultiplayerEndOfBattleScreenWidget : Widget
	{
		// Token: 0x060007B5 RID: 1973 RVA: 0x00016A0A File Offset: 0x00014C0A
		public MultiplayerEndOfBattleScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x00016A2C File Offset: 0x00014C2C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isAnimationStarted)
			{
				this.SetGlobalAlphaRecursively(MathF.Lerp(this._initialAlpha, this._targetAlpha, this._fadeInTimeElapsed / this.FadeInDuration, 1E-05f));
				this._fadeInTimeElapsed += dt;
				if (this._fadeInTimeElapsed >= this.FadeInDuration)
				{
					this._isAnimationStarted = false;
				}
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x060007B7 RID: 1975 RVA: 0x00016A94 File Offset: 0x00014C94
		// (set) Token: 0x060007B8 RID: 1976 RVA: 0x00016A9C File Offset: 0x00014C9C
		[Editor(false)]
		public bool IsShown
		{
			get
			{
				return this._isShown;
			}
			set
			{
				if (value != this._isShown)
				{
					this._isShown = value;
					base.OnPropertyChanged(value, "IsShown");
					this.SetGlobalAlphaRecursively(0f);
					this._isAnimationStarted = value;
					base.IsVisible = value;
					this._fadeInTimeElapsed = 0f;
				}
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x060007B9 RID: 1977 RVA: 0x00016AE9 File Offset: 0x00014CE9
		// (set) Token: 0x060007BA RID: 1978 RVA: 0x00016AF1 File Offset: 0x00014CF1
		[Editor(false)]
		public float FadeInDuration
		{
			get
			{
				return this._fadeInDuration;
			}
			set
			{
				if (value != this._fadeInDuration)
				{
					this._fadeInDuration = value;
					base.OnPropertyChanged(value, "FadeInDuration");
				}
			}
		}

		// Token: 0x0400035D RID: 861
		private float _initialAlpha;

		// Token: 0x0400035E RID: 862
		private float _targetAlpha = 1f;

		// Token: 0x0400035F RID: 863
		private bool _isAnimationStarted;

		// Token: 0x04000360 RID: 864
		private float _fadeInTimeElapsed;

		// Token: 0x04000361 RID: 865
		private bool _isShown;

		// Token: 0x04000362 RID: 866
		private float _fadeInDuration = 0.3f;
	}
}
