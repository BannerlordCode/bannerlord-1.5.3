using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x020000AF RID: 175
	public class MBHaltonColorGenerator
	{
		// Token: 0x17000324 RID: 804
		// (get) Token: 0x0600092E RID: 2350 RVA: 0x0001E1E8 File Offset: 0x0001C3E8
		public int Base
		{
			get
			{
				return this._base;
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x0600092F RID: 2351 RVA: 0x0001E1F0 File Offset: 0x0001C3F0
		public float Offset
		{
			get
			{
				return this._offset;
			}
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0001E1F8 File Offset: 0x0001C3F8
		public MBHaltonColorGenerator()
		{
			this.SetRandomOffset();
			this.SetBase();
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0001E20C File Offset: 0x0001C40C
		public void SetBase()
		{
			this._base = 2;
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0001E215 File Offset: 0x0001C415
		public void SetBase(int baseValue)
		{
			this._base = baseValue;
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0001E21E File Offset: 0x0001C41E
		public void SetOffset(float offset)
		{
			this._offset = MathF.Clamp(offset, 0f, 1f);
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0001E236 File Offset: 0x0001C436
		public void SetRandomOffset()
		{
			this._offset = MBRandom.RandomFloat;
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0001E243 File Offset: 0x0001C443
		public Color GetColor(int index, int maxIndex)
		{
			return Color.FromHSV(MBHaltonColorGenerator.HaltonSequence(((float)index / (float)maxIndex + this._offset) % 1f, this._base), 1f, 1f);
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0001E274 File Offset: 0x0001C474
		private static float HaltonSequence(float normalizedIndex, int baseValue)
		{
			float num = 1f;
			float num2 = 0f;
			for (float num3 = normalizedIndex * (float)baseValue; num3 > 0f; num3 = (float)Math.Floor((double)(num3 / (float)baseValue)))
			{
				num /= (float)baseValue;
				num2 += num3 % (float)baseValue * num;
			}
			return num2;
		}

		// Token: 0x04000520 RID: 1312
		public const int DefaultBase = 2;

		// Token: 0x04000521 RID: 1313
		private int _base;

		// Token: 0x04000522 RID: 1314
		private float _offset;
	}
}
