using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200025F RID: 607
	public abstract class MissionObject : ScriptComponentBehavior
	{
		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x060022A6 RID: 8870 RVA: 0x0007A15A File Offset: 0x0007835A
		private Mission Mission
		{
			get
			{
				return Mission.Current;
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x060022A7 RID: 8871 RVA: 0x0007A161 File Offset: 0x00078361
		// (set) Token: 0x060022A8 RID: 8872 RVA: 0x0007A169 File Offset: 0x00078369
		public MissionObjectId Id { get; set; }

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x060022A9 RID: 8873 RVA: 0x0007A172 File Offset: 0x00078372
		// (set) Token: 0x060022AA RID: 8874 RVA: 0x0007A17A File Offset: 0x0007837A
		public bool IsDisabled { get; private set; }

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x060022AB RID: 8875 RVA: 0x0007A183 File Offset: 0x00078383
		public virtual TextObject HitObjectName { get; }

		// Token: 0x060022AC RID: 8876 RVA: 0x0007A18C File Offset: 0x0007838C
		public MissionObject()
		{
			MissionObjectId missionObjectId = new MissionObjectId(-1, false);
			this.Id = missionObjectId;
		}

		// Token: 0x060022AD RID: 8877 RVA: 0x0007A1BC File Offset: 0x000783BC
		public virtual void SetAbilityOfFaces(bool enabled)
		{
			if (this.DynamicNavmeshIdStart > 0)
			{
				for (int i = this.DynamicNavmeshIdStart; i < this.DynamicNavmeshIdStart + 10; i++)
				{
					base.GameEntity.Scene.SetAbilityOfFacesWithId(i, enabled);
				}
			}
		}

		// Token: 0x060022AE RID: 8878 RVA: 0x0007A204 File Offset: 0x00078404
		protected void SetAbilityOfConditionalFaces(bool enabled)
		{
			if (this.DynamicNavmeshIdStart > 0)
			{
				base.GameEntity.Scene.SetAbilityOfFacesWithId(this.DynamicNavmeshIdStart + 8, enabled);
			}
		}

		// Token: 0x060022AF RID: 8879 RVA: 0x0007A238 File Offset: 0x00078438
		protected internal override void OnInit()
		{
			base.OnInit();
			if (!GameNetwork.IsClientOrReplay)
			{
				this.AttachDynamicNavmeshToEntity();
				this.SetAbilityOfFaces(base.GameEntity.IsValid && base.GameEntity.IsVisibleIncludeParents());
				this.SetAbilityOfConditionalFaces(base.GameEntity.IsValid && base.GameEntity.IsVisibleIncludeParents());
			}
		}

		// Token: 0x060022B0 RID: 8880 RVA: 0x0007A2A8 File Offset: 0x000784A8
		protected virtual void AttachDynamicNavmeshToEntity()
		{
			if (this.NavMeshPrefabName.Length > 0)
			{
				this.DynamicNavmeshIdStart = Mission.Current.GetNextDynamicNavMeshIdStart();
				base.GameEntity.Scene.ImportNavigationMeshPrefab(this.NavMeshPrefabName, this.DynamicNavmeshIdStart);
				this.GetEntityToAttachNavMeshFaces().AttachNavigationMeshFaces(this.DynamicNavmeshIdStart + 1, false, false, false, false, true);
				this.GetEntityToAttachNavMeshFaces().AttachNavigationMeshFaces(this.DynamicNavmeshIdStart + 2, true, false, false, false, true);
				this.GetEntityToAttachNavMeshFaces().AttachNavigationMeshFaces(this.DynamicNavmeshIdStart + 3, true, false, false, false, true);
				this.GetEntityToAttachNavMeshFaces().AttachNavigationMeshFaces(this.DynamicNavmeshIdStart + 4, false, true, false, false, false);
				this.GetEntityToAttachNavMeshFaces().AttachNavigationMeshFaces(this.DynamicNavmeshIdStart + 8, false, true, false, true, true);
				this.SetAbilityOfFaces(base.GameEntity.IsValid && base.GameEntity.GetPhysicsState());
			}
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x0007A3A0 File Offset: 0x000785A0
		protected virtual WeakGameEntity GetEntityToAttachNavMeshFaces()
		{
			return base.GameEntity;
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x0007A3A8 File Offset: 0x000785A8
		protected internal override bool OnCheckForProblems()
		{
			base.OnCheckForProblems();
			bool flag = false;
			List<WeakGameEntity> list = new List<WeakGameEntity>();
			list.Add(base.GameEntity);
			base.GameEntity.GetChildrenRecursive(ref list);
			bool flag2 = false;
			foreach (WeakGameEntity weakGameEntity in list)
			{
				flag2 = flag2 || (weakGameEntity.HasPhysicsDefinitionWithoutFlags(1) && !weakGameEntity.PhysicsDescBodyFlag.HasAnyFlag(BodyFlags.CommonCollisionExcludeFlagsForMissile));
			}
			Vec3 scaleVector = base.GameEntity.GetGlobalFrame().rotation.GetScaleVector();
			bool flag3 = MathF.Abs(scaleVector.x - scaleVector.y) >= 0.01f || MathF.Abs(scaleVector.x - scaleVector.z) >= 0.01f;
			if (flag2 && flag3)
			{
				MBEditor.AddEntityWarning(base.GameEntity, "Mission object has non-uniform scale and physics object. This is not supported because any attached focusable item to this mesh will not work within this configuration.");
				flag = true;
			}
			return flag;
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x0007A4B8 File Offset: 0x000786B8
		protected internal override void OnPreInit()
		{
			base.OnPreInit();
			if (this.Mission != null)
			{
				int num = -1;
				bool flag;
				if (this.Mission.IsLoadingFinished)
				{
					flag = true;
					if (!GameNetwork.IsClientOrReplay)
					{
						num = this.Mission.GetFreeRuntimeMissionObjectId();
					}
				}
				else
				{
					flag = false;
					num = this.Mission.GetFreeSceneMissionObjectId();
				}
				this.Id = new MissionObjectId(num, flag);
				this.Mission.AddActiveMissionObject(this);
			}
			base.GameEntity.SetAsReplayEntity();
			WeakGameEntity firstChildEntityWithTag = base.GameEntity.GetFirstChildEntityWithTag("batched_physics_entity");
			if (firstChildEntityWithTag != WeakGameEntity.Invalid)
			{
				firstChildEntityWithTag.CreateVariableRatePhysics(true);
			}
			string name = base.GameEntity.Name;
			foreach (WeakGameEntity weakGameEntity in base.GameEntity.GetChildren())
			{
				BodyFlags bodyFlag = weakGameEntity.BodyFlag;
				if (weakGameEntity != firstChildEntityWithTag)
				{
					weakGameEntity.CreateVariableRatePhysics(true);
				}
			}
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x0007A5C8 File Offset: 0x000787C8
		public override int GetHashCode()
		{
			return this.Id.GetHashCode();
		}

		// Token: 0x060022B5 RID: 8885 RVA: 0x0007A5E9 File Offset: 0x000787E9
		protected internal virtual void OnMissionReset()
		{
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x0007A5EB File Offset: 0x000787EB
		public virtual void AfterMissionStart()
		{
		}

		// Token: 0x060022B7 RID: 8887 RVA: 0x0007A5ED File Offset: 0x000787ED
		public virtual void OnMissionEnded()
		{
		}

		// Token: 0x060022B8 RID: 8888 RVA: 0x0007A5EF File Offset: 0x000787EF
		public virtual void OnDeploymentFinished()
		{
		}

		// Token: 0x060022B9 RID: 8889 RVA: 0x0007A5F1 File Offset: 0x000787F1
		protected internal virtual bool OnHit(Agent attackerAgent, int damage, Vec3 impactPosition, Vec3 impactDirection, in MissionWeapon weapon, int affectorWeaponSlotOrMissileIndex, ScriptComponentBehavior attackerScriptComponentBehavior, out bool reportDamage, out float finalDamage, out float fireDamage, out float modifiedFireDamage)
		{
			reportDamage = false;
			finalDamage = (float)damage;
			fireDamage = -1f;
			modifiedFireDamage = -1f;
			return false;
		}

		// Token: 0x060022BA RID: 8890 RVA: 0x0007A610 File Offset: 0x00078810
		public void SetEnabled(bool isParentObject = false)
		{
			if (this.IsDisabled)
			{
				if (!GameNetwork.IsClientOrReplay)
				{
					this.SetAbilityOfFaces(true);
				}
				if (isParentObject && base.GameEntity != null)
				{
					List<WeakGameEntity> list = new List<WeakGameEntity>();
					base.GameEntity.GetChildrenRecursive(ref list);
					foreach (MissionObject missionObject in from sc in list.SelectMany<WeakGameEntity, ScriptComponentBehavior>((WeakGameEntity ac) => ac.GetScriptComponents())
						where sc is MissionObject
						select sc as MissionObject)
					{
						missionObject.SetEnabled(false);
					}
				}
				Mission.Current.ActivateMissionObject(this);
				this.IsDisabled = false;
			}
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x0007A71C File Offset: 0x0007891C
		public void SetEnabledAndMakeVisible(bool isParentObject = false, bool enableFaces = false)
		{
			this.SetEnabledAndMakeVisibleAux(isParentObject, enableFaces);
			base.SetScriptComponentToTick(this.GetTickRequirement());
			if (base.GameEntity != null)
			{
				List<WeakGameEntity> list = new List<WeakGameEntity>();
				base.GameEntity.GetChildrenRecursive(ref list);
				foreach (WeakGameEntity weakGameEntity in list)
				{
					int scriptCount = weakGameEntity.GetScriptCount();
					for (int i = 0; i < scriptCount; i++)
					{
						ScriptComponentBehavior scriptAtIndex = weakGameEntity.GetScriptAtIndex(i);
						if (scriptAtIndex != null)
						{
							scriptAtIndex.SetScriptComponentToTick(scriptAtIndex.GetTickRequirement());
						}
					}
				}
			}
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x0007A7D4 File Offset: 0x000789D4
		private void SetEnabledAndMakeVisibleAux(bool isParentObject, bool enableFaces)
		{
			if (enableFaces && !GameNetwork.IsClientOrReplay)
			{
				this.SetAbilityOfFaces(true);
			}
			if (isParentObject && base.GameEntity != null)
			{
				List<WeakGameEntity> list = new List<WeakGameEntity>();
				base.GameEntity.GetChildrenRecursive(ref list);
				foreach (MissionObject missionObject in list.SelectMany<WeakGameEntity, MissionObject>((WeakGameEntity ac) => ac.GetScriptComponents<MissionObject>()))
				{
					missionObject.SetEnabledAndMakeVisibleAux(false, enableFaces);
				}
			}
			Mission.Current.ActivateMissionObject(this);
			this.IsDisabled = false;
			if (base.GameEntity != null)
			{
				base.GameEntity.SetVisibilityExcludeParents(true);
				base.GameEntity.SetPhysicsState(true, false);
			}
		}

		// Token: 0x060022BD RID: 8893 RVA: 0x0007A8B4 File Offset: 0x00078AB4
		public void SetDisabled(bool isParentObject = false)
		{
			if (!this.IsDisabled)
			{
				if (!GameNetwork.IsClientOrReplay)
				{
					this.SetAbilityOfFaces(false);
				}
				if (isParentObject && base.GameEntity.IsValid)
				{
					List<WeakGameEntity> list = new List<WeakGameEntity>();
					base.GameEntity.GetChildrenRecursive(ref list);
					foreach (MissionObject missionObject in list.SelectMany<WeakGameEntity, MissionObject>((WeakGameEntity ac) => ac.GetScriptComponents<MissionObject>()))
					{
						missionObject.SetDisabled(false);
					}
				}
				Mission.Current.DeactivateMissionObject(this);
				this.IsDisabled = true;
			}
		}

		// Token: 0x060022BE RID: 8894 RVA: 0x0007A974 File Offset: 0x00078B74
		public void SetDisabledAndMakeInvisible(bool isParentObject = false, bool disableFaces = false)
		{
			if (disableFaces && !GameNetwork.IsClientOrReplay)
			{
				this.SetAbilityOfFaces(false);
			}
			if (isParentObject && base.GameEntity.IsValid)
			{
				List<WeakGameEntity> list = new List<WeakGameEntity>();
				base.GameEntity.GetChildrenRecursive(ref list);
				foreach (MissionObject missionObject in list.SelectMany<WeakGameEntity, MissionObject>((WeakGameEntity ac) => ac.GetScriptComponents<MissionObject>()))
				{
					missionObject.SetDisabledAndMakeInvisible(false, disableFaces);
				}
			}
			Mission.Current.DeactivateMissionObject(this);
			this.IsDisabled = true;
			if (base.GameEntity.IsValid)
			{
				base.GameEntity.SetVisibilityExcludeParents(false);
				base.GameEntity.SetPhysicsState(false, false);
				base.SetScriptComponentToTick(this.GetTickRequirement());
			}
		}

		// Token: 0x060022BF RID: 8895 RVA: 0x0007AA64 File Offset: 0x00078C64
		protected override void OnRemoved(int removeReason)
		{
			base.OnRemoved(removeReason);
			if (!GameNetwork.IsClientOrReplay)
			{
				this.SetAbilityOfFaces(false);
			}
			if (this.Mission != null)
			{
				this.Mission.OnMissionObjectRemoved(this, removeReason);
			}
		}

		// Token: 0x060022C0 RID: 8896 RVA: 0x0007AA91 File Offset: 0x00078C91
		public virtual void OnEndMission()
		{
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x060022C1 RID: 8897 RVA: 0x0007AA93 File Offset: 0x00078C93
		public bool CreatedAtRuntime
		{
			get
			{
				return this.Id.CreatedAtRuntime;
			}
		}

		// Token: 0x060022C2 RID: 8898 RVA: 0x0007AAA0 File Offset: 0x00078CA0
		protected internal override bool MovesEntity()
		{
			return true;
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x0007AAA4 File Offset: 0x00078CA4
		public virtual void AddStuckMissile(GameEntity missileEntity)
		{
			base.GameEntity.AddChild(missileEntity.WeakEntity, false);
		}

		// Token: 0x04000D7B RID: 3451
		public const int MaxNavMeshPerDynamicObject = 50;

		// Token: 0x04000D7E RID: 3454
		[EditableScriptComponentVariable(true, "")]
		protected string NavMeshPrefabName = "";

		// Token: 0x04000D7F RID: 3455
		protected int DynamicNavmeshIdStart;

		// Token: 0x0200054B RID: 1355
		protected enum DynamicNavmeshLocalIds
		{
			// Token: 0x04001E08 RID: 7688
			Inside = 1,
			// Token: 0x04001E09 RID: 7689
			Enter,
			// Token: 0x04001E0A RID: 7690
			Exit,
			// Token: 0x04001E0B RID: 7691
			Blocker,
			// Token: 0x04001E0C RID: 7692
			Extra1,
			// Token: 0x04001E0D RID: 7693
			Extra2,
			// Token: 0x04001E0E RID: 7694
			Extra3,
			// Token: 0x04001E0F RID: 7695
			ConditionalBlocker,
			// Token: 0x04001E10 RID: 7696
			Reserved1,
			// Token: 0x04001E11 RID: 7697
			Count
		}
	}
}
