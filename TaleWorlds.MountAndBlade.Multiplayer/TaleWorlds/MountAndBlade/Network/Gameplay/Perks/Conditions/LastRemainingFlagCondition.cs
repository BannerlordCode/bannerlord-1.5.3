using System;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Conditions
{
	// Token: 0x0200004D RID: 77
	public class LastRemainingFlagCondition : MPPerkCondition<MissionMultiplayerFlagDomination>
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000285 RID: 645 RVA: 0x0000B25E File Offset: 0x0000945E
		public override MPPerkCondition.PerkEventFlags EventFlags
		{
			get
			{
				return MPPerkCondition.PerkEventFlags.FlagCapture | MPPerkCondition.PerkEventFlags.FlagRemoval;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0000B261 File Offset: 0x00009461
		public override bool IsPeerCondition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000B264 File Offset: 0x00009464
		protected LastRemainingFlagCondition()
		{
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000B26C File Offset: 0x0000946C
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
					XmlAttribute xmlAttribute = attributes["owner"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			this._owner = LastRemainingFlagCondition.FlagOwner.Any;
			if (text2 != null && !Enum.TryParse<LastRemainingFlagCondition.FlagOwner>(text2, true, out this._owner))
			{
				this._owner = LastRemainingFlagCondition.FlagOwner.Any;
				Debug.FailedAssert("provided 'owner' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Conditions\\LastRemainingFlagCondition.cs", "Deserialize", 40);
			}
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000B2D9 File Offset: 0x000094D9
		public override bool Check(MissionPeer peer)
		{
			return this.Check((peer != null) ? peer.ControlledAgent : null);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000B2F0 File Offset: 0x000094F0
		public override bool Check(Agent agent)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			if (agent != null)
			{
				MissionMultiplayerFlagDomination gameModeInstance = base.GameModeInstance;
				LastRemainingFlagCondition.FlagOwner flagOwner = LastRemainingFlagCondition.FlagOwner.None;
				int num = 0;
				foreach (FlagCapturePoint flagCapturePoint in gameModeInstance.AllCapturePoints)
				{
					if (!flagCapturePoint.IsDeactivated)
					{
						num++;
						Team flagOwnerTeam = gameModeInstance.GetFlagOwnerTeam(flagCapturePoint);
						if (flagOwnerTeam == null)
						{
							flagOwner = LastRemainingFlagCondition.FlagOwner.None;
						}
						else if (flagOwnerTeam == agent.Team)
						{
							flagOwner = LastRemainingFlagCondition.FlagOwner.Ally;
						}
						else
						{
							flagOwner = LastRemainingFlagCondition.FlagOwner.Enemy;
						}
					}
				}
				return num == 1 && (this._owner == LastRemainingFlagCondition.FlagOwner.Any || this._owner == flagOwner);
			}
			return false;
		}

		// Token: 0x040000CC RID: 204
		protected static string StringType = "FlagDominationLastRemainingFlag";

		// Token: 0x040000CD RID: 205
		private LastRemainingFlagCondition.FlagOwner _owner;

		// Token: 0x020000AE RID: 174
		private enum FlagOwner
		{
			// Token: 0x040001D2 RID: 466
			Ally,
			// Token: 0x040001D3 RID: 467
			Enemy,
			// Token: 0x040001D4 RID: 468
			None,
			// Token: 0x040001D5 RID: 469
			Any
		}
	}
}
