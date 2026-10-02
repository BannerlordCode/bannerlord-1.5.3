using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterDeveloper
{
	// Token: 0x02000184 RID: 388
	public class PerkItemButtonWidget : ButtonWidget
	{
		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06001438 RID: 5176 RVA: 0x00037560 File Offset: 0x00035760
		// (set) Token: 0x06001439 RID: 5177 RVA: 0x00037568 File Offset: 0x00035768
		public Brush NotEarnedPerkBrush { get; set; }

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x0600143A RID: 5178 RVA: 0x00037571 File Offset: 0x00035771
		// (set) Token: 0x0600143B RID: 5179 RVA: 0x00037579 File Offset: 0x00035779
		public Brush EarnedNotSelectedPerkBrush { get; set; }

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x0600143C RID: 5180 RVA: 0x00037582 File Offset: 0x00035782
		// (set) Token: 0x0600143D RID: 5181 RVA: 0x0003758A File Offset: 0x0003578A
		public Brush EarnedActivePerkBrush { get; set; }

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x0600143E RID: 5182 RVA: 0x00037593 File Offset: 0x00035793
		// (set) Token: 0x0600143F RID: 5183 RVA: 0x0003759B File Offset: 0x0003579B
		public Brush EarnedNotActivePerkBrush { get; set; }

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06001440 RID: 5184 RVA: 0x000375A4 File Offset: 0x000357A4
		// (set) Token: 0x06001441 RID: 5185 RVA: 0x000375AC File Offset: 0x000357AC
		public Brush EarnedPreviousPerkNotSelectedPerkBrush { get; set; }

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06001442 RID: 5186 RVA: 0x000375B5 File Offset: 0x000357B5
		// (set) Token: 0x06001443 RID: 5187 RVA: 0x000375BD File Offset: 0x000357BD
		public BrushWidget PerkVisualWidgetParent { get; set; }

		// Token: 0x06001444 RID: 5188 RVA: 0x000375C6 File Offset: 0x000357C6
		public PerkItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x000375D8 File Offset: 0x000357D8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.PerkVisualWidget != null && ((this.PerkVisualWidget.Sprite != null && base.Context.SpriteData.GetSprite(this.PerkVisualWidget.Sprite.Name) == null) || this.PerkVisualWidget.Sprite == null))
			{
				this.PerkVisualWidget.Sprite = base.Context.SpriteData.GetSprite("SPPerks\\locked_fallback");
			}
			if (this._animState == PerkItemButtonWidget.AnimState.Start)
			{
				this._tickCount++;
				if (this._tickCount > 20)
				{
					this._animState = PerkItemButtonWidget.AnimState.Starting;
					return;
				}
			}
			else if (this._animState == PerkItemButtonWidget.AnimState.Starting)
			{
				BrushWidget perkVisualWidgetParent = this.PerkVisualWidgetParent;
				if (perkVisualWidgetParent != null)
				{
					perkVisualWidgetParent.BrushRenderer.RestartAnimation();
				}
				this._animState = PerkItemButtonWidget.AnimState.Playing;
			}
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x000376A0 File Offset: 0x000358A0
		private void SetColorState(bool isActive)
		{
			if (this.PerkVisualWidget != null)
			{
				float num = (isActive ? 1f : 1f);
				float num2 = (isActive ? 1.3f : 0.75f);
				List<BrushWidget> list = base.FindChildrenWithType<BrushWidget>(false);
				for (int i = 0; i < list.Count; i++)
				{
					foreach (Style style in list[i].Brush.Styles)
					{
						for (int j = 0; j < style.LayerCount; j++)
						{
							StyleLayer layer = style.GetLayer(j);
							layer.AlphaFactor = num;
							layer.ColorFactor = num2;
						}
					}
				}
			}
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x0003776C File Offset: 0x0003596C
		protected override void HandleClick()
		{
			base.HandleClick();
			if (this._isSelectable)
			{
				base.Context.TwoDimensionContext.PlaySound("popup");
			}
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x00037794 File Offset: 0x00035994
		private void UpdatePerkStateVisual(int perkState)
		{
			if (this.PerkVisualWidgetParent == null)
			{
				return;
			}
			switch (perkState)
			{
			case 0:
				this.PerkVisualWidgetParent.Brush = this.NotEarnedPerkBrush;
				this._isSelectable = false;
				return;
			case 1:
			{
				this.PerkVisualWidgetParent.Brush = this.EarnedNotSelectedPerkBrush;
				this._animState = PerkItemButtonWidget.AnimState.Start;
				this._isSelectable = true;
				float transitionDuration = this.PerkVisualWidgetParent.Brush.TransitionDuration;
				return;
			}
			case 2:
				this.PerkVisualWidgetParent.Brush = this.EarnedActivePerkBrush;
				this._isSelectable = false;
				return;
			case 3:
				this.PerkVisualWidgetParent.Brush = this.EarnedNotActivePerkBrush;
				this._isSelectable = false;
				return;
			case 4:
				this.PerkVisualWidgetParent.Brush = this.EarnedPreviousPerkNotSelectedPerkBrush;
				this._isSelectable = false;
				return;
			default:
				Debug.FailedAssert("Perk visual state is not defined", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\CharacterDeveloper\\PerkItemButtonWidget.cs", "UpdatePerkStateVisual", 132);
				return;
			}
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06001449 RID: 5193 RVA: 0x00037877 File Offset: 0x00035A77
		// (set) Token: 0x0600144A RID: 5194 RVA: 0x0003787F File Offset: 0x00035A7F
		public int Level
		{
			get
			{
				return this._level;
			}
			set
			{
				if (this._level != value)
				{
					this._level = value;
					base.OnPropertyChanged(value, "Level");
				}
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x0600144B RID: 5195 RVA: 0x0003789D File Offset: 0x00035A9D
		// (set) Token: 0x0600144C RID: 5196 RVA: 0x000378A5 File Offset: 0x00035AA5
		public Widget PerkVisualWidget
		{
			get
			{
				return this._perkVisualWidget;
			}
			set
			{
				if (this._perkVisualWidget != value)
				{
					this._perkVisualWidget = value;
					base.OnPropertyChanged<Widget>(value, "PerkVisualWidget");
				}
			}
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x0600144D RID: 5197 RVA: 0x000378C3 File Offset: 0x00035AC3
		// (set) Token: 0x0600144E RID: 5198 RVA: 0x000378CB File Offset: 0x00035ACB
		public int PerkState
		{
			get
			{
				return this._perkState;
			}
			set
			{
				if (this._perkState != value)
				{
					this._perkState = value;
					base.OnPropertyChanged(value, "PerkState");
					this.UpdatePerkStateVisual(this.PerkState);
				}
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x0600144F RID: 5199 RVA: 0x000378F5 File Offset: 0x00035AF5
		// (set) Token: 0x06001450 RID: 5200 RVA: 0x000378FD File Offset: 0x00035AFD
		public int AlternativeType
		{
			get
			{
				return this._alternativeType;
			}
			set
			{
				if (this._alternativeType != value)
				{
					this._alternativeType = value;
					base.OnPropertyChanged(value, "AlternativeType");
				}
			}
		}

		// Token: 0x0400093B RID: 2363
		private PerkItemButtonWidget.AnimState _animState;

		// Token: 0x0400093C RID: 2364
		private int _tickCount;

		// Token: 0x0400093D RID: 2365
		private bool _isSelectable;

		// Token: 0x0400093E RID: 2366
		private int _level;

		// Token: 0x0400093F RID: 2367
		private int _alternativeType;

		// Token: 0x04000940 RID: 2368
		private int _perkState = -1;

		// Token: 0x04000941 RID: 2369
		private Widget _perkVisualWidget;

		// Token: 0x020001D9 RID: 473
		public enum AnimState
		{
			// Token: 0x04000A86 RID: 2694
			Idle,
			// Token: 0x04000A87 RID: 2695
			Start,
			// Token: 0x04000A88 RID: 2696
			Starting,
			// Token: 0x04000A89 RID: 2697
			Playing
		}
	}
}
