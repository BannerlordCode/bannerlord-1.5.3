using System;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Conditions
{
	// Token: 0x02000050 RID: 80
	public class OwnedFlagCountCondition : MPPerkCondition<MissionMultiplayerFlagDomination>
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000B6CD File Offset: 0x000098CD
		public override MPPerkCondition.PerkEventFlags EventFlags
		{
			get
			{
				return MPPerkCondition.PerkEventFlags.FlagCapture | MPPerkCondition.PerkEventFlags.FlagRemoval;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600029A RID: 666 RVA: 0x0000B6D0 File Offset: 0x000098D0
		public override bool IsPeerCondition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000B6D3 File Offset: 0x000098D3
		protected OwnedFlagCountCondition()
		{
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000B6DC File Offset: 0x000098DC
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
				this._min = 0;
			}
			else if (!int.TryParse(text2, out this._min))
			{
				Debug.FailedAssert("provided 'min' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Conditions\\OwnedFlagCountCondition.cs", "Deserialize", 35);
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
				this._max = int.MaxValue;
				return;
			}
			if (!int.TryParse(text4, out this._max))
			{
				Debug.FailedAssert("provided 'max' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Conditions\\OwnedFlagCountCondition.cs", "Deserialize", 45);
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000B7A0 File Offset: 0x000099A0
		public override bool Check(MissionPeer peer)
		{
			return this.Check((peer != null) ? peer.ControlledAgent : null);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000B7B4 File Offset: 0x000099B4
		public override bool Check(Agent agent)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			if (agent != null)
			{
				MissionMultiplayerFlagDomination gameModeInstance = base.GameModeInstance;
				int num = 0;
				foreach (FlagCapturePoint flagCapturePoint in gameModeInstance.AllCapturePoints)
				{
					if (!flagCapturePoint.IsDeactivated && gameModeInstance.GetFlagOwnerTeam(flagCapturePoint) == agent.Team)
					{
						num++;
					}
				}
				return num >= this._min && num <= this._max;
			}
			return false;
		}

		// Token: 0x040000D5 RID: 213
		protected static string StringType = "FlagDominationOwnedFlagCount";

		// Token: 0x040000D6 RID: 214
		private int _min;

		// Token: 0x040000D7 RID: 215
		private int _max;
	}
}
