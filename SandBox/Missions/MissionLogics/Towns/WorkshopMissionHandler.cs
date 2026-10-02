using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Objects.AreaMarkers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics.Towns
{
	// Token: 0x02000090 RID: 144
	public class WorkshopMissionHandler : MissionLogic
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060005B2 RID: 1458 RVA: 0x00025774 File Offset: 0x00023974
		public IEnumerable<Tuple<Workshop, GameEntity>> WorkshopSignEntities
		{
			get
			{
				return this._workshopSignEntities.AsEnumerable<Tuple<Workshop, GameEntity>>();
			}
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00025784 File Offset: 0x00023984
		public WorkshopMissionHandler(Settlement settlement)
		{
			this._settlement = settlement;
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x000257DA File Offset: 0x000239DA
		public override void OnBehaviorInitialize()
		{
			this._workshopSignEntities = new List<Tuple<Workshop, GameEntity>>();
			this._listOfCurrentProps = new List<GameEntity>();
			this._propFrames = new Dictionary<int, Dictionary<string, List<MatrixFrame>>>();
			this._areaMarkers = new List<WorkshopAreaMarker>();
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00025808 File Offset: 0x00023A08
		public override void EarlyStart()
		{
			for (int i = 0; i < this._settlement.Town.Workshops.Length; i++)
			{
				if (!this._settlement.Town.Workshops[i].WorkshopType.IsHidden)
				{
					this._propFrames.Add(i, new Dictionary<string, List<MatrixFrame>>());
					foreach (string text in this._propKinds)
					{
						this._propFrames[i].Add(text, new List<MatrixFrame>());
					}
				}
			}
			List<WorkshopAreaMarker> list = base.Mission.ActiveMissionObjects.FindAllWithType<WorkshopAreaMarker>().ToList<WorkshopAreaMarker>();
			this._areaMarkers = list.FindAll((WorkshopAreaMarker x) => x.GameEntity.HasTag("workshop_area_marker"));
			foreach (WorkshopAreaMarker workshopAreaMarker in this._areaMarkers)
			{
			}
			foreach (GameEntity gameEntity in base.Mission.Scene.FindEntitiesWithTag("shop_prop").ToList<GameEntity>())
			{
				WorkshopAreaMarker workshopAreaMarker2 = this.FindWorkshop(gameEntity);
				if (workshopAreaMarker2 != null && this._propFrames.ContainsKey(workshopAreaMarker2.AreaIndex) && (this._settlement.Town.Workshops[workshopAreaMarker2.AreaIndex] == null || !this._settlement.Town.Workshops[workshopAreaMarker2.AreaIndex].WorkshopType.IsHidden))
				{
					foreach (string text2 in this._propKinds)
					{
						if (gameEntity.HasTag(text2))
						{
							this._propFrames[workshopAreaMarker2.AreaIndex][text2].Add(gameEntity.GetGlobalFrame());
							this._listOfCurrentProps.Add(gameEntity);
							break;
						}
					}
				}
			}
			this.SetBenches();
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00025A30 File Offset: 0x00023C30
		public override void AfterStart()
		{
			this.InitShopSigns();
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00025A38 File Offset: 0x00023C38
		private WorkshopAreaMarker FindWorkshop(GameEntity prop)
		{
			foreach (WorkshopAreaMarker workshopAreaMarker in this._areaMarkers)
			{
				if (workshopAreaMarker.IsPositionInRange(prop.GlobalPosition))
				{
					return workshopAreaMarker;
				}
			}
			return null;
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00025A9C File Offset: 0x00023C9C
		private void SetBenches()
		{
			foreach (GameEntity gameEntity in this._listOfCurrentProps)
			{
				gameEntity.SetVisibilityExcludeParents(false);
				MissionObject missionObject = gameEntity.GetFirstScriptOfType<MissionObject>();
				if (missionObject != null)
				{
					missionObject.SetDisabled(true);
				}
				foreach (GameEntity gameEntity2 in gameEntity.GetChildren())
				{
					missionObject = gameEntity2.GetFirstScriptOfType<UsableMachine>();
					if (missionObject != null)
					{
						missionObject.SetDisabled(true);
					}
				}
			}
			this._listOfCurrentProps.Clear();
			foreach (KeyValuePair<int, Dictionary<string, List<MatrixFrame>>> keyValuePair in this._propFrames)
			{
				int key = keyValuePair.Key;
				foreach (KeyValuePair<string, List<MatrixFrame>> keyValuePair2 in keyValuePair.Value)
				{
					List<string> prefabNames = this.GetPrefabNames(key, keyValuePair2.Key);
					if (prefabNames.Count != 0)
					{
						for (int i = 0; i < keyValuePair2.Value.Count; i++)
						{
							MatrixFrame matrixFrame = keyValuePair2.Value[i];
							this._listOfCurrentProps.Add(GameEntity.Instantiate(base.Mission.Scene, prefabNames[i % prefabNames.Count], matrixFrame, true));
						}
					}
				}
			}
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00025C50 File Offset: 0x00023E50
		private void InitShopSigns()
		{
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign && this._settlement != null && this._settlement.IsTown)
			{
				List<GameEntity> list = base.Mission.Scene.FindEntitiesWithTag("shop_sign").ToList<GameEntity>();
				foreach (WorkshopAreaMarker workshopAreaMarker in base.Mission.ActiveMissionObjects.FindAllWithType<WorkshopAreaMarker>().ToList<WorkshopAreaMarker>())
				{
					Workshop workshop = this._settlement.Town.Workshops[workshopAreaMarker.AreaIndex];
					if (this._workshopSignEntities.All<Tuple<Workshop, GameEntity>>((Tuple<Workshop, GameEntity> x) => x.Item1 != workshop))
					{
						for (int i = 0; i < list.Count; i++)
						{
							GameEntity gameEntity = list[i];
							if (workshopAreaMarker.IsPositionInRange(gameEntity.GlobalPosition))
							{
								this._workshopSignEntities.Add(new Tuple<Workshop, GameEntity>(workshop, gameEntity));
								list.RemoveAt(i);
								break;
							}
						}
					}
				}
				foreach (Tuple<Workshop, GameEntity> tuple in this._workshopSignEntities)
				{
					GameEntity item = tuple.Item2;
					WorkshopType workshopType = tuple.Item1.WorkshopType;
					item.ClearComponents();
					MetaMesh copy = MetaMesh.GetCopy((workshopType != null) ? workshopType.SignMeshName : "shop_sign_merchantavailable", true, false);
					item.AddMultiMesh(copy, true);
				}
			}
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00025E00 File Offset: 0x00024000
		private List<string> GetPrefabNames(int areaIndex, string propKind)
		{
			List<string> list = new List<string>();
			Workshop workshop = this._settlement.Town.Workshops[areaIndex];
			if (workshop.WorkshopType != null)
			{
				if (propKind == this._propKinds[0])
				{
					list.Add(workshop.WorkshopType.PropMeshName1);
				}
				else if (propKind == this._propKinds[1])
				{
					list.Add(workshop.WorkshopType.PropMeshName2);
				}
				else if (propKind == this._propKinds[2])
				{
					list.AddRange(workshop.WorkshopType.PropMeshName3List);
				}
				else if (propKind == this._propKinds[3])
				{
					list.Add(workshop.WorkshopType.PropMeshName4);
				}
				else if (propKind == this._propKinds[4])
				{
					list.Add(workshop.WorkshopType.PropMeshName5);
				}
				else if (propKind == this._propKinds[5])
				{
					list.Add(workshop.WorkshopType.PropMeshName6);
				}
			}
			return list;
		}

		// Token: 0x040002ED RID: 749
		private Settlement _settlement;

		// Token: 0x040002EE RID: 750
		private string[] _propKinds = new string[] { "a", "b", "c", "d", "e", "f" };

		// Token: 0x040002EF RID: 751
		private Dictionary<int, Dictionary<string, List<MatrixFrame>>> _propFrames;

		// Token: 0x040002F0 RID: 752
		private List<GameEntity> _listOfCurrentProps;

		// Token: 0x040002F1 RID: 753
		private List<WorkshopAreaMarker> _areaMarkers;

		// Token: 0x040002F2 RID: 754
		private List<Tuple<Workshop, GameEntity>> _workshopSignEntities;
	}
}
