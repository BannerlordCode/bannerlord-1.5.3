using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CharacterDevelopment
{
	// Token: 0x020003C5 RID: 965
	public sealed class FeatObject : PropertyObject
	{
		// Token: 0x17000D4E RID: 3406
		// (get) Token: 0x0600387A RID: 14458 RVA: 0x000EA7BE File Offset: 0x000E89BE
		public static MBReadOnlyList<FeatObject> All
		{
			get
			{
				return Campaign.Current.AllFeats;
			}
		}

		// Token: 0x17000D4F RID: 3407
		// (get) Token: 0x0600387B RID: 14459 RVA: 0x000EA7CA File Offset: 0x000E89CA
		// (set) Token: 0x0600387C RID: 14460 RVA: 0x000EA7D2 File Offset: 0x000E89D2
		public float EffectBonus { get; private set; }

		// Token: 0x17000D50 RID: 3408
		// (get) Token: 0x0600387D RID: 14461 RVA: 0x000EA7DB File Offset: 0x000E89DB
		// (set) Token: 0x0600387E RID: 14462 RVA: 0x000EA7E3 File Offset: 0x000E89E3
		public FeatObject.AdditionType IncrementType { get; private set; }

		// Token: 0x17000D51 RID: 3409
		// (get) Token: 0x0600387F RID: 14463 RVA: 0x000EA7EC File Offset: 0x000E89EC
		// (set) Token: 0x06003880 RID: 14464 RVA: 0x000EA7F4 File Offset: 0x000E89F4
		public bool IsPositive { get; private set; }

		// Token: 0x06003881 RID: 14465 RVA: 0x000EA7FD File Offset: 0x000E89FD
		public FeatObject(string stringId)
			: base(stringId)
		{
		}

		// Token: 0x06003882 RID: 14466 RVA: 0x000EA806 File Offset: 0x000E8A06
		public void Initialize(string name, string description, float effectBonus, bool isPositiveEffect, FeatObject.AdditionType incrementType)
		{
			base.Initialize(new TextObject(name, null), new TextObject(description, null));
			this.EffectBonus = effectBonus;
			this.IncrementType = incrementType;
			this.IsPositive = isPositiveEffect;
			base.AfterInitialized();
		}

		// Token: 0x020007B5 RID: 1973
		public enum AdditionType
		{
			// Token: 0x04001FDD RID: 8157
			Add,
			// Token: 0x04001FDE RID: 8158
			AddFactor
		}
	}
}
