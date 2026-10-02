using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000097 RID: 151
	public class PartyThinkParams
	{
		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x060012E5 RID: 4837 RVA: 0x00057AFA File Offset: 0x00055CFA
		public MBReadOnlyList<ValueTuple<AIBehaviorData, float>> AIBehaviorScores
		{
			get
			{
				return this._aiBehaviorScores;
			}
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x060012E6 RID: 4838 RVA: 0x00057B02 File Offset: 0x00055D02
		public MBReadOnlyList<MobileParty> PossibleArmyMembersUponArmyCreation
		{
			get
			{
				return this._possibleArmyMembersUponArmyCreation;
			}
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x00057B0A File Offset: 0x00055D0A
		public PartyThinkParams(MobileParty mobileParty)
		{
			this._aiBehaviorScores = new MBList<ValueTuple<AIBehaviorData, float>>(32);
			this._possibleArmyMembersUponArmyCreation = null;
			this.MobilePartyOf = mobileParty;
			this.WillGatherAnArmy = false;
			this.DoNotChangeBehavior = false;
			this.CurrentObjectiveValue = 0f;
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x00057B48 File Offset: 0x00055D48
		public void Reset(MobileParty mobileParty)
		{
			this._aiBehaviorScores.Clear();
			MBList<MobileParty> possibleArmyMembersUponArmyCreation = this._possibleArmyMembersUponArmyCreation;
			if (possibleArmyMembersUponArmyCreation != null)
			{
				possibleArmyMembersUponArmyCreation.Clear();
			}
			this.MobilePartyOf = mobileParty;
			this.WillGatherAnArmy = false;
			this.DoNotChangeBehavior = false;
			this.CurrentObjectiveValue = 0f;
			this.StrengthOfLordsWithoutArmy = 0f;
			this.StrengthOfLordsWithArmy = 0f;
			this.StrengthOfLordsAtSameClanWithoutArmy = 0f;
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x00057BB4 File Offset: 0x00055DB4
		public void Initialization()
		{
			this.StrengthOfLordsWithoutArmy = 0f;
			this.StrengthOfLordsWithArmy = 0f;
			this.StrengthOfLordsAtSameClanWithoutArmy = 0f;
			foreach (Hero hero in this.MobilePartyOf.MapFaction.Heroes)
			{
				if (hero.PartyBelongedTo != null)
				{
					MobileParty partyBelongedTo = hero.PartyBelongedTo;
					if (partyBelongedTo.Army != null)
					{
						this.StrengthOfLordsWithArmy += partyBelongedTo.Party.EstimatedStrength;
					}
					else
					{
						this.StrengthOfLordsWithoutArmy += partyBelongedTo.Party.EstimatedStrength;
						Clan clan = hero.Clan;
						Hero leaderHero = this.MobilePartyOf.LeaderHero;
						if (clan == ((leaderHero != null) ? leaderHero.Clan : null))
						{
							this.StrengthOfLordsAtSameClanWithoutArmy += partyBelongedTo.Party.EstimatedStrength;
						}
					}
				}
			}
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x00057CB4 File Offset: 0x00055EB4
		public void SetArmyMembers(MBList<MobileParty> armyMembers)
		{
			this._possibleArmyMembersUponArmyCreation = armyMembers;
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x00057CC0 File Offset: 0x00055EC0
		public bool TryGetBehaviorScore(in AIBehaviorData aiBehaviorData, out float score)
		{
			foreach (ValueTuple<AIBehaviorData, float> valueTuple in this._aiBehaviorScores)
			{
				AIBehaviorData item = valueTuple.Item1;
				if (item.Equals(aiBehaviorData))
				{
					score = valueTuple.Item2;
					return true;
				}
			}
			score = 0f;
			return false;
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x00057D38 File Offset: 0x00055F38
		public void SetBehaviorScore(in AIBehaviorData aiBehaviorData, float score)
		{
			for (int i = 0; i < this._aiBehaviorScores.Count; i++)
			{
				if (this._aiBehaviorScores[i].Item1.Equals(aiBehaviorData))
				{
					this._aiBehaviorScores[i] = new ValueTuple<AIBehaviorData, float>(this._aiBehaviorScores[i].Item1, score);
					return;
				}
			}
			Debug.FailedAssert("AIBehaviorScore not found.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\ICampaignBehaviorManager.cs", "SetBehaviorScore", 196);
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x00057DB9 File Offset: 0x00055FB9
		public void AddBehaviorScore(in ValueTuple<AIBehaviorData, float> value)
		{
			this._aiBehaviorScores.Add(value);
		}

		// Token: 0x04000637 RID: 1591
		public MobileParty MobilePartyOf;

		// Token: 0x04000638 RID: 1592
		private readonly MBList<ValueTuple<AIBehaviorData, float>> _aiBehaviorScores;

		// Token: 0x04000639 RID: 1593
		private MBList<MobileParty> _possibleArmyMembersUponArmyCreation;

		// Token: 0x0400063A RID: 1594
		public float CurrentObjectiveValue;

		// Token: 0x0400063B RID: 1595
		public bool WillGatherAnArmy;

		// Token: 0x0400063C RID: 1596
		public bool DoNotChangeBehavior;

		// Token: 0x0400063D RID: 1597
		public float StrengthOfLordsWithoutArmy;

		// Token: 0x0400063E RID: 1598
		public float StrengthOfLordsWithArmy;

		// Token: 0x0400063F RID: 1599
		public float StrengthOfLordsAtSameClanWithoutArmy;
	}
}
