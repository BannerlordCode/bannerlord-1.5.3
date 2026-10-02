using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x02000076 RID: 118
	public class MissionItemContourControllerView : MissionView
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x00021BD4 File Offset: 0x0001FDD4
		private static bool IsAllowedByOption
		{
			get
			{
				return !BannerlordConfig.HideBattleUI || GameNetwork.IsMultiplayer;
			}
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00021BE4 File Offset: 0x0001FDE4
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			this.RemoveContourFromAllItems();
			this._contourItemContainer.ClearRegularContourEntities();
			this._contourItemContainer.ClearAdditionalontourEntities();
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00021C08 File Offset: 0x0001FE08
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (MissionItemContourControllerView.IsAllowedByOption)
			{
				if (Agent.Main != null && base.MissionScreen.InputManager.IsGameKeyDown(5))
				{
					this.RemoveContourFromAllItems();
					this.PopulateContourListWithNearbyItems();
					this.ApplyContourToAllItems();
					this._lastItemQueryTime = base.Mission.CurrentTime;
				}
				else
				{
					this.RemoveContourFromAllItems();
					this._contourItemContainer.ClearRegularContourEntities();
				}
				if (this._isContourAppliedToAllItems)
				{
					float currentTime = base.Mission.CurrentTime;
					if (currentTime - this._lastItemQueryTime > 1f)
					{
						this.RemoveContourFromAllItems();
						this.PopulateContourListWithNearbyItems();
						this._lastItemQueryTime = currentTime;
					}
				}
			}
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00021CAC File Offset: 0x0001FEAC
		public override void OnFocusGained(Agent agent, IFocusable focusableObject, bool isInteractable)
		{
			base.OnFocusGained(agent, focusableObject, isInteractable);
			if (MissionItemContourControllerView.IsAllowedByOption && focusableObject != this._currentFocusedObject && isInteractable)
			{
				this._currentFocusedObject = focusableObject;
				UsableMissionObject usableMissionObject;
				if ((usableMissionObject = focusableObject as UsableMissionObject) != null)
				{
					SpawnedItemEntity spawnedItemEntity;
					if ((spawnedItemEntity = usableMissionObject as SpawnedItemEntity) != null)
					{
						this._focusedGameEntity = GameEntity.CreateFromWeakEntity(spawnedItemEntity.GameEntity);
					}
					else if (!string.IsNullOrEmpty(usableMissionObject.ActionMessage.ToString()) && !string.IsNullOrEmpty(usableMissionObject.DescriptionMessage.ToString()))
					{
						this._focusedGameEntity = GameEntity.CreateFromWeakEntity(usableMissionObject.GameEntity);
					}
					else
					{
						UsableMachine usableMachineFromPoint = this.GetUsableMachineFromPoint(usableMissionObject);
						if (usableMachineFromPoint != null)
						{
							this._focusedGameEntity = GameEntity.CreateFromWeakEntity(usableMachineFromPoint.GameEntity);
						}
					}
				}
				this.AddContourToFocusedItem();
			}
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00021D68 File Offset: 0x0001FF68
		public override void OnFocusLost(Agent agent, IFocusable focusableObject)
		{
			base.OnFocusLost(agent, focusableObject);
			if (MissionItemContourControllerView.IsAllowedByOption)
			{
				if (this._focusedGameEntity != null && this._isContourAppliedToFocusedItem)
				{
					this.RemoveContourFromItem(this._focusedGameEntity);
					this._isContourAppliedToFocusedItem = false;
				}
				this._currentFocusedObject = null;
				this._focusedGameEntity = null;
			}
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00021DBB File Offset: 0x0001FFBB
		public void AddAdditionalContourEntity(WeakGameEntity entity)
		{
			this._contourItemContainer.AddAdditionalContourEntity(entity);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00021DC9 File Offset: 0x0001FFC9
		public void RemoveAdditionalContourEntity(WeakGameEntity entity)
		{
			this.RemoveContourFromItem(entity);
			this._contourItemContainer.RemoveAdditionalContourEntity(entity);
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00021DE0 File Offset: 0x0001FFE0
		private void PopulateContourListWithNearbyItems()
		{
			this._contourItemContainer.ClearRegularContourEntities();
			float num = (GameNetwork.IsSessionActive ? 1f : 3f);
			Agent main = Agent.Main;
			float num2 = main.GetMaximumForwardUnlimitedSpeed() * num;
			Vec3 vec = main.Position - new Vec3(num2, num2, 1f, -1f);
			Vec3 vec2 = main.Position + new Vec3(num2, num2, 2.5f, -1f);
			Vec3 position = base.MissionScreen.CombatCamera.Position;
			Vec3 position2 = main.Position;
			float num3 = new Vec3(position.x, position.y, 0f, -1f).Distance(new Vec3(position2.x, position2.y, 0f, -1f));
			Vec3 vec3 = position * (1f - num3) + (position + base.MissionScreen.CombatCamera.Direction) * num3;
			int num4 = base.Mission.Scene.SelectEntitiesInBoxWithScriptComponent<SpawnedItemEntity>(ref vec, ref vec2, this._tempPickableEntities, this._pickableItemsId, false);
			for (int i = 0; i < num4; i++)
			{
				WeakGameEntity weakGameEntity = this._tempPickableEntities[i];
				SpawnedItemEntity firstScriptOfType = weakGameEntity.GetFirstScriptOfType<SpawnedItemEntity>();
				if (firstScriptOfType != null)
				{
					Vec3 vec4 = weakGameEntity.ComputeGlobalPhysicsBoundingBoxCenter();
					Vec3 vec5 = (vec4 - vec3).NormalizedCopy();
					Vec3 globalPosition = weakGameEntity.GlobalPosition;
					Vec3 vec6 = (globalPosition - vec3).NormalizedCopy();
					float num5;
					WeakGameEntity weakGameEntity2;
					WeakGameEntity weakGameEntity3;
					if ((base.Mission.Scene.RayCastForClosestEntityOrTerrain(vec3 + vec5 * 0.2f, vec4, out num5, out weakGameEntity2, 0.2f, BodyFlags.CommonFocusRayCastExcludeFlags) && weakGameEntity2.IsValid && weakGameEntity2 == weakGameEntity) || (base.Mission.Scene.RayCastForClosestEntityOrTerrain(vec3 + vec6 * 0.2f, globalPosition, out num5, out weakGameEntity3, 0.2f, BodyFlags.CommonFocusRayCastExcludeFlags) && weakGameEntity3.IsValid && weakGameEntity3 == weakGameEntity))
					{
						if (firstScriptOfType.IsBanner())
						{
							if (MissionGameModels.Current.BattleBannerBearersModel.IsInteractableFormationBanner(firstScriptOfType, main))
							{
								this._contourItemContainer.AddRegularContourEntity(GameEntity.CreateFromWeakEntity(weakGameEntity));
							}
						}
						else
						{
							this._contourItemContainer.AddRegularContourEntity(GameEntity.CreateFromWeakEntity(weakGameEntity));
						}
					}
				}
			}
			int num6 = base.Mission.Scene.SelectEntitiesInBoxWithScriptComponent<SpawnedItemEntity>(ref vec, ref vec2, this._tempPickableEntities, this._pickableItemsId, true);
			for (int j = 0; j < num6; j++)
			{
				WeakGameEntity weakGameEntity4 = this._tempPickableEntities[j];
				SpawnedItemEntity firstScriptOfType2 = weakGameEntity4.GetFirstScriptOfType<SpawnedItemEntity>();
				if (firstScriptOfType2 != null)
				{
					Vec3 vec7 = weakGameEntity4.ComputeGlobalPhysicsBoundingBoxCenter();
					Vec3 vec8 = (vec7 - vec3).NormalizedCopy();
					Vec3 globalPosition2 = weakGameEntity4.GlobalPosition;
					Vec3 vec9 = (globalPosition2 - vec3).NormalizedCopy();
					float num5;
					WeakGameEntity weakGameEntity5;
					WeakGameEntity weakGameEntity6;
					if ((base.Mission.Scene.RayCastForClosestEntityOrTerrainFixedPhysics(vec3 + vec8 * 0.2f, vec7, out num5, out weakGameEntity5, 0.2f, BodyFlags.CommonFocusRayCastExcludeFlags) && weakGameEntity5.IsValid && weakGameEntity5 == weakGameEntity4) || (base.Mission.Scene.RayCastForClosestEntityOrTerrainFixedPhysics(vec3 + vec9 * 0.2f, globalPosition2, out num5, out weakGameEntity6, 0.2f, BodyFlags.CommonFocusRayCastExcludeFlags) && weakGameEntity6.IsValid && weakGameEntity6 == weakGameEntity4))
					{
						if (firstScriptOfType2.IsBanner())
						{
							if (MissionGameModels.Current.BattleBannerBearersModel.IsInteractableFormationBanner(firstScriptOfType2, main))
							{
								this._contourItemContainer.AddRegularContourEntity(GameEntity.CreateFromWeakEntity(weakGameEntity4));
							}
						}
						else
						{
							this._contourItemContainer.AddRegularContourEntity(GameEntity.CreateFromWeakEntity(weakGameEntity4));
						}
					}
				}
			}
			int num7 = base.Mission.Scene.SelectEntitiesInBoxWithScriptComponent<UsableMachine>(ref vec, ref vec2, this._tempPickableEntities, this._pickableItemsId, false);
			for (int k = 0; k < num7; k++)
			{
				WeakGameEntity weakGameEntity7 = this._tempPickableEntities[k];
				UsableMachine firstScriptOfType3 = weakGameEntity7.GetFirstScriptOfType<UsableMachine>();
				if (firstScriptOfType3 != null && !firstScriptOfType3.IsDisabled)
				{
					WeakGameEntity validStandingPointForAgentWithoutDistanceCheck = firstScriptOfType3.GetValidStandingPointForAgentWithoutDistanceCheck(main);
					if (validStandingPointForAgentWithoutDistanceCheck.IsValid && !(validStandingPointForAgentWithoutDistanceCheck.GetFirstScriptOfType<UsableMissionObject>() is SpawnedItemEntity))
					{
						IFocusable focusable;
						if ((focusable = validStandingPointForAgentWithoutDistanceCheck.GetScriptComponents().FirstOrDefault<ScriptComponentBehavior>((ScriptComponentBehavior sc) => sc is IFocusable) as IFocusable) != null && focusable is UsableMissionObject)
						{
							this._contourItemContainer.AddRegularContourEntity(GameEntity.CreateFromWeakEntity(weakGameEntity7));
						}
					}
				}
			}
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0002228C File Offset: 0x0002048C
		private void ApplyContourToAllItems()
		{
			if (!this._isContourAppliedToAllItems)
			{
				foreach (GameEntity gameEntity in this._contourItemContainer.GetAllEligibleContourItems())
				{
					uint nonFocusedColor = this.GetNonFocusedColor(gameEntity);
					uint num = ((gameEntity == this._focusedGameEntity) ? this._focusedContourColor : nonFocusedColor);
					gameEntity.SetContourColor(new uint?(num), true);
				}
				this._isContourAppliedToAllItems = true;
			}
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0002231C File Offset: 0x0002051C
		private uint GetNonFocusedColor(GameEntity entity)
		{
			SpawnedItemEntity firstScriptOfType = entity.GetFirstScriptOfType<SpawnedItemEntity>();
			ItemObject itemObject = ((firstScriptOfType != null) ? firstScriptOfType.WeaponCopy.Item : null);
			WeaponComponentData weaponComponentData = ((itemObject != null) ? itemObject.PrimaryWeapon : null);
			ItemObject.ItemTypeEnum? itemTypeEnum = ((itemObject != null) ? new ItemObject.ItemTypeEnum?(itemObject.ItemType) : null);
			if (itemObject != null && itemObject.HasBannerComponent)
			{
				return this._nonFocusedBannerContourColor;
			}
			if (weaponComponentData == null || !weaponComponentData.IsAmmo)
			{
				ItemObject.ItemTypeEnum? itemTypeEnum2 = itemTypeEnum;
				ItemObject.ItemTypeEnum itemTypeEnum3 = ItemObject.ItemTypeEnum.Arrows;
				if (!((itemTypeEnum2.GetValueOrDefault() == itemTypeEnum3) & (itemTypeEnum2 != null)))
				{
					itemTypeEnum2 = itemTypeEnum;
					itemTypeEnum3 = ItemObject.ItemTypeEnum.Bolts;
					if (!((itemTypeEnum2.GetValueOrDefault() == itemTypeEnum3) & (itemTypeEnum2 != null)))
					{
						itemTypeEnum2 = itemTypeEnum;
						itemTypeEnum3 = ItemObject.ItemTypeEnum.SlingStones;
						if (!((itemTypeEnum2.GetValueOrDefault() == itemTypeEnum3) & (itemTypeEnum2 != null)))
						{
							itemTypeEnum2 = itemTypeEnum;
							itemTypeEnum3 = ItemObject.ItemTypeEnum.Bullets;
							if (!((itemTypeEnum2.GetValueOrDefault() == itemTypeEnum3) & (itemTypeEnum2 != null)))
							{
								itemTypeEnum2 = itemTypeEnum;
								itemTypeEnum3 = ItemObject.ItemTypeEnum.Thrown;
								if ((itemTypeEnum2.GetValueOrDefault() == itemTypeEnum3) & (itemTypeEnum2 != null))
								{
									return this._nonFocusedThrowableContourColor;
								}
								return this._nonFocusedDefaultContourColor;
							}
						}
					}
				}
			}
			return this._nonFocusedAmmoContourColor;
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00022424 File Offset: 0x00020624
		private void RemoveContourFromAllItems()
		{
			if (this._isContourAppliedToAllItems)
			{
				foreach (GameEntity gameEntity in this._contourItemContainer.GetAllEligibleContourItems())
				{
					if (this._focusedGameEntity == null || gameEntity != this._focusedGameEntity)
					{
						gameEntity.SetContourColor(null, true);
					}
				}
				this._isContourAppliedToAllItems = false;
			}
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x000224B0 File Offset: 0x000206B0
		private void AddContourToFocusedItem()
		{
			if (this._focusedGameEntity != null && !this._isContourAppliedToFocusedItem)
			{
				this._focusedGameEntity.SetContourColor(new uint?(this._focusedContourColor), true);
				this._isContourAppliedToFocusedItem = true;
			}
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x000224E8 File Offset: 0x000206E8
		private void RemoveContourFromItem(WeakGameEntity entity)
		{
			if (!entity.IsValid)
			{
				return;
			}
			GameEntity gameEntity = GameEntity.CreateFromWeakEntity(entity);
			this.RemoveContourFromItem(gameEntity);
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00022510 File Offset: 0x00020710
		private void RemoveContourFromItem(GameEntity entity)
		{
			if (entity == null)
			{
				return;
			}
			if (this._contourItemContainer.GetAllEligibleContourItems().Contains(entity))
			{
				entity.SetContourColor(new uint?(this._nonFocusedDefaultContourColor), true);
				return;
			}
			entity.SetContourColor(null, true);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00022560 File Offset: 0x00020760
		private UsableMachine GetUsableMachineFromPoint(UsableMissionObject standingPoint)
		{
			WeakGameEntity weakGameEntity = standingPoint.GameEntity;
			while (weakGameEntity.IsValid && !weakGameEntity.HasScriptOfType<UsableMachine>())
			{
				weakGameEntity = weakGameEntity.Parent;
			}
			if (weakGameEntity.IsValid)
			{
				UsableMachine firstScriptOfType = weakGameEntity.GetFirstScriptOfType<UsableMachine>();
				if (firstScriptOfType != null)
				{
					return firstScriptOfType;
				}
			}
			return null;
		}

		// Token: 0x04000297 RID: 663
		private const float SceneItemQueryFreq = 1f;

		// Token: 0x04000298 RID: 664
		private readonly WeakGameEntity[] _tempPickableEntities = new WeakGameEntity[128];

		// Token: 0x04000299 RID: 665
		private readonly UIntPtr[] _pickableItemsId = new UIntPtr[128];

		// Token: 0x0400029A RID: 666
		private readonly MissionItemContourControllerView.ContourItemContainer _contourItemContainer = new MissionItemContourControllerView.ContourItemContainer();

		// Token: 0x0400029B RID: 667
		private GameEntity _focusedGameEntity;

		// Token: 0x0400029C RID: 668
		private IFocusable _currentFocusedObject;

		// Token: 0x0400029D RID: 669
		private bool _isContourAppliedToAllItems;

		// Token: 0x0400029E RID: 670
		private bool _isContourAppliedToFocusedItem;

		// Token: 0x0400029F RID: 671
		private readonly uint _nonFocusedDefaultContourColor = new Color(0.85f, 0.85f, 0.85f, 1f).ToUnsignedInteger();

		// Token: 0x040002A0 RID: 672
		private readonly uint _nonFocusedAmmoContourColor = new Color(0f, 0.73f, 1f, 1f).ToUnsignedInteger();

		// Token: 0x040002A1 RID: 673
		private readonly uint _nonFocusedThrowableContourColor = new Color(0.051f, 0.988f, 0.18f, 1f).ToUnsignedInteger();

		// Token: 0x040002A2 RID: 674
		private readonly uint _nonFocusedBannerContourColor = new Color(0.521f, 0.988f, 0.521f, 1f).ToUnsignedInteger();

		// Token: 0x040002A3 RID: 675
		private readonly uint _focusedContourColor = new Color(1f, 0.84f, 0.35f, 1f).ToUnsignedInteger();

		// Token: 0x040002A4 RID: 676
		private float _lastItemQueryTime;

		// Token: 0x020000DE RID: 222
		private class ContourItemContainer
		{
			// Token: 0x0600065C RID: 1628 RVA: 0x0002B67C File Offset: 0x0002987C
			public void AddRegularContourEntity(GameEntity entity)
			{
				this._regularContourEntities.Add(entity);
			}

			// Token: 0x0600065D RID: 1629 RVA: 0x0002B68A File Offset: 0x0002988A
			public void RemoveRegularContourEntity(GameEntity entity)
			{
				this._regularContourEntities.Remove(entity);
			}

			// Token: 0x0600065E RID: 1630 RVA: 0x0002B699 File Offset: 0x00029899
			public void ClearRegularContourEntities()
			{
				this._regularContourEntities.Clear();
			}

			// Token: 0x0600065F RID: 1631 RVA: 0x0002B6A6 File Offset: 0x000298A6
			public void AddAdditionalContourEntity(WeakGameEntity entity)
			{
				this._additionalContourEntities.Add(entity);
			}

			// Token: 0x06000660 RID: 1632 RVA: 0x0002B6B4 File Offset: 0x000298B4
			public void RemoveAdditionalContourEntity(WeakGameEntity entity)
			{
				this._additionalContourEntities.Remove(entity);
			}

			// Token: 0x06000661 RID: 1633 RVA: 0x0002B6C3 File Offset: 0x000298C3
			public void ClearAdditionalontourEntities()
			{
				this._additionalContourEntities.Clear();
			}

			// Token: 0x06000662 RID: 1634 RVA: 0x0002B6D0 File Offset: 0x000298D0
			public List<GameEntity> GetAllEligibleContourItems()
			{
				this._allEligibleContourItemsCache.Clear();
				for (int i = 0; i < this._regularContourEntities.Count; i++)
				{
					this._allEligibleContourItemsCache.Add(this._regularContourEntities[i]);
				}
				for (int j = 0; j < this._additionalContourEntities.Count; j++)
				{
					GameEntity gameEntity = GameEntity.CreateFromWeakEntity(this._additionalContourEntities[j]);
					this._allEligibleContourItemsCache.Add(gameEntity);
				}
				return this._allEligibleContourItemsCache;
			}

			// Token: 0x040003F5 RID: 1013
			private readonly List<WeakGameEntity> _additionalContourEntities = new List<WeakGameEntity>();

			// Token: 0x040003F6 RID: 1014
			private readonly List<GameEntity> _regularContourEntities = new List<GameEntity>();

			// Token: 0x040003F7 RID: 1015
			private readonly List<GameEntity> _allEligibleContourItemsCache = new List<GameEntity>();
		}
	}
}
