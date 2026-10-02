using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.AdvancedStartOptions;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000061 RID: 97
	public class CampaignOptions
	{
		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000982 RID: 2434 RVA: 0x00029600 File Offset: 0x00027800
		private static CampaignOptions _current
		{
			get
			{
				Campaign campaign = Campaign.Current;
				if (campaign == null)
				{
					return null;
				}
				return campaign.Options;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000983 RID: 2435 RVA: 0x00029612 File Offset: 0x00027812
		public AdvancedStartOptionsData AdvancedStartOptionsData
		{
			get
			{
				return this._advancedStartOptionsData;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000984 RID: 2436 RVA: 0x0002961A File Offset: 0x0002781A
		public uint Seed
		{
			get
			{
				return this._seed;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x00029622 File Offset: 0x00027822
		public bool IsHighRebellionEnabled
		{
			get
			{
				return this._highRebellion;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000986 RID: 2438 RVA: 0x0002962A File Offset: 0x0002782A
		public bool IsRecruitmentRateModifierEnabled
		{
			get
			{
				return this._recruitmentRate;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000987 RID: 2439 RVA: 0x00029632 File Offset: 0x00027832
		public bool IsIncreasedGlobalMovementSpeedEnabled
		{
			get
			{
				return this._increasedGlobalMovementSpeed;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000988 RID: 2440 RVA: 0x0002963A File Offset: 0x0002783A
		public bool IsRisenBanditsEnabled
		{
			get
			{
				return this._risenBanditsEnabled;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x00029642 File Offset: 0x00027842
		// (set) Token: 0x0600098A RID: 2442 RVA: 0x00029654 File Offset: 0x00027854
		public static bool IsLifeDeathCycleDisabled
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				return current != null && current._isLifeDeathCycleDisabled;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._isLifeDeathCycleDisabled = value;
				}
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x00029668 File Offset: 0x00027868
		// (set) Token: 0x0600098C RID: 2444 RVA: 0x0002967A File Offset: 0x0002787A
		public static bool AutoAllocateClanMemberPerks
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				return current != null && current._autoAllocateClanMemberPerks;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._autoAllocateClanMemberPerks = value;
				}
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x0002968E File Offset: 0x0002788E
		// (set) Token: 0x0600098E RID: 2446 RVA: 0x000296A0 File Offset: 0x000278A0
		public static bool IsIronmanMode
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				return current != null && current._isIronmanMode;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._isIronmanMode = value;
				}
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x0600098F RID: 2447 RVA: 0x000296B4 File Offset: 0x000278B4
		// (set) Token: 0x06000990 RID: 2448 RVA: 0x000296C6 File Offset: 0x000278C6
		public static CampaignOptions.Difficulty PlayerTroopsReceivedDamage
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				if (current == null)
				{
					return CampaignOptions.Difficulty.Realistic;
				}
				return current._playerTroopsReceivedDamage;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._playerTroopsReceivedDamage = value;
				}
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000991 RID: 2449 RVA: 0x000296DA File Offset: 0x000278DA
		// (set) Token: 0x06000992 RID: 2450 RVA: 0x000296EC File Offset: 0x000278EC
		public static CampaignOptions.Difficulty RecruitmentDifficulty
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				if (current == null)
				{
					return CampaignOptions.Difficulty.Realistic;
				}
				return current._recruitmentDifficulty;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._recruitmentDifficulty = value;
				}
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000993 RID: 2451 RVA: 0x00029700 File Offset: 0x00027900
		// (set) Token: 0x06000994 RID: 2452 RVA: 0x00029712 File Offset: 0x00027912
		public static CampaignOptions.Difficulty PlayerMapMovementSpeed
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				if (current == null)
				{
					return CampaignOptions.Difficulty.Realistic;
				}
				return current._playerMapMovementSpeed;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._playerMapMovementSpeed = value;
				}
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000995 RID: 2453 RVA: 0x00029726 File Offset: 0x00027926
		// (set) Token: 0x06000996 RID: 2454 RVA: 0x00029738 File Offset: 0x00027938
		public static CampaignOptions.Difficulty StealthAndDisguiseDifficulty
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				if (current == null)
				{
					return CampaignOptions.Difficulty.Realistic;
				}
				return current._stealthAndDisguiseDifficulty;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._stealthAndDisguiseDifficulty = value;
				}
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000997 RID: 2455 RVA: 0x0002974C File Offset: 0x0002794C
		// (set) Token: 0x06000998 RID: 2456 RVA: 0x0002975E File Offset: 0x0002795E
		public static CampaignOptions.Difficulty CombatAIDifficulty
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				if (current == null)
				{
					return CampaignOptions.Difficulty.Realistic;
				}
				return current._combatAIDifficulty;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._combatAIDifficulty = value;
				}
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000999 RID: 2457 RVA: 0x00029772 File Offset: 0x00027972
		// (set) Token: 0x0600099A RID: 2458 RVA: 0x00029784 File Offset: 0x00027984
		public static CampaignOptions.Difficulty PersuasionSuccessChance
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				if (current == null)
				{
					return CampaignOptions.Difficulty.Realistic;
				}
				return current._persuasionSuccessChance;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._persuasionSuccessChance = value;
				}
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x0600099B RID: 2459 RVA: 0x00029798 File Offset: 0x00027998
		// (set) Token: 0x0600099C RID: 2460 RVA: 0x000297AA File Offset: 0x000279AA
		public static CampaignOptions.Difficulty ClanMemberDeathChance
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				if (current == null)
				{
					return CampaignOptions.Difficulty.Realistic;
				}
				return current._clanMemberDeathChance;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._clanMemberDeathChance = value;
				}
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x0600099D RID: 2461 RVA: 0x000297BE File Offset: 0x000279BE
		// (set) Token: 0x0600099E RID: 2462 RVA: 0x000297D0 File Offset: 0x000279D0
		public static CampaignOptions.Difficulty BattleDeath
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				if (current == null)
				{
					return CampaignOptions.Difficulty.Realistic;
				}
				return current._battleDeath;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._battleDeath = value;
				}
			}
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x000297E4 File Offset: 0x000279E4
		public CampaignOptions()
		{
			this._playerTroopsReceivedDamage = CampaignOptions.Difficulty.VeryEasy;
			this._recruitmentDifficulty = CampaignOptions.Difficulty.VeryEasy;
			this._playerMapMovementSpeed = CampaignOptions.Difficulty.VeryEasy;
			this._combatAIDifficulty = CampaignOptions.Difficulty.VeryEasy;
			this._persuasionSuccessChance = CampaignOptions.Difficulty.VeryEasy;
			this._clanMemberDeathChance = CampaignOptions.Difficulty.VeryEasy;
			this._battleDeath = CampaignOptions.Difficulty.VeryEasy;
			this._stealthAndDisguiseDifficulty = CampaignOptions.Difficulty.VeryEasy;
			this._isLifeDeathCycleDisabled = false;
			this._autoAllocateClanMemberPerks = false;
			this._isIronmanMode = false;
			this.AccelerationMode = GameAccelerationMode.Default;
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0002984C File Offset: 0x00027A4C
		public CampaignOptions(AdvancedStartOptionsData startOptions)
			: this()
		{
			this._advancedStartOptionsData = startOptions;
			if (!startOptions.TryGetSeed(out this._seed))
			{
				this._seed = (uint)Environment.TickCount;
			}
			this._highRebellion = startOptions.IsHighRebellionEnabled();
			this._recruitmentRate = startOptions.IsRecruitmentRateModifierEnabled();
			this._increasedGlobalMovementSpeed = startOptions.IsIncreasedGlobalMovementSpeedEnabled();
			this._risenBanditsEnabled = startOptions.IsRisenBanditsEnabled();
			this.AccelerationMode = (startOptions.IsFastModeEnabled() ? GameAccelerationMode.Fast : GameAccelerationMode.Default);
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x000298C1 File Offset: 0x00027AC1
		internal static void AutoGeneratedStaticCollectObjectsCampaignOptions(object o, List<object> collectedObjects)
		{
			((CampaignOptions)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x000298CF File Offset: 0x00027ACF
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			collectedObjects.Add(this._advancedStartOptionsData);
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x000298DD File Offset: 0x00027ADD
		internal static object AutoGeneratedGetMemberValueAccelerationMode(object o)
		{
			return ((CampaignOptions)o).AccelerationMode;
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x000298EF File Offset: 0x00027AEF
		internal static object AutoGeneratedGetMemberValue_advancedStartOptionsData(object o)
		{
			return ((CampaignOptions)o)._advancedStartOptionsData;
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x000298FC File Offset: 0x00027AFC
		internal static object AutoGeneratedGetMemberValue_autoAllocateClanMemberPerks(object o)
		{
			return ((CampaignOptions)o)._autoAllocateClanMemberPerks;
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x0002990E File Offset: 0x00027B0E
		internal static object AutoGeneratedGetMemberValue_playerTroopsReceivedDamage(object o)
		{
			return ((CampaignOptions)o)._playerTroopsReceivedDamage;
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x00029920 File Offset: 0x00027B20
		internal static object AutoGeneratedGetMemberValue_recruitmentDifficulty(object o)
		{
			return ((CampaignOptions)o)._recruitmentDifficulty;
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x00029932 File Offset: 0x00027B32
		internal static object AutoGeneratedGetMemberValue_playerMapMovementSpeed(object o)
		{
			return ((CampaignOptions)o)._playerMapMovementSpeed;
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x00029944 File Offset: 0x00027B44
		internal static object AutoGeneratedGetMemberValue_stealthAndDisguiseDifficulty(object o)
		{
			return ((CampaignOptions)o)._stealthAndDisguiseDifficulty;
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x00029956 File Offset: 0x00027B56
		internal static object AutoGeneratedGetMemberValue_combatAIDifficulty(object o)
		{
			return ((CampaignOptions)o)._combatAIDifficulty;
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x00029968 File Offset: 0x00027B68
		internal static object AutoGeneratedGetMemberValue_isLifeDeathCycleDisabled(object o)
		{
			return ((CampaignOptions)o)._isLifeDeathCycleDisabled;
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x0002997A File Offset: 0x00027B7A
		internal static object AutoGeneratedGetMemberValue_persuasionSuccessChance(object o)
		{
			return ((CampaignOptions)o)._persuasionSuccessChance;
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0002998C File Offset: 0x00027B8C
		internal static object AutoGeneratedGetMemberValue_clanMemberDeathChance(object o)
		{
			return ((CampaignOptions)o)._clanMemberDeathChance;
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0002999E File Offset: 0x00027B9E
		internal static object AutoGeneratedGetMemberValue_isIronmanMode(object o)
		{
			return ((CampaignOptions)o)._isIronmanMode;
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x000299B0 File Offset: 0x00027BB0
		internal static object AutoGeneratedGetMemberValue_battleDeath(object o)
		{
			return ((CampaignOptions)o)._battleDeath;
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x000299C2 File Offset: 0x00027BC2
		internal static object AutoGeneratedGetMemberValue_seed(object o)
		{
			return ((CampaignOptions)o)._seed;
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x000299D4 File Offset: 0x00027BD4
		internal static object AutoGeneratedGetMemberValue_risenBanditsEnabled(object o)
		{
			return ((CampaignOptions)o)._risenBanditsEnabled;
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x000299E6 File Offset: 0x00027BE6
		internal static object AutoGeneratedGetMemberValue_highRebellion(object o)
		{
			return ((CampaignOptions)o)._highRebellion;
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x000299F8 File Offset: 0x00027BF8
		internal static object AutoGeneratedGetMemberValue_recruitmentRate(object o)
		{
			return ((CampaignOptions)o)._recruitmentRate;
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x00029A0A File Offset: 0x00027C0A
		internal static object AutoGeneratedGetMemberValue_increasedGlobalMovementSpeed(object o)
		{
			return ((CampaignOptions)o)._increasedGlobalMovementSpeed;
		}

		// Token: 0x040002EE RID: 750
		[SaveableField(26)]
		private readonly AdvancedStartOptionsData _advancedStartOptionsData;

		// Token: 0x040002EF RID: 751
		[SaveableField(4)]
		private bool _autoAllocateClanMemberPerks;

		// Token: 0x040002F0 RID: 752
		[SaveableField(5)]
		private CampaignOptions.Difficulty _playerTroopsReceivedDamage;

		// Token: 0x040002F1 RID: 753
		[SaveableField(8)]
		private CampaignOptions.Difficulty _recruitmentDifficulty;

		// Token: 0x040002F2 RID: 754
		[SaveableField(9)]
		private CampaignOptions.Difficulty _playerMapMovementSpeed;

		// Token: 0x040002F3 RID: 755
		[SaveableField(18)]
		private CampaignOptions.Difficulty _stealthAndDisguiseDifficulty;

		// Token: 0x040002F4 RID: 756
		[SaveableField(11)]
		private CampaignOptions.Difficulty _combatAIDifficulty;

		// Token: 0x040002F5 RID: 757
		[SaveableField(12)]
		private bool _isLifeDeathCycleDisabled;

		// Token: 0x040002F6 RID: 758
		[SaveableField(13)]
		private CampaignOptions.Difficulty _persuasionSuccessChance;

		// Token: 0x040002F7 RID: 759
		[SaveableField(14)]
		private CampaignOptions.Difficulty _clanMemberDeathChance;

		// Token: 0x040002F8 RID: 760
		[SaveableField(15)]
		private bool _isIronmanMode;

		// Token: 0x040002F9 RID: 761
		[SaveableField(17)]
		private CampaignOptions.Difficulty _battleDeath;

		// Token: 0x040002FA RID: 762
		[SaveableField(19)]
		public GameAccelerationMode AccelerationMode;

		// Token: 0x040002FB RID: 763
		[SaveableField(20)]
		private readonly uint _seed;

		// Token: 0x040002FC RID: 764
		[SaveableField(21)]
		private readonly bool _risenBanditsEnabled;

		// Token: 0x040002FD RID: 765
		[SaveableField(22)]
		private readonly bool _highRebellion;

		// Token: 0x040002FE RID: 766
		[SaveableField(23)]
		private readonly bool _recruitmentRate;

		// Token: 0x040002FF RID: 767
		[SaveableField(24)]
		private readonly bool _increasedGlobalMovementSpeed;

		// Token: 0x02000544 RID: 1348
		public enum Difficulty : short
		{
			// Token: 0x04001712 RID: 5906
			VeryEasy,
			// Token: 0x04001713 RID: 5907
			Easy,
			// Token: 0x04001714 RID: 5908
			Realistic
		}
	}
}
