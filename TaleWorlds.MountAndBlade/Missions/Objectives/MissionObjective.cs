using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Missions.Objectives
{
	// Token: 0x020003EE RID: 1006
	public abstract class MissionObjective
	{
		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x060037AC RID: 14252
		public abstract string UniqueId { get; }

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x060037AD RID: 14253
		public abstract TextObject Name { get; }

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x060037AE RID: 14254
		public abstract TextObject Description { get; }

		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x060037AF RID: 14255 RVA: 0x000E767C File Offset: 0x000E587C
		public bool IsActive
		{
			get
			{
				return this.IsStarted && !this.IsCompleted;
			}
		}

		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x060037B0 RID: 14256 RVA: 0x000E7691 File Offset: 0x000E5891
		// (set) Token: 0x060037B1 RID: 14257 RVA: 0x000E7699 File Offset: 0x000E5899
		public bool IsStarted { get; private set; }

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x060037B2 RID: 14258 RVA: 0x000E76A2 File Offset: 0x000E58A2
		// (set) Token: 0x060037B3 RID: 14259 RVA: 0x000E76AA File Offset: 0x000E58AA
		public bool IsCompleted { get; private set; }

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x060037B4 RID: 14260 RVA: 0x000E76B3 File Offset: 0x000E58B3
		// (set) Token: 0x060037B5 RID: 14261 RVA: 0x000E76BB File Offset: 0x000E58BB
		public Mission Mission { get; private set; }

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x060037B6 RID: 14262 RVA: 0x000E76C4 File Offset: 0x000E58C4
		// (set) Token: 0x060037B7 RID: 14263 RVA: 0x000E76CC File Offset: 0x000E58CC
		public BasicCharacterObject ObjectiveGiver { get; private set; }

		// Token: 0x140000AE RID: 174
		// (add) Token: 0x060037B8 RID: 14264 RVA: 0x000E76D8 File Offset: 0x000E58D8
		// (remove) Token: 0x060037B9 RID: 14265 RVA: 0x000E7710 File Offset: 0x000E5910
		public event Action OnUpdated;

		// Token: 0x060037BA RID: 14266 RVA: 0x000E7745 File Offset: 0x000E5945
		public MissionObjective(Mission mission)
		{
			this._targets = new MBList<MissionObjectiveTarget>();
			this.Mission = mission;
		}

		// Token: 0x060037BB RID: 14267 RVA: 0x000E7760 File Offset: 0x000E5960
		internal void Start()
		{
			if (this.IsStarted)
			{
				Debug.FailedAssert("Trying to start an objective that was already started.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Objectives\\MissionObjective.cs", "Start", 38);
				return;
			}
			if (this.IsCompleted)
			{
				Debug.FailedAssert("Trying to start a completed objective. This is not allowed, create a new objective instead.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Objectives\\MissionObjective.cs", "Start", 44);
				return;
			}
			this.IsStarted = true;
			this.OnStart();
		}

		// Token: 0x060037BC RID: 14268 RVA: 0x000E77B8 File Offset: 0x000E59B8
		internal void Tick(float dt)
		{
			this.CheckNameUpdates();
			this.OnTick(dt);
		}

		// Token: 0x060037BD RID: 14269 RVA: 0x000E77C8 File Offset: 0x000E59C8
		internal void Complete()
		{
			if (!this.IsStarted)
			{
				Debug.FailedAssert("Trying to complete an objective that was not started yet.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Objectives\\MissionObjective.cs", "Complete", 62);
				return;
			}
			if (this.IsCompleted)
			{
				Debug.FailedAssert("Trying to complete an objective more than once.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Objectives\\MissionObjective.cs", "Complete", 68);
				return;
			}
			this.IsCompleted = true;
			this.OnComplete();
		}

		// Token: 0x060037BE RID: 14270 RVA: 0x000E7820 File Offset: 0x000E5A20
		private void CheckNameUpdates()
		{
			bool flag = false;
			if (this._cachedName != this.Name)
			{
				this._cachedName = this.Name;
				flag = true;
			}
			if (this._cachedDescription != this.Description)
			{
				this._cachedDescription = this.Description;
				flag = true;
			}
			if (flag)
			{
				Action onUpdated = this.OnUpdated;
				if (onUpdated == null)
				{
					return;
				}
				onUpdated();
			}
		}

		// Token: 0x060037BF RID: 14271 RVA: 0x000E7884 File Offset: 0x000E5A84
		public virtual MissionObjectiveProgressInfo GetCurrentProgress()
		{
			return default(MissionObjectiveProgressInfo);
		}

		// Token: 0x060037C0 RID: 14272 RVA: 0x000E789A File Offset: 0x000E5A9A
		internal bool GetIsActivationRequirementsMet()
		{
			return this.IsActivationRequirementsMet();
		}

		// Token: 0x060037C1 RID: 14273 RVA: 0x000E78A2 File Offset: 0x000E5AA2
		internal bool GetIsCompletionRequirementsMet()
		{
			return this.IsCompletionRequirementsMet();
		}

		// Token: 0x060037C2 RID: 14274 RVA: 0x000E78AA File Offset: 0x000E5AAA
		public void SetObjectiveGiver(BasicCharacterObject objectiveGiver)
		{
			this.ObjectiveGiver = objectiveGiver;
			Action onUpdated = this.OnUpdated;
			if (onUpdated == null)
			{
				return;
			}
			onUpdated();
		}

		// Token: 0x060037C3 RID: 14275 RVA: 0x000E78C4 File Offset: 0x000E5AC4
		public void AddTarget(MissionObjectiveTarget target)
		{
			if (target == null)
			{
				Debug.FailedAssert("Cannot add null target to mission objective", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Objectives\\MissionObjective.cs", "AddTarget", 123);
				return;
			}
			if (this._targets.Contains(target))
			{
				Debug.FailedAssert(string.Concat(new string[]
				{
					"Trying to add target (",
					target.GetName().ToString(),
					") twice to mission objective (",
					this.UniqueId,
					")"
				}), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Objectives\\MissionObjective.cs", "AddTarget", 129);
				return;
			}
			this._targets.Add(target);
			Action onUpdated = this.OnUpdated;
			if (onUpdated == null)
			{
				return;
			}
			onUpdated();
		}

		// Token: 0x060037C4 RID: 14276 RVA: 0x000E7964 File Offset: 0x000E5B64
		public void RemoveTarget(MissionObjectiveTarget target)
		{
			if (target == null)
			{
				Debug.FailedAssert("Cannot remove null target from mission objective", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Objectives\\MissionObjective.cs", "RemoveTarget", 141);
				return;
			}
			if (!this._targets.Contains(target))
			{
				Debug.FailedAssert(string.Concat(new string[]
				{
					"Trying to remove non-existent target (",
					target.GetName().ToString(),
					") from objective (",
					this.UniqueId,
					")"
				}), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Objectives\\MissionObjective.cs", "RemoveTarget", 147);
				return;
			}
			this._targets.Remove(target);
			Action onUpdated = this.OnUpdated;
			if (onUpdated == null)
			{
				return;
			}
			onUpdated();
		}

		// Token: 0x060037C5 RID: 14277 RVA: 0x000E7A08 File Offset: 0x000E5C08
		public void ClearTargets()
		{
			this._targets.Clear();
			Action onUpdated = this.OnUpdated;
			if (onUpdated == null)
			{
				return;
			}
			onUpdated();
		}

		// Token: 0x060037C6 RID: 14278 RVA: 0x000E7A25 File Offset: 0x000E5C25
		public MBReadOnlyList<MissionObjectiveTarget> GetTargetsCopy()
		{
			return this._targets.ToMBList<MissionObjectiveTarget>();
		}

		// Token: 0x060037C7 RID: 14279 RVA: 0x000E7A34 File Offset: 0x000E5C34
		protected MBReadOnlyList<TTarget> GetTargetsCopy<TTarget>() where TTarget : MissionObjectiveTarget
		{
			MBList<TTarget> mblist = new MBList<TTarget>();
			for (int i = 0; i < this._targets.Count; i++)
			{
				TTarget ttarget;
				if ((ttarget = this._targets[i] as TTarget) != null)
				{
					mblist.Add(ttarget);
				}
			}
			return mblist;
		}

		// Token: 0x060037C8 RID: 14280 RVA: 0x000E7A84 File Offset: 0x000E5C84
		protected virtual bool IsActivationRequirementsMet()
		{
			return true;
		}

		// Token: 0x060037C9 RID: 14281 RVA: 0x000E7A87 File Offset: 0x000E5C87
		protected virtual bool IsCompletionRequirementsMet()
		{
			return false;
		}

		// Token: 0x060037CA RID: 14282 RVA: 0x000E7A8A File Offset: 0x000E5C8A
		protected virtual void OnStart()
		{
		}

		// Token: 0x060037CB RID: 14283 RVA: 0x000E7A8C File Offset: 0x000E5C8C
		protected virtual void OnComplete()
		{
		}

		// Token: 0x060037CC RID: 14284 RVA: 0x000E7A8E File Offset: 0x000E5C8E
		protected virtual void OnTick(float dt)
		{
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x000E7A90 File Offset: 0x000E5C90
		protected virtual void OnTargetAdded(MissionObjectiveTarget target)
		{
		}

		// Token: 0x060037CE RID: 14286 RVA: 0x000E7A92 File Offset: 0x000E5C92
		protected virtual void OnTargetRemoved(MissionObjectiveTarget target)
		{
		}

		// Token: 0x060037CF RID: 14287 RVA: 0x000E7A94 File Offset: 0x000E5C94
		protected virtual void OnTargetsCleared()
		{
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x000E7A98 File Offset: 0x000E5C98
		public static MissionObjective.GenericMissionObjectiveBuilder CreateGenericObjectiveBuilder(Mission mission, string id, TextObject name = null, TextObject description = null)
		{
			GenericMissionObjective genericMissionObjective = new GenericMissionObjective(mission, id, name, description);
			return new MissionObjective.GenericMissionObjectiveBuilder
			{
				Objective = genericMissionObjective
			};
		}

		// Token: 0x060037D1 RID: 14289 RVA: 0x000E7AC0 File Offset: 0x000E5CC0
		public static MissionObjective.GenericMissionObjectiveTargetBuilder<T> CreateGenericTargetBuilder<T>(T target, TextObject name, Vec3 staticPosition)
		{
			GenericMissionObjectiveTarget<T> genericMissionObjectiveTarget = new GenericMissionObjectiveTarget<T>(target);
			genericMissionObjectiveTarget.Name = name;
			genericMissionObjectiveTarget.StaticPosition = staticPosition;
			return new MissionObjective.GenericMissionObjectiveTargetBuilder<T>
			{
				Target = genericMissionObjectiveTarget
			};
		}

		// Token: 0x060037D2 RID: 14290 RVA: 0x000E7AF3 File Offset: 0x000E5CF3
		public static MissionObjective.GenericMissionObjectiveTargetBuilder<T> CreateGenericTargetBuilder<T>(T target)
		{
			return MissionObjective.CreateGenericTargetBuilder<T>(target, null, Vec3.Invalid);
		}

		// Token: 0x060037D3 RID: 14291 RVA: 0x000E7B01 File Offset: 0x000E5D01
		public static MissionObjective.GenericMissionObjectiveTargetBuilder<T> CreateGenericTargetBuilder<T>(T target, TextObject name)
		{
			return MissionObjective.CreateGenericTargetBuilder<T>(target, name, Vec3.Invalid);
		}

		// Token: 0x060037D4 RID: 14292 RVA: 0x000E7B0F File Offset: 0x000E5D0F
		public static MissionObjective.GenericMissionObjectiveTargetBuilder<T> CreateGenericTargetBuilder<T>(T target, Vec3 staticPosition)
		{
			return MissionObjective.CreateGenericTargetBuilder<T>(target, null, staticPosition);
		}

		// Token: 0x0400181B RID: 6171
		private MBList<MissionObjectiveTarget> _targets;

		// Token: 0x0400181D RID: 6173
		private TextObject _cachedName;

		// Token: 0x0400181E RID: 6174
		private TextObject _cachedDescription;

		// Token: 0x020006A5 RID: 1701
		public struct GenericMissionObjectiveBuilder
		{
			// Token: 0x06004289 RID: 17033 RVA: 0x00100843 File Offset: 0x000FEA43
			public MissionObjective.GenericMissionObjectiveBuilder SetName(TextObject name)
			{
				this.Objective.IName = name;
				return this;
			}

			// Token: 0x0600428A RID: 17034 RVA: 0x00100857 File Offset: 0x000FEA57
			public MissionObjective.GenericMissionObjectiveBuilder SetDescription(TextObject description)
			{
				this.Objective.IDescription = description;
				return this;
			}

			// Token: 0x0600428B RID: 17035 RVA: 0x0010086B File Offset: 0x000FEA6B
			public MissionObjective.GenericMissionObjectiveBuilder SetObjectiveGiver(BasicCharacterObject objectiveGiver)
			{
				this.Objective.SetObjectiveGiver(objectiveGiver);
				return this;
			}

			// Token: 0x0600428C RID: 17036 RVA: 0x00100880 File Offset: 0x000FEA80
			public MissionObjective.GenericMissionObjectiveBuilder SetInitialTargets(params MissionObjectiveTarget[] targets)
			{
				this.Objective.ClearTargets();
				if (targets != null)
				{
					for (int i = 0; i < targets.Length; i++)
					{
						this.Objective.AddTarget(targets[i]);
					}
				}
				return this;
			}

			// Token: 0x0600428D RID: 17037 RVA: 0x001008BD File Offset: 0x000FEABD
			public MissionObjective.GenericMissionObjectiveBuilder SetIsActivationRequirementsMetCallback(Func<MissionObjective, bool> callback)
			{
				this.Objective.IsActivationRequirementsMetCallback = callback;
				return this;
			}

			// Token: 0x0600428E RID: 17038 RVA: 0x001008D1 File Offset: 0x000FEAD1
			public MissionObjective.GenericMissionObjectiveBuilder SetIsCompletionRequirementsMetCallback(Func<MissionObjective, bool> callback)
			{
				this.Objective.IsCompletionRequirementsMetCallback = callback;
				return this;
			}

			// Token: 0x0600428F RID: 17039 RVA: 0x001008E5 File Offset: 0x000FEAE5
			public MissionObjective.GenericMissionObjectiveBuilder SetOnStartCallback(Action<MissionObjective> callback)
			{
				this.Objective.OnStartCallback = callback;
				return this;
			}

			// Token: 0x06004290 RID: 17040 RVA: 0x001008F9 File Offset: 0x000FEAF9
			public MissionObjective.GenericMissionObjectiveBuilder SetOnCompleteCallback(Action<MissionObjective> callback)
			{
				this.Objective.OnCompleteCallback = callback;
				return this;
			}

			// Token: 0x06004291 RID: 17041 RVA: 0x0010090D File Offset: 0x000FEB0D
			public MissionObjective.GenericMissionObjectiveBuilder SetOnTickCallback(Action<MissionObjective, float> callback)
			{
				this.Objective.OnTickCallback = callback;
				return this;
			}

			// Token: 0x06004292 RID: 17042 RVA: 0x00100921 File Offset: 0x000FEB21
			public MissionObjective.GenericMissionObjectiveBuilder SetProgressCallback(Func<MissionObjective, MissionObjectiveProgressInfo> callback)
			{
				this.Objective.GetProgressCallback = callback;
				return this;
			}

			// Token: 0x06004293 RID: 17043 RVA: 0x00100935 File Offset: 0x000FEB35
			public MissionObjective Build()
			{
				return this.Objective;
			}

			// Token: 0x04002356 RID: 9046
			internal GenericMissionObjective Objective;
		}

		// Token: 0x020006A6 RID: 1702
		public struct GenericMissionObjectiveTargetBuilder<T>
		{
			// Token: 0x06004294 RID: 17044 RVA: 0x0010093D File Offset: 0x000FEB3D
			public MissionObjective.GenericMissionObjectiveTargetBuilder<T> SetIsActiveCallback(Func<T, bool> callback)
			{
				this.Target.IsActiveCallback = callback;
				return this;
			}

			// Token: 0x06004295 RID: 17045 RVA: 0x00100951 File Offset: 0x000FEB51
			public MissionObjective.GenericMissionObjectiveTargetBuilder<T> SetGetGlobalPositionCallback(Func<T, Vec3> callback)
			{
				this.Target.GetGlobalPositionCallback = callback;
				return this;
			}

			// Token: 0x06004296 RID: 17046 RVA: 0x00100965 File Offset: 0x000FEB65
			public MissionObjective.GenericMissionObjectiveTargetBuilder<T> SetGetNameCallback(Func<T, TextObject> callback)
			{
				this.Target.GetNameCallback = callback;
				return this;
			}

			// Token: 0x06004297 RID: 17047 RVA: 0x00100979 File Offset: 0x000FEB79
			public MissionObjectiveTarget<T> Build()
			{
				return this.Target;
			}

			// Token: 0x04002357 RID: 9047
			internal GenericMissionObjectiveTarget<T> Target;
		}
	}
}
