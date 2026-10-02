using System;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000096 RID: 150
	public struct AIBehaviorData : IEquatable<AIBehaviorData>
	{
		// Token: 0x060012DD RID: 4829 RVA: 0x00057920 File Offset: 0x00055B20
		public AIBehaviorData(IMapPoint party, AiBehavior aiBehavior, MobileParty.NavigationType navigationType, bool willGatherArmy, bool isFromPort, bool isTargetingPort)
		{
			this.Party = party;
			this.AiBehavior = aiBehavior;
			this.NavigationType = navigationType;
			this.WillGatherArmy = willGatherArmy;
			this.IsFromPort = isFromPort;
			this.IsTargetingPort = isTargetingPort;
			this.Position = CampaignVec2.Zero;
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x0005795A File Offset: 0x00055B5A
		public AIBehaviorData(CampaignVec2 position, AiBehavior aiBehavior, MobileParty.NavigationType navigationType, bool willGatherArmy, bool isFromPort, bool isTargetingPort)
		{
			this.Position = position;
			this.Party = null;
			this.AiBehavior = aiBehavior;
			this.NavigationType = navigationType;
			this.WillGatherArmy = willGatherArmy;
			this.IsFromPort = isFromPort;
			this.IsTargetingPort = isTargetingPort;
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x00057990 File Offset: 0x00055B90
		public override bool Equals(object obj)
		{
			return obj is AIBehaviorData && (AIBehaviorData)obj == this;
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x000579AD File Offset: 0x00055BAD
		public bool Equals(AIBehaviorData other)
		{
			return other == this;
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x000579BC File Offset: 0x00055BBC
		public override int GetHashCode()
		{
			int aiBehavior = (int)this.AiBehavior;
			int num = aiBehavior.GetHashCode();
			num = ((this.Party != null) ? ((num * 397) ^ this.Party.GetHashCode()) : num);
			num = (num * 397) ^ this.WillGatherArmy.GetHashCode();
			num = (num * 397) ^ this.IsTargetingPort.GetHashCode();
			num = (num * 397) ^ this.IsFromPort.GetHashCode();
			num = (num * 397) ^ this.NavigationType.GetHashCode();
			return (num * 397) ^ this.Position.GetHashCode();
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x00057A68 File Offset: 0x00055C68
		public static bool operator ==(AIBehaviorData a, AIBehaviorData b)
		{
			return a.Party == b.Party && a.AiBehavior == b.AiBehavior && a.NavigationType == b.NavigationType && a.WillGatherArmy == b.WillGatherArmy && a.IsFromPort == b.IsFromPort && a.IsTargetingPort == b.IsTargetingPort && a.Position == b.Position;
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x00057ADC File Offset: 0x00055CDC
		public static bool operator !=(AIBehaviorData a, AIBehaviorData b)
		{
			return !(a == b);
		}

		// Token: 0x0400062F RID: 1583
		public static readonly AIBehaviorData Invalid = new AIBehaviorData(null, AiBehavior.None, MobileParty.NavigationType.None, false, false, false);

		// Token: 0x04000630 RID: 1584
		public IMapPoint Party;

		// Token: 0x04000631 RID: 1585
		public CampaignVec2 Position;

		// Token: 0x04000632 RID: 1586
		public AiBehavior AiBehavior;

		// Token: 0x04000633 RID: 1587
		public bool WillGatherArmy;

		// Token: 0x04000634 RID: 1588
		public bool IsFromPort;

		// Token: 0x04000635 RID: 1589
		public bool IsTargetingPort;

		// Token: 0x04000636 RID: 1590
		public MobileParty.NavigationType NavigationType;
	}
}
