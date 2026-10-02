using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.CampaignSystem.ViewModelCollection.Quests;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Overlay
{
	// Token: 0x020000C0 RID: 192
	public class GameMenuPartyItemVM : ViewModel
	{
		// Token: 0x06001278 RID: 4728 RVA: 0x0004AE0C File Offset: 0x0004900C
		public GameMenuPartyItemVM()
		{
			this.Visual = new CharacterImageIdentifierVM(null);
			this.RegisterEvents();
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x0004AE38 File Offset: 0x00049038
		public GameMenuPartyItemVM(Action<GameMenuPartyItemVM> onSetAsContextMenuActiveItem, Settlement settlement)
		{
			this._onSetAsContextMenuActiveItem = onSetAsContextMenuActiveItem;
			this.Settlement = settlement;
			SettlementComponent settlementComponent = settlement.SettlementComponent;
			this.SettlementPath = ((settlementComponent == null) ? "placeholder" : (settlementComponent.BackgroundMeshName + "_t"));
			this.Visual = new CharacterImageIdentifierVM(null);
			this.NameText = settlement.Name.ToString();
			this.PartySize = -1;
			this.PartyWoundedSize = -1;
			this.PartySizeLbl = "";
			this.IsPlayer = false;
			this.IsAlly = false;
			this.IsEnemy = false;
			this.Quests = new MBBindingList<QuestMarkerVM>();
			this.RefreshProperties();
			this.RegisterEvents();
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x0004AEF4 File Offset: 0x000490F4
		public GameMenuPartyItemVM(Action<GameMenuPartyItemVM> onSetAsContextMenuActiveItem, PartyBase item, bool canShowQuest)
		{
			this._onSetAsContextMenuActiveItem = onSetAsContextMenuActiveItem;
			this.Party = item;
			CharacterObject visualPartyLeader = PartyBaseHelper.GetVisualPartyLeader(this.Party);
			if (visualPartyLeader != null)
			{
				CharacterCode characterCode = CampaignUIHelper.GetCharacterCode(visualPartyLeader, false);
				this.Visual = new CharacterImageIdentifierVM(characterCode);
			}
			else
			{
				this.Visual = new CharacterImageIdentifierVM(null);
			}
			this.Quests = new MBBindingList<QuestMarkerVM>();
			this._canShowQuest = canShowQuest;
			this.RefreshProperties();
			this.RegisterEvents();
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x0004AF74 File Offset: 0x00049174
		public GameMenuPartyItemVM(Action<GameMenuPartyItemVM> onSetAsContextMenuActiveItem, CharacterObject character, bool useCivilianEquipment)
		{
			this._onSetAsContextMenuActiveItem = onSetAsContextMenuActiveItem;
			this.Character = character;
			this._useCivilianEquipment = useCivilianEquipment;
			CharacterCode characterCode = CampaignUIHelper.GetCharacterCode(character, useCivilianEquipment);
			this.Visual = new CharacterImageIdentifierVM(characterCode);
			Hero heroObject = this.Character.HeroObject;
			this.Banner_9 = (((heroObject != null && heroObject.IsLord) || (this.Character.IsHero && this.Character.HeroObject.Clan == Clan.PlayerClan && character.HeroObject.IsLord)) ? new BannerImageIdentifierVM(this.Character.HeroObject.ClanBanner, true) : new BannerImageIdentifierVM(null, false));
			this.NameText = this.Character.Name.ToString();
			this.PartySize = -1;
			this.PartyWoundedSize = -1;
			this.PartySizeLbl = "";
			this.IsPlayer = character.IsPlayerCharacter;
			this.Quests = new MBBindingList<QuestMarkerVM>();
			this.RefreshProperties();
			this.RegisterEvents();
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x0004B088 File Offset: 0x00049288
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.RefreshProperties();
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x0004B096 File Offset: 0x00049296
		public void ExecuteSetAsContextMenuItem()
		{
			Action<GameMenuPartyItemVM> onSetAsContextMenuActiveItem = this._onSetAsContextMenuActiveItem;
			if (onSetAsContextMenuActiveItem == null)
			{
				return;
			}
			onSetAsContextMenuActiveItem(this);
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x0004B0AC File Offset: 0x000492AC
		public void ExecuteOpenEncyclopedia()
		{
			string encyclopediaPageLink = this.GetEncyclopediaPageLink();
			if (!string.IsNullOrEmpty(encyclopediaPageLink))
			{
				Campaign.Current.EncyclopediaManager.GoToLink(encyclopediaPageLink);
			}
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x0004B0D8 File Offset: 0x000492D8
		public void ExecuteCloseTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x0004B0E0 File Offset: 0x000492E0
		public void ExecuteOpenTooltip()
		{
			PartyBase party = this.Party;
			if (((party != null) ? party.MobileParty : null) != null)
			{
				InformationManager.ShowTooltip(typeof(MobileParty), new object[]
				{
					this.Party.MobileParty,
					true,
					false
				});
				return;
			}
			if (this.Settlement != null)
			{
				InformationManager.ShowTooltip(typeof(Settlement), new object[] { this.Settlement });
				return;
			}
			InformationManager.ShowTooltip(typeof(Hero), new object[]
			{
				this.Character.HeroObject,
				true
			});
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x0004B18C File Offset: 0x0004938C
		public void RefreshProperties()
		{
			this.EncyclopediaCursorEffect = ((!string.IsNullOrEmpty(this.GetEncyclopediaPageLink())) ? "RightClickLink" : null);
			if (this.Party != null)
			{
				this.RefreshCounts();
				this.Relation = HeroVM.GetRelation(this.Party.LeaderHero);
				this.LocationText = " ";
				TextObject textObject = this.Party.Name;
				if (this.Party.IsMobile)
				{
					textObject = this.Party.MobileParty.Name;
					this.UpdateLocationText();
					this.DescriptionText = this.GetPartyDescriptionTextFromValues();
					this.IsMergedWithArmy = true;
					if (this.Party.MobileParty.Army != null)
					{
						this.IsMergedWithArmy = this.Party.MobileParty.Army.DoesLeaderPartyAndAttachedPartiesContain(this.Party.MobileParty);
					}
				}
				this.NameText = textObject.ToString();
				this.ProfessionText = " ";
				this.HasShips = this.Party.Ships.Count > 0;
			}
			else if (this.Character != null)
			{
				this.Relation = HeroVM.GetRelation(this.Character.HeroObject);
				Hero heroObject = this.Character.HeroObject;
				this.IsCharacterInPrison = heroObject != null && heroObject.IsPrisoner;
				GameTexts.SetVariable("PROFESSION", HeroHelper.GetCharacterTypeName(this.Character.HeroObject));
				string text = "LOCATION";
				Hero heroObject2 = this.Character.HeroObject;
				GameTexts.SetVariable(text, (((heroObject2 != null) ? heroObject2.CurrentSettlement : null) != null) ? this.Character.HeroObject.CurrentSettlement.Name.ToString() : "");
				Hero heroObject3 = this.Character.HeroObject;
				this.DescriptionText = ((heroObject3 != null && !heroObject3.IsSpecial) ? GameTexts.FindText("str_character_in_town", null).ToString() : string.Empty);
				string text2 = "LOCATION";
				LocationComplex locationComplex = LocationComplex.Current;
				TextObject textObject2;
				if (locationComplex == null)
				{
					textObject2 = null;
				}
				else
				{
					Location locationOfCharacter = locationComplex.GetLocationOfCharacter(this.Character.HeroObject);
					textObject2 = ((locationOfCharacter != null) ? locationOfCharacter.Name : null);
				}
				GameTexts.SetVariable(text2, textObject2 ?? TextObject.GetEmpty());
				this.LocationText = GameTexts.FindText("str_location_colon", null).ToString();
				GameTexts.SetVariable("PROFESSION", HeroHelper.GetCharacterTypeName(this.Character.HeroObject));
				this.ProfessionText = GameTexts.FindText("str_profession_colon", null).ToString();
				if (this.Character.IsHero && this.Character.HeroObject.IsNotable)
				{
					GameTexts.SetVariable("POWER", Campaign.Current.Models.NotablePowerModel.GetPowerRankName(this.Character.HeroObject).ToString());
					this.PowerText = GameTexts.FindText("str_power_colon", null).ToString();
				}
				this.NameText = this.Character.Name.ToString();
				this.HasShips = false;
			}
			this.RefreshQuestStatus();
			this.RefreshRelationStatus();
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x0004B478 File Offset: 0x00049678
		public void RefreshQuestStatus()
		{
			this.Quests.Clear();
			PartyBase party = this.Party;
			Hero hero;
			if ((hero = ((party != null) ? party.LeaderHero : null)) == null)
			{
				CharacterObject character = this.Character;
				hero = ((character != null) ? character.HeroObject : null);
			}
			Hero hero2 = hero;
			if (hero2 != null)
			{
				GameMenuPartyItemVM.<>c__DisplayClass16_0 CS$<>8__locals1 = new GameMenuPartyItemVM.<>c__DisplayClass16_0();
				CS$<>8__locals1.questTypes = CampaignUIHelper.GetQuestStateOfHero(hero2);
				int k;
				int i;
				for (i = 0; i < CS$<>8__locals1.questTypes.Count; i = k + 1)
				{
					if (!this.Quests.Any<QuestMarkerVM>((QuestMarkerVM q) => q.QuestMarkerType == (int)CS$<>8__locals1.questTypes[i].Item1))
					{
						this.Quests.Add(new QuestMarkerVM(CS$<>8__locals1.questTypes[i].Item1, CS$<>8__locals1.questTypes[i].Item2, CS$<>8__locals1.questTypes[i].Item3));
					}
					k = i;
				}
			}
			else
			{
				PartyBase party2 = this.Party;
				if (((party2 != null) ? party2.MobileParty : null) != null)
				{
					List<QuestBase> questsRelatedToParty = CampaignUIHelper.GetQuestsRelatedToParty(this.Party.MobileParty);
					for (int j = 0; j < questsRelatedToParty.Count; j++)
					{
						TextObject textObject = ((questsRelatedToParty[j].JournalEntries.Count > 0) ? questsRelatedToParty[j].JournalEntries[0].LogText : TextObject.GetEmpty());
						CampaignUIHelper.IssueQuestFlags issueQuestFlags;
						if (hero2 != null && questsRelatedToParty[j].QuestGiver == hero2)
						{
							issueQuestFlags = (questsRelatedToParty[j].IsSpecialQuest ? CampaignUIHelper.IssueQuestFlags.ActiveStoryQuest : CampaignUIHelper.IssueQuestFlags.ActiveIssue);
						}
						else
						{
							issueQuestFlags = (questsRelatedToParty[j].IsSpecialQuest ? CampaignUIHelper.IssueQuestFlags.TrackedStoryQuest : CampaignUIHelper.IssueQuestFlags.TrackedIssue);
						}
						this.Quests.Add(new QuestMarkerVM(issueQuestFlags, questsRelatedToParty[j].Title, textObject));
					}
				}
			}
			this.Quests.Sort(new GameMenuPartyItemVM.QuestMarkerComparer());
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x0004B688 File Offset: 0x00049888
		private void UpdateLocationText()
		{
			float getEncounterJoiningRadius = Campaign.Current.Models.EncounterModel.GetEncounterJoiningRadius;
			this.LocationText = string.Empty;
			if (this.Party.MobileParty.Position.DistanceSquared(MobileParty.MainParty.Position) >= getEncounterJoiningRadius * getEncounterJoiningRadius)
			{
				if (this.Party.MobileParty.MapEvent != null)
				{
					TextObject textObject = GameTexts.FindText("str_at_map_event", null);
					TextObject textObject2 = new TextObject("{=zawBaxl5}Distance : {DISTANCE}", null);
					textObject2.SetTextVariable("DISTANCE", textObject);
					this.LocationText = textObject2.ToString();
					return;
				}
				MobileParty attachedTo = this.Party.MobileParty.AttachedTo;
				if (((attachedTo != null) ? attachedTo.Army : null) == null)
				{
					GameTexts.SetVariable("LEFT", GameTexts.FindText("str_distance_to_army_leader", null));
					float num = DistanceHelper.FindClosestDistanceFromMobilePartyToMobileParty(this.Party.MobileParty, MobileParty.MainParty, this.Party.MobileParty.NavigationCapability);
					GameTexts.SetVariable("RIGHT", CampaignUIHelper.GetPartyDistanceByTimeText((float)((int)num), this.Party.MobileParty.Speed));
					this.LocationText = GameTexts.FindText("str_LEFT_colon_RIGHT_wSpaceAfterColon", null).ToString();
				}
			}
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x0004B7B8 File Offset: 0x000499B8
		private void RefreshRelationStatus()
		{
			this.IsEnemy = false;
			this.IsAlly = false;
			this.IsNeutral = false;
			IFaction faction = null;
			bool flag = false;
			if (this.Character != null)
			{
				this.IsPlayer = this.Character.IsPlayerCharacter;
				flag = this.Character.IsHero && this.Character.HeroObject.IsNotable;
				IFaction faction2;
				if (!this.IsPlayer)
				{
					CharacterObject character = this.Character;
					faction2 = ((character != null) ? character.HeroObject.MapFaction : null);
				}
				else
				{
					faction2 = null;
				}
				faction = faction2;
				bool flag2;
				if (!this.IsPlayer)
				{
					Hero heroObject = this.Character.HeroObject;
					if (heroObject == null)
					{
						flag2 = false;
					}
					else
					{
						Clan clan = heroObject.Clan;
						bool? flag3 = ((clan != null) ? new bool?(clan.HasBloodFeudWithPlayer) : null);
						bool flag4 = true;
						flag2 = (flag3.GetValueOrDefault() == flag4) & (flag3 != null);
					}
				}
				else
				{
					flag2 = false;
				}
				this.HasBloodFeud = flag2;
			}
			else if (this.Party != null)
			{
				bool flag5;
				if (this.Party.IsMobile)
				{
					MobileParty mobileParty = this.Party.MobileParty;
					flag5 = mobileParty != null && mobileParty.IsMainParty;
				}
				else
				{
					flag5 = false;
				}
				this.IsPlayer = flag5;
				flag = false;
				IFaction faction3;
				if (!this.IsPlayer)
				{
					PartyBase party = this.Party;
					if (party == null)
					{
						faction3 = null;
					}
					else
					{
						MobileParty mobileParty2 = party.MobileParty;
						faction3 = ((mobileParty2 != null) ? mobileParty2.MapFaction : null);
					}
				}
				else
				{
					faction3 = null;
				}
				faction = faction3;
				bool flag6;
				if (!this.IsPlayer)
				{
					MobileParty mobileParty3 = this.Party.MobileParty;
					if (mobileParty3 == null)
					{
						flag6 = false;
					}
					else
					{
						Clan actualClan = mobileParty3.ActualClan;
						bool? flag3 = ((actualClan != null) ? new bool?(actualClan.HasBloodFeudWithPlayer) : null);
						bool flag4 = true;
						flag6 = (flag3.GetValueOrDefault() == flag4) & (flag3 != null);
					}
				}
				else
				{
					flag6 = false;
				}
				this.HasBloodFeud = flag6;
			}
			if (this.IsPlayer || faction == null || flag)
			{
				if (!this.IsPlayer)
				{
					this.IsNeutral = true;
				}
				return;
			}
			if (FactionManager.IsAtWarAgainstFaction(faction, Hero.MainHero.MapFaction))
			{
				this.IsEnemy = true;
				return;
			}
			if (DiplomacyHelper.IsSameFactionAndNotEliminated(faction, Hero.MainHero.MapFaction))
			{
				this.IsAlly = true;
				return;
			}
			this.IsNeutral = true;
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x0004B9B0 File Offset: 0x00049BB0
		public void RefreshVisual()
		{
			if (this.Visual.IsEmpty)
			{
				if (this.Character != null)
				{
					CharacterCode characterCode = CampaignUIHelper.GetCharacterCode(this.Character, this._useCivilianEquipment);
					this.Visual = new CharacterImageIdentifierVM(characterCode);
					return;
				}
				if (this.Party != null)
				{
					CharacterObject visualPartyLeader = PartyBaseHelper.GetVisualPartyLeader(this.Party);
					if (visualPartyLeader != null)
					{
						CharacterCode characterCode2 = CampaignUIHelper.GetCharacterCode(visualPartyLeader, false);
						this.Visual = new CharacterImageIdentifierVM(characterCode2);
						return;
					}
					this.Visual = new CharacterImageIdentifierVM(null);
				}
			}
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x0004BA2C File Offset: 0x00049C2C
		public void RefreshCounts()
		{
			if (this.PartySize != this.Party.NumberOfHealthyMembers || this.PartyWoundedSize != this.Party.NumberOfAllMembers - this.Party.NumberOfHealthyMembers)
			{
				this.PartyWoundedSize = this.Party.NumberOfAllMembers - this.Party.NumberOfHealthyMembers;
				this.PartySize = this.Party.NumberOfHealthyMembers;
				MobileParty mobileParty = this.Party.MobileParty;
				this.PartySizeLbl = ((mobileParty != null && mobileParty.IsInfoHidden) ? "?" : this.Party.NumberOfHealthyMembers.ToString());
			}
			MBReadOnlyList<Ship> ships = this.Party.Ships;
			this.ShipCount = ((ships != null) ? ships.Count : 0);
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x0004BAF0 File Offset: 0x00049CF0
		public string GetPartyDescriptionTextFromValues()
		{
			GameTexts.SetVariable("newline", "\n");
			string text = ((this.Party.MobileParty.CurrentSettlement != null && this.Party.MobileParty.MapEvent == null) ? "" : CampaignUIHelper.GetMobilePartyBehaviorText(this.Party.MobileParty));
			GameTexts.SetVariable("LEFT", GameTexts.FindText("str_food", null).ToString());
			GameTexts.SetVariable("RIGHT", this.Party.MobileParty.Food);
			string text2 = GameTexts.FindText("str_LEFT_colon_RIGHT", null).ToString();
			GameTexts.SetVariable("LEFT", GameTexts.FindText("str_map_tooltip_speed", null).ToString());
			GameTexts.SetVariable("RIGHT", this.Party.MobileParty.Speed.ToString("F"));
			string text3 = GameTexts.FindText("str_LEFT_colon_RIGHT", null).ToString();
			GameTexts.SetVariable("LEFT", GameTexts.FindText("str_view_distance", null).ToString());
			GameTexts.SetVariable("RIGHT", this.Party.MobileParty.SeeingRange);
			string text4 = GameTexts.FindText("str_LEFT_colon_RIGHT", null).ToString();
			GameTexts.SetVariable("STR1", text);
			GameTexts.SetVariable("STR2", text2);
			string text5 = GameTexts.FindText("str_string_newline_string", null).ToString();
			GameTexts.SetVariable("STR1", text5);
			GameTexts.SetVariable("STR2", text3);
			text5 = GameTexts.FindText("str_string_newline_string", null).ToString();
			GameTexts.SetVariable("STR1", text5);
			GameTexts.SetVariable("STR2", text4);
			return GameTexts.FindText("str_string_newline_string", null).ToString();
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x0004BC9D File Offset: 0x00049E9D
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.UnregisterEvents();
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x0004BCAB File Offset: 0x00049EAB
		private void RegisterEvents()
		{
			CampaignEvents.OnPlayerBodyPropertiesChangedEvent.AddNonSerializedListener(this, new Action(this.OnPlayerCharacterChangedEvent));
			CampaignEvents.OnBloodFeudStateChangedEvent.AddNonSerializedListener(this, new Action<Clan, Hero, ChangeBloodFeudStateAction.ChangeBloodFeudActionDetail>(this.OnBloodFeudStateChanged));
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x0004BCDC File Offset: 0x00049EDC
		private void OnPlayerCharacterChangedEvent()
		{
			CharacterObject characterObject = ((this.Party != null) ? PartyBaseHelper.GetVisualPartyLeader(this.Party) : this.Character);
			if (characterObject == CharacterObject.PlayerCharacter)
			{
				CharacterCode characterCode = CampaignUIHelper.GetCharacterCode(characterObject, false);
				this.Visual = new CharacterImageIdentifierVM(characterCode);
			}
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x0004BD24 File Offset: 0x00049F24
		private void OnBloodFeudStateChanged(Clan clan, Hero executedHero, ChangeBloodFeudStateAction.ChangeBloodFeudActionDetail detail)
		{
			PartyBase party = this.Party;
			Clan clan2;
			if (party == null)
			{
				clan2 = null;
			}
			else
			{
				MobileParty mobileParty = party.MobileParty;
				clan2 = ((mobileParty != null) ? mobileParty.ActualClan : null);
			}
			if (clan2 != clan)
			{
				CharacterObject character = this.Character;
				Clan clan3;
				if (character == null)
				{
					clan3 = null;
				}
				else
				{
					Hero heroObject = character.HeroObject;
					clan3 = ((heroObject != null) ? heroObject.Clan : null);
				}
				if (clan3 != clan)
				{
					return;
				}
			}
			this.RefreshRelationStatus();
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x0004BD79 File Offset: 0x00049F79
		private void UnregisterEvents()
		{
			CampaignEventDispatcher.Instance.RemoveListeners(this);
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x0004BD88 File Offset: 0x00049F88
		private string GetEncyclopediaPageLink()
		{
			PartyBase party = this.Party;
			if (party == null || !party.MobileParty.IsCaravan)
			{
				PartyBase party2 = this.Party;
				if (party2 == null || !party2.MobileParty.IsGarrison)
				{
					PartyBase party3 = this.Party;
					if (party3 == null || !party3.MobileParty.IsMilitia)
					{
						PartyBase party4 = this.Party;
						if (party4 == null || !party4.MobileParty.IsVillager)
						{
							if (this.Character != null)
							{
								return this.Character.EncyclopediaLink;
							}
							if (this.Party != null)
							{
								if (this.Party.LeaderHero != null)
								{
									return this.Party.LeaderHero.EncyclopediaLink;
								}
								if (this.Party.Owner != null)
								{
									return this.Party.Owner.EncyclopediaLink;
								}
								CharacterObject visualPartyLeader = CampaignUIHelper.GetVisualPartyLeader(this.Party);
								if (visualPartyLeader != null)
								{
									return visualPartyLeader.EncyclopediaLink;
								}
							}
							return null;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x0600128E RID: 4750 RVA: 0x0004BE6A File Offset: 0x0004A06A
		// (set) Token: 0x0600128F RID: 4751 RVA: 0x0004BE72 File Offset: 0x0004A072
		[DataSourceProperty]
		public int Relation
		{
			get
			{
				return this._relation;
			}
			set
			{
				if (value != this._relation)
				{
					this._relation = value;
					base.OnPropertyChangedWithValue(value, "Relation");
				}
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001290 RID: 4752 RVA: 0x0004BE90 File Offset: 0x0004A090
		// (set) Token: 0x06001291 RID: 4753 RVA: 0x0004BE98 File Offset: 0x0004A098
		[DataSourceProperty]
		public MBBindingList<QuestMarkerVM> Quests
		{
			get
			{
				return this._quests;
			}
			set
			{
				if (value != this._quests)
				{
					this._quests = value;
					base.OnPropertyChangedWithValue<MBBindingList<QuestMarkerVM>>(value, "Quests");
				}
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06001292 RID: 4754 RVA: 0x0004BEB6 File Offset: 0x0004A0B6
		// (set) Token: 0x06001293 RID: 4755 RVA: 0x0004BEBE File Offset: 0x0004A0BE
		[DataSourceProperty]
		public bool IsHighlightEnabled
		{
			get
			{
				return this._isHighlightEnabled;
			}
			set
			{
				if (value != this._isHighlightEnabled)
				{
					this._isHighlightEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsHighlightEnabled");
				}
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001294 RID: 4756 RVA: 0x0004BEDC File Offset: 0x0004A0DC
		// (set) Token: 0x06001295 RID: 4757 RVA: 0x0004BEE4 File Offset: 0x0004A0E4
		[DataSourceProperty]
		public bool IsCharacterInPrison
		{
			get
			{
				return this._isCharacterInPrison;
			}
			set
			{
				if (value != this._isCharacterInPrison)
				{
					this._isCharacterInPrison = value;
					base.OnPropertyChangedWithValue(value, "IsCharacterInPrison");
				}
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001296 RID: 4758 RVA: 0x0004BF02 File Offset: 0x0004A102
		// (set) Token: 0x06001297 RID: 4759 RVA: 0x0004BF0A File Offset: 0x0004A10A
		[DataSourceProperty]
		public bool HasShips
		{
			get
			{
				return this._hasShips;
			}
			set
			{
				if (value != this._hasShips)
				{
					this._hasShips = value;
					base.OnPropertyChangedWithValue(value, "HasShips");
				}
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001298 RID: 4760 RVA: 0x0004BF28 File Offset: 0x0004A128
		// (set) Token: 0x06001299 RID: 4761 RVA: 0x0004BF30 File Offset: 0x0004A130
		[DataSourceProperty]
		public bool IsIdle
		{
			get
			{
				return this._isIdle;
			}
			set
			{
				if (value != this._isIdle)
				{
					this._isIdle = value;
					base.OnPropertyChangedWithValue(value, "IsIdle");
				}
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x0600129A RID: 4762 RVA: 0x0004BF4E File Offset: 0x0004A14E
		// (set) Token: 0x0600129B RID: 4763 RVA: 0x0004BF56 File Offset: 0x0004A156
		[DataSourceProperty]
		public bool IsPlayer
		{
			get
			{
				return this._isPlayer;
			}
			set
			{
				if (value != this._isPlayer)
				{
					this._isPlayer = value;
					base.OnPropertyChanged("IsPlayerParty");
				}
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x0600129C RID: 4764 RVA: 0x0004BF73 File Offset: 0x0004A173
		// (set) Token: 0x0600129D RID: 4765 RVA: 0x0004BF7B File Offset: 0x0004A17B
		[DataSourceProperty]
		public bool IsEnemy
		{
			get
			{
				return this._isEnemy;
			}
			set
			{
				if (value != this._isEnemy)
				{
					this._isEnemy = value;
					base.OnPropertyChangedWithValue(value, "IsEnemy");
				}
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x0600129E RID: 4766 RVA: 0x0004BF99 File Offset: 0x0004A199
		// (set) Token: 0x0600129F RID: 4767 RVA: 0x0004BFA1 File Offset: 0x0004A1A1
		[DataSourceProperty]
		public bool IsAlly
		{
			get
			{
				return this._isAlly;
			}
			set
			{
				if (value != this._isAlly)
				{
					this._isAlly = value;
					base.OnPropertyChangedWithValue(value, "IsAlly");
				}
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x060012A0 RID: 4768 RVA: 0x0004BFBF File Offset: 0x0004A1BF
		// (set) Token: 0x060012A1 RID: 4769 RVA: 0x0004BFC7 File Offset: 0x0004A1C7
		[DataSourceProperty]
		public bool IsNeutral
		{
			get
			{
				return this._isNeutral;
			}
			set
			{
				if (value != this._isNeutral)
				{
					this._isNeutral = value;
					base.OnPropertyChangedWithValue(value, "IsNeutral");
				}
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x060012A2 RID: 4770 RVA: 0x0004BFE5 File Offset: 0x0004A1E5
		// (set) Token: 0x060012A3 RID: 4771 RVA: 0x0004BFED File Offset: 0x0004A1ED
		[DataSourceProperty]
		public bool IsMergedWithArmy
		{
			get
			{
				return this._isMergedWithArmy;
			}
			set
			{
				if (value != this._isMergedWithArmy)
				{
					this._isMergedWithArmy = value;
					base.OnPropertyChangedWithValue(value, "IsMergedWithArmy");
				}
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x060012A4 RID: 4772 RVA: 0x0004C00B File Offset: 0x0004A20B
		// (set) Token: 0x060012A5 RID: 4773 RVA: 0x0004C013 File Offset: 0x0004A213
		[DataSourceProperty]
		public bool HasBloodFeud
		{
			get
			{
				return this._hasBloodFeud;
			}
			set
			{
				if (value != this._hasBloodFeud)
				{
					this._hasBloodFeud = value;
					base.OnPropertyChangedWithValue(value, "HasBloodFeud");
				}
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x060012A6 RID: 4774 RVA: 0x0004C031 File Offset: 0x0004A231
		// (set) Token: 0x060012A7 RID: 4775 RVA: 0x0004C039 File Offset: 0x0004A239
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x060012A8 RID: 4776 RVA: 0x0004C05C File Offset: 0x0004A25C
		// (set) Token: 0x060012A9 RID: 4777 RVA: 0x0004C064 File Offset: 0x0004A264
		[DataSourceProperty]
		public string SettlementPath
		{
			get
			{
				return this._settlementPath;
			}
			set
			{
				if (value != this._settlementPath)
				{
					this._settlementPath = value;
					base.OnPropertyChangedWithValue<string>(value, "SettlementPath");
				}
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x060012AA RID: 4778 RVA: 0x0004C087 File Offset: 0x0004A287
		// (set) Token: 0x060012AB RID: 4779 RVA: 0x0004C08F File Offset: 0x0004A28F
		[DataSourceProperty]
		public string LocationText
		{
			get
			{
				return this._locationText;
			}
			set
			{
				if (value != this._locationText)
				{
					this._locationText = value;
					base.OnPropertyChangedWithValue<string>(value, "LocationText");
				}
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x060012AC RID: 4780 RVA: 0x0004C0B2 File Offset: 0x0004A2B2
		// (set) Token: 0x060012AD RID: 4781 RVA: 0x0004C0BA File Offset: 0x0004A2BA
		[DataSourceProperty]
		public string PowerText
		{
			get
			{
				return this._powerText;
			}
			set
			{
				if (value != this._powerText)
				{
					this._powerText = value;
					base.OnPropertyChangedWithValue<string>(value, "PowerText");
				}
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x060012AE RID: 4782 RVA: 0x0004C0DD File Offset: 0x0004A2DD
		// (set) Token: 0x060012AF RID: 4783 RVA: 0x0004C0E5 File Offset: 0x0004A2E5
		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (value != this._descriptionText)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptionText");
				}
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x060012B0 RID: 4784 RVA: 0x0004C108 File Offset: 0x0004A308
		// (set) Token: 0x060012B1 RID: 4785 RVA: 0x0004C110 File Offset: 0x0004A310
		[DataSourceProperty]
		public string ProfessionText
		{
			get
			{
				return this._professionText;
			}
			set
			{
				if (value != this._professionText)
				{
					this._professionText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProfessionText");
				}
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x060012B2 RID: 4786 RVA: 0x0004C133 File Offset: 0x0004A333
		// (set) Token: 0x060012B3 RID: 4787 RVA: 0x0004C13B File Offset: 0x0004A33B
		[DataSourceProperty]
		public string EncyclopediaCursorEffect
		{
			get
			{
				return this._encyclopediaCursorEffect;
			}
			set
			{
				if (value != this._encyclopediaCursorEffect)
				{
					this._encyclopediaCursorEffect = value;
					base.OnPropertyChangedWithValue<string>(value, "EncyclopediaCursorEffect");
				}
			}
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x060012B4 RID: 4788 RVA: 0x0004C15E File Offset: 0x0004A35E
		// (set) Token: 0x060012B5 RID: 4789 RVA: 0x0004C166 File Offset: 0x0004A366
		[DataSourceProperty]
		public CharacterImageIdentifierVM Visual
		{
			get
			{
				return this._visual;
			}
			set
			{
				if (value != this._visual)
				{
					this._visual = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x060012B6 RID: 4790 RVA: 0x0004C184 File Offset: 0x0004A384
		// (set) Token: 0x060012B7 RID: 4791 RVA: 0x0004C18C File Offset: 0x0004A38C
		[DataSourceProperty]
		public BannerImageIdentifierVM Banner_9
		{
			get
			{
				return this._banner_9;
			}
			set
			{
				if (value != this._banner_9)
				{
					this._banner_9 = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "Banner_9");
				}
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x060012B8 RID: 4792 RVA: 0x0004C1AA File Offset: 0x0004A3AA
		// (set) Token: 0x060012B9 RID: 4793 RVA: 0x0004C1B2 File Offset: 0x0004A3B2
		[DataSourceProperty]
		public int PartySize
		{
			get
			{
				return this._partySize;
			}
			set
			{
				if (value != this._partySize)
				{
					this._partySize = value;
					base.OnPropertyChangedWithValue(value, "PartySize");
				}
			}
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x060012BA RID: 4794 RVA: 0x0004C1D0 File Offset: 0x0004A3D0
		// (set) Token: 0x060012BB RID: 4795 RVA: 0x0004C1D8 File Offset: 0x0004A3D8
		[DataSourceProperty]
		public int PartyWoundedSize
		{
			get
			{
				return this._partyWoundedSize;
			}
			set
			{
				if (value != this._partySize)
				{
					this._partyWoundedSize = value;
					base.OnPropertyChangedWithValue(value, "PartyWoundedSize");
				}
			}
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x060012BC RID: 4796 RVA: 0x0004C1F6 File Offset: 0x0004A3F6
		// (set) Token: 0x060012BD RID: 4797 RVA: 0x0004C1FE File Offset: 0x0004A3FE
		[DataSourceProperty]
		public int ShipCount
		{
			get
			{
				return this._shipCount;
			}
			set
			{
				if (value != this._shipCount)
				{
					this._shipCount = value;
					base.OnPropertyChangedWithValue(value, "ShipCount");
				}
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x060012BE RID: 4798 RVA: 0x0004C21C File Offset: 0x0004A41C
		// (set) Token: 0x060012BF RID: 4799 RVA: 0x0004C224 File Offset: 0x0004A424
		[DataSourceProperty]
		public string PartySizeLbl
		{
			get
			{
				return this._partySizeLbl;
			}
			set
			{
				if (value != this._partySizeLbl)
				{
					this._partySizeLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "PartySizeLbl");
				}
			}
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x060012C0 RID: 4800 RVA: 0x0004C247 File Offset: 0x0004A447
		// (set) Token: 0x060012C1 RID: 4801 RVA: 0x0004C24F File Offset: 0x0004A44F
		[DataSourceProperty]
		public bool IsLeader
		{
			get
			{
				return this._isLeader;
			}
			set
			{
				if (value != this._isLeader)
				{
					this._isLeader = value;
					base.OnPropertyChangedWithValue(value, "IsLeader");
				}
			}
		}

		// Token: 0x04000862 RID: 2146
		public CharacterObject Character;

		// Token: 0x04000863 RID: 2147
		public PartyBase Party;

		// Token: 0x04000864 RID: 2148
		public Settlement Settlement;

		// Token: 0x04000865 RID: 2149
		private readonly bool _canShowQuest = true;

		// Token: 0x04000866 RID: 2150
		private readonly bool _useCivilianEquipment;

		// Token: 0x04000867 RID: 2151
		private readonly Action<GameMenuPartyItemVM> _onSetAsContextMenuActiveItem;

		// Token: 0x04000868 RID: 2152
		private MBBindingList<QuestMarkerVM> _quests;

		// Token: 0x04000869 RID: 2153
		private int _partySize;

		// Token: 0x0400086A RID: 2154
		private int _partyWoundedSize;

		// Token: 0x0400086B RID: 2155
		private int _shipCount;

		// Token: 0x0400086C RID: 2156
		private int _relation = -101;

		// Token: 0x0400086D RID: 2157
		private CharacterImageIdentifierVM _visual;

		// Token: 0x0400086E RID: 2158
		private BannerImageIdentifierVM _banner_9;

		// Token: 0x0400086F RID: 2159
		private string _settlementPath;

		// Token: 0x04000870 RID: 2160
		private string _partySizeLbl;

		// Token: 0x04000871 RID: 2161
		private string _nameText;

		// Token: 0x04000872 RID: 2162
		private string _locationText;

		// Token: 0x04000873 RID: 2163
		private string _descriptionText;

		// Token: 0x04000874 RID: 2164
		private string _professionText;

		// Token: 0x04000875 RID: 2165
		private string _powerText;

		// Token: 0x04000876 RID: 2166
		private string _encyclopediaCursorEffect;

		// Token: 0x04000877 RID: 2167
		private bool _isIdle;

		// Token: 0x04000878 RID: 2168
		private bool _isPlayer;

		// Token: 0x04000879 RID: 2169
		private bool _isEnemy;

		// Token: 0x0400087A RID: 2170
		private bool _isAlly;

		// Token: 0x0400087B RID: 2171
		private bool _isNeutral;

		// Token: 0x0400087C RID: 2172
		private bool _isHighlightEnabled;

		// Token: 0x0400087D RID: 2173
		private bool _isLeader;

		// Token: 0x0400087E RID: 2174
		private bool _isMergedWithArmy;

		// Token: 0x0400087F RID: 2175
		private bool _isCharacterInPrison;

		// Token: 0x04000880 RID: 2176
		private bool _hasShips;

		// Token: 0x04000881 RID: 2177
		private bool _hasBloodFeud;

		// Token: 0x02000239 RID: 569
		private class QuestMarkerComparer : IComparer<QuestMarkerVM>
		{
			// Token: 0x060025FA RID: 9722 RVA: 0x000829F4 File Offset: 0x00080BF4
			public int Compare(QuestMarkerVM x, QuestMarkerVM y)
			{
				return x.QuestMarkerType.CompareTo(y.QuestMarkerType);
			}
		}
	}
}
