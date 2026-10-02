using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x02000210 RID: 528
	public sealed class CharacterCreationContent
	{
		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x0600206D RID: 8301 RVA: 0x00092EDD File Offset: 0x000910DD
		// (set) Token: 0x0600206E RID: 8302 RVA: 0x00092EE5 File Offset: 0x000910E5
		public string SelectedTitleType { get; set; }

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x0600206F RID: 8303 RVA: 0x00092EEE File Offset: 0x000910EE
		// (set) Token: 0x06002070 RID: 8304 RVA: 0x00092EF6 File Offset: 0x000910F6
		public string SelectedParentOccupation { get; private set; }

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06002071 RID: 8305 RVA: 0x00092EFF File Offset: 0x000910FF
		// (set) Token: 0x06002072 RID: 8306 RVA: 0x00092F07 File Offset: 0x00091107
		public string DefaultSelectedTitleType { get; set; }

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06002073 RID: 8307 RVA: 0x00092F10 File Offset: 0x00091110
		// (set) Token: 0x06002074 RID: 8308 RVA: 0x00092F18 File Offset: 0x00091118
		public TextObject ReviewPageDescription { get; private set; }

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06002075 RID: 8309 RVA: 0x00092F21 File Offset: 0x00091121
		// (set) Token: 0x06002076 RID: 8310 RVA: 0x00092F29 File Offset: 0x00091129
		public string MainCharacterName { get; private set; }

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06002077 RID: 8311 RVA: 0x00092F32 File Offset: 0x00091132
		// (set) Token: 0x06002078 RID: 8312 RVA: 0x00092F3A File Offset: 0x0009113A
		public CultureObject SelectedCulture { get; private set; }

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06002079 RID: 8313 RVA: 0x00092F43 File Offset: 0x00091143
		// (set) Token: 0x0600207A RID: 8314 RVA: 0x00092F4B File Offset: 0x0009114B
		public Banner SelectedBanner { get; private set; }

		// Token: 0x0600207B RID: 8315 RVA: 0x00092F54 File Offset: 0x00091154
		public CharacterCreationContent()
		{
			this.SetMainHeroInitialStats();
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x00092FA1 File Offset: 0x000911A1
		public void AddCharacterCreationCulture(CultureObject culture, int focusToAddByCulture, int skillLevelToAddByCulture)
		{
			if (!this._characterCreationCultures.ContainsKey(culture))
			{
				this._characterCreationCultures.Add(culture, new KeyValuePair<int, int>(focusToAddByCulture, skillLevelToAddByCulture));
				return;
			}
			this._characterCreationCultures[culture] = new KeyValuePair<int, int>(focusToAddByCulture, skillLevelToAddByCulture);
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x00092FD8 File Offset: 0x000911D8
		public int GetFocusToAddByCulture(CultureObject culture)
		{
			return this._characterCreationCultures[culture].Key;
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x00092FFC File Offset: 0x000911FC
		public int GetSkillLevelToAddByCulture(CultureObject culture)
		{
			return this._characterCreationCultures[culture].Value;
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x0009301D File Offset: 0x0009121D
		public void ChangeReviewPageDescription(TextObject reviewPageDescription)
		{
			this.ReviewPageDescription = reviewPageDescription;
		}

		// Token: 0x06002080 RID: 8320 RVA: 0x00093026 File Offset: 0x00091226
		public void SetMainCharacterName(string name)
		{
			this.MainCharacterName = name;
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x0009302F File Offset: 0x0009122F
		public void SetParentOccupation(string occupationType)
		{
			this.SelectedParentOccupation = occupationType;
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x00093038 File Offset: 0x00091238
		public void ApplySkillAndAttributeEffects(List<SkillObject> skills, int focusToAdd, int skillLevelToAdd, CharacterAttribute attribute, int attributeLevelToAdd, List<TraitObject> traits = null, int traitLevelToAdd = 0, int renownToAdd = 0, int goldToAdd = 0, int unspentFocusPoints = 0, int unspentAttributePoints = 0)
		{
			foreach (SkillObject skillObject in skills)
			{
				Hero.MainHero.HeroDeveloper.AddFocus(skillObject, focusToAdd, false);
				if (Hero.MainHero.GetSkillValue(skillObject) == 1)
				{
					Hero.MainHero.HeroDeveloper.ChangeSkillLevel(skillObject, skillLevelToAdd - 1, false);
				}
				else
				{
					Hero.MainHero.HeroDeveloper.ChangeSkillLevel(skillObject, skillLevelToAdd, false);
				}
			}
			Hero.MainHero.HeroDeveloper.UnspentFocusPoints += unspentFocusPoints;
			Hero.MainHero.HeroDeveloper.UnspentAttributePoints += unspentAttributePoints;
			if (attribute != null)
			{
				Hero.MainHero.HeroDeveloper.AddAttribute(attribute, attributeLevelToAdd, false);
			}
			if (traits != null && traitLevelToAdd > 0 && traits.Count > 0)
			{
				foreach (TraitObject traitObject in traits)
				{
					Hero.MainHero.SetTraitLevel(traitObject, Hero.MainHero.GetTraitLevel(traitObject) + traitLevelToAdd);
				}
			}
			if (renownToAdd > 0)
			{
				GainRenownAction.Apply(Hero.MainHero, (float)renownToAdd, true);
			}
			if (goldToAdd > 0)
			{
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, goldToAdd, true);
			}
			Hero.MainHero.HeroDeveloper.ResetTotalXpForPlayerCharacter();
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x000931A8 File Offset: 0x000913A8
		public void SetMainClanBanner(Banner banner)
		{
			this.SelectedBanner = banner;
		}

		// Token: 0x06002084 RID: 8324 RVA: 0x000931B4 File Offset: 0x000913B4
		public void SetSelectedCulture(CultureObject culture, CharacterCreationManager characterCreationManager)
		{
			this.SelectedCulture = culture;
			characterCreationManager.ResetMenuOptions();
			this.SelectedTitleType = this.DefaultSelectedTitleType;
			TextObject textObject = FactionHelper.GenerateClanNameforPlayer();
			Clan.PlayerClan.ChangeClanName(textObject, textObject);
		}

		// Token: 0x06002085 RID: 8325 RVA: 0x000931EC File Offset: 0x000913EC
		public void ApplyCulture(CharacterCreationManager characterCreationManager)
		{
			Hero.MainHero.Culture = this.SelectedCulture;
			Clan.PlayerClan.Culture = this.SelectedCulture;
			Clan.PlayerClan.ResetPlayerHomeAndFactionMidSettlement();
			Hero.MainHero.BornSettlement = Clan.PlayerClan.HomeSettlement;
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x0009322C File Offset: 0x0009142C
		public IEnumerable<CultureObject> GetCultures()
		{
			foreach (KeyValuePair<CultureObject, KeyValuePair<int, int>> keyValuePair in this._characterCreationCultures)
			{
				yield return keyValuePair.Key;
			}
			Dictionary<CultureObject, KeyValuePair<int, int>>.Enumerator enumerator = default(Dictionary<CultureObject, KeyValuePair<int, int>>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06002087 RID: 8327 RVA: 0x0009323C File Offset: 0x0009143C
		private void SetMainHeroInitialStats()
		{
			Hero.MainHero.HeroDeveloper.ClearHero();
			Hero.MainHero.HitPoints = 100;
			foreach (SkillObject skillObject in Skills.All)
			{
				Hero.MainHero.HeroDeveloper.InitializeSkillXp(skillObject);
			}
			foreach (CharacterAttribute characterAttribute in Attributes.All)
			{
				Hero.MainHero.HeroDeveloper.AddAttribute(characterAttribute, 2, false);
			}
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x00093300 File Offset: 0x00091500
		public void AddEquipmentToUseGetter(CharacterCreationContent.TryGetEquipmentIdDelegate tryGetEquipmentIdDelegate)
		{
			this._tryGetEquipmentIdDelegates.Add(tryGetEquipmentIdDelegate);
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x00093310 File Offset: 0x00091510
		public bool TryGetEquipmentToUse(string occupationId, out string equipmentId)
		{
			for (int i = this._tryGetEquipmentIdDelegates.Count - 1; i >= 0; i--)
			{
				if (this._tryGetEquipmentIdDelegates[i](occupationId, out equipmentId))
				{
					return true;
				}
			}
			equipmentId = null;
			return false;
		}

		// Token: 0x0400096C RID: 2412
		public int FocusToAdd = 1;

		// Token: 0x0400096D RID: 2413
		public int SkillLevelToAdd = 10;

		// Token: 0x0400096E RID: 2414
		public int AttributeLevelToAdd = 1;

		// Token: 0x04000976 RID: 2422
		public int StartingAge = 20;

		// Token: 0x04000977 RID: 2423
		private readonly Dictionary<CultureObject, KeyValuePair<int, int>> _characterCreationCultures = new Dictionary<CultureObject, KeyValuePair<int, int>>();

		// Token: 0x04000978 RID: 2424
		private readonly List<CharacterCreationContent.TryGetEquipmentIdDelegate> _tryGetEquipmentIdDelegates = new List<CharacterCreationContent.TryGetEquipmentIdDelegate>();

		// Token: 0x02000633 RID: 1587
		// (Invoke) Token: 0x06005327 RID: 21287
		public delegate bool TryGetEquipmentIdDelegate(string occupationId, out string equipmentId);
	}
}
