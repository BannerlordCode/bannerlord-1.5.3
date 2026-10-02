using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.OrderOfBattle
{
	// Token: 0x020000F2 RID: 242
	public class OrderOfBattleHeroDragWidget : Widget
	{
		// Token: 0x06000C7F RID: 3199 RVA: 0x0002239E File Offset: 0x0002059E
		public OrderOfBattleHeroDragWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x000223A8 File Offset: 0x000205A8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!base.IsVisible)
			{
				return;
			}
			if (this._isDirty && this.StackDragWidget != null)
			{
				base.RemoveAllChildren();
				for (int i = 0; i < this.StackCount; i++)
				{
					BrushWidget brushWidget = new BrushWidget(base.Context)
					{
						Brush = this.StackDragWidget.ReadOnlyBrush,
						DoNotAcceptEvents = false,
						SuggestedHeight = this.StackDragWidget.SuggestedHeight,
						SuggestedWidth = this.StackDragWidget.SuggestedWidth,
						ScaledPositionXOffset = (float)(i * 5),
						ScaledPositionYOffset = (float)(i * 5)
					};
					if (i == this.StackCount - 1)
					{
						BrushWidget brushWidget2 = new BrushWidget(brushWidget.Context)
						{
							Brush = base.Context.GetBrush(this.InnerBrushName),
							WidthSizePolicy = SizePolicy.StretchToParent,
							HeightSizePolicy = SizePolicy.StretchToParent,
							MarginBottom = 5f,
							MarginTop = 5f,
							MarginLeft = 5f,
							MarginRight = 5f,
							HorizontalAlignment = HorizontalAlignment.Center,
							VerticalAlignment = VerticalAlignment.Center
						};
						ImageIdentifierWidget imageIdentifierWidget = new ImageIdentifierWidget(brushWidget.Context)
						{
							WidthSizePolicy = SizePolicy.Fixed,
							HeightSizePolicy = SizePolicy.Fixed,
							SuggestedWidth = this.StackThumbnailWidget.SuggestedWidth,
							SuggestedHeight = this.StackThumbnailWidget.SuggestedHeight,
							MarginTop = this.StackThumbnailWidget.MarginTop,
							MarginLeft = this.StackThumbnailWidget.MarginLeft,
							AdditionalArgs = this.StackThumbnailWidget.AdditionalArgs,
							ImageId = this.StackThumbnailWidget.ImageId,
							TextureProviderName = this.StackThumbnailWidget.TextureProviderName
						};
						brushWidget.AddChild(brushWidget2);
						brushWidget.AddChild(imageIdentifierWidget);
					}
					base.AddChild(brushWidget);
				}
				this._isDirty = false;
			}
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x00022578 File Offset: 0x00020778
		private void OnStackCountChanged()
		{
			this._isDirty = true;
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06000C82 RID: 3202 RVA: 0x00022581 File Offset: 0x00020781
		// (set) Token: 0x06000C83 RID: 3203 RVA: 0x00022589 File Offset: 0x00020789
		[Editor(false)]
		public int StackCount
		{
			get
			{
				return this._stackCount;
			}
			set
			{
				if (value != this._stackCount)
				{
					this._stackCount = value;
					base.OnPropertyChanged(value, "StackCount");
					this.OnStackCountChanged();
				}
			}
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06000C84 RID: 3204 RVA: 0x000225AD File Offset: 0x000207AD
		// (set) Token: 0x06000C85 RID: 3205 RVA: 0x000225B5 File Offset: 0x000207B5
		[Editor(false)]
		public BrushWidget StackDragWidget
		{
			get
			{
				return this._stackDragWidget;
			}
			set
			{
				if (value != this._stackDragWidget)
				{
					this._stackDragWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "StackDragWidget");
				}
			}
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06000C86 RID: 3206 RVA: 0x000225D3 File Offset: 0x000207D3
		// (set) Token: 0x06000C87 RID: 3207 RVA: 0x000225DB File Offset: 0x000207DB
		[Editor(false)]
		public ImageIdentifierWidget StackThumbnailWidget
		{
			get
			{
				return this._stackThumbnailWidget;
			}
			set
			{
				if (value != this._stackThumbnailWidget)
				{
					this._stackThumbnailWidget = value;
					base.OnPropertyChanged<ImageIdentifierWidget>(value, "StackThumbnailWidget");
				}
			}
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06000C88 RID: 3208 RVA: 0x000225F9 File Offset: 0x000207F9
		// (set) Token: 0x06000C89 RID: 3209 RVA: 0x00022601 File Offset: 0x00020801
		[Editor(false)]
		public string InnerBrushName
		{
			get
			{
				return this._innerBrushName;
			}
			set
			{
				if (value != this._innerBrushName)
				{
					this._innerBrushName = value;
					base.OnPropertyChanged<string>(value, "InnerBrushName");
				}
			}
		}

		// Token: 0x040005A7 RID: 1447
		private bool _isDirty;

		// Token: 0x040005A8 RID: 1448
		private int _stackCount;

		// Token: 0x040005A9 RID: 1449
		private BrushWidget _stackDragWidget;

		// Token: 0x040005AA RID: 1450
		private ImageIdentifierWidget _stackThumbnailWidget;

		// Token: 0x040005AB RID: 1451
		private string _innerBrushName;
	}
}
