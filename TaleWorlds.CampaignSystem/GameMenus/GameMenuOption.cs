using System;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameMenus
{
	// Token: 0x020000EF RID: 239
	public class GameMenuOption
	{
		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001638 RID: 5688 RVA: 0x00065C35 File Offset: 0x00063E35
		// (set) Token: 0x06001639 RID: 5689 RVA: 0x00065C3D File Offset: 0x00063E3D
		public GameMenu.MenuAndOptionType Type { get; private set; }

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x0600163A RID: 5690 RVA: 0x00065C46 File Offset: 0x00063E46
		// (set) Token: 0x0600163B RID: 5691 RVA: 0x00065C4E File Offset: 0x00063E4E
		public GameMenuOption.LeaveType OptionLeaveType { get; set; }

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x0600163C RID: 5692 RVA: 0x00065C57 File Offset: 0x00063E57
		// (set) Token: 0x0600163D RID: 5693 RVA: 0x00065C5F File Offset: 0x00063E5F
		public GameMenuOption.IssueQuestFlags OptionQuestData { get; set; }

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x0600163E RID: 5694 RVA: 0x00065C68 File Offset: 0x00063E68
		// (set) Token: 0x0600163F RID: 5695 RVA: 0x00065C70 File Offset: 0x00063E70
		public string IdString { get; private set; }

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06001640 RID: 5696 RVA: 0x00065C79 File Offset: 0x00063E79
		// (set) Token: 0x06001641 RID: 5697 RVA: 0x00065C81 File Offset: 0x00063E81
		public TextObject Text { get; private set; }

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06001642 RID: 5698 RVA: 0x00065C8A File Offset: 0x00063E8A
		// (set) Token: 0x06001643 RID: 5699 RVA: 0x00065C92 File Offset: 0x00063E92
		public TextObject Text2 { get; private set; }

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06001644 RID: 5700 RVA: 0x00065C9B File Offset: 0x00063E9B
		// (set) Token: 0x06001645 RID: 5701 RVA: 0x00065CA3 File Offset: 0x00063EA3
		public TextObject Tooltip { get; private set; }

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06001646 RID: 5702 RVA: 0x00065CAC File Offset: 0x00063EAC
		// (set) Token: 0x06001647 RID: 5703 RVA: 0x00065CB4 File Offset: 0x00063EB4
		public bool IsLeave { get; private set; }

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06001648 RID: 5704 RVA: 0x00065CBD File Offset: 0x00063EBD
		// (set) Token: 0x06001649 RID: 5705 RVA: 0x00065CC5 File Offset: 0x00063EC5
		public bool IsRepeatable { get; private set; }

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x0600164A RID: 5706 RVA: 0x00065CCE File Offset: 0x00063ECE
		// (set) Token: 0x0600164B RID: 5707 RVA: 0x00065CD6 File Offset: 0x00063ED6
		public bool IsEnabled { get; private set; }

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x0600164C RID: 5708 RVA: 0x00065CDF File Offset: 0x00063EDF
		// (set) Token: 0x0600164D RID: 5709 RVA: 0x00065CE7 File Offset: 0x00063EE7
		public object RelatedObject { get; private set; }

		// Token: 0x0600164E RID: 5710 RVA: 0x00065CF0 File Offset: 0x00063EF0
		internal GameMenuOption()
		{
			this.Text = null;
			this.Tooltip = null;
			this.IsEnabled = true;
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x00065D10 File Offset: 0x00063F10
		public GameMenuOption(GameMenu.MenuAndOptionType type, string idString, TextObject text, TextObject text2, GameMenuOption.OnConditionDelegate condition, GameMenuOption.OnConsequenceDelegate consequence, bool isLeave = false, bool isRepeatable = false, object relatedObject = null)
		{
			this.Type = type;
			this.IdString = idString;
			this.Text = text;
			this.Text2 = text2;
			this.OnCondition = condition;
			this.OnConsequence = consequence;
			this.Tooltip = null;
			this.IsRepeatable = isRepeatable;
			this.IsEnabled = true;
			this.IsLeave = isLeave;
			this.RelatedObject = relatedObject;
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x00065D78 File Offset: 0x00063F78
		public bool GetConditionsHold(Game game, MenuContext menuContext)
		{
			if (this.OnCondition != null)
			{
				MenuCallbackArgs menuCallbackArgs = new MenuCallbackArgs(menuContext, this.Text);
				bool flag = this.OnCondition(menuCallbackArgs);
				this.IsEnabled = menuCallbackArgs.IsEnabled;
				this.Tooltip = menuCallbackArgs.Tooltip;
				this.OptionQuestData = menuCallbackArgs.OptionQuestData;
				this.OptionLeaveType = menuCallbackArgs.optionLeaveType;
				return flag;
			}
			return true;
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x00065DD8 File Offset: 0x00063FD8
		public void RunConsequence(MenuContext menuContext)
		{
			if (this.OnConsequence != null)
			{
				MenuCallbackArgs menuCallbackArgs = new MenuCallbackArgs(menuContext, this.Text);
				this.OnConsequence(menuCallbackArgs);
			}
			menuContext.OnConsequence(this);
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x00065E0D File Offset: 0x0006400D
		public void SetEnable(bool isEnable)
		{
			this.IsEnabled = isEnable;
		}

		// Token: 0x04000746 RID: 1862
		public static GameMenuOption.IssueQuestFlags[] IssueQuestFlagsValues = (GameMenuOption.IssueQuestFlags[])Enum.GetValues(typeof(GameMenuOption.IssueQuestFlags));

		// Token: 0x0400074E RID: 1870
		public GameMenuOption.OnConditionDelegate OnCondition;

		// Token: 0x0400074F RID: 1871
		public GameMenuOption.OnConsequenceDelegate OnConsequence;

		// Token: 0x02000592 RID: 1426
		// (Invoke) Token: 0x060050C7 RID: 20679
		public delegate bool OnConditionDelegate(MenuCallbackArgs args);

		// Token: 0x02000593 RID: 1427
		// (Invoke) Token: 0x060050CB RID: 20683
		public delegate void OnConsequenceDelegate(MenuCallbackArgs args);

		// Token: 0x02000594 RID: 1428
		public enum LeaveType
		{
			// Token: 0x04001833 RID: 6195
			Default,
			// Token: 0x04001834 RID: 6196
			Mission,
			// Token: 0x04001835 RID: 6197
			Submenu,
			// Token: 0x04001836 RID: 6198
			BribeAndEscape,
			// Token: 0x04001837 RID: 6199
			Escape,
			// Token: 0x04001838 RID: 6200
			Craft,
			// Token: 0x04001839 RID: 6201
			ForceToGiveGoods,
			// Token: 0x0400183A RID: 6202
			ForceToGiveTroops,
			// Token: 0x0400183B RID: 6203
			Bribe,
			// Token: 0x0400183C RID: 6204
			LeaveTroopsAndFlee,
			// Token: 0x0400183D RID: 6205
			OrderTroopsToAttack,
			// Token: 0x0400183E RID: 6206
			Raid,
			// Token: 0x0400183F RID: 6207
			HostileAction,
			// Token: 0x04001840 RID: 6208
			Recruit,
			// Token: 0x04001841 RID: 6209
			Trade,
			// Token: 0x04001842 RID: 6210
			Wait,
			// Token: 0x04001843 RID: 6211
			Leave,
			// Token: 0x04001844 RID: 6212
			Continue,
			// Token: 0x04001845 RID: 6213
			Manage,
			// Token: 0x04001846 RID: 6214
			TroopSelection,
			// Token: 0x04001847 RID: 6215
			WaitQuest,
			// Token: 0x04001848 RID: 6216
			Surrender,
			// Token: 0x04001849 RID: 6217
			Conversation,
			// Token: 0x0400184A RID: 6218
			DefendAction,
			// Token: 0x0400184B RID: 6219
			Devastate,
			// Token: 0x0400184C RID: 6220
			Pillage,
			// Token: 0x0400184D RID: 6221
			ShowMercy,
			// Token: 0x0400184E RID: 6222
			Leaderboard,
			// Token: 0x0400184F RID: 6223
			OpenStash,
			// Token: 0x04001850 RID: 6224
			ManageGarrison,
			// Token: 0x04001851 RID: 6225
			StagePrisonBreak,
			// Token: 0x04001852 RID: 6226
			ManagePrisoners,
			// Token: 0x04001853 RID: 6227
			Ransom,
			// Token: 0x04001854 RID: 6228
			PracticeFight,
			// Token: 0x04001855 RID: 6229
			BesiegeTown,
			// Token: 0x04001856 RID: 6230
			SneakIn,
			// Token: 0x04001857 RID: 6231
			LeadAssault,
			// Token: 0x04001858 RID: 6232
			DonateTroops,
			// Token: 0x04001859 RID: 6233
			DonatePrisoners,
			// Token: 0x0400185A RID: 6234
			SiegeAmbush,
			// Token: 0x0400185B RID: 6235
			Warehouse,
			// Token: 0x0400185C RID: 6236
			VisitPort,
			// Token: 0x0400185D RID: 6237
			VisitTown,
			// Token: 0x0400185E RID: 6238
			SetSail,
			// Token: 0x0400185F RID: 6239
			ManageFleet,
			// Token: 0x04001860 RID: 6240
			CallFleet,
			// Token: 0x04001861 RID: 6241
			OrderShipsToAttack,
			// Token: 0x04001862 RID: 6242
			RepairShips,
			// Token: 0x04001863 RID: 6243
			TakeFerry
		}

		// Token: 0x02000595 RID: 1429
		[Flags]
		public enum IssueQuestFlags
		{
			// Token: 0x04001865 RID: 6245
			None = 0,
			// Token: 0x04001866 RID: 6246
			AvailableIssue = 1,
			// Token: 0x04001867 RID: 6247
			ActiveIssue = 2,
			// Token: 0x04001868 RID: 6248
			ActiveStoryQuest = 4,
			// Token: 0x04001869 RID: 6249
			TrackedIssue = 8,
			// Token: 0x0400186A RID: 6250
			TrackedStoryQuest = 16
		}
	}
}
