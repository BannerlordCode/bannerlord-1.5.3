using System;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Conditions
{
	// Token: 0x0200004E RID: 78
	public class MoraleCondition : MPPerkCondition<MissionMultiplayerFlagDomination>
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0000B3BC File Offset: 0x000095BC
		public override MPPerkCondition.PerkEventFlags EventFlags
		{
			get
			{
				return MPPerkCondition.PerkEventFlags.MoraleChange;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600028D RID: 653 RVA: 0x0000B3BF File Offset: 0x000095BF
		public override bool IsPeerCondition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000B3C2 File Offset: 0x000095C2
		protected MoraleCondition()
		{
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000B3CC File Offset: 0x000095CC
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
					XmlAttribute xmlAttribute = attributes["min"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			if (text2 == null)
			{
				this._min = -1f;
			}
			else if (!float.TryParse(text2, out this._min))
			{
				Debug.FailedAssert("provided 'min' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Conditions\\MoraleCondition.cs", "Deserialize", 35);
			}
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
					XmlAttribute xmlAttribute2 = attributes2["max"];
					text3 = ((xmlAttribute2 != null) ? xmlAttribute2.Value : null);
				}
			}
			string text4 = text3;
			if (text4 == null)
			{
				this._max = 1f;
				return;
			}
			if (!float.TryParse(text4, out this._max))
			{
				Debug.FailedAssert("provided 'max' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Conditions\\MoraleCondition.cs", "Deserialize", 45);
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000B494 File Offset: 0x00009694
		public override bool Check(MissionPeer peer)
		{
			return this.Check((peer != null) ? peer.ControlledAgent : null);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000B4A8 File Offset: 0x000096A8
		public override bool Check(Agent agent)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			Team team = ((agent != null) ? agent.Team : null);
			if (team != null)
			{
				MissionMultiplayerFlagDomination gameModeInstance = base.GameModeInstance;
				float num = ((team.Side == BattleSideEnum.Attacker) ? gameModeInstance.MoraleRounded : (-gameModeInstance.MoraleRounded));
				return num >= this._min && num <= this._max;
			}
			return false;
		}

		// Token: 0x040000CE RID: 206
		protected static string StringType = "FlagDominationMorale";

		// Token: 0x040000CF RID: 207
		private float _min;

		// Token: 0x040000D0 RID: 208
		private float _max;
	}
}
