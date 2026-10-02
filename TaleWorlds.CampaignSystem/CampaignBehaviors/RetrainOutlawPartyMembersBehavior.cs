using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200045F RID: 1119
	public class RetrainOutlawPartyMembersBehavior : CampaignBehaviorBase, IRetrainOutlawPartyMembersCampaignBehavior, ICampaignBehavior
	{
		// Token: 0x06004836 RID: 18486 RVA: 0x001656C8 File Offset: 0x001638C8
		private int GetRetrainedNumberInternal(CharacterObject character)
		{
			int num;
			if (!this._retrainTable.TryGetValue(character, out num))
			{
				return 0;
			}
			return num;
		}

		// Token: 0x06004837 RID: 18487 RVA: 0x001656E8 File Offset: 0x001638E8
		private int SetRetrainedNumberInternal(CharacterObject character, int numberRetrained)
		{
			this._retrainTable[character] = numberRetrained;
			return numberRetrained;
		}

		// Token: 0x06004838 RID: 18488 RVA: 0x00165705 File Offset: 0x00163905
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
		}

		// Token: 0x06004839 RID: 18489 RVA: 0x00165720 File Offset: 0x00163920
		private void DailyTick()
		{
			if (MBRandom.RandomFloat > 0.5f)
			{
				int num = MBRandom.RandomInt(MobileParty.MainParty.MemberRoster.Count);
				bool flag = false;
				int num2 = 0;
				while (num2 < MobileParty.MainParty.MemberRoster.Count && !flag)
				{
					int num3 = (num2 + num) % MobileParty.MainParty.MemberRoster.Count;
					CharacterObject characterAtIndex = MobileParty.MainParty.MemberRoster.GetCharacterAtIndex(num3);
					if (characterAtIndex.Occupation == Occupation.Bandit)
					{
						int elementNumber = MobileParty.MainParty.MemberRoster.GetElementNumber(num3);
						int num4 = this.GetRetrainedNumberInternal(characterAtIndex);
						if (num4 < elementNumber && !flag)
						{
							num4++;
							this.SetRetrainedNumberInternal(characterAtIndex, num4);
						}
						else if (num4 > elementNumber)
						{
							this.SetRetrainedNumberInternal(characterAtIndex, elementNumber);
						}
					}
					num2++;
				}
			}
		}

		// Token: 0x0600483A RID: 18490 RVA: 0x001657ED File Offset: 0x001639ED
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<CharacterObject, int>>("_retrainTable", ref this._retrainTable);
		}

		// Token: 0x0600483B RID: 18491 RVA: 0x00165804 File Offset: 0x00163A04
		public int GetRetrainedNumber(CharacterObject character)
		{
			if (character.Occupation == Occupation.Bandit)
			{
				int retrainedNumberInternal = this.GetRetrainedNumberInternal(character);
				int troopCount = MobileParty.MainParty.MemberRoster.GetTroopCount(character);
				return MathF.Min(retrainedNumberInternal, troopCount);
			}
			return 0;
		}

		// Token: 0x0600483C RID: 18492 RVA: 0x0016583B File Offset: 0x00163A3B
		public void SetRetrainedNumber(CharacterObject character, int number)
		{
			this.SetRetrainedNumberInternal(character, number);
		}

		// Token: 0x04001475 RID: 5237
		private Dictionary<CharacterObject, int> _retrainTable = new Dictionary<CharacterObject, int>();
	}
}
