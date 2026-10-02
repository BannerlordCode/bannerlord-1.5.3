using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Objects.AreaMarkers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000B0 RID: 176
	public class NotableSpawnPointHandler : MissionLogic
	{
		// Token: 0x0600075B RID: 1883 RVA: 0x00031C6C File Offset: 0x0002FE6C
		public override void OnBehaviorInitialize()
		{
			List<GameEntity> list = Mission.Current.Scene.FindEntitiesWithTag("sp_notables_parent").ToList<GameEntity>();
			Settlement settlement = PlayerEncounter.LocationEncounter.Settlement;
			this._workshopAssignedHeroes = new List<Hero>();
			foreach (Hero hero in settlement.Notables)
			{
				if (hero.IsGangLeader)
				{
					this._gangLeaderNotableCount++;
				}
				else if (hero.IsPreacher)
				{
					this._preacherNotableCount++;
				}
				else if (hero.IsArtisan)
				{
					this._artisanNotableCount++;
				}
				else if (hero.IsRuralNotable || hero.IsHeadman)
				{
					this._ruralNotableCount++;
				}
				else if (hero.IsMerchant)
				{
					this._merchantNotableCount++;
				}
			}
			foreach (GameEntity gameEntity in list.ToList<GameEntity>())
			{
				foreach (GameEntity gameEntity2 in gameEntity.GetChildren())
				{
					this.FindAndSetChild(gameEntity2);
				}
				foreach (WorkshopAreaMarker workshopAreaMarker in (from x in base.Mission.ActiveMissionObjects.FindAllWithType<WorkshopAreaMarker>().ToList<WorkshopAreaMarker>()
					orderby x.AreaIndex
					select x).ToList<WorkshopAreaMarker>())
				{
					if (workshopAreaMarker.IsPositionInRange(gameEntity.GlobalPosition))
					{
						if (workshopAreaMarker.GetWorkshop().Owner.OwnedWorkshops.First<Workshop>((Workshop x) => !x.WorkshopType.IsHidden).Tag == workshopAreaMarker.Tag)
						{
							this.ActivateParentSetInsideWorkshop(workshopAreaMarker);
							list.Remove(gameEntity);
							break;
						}
					}
				}
			}
			foreach (GameEntity gameEntity3 in list)
			{
				foreach (GameEntity gameEntity4 in gameEntity3.GetChildren())
				{
					this.FindAndSetChild(gameEntity4);
				}
				this.ActivateParentSetOutsideWorkshop();
			}
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00031F98 File Offset: 0x00030198
		private void FindAndSetChild(GameEntity childGameEntity)
		{
			if (childGameEntity.HasTag("merchant_notary_talking_set"))
			{
				this._currentMerchantSetGameEntity = childGameEntity;
				return;
			}
			if (childGameEntity.HasTag("preacher_notary_talking_set"))
			{
				this._currentPreacherSetGameEntity = childGameEntity;
				return;
			}
			if (childGameEntity.HasTag("gangleader_sitting_and_talking_with_guards_set"))
			{
				this._currentGangLeaderSetGameEntity = childGameEntity;
				return;
			}
			if (childGameEntity.HasTag("sp_artisan_notary_talking_set"))
			{
				this._currentArtisanSetGameEntity = childGameEntity;
				return;
			}
			if (childGameEntity.HasTag("sp_ruralnotable_notary_talking_set"))
			{
				this._currentRuralNotableSetGameEntity = childGameEntity;
			}
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x00032010 File Offset: 0x00030210
		private void ActivateParentSetInsideWorkshop(WorkshopAreaMarker areaMarker)
		{
			Hero owner = areaMarker.GetWorkshop().Owner;
			if (!this._workshopAssignedHeroes.Contains(owner))
			{
				this._workshopAssignedHeroes.Add(owner);
				if (owner.IsMerchant)
				{
					this.DeactivateAllExcept(this._currentMerchantSetGameEntity);
					this._merchantNotableCount--;
					return;
				}
				if (owner.IsArtisan)
				{
					this.DeactivateAllExcept(this._currentArtisanSetGameEntity);
					this._artisanNotableCount--;
					return;
				}
				if (owner.IsGangLeader)
				{
					this.DeactivateAllExcept(this._currentGangLeaderSetGameEntity);
					this._gangLeaderNotableCount--;
					return;
				}
				if (owner.IsPreacher)
				{
					this.DeactivateAllExcept(this._currentPreacherSetGameEntity);
					this._preacherNotableCount--;
					return;
				}
				if (owner.IsRuralNotable)
				{
					this.DeactivateAllExcept(this._currentRuralNotableSetGameEntity);
					this._ruralNotableCount--;
					return;
				}
			}
			else
			{
				this.DeactivateAll();
			}
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x000320FC File Offset: 0x000302FC
		private void ActivateParentSetOutsideWorkshop()
		{
			if (this._gangLeaderNotableCount > 0)
			{
				this.DeactivateAllExcept(this._currentGangLeaderSetGameEntity);
				this._gangLeaderNotableCount--;
				return;
			}
			if (this._merchantNotableCount > 0)
			{
				this.DeactivateAllExcept(this._currentMerchantSetGameEntity);
				this._merchantNotableCount--;
				return;
			}
			if (this._preacherNotableCount > 0)
			{
				this.DeactivateAllExcept(this._currentPreacherSetGameEntity);
				this._preacherNotableCount--;
				return;
			}
			if (this._artisanNotableCount > 0)
			{
				this.DeactivateAllExcept(this._currentArtisanSetGameEntity);
				this._artisanNotableCount--;
				return;
			}
			if (this._ruralNotableCount > 0)
			{
				this.DeactivateAllExcept(this._currentRuralNotableSetGameEntity);
				this._ruralNotableCount--;
				return;
			}
			this.DeactivateAll();
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x000321C3 File Offset: 0x000303C3
		private void DeactivateAll()
		{
			this.MakeInvisibleAndDeactivate(this._currentGangLeaderSetGameEntity);
			this.MakeInvisibleAndDeactivate(this._currentMerchantSetGameEntity);
			this.MakeInvisibleAndDeactivate(this._currentPreacherSetGameEntity);
			this.MakeInvisibleAndDeactivate(this._currentArtisanSetGameEntity);
			this.MakeInvisibleAndDeactivate(this._currentRuralNotableSetGameEntity);
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x00032204 File Offset: 0x00030404
		private void DeactivateAllExcept(GameEntity gameEntity)
		{
			if (gameEntity != this._currentMerchantSetGameEntity)
			{
				this.MakeInvisibleAndDeactivate(this._currentMerchantSetGameEntity);
			}
			if (gameEntity != this._currentGangLeaderSetGameEntity)
			{
				this.MakeInvisibleAndDeactivate(this._currentGangLeaderSetGameEntity);
			}
			if (gameEntity != this._currentPreacherSetGameEntity)
			{
				this.MakeInvisibleAndDeactivate(this._currentPreacherSetGameEntity);
			}
			if (gameEntity != this._currentArtisanSetGameEntity)
			{
				this.MakeInvisibleAndDeactivate(this._currentArtisanSetGameEntity);
			}
			if (gameEntity != this._currentRuralNotableSetGameEntity)
			{
				this.MakeInvisibleAndDeactivate(this._currentRuralNotableSetGameEntity);
			}
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00032294 File Offset: 0x00030494
		private void MakeInvisibleAndDeactivate(GameEntity gameEntity)
		{
			gameEntity.SetVisibilityExcludeParents(false);
			UsableMachine firstScriptOfType = gameEntity.GetFirstScriptOfType<UsableMachine>();
			if (firstScriptOfType != null)
			{
				firstScriptOfType.Deactivate();
			}
			foreach (GameEntity gameEntity2 in gameEntity.GetChildren())
			{
				this.MakeInvisibleAndDeactivate(gameEntity2);
			}
		}

		// Token: 0x040003E3 RID: 995
		private int _merchantNotableCount;

		// Token: 0x040003E4 RID: 996
		private int _gangLeaderNotableCount;

		// Token: 0x040003E5 RID: 997
		private int _preacherNotableCount;

		// Token: 0x040003E6 RID: 998
		private int _artisanNotableCount;

		// Token: 0x040003E7 RID: 999
		private int _ruralNotableCount;

		// Token: 0x040003E8 RID: 1000
		private GameEntity _currentMerchantSetGameEntity;

		// Token: 0x040003E9 RID: 1001
		private GameEntity _currentPreacherSetGameEntity;

		// Token: 0x040003EA RID: 1002
		private GameEntity _currentGangLeaderSetGameEntity;

		// Token: 0x040003EB RID: 1003
		private GameEntity _currentArtisanSetGameEntity;

		// Token: 0x040003EC RID: 1004
		private GameEntity _currentRuralNotableSetGameEntity;

		// Token: 0x040003ED RID: 1005
		private List<Hero> _workshopAssignedHeroes;
	}
}
