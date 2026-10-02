using System;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Overlay
{
	// Token: 0x020000BD RID: 189
	public class GameMenuOverlay : ViewModel
	{
		// Token: 0x06001258 RID: 4696 RVA: 0x0004A49E File Offset: 0x0004869E
		public GameMenuOverlay()
		{
			this.ContextList = new MBBindingList<StringItemWithEnabledAndHintVM>();
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x0004A4BF File Offset: 0x000486BF
		public override void RefreshValues()
		{
			base.RefreshValues();
			GameMenuPartyItemVM contextMenuItem = this._contextMenuItem;
			if (contextMenuItem == null)
			{
				return;
			}
			contextMenuItem.RefreshValues();
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x0004A4D7 File Offset: 0x000486D7
		protected virtual void ExecuteOnSetAsActiveContextMenuItem(GameMenuPartyItemVM troop)
		{
			this._contextMenuItem = troop;
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x0004A4E0 File Offset: 0x000486E0
		public virtual void ExecuteOnOverlayClosed()
		{
			if (!this._closedHandled)
			{
				CampaignEventDispatcher.Instance.OnCharacterPortraitPopUpClosed();
				this._closedHandled = true;
			}
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x0004A4FB File Offset: 0x000486FB
		public virtual void ExecuteOnOverlayOpened()
		{
			this._closedHandled = false;
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x0004A504 File Offset: 0x00048704
		public override void OnFinalize()
		{
			base.OnFinalize();
			if (!this._closedHandled)
			{
				this.ExecuteOnOverlayClosed();
			}
			InputKeyItemVM exitInputKey = this.ExitInputKey;
			if (exitInputKey == null)
			{
				return;
			}
			exitInputKey.OnFinalize();
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x0004A52C File Offset: 0x0004872C
		protected void ExecuteTroopAction(object o)
		{
			switch ((GameMenuOverlay.MenuOverlayContextList)o)
			{
			case GameMenuOverlay.MenuOverlayContextList.Encyclopedia:
				if (this._contextMenuItem.Character != null)
				{
					if (this._contextMenuItem.Character.IsHero)
					{
						Campaign.Current.EncyclopediaManager.GoToLink(this._contextMenuItem.Character.HeroObject.EncyclopediaLink);
					}
					else
					{
						Debug.FailedAssert("Character object in menu overlay", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\GameMenu\\Overlay\\GameMenuOverlay.cs", "ExecuteTroopAction", 101);
						Campaign.Current.EncyclopediaManager.GoToLink(this._contextMenuItem.Character.EncyclopediaLink);
					}
				}
				else if (this._contextMenuItem.Party != null)
				{
					CharacterObject visualPartyLeader = CampaignUIHelper.GetVisualPartyLeader(this._contextMenuItem.Party);
					if (visualPartyLeader != null)
					{
						Campaign.Current.EncyclopediaManager.GoToLink(visualPartyLeader.EncyclopediaLink);
					}
				}
				else if (this._contextMenuItem.Settlement != null)
				{
					Campaign.Current.EncyclopediaManager.GoToLink(this._contextMenuItem.Settlement.EncyclopediaLink);
				}
				break;
			case GameMenuOverlay.MenuOverlayContextList.Conversation:
				if (this._contextMenuItem.Character != null)
				{
					if (this._contextMenuItem.Character.IsHero)
					{
						if (PlayerEncounter.Current != null || LocationComplex.Current != null || Campaign.Current.CurrentMenuContext != null)
						{
							Location location = LocationComplex.Current.GetLocationOfCharacter(this._contextMenuItem.Character.HeroObject);
							if (location.StringId == "alley")
							{
								location = LocationComplex.Current.GetLocationWithId("center");
							}
							CampaignEventDispatcher.Instance.OnPlayerStartTalkFromMenu(this._contextMenuItem.Character.HeroObject);
							PlayerEncounter.LocationEncounter.CreateAndOpenMissionController(location, null, this._contextMenuItem.Character, null);
						}
						else
						{
							EncounterManager.StartPartyEncounter(PartyBase.MainParty, this._contextMenuItem.Character.HeroObject.PartyBelongedTo.Party);
						}
					}
					else
					{
						Debug.FailedAssert("Character object in menu overlay", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\GameMenu\\Overlay\\GameMenuOverlay.cs", "ExecuteTroopAction", 145);
					}
				}
				break;
			case GameMenuOverlay.MenuOverlayContextList.QuickConversation:
				if (this._contextMenuItem.Character != null)
				{
					if (this._contextMenuItem.Character.IsHero)
					{
						if (PlayerEncounter.Current != null || LocationComplex.Current != null || Campaign.Current.CurrentMenuContext != null)
						{
							CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, false, false, false, true, false, false), new ConversationCharacterData(this._contextMenuItem.Character, null, false, false, false, true, false, false));
						}
						else
						{
							EncounterManager.StartPartyEncounter(PartyBase.MainParty, this._contextMenuItem.Character.HeroObject.PartyBelongedTo.Party);
						}
					}
					else
					{
						Debug.FailedAssert("Character object in menu overlay", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\GameMenu\\Overlay\\GameMenuOverlay.cs", "ExecuteTroopAction", 168);
					}
				}
				break;
			case GameMenuOverlay.MenuOverlayContextList.ConverseWithLeader:
			{
				PartyBase party = this._contextMenuItem.Party;
				if (((party != null) ? party.LeaderHero : null) != null)
				{
					if (Settlement.CurrentSettlement != null || LocationComplex.Current != null || Campaign.Current.CurrentMenuContext != null)
					{
						this.ConverseWithLeader(PartyBase.MainParty, this._contextMenuItem.Party);
					}
					else
					{
						EncounterManager.StartPartyEncounter(PartyBase.MainParty, this._contextMenuItem.Party);
					}
				}
				break;
			}
			case GameMenuOverlay.MenuOverlayContextList.ArmyDismiss:
			{
				PartyBase party2 = this._contextMenuItem.Party;
				if (((party2 != null) ? party2.MobileParty.Army : null) != null && this._contextMenuItem.Party.MapEvent == null && this._contextMenuItem.Party.MobileParty.Army.LeaderParty != this._contextMenuItem.Party.MobileParty)
				{
					if (this._contextMenuItem.Party.MobileParty.Army.LeaderParty == MobileParty.MainParty && this._contextMenuItem.Party.MobileParty.Army.Parties.Count <= 2)
					{
						DisbandArmyAction.ApplyByNotEnoughParty(this._contextMenuItem.Party.MobileParty.Army);
					}
					else
					{
						this._contextMenuItem.Party.MobileParty.Army = null;
						this._contextMenuItem.Party.MobileParty.SetMoveModeHold();
					}
				}
				break;
			}
			case GameMenuOverlay.MenuOverlayContextList.ManageGarrison:
				if (this._contextMenuItem.Party != null)
				{
					PartyScreenHelper.OpenScreenAsManageTroops(this._contextMenuItem.Party.MobileParty);
				}
				break;
			case GameMenuOverlay.MenuOverlayContextList.DonateTroops:
				if (this._contextMenuItem.Party != null)
				{
					if (this._contextMenuItem.Party.MobileParty.IsGarrison)
					{
						PartyScreenHelper.OpenScreenAsDonateGarrisonWithCurrentSettlement();
					}
					else
					{
						PartyScreenHelper.OpenScreenAsDonateTroops(this._contextMenuItem.Party.MobileParty);
					}
				}
				break;
			case GameMenuOverlay.MenuOverlayContextList.JoinArmy:
			{
				CharacterObject character = this._contextMenuItem.Character;
				if (character != null && character.IsHero && this._contextMenuItem.Character.HeroObject.PartyBelongedTo != null)
				{
					MobileParty.MainParty.Army = this._contextMenuItem.Character.HeroObject.PartyBelongedTo.Army;
					MobileParty.MainParty.Army.AddPartyToMergedParties(MobileParty.MainParty);
					MenuContext currentMenuContext = Campaign.Current.CurrentMenuContext;
					if (currentMenuContext != null)
					{
						currentMenuContext.Refresh();
					}
				}
				break;
			}
			case GameMenuOverlay.MenuOverlayContextList.TakeToParty:
			{
				CharacterObject character2 = this._contextMenuItem.Character;
				if (character2 != null && character2.IsHero && this._contextMenuItem.Character.HeroObject.PartyBelongedTo == null)
				{
					Settlement currentSettlement = this._contextMenuItem.Character.HeroObject.CurrentSettlement;
					bool flag;
					if (currentSettlement == null)
					{
						flag = false;
					}
					else
					{
						MBReadOnlyList<Hero> notables = currentSettlement.Notables;
						bool? flag2 = ((notables != null) ? new bool?(notables.Contains(this._contextMenuItem.Character.HeroObject)) : null);
						bool flag3 = true;
						flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
					}
					if (flag)
					{
						LeaveSettlementAction.ApplyForCharacterOnly(this._contextMenuItem.Character.HeroObject);
					}
					AddHeroToPartyAction.Apply(this._contextMenuItem.Character.HeroObject, MobileParty.MainParty, true);
				}
				break;
			}
			case GameMenuOverlay.MenuOverlayContextList.ManageTroops:
			{
				PartyBase party3 = this._contextMenuItem.Party;
				if (((party3 != null) ? party3.MobileParty : null) != null && this._contextMenuItem.Party.MobileParty.ActualClan == Clan.PlayerClan)
				{
					PartyScreenHelper.OpenScreenAsManageTroopsAndPrisoners(this._contextMenuItem.Party.MobileParty, new PartyScreenClosedDelegate(PartyScreenHelper.OpenScreenAsManagePlayerClanPartyClosed));
				}
				break;
			}
			}
			if (!this._closedHandled)
			{
				CampaignEventDispatcher.Instance.OnCharacterPortraitPopUpClosed();
				this._closedHandled = true;
			}
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x0004ABC0 File Offset: 0x00048DC0
		private void ConverseWithLeader(PartyBase mainParty1, PartyBase party2)
		{
			bool flag;
			if (mainParty1.Side != BattleSideEnum.Attacker)
			{
				PlayerEncounter playerEncounter = PlayerEncounter.Current;
				flag = playerEncounter != null && playerEncounter.PlayerSide == BattleSideEnum.Attacker;
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (LocationComplex.Current != null && !flag2)
			{
				Location locationOfCharacter = LocationComplex.Current.GetLocationOfCharacter(party2.LeaderHero);
				CampaignEventDispatcher.Instance.OnPlayerStartTalkFromMenu(party2.LeaderHero);
				PlayerEncounter.LocationEncounter.CreateAndOpenMissionController(locationOfCharacter, null, party2.LeaderHero.CharacterObject, null);
				return;
			}
			ConversationCharacterData conversationCharacterData = new ConversationCharacterData(CharacterObject.PlayerCharacter, mainParty1, false, false, false, false, false, false);
			ConversationCharacterData conversationCharacterData2 = new ConversationCharacterData(ConversationHelper.GetConversationCharacterPartyLeader(party2), party2, false, false, false, false, false, false);
			if (PartyBase.MainParty.MobileParty.IsCurrentlyAtSea)
			{
				CampaignMission.OpenConversationMission(conversationCharacterData, conversationCharacterData2, "", "", false);
				return;
			}
			CampaignMapConversation.OpenConversation(conversationCharacterData, conversationCharacterData2);
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x0004AC88 File Offset: 0x00048E88
		public virtual void Refresh()
		{
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x0004AC8A File Offset: 0x00048E8A
		public virtual void UpdateOverlayType(GameMenu.MenuOverlayType newType)
		{
			this.Refresh();
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x0004AC92 File Offset: 0x00048E92
		public virtual void OnFrameTick(float dt)
		{
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x0004AC94 File Offset: 0x00048E94
		public void HourlyTick()
		{
			this.Refresh();
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06001264 RID: 4708 RVA: 0x0004AC9C File Offset: 0x00048E9C
		// (set) Token: 0x06001265 RID: 4709 RVA: 0x0004ACA4 File Offset: 0x00048EA4
		[DataSourceProperty]
		public bool IsContextMenuEnabled
		{
			get
			{
				return this._isContextMenuEnabled;
			}
			set
			{
				this._isContextMenuEnabled = value;
				base.OnPropertyChangedWithValue(value, "IsContextMenuEnabled");
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001266 RID: 4710 RVA: 0x0004ACB9 File Offset: 0x00048EB9
		// (set) Token: 0x06001267 RID: 4711 RVA: 0x0004ACC1 File Offset: 0x00048EC1
		[DataSourceProperty]
		public bool IsInitializationOver
		{
			get
			{
				return this._isInitializationOver;
			}
			set
			{
				this._isInitializationOver = value;
				base.OnPropertyChangedWithValue(value, "IsInitializationOver");
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06001268 RID: 4712 RVA: 0x0004ACD6 File Offset: 0x00048ED6
		// (set) Token: 0x06001269 RID: 4713 RVA: 0x0004ACDE File Offset: 0x00048EDE
		[DataSourceProperty]
		public bool IsInfoBarExtended
		{
			get
			{
				return this._isInfoBarExtended;
			}
			set
			{
				this._isInfoBarExtended = value;
				base.OnPropertyChangedWithValue(value, "IsInfoBarExtended");
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x0600126A RID: 4714 RVA: 0x0004ACF3 File Offset: 0x00048EF3
		// (set) Token: 0x0600126B RID: 4715 RVA: 0x0004ACFB File Offset: 0x00048EFB
		[DataSourceProperty]
		public MBBindingList<StringItemWithEnabledAndHintVM> ContextList
		{
			get
			{
				return this._contextList;
			}
			set
			{
				if (value != this._contextList)
				{
					this._contextList = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringItemWithEnabledAndHintVM>>(value, "ContextList");
				}
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x0600126C RID: 4716 RVA: 0x0004AD19 File Offset: 0x00048F19
		// (set) Token: 0x0600126D RID: 4717 RVA: 0x0004AD21 File Offset: 0x00048F21
		[DataSourceProperty]
		public int CurrentOverlayType
		{
			get
			{
				return this._currentOverlayType;
			}
			set
			{
				if (value != this._currentOverlayType)
				{
					this._currentOverlayType = value;
					base.OnPropertyChangedWithValue(value, "CurrentOverlayType");
				}
			}
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x0004AD3F File Offset: 0x00048F3F
		public void SetExitInputKey(HotKey hotKey)
		{
			this.ExitInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x0600126F RID: 4719 RVA: 0x0004AD4E File Offset: 0x00048F4E
		// (set) Token: 0x06001270 RID: 4720 RVA: 0x0004AD56 File Offset: 0x00048F56
		[DataSourceProperty]
		public InputKeyItemVM ExitInputKey
		{
			get
			{
				return this._exitInputKey;
			}
			set
			{
				if (value != this._exitInputKey)
				{
					this._exitInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ExitInputKey");
				}
			}
		}

		// Token: 0x04000857 RID: 2135
		public string GameMenuOverlayName;

		// Token: 0x04000858 RID: 2136
		private bool _closedHandled = true;

		// Token: 0x04000859 RID: 2137
		private bool _isContextMenuEnabled;

		// Token: 0x0400085A RID: 2138
		private int _currentOverlayType = -1;

		// Token: 0x0400085B RID: 2139
		private bool _isInfoBarExtended;

		// Token: 0x0400085C RID: 2140
		private bool _isInitializationOver;

		// Token: 0x0400085D RID: 2141
		private MBBindingList<StringItemWithEnabledAndHintVM> _contextList;

		// Token: 0x0400085E RID: 2142
		protected GameMenuPartyItemVM _contextMenuItem;

		// Token: 0x0400085F RID: 2143
		private InputKeyItemVM _exitInputKey;

		// Token: 0x02000238 RID: 568
		protected internal enum MenuOverlayContextList
		{
			// Token: 0x0400126B RID: 4715
			Encyclopedia,
			// Token: 0x0400126C RID: 4716
			Conversation,
			// Token: 0x0400126D RID: 4717
			QuickConversation,
			// Token: 0x0400126E RID: 4718
			ConverseWithLeader,
			// Token: 0x0400126F RID: 4719
			ArmyDismiss,
			// Token: 0x04001270 RID: 4720
			ManageGarrison,
			// Token: 0x04001271 RID: 4721
			DonateTroops,
			// Token: 0x04001272 RID: 4722
			JoinArmy,
			// Token: 0x04001273 RID: 4723
			TakeToParty,
			// Token: 0x04001274 RID: 4724
			ManageTroops
		}
	}
}
