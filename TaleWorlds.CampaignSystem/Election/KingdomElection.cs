using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Election
{
	// Token: 0x020002E2 RID: 738
	public class KingdomElection
	{
		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x060027E6 RID: 10214 RVA: 0x000A6056 File Offset: 0x000A4256
		public MBReadOnlyList<DecisionOutcome> PossibleOutcomes
		{
			get
			{
				return this._possibleOutcomes;
			}
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x060027E7 RID: 10215 RVA: 0x000A605E File Offset: 0x000A425E
		// (set) Token: 0x060027E8 RID: 10216 RVA: 0x000A6066 File Offset: 0x000A4266
		[SaveableProperty(7)]
		public bool IsCancelled { get; private set; }

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x060027E9 RID: 10217 RVA: 0x000A606F File Offset: 0x000A426F
		public bool IsPlayerSupporter
		{
			get
			{
				return this.PlayerAsSupporter != null;
			}
		}

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x060027EA RID: 10218 RVA: 0x000A607A File Offset: 0x000A427A
		private Supporter PlayerAsSupporter
		{
			get
			{
				return this._supporters.FirstOrDefault<Supporter>((Supporter x) => x.IsPlayer);
			}
		}

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x060027EB RID: 10219 RVA: 0x000A60A6 File Offset: 0x000A42A6
		public bool IsPlayerChooser
		{
			get
			{
				return this._chooser.Leader.IsHumanPlayerCharacter;
			}
		}

		// Token: 0x060027EC RID: 10220 RVA: 0x000A60B8 File Offset: 0x000A42B8
		public KingdomElection(KingdomDecision decision)
		{
			this._decision = decision;
			this.Setup();
		}

		// Token: 0x060027ED RID: 10221 RVA: 0x000A60D0 File Offset: 0x000A42D0
		private void Setup()
		{
			MBList<DecisionOutcome> mblist = this._decision.DetermineInitialCandidates().ToMBList<DecisionOutcome>();
			this._possibleOutcomes = this._decision.NarrowDownCandidates(mblist, 3);
			this._supporters = this._decision.DetermineSupporters().ToList<Supporter>();
			this._chooser = this._decision.DetermineChooser();
			this._decision.DetermineSponsors(this._possibleOutcomes);
			this._hasPlayerVoted = false;
			this.IsCancelled = false;
			foreach (DecisionOutcome decisionOutcome in this._possibleOutcomes)
			{
				decisionOutcome.InitialSupport = this.DetermineInitialSupport(decisionOutcome);
			}
			float num = this._possibleOutcomes.Sum<DecisionOutcome>((DecisionOutcome x) => x.InitialSupport);
			foreach (DecisionOutcome decisionOutcome2 in this._possibleOutcomes)
			{
				decisionOutcome2.Likelihood = ((num == 0f) ? 0f : (decisionOutcome2.InitialSupport / num));
			}
		}

		// Token: 0x060027EE RID: 10222 RVA: 0x000A621C File Offset: 0x000A441C
		public void StartElection()
		{
			this.Setup();
			this.DetermineSupport(this._possibleOutcomes, false);
			this._decision.DetermineSponsors(this._possibleOutcomes);
			this.UpdateSupport(this._possibleOutcomes);
			if (this._decision.ShouldBeCancelled())
			{
				Debug.Print("SELIM_DEBUG - " + this._decision.GetSupportTitle() + " has been cancelled", 0, Debug.DebugColor.White, 17592186044416UL);
				this.IsCancelled = true;
				bool flag;
				if (!this._decision.DetermineChooser().Leader.IsHumanPlayerCharacter)
				{
					flag = this._decision.DetermineSupporters().Any<Supporter>((Supporter x) => x.IsPlayer);
				}
				else
				{
					flag = true;
				}
				bool flag2 = flag;
				CampaignEventDispatcher.Instance.OnKingdomDecisionCancelled(this._decision, flag2);
				return;
			}
			if (!this.IsPlayerSupporter || this._ignorePlayerSupport)
			{
				this.ReadyToAiChoose();
				return;
			}
			if (this._decision.IsSingleClanDecision())
			{
				this._chosenOutcome = this._possibleOutcomes.FirstOrDefault<DecisionOutcome>((DecisionOutcome t) => t.SponsorClan != null && t.SponsorClan == Clan.PlayerClan);
				Supporter supporter = new Supporter(Clan.PlayerClan);
				supporter.SupportWeight = Supporter.SupportWeights.FullyPush;
				this._chosenOutcome.AddSupport(supporter);
			}
		}

		// Token: 0x060027EF RID: 10223 RVA: 0x000A6369 File Offset: 0x000A4569
		public static KingdomElection.ElectionOutcomeSupport GetElectionOutcomeSupport(KingdomDecision decision, Clan sponsor)
		{
			KingdomElection kingdomElection = new KingdomElection(decision);
			kingdomElection.SetupResultWithoutPlayerSupport();
			return kingdomElection.GetDecisionOutcomeSupportForSponsor(sponsor);
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x000A637D File Offset: 0x000A457D
		public void SetupResultWithoutPlayerSupport()
		{
			this.DetermineSupport(this._possibleOutcomes, false);
			this._decision.DetermineSponsors(this._possibleOutcomes);
			this.UpdateSupport(this._possibleOutcomes);
			this.DetermineOfficialSupport();
		}

		// Token: 0x060027F1 RID: 10225 RVA: 0x000A63B0 File Offset: 0x000A45B0
		private float DetermineInitialSupport(DecisionOutcome possibleOutcome)
		{
			float num = 0f;
			foreach (Supporter supporter in this._supporters)
			{
				if (!supporter.IsPlayer)
				{
					num += MathF.Clamp(this._decision.DetermineSupport(supporter.Clan, possibleOutcome), 0f, 100f);
				}
			}
			return num;
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x000A6430 File Offset: 0x000A4630
		public void StartElectionWithoutPlayer()
		{
			this._ignorePlayerSupport = true;
			this.StartElection();
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x000A6440 File Offset: 0x000A4640
		public float GetLikelihoodForSponsor(Clan sponsor)
		{
			foreach (DecisionOutcome decisionOutcome in this._possibleOutcomes)
			{
				if (decisionOutcome.SponsorClan == sponsor)
				{
					return decisionOutcome.Likelihood;
				}
			}
			Debug.FailedAssert("This clan is not a sponsor of any of the outcomes.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Election\\KingdomDecisionMaker.cs", "GetLikelihoodForSponsor", 174);
			return -1f;
		}

		// Token: 0x060027F4 RID: 10228 RVA: 0x000A64C0 File Offset: 0x000A46C0
		public float GetWinChanceForSponsor(Clan sponsor)
		{
			foreach (DecisionOutcome decisionOutcome in this._possibleOutcomes)
			{
				if (decisionOutcome.SponsorClan == sponsor)
				{
					return decisionOutcome.WinChance;
				}
			}
			Debug.FailedAssert("This clan is not a sponsor of any of the outcomes.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Election\\KingdomDecisionMaker.cs", "GetWinChanceForSponsor", 189);
			return -1f;
		}

		// Token: 0x060027F5 RID: 10229 RVA: 0x000A6540 File Offset: 0x000A4740
		public KingdomElection.ElectionOutcomeSupport GetDecisionOutcomeSupportForSponsor(Clan sponsor)
		{
			float winChanceForSponsor = this.GetWinChanceForSponsor(sponsor);
			if (winChanceForSponsor > 0.75f)
			{
				return KingdomElection.ElectionOutcomeSupport.StrongSupport;
			}
			if (winChanceForSponsor > 0.5f)
			{
				return KingdomElection.ElectionOutcomeSupport.GoodSupport;
			}
			if (winChanceForSponsor > 0.2f)
			{
				return KingdomElection.ElectionOutcomeSupport.SlightSupport;
			}
			return KingdomElection.ElectionOutcomeSupport.LowSupport;
		}

		// Token: 0x060027F6 RID: 10230 RVA: 0x000A6574 File Offset: 0x000A4774
		private void DetermineSupport(MBReadOnlyList<DecisionOutcome> possibleOutcomes, bool calculateRelationshipEffect)
		{
			foreach (Supporter supporter in this._supporters)
			{
				if (!supporter.IsPlayer)
				{
					Supporter.SupportWeights supportWeights = Supporter.SupportWeights.StayNeutral;
					DecisionOutcome decisionOutcome = this._decision.DetermineSupportOption(supporter, possibleOutcomes, out supportWeights, calculateRelationshipEffect);
					if (decisionOutcome != null)
					{
						supporter.SupportWeight = supportWeights;
						decisionOutcome.AddSupport(supporter);
					}
				}
			}
		}

		// Token: 0x060027F7 RID: 10231 RVA: 0x000A65EC File Offset: 0x000A47EC
		private void UpdateSupport(MBReadOnlyList<DecisionOutcome> possibleOutcomes)
		{
			foreach (DecisionOutcome decisionOutcome in this._possibleOutcomes)
			{
				foreach (Supporter supporter in new List<Supporter>(decisionOutcome.SupporterList))
				{
					decisionOutcome.ResetSupport(supporter);
				}
			}
			this.DetermineSupport(possibleOutcomes, true);
		}

		// Token: 0x060027F8 RID: 10232 RVA: 0x000A6688 File Offset: 0x000A4888
		private void ReadyToAiChoose()
		{
			this._chosenOutcome = this.GetAiChoice(this._possibleOutcomes);
			if (this._decision.OnShowDecision())
			{
				this.ApplyChosenOutcome();
			}
		}

		// Token: 0x060027F9 RID: 10233 RVA: 0x000A66B0 File Offset: 0x000A48B0
		private void ApplyChosenOutcome()
		{
			this._decision.ApplyChosenOutcome(this._chosenOutcome);
			this._decision.SupportStatusOfFinalDecision = this.GetSupportStatusOfDecisionOutcome(this._chosenOutcome);
			this.HandleInfluenceCosts();
			this.ApplySecondaryEffects(this._possibleOutcomes, this._chosenOutcome);
			for (int i = 0; i < this._possibleOutcomes.Count; i++)
			{
				if (this._possibleOutcomes[i].SponsorClan != null)
				{
					foreach (Supporter supporter in this._possibleOutcomes[i].SupporterList)
					{
						if (supporter.Clan.Leader != this._possibleOutcomes[i].SponsorClan.Leader && supporter.Clan == Clan.PlayerClan)
						{
							int num = this.GetRelationChangeWithSponsor(supporter.Clan.Leader, supporter.SupportWeight, false);
							if (num != 0)
							{
								num *= ((this._possibleOutcomes.Count > 2) ? 2 : 1);
								ChangeRelationAction.ApplyRelationChangeBetweenHeroes(supporter.Clan.Leader, this._possibleOutcomes[i].SponsorClan.Leader, num, true);
							}
						}
					}
					for (int j = 0; j < this._possibleOutcomes.Count; j++)
					{
						if (i != j)
						{
							foreach (Supporter supporter2 in this._possibleOutcomes[j].SupporterList)
							{
								if (supporter2.Clan.Leader != this._possibleOutcomes[i].SponsorClan.Leader && supporter2.Clan == Clan.PlayerClan)
								{
									int relationChangeWithSponsor = this.GetRelationChangeWithSponsor(supporter2.Clan.Leader, supporter2.SupportWeight, true);
									if (relationChangeWithSponsor != 0)
									{
										ChangeRelationAction.ApplyRelationChangeBetweenHeroes(supporter2.Clan.Leader, this._possibleOutcomes[i].SponsorClan.Leader, relationChangeWithSponsor, true);
									}
								}
							}
						}
					}
				}
			}
			this._decision.Kingdom.RemoveDecision(this._decision);
			this._decision.Kingdom.OnKingdomDecisionConcluded();
			CampaignEventDispatcher.Instance.OnKingdomDecisionConcluded(this._decision, this._chosenOutcome, this.IsPlayerChooser || this._hasPlayerVoted);
		}

		// Token: 0x060027FA RID: 10234 RVA: 0x000A6948 File Offset: 0x000A4B48
		public int GetRelationChangeWithSponsor(Hero opposerOrSupporter, Supporter.SupportWeights supportWeight, bool isOpposingSides)
		{
			int num = 0;
			Clan clan = opposerOrSupporter.Clan;
			if (supportWeight == Supporter.SupportWeights.FullyPush)
			{
				num = (int)((float)this._decision.GetInfluenceCostOfSupport(clan, Supporter.SupportWeights.FullyPush) / 20f);
			}
			else if (supportWeight == Supporter.SupportWeights.StronglyFavor)
			{
				num = (int)((float)this._decision.GetInfluenceCostOfSupport(clan, Supporter.SupportWeights.StronglyFavor) / 20f);
			}
			else if (supportWeight == Supporter.SupportWeights.SlightlyFavor)
			{
				num = (int)((float)this._decision.GetInfluenceCostOfSupport(clan, Supporter.SupportWeights.SlightlyFavor) / 20f);
			}
			int num2 = (isOpposingSides ? (num * -1) : (num * 2));
			if (isOpposingSides && opposerOrSupporter.Culture.HasFeat(DefaultCulturalFeats.SturgianDecisionPenaltyFeat))
			{
				num2 += (int)((float)num2 * DefaultCulturalFeats.SturgianDecisionPenaltyFeat.EffectBonus);
			}
			return num2;
		}

		// Token: 0x060027FB RID: 10235 RVA: 0x000A69E4 File Offset: 0x000A4BE4
		private void HandleInfluenceCosts()
		{
			DecisionOutcome decisionOutcome = this._possibleOutcomes[0];
			foreach (DecisionOutcome decisionOutcome2 in this._possibleOutcomes)
			{
				if (decisionOutcome2.TotalSupportPoints > decisionOutcome.TotalSupportPoints)
				{
					decisionOutcome = decisionOutcome2;
				}
				for (int i = 0; i < decisionOutcome2.SupporterList.Count; i++)
				{
					Clan clan = decisionOutcome2.SupporterList[i].Clan;
					int num = this._decision.GetInfluenceCost(decisionOutcome2, clan, decisionOutcome2.SupporterList[i].SupportWeight);
					if (this._supporters.Count == 1)
					{
						num = 0;
					}
					if (this._chosenOutcome != decisionOutcome2)
					{
						num /= 2;
					}
					if (decisionOutcome2 == this._chosenOutcome || !clan.Leader.GetPerkValue(DefaultPerks.Charm.GoodNatured))
					{
						ChangeClanInfluenceAction.Apply(clan, (float)(-(float)num));
					}
				}
			}
			if (this._chosenOutcome != decisionOutcome)
			{
				int influenceRequiredToOverrideKingdomDecision = Campaign.Current.Models.ClanPoliticsModel.GetInfluenceRequiredToOverrideKingdomDecision(decisionOutcome, this._chosenOutcome, this._decision);
				ChangeClanInfluenceAction.Apply(this._chooser, (float)(-(float)influenceRequiredToOverrideKingdomDecision));
			}
		}

		// Token: 0x060027FC RID: 10236 RVA: 0x000A6B28 File Offset: 0x000A4D28
		private void ApplySecondaryEffects(MBReadOnlyList<DecisionOutcome> possibleOutcomes, DecisionOutcome chosenOutcome)
		{
			this._decision.ApplySecondaryEffects(possibleOutcomes, chosenOutcome);
		}

		// Token: 0x060027FD RID: 10237 RVA: 0x000A6B37 File Offset: 0x000A4D37
		private int GetInfluenceRequiredToOverrideDecision(DecisionOutcome popularOutcome, DecisionOutcome overridingOutcome)
		{
			return Campaign.Current.Models.ClanPoliticsModel.GetInfluenceRequiredToOverrideKingdomDecision(popularOutcome, overridingOutcome, this._decision);
		}

		// Token: 0x060027FE RID: 10238 RVA: 0x000A6B58 File Offset: 0x000A4D58
		private DecisionOutcome GetAiChoice(MBReadOnlyList<DecisionOutcome> possibleOutcomes)
		{
			this.DetermineOfficialSupport();
			DecisionOutcome decisionOutcome = possibleOutcomes.MaxBy<DecisionOutcome, float>((DecisionOutcome t) => t.TotalSupportPoints);
			DecisionOutcome decisionOutcome2 = decisionOutcome;
			if (this._decision.IsKingsVoteAllowed)
			{
				DecisionOutcome decisionOutcome3 = possibleOutcomes.MaxBy<DecisionOutcome, float>((DecisionOutcome t) => this._decision.DetermineSupport(this._chooser, t));
				float num = this._decision.DetermineSupport(this._chooser, decisionOutcome3);
				float num2 = this._decision.DetermineSupport(this._chooser, decisionOutcome);
				float num3 = num - num2;
				num3 = MathF.Min(num3, this._chooser.Influence);
				if (num3 > 10f)
				{
					float num4 = 300f + (float)this.GetInfluenceRequiredToOverrideDecision(decisionOutcome, decisionOutcome3);
					if (num3 > num4)
					{
						float num5 = num4 / num3;
						if (MBRandom.RandomFloat > num5)
						{
							decisionOutcome2 = decisionOutcome3;
						}
					}
				}
			}
			return decisionOutcome2;
		}

		// Token: 0x060027FF RID: 10239 RVA: 0x000A6C28 File Offset: 0x000A4E28
		public TextObject GetChosenOutcomeText()
		{
			return this._decision.GetChosenOutcomeText(this._chosenOutcome, this._decision.SupportStatusOfFinalDecision, false);
		}

		// Token: 0x06002800 RID: 10240 RVA: 0x000A6C48 File Offset: 0x000A4E48
		private KingdomDecision.SupportStatus GetSupportStatusOfDecisionOutcome(DecisionOutcome chosenOutcome)
		{
			KingdomDecision.SupportStatus supportStatus = KingdomDecision.SupportStatus.Equal;
			float num = chosenOutcome.WinChance * 100f;
			int num2 = 50;
			if (num > (float)(num2 + 5))
			{
				supportStatus = KingdomDecision.SupportStatus.Majority;
			}
			else if (num < (float)(num2 - 5))
			{
				supportStatus = KingdomDecision.SupportStatus.Minority;
			}
			return supportStatus;
		}

		// Token: 0x06002801 RID: 10241 RVA: 0x000A6C7C File Offset: 0x000A4E7C
		public void DetermineOfficialSupport()
		{
			float num = 0.001f;
			foreach (DecisionOutcome decisionOutcome in this._possibleOutcomes)
			{
				float num2 = 0f;
				foreach (Supporter supporter in decisionOutcome.SupporterList)
				{
					num2 += (float)MathF.Max(0, supporter.SupportWeight - Supporter.SupportWeights.StayNeutral);
				}
				decisionOutcome.TotalSupportPoints = num2;
				num += decisionOutcome.TotalSupportPoints;
			}
			foreach (DecisionOutcome decisionOutcome2 in this._possibleOutcomes)
			{
				decisionOutcome2.TotalSupportPoints /= num;
			}
		}

		// Token: 0x06002802 RID: 10242 RVA: 0x000A6D7C File Offset: 0x000A4F7C
		public float GetWinChanceWithPlayerSupport(DecisionOutcome supportedOutcome, Supporter.SupportWeights supportWeight)
		{
			if (this._possibleOutcomes.Contains(supportedOutcome))
			{
				float num;
				if (supportedOutcome.WinChance != 0f)
				{
					num = supportedOutcome.TotalSupportPoints / supportedOutcome.WinChance;
				}
				else
				{
					num = this._possibleOutcomes.First<DecisionOutcome>((DecisionOutcome x) => x != supportedOutcome).TotalSupportPoints;
				}
				int num2 = MathF.Max(0, supportWeight - Supporter.SupportWeights.StayNeutral);
				supportedOutcome.TotalSupportPoints += (float)num2;
				num += (float)num2;
				return supportedOutcome.TotalSupportPoints / num;
			}
			return 0f;
		}

		// Token: 0x06002803 RID: 10243 RVA: 0x000A6E31 File Offset: 0x000A5031
		public int GetInfluenceCostOfOutcome(DecisionOutcome outcome, Clan supporter, Supporter.SupportWeights weight)
		{
			return this._decision.GetInfluenceCostOfSupport(supporter, weight);
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x000A6E40 File Offset: 0x000A5040
		public TextObject GetSecondaryEffects()
		{
			return this._decision.GetSecondaryEffects();
		}

		// Token: 0x06002805 RID: 10245 RVA: 0x000A6E50 File Offset: 0x000A5050
		public void OnPlayerSupport(DecisionOutcome decisionOutcome, Supporter.SupportWeights supportWeight)
		{
			if (!this.IsPlayerChooser)
			{
				foreach (DecisionOutcome decisionOutcome2 in this._possibleOutcomes)
				{
					decisionOutcome2.ResetSupport(this.PlayerAsSupporter);
				}
				this._hasPlayerVoted = true;
				if (decisionOutcome != null)
				{
					this.PlayerAsSupporter.SupportWeight = supportWeight;
					decisionOutcome.AddSupport(this.PlayerAsSupporter);
					return;
				}
			}
			else
			{
				this._chosenOutcome = decisionOutcome;
			}
		}

		// Token: 0x06002806 RID: 10246 RVA: 0x000A6ED8 File Offset: 0x000A50D8
		public void OnPlayerAbstainedAsRuler()
		{
			this._chosenOutcome = this.GetAiChoice(this._possibleOutcomes);
		}

		// Token: 0x06002807 RID: 10247 RVA: 0x000A6EEC File Offset: 0x000A50EC
		public void ApplySelection()
		{
			if (!this.IsCancelled)
			{
				if (this._chooser != Clan.PlayerClan)
				{
					this.ReadyToAiChoose();
					return;
				}
				this.ApplyChosenOutcome();
			}
		}

		// Token: 0x06002808 RID: 10248 RVA: 0x000A6F10 File Offset: 0x000A5110
		public MBList<DecisionOutcome> GetSortedDecisionOutcomes()
		{
			return this._decision.SortDecisionOutcomes(this._possibleOutcomes);
		}

		// Token: 0x06002809 RID: 10249 RVA: 0x000A6F23 File Offset: 0x000A5123
		public TextObject GetGeneralTitle()
		{
			return this._decision.GetGeneralTitle();
		}

		// Token: 0x0600280A RID: 10250 RVA: 0x000A6F30 File Offset: 0x000A5130
		public TextObject GetTitle()
		{
			if (this.IsPlayerChooser)
			{
				return this._decision.GetChooseTitle();
			}
			return this._decision.GetSupportTitle();
		}

		// Token: 0x0600280B RID: 10251 RVA: 0x000A6F51 File Offset: 0x000A5151
		public TextObject GetDescription()
		{
			if (this.IsPlayerChooser)
			{
				return this._decision.GetChooseDescription();
			}
			return this._decision.GetSupportDescription();
		}

		// Token: 0x04000BA3 RID: 2979
		private const float StrongSupportThreshold = 0.75f;

		// Token: 0x04000BA4 RID: 2980
		private const float GoodSupportThreshold = 0.5f;

		// Token: 0x04000BA5 RID: 2981
		private const float SlightSupportThreshold = 0.2f;

		// Token: 0x04000BA6 RID: 2982
		[SaveableField(0)]
		private readonly KingdomDecision _decision;

		// Token: 0x04000BA7 RID: 2983
		private MBList<DecisionOutcome> _possibleOutcomes;

		// Token: 0x04000BA8 RID: 2984
		[SaveableField(2)]
		private List<Supporter> _supporters;

		// Token: 0x04000BA9 RID: 2985
		[SaveableField(3)]
		private Clan _chooser;

		// Token: 0x04000BAA RID: 2986
		[SaveableField(4)]
		private DecisionOutcome _chosenOutcome;

		// Token: 0x04000BAB RID: 2987
		[SaveableField(5)]
		private bool _ignorePlayerSupport;

		// Token: 0x04000BAC RID: 2988
		[SaveableField(6)]
		private bool _hasPlayerVoted;

		// Token: 0x020006A9 RID: 1705
		public enum ElectionOutcomeSupport
		{
			// Token: 0x04001B71 RID: 7025
			LowSupport,
			// Token: 0x04001B72 RID: 7026
			SlightSupport,
			// Token: 0x04001B73 RID: 7027
			GoodSupport,
			// Token: 0x04001B74 RID: 7028
			StrongSupport
		}
	}
}
