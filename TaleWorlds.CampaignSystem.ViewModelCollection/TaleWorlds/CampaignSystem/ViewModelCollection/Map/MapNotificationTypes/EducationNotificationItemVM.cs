using System;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000048 RID: 72
	public class EducationNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x06000608 RID: 1544 RVA: 0x0001FB5C File Offset: 0x0001DD5C
		public EducationNotificationItemVM(EducationMapNotification data)
			: base(data)
		{
			base.NotificationIdentifier = "education";
			base.ForceInspection = true;
			this._child = data.Child;
			this._age = data.Age;
			this._onInspect = new Action(this.OnInspect);
			CampaignEvents.ChildEducationCompletedEvent.AddNonSerializedListener(this, new Action<Hero, int>(this.OnEducationCompletedForChild));
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0001FBC4 File Offset: 0x0001DDC4
		private void OnInspect()
		{
			EducationMapNotification educationMapNotification = (EducationMapNotification)base.Data;
			if (educationMapNotification != null && !educationMapNotification.IsValid())
			{
				InformationManager.ShowInquiry(new InquiryData("", new TextObject("{=wGWYNYYX}This education stage is no longer relevant.", null).ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), "", null, null, "", 0f, null, null, null), false, false);
				base.ExecuteRemove();
				return;
			}
			Game.Current.GameStateManager.PushState(Game.Current.GameStateManager.CreateState<EducationState>(new object[] { this._child }), 0);
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x0001FC64 File Offset: 0x0001DE64
		private void OnEducationCompletedForChild(Hero child, int age)
		{
			if (child == this._child && age >= this._age)
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0001FC7E File Offset: 0x0001DE7E
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEvents.ChildEducationCompletedEvent.ClearListeners(this);
		}

		// Token: 0x04000286 RID: 646
		private readonly Hero _child;

		// Token: 0x04000287 RID: 647
		private readonly int _age;
	}
}
