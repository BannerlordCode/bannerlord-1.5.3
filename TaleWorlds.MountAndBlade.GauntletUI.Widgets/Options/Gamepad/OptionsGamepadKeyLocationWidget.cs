using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Options.Gamepad
{
	// Token: 0x0200007B RID: 123
	public class OptionsGamepadKeyLocationWidget : Widget
	{
		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x00013C07 File Offset: 0x00011E07
		// (set) Token: 0x060006C2 RID: 1730 RVA: 0x00013C0F File Offset: 0x00011E0F
		public bool ForceVisible { get; set; }

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x00013C18 File Offset: 0x00011E18
		// (set) Token: 0x060006C4 RID: 1732 RVA: 0x00013C20 File Offset: 0x00011E20
		public int KeyID { get; set; }

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x00013C29 File Offset: 0x00011E29
		// (set) Token: 0x060006C6 RID: 1734 RVA: 0x00013C31 File Offset: 0x00011E31
		public int NormalPositionXOffset { get; set; }

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x00013C3A File Offset: 0x00011E3A
		// (set) Token: 0x060006C8 RID: 1736 RVA: 0x00013C42 File Offset: 0x00011E42
		public int NormalPositionYOffset { get; set; }

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x00013C4B File Offset: 0x00011E4B
		// (set) Token: 0x060006CA RID: 1738 RVA: 0x00013C53 File Offset: 0x00011E53
		public int NormalSizeXOfImage { get; private set; } = -1;

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x00013C5C File Offset: 0x00011E5C
		// (set) Token: 0x060006CC RID: 1740 RVA: 0x00013C64 File Offset: 0x00011E64
		public int NormalSizeYOfImage { get; private set; } = -1;

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x00013C6D File Offset: 0x00011E6D
		// (set) Token: 0x060006CE RID: 1742 RVA: 0x00013C75 File Offset: 0x00011E75
		public int CurrentSizeXOfImage { get; private set; } = -1;

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x00013C7E File Offset: 0x00011E7E
		// (set) Token: 0x060006D0 RID: 1744 RVA: 0x00013C86 File Offset: 0x00011E86
		public int CurrentSizeYOfImage { get; private set; } = -1;

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x00013C8F File Offset: 0x00011E8F
		// (set) Token: 0x060006D2 RID: 1746 RVA: 0x00013C97 File Offset: 0x00011E97
		public bool IsKeyToTheLeftOfTheGamepad { get; private set; }

		// Token: 0x060006D3 RID: 1747 RVA: 0x00013CA0 File Offset: 0x00011EA0
		public OptionsGamepadKeyLocationWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00013CD0 File Offset: 0x00011ED0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._valuesInitialized)
			{
				this.NormalSizeXOfImage = base.ParentWidget.Sprite.Width;
				this.NormalSizeYOfImage = base.ParentWidget.Sprite.Height;
				this.CurrentSizeXOfImage = (int)(base.ParentWidget.SuggestedWidth * base._scaleToUse);
				this.CurrentSizeYOfImage = (int)(base.ParentWidget.SuggestedHeight * base._scaleToUse);
				this._keyVisualWidget = null;
				this._keyNameTextWidgets.Clear();
				List<Widget> allChildrenRecursive = base.GetAllChildrenRecursive(null);
				for (int i = 0; i < allChildrenRecursive.Count; i++)
				{
					TextWidget textWidget;
					if ((textWidget = allChildrenRecursive[i] as TextWidget) != null)
					{
						this._keyNameTextWidgets.Add(textWidget);
					}
					InputKeyVisualWidget inputKeyVisualWidget;
					if (this._keyVisualWidget == null && (inputKeyVisualWidget = allChildrenRecursive[i] as InputKeyVisualWidget) != null)
					{
						this._keyVisualWidget = inputKeyVisualWidget;
					}
				}
				this._valuesInitialized = true;
				this.IsKeyToTheLeftOfTheGamepad = (float)this.NormalPositionXOffset < (float)this.NormalSizeXOfImage / 2f;
			}
			float num = base.ParentWidget.SuggestedWidth / (float)this.NormalSizeXOfImage;
			float num2 = base.ParentWidget.SuggestedHeight / (float)this.NormalSizeYOfImage;
			base.PositionXOffset = (float)this.NormalPositionXOffset * num;
			base.PositionYOffset = (float)this.NormalPositionYOffset * num2;
			List<TextWidget> keyNameTextWidgets = this._keyNameTextWidgets;
			if (keyNameTextWidgets != null && keyNameTextWidgets.Count == 1)
			{
				this._keyNameTextWidgets[0].Text = this._actionText;
			}
			base.IsVisible = !string.IsNullOrEmpty(this._actionText) || this.ForceVisible;
			if (this._valuesInitialized)
			{
				if (this.IsKeyToTheLeftOfTheGamepad)
				{
					this._keyNameTextWidgets.ForEach(delegate(TextWidget t)
					{
						t.ScaledSuggestedWidth = MathF.Abs(this._parentAreaWidget.GlobalPosition.X - this._keyVisualWidget.GlobalPosition.X);
						t.Brush.TextHorizontalAlignment = TextHorizontalAlignment.Right;
					});
					return;
				}
				this._keyNameTextWidgets.ForEach(delegate(TextWidget t)
				{
					t.ScaledSuggestedWidth = this._parentAreaWidget.GlobalPosition.X + this._parentAreaWidget.Size.X - (this._keyVisualWidget.GlobalPosition.X + this._keyVisualWidget.Size.X);
					t.Brush.TextHorizontalAlignment = TextHorizontalAlignment.Left;
				});
			}
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x00013EAC File Offset: 0x000120AC
		internal void SetKeyProperties(string actionText, Widget parentAreaWidget)
		{
			this._actionText = actionText;
			List<TextWidget> keyNameTextWidgets = this._keyNameTextWidgets;
			if (keyNameTextWidgets != null && keyNameTextWidgets.Count == 1)
			{
				this._keyNameTextWidgets[0].Text = this._actionText;
			}
			this._parentAreaWidget = parentAreaWidget;
			this._valuesInitialized = false;
		}

		// Token: 0x040002EA RID: 746
		private bool _valuesInitialized;

		// Token: 0x040002EB RID: 747
		private string _actionText;

		// Token: 0x040002EC RID: 748
		private Widget _parentAreaWidget;

		// Token: 0x040002ED RID: 749
		private List<TextWidget> _keyNameTextWidgets = new List<TextWidget>();

		// Token: 0x040002EE RID: 750
		private InputKeyVisualWidget _keyVisualWidget;
	}
}
