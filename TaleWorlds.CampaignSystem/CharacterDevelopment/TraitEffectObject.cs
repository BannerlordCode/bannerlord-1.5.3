using System;
using Helpers;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CharacterDevelopment
{
	// Token: 0x020003C8 RID: 968
	public class TraitEffectObject : PropertyObject
	{
		// Token: 0x17000D53 RID: 3411
		// (get) Token: 0x060038E6 RID: 14566 RVA: 0x000EAB13 File Offset: 0x000E8D13
		// (set) Token: 0x060038E7 RID: 14567 RVA: 0x000EAB1B File Offset: 0x000E8D1B
		public TraitObject Trait { get; private set; }

		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x060038E8 RID: 14568 RVA: 0x000EAB24 File Offset: 0x000E8D24
		// (set) Token: 0x060038E9 RID: 14569 RVA: 0x000EAB2C File Offset: 0x000E8D2C
		public EffectIncrementType IncrementType { get; private set; }

		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x060038EA RID: 14570 RVA: 0x000EAB35 File Offset: 0x000E8D35
		// (set) Token: 0x060038EB RID: 14571 RVA: 0x000EAB3D File Offset: 0x000E8D3D
		public bool IsPositive { get; private set; }

		// Token: 0x060038EC RID: 14572 RVA: 0x000EAB46 File Offset: 0x000E8D46
		public TraitEffectObject(string stringId)
			: base(stringId)
		{
		}

		// Token: 0x060038ED RID: 14573 RVA: 0x000EAB50 File Offset: 0x000E8D50
		public void Initialize(string description, TraitObject trait, float[] effectBonuses, bool isPositiveEffect, EffectIncrementType incrementType)
		{
			base.Initialize(new TextObject("{=!}" + base.StringId, null), new TextObject(description, null));
			this.Trait = trait;
			this.IncrementType = incrementType;
			this.IsPositive = isPositiveEffect;
			this._effectBonuses = effectBonuses;
			base.AfterInitialized();
		}

		// Token: 0x060038EE RID: 14574 RVA: 0x000EABA4 File Offset: 0x000E8DA4
		public float GetBonus(int level)
		{
			return this._effectBonuses[Math.Abs(this.Trait.MinValue) + level];
		}

		// Token: 0x060038EF RID: 14575 RVA: 0x000EABBF File Offset: 0x000E8DBF
		public string GetDescription(int level)
		{
			StringHelpers.SetEffectIncrementTypeTextVariable("VALUE", base.Description, this.GetBonus(level), this.IncrementType);
			return base.Description.ToString();
		}

		// Token: 0x04001178 RID: 4472
		private float[] _effectBonuses;
	}
}
