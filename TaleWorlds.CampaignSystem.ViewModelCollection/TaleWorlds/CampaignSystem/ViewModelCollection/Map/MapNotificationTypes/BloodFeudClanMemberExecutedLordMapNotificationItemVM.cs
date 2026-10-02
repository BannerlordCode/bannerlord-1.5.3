using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000042 RID: 66
	public class BloodFeudClanMemberExecutedLordMapNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x060005F8 RID: 1528 RVA: 0x0001F8A3 File Offset: 0x0001DAA3
		public BloodFeudClanMemberExecutedLordMapNotificationItemVM(BloodFeudClanMemberExecutedLordMapNotification data)
			: base(data)
		{
			base.NotificationIdentifier = "blood_feud_clan_member_executed_lord";
			base.ForceInspection = true;
			this._onInspect = new Action(this.OnInspect);
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0001F8D0 File Offset: 0x0001DAD0
		private void OnInspect()
		{
			BloodFeudClanMemberExecutedLordMapNotification bloodFeudClanMemberExecutedLordMapNotification = (BloodFeudClanMemberExecutedLordMapNotification)base.Data;
			MBInformationManager.ShowSceneNotification(HeroExecutionSceneNotificationData.CreateForInformingPlayer(bloodFeudClanMemberExecutedLordMapNotification.Executor, bloodFeudClanMemberExecutedLordMapNotification.ExecutedHero, bloodFeudClanMemberExecutedLordMapNotification.ExecutionDate, SceneNotificationData.RelevantContextType.Map, null, true, false, true, false, null));
			base.ExecuteRemove();
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0001F912 File Offset: 0x0001DB12
		public override void OnFinalize()
		{
			base.OnFinalize();
		}
	}
}
