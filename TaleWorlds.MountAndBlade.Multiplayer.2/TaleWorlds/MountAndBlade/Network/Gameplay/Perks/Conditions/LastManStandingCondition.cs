using System;
using System.Xml;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Conditions
{
	// Token: 0x0200004C RID: 76
	public class LastManStandingCondition : MPPerkCondition
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600027F RID: 639 RVA: 0x0000B1AD File Offset: 0x000093AD
		public override MPPerkCondition.PerkEventFlags EventFlags
		{
			get
			{
				return MPPerkCondition.PerkEventFlags.AliveBotCountChange | MPPerkCondition.PerkEventFlags.SpawnEnd;
			}
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000B1B4 File Offset: 0x000093B4
		protected LastManStandingCondition()
		{
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000B1BC File Offset: 0x000093BC
		protected override void Deserialize(XmlNode node)
		{
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000B1BE File Offset: 0x000093BE
		public override bool Check(MissionPeer peer)
		{
			return this.Check((peer != null) ? peer.ControlledAgent : null);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000B1D4 File Offset: 0x000093D4
		public override bool Check(Agent agent)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			MissionPeer missionPeer = ((agent != null) ? agent.MissionPeer : null) ?? ((agent != null) ? agent.OwningAgentMissionPeer : null);
			if (MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) <= 0 || ((missionPeer != null) ? missionPeer.ControlledFormation : null) == null || !agent.IsActive())
			{
				return false;
			}
			if (!agent.IsPlayerControlled)
			{
				return missionPeer.BotsUnderControlAlive == 1;
			}
			return missionPeer.BotsUnderControlAlive == 0;
		}

		// Token: 0x040000CB RID: 203
		protected static string StringType = "LastManStanding";
	}
}
