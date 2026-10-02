using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Core.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.BarterSystem.Barterables
{
	// Token: 0x020004A9 RID: 1193
	public class MarriageBarterable : Barterable
	{
		// Token: 0x17000EEC RID: 3820
		// (get) Token: 0x06004C48 RID: 19528 RVA: 0x00182B79 File Offset: 0x00180D79
		public override string StringID
		{
			get
			{
				return "marriage_barterable";
			}
		}

		// Token: 0x06004C49 RID: 19529 RVA: 0x00182B80 File Offset: 0x00180D80
		public MarriageBarterable(Hero owner, PartyBase ownerParty, Hero heroBeingProposedTo, Hero proposingHero)
			: base(owner, ownerParty)
		{
			this.HeroBeingProposedTo = heroBeingProposedTo;
			this.ProposingHero = proposingHero;
		}

		// Token: 0x17000EED RID: 3821
		// (get) Token: 0x06004C4A RID: 19530 RVA: 0x00182B99 File Offset: 0x00180D99
		public override TextObject Name
		{
			get
			{
				StringHelpers.SetCharacterProperties("HERO_BEING_PROPOSED_TO", this.HeroBeingProposedTo.CharacterObject, null, false);
				StringHelpers.SetCharacterProperties("HERO_TO_MARRY", this.ProposingHero.CharacterObject, null, false);
				return new TextObject("{=rv6hk8X2}{HERO_BEING_PROPOSED_TO.NAME} to marry {HERO_TO_MARRY.NAME}", null);
			}
		}

		// Token: 0x06004C4B RID: 19531 RVA: 0x00182BD8 File Offset: 0x00180DD8
		public override int GetUnitValueForFaction(IFaction faction)
		{
			if (faction == this.ProposingHero.Clan)
			{
				float num = -50000f;
				float num2 = (float)this.ProposingHero.RandomInt(10000);
				float num3 = (float)(this.ProposingHero.RandomInt(-25000, 25000) + this.HeroBeingProposedTo.RandomInt(-25000, 25000));
				if (this.ProposingHero == Hero.MainHero)
				{
					num3 = 0f;
					num2 = 0f;
				}
				float num4 = (float)(this.ProposingHero.GetRelation(this.HeroBeingProposedTo) * 1000);
				Campaign.Current.Models.DiplomacyModel.GetHeroCommandingStrengthForClan(this.ProposingHero);
				Campaign.Current.Models.DiplomacyModel.GetHeroCommandingStrengthForClan(this.HeroBeingProposedTo);
				float num5 = ((this.ProposingHero.Clan == null) ? 0f : ((float)this.ProposingHero.Clan.Tier + ((this.ProposingHero.Clan.Leader == this.ProposingHero.MapFaction.Leader) ? (MathF.Min(3f, (float)this.ProposingHero.MapFaction.Fiefs.Count / 10f) + 0.5f) : 0f)));
				float num6 = ((this.HeroBeingProposedTo.Clan == null) ? 0f : ((float)this.HeroBeingProposedTo.Clan.Tier + ((this.HeroBeingProposedTo.Clan.Leader == this.HeroBeingProposedTo.MapFaction.Leader) ? (MathF.Min(3f, (float)this.HeroBeingProposedTo.MapFaction.Fiefs.Count / 10f) + 0.5f) : 0f)));
				float num7 = ((faction == this.ProposingHero.Clan) ? ((num6 - num5) * MathF.Abs(num6 - num5) * 1000f) : ((num5 - num6) * MathF.Abs(num5 - num6) * 1000f));
				int relationBetweenClans = FactionManager.GetRelationBetweenClans(this.HeroBeingProposedTo.Clan, this.ProposingHero.Clan);
				int num8 = 1000 * relationBetweenClans;
				Clan clanAfterMarriage = Campaign.Current.Models.MarriageModel.GetClanAfterMarriage(this.HeroBeingProposedTo, this.ProposingHero);
				float num9 = 0f;
				float num10 = 0f;
				if (clanAfterMarriage != this.HeroBeingProposedTo.Clan)
				{
					if (faction == clanAfterMarriage)
					{
						num9 = Campaign.Current.Models.DiplomacyModel.GetValueOfHeroForFaction(this.HeroBeingProposedTo, clanAfterMarriage, true);
					}
					else if (faction == this.HeroBeingProposedTo.Clan)
					{
						num9 = Campaign.Current.Models.DiplomacyModel.GetValueOfHeroForFaction(this.HeroBeingProposedTo, this.HeroBeingProposedTo.Clan, true);
					}
					if (clanAfterMarriage.Kingdom != null && clanAfterMarriage.Kingdom != this.HeroBeingProposedTo.Clan.Kingdom)
					{
						num10 = Campaign.Current.Models.DiplomacyModel.GetValueOfHeroForFaction(this.HeroBeingProposedTo, clanAfterMarriage.Kingdom, true);
					}
				}
				float num11 = (float)Campaign.Current.Models.AgeModel.HeroComesOfAge;
				float num12 = 2f * MathF.Min(0f, 20f - MathF.Max(this.HeroBeingProposedTo.Age - num11, 0f)) * MathF.Min(0f, 20f - MathF.Max(this.HeroBeingProposedTo.Age - num11, 0f)) * MathF.Min(0f, 20f - MathF.Max(this.HeroBeingProposedTo.Age - num11, 0f));
				return (int)(num + num2 + num3 + num4 + num9 + (float)num8 + num10 + num7 + num12);
			}
			float num13 = -this.HeroBeingProposedTo.Clan.Renown;
			float num14 = (float)Campaign.Current.Models.AgeModel.HeroComesOfAge;
			float num15 = -(2f * MathF.Min(0f, 20f - MathF.Max(this.HeroBeingProposedTo.Age - num14, 0f)) * MathF.Min(0f, 20f - MathF.Max(this.HeroBeingProposedTo.Age - num14, 0f)) * MathF.Min(0f, 20f - MathF.Max(this.HeroBeingProposedTo.Age - num14, 0f)));
			return (int)(num13 + num15);
		}

		// Token: 0x06004C4C RID: 19532 RVA: 0x00183040 File Offset: 0x00181240
		public override void CheckBarterLink(Barterable linkedBarterable)
		{
			if (linkedBarterable.GetType() == typeof(MarriageBarterable) && linkedBarterable.OriginalOwner == base.OriginalOwner && ((MarriageBarterable)linkedBarterable).HeroBeingProposedTo == this.HeroBeingProposedTo && ((MarriageBarterable)linkedBarterable).ProposingHero == this.ProposingHero)
			{
				base.AddBarterLink(linkedBarterable);
			}
		}

		// Token: 0x06004C4D RID: 19533 RVA: 0x001830A0 File Offset: 0x001812A0
		public override bool IsCompatible(Barterable barterable)
		{
			MarriageBarterable marriageBarterable = barterable as MarriageBarterable;
			return marriageBarterable == null || (marriageBarterable.HeroBeingProposedTo != this.HeroBeingProposedTo && marriageBarterable.HeroBeingProposedTo != this.ProposingHero && marriageBarterable.ProposingHero != this.HeroBeingProposedTo && marriageBarterable.ProposingHero != this.ProposingHero);
		}

		// Token: 0x06004C4E RID: 19534 RVA: 0x001830F6 File Offset: 0x001812F6
		public override ImageIdentifier GetVisualIdentifier()
		{
			return new CharacterImageIdentifier(CharacterCode.CreateFrom(this.HeroBeingProposedTo.CharacterObject));
		}

		// Token: 0x06004C4F RID: 19535 RVA: 0x0018310D File Offset: 0x0018130D
		public override string GetEncyclopediaLink()
		{
			return this.HeroBeingProposedTo.EncyclopediaLink;
		}

		// Token: 0x06004C50 RID: 19536 RVA: 0x0018311A File Offset: 0x0018131A
		public override void Apply()
		{
			MarriageAction.Apply(this.HeroBeingProposedTo, this.ProposingHero, this.HeroBeingProposedTo.Clan == Clan.PlayerClan || this.ProposingHero.Clan == Clan.PlayerClan);
		}

		// Token: 0x06004C51 RID: 19537 RVA: 0x00183154 File Offset: 0x00181354
		internal static void AutoGeneratedStaticCollectObjectsMarriageBarterable(object o, List<object> collectedObjects)
		{
			((MarriageBarterable)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06004C52 RID: 19538 RVA: 0x00183162 File Offset: 0x00181362
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this.ProposingHero);
			collectedObjects.Add(this.HeroBeingProposedTo);
		}

		// Token: 0x06004C53 RID: 19539 RVA: 0x00183183 File Offset: 0x00181383
		internal static object AutoGeneratedGetMemberValueProposingHero(object o)
		{
			return ((MarriageBarterable)o).ProposingHero;
		}

		// Token: 0x06004C54 RID: 19540 RVA: 0x00183190 File Offset: 0x00181390
		internal static object AutoGeneratedGetMemberValueHeroBeingProposedTo(object o)
		{
			return ((MarriageBarterable)o).HeroBeingProposedTo;
		}

		// Token: 0x04001537 RID: 5431
		[SaveableField(600)]
		public readonly Hero ProposingHero;

		// Token: 0x04001538 RID: 5432
		[SaveableField(601)]
		public readonly Hero HeroBeingProposedTo;
	}
}
