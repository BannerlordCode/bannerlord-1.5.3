using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003EA RID: 1002
	public class AllianceCampaignBehavior : CampaignBehaviorBase, IAllianceCampaignBehavior
	{
		// Token: 0x06003C72 RID: 15474 RVA: 0x000F6EF4 File Offset: 0x000F50F4
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickClanEvent.AddNonSerializedListener(this, new Action<Clan>(this.DailyTickClan));
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
			CampaignEvents.MakePeace.AddNonSerializedListener(this, new Action<IFaction, IFaction, MakePeaceAction.MakePeaceDetail>(this.OnMakePeace));
			CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnKingdomDestroyed));
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
		}

		// Token: 0x06003C73 RID: 15475 RVA: 0x000F6FA2 File Offset: 0x000F51A2
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<List<AllianceCampaignBehavior.Alliance>>("_alliances", ref this._alliances);
			dataStore.SyncData<List<AllianceCampaignBehavior.CallToWarAgreement>>("_callToWarAgreements", ref this._callToWarAgreements);
		}

		// Token: 0x06003C74 RID: 15476 RVA: 0x000F6FC8 File Offset: 0x000F51C8
		public void OnAllianceOfferedToPlayer(Kingdom offeringKingdom)
		{
			if (Clan.PlayerClan.Kingdom.Clans.Count == 1)
			{
				object obj = new TextObject("{=ho5EndaV}Decision", null);
				TextObject textObject = new TextObject("{=eAhgrwkZ}As {RULER_NAME_AND_TITLE}, you must decide if an alliance will be formed with the {KINGDOM_NAME}.", null);
				TextObject textObject2 = GameTexts.FindText("str_faction_ruler_name_with_title", Hero.MainHero.MapFaction.Culture.StringId);
				textObject2.SetCharacterProperties("RULER", Hero.MainHero.CharacterObject, false);
				textObject.SetTextVariable("RULER_NAME_AND_TITLE", textObject2);
				textObject.SetTextVariable("KINGDOM_NAME", offeringKingdom.Name);
				TextObject textObject3 = new TextObject("{=Y94H6XnK}Accept", null);
				TextObject textObject4 = new TextObject("{=cOgmdp9e}Decline", null);
				InformationManager.ShowInquiry(new InquiryData(obj.ToString(), textObject.ToString(), true, true, textObject3.ToString(), textObject4.ToString(), delegate
				{
					this.AcceptStartingAlliance(offeringKingdom);
				}, null, "", 0f, null, null, null), false, false);
				return;
			}
			object obj2 = new TextObject("{=ho5EndaV}Decision", null);
			TextObject textObject5 = new TextObject("{=eTylgLCc}A courier has arrived from the {KINGDOM_NAME}. They offer you an alliance. Your kingdom will vote whether to accept the offer.", null);
			textObject5.SetTextVariable("KINGDOM_NAME", offeringKingdom.Name);
			TextObject textObject6 = new TextObject("{=oHaWR73d}Ok", null);
			InformationManager.ShowInquiry(new InquiryData(obj2.ToString(), textObject5.ToString(), true, false, textObject6.ToString(), textObject6.ToString(), delegate
			{
				this.ConfirmAllianceOffer(offeringKingdom);
			}, null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06003C75 RID: 15477 RVA: 0x000F714C File Offset: 0x000F534C
		public void OnAllianceOfferedToPlayerKingdom(Kingdom offeringKingdom)
		{
			if (Clan.PlayerClan.Kingdom.Clans.Count == 1)
			{
				TextObject textObject = new TextObject("{=1V8f9vRM}A courier bearing an alliance offer from the {PROPOSER_KINGDOM} has arrived at the court of your realm.", null);
				textObject.SetTextVariable("PROPOSER_KINGDOM", offeringKingdom.InformalName);
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new AllianceOfferMapNotification(offeringKingdom, textObject));
				return;
			}
			this.AddAllianceDecision(Clan.PlayerClan.Kingdom, offeringKingdom);
		}

		// Token: 0x06003C76 RID: 15478 RVA: 0x000F71B8 File Offset: 0x000F53B8
		public void OnCallToWarAgreementProposedToPlayer(Kingdom proposerKingdom, Kingdom kingdomToCallToWarAgainst)
		{
			if (Clan.PlayerClan.Kingdom.Clans.Count == 1)
			{
				int callToWarCost = Campaign.Current.Models.AllianceModel.GetCallToWarCost(proposerKingdom, Clan.PlayerClan.Kingdom, kingdomToCallToWarAgainst);
				object obj = new TextObject("{=ho5EndaV}Decision", null);
				TextObject textObject = new TextObject("{=L81DPSom}As {RULER_NAME_AND_TITLE}, you must decide if your realm will answer the call of the {CALLING_KINGDOM} and declare war on the {KINGDOM_TO_CALL_TO_WAR_AGAINST} for {CALL_TO_WAR_COST}{GOLD_ICON}.", null);
				TextObject textObject2 = GameTexts.FindText("str_faction_ruler_name_with_title", Hero.MainHero.MapFaction.Culture.StringId);
				textObject2.SetCharacterProperties("RULER", Hero.MainHero.CharacterObject, false);
				textObject.SetTextVariable("RULER_NAME_AND_TITLE", textObject2);
				textObject.SetTextVariable("CALLING_KINGDOM", proposerKingdom.Name);
				textObject.SetTextVariable("KINGDOM_TO_CALL_TO_WAR_AGAINST", kingdomToCallToWarAgainst.Name);
				textObject.SetTextVariable("CALL_TO_WAR_COST", callToWarCost);
				textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
				TextObject textObject3 = new TextObject("{=Y94H6XnK}Accept", null);
				TextObject textObject4 = new TextObject("{=cOgmdp9e}Decline", null);
				InformationManager.ShowInquiry(new InquiryData(obj.ToString(), textObject.ToString(), true, true, textObject3.ToString(), textObject4.ToString(), delegate
				{
					this.StartCallToWarAgreement(proposerKingdom, Clan.PlayerClan.Kingdom, kingdomToCallToWarAgainst, callToWarCost, false);
				}, delegate
				{
					this.DenyCallToWarAgreement(proposerKingdom, Clan.PlayerClan.Kingdom);
				}, "", 0f, null, null, null), false, false);
				return;
			}
			object obj2 = new TextObject("{=ho5EndaV}Decision", null);
			TextObject textObject5 = new TextObject("{=FuNbouTu}A courier has arrived from the {KINGDOM_NAME}. They call your kingdom to war against {KINGDOM_TO_CALL_TO_WAR_AGAINST}. Your kingdom will vote whether to accept the offer.", null);
			textObject5.SetTextVariable("KINGDOM_NAME", proposerKingdom.Name);
			textObject5.SetTextVariable("KINGDOM_TO_CALL_TO_WAR_AGAINST", kingdomToCallToWarAgainst.Name);
			TextObject textObject6 = new TextObject("{=oHaWR73d}Ok", null);
			InformationManager.ShowInquiry(new InquiryData(obj2.ToString(), textObject5.ToString(), true, false, textObject6.ToString(), textObject6.ToString(), delegate
			{
				this.ConfirmCallToWarAgreementOffer(proposerKingdom, kingdomToCallToWarAgainst);
			}, null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06003C77 RID: 15479 RVA: 0x000F73F8 File Offset: 0x000F55F8
		public void OnCallToWarAgreementProposedToPlayerKingdom(Kingdom proposerKingdom, Kingdom kingdomToCallToWarAgainst)
		{
			if (Clan.PlayerClan.Kingdom.Clans.Count == 1)
			{
				TextObject textObject = new TextObject("{=PneX4Ayw}A courier bearing a call to war offer from the {KINGDOM_NAME} against {KINGDOM_TO_CALL_TO_WAR_AGAINST} has arrived at the court of your realm.", null);
				textObject.SetTextVariable("KINGDOM_NAME", proposerKingdom.Name);
				textObject.SetTextVariable("KINGDOM_TO_CALL_TO_WAR_AGAINST", kingdomToCallToWarAgainst.Name);
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new AcceptCallToWarOfferMapNotification(proposerKingdom, kingdomToCallToWarAgainst, textObject));
				return;
			}
			KingdomDecision kingdomDecision = Clan.PlayerClan.Kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>(delegate(KingdomDecision s)
			{
				AcceptCallToWarAgreementDecision acceptCallToWarAgreementDecision2;
				return (acceptCallToWarAgreementDecision2 = s as AcceptCallToWarAgreementDecision) != null && acceptCallToWarAgreementDecision2.CallingKingdom == proposerKingdom && acceptCallToWarAgreementDecision2.KingdomToCallToWarAgainst == kingdomToCallToWarAgainst;
			});
			if (kingdomDecision != null)
			{
				Clan.PlayerClan.Kingdom.RemoveDecision(kingdomDecision);
			}
			AcceptCallToWarAgreementDecision acceptCallToWarAgreementDecision = new AcceptCallToWarAgreementDecision(Clan.PlayerClan, proposerKingdom, kingdomToCallToWarAgainst);
			Clan.PlayerClan.Kingdom.AddDecision(acceptCallToWarAgreementDecision, true);
		}

		// Token: 0x06003C78 RID: 15480 RVA: 0x000F74E8 File Offset: 0x000F56E8
		public void OnCallToWarAgreementProposedByPlayer(Kingdom proposedKingdom, Kingdom kingdomToCallToWarAgainst)
		{
			if (Clan.PlayerClan.Kingdom.Clans.Count == 1)
			{
				int callToWarCost = Campaign.Current.Models.AllianceModel.GetCallToWarCost(Clan.PlayerClan.Kingdom, proposedKingdom, kingdomToCallToWarAgainst);
				if (callToWarCost <= Clan.PlayerClan.Gold)
				{
					object obj = new TextObject("{=ho5EndaV}Decision", null);
					TextObject textObject = new TextObject("{=AwCnrOan}As {RULER_NAME_AND_TITLE}, you must decide if the {CALLED_KINGDOM} will be called to war against the {KINGDOM_TO_CALL_TO_WAR_AGAINST} for {CALL_TO_WAR_COST}{GOLD_ICON}.", null);
					TextObject textObject2 = GameTexts.FindText("str_faction_ruler_name_with_title", Hero.MainHero.MapFaction.Culture.StringId);
					textObject2.SetCharacterProperties("RULER", Hero.MainHero.CharacterObject, false);
					textObject.SetTextVariable("RULER_NAME_AND_TITLE", textObject2);
					textObject.SetTextVariable("CALLED_KINGDOM", proposedKingdom.Name);
					textObject.SetTextVariable("KINGDOM_TO_CALL_TO_WAR_AGAINST", kingdomToCallToWarAgainst.Name);
					textObject.SetTextVariable("CALL_TO_WAR_COST", callToWarCost);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					TextObject textObject3 = new TextObject("{=Y94H6XnK}Accept", null);
					TextObject textObject4 = new TextObject("{=cOgmdp9e}Decline", null);
					InformationManager.ShowInquiry(new InquiryData(obj.ToString(), textObject.ToString(), true, true, textObject3.ToString(), textObject4.ToString(), delegate
					{
						this.StartCallToWarAgreement(Clan.PlayerClan.Kingdom, proposedKingdom, kingdomToCallToWarAgainst, callToWarCost, false);
					}, null, "", 0f, null, null, null), false, false);
					return;
				}
			}
			else
			{
				object obj2 = new TextObject("{=ho5EndaV}Decision", null);
				TextObject textObject5 = new TextObject("{=qgN9o2ip}It is time to call our ally the {KINGDOM_NAME} to war againts {KINGDOM_TO_CALL_TO_WAR_AGAINST}!. Your kingdom will vote whether to propose a call to war agreement to them.", null);
				textObject5.SetTextVariable("KINGDOM_NAME", proposedKingdom.Name);
				textObject5.SetTextVariable("KINGDOM_TO_CALL_TO_WAR_AGAINST", kingdomToCallToWarAgainst.Name);
				TextObject textObject6 = new TextObject("{=oHaWR73d}Ok", null);
				InformationManager.ShowInquiry(new InquiryData(obj2.ToString(), textObject5.ToString(), true, false, textObject6.ToString(), textObject6.ToString(), delegate
				{
					this.ConfirmCallToWarAgreementProposalOffer(proposedKingdom, kingdomToCallToWarAgainst);
				}, null, "", 0f, null, null, null), false, false);
			}
		}

		// Token: 0x06003C79 RID: 15481 RVA: 0x000F7730 File Offset: 0x000F5930
		public CampaignTime GetAllianceEndDate(Kingdom kingdom1, Kingdom kingdom2)
		{
			AllianceCampaignBehavior.Alliance alliance;
			if (!this.TryGetAlliance(kingdom1, kingdom2, out alliance))
			{
				Debug.FailedAssert("Cant find alliance", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\AllianceCampaignBehavior.cs", "GetAllianceEndDate", 277);
				return CampaignTime.Zero;
			}
			return alliance.EndTime;
		}

		// Token: 0x06003C7A RID: 15482 RVA: 0x000F7770 File Offset: 0x000F5970
		public void OnCallToWarAgreementProposedByPlayerKingdom(Kingdom proposedKingdom, Kingdom kingdomToCallToWarAgainst)
		{
			if (Clan.PlayerClan.Kingdom.Clans.Count == 1)
			{
				TextObject textObject = new TextObject("{=dDsJyerw}Call the {CALLED_KINGDOM} to War Against the {KINGDOM_TO_CALL_TO_WAR_AGAINST}.", null);
				textObject.SetTextVariable("CALLED_KINGDOM", proposedKingdom.Name);
				textObject.SetTextVariable("KINGDOM_TO_CALL_TO_WAR_AGAINST", kingdomToCallToWarAgainst.Name);
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new ProposeCallToWarOfferMapNotification(proposedKingdom, kingdomToCallToWarAgainst, textObject));
				return;
			}
			KingdomDecision kingdomDecision = Clan.PlayerClan.Kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>(delegate(KingdomDecision s)
			{
				ProposeCallToWarAgreementDecision proposeCallToWarAgreementDecision2;
				return (proposeCallToWarAgreementDecision2 = s as ProposeCallToWarAgreementDecision) != null && proposeCallToWarAgreementDecision2.CalledKingdom == proposedKingdom && proposeCallToWarAgreementDecision2.KingdomToCallToWarAgainst == kingdomToCallToWarAgainst;
			});
			if (kingdomDecision != null)
			{
				Clan.PlayerClan.Kingdom.RemoveDecision(kingdomDecision);
			}
			ProposeCallToWarAgreementDecision proposeCallToWarAgreementDecision = new ProposeCallToWarAgreementDecision(Clan.PlayerClan, proposedKingdom, kingdomToCallToWarAgainst);
			Clan.PlayerClan.Kingdom.AddDecision(proposeCallToWarAgreementDecision, true);
		}

		// Token: 0x06003C7B RID: 15483 RVA: 0x000F7860 File Offset: 0x000F5A60
		public bool IsAllyWithKingdom(Kingdom kingdom1, Kingdom kingdom2)
		{
			AllianceCampaignBehavior.Alliance alliance;
			return kingdom1 != null && kingdom2 != null && kingdom1 != kingdom2 && !kingdom1.IsEliminated && !kingdom2.IsEliminated && this.TryGetAlliance(kingdom1, kingdom2, out alliance);
		}

		// Token: 0x06003C7C RID: 15484 RVA: 0x000F7894 File Offset: 0x000F5A94
		public void StartAlliance(Kingdom proposerKingdom, Kingdom receiverKingdom)
		{
			if (!this.IsAllyWithKingdom(proposerKingdom, receiverKingdom))
			{
				StanceLink stanceWith = proposerKingdom.GetStanceWith(receiverKingdom);
				if (stanceWith.GetDailyTributeToPay(proposerKingdom) != 0)
				{
					stanceWith.SetDailyTributePaid(proposerKingdom, 0, 0);
				}
				if (stanceWith.GetDailyTributeToPay(proposerKingdom) != 0)
				{
					stanceWith.SetDailyTributePaid(proposerKingdom, 0, 0);
				}
				this.AddAlliance(proposerKingdom, receiverKingdom);
				CampaignEventDispatcher.Instance.OnAllianceStarted(proposerKingdom, receiverKingdom);
				foreach (IFaction faction in proposerKingdom.FactionsAtWarWith.WhereQ<IFaction>((IFaction f) => f.IsKingdomFaction && !f.IsAtWarWith(receiverKingdom)).ToList<IFaction>())
				{
					if (proposerKingdom == Clan.PlayerClan.Kingdom && !Hero.MainHero.Clan.IsUnderMercenaryService)
					{
						this.OnCallToWarAgreementProposedByPlayerKingdom(receiverKingdom, (Kingdom)faction);
					}
					else
					{
						ProposeCallToWarAgreementDecision proposeCallToWarAgreementDecision = new ProposeCallToWarAgreementDecision(proposerKingdom.RulingClan, receiverKingdom, (Kingdom)faction);
						proposerKingdom.AddDecision(proposeCallToWarAgreementDecision, true);
					}
				}
				foreach (IFaction faction2 in receiverKingdom.FactionsAtWarWith.WhereQ<IFaction>((IFaction f) => f.IsKingdomFaction && !f.IsAtWarWith(proposerKingdom)).ToList<IFaction>())
				{
					if (receiverKingdom == Clan.PlayerClan.Kingdom && !Hero.MainHero.Clan.IsUnderMercenaryService)
					{
						this.OnCallToWarAgreementProposedByPlayerKingdom(proposerKingdom, (Kingdom)faction2);
					}
					else
					{
						ProposeCallToWarAgreementDecision proposeCallToWarAgreementDecision2 = new ProposeCallToWarAgreementDecision(receiverKingdom.RulingClan, proposerKingdom, (Kingdom)faction2);
						receiverKingdom.AddDecision(proposeCallToWarAgreementDecision2, true);
					}
				}
			}
		}

		// Token: 0x06003C7D RID: 15485 RVA: 0x000F7AB8 File Offset: 0x000F5CB8
		public void EndAlliance(Kingdom kingdom1, Kingdom kingdom2)
		{
			foreach (AllianceCampaignBehavior.CallToWarAgreement callToWarAgreement in this.GetCallToWarAgreements(kingdom1, kingdom2))
			{
				this.EndCallToWarAgreement(callToWarAgreement.CallingKingdom, callToWarAgreement.CalledKingdom, callToWarAgreement.KingdomToCallToWarAgainst);
			}
			this.RemoveAlliance(kingdom1, kingdom2);
			CampaignEventDispatcher.Instance.OnAllianceEnded(kingdom1, kingdom2);
		}

		// Token: 0x06003C7E RID: 15486 RVA: 0x000F7B34 File Offset: 0x000F5D34
		public bool HasCalledToWar(Kingdom callingKingdom, Kingdom calledKingdom)
		{
			return callingKingdom != null && calledKingdom != null && callingKingdom != calledKingdom && !callingKingdom.IsEliminated && !calledKingdom.IsEliminated && calledKingdom.IsAllyWith(callingKingdom) && this._callToWarAgreements.AnyQ<AllianceCampaignBehavior.CallToWarAgreement>((AllianceCampaignBehavior.CallToWarAgreement c) => c.CallingKingdom == callingKingdom && c.CalledKingdom == calledKingdom);
		}

		// Token: 0x06003C7F RID: 15487 RVA: 0x000F7BBC File Offset: 0x000F5DBC
		public bool IsAtWarByCallToWarAgreement(Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst, out Kingdom callingKingdom)
		{
			callingKingdom = null;
			if (kingdomToCallToWarAgainst == null || calledKingdom == null || kingdomToCallToWarAgainst == calledKingdom || kingdomToCallToWarAgainst.IsEliminated || calledKingdom.IsEliminated)
			{
				return false;
			}
			for (int i = 0; i < this._callToWarAgreements.Count; i++)
			{
				AllianceCampaignBehavior.CallToWarAgreement callToWarAgreement = this._callToWarAgreements[i];
				if (callToWarAgreement.CalledKingdom == calledKingdom && callToWarAgreement.KingdomToCallToWarAgainst == kingdomToCallToWarAgainst)
				{
					callingKingdom = callToWarAgreement.CallingKingdom;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003C80 RID: 15488 RVA: 0x000F7C28 File Offset: 0x000F5E28
		public void StartCallToWarAgreement(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst, int callToWarCost, bool isPlayerPaying = false)
		{
			if (this.IsAllyWithKingdom(callingKingdom, calledKingdom) && !calledKingdom.IsAtWarWith(kingdomToCallToWarAgainst))
			{
				AllianceCampaignBehavior.CallToWarAgreement callToWarAgreement = this.AddCallToWarAgreement(callingKingdom, calledKingdom, kingdomToCallToWarAgainst);
				this.UpdateAllianceEndTime(callingKingdom, calledKingdom, callToWarAgreement.EndTime);
				if (isPlayerPaying)
				{
					Hero.MainHero.ChangeHeroGold(-callToWarCost);
					calledKingdom.CallToWarWallet += callToWarCost;
				}
				else
				{
					callingKingdom.CallToWarWallet -= callToWarCost;
					calledKingdom.CallToWarWallet += callToWarCost;
				}
				CampaignEventDispatcher.Instance.OnCallToWarAgreementStarted(callingKingdom, calledKingdom, kingdomToCallToWarAgainst);
				this.ApplyAcceptingCallToWarOfferBonus(callingKingdom, calledKingdom);
				DeclareWarAction.ApplyByCallToWarAgreement(calledKingdom, kingdomToCallToWarAgainst);
			}
		}

		// Token: 0x06003C81 RID: 15489 RVA: 0x000F7CBC File Offset: 0x000F5EBC
		public void EndCallToWarAgreement(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst)
		{
			this.RemoveCallToWarAgreement(callingKingdom, calledKingdom, kingdomToCallToWarAgainst);
			CampaignEventDispatcher.Instance.OnCallToWarAgreementEnded(callingKingdom, calledKingdom, kingdomToCallToWarAgainst);
		}

		// Token: 0x06003C82 RID: 15490 RVA: 0x000F7CD4 File Offset: 0x000F5ED4
		public void DenyCallToWarAgreement(Kingdom callingKingdom, Kingdom calledKingdom)
		{
			this.ApplyDenyingCallToWarOfferPenalty(callingKingdom, calledKingdom);
		}

		// Token: 0x06003C83 RID: 15491 RVA: 0x000F7CE0 File Offset: 0x000F5EE0
		public List<Kingdom> GetKingdomsToCallToWarAgainst(Kingdom callingKingdom, Kingdom calledKingdom)
		{
			if (callingKingdom != calledKingdom)
			{
				return this._callToWarAgreements.WhereQ<AllianceCampaignBehavior.CallToWarAgreement>((AllianceCampaignBehavior.CallToWarAgreement c) => c.CallingKingdom == callingKingdom && c.CalledKingdom == calledKingdom).SelectQ<AllianceCampaignBehavior.CallToWarAgreement, Kingdom>((AllianceCampaignBehavior.CallToWarAgreement x) => x.KingdomToCallToWarAgainst).ToListQ<Kingdom>();
			}
			return new List<Kingdom>();
		}

		// Token: 0x06003C84 RID: 15492 RVA: 0x000F7D58 File Offset: 0x000F5F58
		private AllianceCampaignBehavior.Alliance AddAlliance(Kingdom kingdom1, Kingdom kingdom2)
		{
			AllianceCampaignBehavior.Alliance alliance = new AllianceCampaignBehavior.Alliance(kingdom1, kingdom2, CampaignTime.Now + Campaign.Current.Models.AllianceModel.MaxDurationOfAlliance);
			this._alliances.Add(alliance);
			kingdom1.UpdateAlliedKingdoms();
			kingdom2.UpdateAlliedKingdoms();
			return alliance;
		}

		// Token: 0x06003C85 RID: 15493 RVA: 0x000F7DA8 File Offset: 0x000F5FA8
		private void RemoveAlliance(Kingdom kingdom1, Kingdom kingdom2)
		{
			int num = this._alliances.Count - 1;
			while (-1 < num)
			{
				AllianceCampaignBehavior.Alliance alliance = this._alliances[num];
				if ((alliance.Kingdom1 == kingdom1 && alliance.Kingdom2 == kingdom2) || (alliance.Kingdom2 == kingdom1 && alliance.Kingdom1 == kingdom2))
				{
					this._alliances.RemoveAt(num);
					break;
				}
				num--;
			}
			kingdom1.UpdateAlliedKingdoms();
			kingdom2.UpdateAlliedKingdoms();
		}

		// Token: 0x06003C86 RID: 15494 RVA: 0x000F7E18 File Offset: 0x000F6018
		private AllianceCampaignBehavior.CallToWarAgreement AddCallToWarAgreement(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst)
		{
			AllianceCampaignBehavior.CallToWarAgreement callToWarAgreement = new AllianceCampaignBehavior.CallToWarAgreement(callingKingdom, calledKingdom, kingdomToCallToWarAgainst, CampaignTime.Now + Campaign.Current.Models.AllianceModel.MaxDurationOfWarParticipation);
			this._callToWarAgreements.Add(callToWarAgreement);
			return callToWarAgreement;
		}

		// Token: 0x06003C87 RID: 15495 RVA: 0x000F7E5C File Offset: 0x000F605C
		private void RemoveCallToWarAgreement(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst)
		{
			int num = this._callToWarAgreements.Count - 1;
			while (-1 < num)
			{
				AllianceCampaignBehavior.CallToWarAgreement callToWarAgreement = this._callToWarAgreements[num];
				if (callToWarAgreement.CallingKingdom == callingKingdom && callToWarAgreement.CalledKingdom == calledKingdom && callToWarAgreement.KingdomToCallToWarAgainst == kingdomToCallToWarAgainst)
				{
					this._callToWarAgreements.RemoveAt(num);
					return;
				}
				num--;
			}
		}

		// Token: 0x06003C88 RID: 15496 RVA: 0x000F7EB8 File Offset: 0x000F60B8
		private List<AllianceCampaignBehavior.CallToWarAgreement> GetCallToWarAgreements(Kingdom kingdom1, Kingdom kingdom2)
		{
			if (kingdom1 != kingdom2)
			{
				return this._callToWarAgreements.Where<AllianceCampaignBehavior.CallToWarAgreement>((AllianceCampaignBehavior.CallToWarAgreement c) => (c.CallingKingdom == kingdom1 && c.CalledKingdom == kingdom2) || (c.CallingKingdom == kingdom2 && c.CalledKingdom == kingdom1)).ToListQ<AllianceCampaignBehavior.CallToWarAgreement>();
			}
			return new List<AllianceCampaignBehavior.CallToWarAgreement>();
		}

		// Token: 0x06003C89 RID: 15497 RVA: 0x000F7F0C File Offset: 0x000F610C
		private bool TryGetAlliance(Kingdom kingdom1, Kingdom kingdom2, out AllianceCampaignBehavior.Alliance foundAlliance)
		{
			bool flag = false;
			foundAlliance = default(AllianceCampaignBehavior.Alliance);
			foreach (AllianceCampaignBehavior.Alliance alliance in this._alliances)
			{
				if ((alliance.Kingdom1 == kingdom1 && alliance.Kingdom2 == kingdom2) || (alliance.Kingdom2 == kingdom1 && alliance.Kingdom1 == kingdom2))
				{
					foundAlliance = alliance;
					flag = true;
					break;
				}
			}
			return flag;
		}

		// Token: 0x06003C8A RID: 15498 RVA: 0x000F7F94 File Offset: 0x000F6194
		private void UpdateAllianceEndTime(Kingdom kingdom1, Kingdom kingdom2, CampaignTime newEndTime)
		{
			for (int i = 0; i < this._alliances.Count; i++)
			{
				AllianceCampaignBehavior.Alliance alliance = this._alliances[i];
				if (((alliance.Kingdom1 == kingdom1 && alliance.Kingdom2 == kingdom2) || (alliance.Kingdom2 == kingdom1 && alliance.Kingdom1 == kingdom2)) && alliance.EndTime < newEndTime)
				{
					this._alliances[i] = new AllianceCampaignBehavior.Alliance(this._alliances[i].Kingdom1, this._alliances[i].Kingdom2, newEndTime);
					return;
				}
			}
		}

		// Token: 0x06003C8B RID: 15499 RVA: 0x000F802E File Offset: 0x000F622E
		private void AcceptStartingAlliance(Kingdom proposerKingdom)
		{
			this.StartAlliance(proposerKingdom, Clan.PlayerClan.Kingdom);
		}

		// Token: 0x06003C8C RID: 15500 RVA: 0x000F8041 File Offset: 0x000F6241
		private void ConfirmAllianceOffer(Kingdom proposerKingdom)
		{
			if (Clan.PlayerClan.IsUnderMercenaryService)
			{
				this.AcceptStartingAlliance(proposerKingdom);
				return;
			}
			this.AddAllianceDecision(Clan.PlayerClan.Kingdom, proposerKingdom);
		}

		// Token: 0x06003C8D RID: 15501 RVA: 0x000F8068 File Offset: 0x000F6268
		private void ConfirmCallToWarAgreementProposalOffer(Kingdom proposedKingdom, Kingdom kingdomToCallToWarAgainst)
		{
			ProposeCallToWarAgreementDecision proposeCallToWarAgreementDecision = new ProposeCallToWarAgreementDecision(Clan.PlayerClan, proposedKingdom, kingdomToCallToWarAgainst);
			int callToWarCost = proposeCallToWarAgreementDecision.CallToWarCost;
			if (Clan.PlayerClan.IsUnderMercenaryService)
			{
				this.StartCallToWarAgreement(Clan.PlayerClan.Kingdom, proposedKingdom, kingdomToCallToWarAgainst, callToWarCost, false);
				return;
			}
			KingdomDecision kingdomDecision = Clan.PlayerClan.Kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>(delegate(KingdomDecision s)
			{
				ProposeCallToWarAgreementDecision proposeCallToWarAgreementDecision2;
				return (proposeCallToWarAgreementDecision2 = s as ProposeCallToWarAgreementDecision) != null && proposeCallToWarAgreementDecision2.CalledKingdom == proposedKingdom && proposeCallToWarAgreementDecision2.KingdomToCallToWarAgainst == kingdomToCallToWarAgainst;
			});
			if (kingdomDecision != null)
			{
				Clan.PlayerClan.Kingdom.RemoveDecision(kingdomDecision);
			}
			Clan.PlayerClan.Kingdom.AddDecision(proposeCallToWarAgreementDecision, true);
		}

		// Token: 0x06003C8E RID: 15502 RVA: 0x000F8118 File Offset: 0x000F6318
		private void ConfirmCallToWarAgreementOffer(Kingdom proposerKingdom, Kingdom kingdomToCallToWarAgainst)
		{
			AcceptCallToWarAgreementDecision acceptCallToWarAgreementDecision = new AcceptCallToWarAgreementDecision(Clan.PlayerClan, proposerKingdom, kingdomToCallToWarAgainst);
			int callToWarCost = acceptCallToWarAgreementDecision.CallToWarCost;
			if (Clan.PlayerClan.IsUnderMercenaryService)
			{
				this.StartCallToWarAgreement(proposerKingdom, Clan.PlayerClan.Kingdom, kingdomToCallToWarAgainst, callToWarCost, false);
				return;
			}
			KingdomDecision kingdomDecision = Clan.PlayerClan.Kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>(delegate(KingdomDecision s)
			{
				AcceptCallToWarAgreementDecision acceptCallToWarAgreementDecision2;
				return (acceptCallToWarAgreementDecision2 = s as AcceptCallToWarAgreementDecision) != null && acceptCallToWarAgreementDecision2.CallingKingdom == proposerKingdom && acceptCallToWarAgreementDecision2.KingdomToCallToWarAgainst == kingdomToCallToWarAgainst;
			});
			if (kingdomDecision != null)
			{
				Clan.PlayerClan.Kingdom.RemoveDecision(kingdomDecision);
			}
			Clan.PlayerClan.Kingdom.AddDecision(acceptCallToWarAgreementDecision, true);
		}

		// Token: 0x06003C8F RID: 15503 RVA: 0x000F81C7 File Offset: 0x000F63C7
		private void ApplyBrokenAlliancePenalty(Kingdom kingdom, Kingdom otherKingdom, DeclareWarAction.DeclareWarDetail detail)
		{
			Hero hero = ((detail == DeclareWarAction.DeclareWarDetail.CausedByPlayerHostility) ? Hero.MainHero : kingdom.Leader);
			ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero, otherKingdom.Leader, -100, true);
			if (hero == Hero.MainHero)
			{
				TraitLevelingHelper.OnAllianceBrokenThroughHostility();
			}
		}

		// Token: 0x06003C90 RID: 15504 RVA: 0x000F81F5 File Offset: 0x000F63F5
		private void ApplyDenyingCallToWarOfferPenalty(Kingdom callingKingdom, Kingdom calledKingdom)
		{
			ChangeRelationAction.ApplyRelationChangeBetweenHeroes(calledKingdom.Leader, callingKingdom.Leader, -50, true);
		}

		// Token: 0x06003C91 RID: 15505 RVA: 0x000F820B File Offset: 0x000F640B
		private void ApplyAcceptingCallToWarOfferBonus(Kingdom callingKingdom, Kingdom calledKingdom)
		{
			ChangeRelationAction.ApplyRelationChangeBetweenHeroes(calledKingdom.Leader, callingKingdom.Leader, 10, true);
		}

		// Token: 0x06003C92 RID: 15506 RVA: 0x000F8224 File Offset: 0x000F6424
		private void AddAllianceDecision(Kingdom kingdomToAddDecision, Kingdom kingdomToOffer)
		{
			KingdomDecision kingdomDecision = kingdomToAddDecision.UnresolvedDecisions.FirstOrDefault<KingdomDecision>(delegate(KingdomDecision s)
			{
				StartAllianceDecision startAllianceDecision2;
				return (startAllianceDecision2 = s as StartAllianceDecision) != null && startAllianceDecision2.KingdomToStartAllianceWith == kingdomToOffer;
			});
			if (kingdomDecision != null)
			{
				kingdomToAddDecision.RemoveDecision(kingdomDecision);
			}
			StartAllianceDecision startAllianceDecision = new StartAllianceDecision(Campaign.Current.Models.AllianceModel.GetProposerClanForAllianceDecision(kingdomToAddDecision, kingdomToOffer), kingdomToOffer);
			TextObject textObject;
			if (startAllianceDecision.CanMakeDecision(out textObject, false))
			{
				kingdomToAddDecision.AddDecision(startAllianceDecision, true);
			}
		}

		// Token: 0x06003C93 RID: 15507 RVA: 0x000F829C File Offset: 0x000F649C
		private static void RefreshAlliedKingdoms()
		{
			foreach (Kingdom kingdom in Kingdom.All)
			{
				kingdom.UpdateAlliedKingdoms();
			}
		}

		// Token: 0x06003C94 RID: 15508 RVA: 0x000F82EC File Offset: 0x000F64EC
		private void DailyTickClan(Clan clan)
		{
			if (!clan.IsEliminated)
			{
				clan.Aggressiveness -= 1f;
				if (clan.Kingdom != null && clan.Kingdom.RulingClan == clan)
				{
					Kingdom kingdom = clan.Kingdom;
					if (!kingdom.AlliedKingdoms.IsEmpty<Kingdom>())
					{
						for (int i = kingdom.AlliedKingdoms.Count - 1; i > -1; i--)
						{
							Kingdom kingdom2 = kingdom.AlliedKingdoms[i];
							AllianceCampaignBehavior.Alliance alliance;
							if (this.TryGetAlliance(kingdom2, kingdom, out alliance))
							{
								List<AllianceCampaignBehavior.CallToWarAgreement> callToWarAgreements = this.GetCallToWarAgreements(kingdom, kingdom2);
								for (int j = callToWarAgreements.Count - 1; j > -1; j--)
								{
									AllianceCampaignBehavior.CallToWarAgreement callToWarAgreement = callToWarAgreements[j];
									if (callToWarAgreement.EndTime.IsPast)
									{
										this.EndCallToWarAgreement(callToWarAgreement.CallingKingdom, callToWarAgreement.CalledKingdom, callToWarAgreement.KingdomToCallToWarAgainst);
									}
								}
								if (alliance.EndTime.IsPast)
								{
									this.EndAlliance(kingdom, kingdom2);
									if (kingdom == Clan.PlayerClan.Kingdom)
									{
										this.AddAllianceDecision(kingdom, kingdom2);
									}
									else
									{
										this.AddAllianceDecision(kingdom2, kingdom);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003C95 RID: 15509 RVA: 0x000F8410 File Offset: 0x000F6610
		private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
		{
			if (faction1.IsKingdomFaction && faction2.IsKingdomFaction)
			{
				Kingdom kingdom = (Kingdom)faction1;
				Kingdom kingdom2 = (Kingdom)faction2;
				if (kingdom.IsAllyWith(kingdom2))
				{
					this.ApplyBrokenAlliancePenalty(kingdom, kingdom2, detail);
					this.EndAlliance(kingdom, kingdom2);
				}
				foreach (Kingdom kingdom3 in kingdom.AlliedKingdoms.ToList<Kingdom>())
				{
					if (!kingdom3.IsAtWarWith(kingdom2))
					{
						if (kingdom == Clan.PlayerClan.Kingdom && !Hero.MainHero.Clan.IsUnderMercenaryService)
						{
							this.OnCallToWarAgreementProposedByPlayerKingdom(kingdom3, kingdom2);
						}
						else
						{
							ProposeCallToWarAgreementDecision proposeCallToWarAgreementDecision = new ProposeCallToWarAgreementDecision(kingdom.RulingClan, kingdom3, kingdom2);
							kingdom.AddDecision(proposeCallToWarAgreementDecision, true);
						}
					}
				}
				foreach (Kingdom kingdom4 in kingdom2.AlliedKingdoms.ToList<Kingdom>())
				{
					if (!kingdom4.IsAtWarWith(kingdom))
					{
						if (kingdom2 == Clan.PlayerClan.Kingdom && !Hero.MainHero.Clan.IsUnderMercenaryService)
						{
							this.OnCallToWarAgreementProposedByPlayerKingdom(kingdom4, kingdom);
						}
						else
						{
							ProposeCallToWarAgreementDecision proposeCallToWarAgreementDecision2 = new ProposeCallToWarAgreementDecision(kingdom2.RulingClan, kingdom4, kingdom);
							kingdom2.AddDecision(proposeCallToWarAgreementDecision2, true);
						}
					}
				}
			}
		}

		// Token: 0x06003C96 RID: 15510 RVA: 0x000F8578 File Offset: 0x000F6778
		private void OnMakePeace(IFaction faction1, IFaction faction2, MakePeaceAction.MakePeaceDetail detail)
		{
			if (faction1.IsKingdomFaction && faction2.IsKingdomFaction)
			{
				Kingdom kingdom1 = (Kingdom)faction1;
				Kingdom kingdom2 = (Kingdom)faction2;
				using (List<Kingdom>.Enumerator enumerator = kingdom1.AlliedKingdoms.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Kingdom ally2 = enumerator.Current;
						if (this._callToWarAgreements.AnyQ<AllianceCampaignBehavior.CallToWarAgreement>((AllianceCampaignBehavior.CallToWarAgreement c) => c.CallingKingdom == kingdom1 && c.CalledKingdom == ally2 && c.KingdomToCallToWarAgainst == kingdom2))
						{
							this.EndCallToWarAgreement(kingdom1, ally2, kingdom2);
						}
					}
				}
				using (List<Kingdom>.Enumerator enumerator = kingdom2.AlliedKingdoms.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Kingdom ally = enumerator.Current;
						if (this._callToWarAgreements.AnyQ<AllianceCampaignBehavior.CallToWarAgreement>((AllianceCampaignBehavior.CallToWarAgreement c) => c.CallingKingdom == kingdom2 && c.CalledKingdom == ally && c.KingdomToCallToWarAgainst == kingdom1))
						{
							this.EndCallToWarAgreement(kingdom2, ally, kingdom1);
						}
					}
				}
			}
		}

		// Token: 0x06003C97 RID: 15511 RVA: 0x000F86D8 File Offset: 0x000F68D8
		private void OnKingdomDestroyed(Kingdom kingdom)
		{
			IEnumerable<AllianceCampaignBehavior.Alliance> alliances = this._alliances;
			Func<AllianceCampaignBehavior.Alliance, bool> <>9__0;
			Func<AllianceCampaignBehavior.Alliance, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = (AllianceCampaignBehavior.Alliance a) => a.Kingdom1 == kingdom || a.Kingdom2 == kingdom);
			}
			foreach (AllianceCampaignBehavior.Alliance alliance in alliances.Where<AllianceCampaignBehavior.Alliance>(func).ToList<AllianceCampaignBehavior.Alliance>())
			{
				this.EndAlliance(alliance.Kingdom1, alliance.Kingdom2);
			}
		}

		// Token: 0x06003C98 RID: 15512 RVA: 0x000F876C File Offset: 0x000F696C
		private void OnGameLoadFinished()
		{
			if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion.IsOlderThan(ApplicationVersion.FromString("v1.4.0.110693", 0)))
			{
				for (int i = 0; i < this._callToWarAgreements.Count; i++)
				{
					AllianceCampaignBehavior.CallToWarAgreement callToWarAgreement = this._callToWarAgreements[i];
					this.UpdateAllianceEndTime(callToWarAgreement.CallingKingdom, callToWarAgreement.CalledKingdom, callToWarAgreement.EndTime);
				}
			}
		}

		// Token: 0x06003C99 RID: 15513 RVA: 0x000F87D5 File Offset: 0x000F69D5
		private void OnNewGameCreated(CampaignGameStarter starter)
		{
			AllianceCampaignBehavior.RefreshAlliedKingdoms();
		}

		// Token: 0x06003C9A RID: 15514 RVA: 0x000F87DC File Offset: 0x000F69DC
		private void OnGameLoaded(CampaignGameStarter starter)
		{
			AllianceCampaignBehavior.RefreshAlliedKingdoms();
		}

		// Token: 0x040012A1 RID: 4769
		private const int BreakingAllianceRelationPenalty = -100;

		// Token: 0x040012A2 RID: 4770
		private const int DenyingCallToWarRelationPenalty = -50;

		// Token: 0x040012A3 RID: 4771
		private const int AcceptingCallToWarRelationBonus = 10;

		// Token: 0x040012A4 RID: 4772
		private List<AllianceCampaignBehavior.Alliance> _alliances = new List<AllianceCampaignBehavior.Alliance>();

		// Token: 0x040012A5 RID: 4773
		private List<AllianceCampaignBehavior.CallToWarAgreement> _callToWarAgreements = new List<AllianceCampaignBehavior.CallToWarAgreement>();

		// Token: 0x020007DE RID: 2014
		public class AllianceCampaignBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x06006646 RID: 26182 RVA: 0x001D04D8 File Offset: 0x001CE6D8
			public AllianceCampaignBehaviorTypeDefiner()
				: base(312270)
			{
			}

			// Token: 0x06006647 RID: 26183 RVA: 0x001D04E5 File Offset: 0x001CE6E5
			protected override void DefineStructTypes()
			{
				base.AddStructDefinition(typeof(AllianceCampaignBehavior.Alliance), 1, null);
				base.AddStructDefinition(typeof(AllianceCampaignBehavior.CallToWarAgreement), 2, null);
			}

			// Token: 0x06006648 RID: 26184 RVA: 0x001D050B File Offset: 0x001CE70B
			protected override void DefineContainerDefinitions()
			{
				base.ConstructContainerDefinition(typeof(List<AllianceCampaignBehavior.Alliance>));
				base.ConstructContainerDefinition(typeof(List<AllianceCampaignBehavior.CallToWarAgreement>));
			}
		}

		// Token: 0x020007DF RID: 2015
		internal struct Alliance
		{
			// Token: 0x06006649 RID: 26185 RVA: 0x001D052D File Offset: 0x001CE72D
			public Alliance(Kingdom kingdom1, Kingdom kingdom2, CampaignTime endTime)
			{
				this.Kingdom1 = kingdom1;
				this.Kingdom2 = kingdom2;
				this.EndTime = endTime;
			}

			// Token: 0x0600664A RID: 26186 RVA: 0x001D0544 File Offset: 0x001CE744
			public static void AutoGeneratedStaticCollectObjectsAlliance(object o, List<object> collectedObjects)
			{
				((AllianceCampaignBehavior.Alliance)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x0600664B RID: 26187 RVA: 0x001D0560 File Offset: 0x001CE760
			private void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this.Kingdom1);
				collectedObjects.Add(this.Kingdom2);
				CampaignTime.AutoGeneratedStaticCollectObjectsCampaignTime(this.EndTime, collectedObjects);
			}

			// Token: 0x0600664C RID: 26188 RVA: 0x001D058B File Offset: 0x001CE78B
			internal static object AutoGeneratedGetMemberValueKingdom1(object o)
			{
				return ((AllianceCampaignBehavior.Alliance)o).Kingdom1;
			}

			// Token: 0x0600664D RID: 26189 RVA: 0x001D0598 File Offset: 0x001CE798
			internal static object AutoGeneratedGetMemberValueKingdom2(object o)
			{
				return ((AllianceCampaignBehavior.Alliance)o).Kingdom2;
			}

			// Token: 0x0600664E RID: 26190 RVA: 0x001D05A5 File Offset: 0x001CE7A5
			internal static object AutoGeneratedGetMemberValueEndTime(object o)
			{
				return ((AllianceCampaignBehavior.Alliance)o).EndTime;
			}

			// Token: 0x04002061 RID: 8289
			[SaveableField(0)]
			public readonly Kingdom Kingdom1;

			// Token: 0x04002062 RID: 8290
			[SaveableField(1)]
			public readonly Kingdom Kingdom2;

			// Token: 0x04002063 RID: 8291
			[SaveableField(2)]
			public CampaignTime EndTime;
		}

		// Token: 0x020007E0 RID: 2016
		internal struct CallToWarAgreement
		{
			// Token: 0x0600664F RID: 26191 RVA: 0x001D05B7 File Offset: 0x001CE7B7
			public CallToWarAgreement(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst, CampaignTime endTime)
			{
				this.CallingKingdom = callingKingdom;
				this.CalledKingdom = calledKingdom;
				this.KingdomToCallToWarAgainst = kingdomToCallToWarAgainst;
				this.EndTime = endTime;
			}

			// Token: 0x06006650 RID: 26192 RVA: 0x001D05D8 File Offset: 0x001CE7D8
			public static void AutoGeneratedStaticCollectObjectsCallToWarAgreement(object o, List<object> collectedObjects)
			{
				((AllianceCampaignBehavior.CallToWarAgreement)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06006651 RID: 26193 RVA: 0x001D05F4 File Offset: 0x001CE7F4
			private void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this.CallingKingdom);
				collectedObjects.Add(this.CalledKingdom);
				collectedObjects.Add(this.KingdomToCallToWarAgainst);
				CampaignTime.AutoGeneratedStaticCollectObjectsCampaignTime(this.EndTime, collectedObjects);
			}

			// Token: 0x06006652 RID: 26194 RVA: 0x001D062B File Offset: 0x001CE82B
			internal static object AutoGeneratedGetMemberValueCallingKingdom(object o)
			{
				return ((AllianceCampaignBehavior.CallToWarAgreement)o).CallingKingdom;
			}

			// Token: 0x06006653 RID: 26195 RVA: 0x001D0638 File Offset: 0x001CE838
			internal static object AutoGeneratedGetMemberValueCalledKingdom(object o)
			{
				return ((AllianceCampaignBehavior.CallToWarAgreement)o).CalledKingdom;
			}

			// Token: 0x06006654 RID: 26196 RVA: 0x001D0645 File Offset: 0x001CE845
			internal static object AutoGeneratedGetMemberValueKingdomToCallToWarAgainst(object o)
			{
				return ((AllianceCampaignBehavior.CallToWarAgreement)o).KingdomToCallToWarAgainst;
			}

			// Token: 0x06006655 RID: 26197 RVA: 0x001D0652 File Offset: 0x001CE852
			internal static object AutoGeneratedGetMemberValueEndTime(object o)
			{
				return ((AllianceCampaignBehavior.CallToWarAgreement)o).EndTime;
			}

			// Token: 0x04002064 RID: 8292
			[SaveableField(0)]
			public readonly Kingdom CallingKingdom;

			// Token: 0x04002065 RID: 8293
			[SaveableField(1)]
			public readonly Kingdom CalledKingdom;

			// Token: 0x04002066 RID: 8294
			[SaveableField(2)]
			public readonly Kingdom KingdomToCallToWarAgainst;

			// Token: 0x04002067 RID: 8295
			[SaveableField(3)]
			public CampaignTime EndTime;
		}
	}
}
