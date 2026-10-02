using System;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Incidents
{
	// Token: 0x02000238 RID: 568
	public class IncidentHint
	{
		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06002239 RID: 8761 RVA: 0x0009831D File Offset: 0x0009651D
		public TextObject Text { get; }

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x0600223A RID: 8762 RVA: 0x00098325 File Offset: 0x00096525
		public IncidentHintType Type { get; }

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x0600223B RID: 8763 RVA: 0x0009832D File Offset: 0x0009652D
		// (set) Token: 0x0600223C RID: 8764 RVA: 0x00098335 File Offset: 0x00096535
		public float Chance { get; private set; } = 1f;

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x0600223D RID: 8765 RVA: 0x0009833E File Offset: 0x0009653E
		public IncidentHint[] Children { get; }

		// Token: 0x0600223E RID: 8766 RVA: 0x00098346 File Offset: 0x00096546
		public IncidentHint(TextObject text, IncidentHintType type = IncidentHintType.Effect)
			: this(text, type, Array.Empty<IncidentHint>())
		{
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x00098355 File Offset: 0x00096555
		public IncidentHint(TextObject text, IncidentHintType type, IncidentHint[] children)
		{
			this.Text = text;
			this.Type = type;
			this.Children = children ?? Array.Empty<IncidentHint>();
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x00098386 File Offset: 0x00096586
		public IncidentHint WithChance(float chance)
		{
			this.Chance = chance;
			return this;
		}
	}
}
