using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x0200012F RID: 303
	public abstract class ClanPartyItemVM : ViewModel
	{
		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x06001BBB RID: 7099
		// (set) Token: 0x06001BBC RID: 7100
		public abstract int Expense { get; protected set; }

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x06001BBD RID: 7101
		// (set) Token: 0x06001BBE RID: 7102
		public abstract int Income { get; protected set; }

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x06001BBF RID: 7103
		public abstract Hero Leader { get; }

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06001BC0 RID: 7104
		public abstract CampaignVec2 Position { get; }

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x06001BC1 RID: 7105 RVA: 0x00067262 File Offset: 0x00065462
		// (set) Token: 0x06001BC2 RID: 7106 RVA: 0x0006726A File Offset: 0x0006546A
		public PartyBase Party { get; protected set; }

		// Token: 0x06001BC3 RID: 7107 RVA: 0x00067273 File Offset: 0x00065473
		public ClanPartyItemVM()
		{
			this.AreNavalControlsVisible = ModuleHelper.IsModuleActive("NavalDLC");
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x0006728C File Offset: 0x0006548C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.MayJoinOtherArmiesText = new TextObject("{=obmd0SWw}Allow joining other armies", null).ToString();
			this.AllowRaidingText = new TextObject("{=Kv7bQSkn}Allow raiding villages", null).ToString();
			this.DonateTroopsToGarrisonsText = new TextObject("{=bdqzhsnR}Allow donating troops to garrisons", null).ToString();
			this.HasFleetText = new TextObject("{=V4F2jNj7}Allow naval fleet", null).ToString();
			this.UpdateProperties();
		}

		// Token: 0x06001BC5 RID: 7109
		public abstract void UpdateProperties();

		// Token: 0x06001BC6 RID: 7110
		public abstract void OnPartySelection();

		// Token: 0x06001BC7 RID: 7111
		public abstract void ExecuteChangeLeader();

		// Token: 0x06001BC8 RID: 7112 RVA: 0x00067300 File Offset: 0x00065500
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.HeroMembers.ApplyActionOnAllItems(delegate(ClanPartyMemberItemVM h)
			{
				h.OnFinalize();
			});
			this.Roles.ApplyActionOnAllItems(delegate(ClanRoleItemVM x)
			{
				x.OnFinalize();
			});
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x00067368 File Offset: 0x00065568
		protected static CharacterCode GetCharacterCode(CharacterObject character)
		{
			if (character.IsHero)
			{
				return CampaignUIHelper.GetCharacterCode(character, false);
			}
			uint color = Hero.MainHero.MapFaction.Color;
			uint color2 = Hero.MainHero.MapFaction.Color2;
			Equipment equipment = character.Equipment;
			string text = ((equipment != null) ? equipment.CalculateEquipmentCode() : null);
			BodyProperties bodyProperties = character.GetBodyProperties(character.Equipment, -1);
			return CharacterCode.CreateFrom(text, bodyProperties, character.IsFemale, character.IsHero, color, color2, character.DefaultFormationClass, character.Race);
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x000673E8 File Offset: 0x000655E8
		private List<TooltipProperty> GetPartyTroopInfo(PartyBase party, FormationClass formationClass)
		{
			List<TooltipProperty> list = new List<TooltipProperty>();
			list.Add(new TooltipProperty("", GameTexts.FindText("str_formation_class_string", formationClass.GetName()).ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.Title));
			foreach (TroopRosterElement troopRosterElement in this.Party.MemberRoster.GetTroopRoster())
			{
				if (!troopRosterElement.Character.IsHero && troopRosterElement.Character.DefaultFormationClass.Equals(formationClass))
				{
					list.Add(new TooltipProperty(troopRosterElement.Character.Name.ToString(), troopRosterElement.Number.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
				}
			}
			return list;
		}

		// Token: 0x06001BCB RID: 7115 RVA: 0x000674D0 File Offset: 0x000656D0
		private void OnMayJoinOtherArmiesChanged(bool value)
		{
			if (this.Leader != null)
			{
				this.Leader.CanJoinArmy = value;
			}
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x000674E6 File Offset: 0x000656E6
		private void OnAllowRaidingChanged(bool value)
		{
			if (this.Leader != null)
			{
				this.Leader.CanRaid = value;
			}
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x000674FC File Offset: 0x000656FC
		private void OnDonateTroopsToGarrisonsChanged(bool value)
		{
			if (this.Leader != null)
			{
				this.Leader.CanDonateTroopsToGarrison = value;
			}
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x00067512 File Offset: 0x00065712
		private void OnHasFleetChanged(bool value)
		{
			if (this.Leader != null)
			{
				this.Leader.CanHaveFleet = value;
			}
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x06001BCF RID: 7119
		// (set) Token: 0x06001BD0 RID: 7120
		[DataSourceProperty]
		public abstract CharacterViewModel CharacterModel { get; set; }

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x06001BD1 RID: 7121
		// (set) Token: 0x06001BD2 RID: 7122
		[DataSourceProperty]
		public abstract CharacterImageIdentifierVM LeaderVisual { get; set; }

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x06001BD3 RID: 7123
		// (set) Token: 0x06001BD4 RID: 7124
		[DataSourceProperty]
		public abstract bool IsPendingPartyCreation { get; set; }

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x06001BD5 RID: 7125
		// (set) Token: 0x06001BD6 RID: 7126
		[DataSourceProperty]
		public abstract bool IsSelected { get; set; }

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06001BD7 RID: 7127
		// (set) Token: 0x06001BD8 RID: 7128
		[DataSourceProperty]
		public abstract bool HasHeroMembers { get; set; }

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06001BD9 RID: 7129
		// (set) Token: 0x06001BDA RID: 7130
		[DataSourceProperty]
		public abstract bool IsClanRoleSelectionHighlightEnabled { get; set; }

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06001BDB RID: 7131
		// (set) Token: 0x06001BDC RID: 7132
		[DataSourceProperty]
		public abstract bool IsRoleSelectionPopupVisible { get; set; }

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06001BDD RID: 7133
		// (set) Token: 0x06001BDE RID: 7134
		[DataSourceProperty]
		public abstract bool IsDisbanding { get; set; }

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06001BDF RID: 7135
		// (set) Token: 0x06001BE0 RID: 7136
		[DataSourceProperty]
		public abstract bool IsInArmy { get; set; }

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x06001BE1 RID: 7137
		// (set) Token: 0x06001BE2 RID: 7138
		[DataSourceProperty]
		public abstract bool CanUseActions { get; set; }

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x06001BE3 RID: 7139
		// (set) Token: 0x06001BE4 RID: 7140
		[DataSourceProperty]
		public abstract bool IsChangeLeaderVisible { get; set; }

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x06001BE5 RID: 7141
		// (set) Token: 0x06001BE6 RID: 7142
		[DataSourceProperty]
		public abstract bool IsChangeLeaderEnabled { get; set; }

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x06001BE7 RID: 7143
		// (set) Token: 0x06001BE8 RID: 7144
		[DataSourceProperty]
		public abstract HintViewModel ActionsDisabledHint { get; set; }

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x06001BE9 RID: 7145
		// (set) Token: 0x06001BEA RID: 7146
		[DataSourceProperty]
		public abstract bool IsCaravan { get; set; }

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x06001BEB RID: 7147
		// (set) Token: 0x06001BEC RID: 7148
		[DataSourceProperty]
		public abstract bool ShouldPartyHaveExpense { get; set; }

		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x06001BED RID: 7149
		// (set) Token: 0x06001BEE RID: 7150
		[DataSourceProperty]
		public abstract bool HasCompanion { get; set; }

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x06001BEF RID: 7151
		// (set) Token: 0x06001BF0 RID: 7152
		[DataSourceProperty]
		public abstract bool IsAutoRecruitmentVisible { get; set; }

		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x06001BF1 RID: 7153
		// (set) Token: 0x06001BF2 RID: 7154
		[DataSourceProperty]
		public abstract bool AutoRecruitmentValue { get; set; }

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x06001BF3 RID: 7155
		// (set) Token: 0x06001BF4 RID: 7156
		[DataSourceProperty]
		public abstract bool IsMembersAndRolesVisible { get; set; }

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x06001BF5 RID: 7157
		[DataSourceProperty]
		public abstract bool IsLeaderTeleporting { get; }

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x06001BF6 RID: 7158
		// (set) Token: 0x06001BF7 RID: 7159
		[DataSourceProperty]
		public abstract bool IsMainHeroParty { get; set; }

		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x06001BF8 RID: 7160
		// (set) Token: 0x06001BF9 RID: 7161
		[DataSourceProperty]
		public abstract ClanFinanceExpenseItemVM ExpenseItem { get; set; }

		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x06001BFA RID: 7162
		// (set) Token: 0x06001BFB RID: 7163
		[DataSourceProperty]
		public abstract ClanPartyMemberItemVM LeaderMember { get; set; }

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06001BFC RID: 7164
		// (set) Token: 0x06001BFD RID: 7165
		[DataSourceProperty]
		public abstract string PartySizeText { get; set; }

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x06001BFE RID: 7166
		// (set) Token: 0x06001BFF RID: 7167
		[DataSourceProperty]
		public abstract string ShipCountText { get; set; }

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x06001C00 RID: 7168
		// (set) Token: 0x06001C01 RID: 7169
		[DataSourceProperty]
		public abstract string MembersText { get; set; }

		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x06001C02 RID: 7170
		// (set) Token: 0x06001C03 RID: 7171
		[DataSourceProperty]
		public abstract string AssigneesText { get; set; }

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x06001C04 RID: 7172
		// (set) Token: 0x06001C05 RID: 7173
		[DataSourceProperty]
		public abstract string RolesText { get; set; }

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x06001C06 RID: 7174
		// (set) Token: 0x06001C07 RID: 7175
		[DataSourceProperty]
		public abstract string PartyLeaderRoleEffectsText { get; set; }

		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x06001C08 RID: 7176
		// (set) Token: 0x06001C09 RID: 7177
		[DataSourceProperty]
		public abstract string PartyLocationText { get; set; }

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x06001C0A RID: 7178
		// (set) Token: 0x06001C0B RID: 7179
		[DataSourceProperty]
		public abstract string Name { get; set; }

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x06001C0C RID: 7180
		// (set) Token: 0x06001C0D RID: 7181
		[DataSourceProperty]
		public abstract string PartySizeSubTitleText { get; set; }

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x06001C0E RID: 7182
		// (set) Token: 0x06001C0F RID: 7183
		[DataSourceProperty]
		public abstract string PartyWageSubTitleText { get; set; }

		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x06001C10 RID: 7184
		// (set) Token: 0x06001C11 RID: 7185
		[DataSourceProperty]
		public abstract int InfantryCount { get; set; }

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x06001C12 RID: 7186
		// (set) Token: 0x06001C13 RID: 7187
		[DataSourceProperty]
		public abstract int RangedCount { get; set; }

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x06001C14 RID: 7188
		// (set) Token: 0x06001C15 RID: 7189
		[DataSourceProperty]
		public abstract int CavalryCount { get; set; }

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x06001C16 RID: 7190
		// (set) Token: 0x06001C17 RID: 7191
		[DataSourceProperty]
		public abstract int HorseArcherCount { get; set; }

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x06001C18 RID: 7192
		// (set) Token: 0x06001C19 RID: 7193
		[DataSourceProperty]
		public abstract int ShipCount { get; set; }

		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x06001C1A RID: 7194
		// (set) Token: 0x06001C1B RID: 7195
		[DataSourceProperty]
		public abstract string InArmyText { get; set; }

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x06001C1C RID: 7196
		// (set) Token: 0x06001C1D RID: 7197
		[DataSourceProperty]
		public abstract string DisbandingText { get; set; }

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x06001C1E RID: 7198
		// (set) Token: 0x06001C1F RID: 7199
		[DataSourceProperty]
		public abstract string AutoRecruitmentText { get; set; }

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x06001C20 RID: 7200
		// (set) Token: 0x06001C21 RID: 7201
		[DataSourceProperty]
		public abstract HintViewModel AutoRecruitmentHint { get; set; }

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x06001C22 RID: 7202
		// (set) Token: 0x06001C23 RID: 7203
		[DataSourceProperty]
		public abstract HintViewModel LeaderIsMovingToPartyHint { get; set; }

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x06001C24 RID: 7204
		// (set) Token: 0x06001C25 RID: 7205
		[DataSourceProperty]
		public abstract HintViewModel InArmyHint { get; set; }

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x06001C26 RID: 7206
		// (set) Token: 0x06001C27 RID: 7207
		[DataSourceProperty]
		public abstract HintViewModel ChangeLeaderHint { get; set; }

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x06001C28 RID: 7208
		// (set) Token: 0x06001C29 RID: 7209
		[DataSourceProperty]
		public abstract BasicTooltipViewModel InfantryHint { get; set; }

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x06001C2A RID: 7210
		// (set) Token: 0x06001C2B RID: 7211
		[DataSourceProperty]
		public abstract BasicTooltipViewModel RangedHint { get; set; }

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x06001C2C RID: 7212
		// (set) Token: 0x06001C2D RID: 7213
		[DataSourceProperty]
		public abstract BasicTooltipViewModel CavalryHint { get; set; }

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x06001C2E RID: 7214
		// (set) Token: 0x06001C2F RID: 7215
		[DataSourceProperty]
		public abstract BasicTooltipViewModel HorseArcherHint { get; set; }

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x06001C30 RID: 7216
		// (set) Token: 0x06001C31 RID: 7217
		[DataSourceProperty]
		public abstract MBBindingList<ClanPartyMemberItemVM> HeroMembers { get; set; }

		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x06001C32 RID: 7218
		// (set) Token: 0x06001C33 RID: 7219
		[DataSourceProperty]
		public abstract MBBindingList<ClanRoleItemVM> Roles { get; set; }

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x06001C34 RID: 7220 RVA: 0x00067528 File Offset: 0x00065728
		// (set) Token: 0x06001C35 RID: 7221 RVA: 0x00067530 File Offset: 0x00065730
		[DataSourceProperty]
		public bool AreCommandControlsVisible
		{
			get
			{
				return this._areCommandControlsVisible;
			}
			set
			{
				if (value != this._areCommandControlsVisible)
				{
					this._areCommandControlsVisible = value;
					base.OnPropertyChangedWithValue(value, "AreCommandControlsVisible");
				}
			}
		}

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06001C36 RID: 7222 RVA: 0x0006754E File Offset: 0x0006574E
		// (set) Token: 0x06001C37 RID: 7223 RVA: 0x00067556 File Offset: 0x00065756
		[DataSourceProperty]
		public bool AreNavalControlsVisible
		{
			get
			{
				return this._areNavalControlsVisible;
			}
			set
			{
				if (value != this._areNavalControlsVisible)
				{
					this._areNavalControlsVisible = value;
					base.OnPropertyChangedWithValue(value, "AreNavalControlsVisible");
				}
			}
		}

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x06001C38 RID: 7224 RVA: 0x00067574 File Offset: 0x00065774
		// (set) Token: 0x06001C39 RID: 7225 RVA: 0x0006757C File Offset: 0x0006577C
		[DataSourceProperty]
		public bool MayJoinOtherArmies
		{
			get
			{
				return this._mayJoinOtherArmies;
			}
			set
			{
				if (value != this._mayJoinOtherArmies)
				{
					this._mayJoinOtherArmies = value;
					base.OnPropertyChangedWithValue(value, "MayJoinOtherArmies");
					this.OnMayJoinOtherArmiesChanged(value);
				}
			}
		}

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x06001C3A RID: 7226 RVA: 0x000675A1 File Offset: 0x000657A1
		// (set) Token: 0x06001C3B RID: 7227 RVA: 0x000675A9 File Offset: 0x000657A9
		[DataSourceProperty]
		public bool AllowRaiding
		{
			get
			{
				return this._allowRaiding;
			}
			set
			{
				if (value != this._allowRaiding)
				{
					this._allowRaiding = value;
					base.OnPropertyChangedWithValue(value, "AllowRaiding");
					this.OnAllowRaidingChanged(value);
				}
			}
		}

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x06001C3C RID: 7228 RVA: 0x000675CE File Offset: 0x000657CE
		// (set) Token: 0x06001C3D RID: 7229 RVA: 0x000675D6 File Offset: 0x000657D6
		[DataSourceProperty]
		public bool DonateTroopsToGarrisons
		{
			get
			{
				return this._donateTroopsToGarrisons;
			}
			set
			{
				if (value != this._donateTroopsToGarrisons)
				{
					this._donateTroopsToGarrisons = value;
					base.OnPropertyChangedWithValue(value, "DonateTroopsToGarrisons");
					this.OnDonateTroopsToGarrisonsChanged(value);
				}
			}
		}

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x06001C3E RID: 7230 RVA: 0x000675FB File Offset: 0x000657FB
		// (set) Token: 0x06001C3F RID: 7231 RVA: 0x00067603 File Offset: 0x00065803
		[DataSourceProperty]
		public bool HasFleet
		{
			get
			{
				return this._hasFleet;
			}
			set
			{
				if (value != this._hasFleet)
				{
					this._hasFleet = value;
					base.OnPropertyChangedWithValue(value, "HasFleet");
					this.OnHasFleetChanged(value);
				}
			}
		}

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x06001C40 RID: 7232 RVA: 0x00067628 File Offset: 0x00065828
		// (set) Token: 0x06001C41 RID: 7233 RVA: 0x00067630 File Offset: 0x00065830
		[DataSourceProperty]
		public string MayJoinOtherArmiesText
		{
			get
			{
				return this._mayJoinOtherArmiesText;
			}
			set
			{
				if (value != this._mayJoinOtherArmiesText)
				{
					this._mayJoinOtherArmiesText = value;
					base.OnPropertyChangedWithValue<string>(value, "MayJoinOtherArmiesText");
				}
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06001C42 RID: 7234 RVA: 0x00067653 File Offset: 0x00065853
		// (set) Token: 0x06001C43 RID: 7235 RVA: 0x0006765B File Offset: 0x0006585B
		[DataSourceProperty]
		public string AllowRaidingText
		{
			get
			{
				return this._allowRaidingText;
			}
			set
			{
				if (value != this._allowRaidingText)
				{
					this._allowRaidingText = value;
					base.OnPropertyChangedWithValue<string>(value, "AllowRaidingText");
				}
			}
		}

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x06001C44 RID: 7236 RVA: 0x0006767E File Offset: 0x0006587E
		// (set) Token: 0x06001C45 RID: 7237 RVA: 0x00067686 File Offset: 0x00065886
		[DataSourceProperty]
		public string DonateTroopsToGarrisonsText
		{
			get
			{
				return this._donateTroopsToGarrisonsText;
			}
			set
			{
				if (value != this._donateTroopsToGarrisonsText)
				{
					this._donateTroopsToGarrisonsText = value;
					base.OnPropertyChangedWithValue<string>(value, "DonateTroopsToGarrisonsText");
				}
			}
		}

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x06001C46 RID: 7238 RVA: 0x000676A9 File Offset: 0x000658A9
		// (set) Token: 0x06001C47 RID: 7239 RVA: 0x000676B1 File Offset: 0x000658B1
		[DataSourceProperty]
		public string HasFleetText
		{
			get
			{
				return this._hasFleetText;
			}
			set
			{
				if (value != this._hasFleetText)
				{
					this._hasFleetText = value;
					base.OnPropertyChangedWithValue<string>(value, "HasFleetText");
				}
			}
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x06001C48 RID: 7240 RVA: 0x000676D4 File Offset: 0x000658D4
		// (set) Token: 0x06001C49 RID: 7241 RVA: 0x000676DC File Offset: 0x000658DC
		[DataSourceProperty]
		public int SmallShipCount
		{
			get
			{
				return this._smallShipCount;
			}
			set
			{
				if (value != this._smallShipCount)
				{
					this._smallShipCount = value;
					base.OnPropertyChangedWithValue(value, "SmallShipCount");
				}
			}
		}

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x06001C4A RID: 7242 RVA: 0x000676FA File Offset: 0x000658FA
		// (set) Token: 0x06001C4B RID: 7243 RVA: 0x00067702 File Offset: 0x00065902
		[DataSourceProperty]
		public int MediumShipCount
		{
			get
			{
				return this._mediumShipCount;
			}
			set
			{
				if (value != this._mediumShipCount)
				{
					this._mediumShipCount = value;
					base.OnPropertyChangedWithValue(value, "MediumShipCount");
				}
			}
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x06001C4C RID: 7244 RVA: 0x00067720 File Offset: 0x00065920
		// (set) Token: 0x06001C4D RID: 7245 RVA: 0x00067728 File Offset: 0x00065928
		[DataSourceProperty]
		public int LargeShipCount
		{
			get
			{
				return this._largeShipCount;
			}
			set
			{
				if (value != this._largeShipCount)
				{
					this._largeShipCount = value;
					base.OnPropertyChangedWithValue(value, "LargeShipCount");
				}
			}
		}

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x06001C4E RID: 7246 RVA: 0x00067746 File Offset: 0x00065946
		// (set) Token: 0x06001C4F RID: 7247 RVA: 0x0006774E File Offset: 0x0006594E
		[DataSourceProperty]
		public HintViewModel SmallShipHint
		{
			get
			{
				return this._smallShipHint;
			}
			set
			{
				if (value != this._smallShipHint)
				{
					this._smallShipHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "SmallShipHint");
				}
			}
		}

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x06001C50 RID: 7248 RVA: 0x0006776C File Offset: 0x0006596C
		// (set) Token: 0x06001C51 RID: 7249 RVA: 0x00067774 File Offset: 0x00065974
		[DataSourceProperty]
		public HintViewModel MediumShipHint
		{
			get
			{
				return this._mediumShipHint;
			}
			set
			{
				if (value != this._mediumShipHint)
				{
					this._mediumShipHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "MediumShipHint");
				}
			}
		}

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x06001C52 RID: 7250 RVA: 0x00067792 File Offset: 0x00065992
		// (set) Token: 0x06001C53 RID: 7251 RVA: 0x0006779A File Offset: 0x0006599A
		[DataSourceProperty]
		public HintViewModel LargeShipHint
		{
			get
			{
				return this._largeShipHint;
			}
			set
			{
				if (value != this._largeShipHint)
				{
					this._largeShipHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "LargeShipHint");
				}
			}
		}

		// Token: 0x04000CE4 RID: 3300
		private bool _areCommandControlsVisible;

		// Token: 0x04000CE5 RID: 3301
		private bool _areNavalControlsVisible;

		// Token: 0x04000CE6 RID: 3302
		private bool _mayJoinOtherArmies;

		// Token: 0x04000CE7 RID: 3303
		private bool _allowRaiding;

		// Token: 0x04000CE8 RID: 3304
		private bool _donateTroopsToGarrisons;

		// Token: 0x04000CE9 RID: 3305
		private bool _hasFleet;

		// Token: 0x04000CEA RID: 3306
		private string _mayJoinOtherArmiesText;

		// Token: 0x04000CEB RID: 3307
		private string _allowRaidingText;

		// Token: 0x04000CEC RID: 3308
		private string _donateTroopsToGarrisonsText;

		// Token: 0x04000CED RID: 3309
		private string _hasFleetText;

		// Token: 0x04000CEE RID: 3310
		private int _smallShipCount;

		// Token: 0x04000CEF RID: 3311
		private int _mediumShipCount;

		// Token: 0x04000CF0 RID: 3312
		private int _largeShipCount;

		// Token: 0x04000CF1 RID: 3313
		private HintViewModel _smallShipHint;

		// Token: 0x04000CF2 RID: 3314
		private HintViewModel _mediumShipHint;

		// Token: 0x04000CF3 RID: 3315
		private HintViewModel _largeShipHint;

		// Token: 0x0200028F RID: 655
		public enum ClanPartyType
		{
			// Token: 0x04001336 RID: 4918
			Main,
			// Token: 0x04001337 RID: 4919
			Member,
			// Token: 0x04001338 RID: 4920
			Caravan,
			// Token: 0x04001339 RID: 4921
			Garrison
		}
	}
}
