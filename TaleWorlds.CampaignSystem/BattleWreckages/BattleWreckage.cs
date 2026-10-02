using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.BattleWreckages
{
	// Token: 0x02000497 RID: 1175
	public sealed class BattleWreckage : MBObjectBase, IInteractablePoint
	{
		// Token: 0x17000EBA RID: 3770
		// (get) Token: 0x06004B78 RID: 19320 RVA: 0x001809B2 File Offset: 0x0017EBB2
		// (set) Token: 0x06004B79 RID: 19321 RVA: 0x001809BA File Offset: 0x0017EBBA
		public bool IsVisible { get; private set; }

		// Token: 0x17000EBB RID: 3771
		// (get) Token: 0x06004B7A RID: 19322 RVA: 0x001809C3 File Offset: 0x0017EBC3
		// (set) Token: 0x06004B7B RID: 19323 RVA: 0x001809CB File Offset: 0x0017EBCB
		public TextObject Name { get; private set; }

		// Token: 0x17000EBC RID: 3772
		// (get) Token: 0x06004B7C RID: 19324 RVA: 0x001809D4 File Offset: 0x0017EBD4
		public bool IsInvestigated
		{
			get
			{
				return this._isInvestigated;
			}
		}

		// Token: 0x17000EBD RID: 3773
		// (get) Token: 0x06004B7D RID: 19325 RVA: 0x001809DC File Offset: 0x0017EBDC
		public bool IsWreckageDestroyable
		{
			get
			{
				return !this.DestroyTime.IsFuture;
			}
		}

		// Token: 0x17000EBE RID: 3774
		// (get) Token: 0x06004B7E RID: 19326 RVA: 0x001809FA File Offset: 0x0017EBFA
		public int TotalNumberOfWoundedInBattle
		{
			get
			{
				return this.AttackerWoundedInBattle.TotalRegulars + this.DefenderWoundedInBattle.TotalRegulars;
			}
		}

		// Token: 0x17000EBF RID: 3775
		// (get) Token: 0x06004B7F RID: 19327 RVA: 0x00180A13 File Offset: 0x0017EC13
		public int TotalNumberOfDiedInBattle
		{
			get
			{
				return this.AttackerDiedInBattle.TotalRegulars + this.DefenderDiedInBattle.TotalRegulars;
			}
		}

		// Token: 0x17000EC0 RID: 3776
		// (get) Token: 0x06004B80 RID: 19328 RVA: 0x00180A2C File Offset: 0x0017EC2C
		public int TotalCasualtyCountInBattle
		{
			get
			{
				return this.TotalNumberOfWoundedInBattle + this.TotalNumberOfDiedInBattle;
			}
		}

		// Token: 0x06004B81 RID: 19329 RVA: 0x00180A3C File Offset: 0x0017EC3C
		private BattleWreckage(MapEvent mapEvent, BattleWreckage.WreckageType wreckageType, CampaignTime destroyTime)
		{
			base.StringId = Campaign.Current.CampaignObjectManager.FindNextUniqueStringId<BattleWreckage>("wreckage_1");
			this.Position = mapEvent.Position;
			this.WreckageTypeCategory = wreckageType;
			this.DestroyTime = destroyTime;
			PartyBase leaderParty = mapEvent.AttackerSide.LeaderParty;
			bool flag;
			if (leaderParty == null)
			{
				flag = false;
			}
			else
			{
				MobileParty mobileParty = leaderParty.MobileParty;
				bool? flag2 = ((mobileParty != null) ? new bool?(mobileParty.IsLordParty) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			this.AttackerLeaderHero = (flag ? mapEvent.AttackerSide.LeaderParty.Owner : null);
			PartyBase leaderParty2 = mapEvent.DefenderSide.LeaderParty;
			bool flag4;
			if (leaderParty2 == null)
			{
				flag4 = false;
			}
			else
			{
				MobileParty mobileParty2 = leaderParty2.MobileParty;
				bool? flag2 = ((mobileParty2 != null) ? new bool?(mobileParty2.IsLordParty) : null);
				bool flag3 = true;
				flag4 = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			this.DefenderLeaderHero = (flag4 ? mapEvent.DefenderSide.LeaderParty.Owner : null);
			this.AttackerLeaderPartyName = mapEvent.AttackerSide.LeaderParty.Name;
			this.DefenderLeaderPartyName = mapEvent.DefenderSide.LeaderParty.Name;
			this.AttackerFaction = mapEvent.AttackerSide.MapFaction;
			this.DefenderFaction = mapEvent.DefenderSide.MapFaction;
			this.WinnerSide = mapEvent.WinningSide;
			this.BattleStartTime = mapEvent.BattleStartTime;
			this.AttackerHealthyTroopCountAtStart = mapEvent.AttackerSide.Parties.SumQ<MapEventParty>((MapEventParty x) => x.HealthyManCountAtStart);
			this.DefenderHealthyTroopCountAtStart = mapEvent.DefenderSide.Parties.SumQ<MapEventParty>((MapEventParty x) => x.HealthyManCountAtStart);
			this.AttackerDiedInBattle = TroopRoster.CreateDummyTroopRoster();
			this.DefenderDiedInBattle = TroopRoster.CreateDummyTroopRoster();
			this.AttackerWoundedInBattle = TroopRoster.CreateDummyTroopRoster();
			this.DefenderWoundedInBattle = TroopRoster.CreateDummyTroopRoster();
			this.Name = this.GetNameOfWreckage();
			foreach (MapEventParty mapEventParty in mapEvent.AttackerSide.Parties)
			{
				this.AttackerWoundedInBattle.Add(mapEventParty.WoundedInBattle);
				this.AttackerDiedInBattle.Add(mapEventParty.DiedInBattle);
			}
			foreach (MapEventParty mapEventParty2 in mapEvent.DefenderSide.Parties)
			{
				this.DefenderWoundedInBattle.Add(mapEventParty2.WoundedInBattle);
				this.DefenderDiedInBattle.Add(mapEventParty2.DiedInBattle);
			}
			this.UpdateVisibility();
		}

		// Token: 0x06004B82 RID: 19330 RVA: 0x00180D20 File Offset: 0x0017EF20
		public static void CreateWreckage(MapEvent mapEvent, BattleWreckage.WreckageType wreckageType, CampaignTime destroyTime)
		{
			BattleWreckage battleWreckage = new BattleWreckage(mapEvent, wreckageType, destroyTime);
			Campaign.Current.CampaignObjectManager.AddWreckage(battleWreckage);
			CampaignEventDispatcher.Instance.OnMapInteractableCreated(battleWreckage);
			LogEntry.AddLogEntry(new WreckageCreatedLogEntry(battleWreckage));
		}

		// Token: 0x06004B83 RID: 19331 RVA: 0x00180D5C File Offset: 0x0017EF5C
		protected override void AfterLoad()
		{
			this.Name = this.GetNameOfWreckage();
		}

		// Token: 0x06004B84 RID: 19332 RVA: 0x00180D6C File Offset: 0x0017EF6C
		private TextObject GetNameOfWreckage()
		{
			bool flag = this.AttackerLeaderHero != null && this.DefenderLeaderHero != null;
			bool isOnLand = this.Position.IsOnLand;
			if (flag && this.WreckageTypeCategory != BattleWreckage.WreckageType.Small)
			{
				TextObject textObject;
				if (isOnLand)
				{
					textObject = new TextObject("{=Um9djvw8}Aftermath of the battle of {CLOSEST_SETTLEMENT_NAME}", null);
				}
				else
				{
					textObject = new TextObject("{=DStYh5rg}Aftermath of the naval battle of {CLOSEST_SETTLEMENT_NAME}", null);
				}
				Settlement settlement = Campaign.Current.Models.MapDistanceModel.GetClosestEntranceToFace(this.Position.Face, isOnLand ? MobileParty.NavigationType.Default : MobileParty.NavigationType.Naval).Item1;
				if (settlement == null)
				{
					settlement = Campaign.Current.Settlements.WhereQ<Settlement>((Settlement x) => isOnLand || x.HasPort).MinBy<Settlement, float>((Settlement x) => x.Position.Distance(MobileParty.MainParty.Position));
				}
				textObject.SetTextVariable("CLOSEST_SETTLEMENT_NAME", settlement.Name);
				return textObject;
			}
			if (this.WreckageTypeCategory == BattleWreckage.WreckageType.Normal)
			{
				if (!isOnLand)
				{
					return new TextObject("{=3tiEeycT}Battle Wreckage", null);
				}
				return new TextObject("{=xg6aoZsH}Battleground", null);
			}
			else
			{
				if (!isOnLand)
				{
					return new TextObject("{=Fxg41jrF}Wreckage", null);
				}
				return new TextObject("{=QG3JTWa8}Skirmish Site", null);
			}
		}

		// Token: 0x06004B85 RID: 19333 RVA: 0x00180EAF File Offset: 0x0017F0AF
		public MBList<TroopRosterElement> GetTotalWoundedInBattle()
		{
			MBList<TroopRosterElement> mblist = new MBList<TroopRosterElement>(this.AttackerWoundedInBattle.GetTroopRoster());
			mblist.AddRange(this.DefenderWoundedInBattle.GetTroopRoster());
			return mblist;
		}

		// Token: 0x06004B86 RID: 19334 RVA: 0x00180ED2 File Offset: 0x0017F0D2
		public MBList<TroopRosterElement> GetTotalDiedInBattle()
		{
			MBList<TroopRosterElement> mblist = new MBList<TroopRosterElement>(this.AttackerDiedInBattle.GetTroopRoster());
			mblist.AddRange(this.DefenderDiedInBattle.GetTroopRoster());
			return mblist;
		}

		// Token: 0x06004B87 RID: 19335 RVA: 0x00180EF5 File Offset: 0x0017F0F5
		public TextObject GetWinnerPartyName()
		{
			if (this.WinnerSide != BattleSideEnum.Attacker)
			{
				return this.DefenderLeaderPartyName;
			}
			return this.AttackerLeaderPartyName;
		}

		// Token: 0x06004B88 RID: 19336 RVA: 0x00180F0D File Offset: 0x0017F10D
		public TextObject GetDefeatedPartyName()
		{
			if (this.WinnerSide != BattleSideEnum.Attacker)
			{
				return this.AttackerLeaderPartyName;
			}
			return this.DefenderLeaderPartyName;
		}

		// Token: 0x06004B89 RID: 19337 RVA: 0x00180F25 File Offset: 0x0017F125
		public IFaction GetWinnerFaction()
		{
			if (this.WinnerSide != BattleSideEnum.Attacker)
			{
				return this.DefenderFaction;
			}
			return this.AttackerFaction;
		}

		// Token: 0x06004B8A RID: 19338 RVA: 0x00180F3D File Offset: 0x0017F13D
		public IFaction GetDefeatedFaction()
		{
			if (this.WinnerSide != BattleSideEnum.Attacker)
			{
				return this.AttackerFaction;
			}
			return this.DefenderFaction;
		}

		// Token: 0x06004B8B RID: 19339 RVA: 0x00180F58 File Offset: 0x0017F158
		public bool CanPartyInteract(MobileParty mobileParty, float dt)
		{
			if (!mobileParty.IsMainParty || (this.IsInvestigated && this.WreckageTypeCategory != BattleWreckage.WreckageType.Epic) || MobileParty.MainParty.Position.IsOnLand != this.Position.IsOnLand)
			{
				return false;
			}
			float num = mobileParty.Position.Distance(this.GetInteractionPosition(mobileParty));
			if (mobileParty.IsCurrentlyAtSea)
			{
				return num < Campaign.Current.Models.EncounterModel.NeededMaximumNavalDistanceForEncounteringMobileParty;
			}
			return num < Campaign.Current.Models.EncounterModel.NeededMaximumLandDistanceForEncounteringMobileParty;
		}

		// Token: 0x06004B8C RID: 19340 RVA: 0x00180FE9 File Offset: 0x0017F1E9
		public CampaignVec2 GetInteractionPosition(MobileParty interactingParty)
		{
			return this.Position;
		}

		// Token: 0x06004B8D RID: 19341 RVA: 0x00180FF1 File Offset: 0x0017F1F1
		public void OnPartyInteraction(MobileParty mobileParty)
		{
			BattleWreckageCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<BattleWreckageCampaignBehavior>();
			if (campaignBehavior == null)
			{
				return;
			}
			campaignBehavior.SetCurrentEncounteredBattleWreckage(this);
		}

		// Token: 0x06004B8E RID: 19342 RVA: 0x00181008 File Offset: 0x0017F208
		public void DestroyWreckage()
		{
			Campaign.Current.CampaignObjectManager.RemoveWreckage(this);
			CampaignEventDispatcher.Instance.OnMapInteractableDestroyed(this);
		}

		// Token: 0x06004B8F RID: 19343 RVA: 0x00181028 File Offset: 0x0017F228
		public void UpdateVisibility()
		{
			if (!Hero.MainHero.IsActive && !Hero.MainHero.IsPrisoner)
			{
				this.IsVisible = false;
				return;
			}
			float num = MobileParty.MainParty.SeeingRange;
			if (num <= 0f)
			{
				this.IsVisible = false;
				return;
			}
			if (this.IsInvestigated)
			{
				num *= 2f;
			}
			float num2 = Hero.MainHero.GetCampaignPosition().Distance(this.Position);
			this.IsVisible = num2 <= num;
		}

		// Token: 0x06004B90 RID: 19344 RVA: 0x001810A6 File Offset: 0x0017F2A6
		public void OnWreckageInvestigated()
		{
			this._isInvestigated = true;
		}

		// Token: 0x06004B91 RID: 19345 RVA: 0x001810AF File Offset: 0x0017F2AF
		internal static void AutoGeneratedStaticCollectObjectsBattleWreckage(object o, List<object> collectedObjects)
		{
			((BattleWreckage)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06004B92 RID: 19346 RVA: 0x001810C0 File Offset: 0x0017F2C0
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			CampaignVec2.AutoGeneratedStaticCollectObjectsCampaignVec2(this.Position, collectedObjects);
			CampaignTime.AutoGeneratedStaticCollectObjectsCampaignTime(this.DestroyTime, collectedObjects);
			collectedObjects.Add(this.AttackerLeaderPartyName);
			collectedObjects.Add(this.DefenderLeaderPartyName);
			collectedObjects.Add(this.AttackerFaction);
			collectedObjects.Add(this.DefenderFaction);
			CampaignTime.AutoGeneratedStaticCollectObjectsCampaignTime(this.BattleStartTime, collectedObjects);
			collectedObjects.Add(this.AttackerDiedInBattle);
			collectedObjects.Add(this.DefenderDiedInBattle);
			collectedObjects.Add(this.AttackerWoundedInBattle);
			collectedObjects.Add(this.DefenderWoundedInBattle);
			collectedObjects.Add(this.AttackerLeaderHero);
			collectedObjects.Add(this.DefenderLeaderHero);
		}

		// Token: 0x06004B93 RID: 19347 RVA: 0x0018117F File Offset: 0x0017F37F
		internal static object AutoGeneratedGetMemberValuePosition(object o)
		{
			return ((BattleWreckage)o).Position;
		}

		// Token: 0x06004B94 RID: 19348 RVA: 0x00181191 File Offset: 0x0017F391
		internal static object AutoGeneratedGetMemberValueDestroyTime(object o)
		{
			return ((BattleWreckage)o).DestroyTime;
		}

		// Token: 0x06004B95 RID: 19349 RVA: 0x001811A3 File Offset: 0x0017F3A3
		internal static object AutoGeneratedGetMemberValueWreckageTypeCategory(object o)
		{
			return ((BattleWreckage)o).WreckageTypeCategory;
		}

		// Token: 0x06004B96 RID: 19350 RVA: 0x001811B5 File Offset: 0x0017F3B5
		internal static object AutoGeneratedGetMemberValueAttackerLeaderPartyName(object o)
		{
			return ((BattleWreckage)o).AttackerLeaderPartyName;
		}

		// Token: 0x06004B97 RID: 19351 RVA: 0x001811C2 File Offset: 0x0017F3C2
		internal static object AutoGeneratedGetMemberValueDefenderLeaderPartyName(object o)
		{
			return ((BattleWreckage)o).DefenderLeaderPartyName;
		}

		// Token: 0x06004B98 RID: 19352 RVA: 0x001811CF File Offset: 0x0017F3CF
		internal static object AutoGeneratedGetMemberValueAttackerFaction(object o)
		{
			return ((BattleWreckage)o).AttackerFaction;
		}

		// Token: 0x06004B99 RID: 19353 RVA: 0x001811DC File Offset: 0x0017F3DC
		internal static object AutoGeneratedGetMemberValueDefenderFaction(object o)
		{
			return ((BattleWreckage)o).DefenderFaction;
		}

		// Token: 0x06004B9A RID: 19354 RVA: 0x001811E9 File Offset: 0x0017F3E9
		internal static object AutoGeneratedGetMemberValueWinnerSide(object o)
		{
			return ((BattleWreckage)o).WinnerSide;
		}

		// Token: 0x06004B9B RID: 19355 RVA: 0x001811FB File Offset: 0x0017F3FB
		internal static object AutoGeneratedGetMemberValueBattleStartTime(object o)
		{
			return ((BattleWreckage)o).BattleStartTime;
		}

		// Token: 0x06004B9C RID: 19356 RVA: 0x0018120D File Offset: 0x0017F40D
		internal static object AutoGeneratedGetMemberValueAttackerHealthyTroopCountAtStart(object o)
		{
			return ((BattleWreckage)o).AttackerHealthyTroopCountAtStart;
		}

		// Token: 0x06004B9D RID: 19357 RVA: 0x0018121F File Offset: 0x0017F41F
		internal static object AutoGeneratedGetMemberValueDefenderHealthyTroopCountAtStart(object o)
		{
			return ((BattleWreckage)o).DefenderHealthyTroopCountAtStart;
		}

		// Token: 0x06004B9E RID: 19358 RVA: 0x00181231 File Offset: 0x0017F431
		internal static object AutoGeneratedGetMemberValueAttackerDiedInBattle(object o)
		{
			return ((BattleWreckage)o).AttackerDiedInBattle;
		}

		// Token: 0x06004B9F RID: 19359 RVA: 0x0018123E File Offset: 0x0017F43E
		internal static object AutoGeneratedGetMemberValueDefenderDiedInBattle(object o)
		{
			return ((BattleWreckage)o).DefenderDiedInBattle;
		}

		// Token: 0x06004BA0 RID: 19360 RVA: 0x0018124B File Offset: 0x0017F44B
		internal static object AutoGeneratedGetMemberValueAttackerWoundedInBattle(object o)
		{
			return ((BattleWreckage)o).AttackerWoundedInBattle;
		}

		// Token: 0x06004BA1 RID: 19361 RVA: 0x00181258 File Offset: 0x0017F458
		internal static object AutoGeneratedGetMemberValueDefenderWoundedInBattle(object o)
		{
			return ((BattleWreckage)o).DefenderWoundedInBattle;
		}

		// Token: 0x06004BA2 RID: 19362 RVA: 0x00181265 File Offset: 0x0017F465
		internal static object AutoGeneratedGetMemberValueAttackerLeaderHero(object o)
		{
			return ((BattleWreckage)o).AttackerLeaderHero;
		}

		// Token: 0x06004BA3 RID: 19363 RVA: 0x00181272 File Offset: 0x0017F472
		internal static object AutoGeneratedGetMemberValueDefenderLeaderHero(object o)
		{
			return ((BattleWreckage)o).DefenderLeaderHero;
		}

		// Token: 0x06004BA4 RID: 19364 RVA: 0x0018127F File Offset: 0x0017F47F
		internal static object AutoGeneratedGetMemberValue_isInvestigated(object o)
		{
			return ((BattleWreckage)o)._isInvestigated;
		}

		// Token: 0x040014FD RID: 5373
		[SaveableField(0)]
		public readonly CampaignVec2 Position;

		// Token: 0x040014FE RID: 5374
		[SaveableField(1)]
		public readonly CampaignTime DestroyTime;

		// Token: 0x040014FF RID: 5375
		[SaveableField(2)]
		public readonly BattleWreckage.WreckageType WreckageTypeCategory;

		// Token: 0x04001500 RID: 5376
		[SaveableField(3)]
		private bool _isInvestigated;

		// Token: 0x04001501 RID: 5377
		[SaveableField(4)]
		public readonly TextObject AttackerLeaderPartyName;

		// Token: 0x04001502 RID: 5378
		[SaveableField(5)]
		public readonly TextObject DefenderLeaderPartyName;

		// Token: 0x04001503 RID: 5379
		[SaveableField(6)]
		public readonly IFaction AttackerFaction;

		// Token: 0x04001504 RID: 5380
		[SaveableField(7)]
		public readonly IFaction DefenderFaction;

		// Token: 0x04001505 RID: 5381
		[SaveableField(8)]
		public readonly BattleSideEnum WinnerSide;

		// Token: 0x04001506 RID: 5382
		[SaveableField(9)]
		public readonly CampaignTime BattleStartTime;

		// Token: 0x04001507 RID: 5383
		[SaveableField(10)]
		public readonly int AttackerHealthyTroopCountAtStart;

		// Token: 0x04001508 RID: 5384
		[SaveableField(11)]
		public readonly int DefenderHealthyTroopCountAtStart;

		// Token: 0x04001509 RID: 5385
		[SaveableField(12)]
		public readonly TroopRoster AttackerDiedInBattle;

		// Token: 0x0400150A RID: 5386
		[SaveableField(13)]
		public readonly TroopRoster DefenderDiedInBattle;

		// Token: 0x0400150B RID: 5387
		[SaveableField(14)]
		public readonly TroopRoster AttackerWoundedInBattle;

		// Token: 0x0400150C RID: 5388
		[SaveableField(15)]
		public readonly TroopRoster DefenderWoundedInBattle;

		// Token: 0x0400150D RID: 5389
		[SaveableField(16)]
		public readonly Hero AttackerLeaderHero;

		// Token: 0x0400150E RID: 5390
		[SaveableField(17)]
		public readonly Hero DefenderLeaderHero;

		// Token: 0x020008CC RID: 2252
		public enum WreckageType
		{
			// Token: 0x0400262F RID: 9775
			Invalid,
			// Token: 0x04002630 RID: 9776
			Small,
			// Token: 0x04002631 RID: 9777
			Normal,
			// Token: 0x04002632 RID: 9778
			Epic
		}
	}
}
