using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Conditions
{
	// Token: 0x02000047 RID: 71
	public class AgentStatusCondition : MPPerkCondition
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000ABD5 File Offset: 0x00008DD5
		public override MPPerkCondition.PerkEventFlags EventFlags
		{
			get
			{
				return MPPerkCondition.PerkEventFlags.MountChange;
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000ABDC File Offset: 0x00008DDC
		protected AgentStatusCondition()
		{
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000ABE4 File Offset: 0x00008DE4
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
					XmlAttribute xmlAttribute = attributes["agent_status"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			if (!Enum.TryParse<AgentStatusCondition.AgentStatus>(text, true, out this._status))
			{
				Debug.FailedAssert("provided 'agent_status' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Conditions\\AgentStatusCondition.cs", "Deserialize", 31);
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000AC3E File Offset: 0x00008E3E
		public override bool Check(MissionPeer peer)
		{
			return this.Check((peer != null) ? peer.ControlledAgent : null);
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000AC52 File Offset: 0x00008E52
		public override bool Check(Agent agent)
		{
			if (agent == null)
			{
				return false;
			}
			if (agent.MountAgent == null)
			{
				return this._status == AgentStatusCondition.AgentStatus.OnFoot;
			}
			return this._status == AgentStatusCondition.AgentStatus.OnMount;
		}

		// Token: 0x040000BF RID: 191
		protected static string StringType = "AgentStatus";

		// Token: 0x040000C0 RID: 192
		private AgentStatusCondition.AgentStatus _status;

		// Token: 0x020000AB RID: 171
		private enum AgentStatus
		{
			// Token: 0x040001C6 RID: 454
			OnFoot,
			// Token: 0x040001C7 RID: 455
			OnMount
		}
	}
}
