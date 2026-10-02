using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000BE RID: 190
	public abstract class EmpireConspiracySupportsSceneNotificationItemBase : SceneNotificationData
	{
		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06001450 RID: 5200 RVA: 0x0005F911 File Offset: 0x0005DB11
		public Hero King { get; }

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06001451 RID: 5201 RVA: 0x0005F919 File Offset: 0x0005DB19
		public override string SceneID
		{
			get
			{
				return "scn_empire_conspiracy_supports_notification";
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06001452 RID: 5202 RVA: 0x0005F920 File Offset: 0x0005DB20
		public override TextObject AffirmativeText
		{
			get
			{
				return GameTexts.FindText("str_ok", null);
			}
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x0005F92D File Offset: 0x0005DB2D
		public override Banner[] GetBanners()
		{
			return new Banner[]
			{
				this.King.MapFaction.Banner,
				this.King.MapFaction.Banner
			};
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x0005F95C File Offset: 0x0005DB5C
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment equipment = this.King.CivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, false, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.King, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			CharacterObject @object = MBObjectManager.Instance.GetObject<CharacterObject>("villager_battania");
			Equipment equipment2 = MBObjectManager.Instance.GetObject<MBEquipmentRoster>("conspirator_cutscene_template").DefaultEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment2, false, false);
			BodyProperties bodyProperties = @object.GetBodyProperties(equipment2, MBRandom.RandomInt(100));
			list.Add(new SceneNotificationData.SceneNotificationCharacter(@object, equipment2, bodyProperties, false, 0U, 0U, false));
			bodyProperties = @object.GetBodyProperties(equipment2, MBRandom.RandomInt(100));
			list.Add(new SceneNotificationData.SceneNotificationCharacter(@object, equipment2, bodyProperties, false, 0U, 0U, false));
			bodyProperties = @object.GetBodyProperties(equipment2, MBRandom.RandomInt(100));
			list.Add(new SceneNotificationData.SceneNotificationCharacter(@object, equipment2, bodyProperties, false, 0U, 0U, false));
			list.Add(CampaignSceneNotificationHelper.GetBodyguardOfCulture(this.King.MapFaction.Culture));
			list.Add(CampaignSceneNotificationHelper.GetBodyguardOfCulture(this.King.MapFaction.Culture));
			return list.ToArray();
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x0005FA7B File Offset: 0x0005DC7B
		protected EmpireConspiracySupportsSceneNotificationItemBase(Hero kingHero)
		{
			this.King = kingHero;
		}
	}
}
