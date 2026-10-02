using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Nameplate
{
	// Token: 0x0200007F RID: 127
	public class PartyPlayerNameplateWidget : PartyNameplateWidget
	{
		// Token: 0x06000732 RID: 1842 RVA: 0x00014E03 File Offset: 0x00013003
		public PartyPlayerNameplateWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00014E0C File Offset: 0x0001300C
		protected override void UpdateNameplatesVisibility(float dt)
		{
			bool flag = !base.IsInSettlement && (base.IsPositionOutsideScreen() || base.IsBehind || base.IsHigh);
			bool flag2 = flag || base.IsInArmy || this.IsPrisoner || base.IsInSettlement;
			base.NameplateTextWidget.IsVisible = !flag2;
			base.NameplateFullNameTextWidget.IsVisible = !flag2;
			base.SpeedTextWidget.IsVisible = !flag2;
			base.SpeedIconWidget.IsVisible = !flag2;
			base.PartyBannerWidget.IsVisible = !flag2;
			base.NameplateExtraInfoTextWidget.IsVisible = !flag2;
			base.DisorganizedWidget.IsVisible = !flag2 && base.IsDisorganized;
			float num = (float)(flag2 ? 0 : 1);
			this.MainPartyArrowWidget.IsVisible = flag;
			base.TrackerFrame.IsVisible = flag;
			base.IsEnabled = flag;
			float num2;
			if (this._initialDelayAmount <= 0f)
			{
				num2 = (float)(base.ShouldShowFullName ? 1 : 0);
			}
			else
			{
				this._initialDelayAmount -= dt;
				num2 = 1f;
			}
			base.NameplateTextWidget.Brush.GlobalAlphaFactor = MathF.Lerp(base.NameplateTextWidget.ReadOnlyBrush.GlobalAlphaFactor, num, dt * base._animSpeedModifier, 1E-05f);
			base.NameplateFullNameTextWidget.Brush.GlobalAlphaFactor = MathF.Lerp(base.NameplateFullNameTextWidget.ReadOnlyBrush.GlobalAlphaFactor, num2, dt * base._animSpeedModifier, 1E-05f);
			base.SpeedTextWidget.Brush.GlobalAlphaFactor = MathF.Lerp(base.SpeedTextWidget.ReadOnlyBrush.GlobalAlphaFactor, num2, dt * base._animSpeedModifier, 1E-05f);
			float num3 = MathF.Lerp(base.SpeedIconWidget.AlphaFactor, num2, dt * base._animSpeedModifier, 1E-05f);
			base.SpeedIconWidget.SetGlobalAlphaRecursively(num3);
			base.NameplateExtraInfoTextWidget.Brush.GlobalAlphaFactor = MathF.Lerp(base.NameplateExtraInfoTextWidget.ReadOnlyBrush.GlobalAlphaFactor, (float)(base.ShouldShowFullName ? 1 : 0), dt * base._animSpeedModifier, 1E-05f);
			base.PartyBannerWidget.Brush.GlobalAlphaFactor = MathF.Lerp(base.PartyBannerWidget.ReadOnlyBrush.GlobalAlphaFactor, num, dt * base._animSpeedModifier, 1E-05f);
			base.ParleyIconWidget.AlphaFactor = MathF.Lerp(base.ParleyIconWidget.AlphaFactor, (float)(base.CanParley ? 1 : 0), dt * base._animSpeedModifier, 1E-05f);
			this._shouldShowShipBanner = base.IsShipBannerVisible && !flag;
			base.ShipBannerContainerWidget.IsVisible = this._shouldShowShipBanner;
			float num4 = MathF.Lerp(base.ShipBannerWidget.ReadOnlyBrush.GlobalAlphaFactor, (float)(this._shouldShowShipBanner ? 1 : 0), dt * base._animSpeedModifier, 1E-05f);
			base.ShipBannerContainerWidget.SetGlobalAlphaRecursively(num4);
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00015104 File Offset: 0x00013304
		protected override void UpdateNameplatesScreenPosition()
		{
			bool flag = base.IsBehind || base.IsPositionOutsideScreen();
			bool flag2 = !base.IsInSettlement && (base.IsHigh || base.IsBehind || base.IsPositionOutsideScreen());
			if (flag)
			{
				Vec2 vec = new Vec2(this._screenWidth / 2f, this._screenHeight / 2f);
				Vec2 vec2 = base.HeadPosition;
				vec2 -= vec;
				if (base.IsBehind)
				{
					vec2 *= -1f;
				}
				float num = Mathf.Atan2(vec2.y, vec2.x) - 1.5707964f;
				float num2 = Mathf.Cos(num);
				float num3 = Mathf.Sin(num);
				float num4 = num2 / num3;
				Vec2 vec3 = vec * 1f;
				vec2 = ((num2 > 0f) ? new Vec2(-vec3.y / num4, vec.y) : new Vec2(vec3.y / num4, -vec.y));
				if (vec2.x > vec3.x)
				{
					vec2 = new Vec2(vec3.x, -vec3.x * num4);
				}
				else if (vec2.x < -vec3.x)
				{
					vec2 = new Vec2(-vec3.x, vec3.x * num4);
				}
				vec2 += vec;
				base.ScaledPositionXOffset = vec2.x - base.Size.X / 2f;
				base.ScaledPositionYOffset = vec2.y - base.Size.Y / 2f;
			}
			else
			{
				Widget headGroupWidget = base.HeadGroupWidget;
				float num5 = ((headGroupWidget != null) ? headGroupWidget.Size.Y : 0f);
				base.NameplateLayoutListPanel.ScaledPositionXOffset = base.Size.X / 2f - base.PartyBannerWidget.Size.X;
				base.NameplateLayoutListPanel.ScaledPositionYOffset = base.Position.y - base.HeadPosition.y + num5;
				base.ScaledPositionXOffset = base.HeadPosition.x - base.Size.X / 2f;
				base.ScaledPositionYOffset = base.HeadPosition.y - num5;
			}
			if (flag2)
			{
				base.ScaledPositionXOffset = MathF.Clamp(base.ScaledPositionXOffset, 0f, this._screenWidth - base.Size.X);
				base.ScaledPositionYOffset = MathF.Clamp(base.ScaledPositionYOffset, 0f, this._screenHeight - base.Size.Y);
			}
			if (this._shouldShowShipBanner)
			{
				base.ShipBannerContainerWidget.ScaledPositionXOffset = base.ShipBannerPosition.x - base.ScaledPositionXOffset - base.ShipBannerContainerWidget.Size.X / 2f;
				base.ShipBannerContainerWidget.ScaledPositionYOffset = base.ShipBannerPosition.y - base.ScaledPositionYOffset - base.ShipBannerContainerWidget.Size.Y;
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x000153FC File Offset: 0x000135FC
		// (set) Token: 0x06000736 RID: 1846 RVA: 0x00015404 File Offset: 0x00013604
		public bool IsPrisoner
		{
			get
			{
				return this._isPrisoner;
			}
			set
			{
				if (this._isPrisoner != value)
				{
					this._isPrisoner = value;
					base.OnPropertyChanged(value, "IsPrisoner");
				}
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x00015422 File Offset: 0x00013622
		// (set) Token: 0x06000738 RID: 1848 RVA: 0x0001542A File Offset: 0x0001362A
		public Widget MainPartyArrowWidget
		{
			get
			{
				return this._mainPartyArrowWidget;
			}
			set
			{
				if (this._mainPartyArrowWidget != value)
				{
					this._mainPartyArrowWidget = value;
					base.OnPropertyChanged<Widget>(value, "MainPartyArrowWidget");
				}
			}
		}

		// Token: 0x0400031E RID: 798
		private bool _isPrisoner;

		// Token: 0x0400031F RID: 799
		private bool _shouldShowShipBanner;

		// Token: 0x04000320 RID: 800
		private Widget _mainPartyArrowWidget;
	}
}
