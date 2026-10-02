using System;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x02000047 RID: 71
	public struct CraftingStatData
	{
		// Token: 0x17000206 RID: 518
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x0001565A File Offset: 0x0001385A
		public bool IsValid
		{
			get
			{
				return this.MaxValue >= 0f;
			}
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x0001566C File Offset: 0x0001386C
		public CraftingStatData(TextObject descriptionText, float curValue, float maxValue, CraftingTemplate.CraftingStatTypes type, DamageTypes damageType = DamageTypes.Invalid)
		{
			this.DescriptionText = descriptionText;
			this.CurValue = curValue;
			this.MaxValue = maxValue;
			this.Type = type;
			this.DamageType = damageType;
		}

		// Token: 0x040002CE RID: 718
		public readonly TextObject DescriptionText;

		// Token: 0x040002CF RID: 719
		public readonly float CurValue;

		// Token: 0x040002D0 RID: 720
		public readonly float MaxValue;

		// Token: 0x040002D1 RID: 721
		public readonly CraftingTemplate.CraftingStatTypes Type;

		// Token: 0x040002D2 RID: 722
		public readonly DamageTypes DamageType;
	}
}
