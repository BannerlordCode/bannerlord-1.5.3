using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x02000065 RID: 101
	public class PartyHealthFillBarWidget : FillBar
	{
		// Token: 0x06000576 RID: 1398 RVA: 0x000106F0 File Offset: 0x0000E8F0
		public PartyHealthFillBarWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0001073C File Offset: 0x0000E93C
		private void HealthUpdated()
		{
			if (this.brushLayer == null)
			{
				this.brushLayer = base.Brush.GetLayer("DefaultFill");
			}
			base.CurrentAmount = (base.InitialAmount = this.Health);
			if (this.IsWounded)
			{
				this.brushLayer.Color = this.WoundedColor;
			}
			else if (this.Health >= this.FullHealthyLimit)
			{
				this.brushLayer.Color = this.FullHealthyColor;
			}
			else
			{
				this.brushLayer.Color = this.HealthyColor;
			}
			if (this.HealthText != null)
			{
				this.HealthText.Text = this.Health + "%";
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x000107F1 File Offset: 0x0000E9F1
		// (set) Token: 0x06000579 RID: 1401 RVA: 0x000107F9 File Offset: 0x0000E9F9
		[Editor(false)]
		public int Health
		{
			get
			{
				return this._health;
			}
			set
			{
				if (this._health != value)
				{
					this._health = value;
					base.OnPropertyChanged(value, "Health");
					this.HealthUpdated();
				}
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x0600057A RID: 1402 RVA: 0x0001081D File Offset: 0x0000EA1D
		// (set) Token: 0x0600057B RID: 1403 RVA: 0x00010825 File Offset: 0x0000EA25
		[Editor(false)]
		public bool IsWounded
		{
			get
			{
				return this._isWounded;
			}
			set
			{
				if (this._isWounded != value)
				{
					this._isWounded = value;
					base.OnPropertyChanged(value, "IsWounded");
					this.HealthUpdated();
				}
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x00010849 File Offset: 0x0000EA49
		// (set) Token: 0x0600057D RID: 1405 RVA: 0x00010851 File Offset: 0x0000EA51
		[Editor(false)]
		public TextWidget HealthText
		{
			get
			{
				return this._healthText;
			}
			set
			{
				if (this._healthText != value)
				{
					this._healthText = value;
					base.OnPropertyChanged<TextWidget>(value, "HealthText");
					this.HealthUpdated();
				}
			}
		}

		// Token: 0x04000251 RID: 593
		private readonly int FullHealthyLimit = 90;

		// Token: 0x04000252 RID: 594
		private readonly Color WoundedColor = Color.FromUint(4290199102U);

		// Token: 0x04000253 RID: 595
		private readonly Color HealthyColor = Color.FromUint(4291732560U);

		// Token: 0x04000254 RID: 596
		private readonly Color FullHealthyColor = Color.FromUint(4284921662U);

		// Token: 0x04000255 RID: 597
		private BrushLayer brushLayer;

		// Token: 0x04000256 RID: 598
		private int _health;

		// Token: 0x04000257 RID: 599
		private bool _isWounded;

		// Token: 0x04000258 RID: 600
		private TextWidget _healthText;
	}
}
