using System;
using Helpers;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000408 RID: 1032
	public class DynamicBodyCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000E8B RID: 3723
		// (get) Token: 0x060040C4 RID: 16580 RVA: 0x0011D0B8 File Offset: 0x0011B2B8
		// (set) Token: 0x060040C5 RID: 16581 RVA: 0x0011D0D7 File Offset: 0x0011B2D7
		private CampaignTime LastSettlementVisitTime
		{
			get
			{
				if (Hero.MainHero.CurrentSettlement != null)
				{
					this._lastSettlementVisitTime = CampaignTime.Now;
				}
				return this._lastSettlementVisitTime;
			}
			set
			{
				this._lastSettlementVisitTime = value;
			}
		}

		// Token: 0x17000E8C RID: 3724
		// (get) Token: 0x060040C6 RID: 16582 RVA: 0x0011D0E0 File Offset: 0x0011B2E0
		private float MaxPlayerWeight
		{
			get
			{
				return MathF.Min(1f, this._unmodifiedWeight * 1.3f);
			}
		}

		// Token: 0x17000E8D RID: 3725
		// (get) Token: 0x060040C7 RID: 16583 RVA: 0x0011D0F8 File Offset: 0x0011B2F8
		private float MinPlayerWeight
		{
			get
			{
				return MathF.Max(0f, this._unmodifiedWeight * 0.7f);
			}
		}

		// Token: 0x17000E8E RID: 3726
		// (get) Token: 0x060040C8 RID: 16584 RVA: 0x0011D110 File Offset: 0x0011B310
		private float MaxPlayerBuild
		{
			get
			{
				return MathF.Min(1f, this._unmodifiedBuild * 1.3f);
			}
		}

		// Token: 0x17000E8F RID: 3727
		// (get) Token: 0x060040C9 RID: 16585 RVA: 0x0011D128 File Offset: 0x0011B328
		private float MinPlayerBuild
		{
			get
			{
				return MathF.Max(0f, this._unmodifiedBuild * 0.7f);
			}
		}

		// Token: 0x060040CA RID: 16586 RVA: 0x0011D140 File Offset: 0x0011B340
		private void DailyTick()
		{
			bool flag = this.LastSettlementVisitTime.ElapsedDaysUntilNow < 1f;
			bool flag2 = Hero.MainHero.PartyBelongedTo != null && Hero.MainHero.PartyBelongedTo.Party.IsStarving;
			float num = ((Hero.MainHero.CurrentSettlement == null && flag2) ? (-0.1f) : (flag ? 0.025f : (-0.025f)));
			Hero.MainHero.Weight = MBMath.ClampFloat(Hero.MainHero.Weight + num, this.MinPlayerWeight, this.MaxPlayerWeight);
			float num2 = ((MapEvent.PlayerMapEvent != null || PlayerSiege.PlayerSiegeEvent != null || this._lastEncounterTime.ElapsedDaysUntilNow < 2f) ? 0.025f : (-0.015f));
			Hero.MainHero.Build = MBMath.ClampFloat(Hero.MainHero.Build + num2, this.MinPlayerBuild, this.MaxPlayerBuild);
		}

		// Token: 0x060040CB RID: 16587 RVA: 0x0011D230 File Offset: 0x0011B430
		private void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
			if (party != null && party.IsMainParty)
			{
				this.LastSettlementVisitTime = CampaignTime.Now;
			}
		}

		// Token: 0x060040CC RID: 16588 RVA: 0x0011D248 File Offset: 0x0011B448
		private void OnMapEventEnded(MapEvent mapEvent)
		{
			if (mapEvent.IsPlayerMapEvent)
			{
				this._lastEncounterTime = CampaignTime.Now;
			}
		}

		// Token: 0x060040CD RID: 16589 RVA: 0x0011D25D File Offset: 0x0011B45D
		private void OnCharacterCreationOver(int index)
		{
			if (index == 1)
			{
				this.OnPlayerBodyPropertiesChanged();
			}
		}

		// Token: 0x060040CE RID: 16590 RVA: 0x0011D269 File Offset: 0x0011B469
		private void OnPlayerBodyPropertiesChanged()
		{
			this._unmodifiedBuild = Hero.MainHero.Build;
			this._unmodifiedWeight = Hero.MainHero.Weight;
		}

		// Token: 0x060040CF RID: 16591 RVA: 0x0011D28B File Offset: 0x0011B48B
		private void OnPlayerCharacterChanged(Hero oldPlayer, Hero newPlayer, MobileParty newMainParty, bool isMainPartyChanged)
		{
			this._unmodifiedBuild = newPlayer.Build;
			this._unmodifiedWeight = newPlayer.Weight;
		}

		// Token: 0x060040D0 RID: 16592 RVA: 0x0011D2A8 File Offset: 0x0011B4A8
		private void OnHeroCreated(Hero hero, bool bornNaturally)
		{
			if (!bornNaturally)
			{
				DynamicBodyProperties dynamicBodyPropertiesBetweenMinMaxRange = CharacterHelper.GetDynamicBodyPropertiesBetweenMinMaxRange(hero.CharacterObject);
				hero.Weight = dynamicBodyPropertiesBetweenMinMaxRange.Weight;
				hero.Build = dynamicBodyPropertiesBetweenMinMaxRange.Build;
			}
		}

		// Token: 0x060040D1 RID: 16593 RVA: 0x0011D2DC File Offset: 0x0011B4DC
		private void OnNewGameCreatedPartialFollowUpEnd(CampaignGameStarter starter)
		{
			this._lastSettlementVisitTime = CampaignTime.Now;
			this._lastEncounterTime = CampaignTime.Now;
			this.OnPlayerBodyPropertiesChanged();
		}

		// Token: 0x060040D2 RID: 16594 RVA: 0x0011D2FC File Offset: 0x0011B4FC
		public override void RegisterEvents()
		{
			CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
			CampaignEvents.HeroCreated.AddNonSerializedListener(this, new Action<Hero, bool>(this.OnHeroCreated));
			CampaignEvents.OnPlayerBodyPropertiesChangedEvent.AddNonSerializedListener(this, new Action(this.OnPlayerBodyPropertiesChanged));
			CampaignEvents.OnCharacterCreationIsOverEvent.AddNonSerializedListener(this, new Action<int>(this.OnCharacterCreationOver));
			CampaignEvents.OnPlayerCharacterChangedEvent.AddNonSerializedListener(this, new Action<Hero, Hero, MobileParty, bool>(this.OnPlayerCharacterChanged));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreatedPartialFollowUpEnd));
		}

		// Token: 0x060040D3 RID: 16595 RVA: 0x0011D3C4 File Offset: 0x0011B5C4
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<CampaignTime>("_lastSettlementVisitTime", ref this._lastSettlementVisitTime);
			dataStore.SyncData<CampaignTime>("_lastEncounterTime", ref this._lastEncounterTime);
			dataStore.SyncData<float>("_unmodifiedWeight", ref this._unmodifiedWeight);
			dataStore.SyncData<float>("_unmodifiedBuild", ref this._unmodifiedBuild);
		}

		// Token: 0x040013A3 RID: 5027
		private const float DailyBuildDecrease = -0.015f;

		// Token: 0x040013A4 RID: 5028
		private const float DailyBuildIncrease = 0.025f;

		// Token: 0x040013A5 RID: 5029
		private const float DailyWeightDecreaseWhenStarving = -0.1f;

		// Token: 0x040013A6 RID: 5030
		private const float DailyWeightDecreaseWhenNotStarving = -0.025f;

		// Token: 0x040013A7 RID: 5031
		private const float DailyWeightIncrease = 0.025f;

		// Token: 0x040013A8 RID: 5032
		private CampaignTime _lastSettlementVisitTime;

		// Token: 0x040013A9 RID: 5033
		private CampaignTime _lastEncounterTime;

		// Token: 0x040013AA RID: 5034
		private float _unmodifiedWeight = -1f;

		// Token: 0x040013AB RID: 5035
		private float _unmodifiedBuild = -1f;
	}
}
