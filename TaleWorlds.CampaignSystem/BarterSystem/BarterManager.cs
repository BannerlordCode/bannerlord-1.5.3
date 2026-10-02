using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.BarterSystem
{
	// Token: 0x020004A0 RID: 1184
	public class BarterManager
	{
		// Token: 0x17000ECB RID: 3787
		// (get) Token: 0x06004BBD RID: 19389 RVA: 0x001814A6 File Offset: 0x0017F6A6
		public static BarterManager Instance
		{
			get
			{
				return Campaign.Current.BarterManager;
			}
		}

		// Token: 0x17000ECC RID: 3788
		// (get) Token: 0x06004BBE RID: 19390 RVA: 0x001814B2 File Offset: 0x0017F6B2
		// (set) Token: 0x06004BBF RID: 19391 RVA: 0x001814BA File Offset: 0x0017F6BA
		[SaveableProperty(1)]
		public bool LastBarterIsAccepted { get; internal set; }

		// Token: 0x06004BC0 RID: 19392 RVA: 0x001814C3 File Offset: 0x0017F6C3
		public BarterManager()
		{
			this._barteredHeroes = new Dictionary<Hero, CampaignTime>();
		}

		// Token: 0x06004BC1 RID: 19393 RVA: 0x001814D6 File Offset: 0x0017F6D6
		public void BeginPlayerBarter(BarterData args)
		{
			if (this.BarterBegin != null)
			{
				this.BarterBegin(args);
			}
			ICampaignMission campaignMission = CampaignMission.Current;
			if (campaignMission == null)
			{
				return;
			}
			campaignMission.SetMissionMode(MissionMode.Barter, false);
		}

		// Token: 0x06004BC2 RID: 19394 RVA: 0x00181500 File Offset: 0x0017F700
		private void AddBaseBarterables(BarterData args, IEnumerable<Barterable> defaultBarterables)
		{
			if (defaultBarterables != null)
			{
				bool flag = false;
				foreach (Barterable barterable in defaultBarterables)
				{
					if (!flag)
					{
						args.AddBarterGroup(new DefaultsBarterGroup());
						flag = true;
					}
					barterable.SetIsOffered(true);
					args.AddBarterable<OtherBarterGroup>(barterable, true);
					barterable.SetIsOffered(true);
				}
			}
		}

		// Token: 0x06004BC3 RID: 19395 RVA: 0x0018156C File Offset: 0x0017F76C
		public void StartBarterOffer(Hero offerer, Hero other, PartyBase offererParty, PartyBase otherParty, Hero beneficiaryOfOtherHero = null, BarterManager.BarterContextInitializer InitContext = null, int persuasionCostReduction = 0, bool isAIBarter = false, IEnumerable<Barterable> defaultBarterables = null)
		{
			this.LastBarterIsAccepted = false;
			if (offerer == Hero.MainHero && other != null && InitContext == null)
			{
				if (!this.CanPlayerBarterWithHero(other))
				{
					Debug.FailedAssert("Barter with the hero is on cooldown.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\BarterSystem\\BarterManager.cs", "StartBarterOffer", 83);
					return;
				}
				this.ClearHeroCooldowns();
			}
			BarterData barterData = new BarterData(offerer, beneficiaryOfOtherHero ?? other, offererParty, otherParty, InitContext, persuasionCostReduction, false);
			this.AddBaseBarterables(barterData, defaultBarterables);
			CampaignEventDispatcher.Instance.OnBarterablesRequested(barterData);
			Campaign.Current.ConversationManager.CurrentConversationIsFirst = false;
			if (!isAIBarter)
			{
				Campaign.Current.BarterManager.BeginPlayerBarter(barterData);
			}
		}

		// Token: 0x06004BC4 RID: 19396 RVA: 0x00181604 File Offset: 0x0017F804
		public void ExecuteAiBarter(IFaction faction1, IFaction faction2, Hero faction1Hero, Hero faction2Hero, Barterable barterable)
		{
			this.ExecuteAiBarter(faction1, faction2, faction1Hero, faction2Hero, new Barterable[] { barterable });
		}

		// Token: 0x06004BC5 RID: 19397 RVA: 0x00181628 File Offset: 0x0017F828
		public void ExecuteAiBarter(IFaction faction1, IFaction faction2, Hero faction1Hero, Hero faction2Hero, IEnumerable<Barterable> baseBarterables)
		{
			BarterData barterData = new BarterData(faction1.Leader, faction2.Leader, null, null, null, 0, true);
			barterData.AddBarterGroup(new DefaultsBarterGroup());
			foreach (Barterable barterable in baseBarterables)
			{
				barterable.SetIsOffered(true);
				barterData.AddBarterable<DefaultsBarterGroup>(barterable, true);
			}
			CampaignEventDispatcher.Instance.OnBarterablesRequested(barterData);
			Campaign.Current.BarterManager.ExecuteAIBarter(barterData, faction1, faction2, faction1Hero, faction2Hero);
		}

		// Token: 0x06004BC6 RID: 19398 RVA: 0x001816BC File Offset: 0x0017F8BC
		public void ExecuteAIBarter(BarterData barterData, IFaction faction1, IFaction faction2, Hero faction1Hero, Hero faction2Hero)
		{
			this.MakeBalanced(barterData, faction1, faction2, faction2Hero, 1f);
			this.MakeBalanced(barterData, faction2, faction1, faction1Hero, 1f);
			float offerValueForFaction = this.GetOfferValueForFaction(barterData, faction1);
			float offerValueForFaction2 = this.GetOfferValueForFaction(barterData, faction2);
			if (offerValueForFaction >= 0f && offerValueForFaction2 >= 0f)
			{
				this.ApplyBarterOffer(barterData.OffererHero, barterData.OtherHero, barterData.GetOfferedBarterables());
			}
		}

		// Token: 0x06004BC7 RID: 19399 RVA: 0x00181724 File Offset: 0x0017F924
		private void MakeBalanced(BarterData args, IFaction faction1, IFaction faction2, Hero faction2Hero, float fulfillRatio)
		{
			foreach (ValueTuple<Barterable, int> valueTuple in BarterHelper.GetAutoBalanceBarterablesAdd(args, faction1, faction2, faction2Hero, fulfillRatio))
			{
				Barterable item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				if (!item.IsOffered)
				{
					item.SetIsOffered(true);
					item.CurrentAmount = 0;
				}
				item.CurrentAmount += item2;
			}
		}

		// Token: 0x06004BC8 RID: 19400 RVA: 0x001817A0 File Offset: 0x0017F9A0
		public void Close()
		{
			if (CampaignMission.Current != null)
			{
				CampaignMission.Current.SetMissionMode(MissionMode.Conversation, false);
			}
			if (this.Closed != null)
			{
				this.Closed();
			}
		}

		// Token: 0x06004BC9 RID: 19401 RVA: 0x001817C8 File Offset: 0x0017F9C8
		public bool IsOfferAcceptable(BarterData args, Hero hero, PartyBase party)
		{
			return this.GetOfferValue(hero, party, args.OffererParty, args.GetOfferedBarterables()) > -0.01f;
		}

		// Token: 0x06004BCA RID: 19402 RVA: 0x001817E8 File Offset: 0x0017F9E8
		public float GetOfferValueForFaction(BarterData barterData, IFaction faction)
		{
			int num = 0;
			foreach (Barterable barterable in barterData.GetOfferedBarterables())
			{
				num += barterable.GetValueForFaction(faction);
			}
			return (float)num;
		}

		// Token: 0x06004BCB RID: 19403 RVA: 0x00181844 File Offset: 0x0017FA44
		public float GetOfferValue(Hero selfHero, PartyBase selfParty, PartyBase offererParty, IEnumerable<Barterable> offeredBarters)
		{
			float num = 0f;
			IFaction faction;
			if (((selfHero != null) ? selfHero.Clan : null) != null)
			{
				IFaction clan = selfHero.Clan;
				faction = clan;
			}
			else
			{
				faction = selfParty.MapFaction;
			}
			IFaction faction2 = faction;
			foreach (Barterable barterable in offeredBarters)
			{
				num += (float)barterable.GetValueForFaction(faction2);
			}
			this._overpayAmount = ((num > 0f) ? num : 0f);
			return num;
		}

		// Token: 0x06004BCC RID: 19404 RVA: 0x001818D0 File Offset: 0x0017FAD0
		public void ApplyAndFinalizePlayerBarter(Hero offererHero, Hero otherHero, BarterData barterData)
		{
			this.LastBarterIsAccepted = true;
			this.ApplyBarterOffer(offererHero, otherHero, barterData.GetOfferedBarterables());
			if (otherHero != null)
			{
				this.HandleHeroCooldown(otherHero);
			}
		}

		// Token: 0x06004BCD RID: 19405 RVA: 0x001818F1 File Offset: 0x0017FAF1
		public void CancelAndFinalizePlayerBarter(Hero offererHero, Hero otherHero, BarterData barterData)
		{
			this.CancelBarter(offererHero, otherHero, barterData.GetOfferedBarterables());
		}

		// Token: 0x06004BCE RID: 19406 RVA: 0x00181904 File Offset: 0x0017FB04
		private void ApplyBarterOffer(Hero offererHero, Hero otherHero, List<Barterable> barters)
		{
			foreach (Barterable barterable in barters)
			{
				barterable.Apply();
			}
			CampaignEventDispatcher.Instance.OnBarterAccepted(offererHero, otherHero, barters);
			if (offererHero == Hero.MainHero)
			{
				if (this._overpayAmount > 0f && otherHero != null)
				{
					this.ApplyOverpayBonus(otherHero);
				}
				this.Close();
				if (Campaign.Current.ConversationManager.IsConversationInProgress)
				{
					Campaign.Current.ConversationManager.ContinueConversation();
				}
				MBInformationManager.AddQuickInformation(GameTexts.FindText("str_offer_accepted", null), 0, null, null, "");
			}
		}

		// Token: 0x06004BCF RID: 19407 RVA: 0x001819BC File Offset: 0x0017FBBC
		private void CancelBarter(Hero offererHero, Hero otherHero, List<Barterable> offeredBarters)
		{
			this.Close();
			MBInformationManager.AddQuickInformation(GameTexts.FindText("str_offer_rejected", null), 0, null, null, "");
			CampaignEventDispatcher.Instance.OnBarterCanceled(offererHero, otherHero, offeredBarters);
			Campaign.Current.ConversationManager.ContinueConversation();
		}

		// Token: 0x06004BD0 RID: 19408 RVA: 0x001819F8 File Offset: 0x0017FBF8
		private void ApplyOverpayBonus(Hero otherHero)
		{
			if (otherHero.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
			{
				return;
			}
			int num = Campaign.Current.Models.BarterModel.CalculateOverpayRelationIncreaseCosts(otherHero, this._overpayAmount);
			if (num > 0)
			{
				ChangeRelationAction.ApplyRelationChangeBetweenHeroes(Hero.MainHero, otherHero, num, true);
			}
		}

		// Token: 0x06004BD1 RID: 19409 RVA: 0x00181A4C File Offset: 0x0017FC4C
		public bool CanPlayerBarterWithHero(Hero hero)
		{
			CampaignTime campaignTime;
			return !this._barteredHeroes.TryGetValue(hero, out campaignTime) || campaignTime.IsPast;
		}

		// Token: 0x06004BD2 RID: 19410 RVA: 0x00181A74 File Offset: 0x0017FC74
		private void HandleHeroCooldown(Hero hero)
		{
			CampaignTime campaignTime = CampaignTime.Now + CampaignTime.Days((float)Campaign.Current.Models.BarterModel.BarterCooldownWithHeroInDays);
			if (!this._barteredHeroes.ContainsKey(hero))
			{
				this._barteredHeroes.Add(hero, campaignTime);
				return;
			}
			this._barteredHeroes[hero] = campaignTime;
		}

		// Token: 0x06004BD3 RID: 19411 RVA: 0x00181AD0 File Offset: 0x0017FCD0
		private void ClearHeroCooldowns()
		{
			foreach (KeyValuePair<Hero, CampaignTime> keyValuePair in new Dictionary<Hero, CampaignTime>(this._barteredHeroes))
			{
				if (keyValuePair.Value.IsPast)
				{
					this._barteredHeroes.Remove(keyValuePair.Key);
				}
			}
		}

		// Token: 0x06004BD4 RID: 19412 RVA: 0x00181B48 File Offset: 0x0017FD48
		public bool InitializeMarriageBarterContext(Barterable barterable, BarterData args, object obj)
		{
			Hero hero = null;
			Hero hero2 = null;
			if (obj != null)
			{
				Tuple<Hero, Hero> tuple = obj as Tuple<Hero, Hero>;
				if (tuple != null)
				{
					hero = tuple.Item1;
					hero2 = tuple.Item2;
				}
			}
			MarriageBarterable marriageBarterable = barterable as MarriageBarterable;
			return marriageBarterable != null && hero != null && hero2 != null && marriageBarterable.ProposingHero == hero2 && marriageBarterable.HeroBeingProposedTo == hero;
		}

		// Token: 0x06004BD5 RID: 19413 RVA: 0x00181B98 File Offset: 0x0017FD98
		public bool InitializeJoinFactionBarterContext(Barterable barterable, BarterData args, object obj)
		{
			return barterable.GetType() == typeof(JoinKingdomAsClanBarterable) && barterable.OriginalOwner == Hero.OneToOneConversationHero;
		}

		// Token: 0x06004BD6 RID: 19414 RVA: 0x00181BC0 File Offset: 0x0017FDC0
		public bool InitializeMakePeaceBarterContext(Barterable barterable, BarterData args, object obj)
		{
			return barterable.GetType() == typeof(PeaceBarterable) && barterable.OriginalOwner == args.OtherHero;
		}

		// Token: 0x06004BD7 RID: 19415 RVA: 0x00181BE9 File Offset: 0x0017FDE9
		public bool InitializeSafePassageBarterContext(Barterable barterable, BarterData args, object obj)
		{
			if (barterable.GetType() == typeof(SafePassageBarterable))
			{
				PartyBase originalParty = barterable.OriginalParty;
				MobileParty conversationParty = MobileParty.ConversationParty;
				return originalParty == ((conversationParty != null) ? conversationParty.Party : null);
			}
			return false;
		}

		// Token: 0x06004BD8 RID: 19416 RVA: 0x00181C1D File Offset: 0x0017FE1D
		internal static void AutoGeneratedStaticCollectObjectsBarterManager(object o, List<object> collectedObjects)
		{
			((BarterManager)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06004BD9 RID: 19417 RVA: 0x00181C2B File Offset: 0x0017FE2B
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			collectedObjects.Add(this._barteredHeroes);
		}

		// Token: 0x06004BDA RID: 19418 RVA: 0x00181C39 File Offset: 0x0017FE39
		internal static object AutoGeneratedGetMemberValueLastBarterIsAccepted(object o)
		{
			return ((BarterManager)o).LastBarterIsAccepted;
		}

		// Token: 0x06004BDB RID: 19419 RVA: 0x00181C4B File Offset: 0x0017FE4B
		internal static object AutoGeneratedGetMemberValue_barteredHeroes(object o)
		{
			return ((BarterManager)o)._barteredHeroes;
		}

		// Token: 0x04001519 RID: 5401
		public BarterManager.BarterCloseEventDelegate Closed;

		// Token: 0x0400151A RID: 5402
		public BarterManager.BarterBeginEventDelegate BarterBegin;

		// Token: 0x0400151B RID: 5403
		[SaveableField(2)]
		private readonly Dictionary<Hero, CampaignTime> _barteredHeroes;

		// Token: 0x0400151C RID: 5404
		private float _overpayAmount;

		// Token: 0x020008D0 RID: 2256
		// (Invoke) Token: 0x06006C82 RID: 27778
		public delegate bool BarterContextInitializer(Barterable barterable, BarterData args, object obj = null);

		// Token: 0x020008D1 RID: 2257
		// (Invoke) Token: 0x06006C86 RID: 27782
		public delegate void BarterCloseEventDelegate();

		// Token: 0x020008D2 RID: 2258
		// (Invoke) Token: 0x06006C8A RID: 27786
		public delegate void BarterBeginEventDelegate(BarterData args);
	}
}
