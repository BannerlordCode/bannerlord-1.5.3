using System;
using TaleWorlds.CampaignSystem.ViewModelCollection.Quests;

namespace SandBox.ViewModelCollection.Missions.NameMarker
{
	// Token: 0x02000032 RID: 50
	public abstract class MissionNameMarkerTargetVM<T> : MissionNameMarkerTargetBaseVM
	{
		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060003E2 RID: 994 RVA: 0x00010ADF File Offset: 0x0000ECDF
		// (set) Token: 0x060003E3 RID: 995 RVA: 0x00010AE7 File Offset: 0x0000ECE7
		public T Target { get; private set; }

		// Token: 0x060003E4 RID: 996 RVA: 0x00010AF0 File Offset: 0x0000ECF0
		protected MissionNameMarkerTargetVM(T target)
		{
			this.Target = target;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00010B00 File Offset: 0x0000ED00
		public override bool Equals(MissionNameMarkerTargetBaseVM other)
		{
			MissionNameMarkerTargetVM<T> missionNameMarkerTargetVM;
			if ((missionNameMarkerTargetVM = other as MissionNameMarkerTargetVM<T>) != null)
			{
				T target = missionNameMarkerTargetVM.Target;
				if (target.Equals(this.Target) && this.AreQuestsEqual(missionNameMarkerTargetVM))
				{
					return base.IsPersistent == missionNameMarkerTargetVM.IsPersistent;
				}
			}
			return false;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00010B54 File Offset: 0x0000ED54
		private bool AreQuestsEqual(MissionNameMarkerTargetVM<T> tOther)
		{
			if (tOther.Quests == null || base.Quests == null)
			{
				return tOther.Quests == null && base.Quests == null;
			}
			if (tOther.Quests.Count != base.Quests.Count)
			{
				return false;
			}
			for (int i = 0; i < base.Quests.Count; i++)
			{
				QuestMarkerVM questMarkerVM = base.Quests[i];
				QuestMarkerVM questMarkerVM2 = tOther.Quests[i];
				if (questMarkerVM.IssueQuestFlag != questMarkerVM2.IssueQuestFlag || questMarkerVM.QuestMarkerType != questMarkerVM2.QuestMarkerType)
				{
					return false;
				}
			}
			return true;
		}
	}
}
