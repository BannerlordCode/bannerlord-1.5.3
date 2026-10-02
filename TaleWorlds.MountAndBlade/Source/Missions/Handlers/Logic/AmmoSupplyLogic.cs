using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.Source.Missions.Handlers.Logic
{
	// Token: 0x020003E7 RID: 999
	public class AmmoSupplyLogic : MissionLogic
	{
		// Token: 0x06003789 RID: 14217 RVA: 0x000E6F66 File Offset: 0x000E5166
		public AmmoSupplyLogic(List<BattleSideEnum> sideList)
		{
			this._sideList = sideList;
		}

		// Token: 0x0600378A RID: 14218 RVA: 0x000E6F78 File Offset: 0x000E5178
		public bool IsAgentEligibleForAmmoSupply(Agent agent)
		{
			if (agent.IsAIControlled && this._sideList.Contains(agent.Team.Side))
			{
				for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
				{
					if (!agent.Equipment[equipmentIndex].IsEmpty && agent.Equipment[equipmentIndex].IsAnyAmmo())
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600378B RID: 14219 RVA: 0x000E6FE0 File Offset: 0x000E51E0
		public override void OnBehaviorInitialize()
		{
			this._checkTimer = new BasicMissionTimer();
		}

		// Token: 0x0600378C RID: 14220 RVA: 0x000E6FF0 File Offset: 0x000E51F0
		public override void OnMissionTick(float dt)
		{
			if (this._checkTimer.ElapsedTime > 3f)
			{
				this._checkTimer.Reset();
				foreach (Team team in base.Mission.Teams)
				{
					if (this._sideList.IndexOf(team.Side) >= 0)
					{
						foreach (Agent agent in team.ActiveAgents)
						{
							for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
							{
								if (agent.IsAIControlled && !agent.Equipment[equipmentIndex].IsEmpty && agent.Equipment[equipmentIndex].IsAnyAmmo())
								{
									short modifiedMaxAmount = agent.Equipment[equipmentIndex].ModifiedMaxAmount;
									short amount = agent.Equipment[equipmentIndex].Amount;
									short num = modifiedMaxAmount;
									if (modifiedMaxAmount > 1)
									{
										num = modifiedMaxAmount - 1;
									}
									if (amount < num)
									{
										agent.SetWeaponAmountInSlot(equipmentIndex, num, false);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x04001806 RID: 6150
		private const float CheckTimePeriod = 3f;

		// Token: 0x04001807 RID: 6151
		private readonly List<BattleSideEnum> _sideList;

		// Token: 0x04001808 RID: 6152
		private BasicMissionTimer _checkTimer;
	}
}
