using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Clan
{
	// Token: 0x02000178 RID: 376
	public class ClanFinanceTextWidget : TextWidget
	{
		// Token: 0x060013C6 RID: 5062 RVA: 0x00035ED2 File Offset: 0x000340D2
		public ClanFinanceTextWidget(UIContext context)
			: base(context)
		{
			base.intPropertyChanged += this.IntText_PropertyChanged;
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x00035EED File Offset: 0x000340ED
		private void IntText_PropertyChanged(PropertyOwnerObject widget, string propertyName, int propertyValue)
		{
			if (this.NegativeMarkWidget != null && propertyName == "IntText")
			{
				this.NegativeMarkWidget.IsVisible = propertyValue < 0;
			}
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x00035F14 File Offset: 0x00034114
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.Text != null && base.Text != string.Empty)
			{
				base.Text = MathF.Abs(base.IntText).ToString();
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x060013C9 RID: 5065 RVA: 0x00035F5B File Offset: 0x0003415B
		// (set) Token: 0x060013CA RID: 5066 RVA: 0x00035F63 File Offset: 0x00034163
		[Editor(false)]
		public TextWidget NegativeMarkWidget
		{
			get
			{
				return this._negativeMarkWidget;
			}
			set
			{
				if (this._negativeMarkWidget != value)
				{
					this._negativeMarkWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "NegativeMarkWidget");
				}
			}
		}

		// Token: 0x04000900 RID: 2304
		private TextWidget _negativeMarkWidget;
	}
}
