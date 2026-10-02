using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000D1 RID: 209
	public class PledgeAllegianceSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x060014DB RID: 5339 RVA: 0x00061E61 File Offset: 0x00060061
		public Hero PlayerHero { get; }

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x060014DC RID: 5340 RVA: 0x00061E69 File Offset: 0x00060069
		public bool PlayerWantsToRestore { get; }

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x060014DD RID: 5341 RVA: 0x00061E71 File Offset: 0x00060071
		public override string SceneID
		{
			get
			{
				return "scn_pledge_allegiance_notification";
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x060014DE RID: 5342 RVA: 0x00061E78 File Offset: 0x00060078
		public override TextObject TitleText
		{
			get
			{
				TextObject textObject = GameTexts.FindText("str_pledge_notification_title", null);
				textObject.SetCharacterProperties("RULER", this.PlayerHero.Clan.Kingdom.Leader.CharacterObject, false);
				textObject.SetTextVariable("PLAYER_WANTS_RESTORE", this.PlayerWantsToRestore ? 1 : 0);
				textObject.SetTextVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				textObject.SetTextVariable("YEAR", this._creationCampaignTime.GetYear);
				return textObject;
			}
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x00061F00 File Offset: 0x00060100
		public override Banner[] GetBanners()
		{
			Banner[] array = new Banner[2];
			array[0] = Hero.MainHero.ClanBanner;
			int num = 1;
			Clan clan = this.PlayerHero.Clan.Kingdom.Leader.Clan;
			array[num] = ((clan != null) ? clan.Kingdom.Banner : null) ?? this.PlayerHero.Clan.Kingdom.Leader.ClanBanner;
			return array;
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x00061F6C File Offset: 0x0006016C
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			ItemObject itemObject = null;
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment equipment = this.PlayerHero.BattleEquipment.Clone(false);
			if (equipment[EquipmentIndex.ArmorItemEndSlot].IsEmpty)
			{
				itemObject = CampaignSceneNotificationHelper.GetDefaultHorseItem();
				equipment[EquipmentIndex.ArmorItemEndSlot] = new EquipmentElement(itemObject, null, null, false);
			}
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, true, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.PlayerHero, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, true));
			Equipment equipment2 = this.PlayerHero.Clan.Kingdom.Leader.BattleEquipment.Clone(false);
			if (equipment2[EquipmentIndex.ArmorItemEndSlot].IsEmpty)
			{
				if (itemObject == null)
				{
					itemObject = CampaignSceneNotificationHelper.GetDefaultHorseItem();
				}
				equipment2[EquipmentIndex.ArmorItemEndSlot] = new EquipmentElement(itemObject, null, null, false);
			}
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment2, true, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.PlayerHero.Clan.Kingdom.Leader, equipment2, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, true));
			IFaction mapFaction = this.PlayerHero.Clan.Kingdom.Leader.MapFaction;
			CultureObject cultureObject = ((((mapFaction != null) ? mapFaction.Culture : null) != null) ? this.PlayerHero.Clan.Kingdom.Leader.MapFaction.Culture : this.PlayerHero.MapFaction.Culture);
			for (int i = 0; i < 24; i++)
			{
				CharacterObject randomTroopForCulture = CampaignSceneNotificationHelper.GetRandomTroopForCulture(cultureObject);
				Equipment equipment3 = randomTroopForCulture.FirstBattleEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment3, false, false);
				BodyProperties bodyProperties = randomTroopForCulture.GetBodyProperties(equipment3, MBRandom.RandomInt(100));
				list.Add(new SceneNotificationData.SceneNotificationCharacter(randomTroopForCulture, equipment3, bodyProperties, false, uint.MaxValue, uint.MaxValue, false));
			}
			return list.ToArray();
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x00062126 File Offset: 0x00060326
		public PledgeAllegianceSceneNotificationItem(Hero playerHero, bool playerWantsToRestore)
		{
			this.PlayerHero = playerHero;
			this.PlayerWantsToRestore = playerWantsToRestore;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x040006EB RID: 1771
		private const int NumberOfTroops = 24;

		// Token: 0x040006EE RID: 1774
		private readonly CampaignTime _creationCampaignTime;
	}
}
