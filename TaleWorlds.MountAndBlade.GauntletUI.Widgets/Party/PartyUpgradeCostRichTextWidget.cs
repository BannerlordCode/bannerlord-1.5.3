using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x0200006D RID: 109
	public class PartyUpgradeCostRichTextWidget : RichTextWidget
	{
		// Token: 0x060005FD RID: 1533 RVA: 0x00011CD8 File Offset: 0x0000FED8
		public PartyUpgradeCostRichTextWidget(UIContext context)
			: base(context)
		{
			this.NormalColor = new Color(1f, 1f, 1f, 1f);
			this.InsufficientColor = new Color(0.753f, 0.071f, 0.098f, 1f);
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00011D31 File Offset: 0x0000FF31
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._requiresRefresh)
			{
				base.Brush.FontColor = (this.IsSufficient ? this.NormalColor : this.InsufficientColor);
				this._requiresRefresh = false;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060005FF RID: 1535 RVA: 0x00011D6A File Offset: 0x0000FF6A
		// (set) Token: 0x06000600 RID: 1536 RVA: 0x00011D72 File Offset: 0x0000FF72
		[Editor(false)]
		public bool IsSufficient
		{
			get
			{
				return this._isSufficient;
			}
			set
			{
				if (value != this._isSufficient)
				{
					this._isSufficient = value;
					base.OnPropertyChanged(value, "IsSufficient");
					this._requiresRefresh = true;
				}
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000601 RID: 1537 RVA: 0x00011D97 File Offset: 0x0000FF97
		// (set) Token: 0x06000602 RID: 1538 RVA: 0x00011D9F File Offset: 0x0000FF9F
		public Color NormalColor
		{
			get
			{
				return this._normalColor;
			}
			set
			{
				if (value != this._normalColor)
				{
					this._normalColor = value;
					this._requiresRefresh = true;
				}
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x00011DBD File Offset: 0x0000FFBD
		// (set) Token: 0x06000604 RID: 1540 RVA: 0x00011DC5 File Offset: 0x0000FFC5
		public Color InsufficientColor
		{
			get
			{
				return this._insufficientColor;
			}
			set
			{
				if (value != this._insufficientColor)
				{
					this._insufficientColor = value;
					this._requiresRefresh = true;
				}
			}
		}

		// Token: 0x0400028E RID: 654
		private bool _requiresRefresh = true;

		// Token: 0x0400028F RID: 655
		private bool _isSufficient;

		// Token: 0x04000290 RID: 656
		private Color _normalColor;

		// Token: 0x04000291 RID: 657
		private Color _insufficientColor;
	}
}
