using System;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x02000007 RID: 7
	public class GauntletCameraFadeView : GlobalLayer, IScreenFadeHandler
	{
		// Token: 0x06000029 RID: 41 RVA: 0x00003500 File Offset: 0x00001700
		public GauntletCameraFadeView()
		{
			this._dataSource = new BindingListFloatItem(this._fadeAlpha);
			this._gauntletLayer = new GauntletLayer("CameraFade", 100000, false);
			this._gauntletLayer.LoadMovie("CameraFade", this._dataSource);
			base.Layer = this._gauntletLayer;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000355D File Offset: 0x0000175D
		public static void Initialize()
		{
			if (!GauntletCameraFadeView._isInitialized)
			{
				GauntletCameraFadeView gauntletCameraFadeView = new GauntletCameraFadeView();
				ScreenManager.AddGlobalLayer(gauntletCameraFadeView, false);
				ScreenFadeController.RegisterHandler(gauntletCameraFadeView);
				GauntletCameraFadeView._isInitialized = true;
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003580 File Offset: 0x00001780
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			switch (this._fadeState)
			{
			case ScreenFadeController.ScreenFadeState.None:
				this._fadeAlpha = 0f;
				break;
			case ScreenFadeController.ScreenFadeState.FadingOut:
				this._currentStateTimer += dt;
				this._fadeAlpha = MathF.Lerp(this._currentStateBeginAlpha, 1f, MathF.Min(this._currentStateTimer / this._fadeOutDuration, 1f), 1E-05f);
				if (this._currentStateTimer > this._fadeOutDuration)
				{
					this.SetFadeState(ScreenFadeController.ScreenFadeState.FadedOut);
				}
				break;
			case ScreenFadeController.ScreenFadeState.FadedOut:
				this._fadeAlpha = 1f;
				if (this._autoFadeIn)
				{
					this._currentStateTimer += dt;
					if (this._currentStateTimer > this._blackOutDuration)
					{
						this.SetFadeState(ScreenFadeController.ScreenFadeState.FadingIn);
					}
				}
				break;
			case ScreenFadeController.ScreenFadeState.FadingIn:
				this._currentStateTimer += dt;
				this._fadeAlpha = MathF.Lerp(this._currentStateBeginAlpha, 0f, MathF.Min(this._currentStateTimer / this._fadeInDuration, 1f), 1E-05f);
				if (this._currentStateTimer > this._fadeInDuration)
				{
					this.SetFadeState(ScreenFadeController.ScreenFadeState.None);
				}
				break;
			}
			this._dataSource.Item = this._fadeAlpha;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000036C4 File Offset: 0x000018C4
		public void BeginFadeOutAndIn(float fadeOutDuration = 0.5f, float blackOutDuration = 0.5f, float fadeInDuration = 0.5f)
		{
			this._fadeOutDuration = MathF.Max(fadeOutDuration, 0f);
			this._blackOutDuration = MathF.Max(blackOutDuration, 0f);
			this._fadeInDuration = MathF.Max(fadeInDuration, 0f);
			this._autoFadeIn = true;
			this.SetFadeState(ScreenFadeController.ScreenFadeState.FadingOut);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003712 File Offset: 0x00001912
		public void BeginFadeOut(float fadeOutDuration = 0.5f)
		{
			this._fadeOutDuration = MathF.Max(fadeOutDuration, 0f);
			this._autoFadeIn = false;
			this.SetFadeState(ScreenFadeController.ScreenFadeState.FadingOut);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003733 File Offset: 0x00001933
		public void BeginFadeIn(float fadeInDuration = 0.5f)
		{
			this._fadeInDuration = MathF.Max(fadeInDuration, 0f);
			if (this._fadeState == ScreenFadeController.ScreenFadeState.FadingOut || this._fadeState == ScreenFadeController.ScreenFadeState.FadedOut)
			{
				this.SetFadeState(ScreenFadeController.ScreenFadeState.FadingIn);
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000375F File Offset: 0x0000195F
		private void SetFadeState(ScreenFadeController.ScreenFadeState fadeState)
		{
			if (this._fadeState != fadeState)
			{
				this._fadeState = fadeState;
				this._currentStateTimer = 0f;
				this._currentStateBeginAlpha = this._fadeAlpha;
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003788 File Offset: 0x00001988
		public ScreenFadeController.ScreenFadeState GetScreenFadeState()
		{
			return this._fadeState;
		}

		// Token: 0x04000025 RID: 37
		private float _fadeAlpha;

		// Token: 0x04000026 RID: 38
		private ScreenFadeController.ScreenFadeState _fadeState;

		// Token: 0x04000027 RID: 39
		private float _currentStateTimer;

		// Token: 0x04000028 RID: 40
		private float _currentStateBeginAlpha;

		// Token: 0x04000029 RID: 41
		private float _fadeOutDuration;

		// Token: 0x0400002A RID: 42
		private float _blackOutDuration;

		// Token: 0x0400002B RID: 43
		private float _fadeInDuration;

		// Token: 0x0400002C RID: 44
		private bool _autoFadeIn;

		// Token: 0x0400002D RID: 45
		private static bool _isInitialized;

		// Token: 0x0400002E RID: 46
		private readonly GauntletLayer _gauntletLayer;

		// Token: 0x0400002F RID: 47
		private readonly BindingListFloatItem _dataSource;
	}
}
