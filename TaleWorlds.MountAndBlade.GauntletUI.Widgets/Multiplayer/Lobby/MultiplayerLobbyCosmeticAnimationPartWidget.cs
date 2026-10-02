using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000A2 RID: 162
	public class MultiplayerLobbyCosmeticAnimationPartWidget : Widget
	{
		// Token: 0x060008D2 RID: 2258 RVA: 0x000197A9 File Offset: 0x000179A9
		public MultiplayerLobbyCosmeticAnimationPartWidget(UIContext context)
			: base(context)
		{
			this.StopAnimation();
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x000197B8 File Offset: 0x000179B8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._isAnimationPlaying)
			{
				return;
			}
			if (this._alphaChangeTimeElapsed >= this._alphaChangeDuration)
			{
				this.InvertAnimationDirection();
				this.InitializeAnimationParameters();
			}
			this._currentAlpha = MathF.Lerp(this._currentAlpha, this._targetAlpha, this._alphaChangeTimeElapsed / this._alphaChangeDuration, 1E-05f);
			base.AlphaFactor = this._currentAlpha;
			this._alphaChangeTimeElapsed += dt;
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x00019832 File Offset: 0x00017A32
		public void InitializeAnimationParameters()
		{
			this._currentAlpha = this._minAlpha;
			this._targetAlpha = this._maxAlpha;
			this._alphaChangeTimeElapsed = 0f;
			base.AlphaFactor = this._currentAlpha;
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x00019864 File Offset: 0x00017A64
		private void InvertAnimationDirection()
		{
			float minAlpha = this._minAlpha;
			this._minAlpha = this._maxAlpha;
			this._maxAlpha = minAlpha;
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0001988B File Offset: 0x00017A8B
		public void StartAnimation(float alphaChangeDuration, float minAlpha, float maxAlpha)
		{
			this._alphaChangeDuration = alphaChangeDuration;
			this._minAlpha = minAlpha;
			this._maxAlpha = maxAlpha;
			this.InitializeAnimationParameters();
			this._isAnimationPlaying = true;
			base.IsVisible = true;
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x000198B6 File Offset: 0x00017AB6
		public void StopAnimation()
		{
			this.InitializeAnimationParameters();
			this._isAnimationPlaying = false;
			base.IsVisible = false;
		}

		// Token: 0x040003FB RID: 1019
		private float _alphaChangeDuration;

		// Token: 0x040003FC RID: 1020
		private float _minAlpha;

		// Token: 0x040003FD RID: 1021
		private float _maxAlpha;

		// Token: 0x040003FE RID: 1022
		private float _currentAlpha;

		// Token: 0x040003FF RID: 1023
		private float _targetAlpha;

		// Token: 0x04000400 RID: 1024
		private float _alphaChangeTimeElapsed;

		// Token: 0x04000401 RID: 1025
		private bool _isAnimationPlaying;
	}
}
