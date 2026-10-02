using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map
{
	// Token: 0x0200011B RID: 283
	public class MapIncidentConnectorWidget : Widget
	{
		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06000F06 RID: 3846 RVA: 0x000297C8 File Offset: 0x000279C8
		// (set) Token: 0x06000F07 RID: 3847 RVA: 0x000297D0 File Offset: 0x000279D0
		public Widget OptionsList { get; set; }

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06000F08 RID: 3848 RVA: 0x000297D9 File Offset: 0x000279D9
		// (set) Token: 0x06000F09 RID: 3849 RVA: 0x000297E1 File Offset: 0x000279E1
		public Widget OptionsPanel { get; set; }

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06000F0A RID: 3850 RVA: 0x000297EA File Offset: 0x000279EA
		// (set) Token: 0x06000F0B RID: 3851 RVA: 0x000297F2 File Offset: 0x000279F2
		public Widget OptionsClipRect { get; set; }

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06000F0C RID: 3852 RVA: 0x000297FB File Offset: 0x000279FB
		// (set) Token: 0x06000F0D RID: 3853 RVA: 0x00029803 File Offset: 0x00027A03
		public Widget ConsequencePanel { get; set; }

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06000F0E RID: 3854 RVA: 0x0002980C File Offset: 0x00027A0C
		// (set) Token: 0x06000F0F RID: 3855 RVA: 0x00029814 File Offset: 0x00027A14
		public Widget OptionsLineClipRect { get; set; }

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06000F10 RID: 3856 RVA: 0x0002981D File Offset: 0x00027A1D
		// (set) Token: 0x06000F11 RID: 3857 RVA: 0x00029825 File Offset: 0x00027A25
		public BrushWidget OptionsLine { get; set; }

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06000F12 RID: 3858 RVA: 0x0002982E File Offset: 0x00027A2E
		// (set) Token: 0x06000F13 RID: 3859 RVA: 0x00029836 File Offset: 0x00027A36
		public BrushWidget VerticalLine { get; set; }

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06000F14 RID: 3860 RVA: 0x0002983F File Offset: 0x00027A3F
		// (set) Token: 0x06000F15 RID: 3861 RVA: 0x00029847 File Offset: 0x00027A47
		public BrushWidget ConsequencesLine { get; set; }

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06000F16 RID: 3862 RVA: 0x00029850 File Offset: 0x00027A50
		// (set) Token: 0x06000F17 RID: 3863 RVA: 0x00029858 File Offset: 0x00027A58
		public float LineThickness { get; set; } = 3f;

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06000F18 RID: 3864 RVA: 0x00029861 File Offset: 0x00027A61
		// (set) Token: 0x06000F19 RID: 3865 RVA: 0x00029869 File Offset: 0x00027A69
		public float LineAnimationSpeed { get; set; } = 15f;

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06000F1A RID: 3866 RVA: 0x00029872 File Offset: 0x00027A72
		// (set) Token: 0x06000F1B RID: 3867 RVA: 0x0002987A File Offset: 0x00027A7A
		public float LineMargin { get; set; } = 2f;

		// Token: 0x06000F1C RID: 3868 RVA: 0x00029883 File Offset: 0x00027A83
		public MapIncidentConnectorWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x000298B0 File Offset: 0x00027AB0
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this.OptionsList == null || this.OptionsPanel == null || this.OptionsClipRect == null || this.ConsequencePanel == null || this.OptionsLineClipRect == null || this.OptionsLine == null || this.VerticalLine == null || this.ConsequencesLine == null)
			{
				Debug.FailedAssert("A required widget is null!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Map\\MapIncidentConnectorWidget.cs", "OnUpdate", 35);
				return;
			}
			Widget widget = (base.IsVisible ? this.GetActiveOption() : null);
			if (widget == null)
			{
				this._previousActiveOption = null;
				this._isLerpingOptionsLine = false;
				return;
			}
			if (this._previousActiveOption != widget)
			{
				this._isLerpingOptionsLine = this._previousActiveOption != null;
				this._previousActiveOption = widget;
			}
			this.OptionsLine.SetState(widget.CurrentState);
			this.VerticalLine.SetState(widget.CurrentState);
			this.ConsequencesLine.SetState(widget.CurrentState);
			this.OptionsLine.SuggestedHeight = this.LineThickness;
			this.VerticalLine.SuggestedWidth = this.LineThickness;
			this.ConsequencesLine.SuggestedHeight = this.LineThickness;
			SimpleRectangle boundingBox = this.AreaRect.GetBoundingBox();
			SimpleRectangle boundingBox2 = this.OptionsClipRect.AreaRect.GetBoundingBox();
			SimpleRectangle boundingBox3 = this.ConsequencePanel.AreaRect.GetBoundingBox();
			float num = this.OptionsPanel.AreaRect.GetBoundingBox().X2 - boundingBox.X;
			float num2 = boundingBox2.Y - boundingBox.Y;
			float num3 = boundingBox2.Y2 - boundingBox.Y;
			float num4 = boundingBox3.X - boundingBox.X;
			float num5 = (num + num4 - this.VerticalLine.ScaledSuggestedWidth) * 0.5f;
			float num6 = num5 + this.VerticalLine.ScaledSuggestedWidth;
			float num7 = boundingBox3.GetCenter().Y - boundingBox.Y - this.ConsequencesLine.ScaledSuggestedHeight * 0.5f;
			this.ConsequencesLine.ScaledSuggestedWidth = num4 - num5 - this.LineMargin * base._scaleToUse;
			this.ConsequencesLine.ScaledPositionXOffset = num5;
			this.ConsequencesLine.ScaledPositionYOffset = num7;
			this.OptionsLineClipRect.ScaledSuggestedWidth = num4 - num;
			this.OptionsLineClipRect.ScaledSuggestedHeight = num3 - num2;
			this.OptionsLineClipRect.ScaledPositionXOffset = num;
			this.OptionsLineClipRect.ScaledPositionYOffset = num2;
			float num8 = widget.AreaRect.GetCenter().Y - boundingBox.Y - this.OptionsLine.ScaledSuggestedHeight * 0.5f;
			if (MathF.Abs(num8 - num7) <= 2f * this.LineThickness)
			{
				num8 = num7;
			}
			if (this._isLerpingOptionsLine)
			{
				this._optionsLineTop = MathF.Lerp(this._optionsLineTop, num8, MathF.Min(this.LineAnimationSpeed * dt, 1f), 0.01f);
				this._isLerpingOptionsLine = this._optionsLineTop != num8;
			}
			else
			{
				this._optionsLineTop = num8;
			}
			this.OptionsLine.ScaledSuggestedWidth = num6 - num - this.LineMargin * base._scaleToUse;
			this.OptionsLine.ScaledPositionXOffset = this.LineMargin * base._scaleToUse;
			this.OptionsLine.ScaledPositionYOffset = this._optionsLineTop - num2;
			float num9 = MathF.Min(MathF.Clamp(this._optionsLineTop, num2, num3), num7);
			float num10 = MathF.Max(MathF.Clamp(this._optionsLineTop + this.OptionsLine.ScaledSuggestedHeight, num2, num3), num7 + this.ConsequencesLine.ScaledSuggestedHeight);
			this.VerticalLine.ScaledSuggestedHeight = num10 - num9;
			this.VerticalLine.ScaledPositionXOffset = num5;
			this.VerticalLine.ScaledPositionYOffset = num9;
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x00029C4C File Offset: 0x00027E4C
		private Widget GetActiveOption()
		{
			Widget widget = null;
			for (int i = 0; i < this.OptionsList.ChildCount; i++)
			{
				Widget child = this.OptionsList.GetChild(i);
				if (child.IsHovered)
				{
					return child;
				}
				ButtonWidget buttonWidget;
				if ((buttonWidget = child as ButtonWidget) != null && buttonWidget.IsSelected)
				{
					widget = child;
				}
			}
			return widget;
		}

		// Token: 0x040006E6 RID: 1766
		private Widget _previousActiveOption;

		// Token: 0x040006E7 RID: 1767
		private float _optionsLineTop;

		// Token: 0x040006E8 RID: 1768
		private bool _isLerpingOptionsLine;
	}
}
