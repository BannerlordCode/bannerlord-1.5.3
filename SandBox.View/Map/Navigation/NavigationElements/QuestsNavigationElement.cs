using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ScreenSystem;

namespace SandBox.View.Map.Navigation.NavigationElements
{
	// Token: 0x02000073 RID: 115
	public class QuestsNavigationElement : MapNavigationElementBase
	{
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060004F8 RID: 1272 RVA: 0x00026508 File Offset: 0x00024708
		public override string StringId
		{
			get
			{
				return "quest";
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x0002650F File Offset: 0x0002470F
		public override bool IsActive
		{
			get
			{
				return base._game.GameStateManager.ActiveState is QuestsState;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x00026529 File Offset: 0x00024729
		public override bool IsLockingNavigation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060004FB RID: 1275 RVA: 0x0002652C File Offset: 0x0002472C
		public override bool HasAlert
		{
			get
			{
				return this._viewDataTracker.IsQuestNotificationActive;
			}
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00026539 File Offset: 0x00024739
		public QuestsNavigationElement(MapNavigationHandler handler)
			: base(handler)
		{
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00026544 File Offset: 0x00024744
		protected override NavigationPermissionItem GetPermission()
		{
			if (!MapNavigationHelper.IsNavigationBarEnabled(this._handler))
			{
				return new NavigationPermissionItem(false, null);
			}
			if (this.IsActive)
			{
				return new NavigationPermissionItem(false, null);
			}
			Mission mission = Mission.Current;
			if (mission != null && !mission.IsQuestScreenAccessAllowed)
			{
				return new NavigationPermissionItem(false, null);
			}
			return new NavigationPermissionItem(true, null);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0002659C File Offset: 0x0002479C
		protected override TextObject GetTooltip()
		{
			if (!Input.IsGamepadActive && (base.Permission.IsAuthorized || this.IsActive))
			{
				string text = Game.Current.GameTextManager.GetHotKeyGameText("GenericCampaignPanelsGameKeyCategory", 42).ToString();
				TextObject textObject = GameTexts.FindText("str_hotkey_with_hint", null);
				textObject.SetTextVariable("TEXT", GameTexts.FindText("str_quest", null).ToString());
				textObject.SetTextVariable("HOTKEY", text);
				return textObject;
			}
			return GameTexts.FindText("str_quest", null);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00026624 File Offset: 0x00024824
		protected override TextObject GetAlertTooltip()
		{
			if (this.HasAlert)
			{
				return this._viewDataTracker.GetQuestNotificationText();
			}
			return TextObject.GetEmpty();
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0002663F File Offset: 0x0002483F
		public override void OpenView()
		{
			this.PrepareToOpenQuestsScreen(delegate
			{
				this.OpenQuestsAction();
			});
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00026654 File Offset: 0x00024854
		public override void OpenView(params object[] parameters)
		{
			if (parameters.Length != 0)
			{
				QuestsNavigationElement.<>c__DisplayClass13_0 CS$<>8__locals1 = new QuestsNavigationElement.<>c__DisplayClass13_0();
				CS$<>8__locals1.<>4__this = this;
				object obj = parameters[0];
				if ((CS$<>8__locals1.issue = obj as IssueBase) != null)
				{
					this.PrepareToOpenQuestsScreen(delegate
					{
						CS$<>8__locals1.<>4__this.OpenQuestsAction(CS$<>8__locals1.issue);
					});
					return;
				}
				QuestsNavigationElement.<>c__DisplayClass13_1 CS$<>8__locals2 = new QuestsNavigationElement.<>c__DisplayClass13_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				if ((CS$<>8__locals2.quest = obj as QuestBase) != null)
				{
					this.PrepareToOpenQuestsScreen(delegate
					{
						CS$<>8__locals2.CS$<>8__locals1.<>4__this.OpenQuestsAction(CS$<>8__locals2.quest);
					});
					return;
				}
				JournalLogEntry log;
				if ((log = obj as JournalLogEntry) != null)
				{
					this.PrepareToOpenQuestsScreen(delegate
					{
						CS$<>8__locals2.CS$<>8__locals1.<>4__this.OpenQuestsAction(log);
					});
					return;
				}
				Debug.FailedAssert(string.Format("Invalid parameter type when opening the quest screen from navigation: {0}", obj.GetType()), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.View\\Map\\Navigation\\NavigationElements\\QuestsNavigationElement.cs", "OpenView", 97);
			}
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00026728 File Offset: 0x00024928
		public override void GoToLink()
		{
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0002672C File Offset: 0x0002492C
		private void PrepareToOpenQuestsScreen(Action openQuestsAction)
		{
			if (base.Permission.IsAuthorized)
			{
				IChangeableScreen changeableScreen;
				if ((changeableScreen = ScreenManager.TopScreen as IChangeableScreen) != null && changeableScreen.AnyUnsavedChanges())
				{
					InformationManager.ShowInquiry(changeableScreen.CanChangesBeApplied() ? MapNavigationHelper.GetUnsavedChangedInquiry(openQuestsAction) : MapNavigationHelper.GetUnapplicableChangedInquiry(), false, false);
					return;
				}
				MapNavigationHelper.SwitchToANewScreen(openQuestsAction);
			}
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00026784 File Offset: 0x00024984
		private void OpenQuestsAction()
		{
			QuestsState questsState = base._game.GameStateManager.CreateState<QuestsState>();
			base._game.GameStateManager.PushState(questsState, 0);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x000267B4 File Offset: 0x000249B4
		private void OpenQuestsAction(IssueBase issue)
		{
			QuestsState questsState = base._game.GameStateManager.CreateState<QuestsState>(new object[] { issue });
			base._game.GameStateManager.PushState(questsState, 0);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x000267F0 File Offset: 0x000249F0
		private void OpenQuestsAction(QuestBase quest)
		{
			QuestsState questsState = base._game.GameStateManager.CreateState<QuestsState>(new object[] { quest });
			base._game.GameStateManager.PushState(questsState, 0);
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0002682C File Offset: 0x00024A2C
		private void OpenQuestsAction(JournalLogEntry log)
		{
			QuestsState questsState = base._game.GameStateManager.CreateState<QuestsState>(new object[] { log });
			base._game.GameStateManager.PushState(questsState, 0);
		}
	}
}
