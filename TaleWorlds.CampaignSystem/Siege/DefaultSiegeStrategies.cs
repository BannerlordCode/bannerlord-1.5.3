using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Siege
{
	// Token: 0x020002F0 RID: 752
	public class DefaultSiegeStrategies
	{
		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x06002925 RID: 10533 RVA: 0x000ABA31 File Offset: 0x000A9C31
		private static DefaultSiegeStrategies Instance
		{
			get
			{
				return Campaign.Current.DefaultSiegeStrategies;
			}
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x06002926 RID: 10534 RVA: 0x000ABA3D File Offset: 0x000A9C3D
		public static SiegeStrategy PreserveStrength
		{
			get
			{
				return DefaultSiegeStrategies.Instance._preserveStrength;
			}
		}

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x06002927 RID: 10535 RVA: 0x000ABA49 File Offset: 0x000A9C49
		public static SiegeStrategy PrepareAgainstAssault
		{
			get
			{
				return DefaultSiegeStrategies.Instance._prepareAgainstAssault;
			}
		}

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x06002928 RID: 10536 RVA: 0x000ABA55 File Offset: 0x000A9C55
		public static SiegeStrategy CounterBombardment
		{
			get
			{
				return DefaultSiegeStrategies.Instance._counterBombardment;
			}
		}

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x06002929 RID: 10537 RVA: 0x000ABA61 File Offset: 0x000A9C61
		public static SiegeStrategy PrepareAssault
		{
			get
			{
				return DefaultSiegeStrategies.Instance._prepareAssault;
			}
		}

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x0600292A RID: 10538 RVA: 0x000ABA6D File Offset: 0x000A9C6D
		public static SiegeStrategy BreachWalls
		{
			get
			{
				return DefaultSiegeStrategies.Instance._breachWalls;
			}
		}

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x0600292B RID: 10539 RVA: 0x000ABA79 File Offset: 0x000A9C79
		public static SiegeStrategy WearOutDefenders
		{
			get
			{
				return DefaultSiegeStrategies.Instance._wearOutDefenders;
			}
		}

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x0600292C RID: 10540 RVA: 0x000ABA85 File Offset: 0x000A9C85
		public static SiegeStrategy Custom
		{
			get
			{
				return DefaultSiegeStrategies.Instance._custom;
			}
		}

		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x0600292D RID: 10541 RVA: 0x000ABA91 File Offset: 0x000A9C91
		public static IEnumerable<SiegeStrategy> AllAttackerStrategies
		{
			get
			{
				yield return DefaultSiegeStrategies.PrepareAssault;
				yield return DefaultSiegeStrategies.BreachWalls;
				yield return DefaultSiegeStrategies.WearOutDefenders;
				yield return DefaultSiegeStrategies.PreserveStrength;
				yield return DefaultSiegeStrategies.Custom;
				yield break;
			}
		}

		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x0600292E RID: 10542 RVA: 0x000ABA9A File Offset: 0x000A9C9A
		public static IEnumerable<SiegeStrategy> AllDefenderStrategies
		{
			get
			{
				yield return DefaultSiegeStrategies.PrepareAgainstAssault;
				yield return DefaultSiegeStrategies.CounterBombardment;
				yield return DefaultSiegeStrategies.PreserveStrength;
				yield return DefaultSiegeStrategies.Custom;
				yield break;
			}
		}

		// Token: 0x0600292F RID: 10543 RVA: 0x000ABAA3 File Offset: 0x000A9CA3
		public DefaultSiegeStrategies()
		{
			this.RegisterAll();
		}

		// Token: 0x06002930 RID: 10544 RVA: 0x000ABAB4 File Offset: 0x000A9CB4
		private void RegisterAll()
		{
			this._preserveStrength = this.Create("siege_strategy_preserve_strength");
			this._prepareAgainstAssault = this.Create("siege_strategy_prepare_against_assault");
			this._counterBombardment = this.Create("siege_strategy_counter_bombardment");
			this._prepareAssault = this.Create("siege_strategy_prepare_assault");
			this._breachWalls = this.Create("siege_strategy_breach_walls");
			this._wearOutDefenders = this.Create("siege_strategy_wear_out_defenders");
			this._custom = this.Create("siege_strategy_custom");
			this.InitializeAll();
		}

		// Token: 0x06002931 RID: 10545 RVA: 0x000ABB3E File Offset: 0x000A9D3E
		private SiegeStrategy Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<SiegeStrategy>(new SiegeStrategy(stringId));
		}

		// Token: 0x06002932 RID: 10546 RVA: 0x000ABB58 File Offset: 0x000A9D58
		private void InitializeAll()
		{
			this._custom.Initialize(new TextObject("{=!}Custom", null), new TextObject("{=!}Custom strategy that can be managed entirely.", null));
			this._preserveStrength.Initialize(new TextObject("{=!}Preserve Strength", null), new TextObject("{=!}Priority is set to preserving our strength.", null));
			this._prepareAgainstAssault.Initialize(new TextObject("{=!}Prepare Against Assault", null), new TextObject("{=!}Priority is set to keep advantage when the enemies' assault starts.", null));
			this._counterBombardment.Initialize(new TextObject("{=!}Counter Bombardment", null), new TextObject("{=!}Priority is set to countering enemy bombardment.", null));
			this._prepareAssault.Initialize(new TextObject("{=!}Prepare Assault", null), new TextObject("{=!}Priority is set to assaulting the walls.", null));
			this._breachWalls.Initialize(new TextObject("{=!}Breach Walls", null), new TextObject("{=!}Priority is set to breaching the walls.", null));
			this._wearOutDefenders.Initialize(new TextObject("{=!}Wear out Defenders", null), new TextObject("{=!}Priority is set to destroying engines of the enemy.", null));
		}

		// Token: 0x04000BE8 RID: 3048
		private SiegeStrategy _preserveStrength;

		// Token: 0x04000BE9 RID: 3049
		private SiegeStrategy _prepareAgainstAssault;

		// Token: 0x04000BEA RID: 3050
		private SiegeStrategy _counterBombardment;

		// Token: 0x04000BEB RID: 3051
		private SiegeStrategy _prepareAssault;

		// Token: 0x04000BEC RID: 3052
		private SiegeStrategy _breachWalls;

		// Token: 0x04000BED RID: 3053
		private SiegeStrategy _wearOutDefenders;

		// Token: 0x04000BEE RID: 3054
		private SiegeStrategy _custom;
	}
}
