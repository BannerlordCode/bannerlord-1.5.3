using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Encyclopedia
{
	// Token: 0x02000161 RID: 353
	public class EncyclopediaUnitTreeNodeItemBrushWidget : BrushWidget
	{
		// Token: 0x060012C7 RID: 4807 RVA: 0x00033DA6 File Offset: 0x00031FA6
		public EncyclopediaUnitTreeNodeItemBrushWidget(UIContext context)
			: base(context)
		{
			this._listItemAddedHandler = new Action<Widget, Widget>(this.OnListItemAdded);
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x00033DC4 File Offset: 0x00031FC4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isLinesDirty)
			{
				if (this.ChildContainer.ChildCount == this.LineContainer.ChildCount)
				{
					float num = base.GlobalPosition.X + base.Size.X * 0.5f;
					for (int i = 0; i < this.ChildContainer.ChildCount; i++)
					{
						Widget child = this.ChildContainer.GetChild(i);
						Widget child2 = this.LineContainer.GetChild(i);
						float num2 = child.GlobalPosition.X + child.Size.X * 0.5f;
						bool flag = num > num2;
						child2.SetState(flag ? "Left" : "Right");
						float num3 = MathF.Abs(num - num2);
						child2.ScaledSuggestedWidth = num3;
						child2.ScaledPositionXOffset = (num3 * 0.5f + 5f * base._scaleToUse) * (float)(flag ? (-1) : 1);
					}
				}
				this._isLinesDirty = false;
			}
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x00033ED0 File Offset: 0x000320D0
		public void OnListItemAdded(Widget parentWidget, Widget addedWidget)
		{
			Widget widget = this.CreateLineWidget();
			if (this.ChildContainer.ChildCount == 1)
			{
				widget.SetState("Straight");
				return;
			}
			this._isLinesDirty = true;
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x00033F08 File Offset: 0x00032108
		private Widget CreateLineWidget()
		{
			BrushWidget brushWidget = new BrushWidget(base.Context)
			{
				WidthSizePolicy = SizePolicy.Fixed,
				HeightSizePolicy = SizePolicy.StretchToParent,
				Brush = this.LineBrush
			};
			brushWidget.SuggestedWidth = (float)brushWidget.ReadOnlyBrush.Sprite.Width;
			brushWidget.SuggestedHeight = (float)brushWidget.ReadOnlyBrush.Sprite.Height;
			brushWidget.HorizontalAlignment = HorizontalAlignment.Center;
			brushWidget.AddState("Left");
			brushWidget.AddState("Right");
			brushWidget.AddState("Straight");
			this.LineContainer.AddChild(brushWidget);
			return brushWidget;
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x060012CB RID: 4811 RVA: 0x00033F9E File Offset: 0x0003219E
		// (set) Token: 0x060012CC RID: 4812 RVA: 0x00033FA6 File Offset: 0x000321A6
		[Editor(false)]
		public bool IsAlternativeUpgrade
		{
			get
			{
				return this._isAlternativeUpgrade;
			}
			set
			{
				if (value != this._isAlternativeUpgrade)
				{
					this._isAlternativeUpgrade = value;
					base.OnPropertyChanged(value, "IsAlternativeUpgrade");
				}
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x060012CD RID: 4813 RVA: 0x00033FC4 File Offset: 0x000321C4
		// (set) Token: 0x060012CE RID: 4814 RVA: 0x00033FCC File Offset: 0x000321CC
		[Editor(false)]
		public ListPanel ChildContainer
		{
			get
			{
				return this._childContainer;
			}
			set
			{
				if (this._childContainer != value)
				{
					ListPanel childContainer = this._childContainer;
					if (childContainer != null)
					{
						childContainer.ItemAddEventHandlers.Remove(this._listItemAddedHandler);
					}
					this._childContainer = value;
					base.OnPropertyChanged<ListPanel>(value, "ChildContainer");
					ListPanel childContainer2 = this._childContainer;
					if (childContainer2 == null)
					{
						return;
					}
					childContainer2.ItemAddEventHandlers.Add(this._listItemAddedHandler);
				}
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x060012CF RID: 4815 RVA: 0x0003402D File Offset: 0x0003222D
		// (set) Token: 0x060012D0 RID: 4816 RVA: 0x00034035 File Offset: 0x00032235
		[Editor(false)]
		public Widget LineContainer
		{
			get
			{
				return this._lineContainer;
			}
			set
			{
				if (this._lineContainer != value)
				{
					this._lineContainer = value;
					base.OnPropertyChanged<Widget>(value, "LineContainer");
				}
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x060012D1 RID: 4817 RVA: 0x00034053 File Offset: 0x00032253
		// (set) Token: 0x060012D2 RID: 4818 RVA: 0x0003405B File Offset: 0x0003225B
		[Editor(false)]
		public Brush LineBrush
		{
			get
			{
				return this._lineBrush;
			}
			set
			{
				if (this._lineBrush != value)
				{
					this._lineBrush = value;
					base.OnPropertyChanged<Brush>(value, "LineBrush");
				}
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x060012D3 RID: 4819 RVA: 0x00034079 File Offset: 0x00032279
		// (set) Token: 0x060012D4 RID: 4820 RVA: 0x00034081 File Offset: 0x00032281
		[Editor(false)]
		public Brush AlternateLineBrush
		{
			get
			{
				return this._alternateLineBrush;
			}
			set
			{
				if (this._alternateLineBrush != value)
				{
					this._alternateLineBrush = value;
					base.OnPropertyChanged<Brush>(value, "AlternateLineBrush");
				}
			}
		}

		// Token: 0x0400088F RID: 2191
		private Action<Widget, Widget> _listItemAddedHandler;

		// Token: 0x04000890 RID: 2192
		private bool _isLinesDirty;

		// Token: 0x04000891 RID: 2193
		private bool _isAlternativeUpgrade;

		// Token: 0x04000892 RID: 2194
		private ListPanel _childContainer;

		// Token: 0x04000893 RID: 2195
		private Widget _lineContainer;

		// Token: 0x04000894 RID: 2196
		private Brush _lineBrush;

		// Token: 0x04000895 RID: 2197
		private Brush _alternateLineBrush;
	}
}
