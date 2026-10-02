using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Armory
{
	// Token: 0x020000B5 RID: 181
	public class MultiplayerArmoryCosmeticsSectionWidget : Widget
	{
		// Token: 0x06000982 RID: 2434 RVA: 0x0001AF12 File Offset: 0x00019112
		public MultiplayerArmoryCosmeticsSectionWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x0001AF1C File Offset: 0x0001911C
		private void AnimateTauntAssignmentStates(float dt)
		{
			this._tauntAssignmentStateTimer += dt;
			float num;
			if (this._tauntAssignmentStateTimer < this.TauntAssignmentStateAnimationDuration)
			{
				num = this._tauntAssignmentStateTimer / this.TauntAssignmentStateAnimationDuration;
				base.EventManager.AddLateUpdateAction(this, new Action<float>(this.AnimateTauntAssignmentStates), 1);
			}
			else
			{
				num = 1f;
			}
			float num2 = (this.IsTauntAssignmentActive ? 1f : this.TauntAssignmentStateAlpha);
			float num3 = (this.IsTauntAssignmentActive ? this.TauntAssignmentStateAlpha : 1f);
			float num4 = MathF.Lerp(num2, num3, num, 1E-05f);
			this.SetWidgetAlpha(this.TopSectionParent, num4);
			this.SetWidgetAlpha(this.BottomSectionParent, num4);
			this.SetWidgetAlpha(this.SortControlsParent, num4);
			this.SetWidgetAlpha(this.CategorySeparatorWidget, num4);
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0001AFE0 File Offset: 0x000191E0
		private void SetWidgetAlpha(Widget widget, float alpha)
		{
			if (widget != null)
			{
				widget.IsVisible = alpha != 0f;
				widget.SetGlobalAlphaRecursively(alpha);
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x0001AFFD File Offset: 0x000191FD
		// (set) Token: 0x06000986 RID: 2438 RVA: 0x0001B008 File Offset: 0x00019208
		public bool IsTauntAssignmentActive
		{
			get
			{
				return this._isTauntAssignmentActive;
			}
			set
			{
				if (value != this._isTauntAssignmentActive)
				{
					this._isTauntAssignmentActive = value;
					base.OnPropertyChanged(value, "IsTauntAssignmentActive");
					this._tauntAssignmentStateTimer = 0f;
					base.EventManager.AddLateUpdateAction(this, new Action<float>(this.AnimateTauntAssignmentStates), 1);
				}
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000987 RID: 2439 RVA: 0x0001B055 File Offset: 0x00019255
		// (set) Token: 0x06000988 RID: 2440 RVA: 0x0001B05D File Offset: 0x0001925D
		public float TauntAssignmentStateAnimationDuration
		{
			get
			{
				return this._tauntAssignmentStateAnimationDuration;
			}
			set
			{
				if (value != this._tauntAssignmentStateAnimationDuration)
				{
					this._tauntAssignmentStateAnimationDuration = value;
					base.OnPropertyChanged(value, "TauntAssignmentStateAnimationDuration");
				}
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x0001B07B File Offset: 0x0001927B
		// (set) Token: 0x0600098A RID: 2442 RVA: 0x0001B083 File Offset: 0x00019283
		public float TauntAssignmentStateAlpha
		{
			get
			{
				return this._tauntAssignmentStateAlpha;
			}
			set
			{
				if (value != this._tauntAssignmentStateAlpha)
				{
					this._tauntAssignmentStateAlpha = value;
					base.OnPropertyChanged(value, "TauntAssignmentStateAlpha");
				}
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x0001B0A1 File Offset: 0x000192A1
		// (set) Token: 0x0600098C RID: 2444 RVA: 0x0001B0A9 File Offset: 0x000192A9
		public Widget TopSectionParent
		{
			get
			{
				return this._topSectionParent;
			}
			set
			{
				if (value != this._topSectionParent)
				{
					this._topSectionParent = value;
					base.OnPropertyChanged<Widget>(value, "TopSectionParent");
				}
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x0001B0C7 File Offset: 0x000192C7
		// (set) Token: 0x0600098E RID: 2446 RVA: 0x0001B0CF File Offset: 0x000192CF
		public Widget BottomSectionParent
		{
			get
			{
				return this._bottomSectionParent;
			}
			set
			{
				if (value != this._bottomSectionParent)
				{
					this._bottomSectionParent = value;
					base.OnPropertyChanged<Widget>(value, "BottomSectionParent");
				}
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x0600098F RID: 2447 RVA: 0x0001B0ED File Offset: 0x000192ED
		// (set) Token: 0x06000990 RID: 2448 RVA: 0x0001B0F5 File Offset: 0x000192F5
		public Widget SortControlsParent
		{
			get
			{
				return this._sortControlsParent;
			}
			set
			{
				if (value != this._sortControlsParent)
				{
					this._sortControlsParent = value;
					base.OnPropertyChanged<Widget>(value, "SortControlsParent");
				}
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000991 RID: 2449 RVA: 0x0001B113 File Offset: 0x00019313
		// (set) Token: 0x06000992 RID: 2450 RVA: 0x0001B11B File Offset: 0x0001931B
		public Widget CategorySeparatorWidget
		{
			get
			{
				return this._categorySeparatorWidget;
			}
			set
			{
				if (value != this._categorySeparatorWidget)
				{
					this._categorySeparatorWidget = value;
					base.OnPropertyChanged<Widget>(value, "CategorySeparatorWidget");
				}
			}
		}

		// Token: 0x04000448 RID: 1096
		private float _tauntAssignmentStateTimer;

		// Token: 0x04000449 RID: 1097
		private bool _isTauntAssignmentActive;

		// Token: 0x0400044A RID: 1098
		private float _tauntAssignmentStateAnimationDuration;

		// Token: 0x0400044B RID: 1099
		private float _tauntAssignmentStateAlpha;

		// Token: 0x0400044C RID: 1100
		private Widget _topSectionParent;

		// Token: 0x0400044D RID: 1101
		private Widget _bottomSectionParent;

		// Token: 0x0400044E RID: 1102
		private Widget _sortControlsParent;

		// Token: 0x0400044F RID: 1103
		private Widget _categorySeparatorWidget;
	}
}
