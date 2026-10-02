using System;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Conditions
{
	// Token: 0x0200004A RID: 74
	public class FlagDominationStatusCondition : MPPerkCondition<MissionMultiplayerFlagDomination>
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0000AEA6 File Offset: 0x000090A6
		public override MPPerkCondition.PerkEventFlags EventFlags
		{
			get
			{
				return MPPerkCondition.PerkEventFlags.FlagCapture | MPPerkCondition.PerkEventFlags.FlagRemoval;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000AEA9 File Offset: 0x000090A9
		public override bool IsPeerCondition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000AEAC File Offset: 0x000090AC
		protected FlagDominationStatusCondition()
		{
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000AEB4 File Offset: 0x000090B4
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
					XmlAttribute xmlAttribute = attributes["status"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			this._status = FlagDominationStatusCondition.Status.Tie;
			if (text2 != null && !Enum.TryParse<FlagDominationStatusCondition.Status>(text2, true, out this._status))
			{
				this._status = FlagDominationStatusCondition.Status.Tie;
				Debug.FailedAssert("provided 'status' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Conditions\\FlagDominationStatusCondition.cs", "Deserialize", 39);
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000AF21 File Offset: 0x00009121
		public override bool Check(MissionPeer peer)
		{
			return this.Check((peer != null) ? peer.ControlledAgent : null);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000AF38 File Offset: 0x00009138
		public override bool Check(Agent agent)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			if (agent == null)
			{
				return false;
			}
			MissionMultiplayerFlagDomination gameModeInstance = base.GameModeInstance;
			int num = 0;
			int num2 = 0;
			foreach (FlagCapturePoint flagCapturePoint in gameModeInstance.AllCapturePoints)
			{
				if (!flagCapturePoint.IsDeactivated)
				{
					Team flagOwnerTeam = gameModeInstance.GetFlagOwnerTeam(flagCapturePoint);
					if (flagOwnerTeam == agent.Team)
					{
						num++;
					}
					else if (flagOwnerTeam != null)
					{
						num2++;
					}
				}
			}
			if (this._status == FlagDominationStatusCondition.Status.Winning)
			{
				return num > num2;
			}
			if (this._status != FlagDominationStatusCondition.Status.Losing)
			{
				return num == num2;
			}
			return num2 > num;
		}

		// Token: 0x040000C5 RID: 197
		protected static string StringType = "FlagDominationStatus";

		// Token: 0x040000C6 RID: 198
		private FlagDominationStatusCondition.Status _status;

		// Token: 0x020000AD RID: 173
		private enum Status
		{
			// Token: 0x040001CE RID: 462
			Winning,
			// Token: 0x040001CF RID: 463
			Losing,
			// Token: 0x040001D0 RID: 464
			Tie
		}
	}
}
