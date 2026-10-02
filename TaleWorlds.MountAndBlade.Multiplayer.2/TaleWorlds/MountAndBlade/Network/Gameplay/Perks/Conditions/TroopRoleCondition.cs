using System;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Conditions
{
	// Token: 0x02000052 RID: 82
	public class TroopRoleCondition : MPPerkCondition
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x0000BA62 File Offset: 0x00009C62
		public override MPPerkCondition.PerkEventFlags EventFlags
		{
			get
			{
				return MPPerkCondition.PerkEventFlags.PeerControlledAgentChange;
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000BA66 File Offset: 0x00009C66
		protected TroopRoleCondition()
		{
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000BA70 File Offset: 0x00009C70
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
					XmlAttribute xmlAttribute = attributes["role"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			this._role = TroopRoleCondition.Role.Sergeant;
			if (text2 != null && !Enum.TryParse<TroopRoleCondition.Role>(text2, true, out this._role))
			{
				this._role = TroopRoleCondition.Role.Sergeant;
				Debug.FailedAssert("provided 'role' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Conditions\\TroopRoleCondition.cs", "Deserialize", 35);
			}
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000BADD File Offset: 0x00009CDD
		public override bool Check(MissionPeer peer)
		{
			return this.Check((peer != null) ? peer.ControlledAgent : null);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000BAF4 File Offset: 0x00009CF4
		public override bool Check(Agent agent)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			if (agent != null && MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) > 0)
			{
				switch (this._role)
				{
				case TroopRoleCondition.Role.Sergeant:
					return this.IsAgentSergeant(agent);
				case TroopRoleCondition.Role.Troop:
					return !this.IsAgentBannerBearer(agent) && !this.IsAgentSergeant(agent);
				case TroopRoleCondition.Role.BannerBearer:
					return this.IsAgentBannerBearer(agent) && !this.IsAgentSergeant(agent);
				}
			}
			return false;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000BB75 File Offset: 0x00009D75
		private bool IsAgentSergeant(Agent agent)
		{
			return agent.Character == MultiplayerClassDivisions.GetMPHeroClassForCharacter(agent.Character).HeroCharacter;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000BB90 File Offset: 0x00009D90
		private bool IsAgentBannerBearer(Agent agent)
		{
			MissionPeer missionPeer = ((agent != null) ? agent.MissionPeer : null) ?? ((agent != null) ? agent.OwningAgentMissionPeer : null);
			Formation formation = ((missionPeer != null) ? missionPeer.ControlledFormation : null);
			if (formation != null)
			{
				MissionWeapon missionWeapon = agent.Equipment[EquipmentIndex.ExtraWeaponSlot];
				if (!missionWeapon.IsEmpty && missionWeapon.Item.ItemType == ItemObject.ItemTypeEnum.Banner && new Banner(formation.BannerCode, missionPeer.Team.Color, missionPeer.Team.Color2).Serialize() == missionWeapon.Banner.Serialize())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040000DC RID: 220
		protected static string StringType = "TroopRole";

		// Token: 0x040000DD RID: 221
		private TroopRoleCondition.Role _role;

		// Token: 0x020000AF RID: 175
		private enum Role
		{
			// Token: 0x040001D7 RID: 471
			Sergeant,
			// Token: 0x040001D8 RID: 472
			Troop,
			// Token: 0x040001D9 RID: 473
			BannerBearer
		}
	}
}
