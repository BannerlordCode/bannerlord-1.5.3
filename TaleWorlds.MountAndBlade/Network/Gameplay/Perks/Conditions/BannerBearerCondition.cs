using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Conditions
{
	// Token: 0x020003CB RID: 971
	public class BannerBearerCondition : MPPerkCondition
	{
		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x060036C7 RID: 14023 RVA: 0x000E2701 File Offset: 0x000E0901
		public override MPPerkCondition.PerkEventFlags EventFlags
		{
			get
			{
				return MPPerkCondition.PerkEventFlags.AliveBotCountChange | MPPerkCondition.PerkEventFlags.BannerPickUp | MPPerkCondition.PerkEventFlags.BannerDrop | MPPerkCondition.PerkEventFlags.SpawnEnd;
			}
		}

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x060036C8 RID: 14024 RVA: 0x000E2708 File Offset: 0x000E0908
		public override bool IsPeerCondition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060036C9 RID: 14025 RVA: 0x000E270B File Offset: 0x000E090B
		protected BannerBearerCondition()
		{
		}

		// Token: 0x060036CA RID: 14026 RVA: 0x000E2713 File Offset: 0x000E0913
		protected override void Deserialize(XmlNode node)
		{
		}

		// Token: 0x060036CB RID: 14027 RVA: 0x000E2718 File Offset: 0x000E0918
		public override bool Check(MissionPeer peer)
		{
			Formation formation = ((peer != null) ? peer.ControlledFormation : null);
			if (formation != null && MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) > 0)
			{
				using (List<IFormationUnit>.Enumerator enumerator = formation.Arrangement.GetAllUnits().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Agent agent;
						if ((agent = enumerator.Current as Agent) != null && agent.IsActive())
						{
							MissionWeapon missionWeapon = agent.Equipment[EquipmentIndex.ExtraWeaponSlot];
							if (!missionWeapon.IsEmpty && missionWeapon.Item.ItemType == ItemObject.ItemTypeEnum.Banner && new Banner(formation.BannerCode, peer.Team.Color, peer.Team.Color2).Serialize() == missionWeapon.Banner.Serialize())
							{
								return true;
							}
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060036CC RID: 14028 RVA: 0x000E2808 File Offset: 0x000E0A08
		public override bool Check(Agent agent)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			MissionPeer missionPeer = ((agent != null) ? agent.MissionPeer : null) ?? ((agent != null) ? agent.OwningAgentMissionPeer : null);
			return this.Check(missionPeer);
		}

		// Token: 0x04001783 RID: 6019
		protected static string StringType = "BannerBearer";
	}
}
