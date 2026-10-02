using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000BD RID: 189
	public class EmpireConspiracyBeginsSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06001447 RID: 5191 RVA: 0x0005F715 File Offset: 0x0005D915
		public Hero PlayerHero { get; }

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06001448 RID: 5192 RVA: 0x0005F71D File Offset: 0x0005D91D
		public Kingdom Empire { get; }

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06001449 RID: 5193 RVA: 0x0005F725 File Offset: 0x0005D925
		public bool IsConspiracyAgainstEmpire { get; }

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x0600144A RID: 5194 RVA: 0x0005F72D File Offset: 0x0005D92D
		public override string SceneID
		{
			get
			{
				return "scn_empire_conspiracy_start_notification";
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x0600144B RID: 5195 RVA: 0x0005F734 File Offset: 0x0005D934
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				if (this.IsConspiracyAgainstEmpire)
				{
					return GameTexts.FindText("str_empire_conspiracy_begins_antiempire", null);
				}
				return GameTexts.FindText("str_empire_conspiracy_begins_proempire", null);
			}
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x0005F78D File Offset: 0x0005D98D
		public override Banner[] GetBanners()
		{
			return new Banner[] { this.Empire.Banner };
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x0005F7A4 File Offset: 0x0005D9A4
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			for (int i = 0; i < 8; i++)
			{
				Equipment equipment = MBObjectManager.Instance.GetObject<MBEquipmentRoster>("conspirator_cutscene_template").DefaultEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, false, false);
				CharacterObject facePropertiesFromAudienceIndex = this.GetFacePropertiesFromAudienceIndex(false, i);
				BodyProperties bodyProperties = facePropertiesFromAudienceIndex.GetBodyProperties(equipment, MBRandom.RandomInt(100));
				uint num = this._audienceColors[MBRandom.RandomInt(this._audienceColors.Length)];
				uint num2 = this._audienceColors[MBRandom.RandomInt(this._audienceColors.Length)];
				list.Add(new SceneNotificationData.SceneNotificationCharacter(facePropertiesFromAudienceIndex, equipment, bodyProperties, false, num, num2, false));
			}
			return list.ToArray();
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x0005F84D File Offset: 0x0005DA4D
		public EmpireConspiracyBeginsSceneNotificationItem(Hero playerHero, Kingdom empire, bool isConspiracyAgainstEmpire)
		{
			this.PlayerHero = playerHero;
			this.Empire = empire;
			this.IsConspiracyAgainstEmpire = isConspiracyAgainstEmpire;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0005F88C File Offset: 0x0005DA8C
		private CharacterObject GetFacePropertiesFromAudienceIndex(bool playerWantsRestore, int audienceMemberIndex)
		{
			if (!playerWantsRestore)
			{
				return MBObjectManager.Instance.GetObject<CharacterObject>("villager_empire");
			}
			string text;
			switch (audienceMemberIndex % 8)
			{
			case 0:
				text = "villager_battania";
				break;
			case 1:
				text = "villager_khuzait";
				break;
			case 2:
				text = "villager_vlandia";
				break;
			case 3:
				text = "villager_aserai";
				break;
			case 4:
				text = "villager_battania";
				break;
			case 5:
				text = "villager_sturgia";
				break;
			default:
				text = "villager_battania";
				break;
			}
			return MBObjectManager.Instance.GetObject<CharacterObject>(text);
		}

		// Token: 0x0400069F RID: 1695
		private const int AudienceNumber = 8;

		// Token: 0x040006A0 RID: 1696
		private readonly uint[] _audienceColors = new uint[] { 4278914065U, 4284308292U, 4281543757U, 4282199842U };

		// Token: 0x040006A4 RID: 1700
		private readonly CampaignTime _creationCampaignTime;
	}
}
