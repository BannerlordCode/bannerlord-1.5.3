using System;
using System.Collections.Generic;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Missions.Objectives
{
	// Token: 0x020003ED RID: 1005
	internal class GenericMissionObjective : MissionObjective
	{
		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x060037A2 RID: 14242 RVA: 0x000E7596 File Offset: 0x000E5796
		public override string UniqueId
		{
			get
			{
				return this.IUniqueId;
			}
		}

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x060037A3 RID: 14243 RVA: 0x000E759E File Offset: 0x000E579E
		public override TextObject Name
		{
			get
			{
				return this.IName;
			}
		}

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x060037A4 RID: 14244 RVA: 0x000E75A6 File Offset: 0x000E57A6
		public override TextObject Description
		{
			get
			{
				return this.IDescription;
			}
		}

		// Token: 0x060037A5 RID: 14245 RVA: 0x000E75AE File Offset: 0x000E57AE
		public GenericMissionObjective(Mission mission, string id, TextObject name, TextObject description)
			: base(mission)
		{
			this._targets = new List<MissionObjectiveTarget>();
			this.IUniqueId = id;
			this.IName = name;
			this.IDescription = description;
		}

		// Token: 0x060037A6 RID: 14246 RVA: 0x000E75D8 File Offset: 0x000E57D8
		public override MissionObjectiveProgressInfo GetCurrentProgress()
		{
			Func<MissionObjective, MissionObjectiveProgressInfo> getProgressCallback = this.GetProgressCallback;
			if (getProgressCallback == null)
			{
				return default(MissionObjectiveProgressInfo);
			}
			return getProgressCallback(this);
		}

		// Token: 0x060037A7 RID: 14247 RVA: 0x000E75FF File Offset: 0x000E57FF
		protected override bool IsActivationRequirementsMet()
		{
			return this.IsActivationRequirementsMetCallback == null || this.IsActivationRequirementsMetCallback(this);
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x000E7617 File Offset: 0x000E5817
		protected override bool IsCompletionRequirementsMet()
		{
			return this.IsCompletionRequirementsMetCallback == null || this.IsCompletionRequirementsMetCallback(this);
		}

		// Token: 0x060037A9 RID: 14249 RVA: 0x000E762F File Offset: 0x000E582F
		protected override void OnStart()
		{
			base.OnStart();
			Action<MissionObjective> onStartCallback = this.OnStartCallback;
			if (onStartCallback == null)
			{
				return;
			}
			onStartCallback(this);
		}

		// Token: 0x060037AA RID: 14250 RVA: 0x000E7648 File Offset: 0x000E5848
		protected override void OnComplete()
		{
			base.OnComplete();
			Action<MissionObjective> onCompleteCallback = this.OnCompleteCallback;
			if (onCompleteCallback == null)
			{
				return;
			}
			onCompleteCallback(this);
		}

		// Token: 0x060037AB RID: 14251 RVA: 0x000E7661 File Offset: 0x000E5861
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			Action<MissionObjective, float> onTickCallback = this.OnTickCallback;
			if (onTickCallback == null)
			{
				return;
			}
			onTickCallback(this, dt);
		}

		// Token: 0x0400180D RID: 6157
		internal string IUniqueId;

		// Token: 0x0400180E RID: 6158
		internal TextObject IName;

		// Token: 0x0400180F RID: 6159
		internal TextObject IDescription;

		// Token: 0x04001810 RID: 6160
		internal Func<MissionObjective, bool> IsActivationRequirementsMetCallback;

		// Token: 0x04001811 RID: 6161
		internal Func<MissionObjective, bool> IsCompletionRequirementsMetCallback;

		// Token: 0x04001812 RID: 6162
		internal Action<MissionObjective> OnStartCallback;

		// Token: 0x04001813 RID: 6163
		internal Action<MissionObjective> OnCompleteCallback;

		// Token: 0x04001814 RID: 6164
		internal Action<MissionObjective, float> OnTickCallback;

		// Token: 0x04001815 RID: 6165
		internal Func<MissionObjective, MissionObjectiveProgressInfo> GetProgressCallback;

		// Token: 0x04001816 RID: 6166
		private List<MissionObjectiveTarget> _targets;
	}
}
