using System;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Conditions
{
	// Token: 0x02000048 RID: 72
	public class ClosestFlagCondition : MPPerkCondition<MissionMultiplayerFlagDomination>
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000265 RID: 613 RVA: 0x0000AC80 File Offset: 0x00008E80
		public override MPPerkCondition.PerkEventFlags EventFlags
		{
			get
			{
				return MPPerkCondition.PerkEventFlags.FlagCapture | MPPerkCondition.PerkEventFlags.FlagRemoval;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000266 RID: 614 RVA: 0x0000AC83 File Offset: 0x00008E83
		public override bool IsPeerCondition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000AC86 File Offset: 0x00008E86
		protected ClosestFlagCondition()
		{
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000AC90 File Offset: 0x00008E90
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
			this._owner = ClosestFlagCondition.FlagOwner.Any;
			if (text2 != null && !Enum.TryParse<ClosestFlagCondition.FlagOwner>(text2, true, out this._owner))
			{
				this._owner = ClosestFlagCondition.FlagOwner.Any;
				Debug.FailedAssert("provided 'owner' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Conditions\\ClosestFlagCondition.cs", "Deserialize", 40);
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000ACFD File Offset: 0x00008EFD
		public override bool Check(MissionPeer peer)
		{
			return this.Check((peer != null) ? peer.ControlledAgent : null);
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000AD14 File Offset: 0x00008F14
		public override bool Check(Agent agent)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			if (agent != null)
			{
				MissionMultiplayerFlagDomination gameModeInstance = base.GameModeInstance;
				ClosestFlagCondition.FlagOwner flagOwner = ClosestFlagCondition.FlagOwner.None;
				float num = float.MaxValue;
				foreach (FlagCapturePoint flagCapturePoint in gameModeInstance.AllCapturePoints)
				{
					if (!flagCapturePoint.IsDeactivated)
					{
						float num2 = agent.Position.DistanceSquared(flagCapturePoint.Position);
						if (num2 < num)
						{
							num = num2;
							Team flagOwnerTeam = gameModeInstance.GetFlagOwnerTeam(flagCapturePoint);
							if (flagOwnerTeam == null)
							{
								flagOwner = ClosestFlagCondition.FlagOwner.None;
							}
							else if (flagOwnerTeam == agent.Team)
							{
								flagOwner = ClosestFlagCondition.FlagOwner.Ally;
							}
							else
							{
								flagOwner = ClosestFlagCondition.FlagOwner.Enemy;
							}
						}
					}
				}
				return this._owner == ClosestFlagCondition.FlagOwner.Any || this._owner == flagOwner;
			}
			return false;
		}

		// Token: 0x040000C1 RID: 193
		protected static string StringType = "FlagDominationClosestFlag";

		// Token: 0x040000C2 RID: 194
		private ClosestFlagCondition.FlagOwner _owner;

		// Token: 0x020000AC RID: 172
		private enum FlagOwner
		{
			// Token: 0x040001C9 RID: 457
			Ally,
			// Token: 0x040001CA RID: 458
			Enemy,
			// Token: 0x040001CB RID: 459
			None,
			// Token: 0x040001CC RID: 460
			Any
		}
	}
}
