using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x02000023 RID: 35
	public static class UISoundsHelper
	{
		// Token: 0x060000EF RID: 239 RVA: 0x00007883 File Offset: 0x00005A83
		public static void PlayUISound(string soundName)
		{
			SoundEvent.PlaySound2D(soundName);
		}

		// Token: 0x020000B1 RID: 177
		public static class DefaultSounds
		{
			// Token: 0x04000356 RID: 854
			public const string DefaultSound = "event:/ui/default";

			// Token: 0x04000357 RID: 855
			public const string CheckboxSound = "event:/ui/checkbox";

			// Token: 0x04000358 RID: 856
			public const string TabSound = "event:/ui/tab";

			// Token: 0x04000359 RID: 857
			public const string SortSound = "event:/ui/sort";

			// Token: 0x0400035A RID: 858
			public const string TransferSound = "event:/ui/transfer";
		}

		// Token: 0x020000B2 RID: 178
		public static class PanelSounds
		{
			// Token: 0x0400035B RID: 859
			public const string NextPanelSound = "event:/ui/panels/next";

			// Token: 0x0400035C RID: 860
			public const string InventoryPanelOpenSound = "event:/ui/panels/panel_inventory_open";

			// Token: 0x0400035D RID: 861
			public const string ClanPanelOpenSound = "event:/ui/panels/panel_clan_open";

			// Token: 0x0400035E RID: 862
			public const string CharacterPanelOpenSound = "event:/ui/panels/panel_character_open";

			// Token: 0x0400035F RID: 863
			public const string KingdomPanelOpenSound = "event:/ui/panels/panel_kingdom_open";

			// Token: 0x04000360 RID: 864
			public const string PartyPanelOpenSound = "event:/ui/panels/panel_party_open";

			// Token: 0x04000361 RID: 865
			public const string QuestPanelOpenSound = "event:/ui/panels/panel_quest_open";

			// Token: 0x04000362 RID: 866
			public const string CraftingPanelOpenSound = "event:/ui/panels/panel_settlement_enter_smithy";
		}

		// Token: 0x020000B3 RID: 179
		public static class SiegeSounds
		{
			// Token: 0x04000363 RID: 867
			public const string SiegeEngineClickSound = "event:/ui/panels/siege/engine_click";

			// Token: 0x04000364 RID: 868
			public const string SiegeEngineBuildCompleteSound = "event:/ui/panels/siege/engine_build_complete";
		}

		// Token: 0x020000B4 RID: 180
		public static class InventorySounds
		{
			// Token: 0x04000365 RID: 869
			public const string TakeAllSound = "event:/ui/inventory/take_all";
		}

		// Token: 0x020000B5 RID: 181
		public static class PartySounds
		{
			// Token: 0x04000366 RID: 870
			public const string UpgradeSound = "event:/ui/party/upgrade";

			// Token: 0x04000367 RID: 871
			public const string RecruitSound = "event:/ui/party/recruit_prisoner";
		}

		// Token: 0x020000B6 RID: 182
		public static class CraftingSounds
		{
			// Token: 0x04000368 RID: 872
			public const string RefineTabSound = "event:/ui/crafting/refine_tab";

			// Token: 0x04000369 RID: 873
			public const string CraftTabSound = "event:/ui/crafting/craft_tab";

			// Token: 0x0400036A RID: 874
			public const string SmeltTabSound = "event:/ui/crafting/smelt_tab";

			// Token: 0x0400036B RID: 875
			public const string RefineSuccessSound = "event:/ui/crafting/refine_success";

			// Token: 0x0400036C RID: 876
			public const string CraftSuccessSound = "event:/ui/crafting/craft_success";

			// Token: 0x0400036D RID: 877
			public const string SmeltSuccessSound = "event:/ui/crafting/smelt_success";
		}

		// Token: 0x020000B7 RID: 183
		public static class EndgameSounds
		{
			// Token: 0x0400036E RID: 878
			public const string ClanDestroyedSound = "event:/ui/endgame/end_clan_destroyed";

			// Token: 0x0400036F RID: 879
			public const string RetirementSound = "event:/ui/endgame/end_retirement";

			// Token: 0x04000370 RID: 880
			public const string VictorySound = "event:/ui/endgame/end_victory";
		}

		// Token: 0x020000B8 RID: 184
		public static class NotificationSounds
		{
			// Token: 0x04000371 RID: 881
			public const string HideoutFoundSound = "event:/ui/notification/hideout_found";
		}

		// Token: 0x020000B9 RID: 185
		public static class CampaignSounds
		{
			// Token: 0x04000372 RID: 882
			public const string PartySound = "event:/ui/campaign/click_party";

			// Token: 0x04000373 RID: 883
			public const string PartyEnemySound = "event:/ui/campaign/click_party_enemy";

			// Token: 0x04000374 RID: 884
			public const string SettlementSound = "event:/ui/campaign/click_settlement";

			// Token: 0x04000375 RID: 885
			public const string SettlementEnemySound = "event:/ui/campaign/click_settlement_enemy";
		}

		// Token: 0x020000BA RID: 186
		public static class MissionSounds
		{
			// Token: 0x04000376 RID: 886
			public const string DeploySound = "event:/ui/mission/deploy";
		}

		// Token: 0x020000BB RID: 187
		public static class MultiplayerSounds
		{
			// Token: 0x04000377 RID: 887
			public const string MatchReadySound = "event:/ui/multiplayer/match_ready";
		}

		// Token: 0x020000BC RID: 188
		public static class OrderOfBattleSounds
		{
			// Token: 0x04000378 RID: 888
			public const string ClassSelectionDropdownSound = "event:/ui/oob/dropdown";

			// Token: 0x04000379 RID: 889
			public const string OfficerPickSound = "event:/ui/oob/officer_pick";
		}

		// Token: 0x020000BD RID: 189
		public static class PortSounds
		{
			// Token: 0x0400037A RID: 890
			public const string ConfirmSound = "event:/ui/port/confirm_ship";

			// Token: 0x0400037B RID: 891
			public const string ChooseShipSound = "event:/ui/port/choose_ship";
		}

		// Token: 0x020000BE RID: 190
		public static class KingdomSounds
		{
			// Token: 0x0400037C RID: 892
			public const string ConfirmVoteSound = "event:/ui/reign/decision";
		}
	}
}
