using System;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x0200005C RID: 92
	public class IntegerInputPercentageTextWidget : IntegerInputTextWidget
	{
		// Token: 0x06000643 RID: 1603 RVA: 0x0001AE71 File Offset: 0x00019071
		public IntegerInputPercentageTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x0001AE7A File Offset: 0x0001907A
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!base.IsFocused)
			{
				this.SetPercentageText();
			}
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x0001AE91 File Offset: 0x00019091
		protected internal override void OnGainFocus()
		{
			base.OnGainFocus();
			this.SetIntText();
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0001AE9F File Offset: 0x0001909F
		private void SetPercentageText()
		{
			base.Text = this.PercentageText;
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0001AEB0 File Offset: 0x000190B0
		private void SetIntText()
		{
			base.Text = base.IntText.ToString();
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x0001AED1 File Offset: 0x000190D1
		// (set) Token: 0x06000649 RID: 1609 RVA: 0x0001AED9 File Offset: 0x000190D9
		[Editor(false)]
		public string PercentageText
		{
			get
			{
				return this._percentageText;
			}
			set
			{
				if (this._percentageText != value)
				{
					this._percentageText = value;
					base.OnPropertyChanged<string>(value, "PercentageText");
				}
			}
		}

		// Token: 0x040002F8 RID: 760
		private string _percentageText;
	}
}
