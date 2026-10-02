using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Information.RundownTooltip
{
	// Token: 0x0200014D RID: 333
	public class RundownColumnDividerCollectionWidget : ListPanel
	{
		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x060011DD RID: 4573 RVA: 0x00031DDC File Offset: 0x0002FFDC
		// (set) Token: 0x060011DE RID: 4574 RVA: 0x00031DE4 File Offset: 0x0002FFE4
		public float DividerWidth { get; set; }

		// Token: 0x060011DF RID: 4575 RVA: 0x00031DED File Offset: 0x0002FFED
		public RundownColumnDividerCollectionWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x00031E18 File Offset: 0x00030018
		public void Refresh(IReadOnlyList<float> columnWidths)
		{
			base.RemoveAllChildren();
			for (int i = 0; i < columnWidths.Count - 1; i++)
			{
				Widget widget = this.CreateFixedSpaceWidget(columnWidths[i] * base._inverseScaleToUse - this.DividerWidth);
				base.AddChild(widget);
				base.AddChild(this.CreateDividerWidget());
			}
			base.AddChild(this.CreateStretchedSpaceWidget());
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x00031E79 File Offset: 0x00030079
		private Widget CreateFixedSpaceWidget(float width)
		{
			return new Widget(base.Context)
			{
				WidthSizePolicy = SizePolicy.Fixed,
				HeightSizePolicy = SizePolicy.StretchToParent,
				SuggestedWidth = width
			};
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x00031E9B File Offset: 0x0003009B
		private Widget CreateStretchedSpaceWidget()
		{
			return new Widget(base.Context)
			{
				WidthSizePolicy = SizePolicy.StretchToParent,
				HeightSizePolicy = SizePolicy.StretchToParent
			};
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x00031EB6 File Offset: 0x000300B6
		private Widget CreateDividerWidget()
		{
			return new Widget(base.Context)
			{
				WidthSizePolicy = SizePolicy.Fixed,
				HeightSizePolicy = SizePolicy.StretchToParent,
				SuggestedWidth = this.DividerWidth,
				Sprite = this.DividerSprite,
				Color = this.DividerColor
			};
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x060011E4 RID: 4580 RVA: 0x00031EF5 File Offset: 0x000300F5
		// (set) Token: 0x060011E5 RID: 4581 RVA: 0x00031EFD File Offset: 0x000300FD
		[Editor(false)]
		public Sprite DividerSprite
		{
			get
			{
				return this._dividerSprite;
			}
			set
			{
				if (value != this._dividerSprite)
				{
					this._dividerSprite = value;
					base.OnPropertyChanged<Sprite>(value, "DividerSprite");
				}
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x060011E6 RID: 4582 RVA: 0x00031F1B File Offset: 0x0003011B
		// (set) Token: 0x060011E7 RID: 4583 RVA: 0x00031F23 File Offset: 0x00030123
		[Editor(false)]
		public Color DividerColor
		{
			get
			{
				return this._dividerColor;
			}
			set
			{
				if (value != this._dividerColor)
				{
					this._dividerColor = value;
					base.OnPropertyChanged(value, "DividerColor");
				}
			}
		}

		// Token: 0x04000834 RID: 2100
		private Sprite _dividerSprite;

		// Token: 0x04000835 RID: 2101
		private Color _dividerColor = new Color(1f, 1f, 1f, 1f);
	}
}
