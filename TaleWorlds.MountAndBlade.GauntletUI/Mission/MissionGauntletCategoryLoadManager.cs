using System;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission
{
	// Token: 0x0200002E RID: 46
	[DefaultView]
	public class MissionGauntletCategoryLoadManager : MissionView, IMissionListener
	{
		// Token: 0x060001E0 RID: 480 RVA: 0x0000B110 File Offset: 0x00009310
		public override void AfterStart()
		{
			base.AfterStart();
			if (this._fullBackgroundCategory == null)
			{
				this._fullBackgroundCategory = UIResourceManager.GetSpriteCategory("ui_fullbackgrounds");
			}
			if (this._encyclopediaCategory == null)
			{
				this._encyclopediaCategory = UIResourceManager.GetSpriteCategory("ui_encyclopedia");
			}
			if (this._mapBarCategory == null)
			{
				SpriteCategory spriteCategory = UIResourceManager.GetSpriteCategory("ui_mapbar");
				if (spriteCategory != null && spriteCategory.IsLoaded)
				{
					this._mapBarCategory = spriteCategory;
				}
			}
			if (this._optionsView == null)
			{
				this._optionsView = base.Mission.GetMissionBehavior<MissionGauntletOptionsUIHandler>();
				base.Mission.AddListener(this);
			}
			this.HandleCategoryLoadingUnloading();
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000B1A3 File Offset: 0x000093A3
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			this._optionsView = null;
			base.Mission.RemoveListener(this);
			this.LoadUnloadAllCategories(true);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000B1C5 File Offset: 0x000093C5
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			this.HandleCategoryLoadingUnloading();
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000B1D4 File Offset: 0x000093D4
		private void HandleCategoryLoadingUnloading()
		{
			bool flag = true;
			if (base.Mission != null)
			{
				flag = this.IsBackgroundsUsedInMission(base.Mission);
			}
			this.LoadUnloadAllCategories(flag);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000B200 File Offset: 0x00009400
		private void LoadUnloadAllCategories(bool load)
		{
			if (load)
			{
				if (!this._fullBackgroundCategory.IsLoaded)
				{
					this._fullBackgroundCategory.Load(UIResourceManager.ResourceContext, UIResourceManager.ResourceDepot);
				}
				if (!this._encyclopediaCategory.IsLoaded)
				{
					this._encyclopediaCategory.Load(UIResourceManager.ResourceContext, UIResourceManager.ResourceDepot);
				}
				SpriteCategory mapBarCategory = this._mapBarCategory;
				if (mapBarCategory != null && !mapBarCategory.IsLoaded)
				{
					this._mapBarCategory.Load(UIResourceManager.ResourceContext, UIResourceManager.ResourceDepot);
					return;
				}
			}
			else
			{
				if (this._fullBackgroundCategory.IsLoaded)
				{
					this._fullBackgroundCategory.Unload();
				}
				if (this._encyclopediaCategory.IsLoaded)
				{
					Mission mission = base.Mission;
					if (mission == null || mission.Mode != MissionMode.Conversation)
					{
						this._encyclopediaCategory.Unload();
					}
				}
				SpriteCategory mapBarCategory2 = this._mapBarCategory;
				if (mapBarCategory2 != null && mapBarCategory2.IsLoaded)
				{
					this._mapBarCategory.Unload();
				}
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000B2EA File Offset: 0x000094EA
		private bool IsBackgroundsUsedInMission(Mission mission)
		{
			return mission.IsInventoryAccessAllowed || mission.IsCharacterWindowAccessAllowed || mission.IsClanWindowAccessAllowed || mission.IsKingdomWindowAccessAllowed || mission.IsQuestScreenAccessAllowed || mission.IsPartyWindowAccessAllowed || mission.IsEncyclopediaWindowAccessAllowed;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000B324 File Offset: 0x00009524
		void IMissionListener.OnEquipItemsFromSpawnEquipmentBegin(Agent agent, Agent.CreationType creationType)
		{
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000B326 File Offset: 0x00009526
		void IMissionListener.OnEquipItemsFromSpawnEquipment(Agent agent, Agent.CreationType creationType)
		{
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000B328 File Offset: 0x00009528
		void IMissionListener.OnEndMission()
		{
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000B32A File Offset: 0x0000952A
		void IMissionListener.OnMissionModeChange(MissionMode oldMissionMode, bool atStart)
		{
			this.HandleCategoryLoadingUnloading();
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000B332 File Offset: 0x00009532
		void IMissionListener.OnConversationCharacterChanged()
		{
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000B334 File Offset: 0x00009534
		void IMissionListener.OnResetMission()
		{
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000B336 File Offset: 0x00009536
		void IMissionListener.OnDeploymentPlanMade(Team team, bool isFirstPlan)
		{
		}

		// Token: 0x040000F5 RID: 245
		private SpriteCategory _fullBackgroundCategory;

		// Token: 0x040000F6 RID: 246
		private SpriteCategory _mapBarCategory;

		// Token: 0x040000F7 RID: 247
		private SpriteCategory _encyclopediaCategory;

		// Token: 0x040000F8 RID: 248
		private MissionGauntletOptionsUIHandler _optionsView;
	}
}
