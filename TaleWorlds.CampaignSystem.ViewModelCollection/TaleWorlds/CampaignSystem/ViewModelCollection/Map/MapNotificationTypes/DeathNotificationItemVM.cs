using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000047 RID: 71
	public class DeathNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x06000607 RID: 1543 RVA: 0x0001FA1C File Offset: 0x0001DC1C
		public DeathNotificationItemVM(DeathMapNotification data)
		{
			DeathNotificationItemVM.<>c__DisplayClass0_0 CS$<>8__locals1 = new DeathNotificationItemVM.<>c__DisplayClass0_0();
			CS$<>8__locals1.data = data;
			base..ctor(CS$<>8__locals1.data);
			CS$<>8__locals1.<>4__this = this;
			base.NotificationIdentifier = "death";
			bool victimDiedAtSea = (CS$<>8__locals1.data.VictimHero.PartyBelongedTo != null && CS$<>8__locals1.data.VictimHero.PartyBelongedTo.IsCurrentlyAtSea) || (CS$<>8__locals1.data.VictimHero.PartyBelongedToAsPrisoner != null && CS$<>8__locals1.data.VictimHero.PartyBelongedToAsPrisoner.IsMobile && CS$<>8__locals1.data.VictimHero.PartyBelongedToAsPrisoner.MobileParty.IsCurrentlyAtSea);
			if (CS$<>8__locals1.data.VictimHero == Hero.MainHero)
			{
				this._onInspect = delegate
				{
					INavigationHandler navigationHandler = CS$<>8__locals1.<>4__this.NavigationHandler;
					if (navigationHandler != null)
					{
						navigationHandler.OpenCharacterDeveloper(Hero.MainHero);
					}
					CS$<>8__locals1.<>4__this.ExecuteRemove();
				};
				return;
			}
			if (CS$<>8__locals1.data.KillDetail == KillCharacterAction.KillCharacterActionDetail.DiedInBattle)
			{
				this._onInspect = delegate
				{
					SceneNotificationData sceneNotificationData;
					if (victimDiedAtSea)
					{
						sceneNotificationData = new NavalDeathSceneNotificationItem(CS$<>8__locals1.data.VictimHero, CS$<>8__locals1.data.CreationTime, CS$<>8__locals1.data.KillDetail);
					}
					else
					{
						sceneNotificationData = new ClanMemberWarDeathSceneNotificationItem(CS$<>8__locals1.data.VictimHero, CS$<>8__locals1.data.CreationTime);
					}
					MBInformationManager.ShowSceneNotification(sceneNotificationData);
					CS$<>8__locals1.<>4__this.ExecuteRemove();
				};
				return;
			}
			this._onInspect = delegate
			{
				SceneNotificationData sceneNotificationData2;
				if (victimDiedAtSea)
				{
					sceneNotificationData2 = new NavalDeathSceneNotificationItem(CS$<>8__locals1.data.VictimHero, CS$<>8__locals1.data.CreationTime, CS$<>8__locals1.data.KillDetail);
				}
				else
				{
					sceneNotificationData2 = new ClanMemberPeaceDeathSceneNotificationItem(CS$<>8__locals1.data.VictimHero, CS$<>8__locals1.data.CreationTime, CS$<>8__locals1.data.KillDetail);
				}
				MBInformationManager.ShowSceneNotification(sceneNotificationData2);
				CS$<>8__locals1.<>4__this.ExecuteRemove();
			};
		}
	}
}
