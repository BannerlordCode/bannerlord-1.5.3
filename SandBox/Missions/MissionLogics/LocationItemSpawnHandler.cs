using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000071 RID: 113
	public class LocationItemSpawnHandler : MissionLogic
	{
		// Token: 0x06000483 RID: 1155 RVA: 0x0001B390 File Offset: 0x00019590
		public override void AfterStart()
		{
			if (CampaignMission.Current.Location != null && CampaignMission.Current.Location.SpecialItems.Count != 0)
			{
				this.SpawnSpecialItems();
			}
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0001B3BC File Offset: 0x000195BC
		private void SpawnSpecialItems()
		{
			this._spawnedEntities = new Dictionary<ItemObject, GameEntity>();
			List<GameEntity> list = base.Mission.Scene.FindEntitiesWithTag("sp_special_item").ToList<GameEntity>();
			foreach (ItemObject itemObject in CampaignMission.Current.Location.SpecialItems)
			{
				if (list.Count != 0)
				{
					MatrixFrame globalFrame = list[0].GetGlobalFrame();
					MissionWeapon missionWeapon = new MissionWeapon(itemObject, null, null);
					GameEntity gameEntity = base.Mission.SpawnWeaponWithNewEntity(ref missionWeapon, Mission.WeaponSpawnFlags.WithStaticPhysics, globalFrame);
					this._spawnedEntities.Add(itemObject, gameEntity);
					list.RemoveAt(0);
				}
			}
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0001B480 File Offset: 0x00019680
		public override void OnEntityRemoved(GameEntity entity)
		{
			if (this._spawnedEntities != null)
			{
				foreach (KeyValuePair<ItemObject, GameEntity> keyValuePair in this._spawnedEntities)
				{
					if (keyValuePair.Value == entity)
					{
						CampaignMission.Current.Location.SpecialItems.Remove(keyValuePair.Key);
					}
				}
			}
		}

		// Token: 0x0400026A RID: 618
		private Dictionary<ItemObject, GameEntity> _spawnedEntities;
	}
}
