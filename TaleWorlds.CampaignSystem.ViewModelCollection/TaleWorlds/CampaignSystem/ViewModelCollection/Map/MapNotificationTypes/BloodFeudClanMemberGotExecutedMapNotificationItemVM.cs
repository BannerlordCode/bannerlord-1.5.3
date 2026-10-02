using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000043 RID: 67
	public class BloodFeudClanMemberGotExecutedMapNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x060005FB RID: 1531 RVA: 0x0001F91A File Offset: 0x0001DB1A
		public BloodFeudClanMemberGotExecutedMapNotificationItemVM(BloodFeudClanMemberGotExecutedMapNotification data)
			: base(data)
		{
			base.NotificationIdentifier = "blood_feud_clan_member_executed";
			this._onInspect = new Action(this.OnInspect);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0001F940 File Offset: 0x0001DB40
		private void OnInspect()
		{
			BloodFeudClanMemberGotExecutedMapNotification bloodFeudClanMemberGotExecutedMapNotification = (BloodFeudClanMemberGotExecutedMapNotification)base.Data;
			MBInformationManager.ShowSceneNotification(HeroExecutionSceneNotificationData.CreateForInformingPlayer(bloodFeudClanMemberGotExecutedMapNotification.ExecutingHero, bloodFeudClanMemberGotExecutedMapNotification.ExecutedHero, bloodFeudClanMemberGotExecutedMapNotification.ExecutionDate, SceneNotificationData.RelevantContextType.Map, null, true, true, true, false, null));
			base.ExecuteRemove();
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0001F982 File Offset: 0x0001DB82
		public override void OnFinalize()
		{
			base.OnFinalize();
		}
	}
}
