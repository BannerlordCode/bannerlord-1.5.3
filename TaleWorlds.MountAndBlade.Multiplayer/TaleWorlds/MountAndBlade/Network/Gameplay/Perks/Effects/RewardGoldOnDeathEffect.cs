using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x02000040 RID: 64
	public class RewardGoldOnDeathEffect : MPPerkEffect
	{
		// Token: 0x06000241 RID: 577 RVA: 0x0000A284 File Offset: 0x00008484
		protected RewardGoldOnDeathEffect()
		{
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000A28C File Offset: 0x0000848C
		protected override void Deserialize(XmlNode node)
		{
			string text;
			if (node == null)
			{
				text = null;
			}
			else
			{
				XmlAttributeCollection attributes = node.Attributes;
				if (attributes == null)
				{
					text = null;
				}
				else
				{
					XmlAttribute xmlAttribute = attributes["is_disabled_in_warmup"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			base.IsDisabledInWarmup = ((text2 != null) ? text2.ToLower() : null) == "true";
			string text3;
			if (node == null)
			{
				text3 = null;
			}
			else
			{
				XmlAttributeCollection attributes2 = node.Attributes;
				if (attributes2 == null)
				{
					text3 = null;
				}
				else
				{
					XmlAttribute xmlAttribute2 = attributes2["value"];
					text3 = ((xmlAttribute2 != null) ? xmlAttribute2.Value : null);
				}
			}
			string text4 = text3;
			if (text4 == null || !int.TryParse(text4, out this._value))
			{
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\RewardGoldOnDeathEffect.cs", "Deserialize", 41);
			}
			string text5;
			if (node == null)
			{
				text5 = null;
			}
			else
			{
				XmlAttributeCollection attributes3 = node.Attributes;
				if (attributes3 == null)
				{
					text5 = null;
				}
				else
				{
					XmlAttribute xmlAttribute3 = attributes3["number_of_receivers"];
					text5 = ((xmlAttribute3 != null) ? xmlAttribute3.Value : null);
				}
			}
			string text6 = text5;
			if (text6 == null || !int.TryParse(text6, out this._count))
			{
				Debug.FailedAssert("provided 'number_of_receivers' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\RewardGoldOnDeathEffect.cs", "Deserialize", 47);
			}
			string text7;
			if (node == null)
			{
				text7 = null;
			}
			else
			{
				XmlAttributeCollection attributes4 = node.Attributes;
				if (attributes4 == null)
				{
					text7 = null;
				}
				else
				{
					XmlAttribute xmlAttribute4 = attributes4["order_by"];
					text7 = ((xmlAttribute4 != null) ? xmlAttribute4.Value : null);
				}
			}
			string text8 = text7;
			this._orderBy = RewardGoldOnDeathEffect.OrderBy.Random;
			if (text8 != null && !Enum.TryParse<RewardGoldOnDeathEffect.OrderBy>(text8, true, out this._orderBy))
			{
				this._orderBy = RewardGoldOnDeathEffect.OrderBy.Random;
				Debug.FailedAssert("provided 'order_by' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\RewardGoldOnDeathEffect.cs", "Deserialize", 55);
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000A3E1 File Offset: 0x000085E1
		public override bool GetIsTeamRewardedOnDeath()
		{
			return true;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000A3E4 File Offset: 0x000085E4
		public override void CalculateRewardedGoldOnDeath(Agent agent, List<ValueTuple<MissionPeer, int>> teamMembers)
		{
			Agent agent2 = agent;
			if (((agent2 != null) ? agent2.MissionPeer : null) == null)
			{
				Agent agent3 = agent;
				if (agent3 != null)
				{
					MissionPeer owningAgentMissionPeer = agent3.OwningAgentMissionPeer;
				}
			}
			switch (this._orderBy)
			{
			case RewardGoldOnDeathEffect.OrderBy.WealthAscending:
				teamMembers.Sort((ValueTuple<MissionPeer, int> a, ValueTuple<MissionPeer, int> b) => a.Item1.Representative.Gold.CompareTo(b.Item1.Representative.Gold));
				break;
			case RewardGoldOnDeathEffect.OrderBy.WealthDescending:
				teamMembers.Sort((ValueTuple<MissionPeer, int> a, ValueTuple<MissionPeer, int> b) => b.Item1.Representative.Gold.CompareTo(a.Item1.Representative.Gold));
				break;
			case RewardGoldOnDeathEffect.OrderBy.DistanceAscending:
				teamMembers.Sort((ValueTuple<MissionPeer, int> a, ValueTuple<MissionPeer, int> b) => this.SortByDistance(agent, a.Item1.Representative.ControlledAgent, b.Item1.Representative.ControlledAgent));
				break;
			case RewardGoldOnDeathEffect.OrderBy.DistanceDescending:
				teamMembers.Sort((ValueTuple<MissionPeer, int> a, ValueTuple<MissionPeer, int> b) => this.SortByDistance(agent, b.Item1.Representative.ControlledAgent, a.Item1.Representative.ControlledAgent));
				break;
			}
			int num = this._count;
			int num2 = 0;
			while (num2 < teamMembers.Count && num > 0)
			{
				bool flag;
				if (this._orderBy >= RewardGoldOnDeathEffect.OrderBy.DistanceAscending)
				{
					MissionRepresentativeBase representative = teamMembers[num2].Item1.Representative;
					if (representative == null)
					{
						flag = false;
					}
					else
					{
						Agent controlledAgent = representative.ControlledAgent;
						bool? flag2 = ((controlledAgent != null) ? new bool?(controlledAgent.IsActive()) : null);
						bool flag3 = true;
						flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					ValueTuple<MissionPeer, int> valueTuple = teamMembers[num2];
					valueTuple.Item2 += this._value;
					teamMembers[num2] = valueTuple;
					num--;
				}
				num2++;
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000A568 File Offset: 0x00008768
		private int SortByDistance(Agent from, Agent a, Agent b)
		{
			if (a == null || !a.IsActive())
			{
				if (b != null && b.IsActive())
				{
					return 1;
				}
				return 0;
			}
			else
			{
				if (b == null || !b.IsActive())
				{
					return -1;
				}
				return a.Position.DistanceSquared(from.Position).CompareTo(b.Position.DistanceSquared(from.Position));
			}
		}

		// Token: 0x040000AD RID: 173
		protected static string StringType = "RewardGoldOnDeath";

		// Token: 0x040000AE RID: 174
		private int _value;

		// Token: 0x040000AF RID: 175
		private int _count;

		// Token: 0x040000B0 RID: 176
		private RewardGoldOnDeathEffect.OrderBy _orderBy;

		// Token: 0x020000A6 RID: 166
		private enum OrderBy
		{
			// Token: 0x040001B2 RID: 434
			Random,
			// Token: 0x040001B3 RID: 435
			WealthAscending,
			// Token: 0x040001B4 RID: 436
			WealthDescending,
			// Token: 0x040001B5 RID: 437
			DistanceAscending,
			// Token: 0x040001B6 RID: 438
			DistanceDescending,
			// Token: 0x040001B7 RID: 439
			DeadCanReceiveEnd = 3
		}
	}
}
