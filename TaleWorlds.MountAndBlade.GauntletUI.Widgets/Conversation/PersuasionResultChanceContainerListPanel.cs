using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Conversation
{
	// Token: 0x02000176 RID: 374
	public class PersuasionResultChanceContainerListPanel : BrushListPanel
	{
		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x060013A6 RID: 5030 RVA: 0x000359BE File Offset: 0x00033BBE
		// (set) Token: 0x060013A7 RID: 5031 RVA: 0x000359C6 File Offset: 0x00033BC6
		public float StayTime { get; set; } = 1f;

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x060013A8 RID: 5032 RVA: 0x000359CF File Offset: 0x00033BCF
		// (set) Token: 0x060013A9 RID: 5033 RVA: 0x000359D7 File Offset: 0x00033BD7
		public Widget CritFailWidget { get; set; }

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x060013AA RID: 5034 RVA: 0x000359E0 File Offset: 0x00033BE0
		// (set) Token: 0x060013AB RID: 5035 RVA: 0x000359E8 File Offset: 0x00033BE8
		public Widget FailWidget { get; set; }

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x060013AC RID: 5036 RVA: 0x000359F1 File Offset: 0x00033BF1
		// (set) Token: 0x060013AD RID: 5037 RVA: 0x000359F9 File Offset: 0x00033BF9
		public Widget SuccessWidget { get; set; }

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x060013AE RID: 5038 RVA: 0x00035A02 File Offset: 0x00033C02
		// (set) Token: 0x060013AF RID: 5039 RVA: 0x00035A0A File Offset: 0x00033C0A
		public Widget CritSuccessWidget { get; set; }

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x060013B0 RID: 5040 RVA: 0x00035A13 File Offset: 0x00033C13
		// (set) Token: 0x060013B1 RID: 5041 RVA: 0x00035A1B File Offset: 0x00033C1B
		public bool IsResultReady { get; set; }

		// Token: 0x060013B2 RID: 5042 RVA: 0x00035A24 File Offset: 0x00033C24
		public PersuasionResultChanceContainerListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x00035A44 File Offset: 0x00033C44
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.IsResultReady)
			{
				if (this._delayStartTime == -1f && base.AlphaFactor <= 0.001f)
				{
					this._delayStartTime = base.EventManager.Time;
				}
				float num = Mathf.Lerp(base.AlphaFactor, 0f, 0.35f);
				this.SetGlobalAlphaRecursively(num);
				Widget resultVisualWidget = this._resultVisualWidget;
				if (resultVisualWidget != null)
				{
					resultVisualWidget.SetGlobalAlphaRecursively(1f);
				}
				if (this._delayStartTime != -1f && base.EventManager.Time - this._delayStartTime > this.StayTime)
				{
					base.EventFired("OnReadyToContinue", Array.Empty<object>());
				}
			}
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x060013B4 RID: 5044 RVA: 0x00035AF8 File Offset: 0x00033CF8
		// (set) Token: 0x060013B5 RID: 5045 RVA: 0x00035B00 File Offset: 0x00033D00
		[Editor(false)]
		public int ResultIndex
		{
			get
			{
				return this._resultIndex;
			}
			set
			{
				if (value != this._resultIndex)
				{
					this._resultIndex = value;
					base.OnPropertyChanged(value, "ResultIndex");
					switch (value)
					{
					case 0:
						this._resultVisualWidget = this.CritFailWidget;
						this.SetState("CriticalFail");
						return;
					case 1:
						this._resultVisualWidget = this.FailWidget;
						this.SetState("Fail");
						return;
					case 2:
						this._resultVisualWidget = this.SuccessWidget;
						this.SetState("Success");
						return;
					case 3:
						this._resultVisualWidget = this.CritSuccessWidget;
						this.SetState("CriticalSuccess");
						break;
					default:
						return;
					}
				}
			}
		}

		// Token: 0x040008F5 RID: 2293
		private Widget _resultVisualWidget;

		// Token: 0x040008F7 RID: 2295
		private float _delayStartTime = -1f;

		// Token: 0x040008F8 RID: 2296
		private int _resultIndex;
	}
}
