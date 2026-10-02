using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000A0 RID: 160
	public class MultiplayerLobbyBattleRewardWidget : Widget
	{
		// Token: 0x1700030B RID: 779
		// (get) Token: 0x060008A5 RID: 2213 RVA: 0x00018E92 File Offset: 0x00017092
		// (set) Token: 0x060008A6 RID: 2214 RVA: 0x00018E9A File Offset: 0x0001709A
		public float AnimationDuration { get; set; } = 0.1f;

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x060008A7 RID: 2215 RVA: 0x00018EA3 File Offset: 0x000170A3
		// (set) Token: 0x060008A8 RID: 2216 RVA: 0x00018EAB File Offset: 0x000170AB
		public float TextRevealAnimationDuration { get; set; } = 0.05f;

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x060008A9 RID: 2217 RVA: 0x00018EB4 File Offset: 0x000170B4
		// (set) Token: 0x060008AA RID: 2218 RVA: 0x00018EBC File Offset: 0x000170BC
		public float AnimationInitialScaleMultiplier { get; set; } = 2f;

		// Token: 0x060008AB RID: 2219 RVA: 0x00018EC5 File Offset: 0x000170C5
		public MultiplayerLobbyBattleRewardWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x00018EF0 File Offset: 0x000170F0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isInPreAnimationState)
			{
				foreach (Widget widget in base.Children)
				{
					if (widget is ValueBasedVisibilityWidget)
					{
						widget.IsVisible = false;
					}
				}
			}
			bool flag = false;
			if (this._isAnimationStarted && base.EventManager.Time - this._animationStartTime < this.AnimationDuration)
			{
				float num = (base.EventManager.Time - this._animationStartTime) / this.AnimationDuration;
				this._rewardIconButton.SuggestedWidth = Mathf.Lerp(this._buttonAnimationStartWidth, this._buttonAnimationEndWidth, num);
				this._rewardIconButton.SuggestedHeight = Mathf.Lerp(this._buttonAnimationStartHeight, this._buttonAnimationEndHeight, num);
				this._rewardIcon.SuggestedWidth = Mathf.Lerp(this._iconAnimationStartWidget, this._iconAnimationEndWidth, num);
				this._rewardIcon.SuggestedHeight = Mathf.Lerp(this._iconAnimationStartHeight, this._iconAnimationEndHeight, num);
				this._rewardIconButton.SetGlobalAlphaRecursively(Mathf.Lerp(0f, 1f, num));
				this._rewardIcon.SetGlobalAlphaRecursively(Mathf.Lerp(0f, 1f, num));
				this._rewardToShow.IsVisible = true;
				flag = true;
			}
			if (!this._isTextAnimationStarted && this._isAnimationStarted && base.EventManager.Time - this._animationStartTime >= this.AnimationDuration)
			{
				this._textAnimationStartTime = base.EventManager.Time;
				this._isTextAnimationStarted = true;
			}
			if (this._isTextAnimationStarted && base.EventManager.Time - this._textAnimationStartTime < this.TextRevealAnimationDuration)
			{
				float num2 = (base.EventManager.Time - this._textAnimationStartTime) / this.TextRevealAnimationDuration;
				this._rewardTextDescription.SetGlobalAlphaRecursively(Mathf.Lerp(0f, 1f, num2));
				flag = true;
			}
			if (this._isAnimationStarted && this._isTextAnimationStarted && !flag)
			{
				this.EndAnimation();
			}
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0001910C File Offset: 0x0001730C
		public void StartAnimation()
		{
			this._isInPreAnimationState = false;
			foreach (Widget widget in base.Children)
			{
				ValueBasedVisibilityWidget valueBasedVisibilityWidget;
				if ((valueBasedVisibilityWidget = widget as ValueBasedVisibilityWidget) != null && valueBasedVisibilityWidget.IndexToWatch == valueBasedVisibilityWidget.IndexToBeVisible)
				{
					this._rewardToShow = valueBasedVisibilityWidget;
					this._rewardIconButton = widget.Children[0].Children[0] as ButtonWidget;
					this._rewardIcon = this._rewardIconButton.Children[0];
					this._rewardTextDescription = widget.Children[0].Children[1] as TextWidget;
					this._buttonAnimationStartWidth = this._rewardIconButton.SuggestedWidth * this.AnimationInitialScaleMultiplier;
					this._buttonAnimationStartHeight = this._rewardIconButton.SuggestedHeight * this.AnimationInitialScaleMultiplier;
					this._buttonAnimationEndWidth = this._rewardIconButton.SuggestedWidth;
					this._buttonAnimationEndHeight = this._rewardIconButton.SuggestedHeight;
					this._iconAnimationStartWidget = this._rewardIcon.SuggestedWidth * this.AnimationInitialScaleMultiplier;
					this._iconAnimationStartHeight = this._rewardIcon.SuggestedHeight * this.AnimationInitialScaleMultiplier;
					this._iconAnimationEndWidth = this._rewardIcon.SuggestedWidth;
					this._iconAnimationEndHeight = this._rewardIcon.SuggestedHeight;
					this._rewardTextDescription.SetGlobalAlphaRecursively(0f);
				}
			}
			this._isAnimationStarted = true;
			this._animationStartTime = base.EventManager.Time;
			base.Context.TwoDimensionContext.PlaySound("inventory/perk");
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x000192D4 File Offset: 0x000174D4
		public void StartPreAnimation()
		{
			this._isInPreAnimationState = true;
			base.IsVisible = true;
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x000192E4 File Offset: 0x000174E4
		public void EndAnimation()
		{
			this._rewardIconButton.SetGlobalAlphaRecursively(1f);
			this._rewardIcon.SetGlobalAlphaRecursively(1f);
			this._rewardTextDescription.SetGlobalAlphaRecursively(1f);
			this._rewardIconButton.SuggestedWidth = this._buttonAnimationEndWidth;
			this._rewardIconButton.SuggestedHeight = this._buttonAnimationEndHeight;
			this._rewardIcon.SuggestedWidth = this._iconAnimationEndWidth;
			this._rewardIcon.SuggestedHeight = this._iconAnimationEndHeight;
			this._isAnimationStarted = false;
			this._isTextAnimationStarted = false;
		}

		// Token: 0x040003DA RID: 986
		private const string _rewardImpactSoundEventName = "inventory/perk";

		// Token: 0x040003DB RID: 987
		private float _buttonAnimationStartWidth;

		// Token: 0x040003DC RID: 988
		private float _buttonAnimationStartHeight;

		// Token: 0x040003DD RID: 989
		private float _buttonAnimationEndWidth;

		// Token: 0x040003DE RID: 990
		private float _buttonAnimationEndHeight;

		// Token: 0x040003DF RID: 991
		private float _iconAnimationStartWidget;

		// Token: 0x040003E0 RID: 992
		private float _iconAnimationStartHeight;

		// Token: 0x040003E1 RID: 993
		private float _iconAnimationEndWidth;

		// Token: 0x040003E2 RID: 994
		private float _iconAnimationEndHeight;

		// Token: 0x040003E6 RID: 998
		private ButtonWidget _rewardIconButton;

		// Token: 0x040003E7 RID: 999
		private Widget _rewardIcon;

		// Token: 0x040003E8 RID: 1000
		private TextWidget _rewardTextDescription;

		// Token: 0x040003E9 RID: 1001
		private ValueBasedVisibilityWidget _rewardToShow;

		// Token: 0x040003EA RID: 1002
		private bool _isAnimationStarted;

		// Token: 0x040003EB RID: 1003
		private bool _isTextAnimationStarted;

		// Token: 0x040003EC RID: 1004
		private bool _isInPreAnimationState;

		// Token: 0x040003ED RID: 1005
		private float _animationStartTime;

		// Token: 0x040003EE RID: 1006
		private float _textAnimationStartTime;
	}
}
