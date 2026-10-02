using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Incidents
{
	// Token: 0x02000236 RID: 566
	public class IncidentEffect
	{
		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06002232 RID: 8754 RVA: 0x0009826D File Offset: 0x0009646D
		// (set) Token: 0x06002233 RID: 8755 RVA: 0x00098275 File Offset: 0x00096475
		public float ChanceToOccur { get; private set; } = 1f;

		// Token: 0x06002234 RID: 8756 RVA: 0x0009827E File Offset: 0x0009647E
		public IncidentEffect(Func<bool> condition, Func<List<TextObject>> consequence, Func<IncidentEffect, IncidentHint> hint)
		{
			this._condition = condition;
			this._consequence = consequence;
			this._hint = hint;
		}

		// Token: 0x06002235 RID: 8757 RVA: 0x000982A6 File Offset: 0x000964A6
		public bool Condition()
		{
			return this._condition == null || this._condition();
		}

		// Token: 0x06002236 RID: 8758 RVA: 0x000982C0 File Offset: 0x000964C0
		public List<TextObject> Consequence()
		{
			List<TextObject> list = new List<TextObject>();
			if (MBRandom.RandomFloat <= this.ChanceToOccur)
			{
				Func<List<TextObject>> consequence = this._consequence;
				list = ((consequence != null) ? consequence() : null);
			}
			return list;
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x000982F4 File Offset: 0x000964F4
		public IncidentHint GetHint()
		{
			Func<IncidentEffect, IncidentHint> hint = this._hint;
			if (hint == null)
			{
				return null;
			}
			return hint(this).WithChance(this.ChanceToOccur);
		}

		// Token: 0x06002238 RID: 8760 RVA: 0x00098313 File Offset: 0x00096513
		public IncidentEffect WithChance(float chance)
		{
			this.ChanceToOccur = chance;
			return this;
		}

		// Token: 0x040009E8 RID: 2536
		private readonly Func<bool> _condition;

		// Token: 0x040009E9 RID: 2537
		private readonly Func<List<TextObject>> _consequence;

		// Token: 0x040009EA RID: 2538
		private readonly Func<IncidentEffect, IncidentHint> _hint;
	}
}
