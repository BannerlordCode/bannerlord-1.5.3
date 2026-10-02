using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x02000015 RID: 21
	public sealed class BannerEffect : PropertyObject
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x000045ED File Offset: 0x000027ED
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x000045F5 File Offset: 0x000027F5
		public EffectIncrementType IncrementType { get; private set; }

		// Token: 0x060000E6 RID: 230 RVA: 0x000045FE File Offset: 0x000027FE
		public BannerEffect(string stringId)
			: base(stringId)
		{
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004614 File Offset: 0x00002814
		public void Initialize(string name, string description, float level1Bonus, float level2Bonus, float level3Bonus, EffectIncrementType incrementType)
		{
			TextObject textObject = new TextObject(description, null);
			this._levelBonuses[0] = level1Bonus;
			this._levelBonuses[1] = level2Bonus;
			this._levelBonuses[2] = level3Bonus;
			this.IncrementType = incrementType;
			base.Initialize(new TextObject(name, null), textObject);
			base.AfterInitialized();
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004664 File Offset: 0x00002864
		public float GetBonusAtLevel(int bannerLevel)
		{
			int num = bannerLevel - 1;
			num = MBMath.ClampIndex(num, 0, this._levelBonuses.Length);
			return this._levelBonuses[num];
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00004690 File Offset: 0x00002890
		public string GetBonusStringAtLevel(int bannerLevel)
		{
			float bonusAtLevel = this.GetBonusAtLevel(bannerLevel);
			return string.Format("{0:P2}", bonusAtLevel);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000046B8 File Offset: 0x000028B8
		public TextObject GetDescription(int bannerLevel)
		{
			float bonusAtLevel = this.GetBonusAtLevel(bannerLevel);
			if (bonusAtLevel > 0f)
			{
				TextObject textObject = new TextObject("{=Ffwgecvr}{PLUS_OR_MINUS}{BONUSEFFECT}", null);
				textObject.SetTextVariable("BONUSEFFECT", bonusAtLevel, 2);
				textObject.SetTextVariable("PLUS_OR_MINUS", "{=eTw2aNV5}+");
				return base.Description.SetTextVariable("BONUS_AMOUNT", textObject);
			}
			return base.Description.SetTextVariable("BONUS_AMOUNT", bonusAtLevel, 2);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00004724 File Offset: 0x00002924
		public override string ToString()
		{
			return base.Name.ToString();
		}

		// Token: 0x04000116 RID: 278
		private readonly float[] _levelBonuses = new float[3];
	}
}
