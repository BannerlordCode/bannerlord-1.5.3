using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Kingdom
{
	// Token: 0x02000138 RID: 312
	public class KingdomDecisionOptionWidget : Widget
	{
		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06001048 RID: 4168 RVA: 0x0002D029 File Offset: 0x0002B229
		// (set) Token: 0x06001049 RID: 4169 RVA: 0x0002D031 File Offset: 0x0002B231
		public Widget SealVisualWidget { get; set; }

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x0600104A RID: 4170 RVA: 0x0002D03A File Offset: 0x0002B23A
		// (set) Token: 0x0600104B RID: 4171 RVA: 0x0002D042 File Offset: 0x0002B242
		public DecisionSupportStrengthListPanel StrengthWidget { get; set; }

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x0600104C RID: 4172 RVA: 0x0002D04B File Offset: 0x0002B24B
		// (set) Token: 0x0600104D RID: 4173 RVA: 0x0002D053 File Offset: 0x0002B253
		public bool IsPlayerSupporter { get; set; }

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x0600104E RID: 4174 RVA: 0x0002D05C File Offset: 0x0002B25C
		// (set) Token: 0x0600104F RID: 4175 RVA: 0x0002D064 File Offset: 0x0002B264
		public bool IsAbstain { get; set; }

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06001050 RID: 4176 RVA: 0x0002D06D File Offset: 0x0002B26D
		// (set) Token: 0x06001051 RID: 4177 RVA: 0x0002D075 File Offset: 0x0002B275
		public float SealStartWidth { get; set; } = 232f;

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001052 RID: 4178 RVA: 0x0002D07E File Offset: 0x0002B27E
		// (set) Token: 0x06001053 RID: 4179 RVA: 0x0002D086 File Offset: 0x0002B286
		public float SealStartHeight { get; set; } = 232f;

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001054 RID: 4180 RVA: 0x0002D08F File Offset: 0x0002B28F
		// (set) Token: 0x06001055 RID: 4181 RVA: 0x0002D097 File Offset: 0x0002B297
		public float SealEndWidth { get; set; } = 140f;

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001056 RID: 4182 RVA: 0x0002D0A0 File Offset: 0x0002B2A0
		// (set) Token: 0x06001057 RID: 4183 RVA: 0x0002D0A8 File Offset: 0x0002B2A8
		public float SealEndHeight { get; set; } = 140f;

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06001058 RID: 4184 RVA: 0x0002D0B1 File Offset: 0x0002B2B1
		// (set) Token: 0x06001059 RID: 4185 RVA: 0x0002D0B9 File Offset: 0x0002B2B9
		public float SealAnimLength { get; set; } = 0.2f;

		// Token: 0x0600105A RID: 4186 RVA: 0x0002D0C4 File Offset: 0x0002B2C4
		public KingdomDecisionOptionWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x0002D11C File Offset: 0x0002B31C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this.StrengthWidget.IsVisible = !this.IsAbstain && this.IsPlayerSupporter && this.IsOptionSelected && !this.IsKingsOption && !this._isKingsDecisionDone;
			if (this._animStartTime != -1f && base.EventManager.Time - this._animStartTime < this.SealAnimLength)
			{
				this.SealVisualWidget.IsVisible = true;
				float num = (base.EventManager.Time - this._animStartTime) / this.SealAnimLength;
				this.SealVisualWidget.SuggestedWidth = Mathf.Lerp(this.SealStartWidth, this.SealEndWidth, num);
				this.SealVisualWidget.SuggestedHeight = Mathf.Lerp(this.SealStartHeight, this.SealEndHeight, num);
				this.SealVisualWidget.SetGlobalAlphaRecursively(Mathf.Lerp(0f, 1f, num));
			}
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x0002D20C File Offset: 0x0002B40C
		internal void OnKingsDecisionDone()
		{
			this._isKingsDecisionDone = true;
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x0002D215 File Offset: 0x0002B415
		internal void OnFinalDone()
		{
			this._isKingsDecisionDone = false;
			this._animStartTime = -1f;
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x0002D229 File Offset: 0x0002B429
		private void OnSelectionChange(bool value)
		{
			if (!this.IsPlayerSupporter)
			{
				this.SealVisualWidget.IsVisible = value;
				this.SealVisualWidget.SetGlobalAlphaRecursively(0.2f);
				return;
			}
			this.SealVisualWidget.IsVisible = false;
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x0002D25C File Offset: 0x0002B45C
		private void HandleKingsOption()
		{
			this._animStartTime = base.EventManager.Time;
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06001060 RID: 4192 RVA: 0x0002D26F File Offset: 0x0002B46F
		// (set) Token: 0x06001061 RID: 4193 RVA: 0x0002D277 File Offset: 0x0002B477
		[Editor(false)]
		public bool IsOptionSelected
		{
			get
			{
				return this._isOptionSelected;
			}
			set
			{
				if (this._isOptionSelected != value)
				{
					this._isOptionSelected = value;
					base.OnPropertyChanged(value, "IsOptionSelected");
					this.OnSelectionChange(value);
					base.GamepadNavigationIndex = (value ? (-1) : 0);
				}
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06001062 RID: 4194 RVA: 0x0002D2A9 File Offset: 0x0002B4A9
		// (set) Token: 0x06001063 RID: 4195 RVA: 0x0002D2B1 File Offset: 0x0002B4B1
		[Editor(false)]
		public bool IsKingsOption
		{
			get
			{
				return this._isKingsOption;
			}
			set
			{
				if (this._isKingsOption != value)
				{
					this._isKingsOption = value;
					base.OnPropertyChanged(value, "IsKingsOption");
					this.HandleKingsOption();
				}
			}
		}

		// Token: 0x04000775 RID: 1909
		private float _animStartTime = -1f;

		// Token: 0x04000776 RID: 1910
		private bool _isKingsDecisionDone;

		// Token: 0x04000777 RID: 1911
		private bool _isOptionSelected;

		// Token: 0x04000778 RID: 1912
		public bool _isKingsOption;
	}
}
