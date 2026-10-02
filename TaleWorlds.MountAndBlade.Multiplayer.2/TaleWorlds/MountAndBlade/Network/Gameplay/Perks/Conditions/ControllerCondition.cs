using System;
using System.Xml;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Conditions
{
	// Token: 0x02000049 RID: 73
	public class ControllerCondition : MPPerkCondition
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600026C RID: 620 RVA: 0x0000ADFC File Offset: 0x00008FFC
		public override MPPerkCondition.PerkEventFlags EventFlags
		{
			get
			{
				return MPPerkCondition.PerkEventFlags.PeerControlledAgentChange;
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000AE00 File Offset: 0x00009000
		protected ControllerCondition()
		{
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000AE08 File Offset: 0x00009008
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
					XmlAttribute xmlAttribute = attributes["is_player_controlled"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			this._isPlayerControlled = ((text2 != null) ? text2.ToLower() : null) == "true";
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000AE5B File Offset: 0x0000905B
		public override bool Check(MissionPeer peer)
		{
			return this.Check((peer != null) ? peer.ControlledAgent : null);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000AE6F File Offset: 0x0000906F
		public override bool Check(Agent agent)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			return agent != null && agent.IsPlayerControlled == this._isPlayerControlled;
		}

		// Token: 0x040000C3 RID: 195
		protected static string StringType = "Controller";

		// Token: 0x040000C4 RID: 196
		private bool _isPlayerControlled;
	}
}
