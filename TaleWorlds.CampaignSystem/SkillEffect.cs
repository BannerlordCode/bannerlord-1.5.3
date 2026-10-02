using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x020000B1 RID: 177
	public sealed class SkillEffect : PropertyObject
	{
		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x060013EB RID: 5099 RVA: 0x0005DE10 File Offset: 0x0005C010
		public static MBReadOnlyList<SkillEffect> All
		{
			get
			{
				return Campaign.Current.AllSkillEffects;
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x060013EC RID: 5100 RVA: 0x0005DE1C File Offset: 0x0005C01C
		// (set) Token: 0x060013ED RID: 5101 RVA: 0x0005DE24 File Offset: 0x0005C024
		public float Bonus { get; private set; }

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x060013EE RID: 5102 RVA: 0x0005DE2D File Offset: 0x0005C02D
		// (set) Token: 0x060013EF RID: 5103 RVA: 0x0005DE35 File Offset: 0x0005C035
		public float BaseValue { get; private set; }

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x060013F0 RID: 5104 RVA: 0x0005DE3E File Offset: 0x0005C03E
		// (set) Token: 0x060013F1 RID: 5105 RVA: 0x0005DE46 File Offset: 0x0005C046
		public float LimitMin { get; private set; }

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x060013F2 RID: 5106 RVA: 0x0005DE4F File Offset: 0x0005C04F
		// (set) Token: 0x060013F3 RID: 5107 RVA: 0x0005DE57 File Offset: 0x0005C057
		public float LimitMax { get; private set; }

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x060013F4 RID: 5108 RVA: 0x0005DE60 File Offset: 0x0005C060
		// (set) Token: 0x060013F5 RID: 5109 RVA: 0x0005DE68 File Offset: 0x0005C068
		public PartyRole Role { get; private set; }

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x060013F6 RID: 5110 RVA: 0x0005DE71 File Offset: 0x0005C071
		// (set) Token: 0x060013F7 RID: 5111 RVA: 0x0005DE79 File Offset: 0x0005C079
		public EffectIncrementType IncrementType { get; private set; }

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x060013F8 RID: 5112 RVA: 0x0005DE82 File Offset: 0x0005C082
		// (set) Token: 0x060013F9 RID: 5113 RVA: 0x0005DE8A File Offset: 0x0005C08A
		public SkillObject EffectedSkill { get; private set; }

		// Token: 0x060013FA RID: 5114 RVA: 0x0005DE93 File Offset: 0x0005C093
		public SkillEffect(string stringId)
			: base(stringId)
		{
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x0005DE9C File Offset: 0x0005C09C
		public void Initialize(TextObject description, SkillObject effectedSkill, PartyRole role, float bonus, EffectIncrementType incrementType, float baseValue = 0f, float limitMin = -3.4028235E+38f, float limitMax = 3.4028235E+38f)
		{
			base.Initialize(TextObject.GetEmpty(), description);
			this.Role = role;
			this.Bonus = bonus;
			this.IncrementType = incrementType;
			this.EffectedSkill = effectedSkill;
			this.BaseValue = baseValue;
			this.LimitMin = limitMin;
			this.LimitMax = limitMax;
			base.AfterInitialized();
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x0005DEF1 File Offset: 0x0005C0F1
		public float GetSkillEffectValue(int skillLevel)
		{
			return MathF.Clamp(this.BaseValue + this.Bonus * (float)skillLevel, this.LimitMin, this.LimitMax);
		}
	}
}
