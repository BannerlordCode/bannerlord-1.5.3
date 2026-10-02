using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000225 RID: 549
	public struct FactoredNumber
	{
		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06001FEC RID: 8172 RVA: 0x0006E973 File Offset: 0x0006CB73
		public float ResultNumber
		{
			get
			{
				return MathF.Clamp(this.BaseNumber + this.BaseNumber * this._sumOfFactors, this.LimitMinValue, this.LimitMaxValue);
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06001FED RID: 8173 RVA: 0x0006E99A File Offset: 0x0006CB9A
		// (set) Token: 0x06001FEE RID: 8174 RVA: 0x0006E9A2 File Offset: 0x0006CBA2
		public float BaseNumber { get; private set; }

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06001FEF RID: 8175 RVA: 0x0006E9AB File Offset: 0x0006CBAB
		public float LimitMinValue
		{
			get
			{
				return this._limitMinValue;
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06001FF0 RID: 8176 RVA: 0x0006E9B4 File Offset: 0x0006CBB4
		public float LimitMaxValue
		{
			get
			{
				return this._limitMaxValue;
			}
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x0006E9BD File Offset: 0x0006CBBD
		public FactoredNumber(float baseNumber = 0f)
		{
			this.BaseNumber = baseNumber;
			this._sumOfFactors = 0f;
			this._limitMinValue = float.MinValue;
			this._limitMaxValue = float.MaxValue;
		}

		// Token: 0x06001FF2 RID: 8178 RVA: 0x0006E9E7 File Offset: 0x0006CBE7
		public void Add(float value)
		{
			if (value.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				return;
			}
			this.BaseNumber += value;
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x0006EA0A File Offset: 0x0006CC0A
		public void AddFactor(float value)
		{
			if (value.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				return;
			}
			this._sumOfFactors += value;
		}

		// Token: 0x06001FF4 RID: 8180 RVA: 0x0006EA2D File Offset: 0x0006CC2D
		public void LimitMin(float minValue)
		{
			this._limitMinValue = minValue;
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x0006EA36 File Offset: 0x0006CC36
		public void LimitMax(float maxValue)
		{
			this._limitMaxValue = maxValue;
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x0006EA3F File Offset: 0x0006CC3F
		public void Clamp(float minValue, float maxValue)
		{
			this.LimitMin(minValue);
			this.LimitMax(maxValue);
		}

		// Token: 0x04000B03 RID: 2819
		private float _limitMinValue;

		// Token: 0x04000B04 RID: 2820
		private float _limitMaxValue;

		// Token: 0x04000B05 RID: 2821
		private float _sumOfFactors;
	}
}
