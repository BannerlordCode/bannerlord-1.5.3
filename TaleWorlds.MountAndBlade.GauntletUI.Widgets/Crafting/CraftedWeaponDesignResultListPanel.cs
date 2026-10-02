using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Crafting
{
	// Token: 0x02000167 RID: 359
	public class CraftedWeaponDesignResultListPanel : ListPanel
	{
		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06001302 RID: 4866 RVA: 0x00034698 File Offset: 0x00032898
		// (set) Token: 0x06001303 RID: 4867 RVA: 0x000346A0 File Offset: 0x000328A0
		public CounterTextBrushWidget ChangeValueTextWidget { get; set; }

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06001304 RID: 4868 RVA: 0x000346A9 File Offset: 0x000328A9
		// (set) Token: 0x06001305 RID: 4869 RVA: 0x000346B1 File Offset: 0x000328B1
		public CounterTextBrushWidget ValueTextWidget { get; set; }

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06001306 RID: 4870 RVA: 0x000346BA File Offset: 0x000328BA
		// (set) Token: 0x06001307 RID: 4871 RVA: 0x000346C2 File Offset: 0x000328C2
		public RichTextWidget GoldEffectorTextWidget { get; set; }

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06001308 RID: 4872 RVA: 0x000346CB File Offset: 0x000328CB
		// (set) Token: 0x06001309 RID: 4873 RVA: 0x000346D3 File Offset: 0x000328D3
		public Brush PositiveChangeBrush { get; set; }

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x0600130A RID: 4874 RVA: 0x000346DC File Offset: 0x000328DC
		// (set) Token: 0x0600130B RID: 4875 RVA: 0x000346E4 File Offset: 0x000328E4
		public Brush NegativeChangeBrush { get; set; }

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x0600130C RID: 4876 RVA: 0x000346ED File Offset: 0x000328ED
		// (set) Token: 0x0600130D RID: 4877 RVA: 0x000346F5 File Offset: 0x000328F5
		public Brush NeutralBrush { get; set; }

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x0600130E RID: 4878 RVA: 0x000346FE File Offset: 0x000328FE
		// (set) Token: 0x0600130F RID: 4879 RVA: 0x00034706 File Offset: 0x00032906
		public float FadeInTimeIndexOffset { get; set; } = 2f;

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06001310 RID: 4880 RVA: 0x0003470F File Offset: 0x0003290F
		// (set) Token: 0x06001311 RID: 4881 RVA: 0x00034717 File Offset: 0x00032917
		public float FadeInTime { get; set; } = 0.5f;

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06001312 RID: 4882 RVA: 0x00034720 File Offset: 0x00032920
		// (set) Token: 0x06001313 RID: 4883 RVA: 0x00034728 File Offset: 0x00032928
		public float CounterStartTime { get; set; } = 2f;

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06001314 RID: 4884 RVA: 0x00034731 File Offset: 0x00032931
		private bool _hasChange
		{
			get
			{
				return this.ChangeAmount != 0f;
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06001315 RID: 4885 RVA: 0x00034743 File Offset: 0x00032943
		private float _valueTextStartFadeInTime
		{
			get
			{
				return (float)base.GetSiblingIndex() * this.FadeInTimeIndexOffset;
			}
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x00034753 File Offset: 0x00032953
		public CraftedWeaponDesignResultListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x00034780 File Offset: 0x00032980
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this.ValueTextWidget.FloatTarget = this.InitValue;
				this.ValueTextWidget.ForceSetValue(this.InitValue);
				if (this._hasChange)
				{
					this.ValueTextWidget.Brush = ((this.ChangeAmount > 0f) ? this.PositiveChangeBrush : this.NegativeChangeBrush);
					this.ChangeValueTextWidget.Brush = ((this.ChangeAmount > 0f) ? this.PositiveChangeBrush : this.NegativeChangeBrush);
					this.ChangeValueTextWidget.IsVisible = true;
				}
				else
				{
					this.ChangeValueTextWidget.IsVisible = false;
					this.ValueTextWidget.Brush = this.NeutralBrush;
				}
				this.ChangeValueTextWidget.SetGlobalAlphaRecursively(0f);
				this.ValueTextWidget.SetGlobalAlphaRecursively(0f);
				this.ChangeValueTextWidget.ShowSign = true;
				if (this.InitValue == 0f)
				{
					this.LabelTextWidget.SetState(this._isExceedingBeneficial ? "Bonus" : "Penalty");
				}
				this._initialized = true;
			}
			if (this._totalTime > this._valueTextStartFadeInTime)
			{
				float num = (this._totalTime - this._valueTextStartFadeInTime) / this.FadeInTime;
				if (num >= 0f && num <= 1f)
				{
					float num2 = MathF.Lerp(0f, 1f, num, 1E-05f);
					if (num2 < 1f)
					{
						this.ValueTextWidget.SetGlobalAlphaRecursively(num2);
					}
				}
				if (this._hasChange && this._totalTime > this._valueTextStartFadeInTime + this.CounterStartTime)
				{
					this.ValueTextWidget.FloatTarget = this.InitValue + this.ChangeAmount;
					num = (this._totalTime - this._valueTextStartFadeInTime - this.FadeInTime) / this.FadeInTime;
					if (num >= 0f && num <= 1f)
					{
						float num3 = MathF.Lerp(0f, 1f, num, 1E-05f);
						if (num3 < 1f)
						{
							this.ChangeValueTextWidget.SetGlobalAlphaRecursively(num3);
						}
					}
					this.ChangeValueTextWidget.FloatTarget = this.ChangeAmount;
				}
			}
			this._totalTime += dt;
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06001318 RID: 4888 RVA: 0x000349AC File Offset: 0x00032BAC
		// (set) Token: 0x06001319 RID: 4889 RVA: 0x000349B4 File Offset: 0x00032BB4
		public RichTextWidget LabelTextWidget
		{
			get
			{
				return this._labelTextWidget;
			}
			set
			{
				if (this._labelTextWidget != value)
				{
					this._labelTextWidget = value;
				}
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x0600131A RID: 4890 RVA: 0x000349C6 File Offset: 0x00032BC6
		// (set) Token: 0x0600131B RID: 4891 RVA: 0x000349CE File Offset: 0x00032BCE
		public float InitValue
		{
			get
			{
				return this._initValue;
			}
			set
			{
				if (this._initValue != value)
				{
					this._initValue = value;
				}
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x0600131C RID: 4892 RVA: 0x000349E0 File Offset: 0x00032BE0
		// (set) Token: 0x0600131D RID: 4893 RVA: 0x000349E8 File Offset: 0x00032BE8
		public float ChangeAmount
		{
			get
			{
				return this._changeAmount;
			}
			set
			{
				if (this._changeAmount != value)
				{
					this._changeAmount = value;
				}
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x0600131E RID: 4894 RVA: 0x000349FA File Offset: 0x00032BFA
		// (set) Token: 0x0600131F RID: 4895 RVA: 0x00034A02 File Offset: 0x00032C02
		public bool IsExceedingBeneficial
		{
			get
			{
				return this._isExceedingBeneficial;
			}
			set
			{
				if (value != this._isExceedingBeneficial)
				{
					this._isExceedingBeneficial = value;
				}
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06001320 RID: 4896 RVA: 0x00034A14 File Offset: 0x00032C14
		// (set) Token: 0x06001321 RID: 4897 RVA: 0x00034A1C File Offset: 0x00032C1C
		public float TargetValue
		{
			get
			{
				return this._targetValue;
			}
			set
			{
				if (value != this._targetValue)
				{
					this._targetValue = value;
				}
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06001322 RID: 4898 RVA: 0x00034A2E File Offset: 0x00032C2E
		// (set) Token: 0x06001323 RID: 4899 RVA: 0x00034A36 File Offset: 0x00032C36
		public bool IsOrderResult
		{
			get
			{
				return this._isOrderResult;
			}
			set
			{
				if (value != this._isOrderResult)
				{
					this._isOrderResult = value;
				}
			}
		}

		// Token: 0x040008B1 RID: 2225
		private bool _initialized;

		// Token: 0x040008B2 RID: 2226
		private float _totalTime;

		// Token: 0x040008B3 RID: 2227
		private RichTextWidget _labelTextWidget;

		// Token: 0x040008B4 RID: 2228
		private float _initValue;

		// Token: 0x040008B5 RID: 2229
		private float _changeAmount;

		// Token: 0x040008B6 RID: 2230
		private float _targetValue;

		// Token: 0x040008B7 RID: 2231
		private bool _isExceedingBeneficial;

		// Token: 0x040008B8 RID: 2232
		private bool _isOrderResult;
	}
}
