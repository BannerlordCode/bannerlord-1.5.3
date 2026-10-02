using System;
using TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Interaction;
using TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Interaction.InteractionItems;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x0200006B RID: 107
	public class MissionAgentStatusUIHandler : MissionBattleUIBaseView, IInteractionInterfaceHandler
	{
		// Token: 0x06000431 RID: 1073 RVA: 0x0001F5A1 File Offset: 0x0001D7A1
		public virtual void AddInteractionMessage(MissionInteractionItemBaseVM message)
		{
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0001F5A3 File Offset: 0x0001D7A3
		public virtual void RemoveInteractionMessage(MissionInteractionItemBaseVM message)
		{
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0001F5A5 File Offset: 0x0001D7A5
		public virtual bool HasInteractionMessage(MissionInteractionItemBaseVM message)
		{
			return false;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0001F5A8 File Offset: 0x0001D7A8
		protected override void OnCreateView()
		{
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0001F5AA File Offset: 0x0001D7AA
		protected override void OnDestroyView()
		{
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0001F5AC File Offset: 0x0001D7AC
		protected override void OnSuspendView()
		{
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0001F5AE File Offset: 0x0001D7AE
		protected override void OnResumeView()
		{
		}
	}
}
