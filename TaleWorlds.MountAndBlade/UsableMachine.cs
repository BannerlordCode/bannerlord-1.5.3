using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000386 RID: 902
	public abstract class UsableMachine : SynchedMissionObject, IFocusable, IOrderable, IDetachment
	{
		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x06003389 RID: 13193 RVA: 0x000D4CF4 File Offset: 0x000D2EF4
		// (set) Token: 0x0600338A RID: 13194 RVA: 0x000D4CFC File Offset: 0x000D2EFC
		public MBList<StandingPoint> StandingPoints { get; private set; }

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x0600338B RID: 13195 RVA: 0x000D4D05 File Offset: 0x000D2F05
		// (set) Token: 0x0600338C RID: 13196 RVA: 0x000D4D0D File Offset: 0x000D2F0D
		public StandingPoint PilotStandingPoint { get; private set; }

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x0600338D RID: 13197 RVA: 0x000D4D16 File Offset: 0x000D2F16
		// (set) Token: 0x0600338E RID: 13198 RVA: 0x000D4D1E File Offset: 0x000D2F1E
		public int PilotStandingPointSlotIndex { get; private set; }

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x0600338F RID: 13199 RVA: 0x000D4D27 File Offset: 0x000D2F27
		// (set) Token: 0x06003390 RID: 13200 RVA: 0x000D4D2F File Offset: 0x000D2F2F
		protected internal List<StandingPoint> AmmoPickUpPoints { get; private set; }

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06003391 RID: 13201 RVA: 0x000D4D38 File Offset: 0x000D2F38
		// (set) Token: 0x06003392 RID: 13202 RVA: 0x000D4D40 File Offset: 0x000D2F40
		private protected List<GameEntity> WaitStandingPoints { protected get; private set; }

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x06003393 RID: 13203 RVA: 0x000D4D49 File Offset: 0x000D2F49
		// (set) Token: 0x06003394 RID: 13204 RVA: 0x000D4D51 File Offset: 0x000D2F51
		public DestructableComponent DestructionComponent { get; private set; }

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x06003395 RID: 13205 RVA: 0x000D4D5A File Offset: 0x000D2F5A
		public bool IsDestructible
		{
			get
			{
				return this.DestructionComponent != null;
			}
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x06003396 RID: 13206 RVA: 0x000D4D65 File Offset: 0x000D2F65
		public bool IsDestroyed
		{
			get
			{
				return this.DestructionComponent != null && this.DestructionComponent.IsDestroyed;
			}
		}

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x06003397 RID: 13207 RVA: 0x000D4D7C File Offset: 0x000D2F7C
		// (set) Token: 0x06003398 RID: 13208 RVA: 0x000D4D84 File Offset: 0x000D2F84
		private protected bool IsDetachmentRecentlyEvaluated { protected get; private set; }

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x06003399 RID: 13209 RVA: 0x000D4D8D File Offset: 0x000D2F8D
		public Agent PilotAgent
		{
			get
			{
				StandingPoint pilotStandingPoint = this.PilotStandingPoint;
				if (pilotStandingPoint == null)
				{
					return null;
				}
				return pilotStandingPoint.UserAgent;
			}
		}

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x0600339A RID: 13210 RVA: 0x000D4DA0 File Offset: 0x000D2FA0
		public bool IsLoose
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x0600339B RID: 13211 RVA: 0x000D4DA4 File Offset: 0x000D2FA4
		public virtual float SinkingReferenceOffset
		{
			get
			{
				return base.GameEntity.GetGlobalScale().z * 0.5f;
			}
		}

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x0600339C RID: 13212 RVA: 0x000D4DCA File Offset: 0x000D2FCA
		public UsableMachineAIBase Ai
		{
			get
			{
				if (this._ai == null)
				{
					this._ai = this.CreateAIBehaviorObject();
				}
				return this._ai;
			}
		}

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x0600339D RID: 13213 RVA: 0x000D4DE6 File Offset: 0x000D2FE6
		public virtual FocusableObjectType FocusableObjectType
		{
			get
			{
				return FocusableObjectType.Item;
			}
		}

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x0600339E RID: 13214 RVA: 0x000D4DE9 File Offset: 0x000D2FE9
		public virtual bool IsFocusable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x0600339F RID: 13215 RVA: 0x000D4DEC File Offset: 0x000D2FEC
		// (set) Token: 0x060033A0 RID: 13216 RVA: 0x000D4DF4 File Offset: 0x000D2FF4
		public StandingPoint CurrentlyUsedAmmoPickUpPoint
		{
			get
			{
				return this._currentlyUsedAmmoPickUpPoint;
			}
			set
			{
				this._currentlyUsedAmmoPickUpPoint = value;
				base.SetScriptComponentToTick(this.GetTickRequirement());
			}
		}

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x060033A1 RID: 13217 RVA: 0x000D4E09 File Offset: 0x000D3009
		public bool HasAIPickingUpAmmo
		{
			get
			{
				return this.CurrentlyUsedAmmoPickUpPoint != null;
			}
		}

		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x060033A2 RID: 13218 RVA: 0x000D4E14 File Offset: 0x000D3014
		// (set) Token: 0x060033A3 RID: 13219 RVA: 0x000D4E1C File Offset: 0x000D301C
		public bool IsDisabledForAI { get; protected set; }

		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x060033A4 RID: 13220 RVA: 0x000D4E25 File Offset: 0x000D3025
		public MBReadOnlyList<Formation> UserFormations
		{
			get
			{
				return this._userFormations;
			}
		}

		// Token: 0x060033A5 RID: 13221 RVA: 0x000D4E30 File Offset: 0x000D3030
		protected UsableMachine()
		{
			this._components = new List<UsableMissionObjectComponent>();
		}

		// Token: 0x060033A6 RID: 13222 RVA: 0x000D4E88 File Offset: 0x000D3088
		public void AddComponent(UsableMissionObjectComponent component)
		{
			this._components.Add(component);
			component.OnAdded(base.Scene);
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x060033A7 RID: 13223 RVA: 0x000D4EAE File Offset: 0x000D30AE
		public void RemoveComponent(UsableMissionObjectComponent component)
		{
			component.OnRemoved();
			this._components.Remove(component);
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x060033A8 RID: 13224 RVA: 0x000D4ED0 File Offset: 0x000D30D0
		public T GetComponent<T>() where T : UsableMissionObjectComponent
		{
			using (List<UsableMissionObjectComponent>.Enumerator enumerator = this._components.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T t;
					if ((t = enumerator.Current as T) != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x060033A9 RID: 13225 RVA: 0x000D4F40 File Offset: 0x000D3140
		public virtual OrderType GetOrder(BattleSideEnum side)
		{
			return OrderType.Use;
		}

		// Token: 0x060033AA RID: 13226 RVA: 0x000D4F44 File Offset: 0x000D3144
		public virtual UsableMachineAIBase CreateAIBehaviorObject()
		{
			return null;
		}

		// Token: 0x060033AB RID: 13227 RVA: 0x000D4F48 File Offset: 0x000D3148
		public WeakGameEntity GetValidVacantReachableStandingPointForAgent(Agent agent)
		{
			float num = float.MaxValue;
			StandingPoint standingPoint = null;
			foreach (StandingPoint standingPoint2 in this.StandingPoints)
			{
				if (!standingPoint2.IsDisabledForAgent(agent) && (!standingPoint2.HasUser || standingPoint2.HasAIUser))
				{
					WorldFrame worldFrame = standingPoint2.GetUserFrameForAgent(agent);
					float num2 = worldFrame.Origin.AsVec2.DistanceSquared(agent.Position.AsVec2);
					float num3;
					if (standingPoint2.UseOwnPositionInsteadOfWorldPosition)
					{
						num3 = standingPoint2.GameEntity.GlobalPosition.z;
					}
					else
					{
						worldFrame = standingPoint2.GetUserFrameForAgent(agent);
						num3 = worldFrame.Origin.GetGroundVec3().z;
					}
					float num4 = num3;
					if (agent.CanReachAndUseObject(standingPoint2, num2) && num2 < num && MathF.Abs(num4 - agent.Position.z) < 1.5f)
					{
						num = num2;
						standingPoint = standingPoint2;
					}
				}
			}
			if (standingPoint == null)
			{
				return WeakGameEntity.Invalid;
			}
			return standingPoint.GameEntity;
		}

		// Token: 0x060033AC RID: 13228 RVA: 0x000D5068 File Offset: 0x000D3268
		public void SetAI(UsableMachineAIBase ai)
		{
			this._ai = ai;
		}

		// Token: 0x060033AD RID: 13229 RVA: 0x000D5074 File Offset: 0x000D3274
		public WeakGameEntity GetValidStandingPointForAgentWithoutDistanceCheck(Agent agent)
		{
			float num = float.MaxValue;
			StandingPoint standingPoint = null;
			foreach (StandingPoint standingPoint2 in this.StandingPoints)
			{
				if (!standingPoint2.IsDisabledForAgent(agent) && (!standingPoint2.HasUser || standingPoint2.HasAIUser))
				{
					WorldFrame worldFrame = standingPoint2.GetUserFrameForAgent(agent);
					float num2 = worldFrame.Origin.AsVec2.DistanceSquared(agent.Position.AsVec2);
					if (num2 < num)
					{
						worldFrame = standingPoint2.GetUserFrameForAgent(agent);
						if (MathF.Abs(worldFrame.Origin.GetGroundVec3().z - agent.Position.z) < 1.5f)
						{
							num = num2;
							standingPoint = standingPoint2;
						}
					}
				}
			}
			if (standingPoint == null)
			{
				return WeakGameEntity.Invalid;
			}
			return standingPoint.GameEntity;
		}

		// Token: 0x060033AE RID: 13230 RVA: 0x000D5164 File Offset: 0x000D3364
		public StandingPoint GetVacantStandingPointForAI(Agent agent)
		{
			if (this.PilotStandingPoint != null && !this.PilotStandingPoint.IsDisabledForAgent(agent) && !this.AmmoPickUpPoints.Contains(this.PilotStandingPoint))
			{
				return this.PilotStandingPoint;
			}
			float num = 100000000f;
			StandingPoint standingPoint = null;
			foreach (StandingPoint standingPoint2 in this.StandingPoints)
			{
				bool flag = true;
				if (this.AmmoPickUpPoints.Contains(standingPoint2))
				{
					foreach (StandingPoint standingPoint3 in this.StandingPoints)
					{
						if (standingPoint3 is StandingPointWithWeaponRequirement && !this.AmmoPickUpPoints.Contains(standingPoint3) && (standingPoint3.IsDeactivated || standingPoint3.HasUser || standingPoint3.HasAIMovingTo))
						{
							flag = false;
							break;
						}
					}
				}
				if (flag && !standingPoint2.IsDisabledForAgent(agent))
				{
					float num2 = (agent.Position - standingPoint2.GetUserFrameForAgent(agent).Origin.GetGroundVec3()).LengthSquared;
					if (!standingPoint2.IsDisabledForPlayers)
					{
						num2 -= 100000f;
					}
					if (num2 < num)
					{
						num = num2;
						standingPoint = standingPoint2;
					}
				}
			}
			return standingPoint;
		}

		// Token: 0x060033AF RID: 13231 RVA: 0x000D52CC File Offset: 0x000D34CC
		public StandingPoint GetTargetStandingPointOfAIAgent(Agent agent)
		{
			foreach (StandingPoint standingPoint in this.StandingPoints)
			{
				if (standingPoint.IsAIMovingTo(agent))
				{
					return standingPoint;
				}
			}
			return null;
		}

		// Token: 0x060033B0 RID: 13232 RVA: 0x000D5328 File Offset: 0x000D3528
		public override void OnMissionEnded()
		{
			foreach (StandingPoint standingPoint in this.StandingPoints)
			{
				Agent userAgent = standingPoint.UserAgent;
				if (userAgent != null)
				{
					userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
				}
				standingPoint.IsDeactivated = true;
			}
		}

		// Token: 0x060033B1 RID: 13233 RVA: 0x000D538C File Offset: 0x000D358C
		public override void SetVisibleSynched(bool value, bool forceChildrenVisible = false)
		{
			base.SetVisibleSynched(value, forceChildrenVisible);
		}

		// Token: 0x060033B2 RID: 13234 RVA: 0x000D5398 File Offset: 0x000D3598
		public override void SetPhysicsStateSynched(bool value, bool setChildren = true)
		{
			base.SetPhysicsStateSynched(value, setChildren);
			this.SetAbilityOfFaces(value);
			foreach (StandingPoint standingPoint in this.StandingPoints)
			{
				standingPoint.OnParentMachinePhysicsStateChanged();
			}
		}

		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x060033B3 RID: 13235 RVA: 0x000D53F8 File Offset: 0x000D35F8
		public int UserCountNotInStruckAction
		{
			get
			{
				int num = 0;
				foreach (StandingPoint standingPoint in this.StandingPoints)
				{
					if (standingPoint.HasUser && !standingPoint.UserAgent.IsInBeingStruckAction)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x060033B4 RID: 13236 RVA: 0x000D5460 File Offset: 0x000D3660
		public int UserCountIncludingInStruckAction
		{
			get
			{
				int num = 0;
				using (List<StandingPoint>.Enumerator enumerator = this.StandingPoints.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.HasUser)
						{
							num++;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x060033B5 RID: 13237 RVA: 0x000D54BC File Offset: 0x000D36BC
		public virtual int MaxUserCount
		{
			get
			{
				return this.StandingPoints.Count;
			}
		}

		// Token: 0x060033B6 RID: 13238 RVA: 0x000D54C9 File Offset: 0x000D36C9
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this.CollectAndSetStandingPoints();
		}

		// Token: 0x060033B7 RID: 13239 RVA: 0x000D54D8 File Offset: 0x000D36D8
		protected internal override void OnInit()
		{
			base.OnInit();
			this.IsDisabledForAttackerAIDueToEnemyInRange = new QueryData<bool>(delegate
			{
				bool flag = false;
				if (this.EnemyRangeToStopUsing > 0f && base.GameEntity != null)
				{
					ref MatrixFrame ptr = ref base.GameEntity.GetGlobalFrame();
					Vec3 vec = new Vec3(this.MachinePositionOffsetToStopUsingLocal, 0f, -1f);
					Vec3 vec2 = ptr.rotation.TransformToParent(in vec);
					Vec3 vec3 = base.GameEntity.GlobalPosition + vec2;
					Agent closestEnemyAgent = Mission.Current.GetClosestEnemyAgent(Mission.Current.Teams.Attacker, vec3, this.EnemyRangeToStopUsing);
					flag = closestEnemyAgent != null && closestEnemyAgent.Position.z > vec3.z - 2f && closestEnemyAgent.Position.z < vec3.z + 4f;
				}
				return flag;
			}, 1f);
			this.IsDisabledForDefenderAIDueToEnemyInRange = new QueryData<bool>(delegate
			{
				bool flag2 = false;
				if (this.EnemyRangeToStopUsing > 0f && base.GameEntity != null)
				{
					ref MatrixFrame ptr2 = ref base.GameEntity.GetGlobalFrame();
					Vec3 vec4 = new Vec3(this.MachinePositionOffsetToStopUsingLocal, 0f, -1f);
					Vec3 vec5 = ptr2.rotation.TransformToParent(in vec4);
					Vec3 vec6 = base.GameEntity.GlobalPosition + vec5;
					Agent closestEnemyAgent2 = Mission.Current.GetClosestEnemyAgent(Mission.Current.Teams.Defender, vec6, this.EnemyRangeToStopUsing);
					flag2 = closestEnemyAgent2 != null && closestEnemyAgent2.Position.z > vec6.z - 2f && closestEnemyAgent2.Position.z < vec6.z + 4f;
				}
				return flag2;
			}, 1f);
			this.CollectAndSetStandingPoints();
			this.AmmoPickUpPoints = new List<StandingPoint>();
			this.DestructionComponent = base.GameEntity.GetFirstScriptOfType<DestructableComponent>();
			this.PilotStandingPoint = null;
			for (int i = 0; i < this.StandingPoints.Count; i++)
			{
				StandingPoint standingPoint = this.StandingPoints[i];
				if (standingPoint.GameEntity.HasTag(this.PilotStandingPointTag))
				{
					this.PilotStandingPoint = standingPoint;
					this.PilotStandingPointSlotIndex = i;
				}
				if (standingPoint.GameEntity.HasTag(this.AmmoPickUpTag))
				{
					this.AmmoPickUpPoints.Add(standingPoint);
				}
				standingPoint.InitializeDefendingAgents();
			}
			this.WaitStandingPoints = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity).CollectChildrenEntitiesWithTag(this.WaitStandingPointTag);
			if (this.WaitStandingPoints.Count > 0)
			{
				this.ActiveWaitStandingPoint = this.WaitStandingPoints[0];
			}
			this._userFormations = new MBList<Formation>();
			this.UsableStandingPoints = new List<ValueTuple<int, StandingPoint>>();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x060033B8 RID: 13240 RVA: 0x000D561C File Offset: 0x000D381C
		private void CollectAndSetStandingPoints()
		{
			if (base.GameEntity.Parent.IsValid && base.GameEntity.Parent.HasTag("machine_parent"))
			{
				this.StandingPoints = base.GameEntity.Parent.CollectScriptComponentsIncludingChildrenRecursive<StandingPoint>();
				return;
			}
			this.StandingPoints = base.GameEntity.CollectScriptComponentsIncludingChildrenRecursive<StandingPoint>();
		}

		// Token: 0x060033B9 RID: 13241 RVA: 0x000D568C File Offset: 0x000D388C
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			bool flag = false;
			using (List<UsableMissionObjectComponent>.Enumerator enumerator = this._components.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsOnTickRequired())
					{
						flag = true;
						break;
					}
				}
			}
			if (base.GameEntity.IsVisibleIncludeParents() && (flag || (!GameNetwork.IsClientOrReplay && this.HasAIPickingUpAmmo) || base.GameEntity.BodyFlag.HasAnyFlag(BodyFlags.Sinking)))
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick;
			}
			return base.GetTickRequirement();
		}

		// Token: 0x060033BA RID: 13242 RVA: 0x000D5730 File Offset: 0x000D3930
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this.MakeVisibilityCheck && !base.GameEntity.IsVisibleIncludeParents())
			{
				return;
			}
			if (base.GameEntity.BodyFlag.HasAnyFlag(BodyFlags.Sinking) && base.GameEntity.GetGlobalFrame().origin.z + this.SinkingReferenceOffset < base.Scene.GetWaterLevelAtPosition(base.GameEntity.GetFrame().origin.AsVec2, !GameNetwork.IsMultiplayer, false))
			{
				this.Disable();
			}
			if (!GameNetwork.IsClientOrReplay && this.HasAIPickingUpAmmo && !this.CurrentlyUsedAmmoPickUpPoint.HasAIMovingTo && !this.CurrentlyUsedAmmoPickUpPoint.HasAIUser)
			{
				this.CurrentlyUsedAmmoPickUpPoint = null;
			}
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				usableMissionObjectComponent.OnTick(dt);
			}
			bool isClientOrReplay = GameNetwork.IsClientOrReplay;
		}

		// Token: 0x060033BB RID: 13243 RVA: 0x000D5848 File Offset: 0x000D3A48
		private static string DebugGetMemberNameOf<T>(object instance, T sp) where T : class
		{
			Type type = instance.GetType();
			foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (!(propertyInfo.GetMethod == null))
				{
					if (propertyInfo.GetValue(instance) == sp)
					{
						return propertyInfo.Name;
					}
					IReadOnlyList<StandingPoint> readOnlyList;
					if (propertyInfo.GetType().IsGenericType && (propertyInfo.GetType().GetGenericTypeDefinition() == typeof(List<>) || propertyInfo.GetType().GetGenericTypeDefinition() == typeof(MBList<>) || propertyInfo.GetType().GetGenericTypeDefinition() == typeof(MBReadOnlyList<>)) && (readOnlyList = propertyInfo.GetValue(instance) as IReadOnlyList<StandingPoint>) != null)
					{
						for (int j = 0; j < readOnlyList.Count; j++)
						{
							StandingPoint standingPoint = readOnlyList[j];
							if (sp == standingPoint)
							{
								return string.Concat(new object[] { propertyInfo.Name, "[", j, "]" });
							}
						}
					}
				}
			}
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (fieldInfo.GetValue(instance) == sp)
				{
					return fieldInfo.Name;
				}
				IReadOnlyList<StandingPoint> readOnlyList2;
				if (fieldInfo.FieldType.IsGenericType && (fieldInfo.FieldType.GetGenericTypeDefinition() == typeof(List<>) || fieldInfo.FieldType.GetGenericTypeDefinition() == typeof(MBList<>) || fieldInfo.FieldType.GetGenericTypeDefinition() == typeof(MBReadOnlyList<>)) && (readOnlyList2 = fieldInfo.GetValue(instance) as IReadOnlyList<StandingPoint>) != null)
				{
					for (int k = 0; k < readOnlyList2.Count; k++)
					{
						StandingPoint standingPoint2 = readOnlyList2[k];
						if (sp == standingPoint2)
						{
							return string.Concat(new object[] { fieldInfo.Name, "[", k, "]" });
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060033BC RID: 13244 RVA: 0x000D5A88 File Offset: 0x000D3C88
		[Conditional("_RGL_KEEP_ASSERTS")]
		protected virtual void DebugTick(float dt)
		{
			if (MBDebug.IsDisplayingHighLevelAI)
			{
				foreach (StandingPoint standingPoint in this.StandingPoints)
				{
					Vec3 globalPosition = standingPoint.GameEntity.GlobalPosition;
					Vec3.One / 3f;
					bool isDeactivated = standingPoint.IsDeactivated;
				}
			}
		}

		// Token: 0x060033BD RID: 13245 RVA: 0x000D5B00 File Offset: 0x000D3D00
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				usableMissionObjectComponent.OnEditorTick(dt);
			}
		}

		// Token: 0x060033BE RID: 13246 RVA: 0x000D5B58 File Offset: 0x000D3D58
		protected internal override void OnEditorValidate()
		{
			base.OnEditorValidate();
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				usableMissionObjectComponent.OnEditorValidate();
			}
		}

		// Token: 0x060033BF RID: 13247 RVA: 0x000D5BB0 File Offset: 0x000D3DB0
		public virtual void OnFocusGain(Agent userAgent)
		{
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				usableMissionObjectComponent.OnFocusGain(userAgent);
			}
		}

		// Token: 0x060033C0 RID: 13248 RVA: 0x000D5C04 File Offset: 0x000D3E04
		public virtual void OnFocusLose(Agent userAgent)
		{
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				usableMissionObjectComponent.OnFocusLose(userAgent);
			}
		}

		// Token: 0x060033C1 RID: 13249 RVA: 0x000D5C58 File Offset: 0x000D3E58
		public virtual void OnPilotAssignedDuringSpawn()
		{
			Debug.FailedAssert("This method must have been overridden", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\Usables\\UsableMachine.cs", "OnPilotAssignedDuringSpawn", 615);
		}

		// Token: 0x060033C2 RID: 13250 RVA: 0x000D5C73 File Offset: 0x000D3E73
		public virtual TextObject GetInfoTextForBeingNotInteractable(Agent userAgent)
		{
			return null;
		}

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x060033C3 RID: 13251 RVA: 0x000D5C76 File Offset: 0x000D3E76
		public virtual bool HasWaitFrame
		{
			get
			{
				return this.ActiveWaitStandingPoint != null;
			}
		}

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x060033C4 RID: 13252 RVA: 0x000D5C84 File Offset: 0x000D3E84
		public MatrixFrame WaitFrame
		{
			get
			{
				if (this.ActiveWaitStandingPoint != null)
				{
					return this.ActiveWaitStandingPoint.GetGlobalFrame();
				}
				return MatrixFrame.Identity;
			}
		}

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x060033C5 RID: 13253 RVA: 0x000D5CA5 File Offset: 0x000D3EA5
		public GameEntity WaitEntity
		{
			get
			{
				return this.ActiveWaitStandingPoint;
			}
		}

		// Token: 0x060033C6 RID: 13254 RVA: 0x000D5CB0 File Offset: 0x000D3EB0
		protected internal override void OnMissionReset()
		{
			base.OnMissionReset();
			if (base.IsDisabled)
			{
				base.SetEnabled(false);
				base.SetScriptComponentToTick(this.GetTickRequirement());
			}
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				usableMissionObjectComponent.OnMissionReset();
			}
		}

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x060033C7 RID: 13255 RVA: 0x000D5D24 File Offset: 0x000D3F24
		public virtual bool IsDeactivated
		{
			get
			{
				return this._isMachineDeactivated || this.IsDestroyed;
			}
		}

		// Token: 0x060033C8 RID: 13256 RVA: 0x000D5D38 File Offset: 0x000D3F38
		public void Deactivate()
		{
			this._isMachineDeactivated = true;
			foreach (StandingPoint standingPoint in this.StandingPoints)
			{
				standingPoint.IsDeactivated = true;
			}
		}

		// Token: 0x060033C9 RID: 13257 RVA: 0x000D5D90 File Offset: 0x000D3F90
		public void Activate()
		{
			this._isMachineDeactivated = false;
			foreach (StandingPoint standingPoint in this.StandingPoints)
			{
				standingPoint.IsDeactivated = false;
			}
		}

		// Token: 0x060033CA RID: 13258 RVA: 0x000D5DE8 File Offset: 0x000D3FE8
		public virtual bool IsDisabledForBattleSide(BattleSideEnum sideEnum)
		{
			return this.IsDeactivated;
		}

		// Token: 0x060033CB RID: 13259 RVA: 0x000D5DF0 File Offset: 0x000D3FF0
		public virtual bool IsDisabledForBattleSideAI(BattleSideEnum sideEnum)
		{
			return base.IsDisabled || this.IsDisabledForAI || this.IsDeactivated || (this.EnemyRangeToStopUsing > 0f && sideEnum != BattleSideEnum.None && this.IsDisabledDueToEnemyInRange(sideEnum));
		}

		// Token: 0x060033CC RID: 13260 RVA: 0x000D5E28 File Offset: 0x000D4028
		public virtual bool ShouldAutoLeaveDetachmentWhenDisabled(BattleSideEnum sideEnum)
		{
			return true;
		}

		// Token: 0x060033CD RID: 13261 RVA: 0x000D5E2B File Offset: 0x000D402B
		protected bool IsDisabledDueToEnemyInRange(BattleSideEnum sideEnum)
		{
			if (sideEnum == BattleSideEnum.Attacker)
			{
				return this.IsDisabledForAttackerAIDueToEnemyInRange.Value;
			}
			return this.IsDisabledForDefenderAIDueToEnemyInRange.Value;
		}

		// Token: 0x060033CE RID: 13262 RVA: 0x000D5E48 File Offset: 0x000D4048
		public virtual bool AutoAttachUserToFormation(BattleSideEnum sideEnum)
		{
			return true;
		}

		// Token: 0x060033CF RID: 13263 RVA: 0x000D5E4B File Offset: 0x000D404B
		public virtual bool HasToBeDefendedByUser(BattleSideEnum sideEnum)
		{
			return false;
		}

		// Token: 0x060033D0 RID: 13264 RVA: 0x000D5E50 File Offset: 0x000D4050
		public virtual void Disable()
		{
			foreach (StandingPoint standingPoint in this.StandingPoints)
			{
				if (standingPoint.HasUser)
				{
					standingPoint.UserAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
				}
				if (standingPoint.HasAIMovingTo)
				{
					standingPoint.MovingAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
				}
			}
			foreach (Team team in Mission.Current.Teams.Where<Team>((Team t) => t.DetachmentManager.ContainsDetachment(this)))
			{
				team.DetachmentManager.DestroyDetachment(this);
			}
			foreach (StandingPoint standingPoint2 in this.StandingPoints)
			{
				if (!standingPoint2.GameEntity.HasTag(this.AmmoPickUpTag))
				{
					if (standingPoint2.HasUser)
					{
						standingPoint2.UserAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
					standingPoint2.SetIsDeactivatedSynched(true);
				}
			}
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				usableMissionObjectComponent.OnMissionObjectDisabled();
			}
			if (this.ShouldDisableTickIfMachineDisabled())
			{
				base.SetScriptComponentToTick(ScriptComponentBehavior.TickRequirement.None);
			}
			base.SetDisabled(false);
		}

		// Token: 0x060033D1 RID: 13265 RVA: 0x000D5FE0 File Offset: 0x000D41E0
		protected override void OnRemoved(int removeReason)
		{
			base.OnRemoved(removeReason);
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				usableMissionObjectComponent.OnRemoved();
			}
		}

		// Token: 0x060033D2 RID: 13266 RVA: 0x000D6038 File Offset: 0x000D4238
		public override string ToString()
		{
			string text = base.GetType() + " with Components:";
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				text = string.Concat(new object[] { text, "[", usableMissionObjectComponent, "]" });
			}
			return text;
		}

		// Token: 0x060033D3 RID: 13267
		public abstract TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject);

		// Token: 0x060033D4 RID: 13268 RVA: 0x000D60BC File Offset: 0x000D42BC
		public virtual StandingPoint GetBestPointAlternativeTo(StandingPoint standingPoint, Agent agent)
		{
			return standingPoint;
		}

		// Token: 0x060033D5 RID: 13269 RVA: 0x000D60C0 File Offset: 0x000D42C0
		public virtual bool IsInRangeToCheckAlternativePoints(Agent agent)
		{
			float num = ((this.StandingPoints.Count > 0) ? (agent.GetInteractionDistanceToUsable(this.StandingPoints[0]) + 1f) : 2f);
			return base.GameEntity.GlobalPosition.DistanceSquared(agent.Position) < num * num;
		}

		// Token: 0x060033D6 RID: 13270 RVA: 0x000D611C File Offset: 0x000D431C
		void IDetachment.OnFormationLeave(Formation formation)
		{
			foreach (StandingPoint standingPoint in this.StandingPoints)
			{
				Agent userAgent = standingPoint.UserAgent;
				if (userAgent != null && userAgent.Formation == formation && userAgent.IsAIControlled)
				{
					this.OnFormationLeaveHelper(formation, userAgent);
				}
				Agent movingAgent = standingPoint.MovingAgent;
				if (movingAgent != null && movingAgent.Formation == formation)
				{
					this.OnFormationLeaveHelper(formation, movingAgent);
				}
				for (int i = standingPoint.GetDefendingAgentCount() - 1; i >= 0; i--)
				{
					Agent agent = standingPoint.DefendingAgents[i];
					if (agent.Formation == formation)
					{
						this.OnFormationLeaveHelper(formation, agent);
					}
				}
			}
		}

		// Token: 0x060033D7 RID: 13271 RVA: 0x000D61E4 File Offset: 0x000D43E4
		private void OnFormationLeaveHelper(Formation formation, Agent agent)
		{
			((IDetachment)this).RemoveAgent(agent);
			formation.AttachUnit(agent);
		}

		// Token: 0x060033D8 RID: 13272 RVA: 0x000D61F4 File Offset: 0x000D43F4
		bool IDetachment.IsAgentUsingOrInterested(Agent agent)
		{
			foreach (StandingPoint standingPoint in this.StandingPoints)
			{
				if (agent.CurrentlyUsedGameObject == standingPoint || (agent.IsAIControlled && agent.AIInterestedInGameObject(standingPoint)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060033D9 RID: 13273 RVA: 0x000D6264 File Offset: 0x000D4464
		protected virtual float GetWeightOfStandingPoint(StandingPoint sp)
		{
			if (!sp.HasAIMovingTo)
			{
				return 0.6f;
			}
			return 0.2f;
		}

		// Token: 0x060033DA RID: 13274 RVA: 0x000D6279 File Offset: 0x000D4479
		float IDetachment.GetDetachmentWeight(BattleSideEnum side)
		{
			return this.GetDetachmentWeightAux(side);
		}

		// Token: 0x060033DB RID: 13275 RVA: 0x000D6284 File Offset: 0x000D4484
		protected virtual float GetDetachmentWeightAux(BattleSideEnum side)
		{
			if (this.IsDisabledForBattleSideAI(side))
			{
				return float.MinValue;
			}
			this.UsableStandingPoints.Clear();
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < this.StandingPoints.Count; i++)
			{
				StandingPoint standingPoint = this.StandingPoints[i];
				if (standingPoint.IsUsableBySide(side))
				{
					if (!standingPoint.HasAIMovingTo)
					{
						if (!flag2)
						{
							this.UsableStandingPoints.Clear();
						}
						flag2 = true;
					}
					else if (flag2 || standingPoint.MovingAgent.Formation.Team.Side != side)
					{
						goto IL_0081;
					}
					flag = true;
					this.UsableStandingPoints.Add(new ValueTuple<int, StandingPoint>(i, standingPoint));
				}
				IL_0081:;
			}
			this.AreUsableStandingPointsVacant = flag2;
			if (!flag)
			{
				return float.MinValue;
			}
			if (flag2)
			{
				return 1f;
			}
			if (!this.IsDetachmentRecentlyEvaluated)
			{
				return 0.1f;
			}
			return 0.01f;
		}

		// Token: 0x060033DC RID: 13276 RVA: 0x000D6350 File Offset: 0x000D4550
		void IDetachment.GetSlotIndexWeightTuples(List<ValueTuple<int, float>> slotIndexWeightTuples)
		{
			foreach (ValueTuple<int, StandingPoint> valueTuple in this.UsableStandingPoints)
			{
				StandingPoint item = valueTuple.Item2;
				slotIndexWeightTuples.Add(new ValueTuple<int, float>(valueTuple.Item1, this.GetWeightOfStandingPoint(item) * ((!this.AreUsableStandingPointsVacant && item.HasRecentlyBeenRechecked) ? 0.1f : 1f)));
			}
		}

		// Token: 0x060033DD RID: 13277 RVA: 0x000D63D8 File Offset: 0x000D45D8
		bool IDetachment.IsSlotAtIndexAvailableForAgent(int slotIndex, Agent agent)
		{
			return agent.CanBeAssignedForScriptedMovement() && !this.StandingPoints[slotIndex].IsDisabledForAgent(agent) && !this.IsAgentOnInconvenientNavmesh(agent, this.StandingPoints[slotIndex]);
		}

		// Token: 0x060033DE RID: 13278 RVA: 0x000D6410 File Offset: 0x000D4610
		protected virtual bool IsAgentOnInconvenientNavmesh(Agent agent, StandingPoint standingPoint)
		{
			if (Mission.Current.MissionTeamAIType != Mission.MissionTeamAITypeEnum.Siege)
			{
				return false;
			}
			int currentNavigationFaceId = agent.GetCurrentNavigationFaceId();
			TeamAISiegeComponent teamAISiegeComponent;
			if ((teamAISiegeComponent = agent.Team.TeamAI as TeamAISiegeComponent) != null)
			{
				if (teamAISiegeComponent is TeamAISiegeAttacker && currentNavigationFaceId % 10 == 1)
				{
					return true;
				}
				if (teamAISiegeComponent is TeamAISiegeDefender && currentNavigationFaceId % 10 != 1)
				{
					return true;
				}
				foreach (int num in teamAISiegeComponent.DifficultNavmeshIDs)
				{
					if (currentNavigationFaceId == num)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060033DF RID: 13279 RVA: 0x000D64B8 File Offset: 0x000D46B8
		bool IDetachment.IsAgentEligible(Agent agent)
		{
			return true;
		}

		// Token: 0x060033E0 RID: 13280 RVA: 0x000D64BC File Offset: 0x000D46BC
		public void AddAgentAtSlotIndex(Agent agent, int slotIndex)
		{
			StandingPoint standingPoint = this.StandingPoints[slotIndex];
			if (standingPoint.HasAIMovingTo)
			{
				Agent movingAgent = standingPoint.MovingAgent;
				if (movingAgent != null)
				{
					((IDetachment)this).RemoveAgent(movingAgent);
					Formation formation = movingAgent.Formation;
					if (formation != null)
					{
						formation.AttachUnit(movingAgent);
					}
				}
			}
			if (standingPoint.HasDefendingAgent)
			{
				for (int i = standingPoint.DefendingAgents.Count - 1; i >= 0; i--)
				{
					Agent agent2 = standingPoint.DefendingAgents[i];
					if (agent2 != null)
					{
						((IDetachment)this).RemoveAgent(agent2);
						Formation formation2 = agent2.Formation;
						if (formation2 != null)
						{
							formation2.AttachUnit(agent2);
						}
					}
				}
			}
			((IDetachment)this).AddAgent(agent, slotIndex, Agent.AIScriptedFrameFlags.None);
			Formation formation3 = agent.Formation;
			if (formation3 != null)
			{
				formation3.DetachUnit(agent, false);
			}
			agent.Detachment = this;
			agent.SetDetachmentWeight(1f);
		}

		// Token: 0x060033E1 RID: 13281 RVA: 0x000D6578 File Offset: 0x000D4778
		public void SetIsDisabledForAI(bool isDisabledForAI)
		{
			if (this.IsDisabledForAI != isDisabledForAI)
			{
				this.IsDisabledForAI = isDisabledForAI;
			}
		}

		// Token: 0x060033E2 RID: 13282 RVA: 0x000D658A File Offset: 0x000D478A
		Agent IDetachment.GetMovingAgentAtSlotIndex(int slotIndex)
		{
			return this.StandingPoints[slotIndex].MovingAgent;
		}

		// Token: 0x060033E3 RID: 13283 RVA: 0x000D659D File Offset: 0x000D479D
		bool IDetachment.IsDetachmentRecentlyEvaluated()
		{
			return this.IsDetachmentRecentlyEvaluated;
		}

		// Token: 0x060033E4 RID: 13284 RVA: 0x000D65A5 File Offset: 0x000D47A5
		void IDetachment.UnmarkDetachment()
		{
			this.IsDetachmentRecentlyEvaluated = false;
		}

		// Token: 0x060033E5 RID: 13285 RVA: 0x000D65B0 File Offset: 0x000D47B0
		void IDetachment.MarkSlotAtIndex(int slotIndex)
		{
			int count = this.UsableStandingPoints.Count;
			int num = this._reevaluatedCount + 1;
			this._reevaluatedCount = num;
			if (num >= count)
			{
				foreach (ValueTuple<int, StandingPoint> valueTuple in this.UsableStandingPoints)
				{
					valueTuple.Item2.HasRecentlyBeenRechecked = false;
				}
				this.IsDetachmentRecentlyEvaluated = true;
				this._reevaluatedCount = 0;
				return;
			}
			this.StandingPoints[slotIndex].HasRecentlyBeenRechecked = true;
		}

		// Token: 0x060033E6 RID: 13286 RVA: 0x000D6648 File Offset: 0x000D4848
		float? IDetachment.GetWeightOfNextSlot(BattleSideEnum side)
		{
			if (this.IsDisabledForBattleSideAI(side))
			{
				return null;
			}
			StandingPoint suitableStandingPointFor = this.GetSuitableStandingPointFor(side, null, null, null);
			if (suitableStandingPointFor != null)
			{
				return new float?(this.GetWeightOfStandingPoint(suitableStandingPointFor));
			}
			return null;
		}

		// Token: 0x060033E7 RID: 13287 RVA: 0x000D668C File Offset: 0x000D488C
		float IDetachment.GetExactCostOfAgentAtSlot(Agent candidate, int slotIndex)
		{
			StandingPoint standingPoint = this.StandingPoints[slotIndex];
			Vec3 globalPosition = standingPoint.GameEntity.GlobalPosition;
			WorldPosition worldPosition = new WorldPosition(candidate.Mission.Scene, globalPosition);
			WorldPosition worldPosition2 = candidate.GetWorldPosition();
			float maxValue;
			if (!standingPoint.Scene.GetPathDistanceBetweenPositions(ref worldPosition, ref worldPosition2, candidate.Monster.BodyCapsuleRadius, out maxValue))
			{
				maxValue = float.MaxValue;
			}
			return maxValue;
		}

		// Token: 0x060033E8 RID: 13288 RVA: 0x000D66F4 File Offset: 0x000D48F4
		List<float> IDetachment.GetTemplateCostsOfAgent(Agent candidate, List<float> oldValue)
		{
			List<float> list = oldValue ?? new List<float>(this.StandingPoints.Count);
			list.Clear();
			for (int i = 0; i < this.StandingPoints.Count; i++)
			{
				list.Add(float.MaxValue);
			}
			foreach (ValueTuple<int, StandingPoint> valueTuple in this.UsableStandingPoints)
			{
				float num = valueTuple.Item2.GameEntity.GlobalPosition.Distance(candidate.Position);
				list[valueTuple.Item1] = num * MissionGameModels.Current.AgentStatCalculateModel.GetDetachmentCostMultiplierOfAgent(candidate, this);
			}
			return list;
		}

		// Token: 0x060033E9 RID: 13289 RVA: 0x000D67C4 File Offset: 0x000D49C4
		float IDetachment.GetTemplateWeightOfAgent(Agent candidate)
		{
			Scene scene = Mission.Current.Scene;
			Vec3 globalPosition = base.GameEntity.GlobalPosition;
			WorldPosition worldPosition = candidate.GetWorldPosition();
			WorldPosition worldPosition2 = new WorldPosition(scene, UIntPtr.Zero, globalPosition, true);
			float maxValue;
			if (!scene.GetPathDistanceBetweenPositions(ref worldPosition2, ref worldPosition, candidate.Monster.BodyCapsuleRadius, out maxValue))
			{
				maxValue = float.MaxValue;
			}
			return maxValue;
		}

		// Token: 0x060033EA RID: 13290 RVA: 0x000D6824 File Offset: 0x000D4A24
		float IDetachment.GetWeightOfOccupiedSlot(Agent agent)
		{
			return this.GetWeightOfStandingPoint(this.StandingPoints.FirstOrDefault<StandingPoint>((StandingPoint sp) => sp.UserAgent == agent || sp.IsAIMovingTo(agent)));
		}

		// Token: 0x060033EB RID: 13291 RVA: 0x000D685C File Offset: 0x000D4A5C
		WorldFrame? IDetachment.GetAgentFrame(Agent agent)
		{
			return null;
		}

		// Token: 0x060033EC RID: 13292 RVA: 0x000D6872 File Offset: 0x000D4A72
		void IDetachment.RemoveAgent(Agent agent)
		{
			agent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.None);
		}

		// Token: 0x060033ED RID: 13293 RVA: 0x000D687C File Offset: 0x000D4A7C
		public int GetNumberOfUsableSlots()
		{
			return this.UsableStandingPoints.Count;
		}

		// Token: 0x060033EE RID: 13294 RVA: 0x000D688C File Offset: 0x000D4A8C
		public bool IsStandingPointAvailableForAgent(Agent agent)
		{
			bool flag = false;
			foreach (StandingPoint standingPoint in this.StandingPoints)
			{
				if (!standingPoint.IsDeactivated && (standingPoint.IsInstantUse || ((!standingPoint.HasUser || standingPoint.UserAgent == agent) && (!standingPoint.HasAIMovingTo || standingPoint.IsAIMovingTo(agent)))) && !standingPoint.IsDisabledForAgent(agent) && !this.IsStandingPointNotUsedOnAccountOfBeingAmmoLoad(standingPoint))
				{
					flag = true;
					break;
				}
			}
			return flag;
		}

		// Token: 0x060033EF RID: 13295 RVA: 0x000D6924 File Offset: 0x000D4B24
		float? IDetachment.GetWeightOfAgentAtNextSlot(List<Agent> candidates, out Agent match)
		{
			BattleSideEnum side = candidates[0].Team.Side;
			StandingPoint suitableStandingPointFor = this.GetSuitableStandingPointFor(side, null, candidates, null);
			if (suitableStandingPointFor == null)
			{
				match = null;
				return null;
			}
			match = UsableMachineAIBase.GetSuitableAgentForStandingPoint(this, suitableStandingPointFor, candidates, new List<Agent>());
			if (match == null)
			{
				return null;
			}
			float? weightOfNextSlot = ((IDetachment)this).GetWeightOfNextSlot(side);
			float num = 1f;
			float? num2 = weightOfNextSlot;
			float num3 = num;
			if (num2 == null)
			{
				return null;
			}
			return new float?(num2.GetValueOrDefault() * num3);
		}

		// Token: 0x060033F0 RID: 13296 RVA: 0x000D69B0 File Offset: 0x000D4BB0
		float? IDetachment.GetWeightOfAgentAtNextSlot(List<ValueTuple<Agent, float>> candidates, out Agent match)
		{
			BattleSideEnum side = candidates[0].Item1.Team.Side;
			StandingPoint suitableStandingPointFor = this.GetSuitableStandingPointFor(side, null, null, candidates);
			if (suitableStandingPointFor == null)
			{
				match = null;
				return null;
			}
			float? weightOfNextSlot = ((IDetachment)this).GetWeightOfNextSlot(side);
			match = UsableMachineAIBase.GetSuitableAgentForStandingPoint(this, suitableStandingPointFor, candidates, new List<Agent>(), weightOfNextSlot.Value);
			if (match == null)
			{
				return null;
			}
			float num = 1f;
			float? num2 = weightOfNextSlot;
			float num3 = num;
			if (num2 == null)
			{
				return null;
			}
			return new float?(num2.GetValueOrDefault() * num3);
		}

		// Token: 0x060033F1 RID: 13297 RVA: 0x000D6A48 File Offset: 0x000D4C48
		float? IDetachment.GetWeightOfAgentAtOccupiedSlot(Agent detachedAgent, List<Agent> candidates, out Agent match)
		{
			BattleSideEnum side = candidates[0].Team.Side;
			match = null;
			foreach (StandingPoint standingPoint in this.StandingPoints)
			{
				if (standingPoint.IsAIMovingTo(detachedAgent) || standingPoint.UserAgent == detachedAgent)
				{
					match = UsableMachineAIBase.GetSuitableAgentForStandingPoint(this, standingPoint, candidates, new List<Agent>());
					break;
				}
			}
			if (match == null)
			{
				return null;
			}
			float? weightOfNextSlot = ((IDetachment)this).GetWeightOfNextSlot(side);
			float num = 1f;
			if (weightOfNextSlot == null)
			{
				return null;
			}
			return new float?(weightOfNextSlot.GetValueOrDefault() * num * 0.5f);
		}

		// Token: 0x060033F2 RID: 13298 RVA: 0x000D6B14 File Offset: 0x000D4D14
		void IDetachment.AddAgent(Agent agent, int slotIndex, Agent.AIScriptedFrameFlags customFlags)
		{
			StandingPoint standingPoint = ((slotIndex == -1) ? this.GetSuitableStandingPointFor(agent.Team.Side, agent, null, null) : this.StandingPoints[slotIndex]);
			if (standingPoint != null)
			{
				if (standingPoint.HasAIMovingTo && !standingPoint.IsInstantUse)
				{
					standingPoint.MovingAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
				}
				while (standingPoint.HasDefendingAgent)
				{
					standingPoint.DefendingAgents[0].StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
				}
				if (customFlags == Agent.AIScriptedFrameFlags.None)
				{
					customFlags = this.Ai.GetScriptedFrameFlags(agent);
				}
				agent.AIMoveToGameObjectEnable(standingPoint, this, customFlags);
				if (standingPoint.GameEntity.HasTag(this.AmmoPickUpTag))
				{
					this.CurrentlyUsedAmmoPickUpPoint = standingPoint;
					return;
				}
			}
			else
			{
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\Usables\\UsableMachine.cs", "AddAgent", 1457);
			}
		}

		// Token: 0x060033F3 RID: 13299 RVA: 0x000D6BD6 File Offset: 0x000D4DD6
		void IDetachment.FormationStartUsing(Formation formation)
		{
			this._userFormations.Add(formation);
		}

		// Token: 0x060033F4 RID: 13300 RVA: 0x000D6BE4 File Offset: 0x000D4DE4
		void IDetachment.FormationStopUsing(Formation formation)
		{
			this._userFormations.Remove(formation);
		}

		// Token: 0x060033F5 RID: 13301 RVA: 0x000D6BF3 File Offset: 0x000D4DF3
		public bool IsUsedByFormation(Formation formation)
		{
			return this._userFormations.Contains(formation);
		}

		// Token: 0x060033F6 RID: 13302 RVA: 0x000D6C01 File Offset: 0x000D4E01
		void IDetachment.ResetEvaluation()
		{
			this._isEvaluated = false;
		}

		// Token: 0x060033F7 RID: 13303 RVA: 0x000D6C0A File Offset: 0x000D4E0A
		bool IDetachment.IsEvaluated()
		{
			return this._isEvaluated;
		}

		// Token: 0x060033F8 RID: 13304 RVA: 0x000D6C12 File Offset: 0x000D4E12
		void IDetachment.SetAsEvaluated()
		{
			this._isEvaluated = true;
		}

		// Token: 0x060033F9 RID: 13305 RVA: 0x000D6C1B File Offset: 0x000D4E1B
		float IDetachment.GetDetachmentWeightFromCache()
		{
			return this._cachedDetachmentWeight;
		}

		// Token: 0x060033FA RID: 13306 RVA: 0x000D6C23 File Offset: 0x000D4E23
		float IDetachment.ComputeAndCacheDetachmentWeight(BattleSideEnum side)
		{
			this._cachedDetachmentWeight = this.GetDetachmentWeightAux(side);
			return this._cachedDetachmentWeight;
		}

		// Token: 0x060033FB RID: 13307 RVA: 0x000D6C38 File Offset: 0x000D4E38
		protected internal virtual bool IsStandingPointNotUsedOnAccountOfBeingAmmoLoad(StandingPoint standingPoint)
		{
			return this.AmmoPickUpPoints.Contains(standingPoint) && (this.StandingPoints.Any<StandingPoint>((StandingPoint standingPoint2) => (standingPoint2.IsDeactivated || standingPoint2.HasUser || standingPoint2.HasAIMovingTo) && !standingPoint2.GameEntity.HasTag(this.AmmoPickUpTag) && standingPoint2 is StandingPointWithWeaponRequirement) || this.HasAIPickingUpAmmo);
		}

		// Token: 0x060033FC RID: 13308 RVA: 0x000D6C70 File Offset: 0x000D4E70
		protected virtual StandingPoint GetSuitableStandingPointFor(BattleSideEnum side, Agent agent = null, List<Agent> agents = null, List<ValueTuple<Agent, float>> agentValuePairs = null)
		{
			return this.StandingPoints.FirstOrDefault<StandingPoint>((StandingPoint sp) => !sp.IsDeactivated && (sp.IsInstantUse || (!sp.HasUser && !sp.HasAIMovingTo)) && (agent == null || !sp.IsDisabledForAgent(agent)) && (agents == null || agents.Any<Agent>((Agent a) => !sp.IsDisabledForAgent(a))) && (agentValuePairs == null || agentValuePairs.Any<ValueTuple<Agent, float>>((ValueTuple<Agent, float> avp) => !sp.IsDisabledForAgent(avp.Item1))) && !this.IsStandingPointNotUsedOnAccountOfBeingAmmoLoad(sp));
		}

		// Token: 0x060033FD RID: 13309
		public abstract TextObject GetDescriptionText(WeakGameEntity gameEntity);

		// Token: 0x060033FE RID: 13310 RVA: 0x000D6CB7 File Offset: 0x000D4EB7
		protected virtual bool ShouldDisableTickIfMachineDisabled()
		{
			return true;
		}

		// Token: 0x060033FF RID: 13311 RVA: 0x000D6CBA File Offset: 0x000D4EBA
		public void SetEnemyRangeToStopUsing(float value)
		{
			this.EnemyRangeToStopUsing = value;
		}

		// Token: 0x040015F8 RID: 5624
		public const string UsableMachineParentTag = "machine_parent";

		// Token: 0x040015F9 RID: 5625
		public string PilotStandingPointTag = "Pilot";

		// Token: 0x040015FA RID: 5626
		public string AmmoPickUpTag = "ammopickup";

		// Token: 0x040015FB RID: 5627
		public string WaitStandingPointTag = "Wait";

		// Token: 0x04001601 RID: 5633
		protected GameEntity ActiveWaitStandingPoint;

		// Token: 0x04001602 RID: 5634
		private readonly List<UsableMissionObjectComponent> _components;

		// Token: 0x04001604 RID: 5636
		protected bool AreUsableStandingPointsVacant = true;

		// Token: 0x04001606 RID: 5638
		protected List<ValueTuple<int, StandingPoint>> UsableStandingPoints;

		// Token: 0x04001607 RID: 5639
		private int _reevaluatedCount;

		// Token: 0x04001608 RID: 5640
		private bool _isEvaluated;

		// Token: 0x04001609 RID: 5641
		private float _cachedDetachmentWeight;

		// Token: 0x0400160A RID: 5642
		protected float EnemyRangeToStopUsing;

		// Token: 0x0400160B RID: 5643
		protected Vec2 MachinePositionOffsetToStopUsingLocal = Vec2.Zero;

		// Token: 0x0400160C RID: 5644
		protected bool MakeVisibilityCheck = true;

		// Token: 0x0400160D RID: 5645
		private UsableMachineAIBase _ai;

		// Token: 0x0400160E RID: 5646
		private StandingPoint _currentlyUsedAmmoPickUpPoint;

		// Token: 0x0400160F RID: 5647
		protected QueryData<bool> IsDisabledForAttackerAIDueToEnemyInRange;

		// Token: 0x04001610 RID: 5648
		protected QueryData<bool> IsDisabledForDefenderAIDueToEnemyInRange;

		// Token: 0x04001612 RID: 5650
		private MBList<Formation> _userFormations;

		// Token: 0x04001613 RID: 5651
		private bool _isMachineDeactivated;
	}
}
