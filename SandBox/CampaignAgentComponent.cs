using System;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.LinQuick;
using TaleWorlds.MountAndBlade;

namespace SandBox
{
	// Token: 0x0200001E RID: 30
	public class CampaignAgentComponent : AgentComponent
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00005E90 File Offset: 0x00004090
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00005E98 File Offset: 0x00004098
		public AgentNavigator AgentNavigator { get; private set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00005EA1 File Offset: 0x000040A1
		public PartyBase OwnerParty
		{
			get
			{
				IAgentOriginBase origin = this.Agent.Origin;
				return (PartyBase)((origin != null) ? origin.BattleCombatant : null);
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00005EBF File Offset: 0x000040BF
		public CampaignAgentComponent(Agent agent)
			: base(agent)
		{
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00005EC8 File Offset: 0x000040C8
		public AgentNavigator CreateAgentNavigator(LocationCharacter locationCharacter)
		{
			this.AgentNavigator = new AgentNavigator(this.Agent, locationCharacter);
			return this.AgentNavigator;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00005EE2 File Offset: 0x000040E2
		public AgentNavigator CreateAgentNavigator()
		{
			this.AgentNavigator = new AgentNavigator(this.Agent);
			return this.AgentNavigator;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00005EFB File Offset: 0x000040FB
		public void OnAgentRemoved(Agent agent)
		{
			AgentNavigator agentNavigator = this.AgentNavigator;
			if (agentNavigator == null)
			{
				return;
			}
			agentNavigator.OnAgentRemoved(agent);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005F0E File Offset: 0x0000410E
		public override void OnTick(float dt)
		{
			if (this.Agent.Mission.AllowAiTicking && this.Agent.IsAIControlled)
			{
				AgentNavigator agentNavigator = this.AgentNavigator;
				if (agentNavigator == null)
				{
					return;
				}
				agentNavigator.Tick(dt, false);
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00005F44 File Offset: 0x00004144
		public override float GetMoraleDecreaseConstant()
		{
			PartyBase ownerParty = this.OwnerParty;
			if (((ownerParty != null) ? ownerParty.MapEvent : null) == null || !this.OwnerParty.MapEvent.IsSiegeAssault)
			{
				return 1f;
			}
			if (this.OwnerParty.MapEvent.AttackerSide.Parties.FindIndexQ<MapEventParty>((MapEventParty p) => p.Party == this.OwnerParty) < 0)
			{
				return 0.5f;
			}
			return 0.33f;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00005FB4 File Offset: 0x000041B4
		public override float GetMoraleAddition()
		{
			float num = 0f;
			PartyBase ownerParty = this.OwnerParty;
			if (((ownerParty != null) ? ownerParty.MapEvent : null) != null)
			{
				float num2;
				float num3;
				this.OwnerParty.MapEvent.GetStrengthsRelativeToParty(this.OwnerParty.Side, out num2, out num3);
				if (this.OwnerParty.IsMobile)
				{
					float num4 = (this.OwnerParty.MobileParty.Morale - 50f) / 2f;
					num += num4;
				}
				float num5 = num2 / (num2 + num3) * 10f - 5f;
				num += num5;
			}
			return num;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00006041 File Offset: 0x00004241
		public override void OnStopUsingGameObject()
		{
			if (this.Agent.IsAIControlled)
			{
				AgentNavigator agentNavigator = this.AgentNavigator;
				if (agentNavigator == null)
				{
					return;
				}
				agentNavigator.OnStopUsingGameObject();
			}
		}
	}
}
