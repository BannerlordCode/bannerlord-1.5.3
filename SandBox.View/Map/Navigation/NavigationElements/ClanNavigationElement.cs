using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ScreenSystem;

namespace SandBox.View.Map.Navigation.NavigationElements
{
	// Token: 0x0200006D RID: 109
	public class ClanNavigationElement : MapNavigationElementBase
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x0002557E File Offset: 0x0002377E
		public override string StringId
		{
			get
			{
				return "clan";
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x00025585 File Offset: 0x00023785
		public override bool IsActive
		{
			get
			{
				return base._game.GameStateManager.ActiveState is ClanState;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x0002559F File Offset: 0x0002379F
		public override bool IsLockingNavigation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x000255A2 File Offset: 0x000237A2
		public override bool HasAlert
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x000255A5 File Offset: 0x000237A5
		public ClanNavigationElement(MapNavigationHandler handler)
			: base(handler)
		{
			this._clanScreenPermissionEvent = new ClanScreenPermissionEvent(new Action<bool, TextObject>(this.OnClanScreenPermission));
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x000255C8 File Offset: 0x000237C8
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
			if (mission != null && !mission.IsClanWindowAccessAllowed)
			{
				return new NavigationPermissionItem(false, null);
			}
			this._mostRecentClanScreenPermission = null;
			Game.Current.EventManager.TriggerEvent<ClanScreenPermissionEvent>(this._clanScreenPermissionEvent);
			NavigationPermissionItem? mostRecentClanScreenPermission = this._mostRecentClanScreenPermission;
			if (mostRecentClanScreenPermission == null)
			{
				return new NavigationPermissionItem(true, null);
			}
			return mostRecentClanScreenPermission.GetValueOrDefault();
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00025658 File Offset: 0x00023858
		protected override TextObject GetTooltip()
		{
			if (!Input.IsGamepadActive && (base.Permission.IsAuthorized || this.IsActive))
			{
				string text = Game.Current.GameTextManager.GetHotKeyGameText("GenericCampaignPanelsGameKeyCategory", 41).ToString();
				TextObject textObject = GameTexts.FindText("str_hotkey_with_hint", null);
				textObject.SetTextVariable("TEXT", GameTexts.FindText("str_clan", null).ToString());
				textObject.SetTextVariable("HOTKEY", text);
				return textObject;
			}
			return GameTexts.FindText("str_clan", null);
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x000256E0 File Offset: 0x000238E0
		protected override TextObject GetAlertTooltip()
		{
			return TextObject.GetEmpty();
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x000256E7 File Offset: 0x000238E7
		public override void OpenView()
		{
			this.PrepareToOpenClanScreen(delegate
			{
				this.OpenClanScreenAction();
			});
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x000256FC File Offset: 0x000238FC
		public override void OpenView(params object[] parameters)
		{
			if (parameters.Length != 0)
			{
				ClanNavigationElement.<>c__DisplayClass15_0 CS$<>8__locals1 = new ClanNavigationElement.<>c__DisplayClass15_0();
				CS$<>8__locals1.<>4__this = this;
				object obj = parameters[0];
				if ((CS$<>8__locals1.hero = obj as Hero) != null)
				{
					this.PrepareToOpenClanScreen(delegate
					{
						CS$<>8__locals1.<>4__this.OpenClanScreenAction(CS$<>8__locals1.hero);
					});
					return;
				}
				ClanNavigationElement.<>c__DisplayClass15_1 CS$<>8__locals2 = new ClanNavigationElement.<>c__DisplayClass15_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				if ((CS$<>8__locals2.party = obj as PartyBase) != null)
				{
					this.PrepareToOpenClanScreen(delegate
					{
						CS$<>8__locals2.CS$<>8__locals1.<>4__this.OpenClanScreenAction(CS$<>8__locals2.party);
					});
					return;
				}
				ClanNavigationElement.<>c__DisplayClass15_2 CS$<>8__locals3 = new ClanNavigationElement.<>c__DisplayClass15_2();
				CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals2;
				if ((CS$<>8__locals3.settlement = obj as Settlement) != null)
				{
					this.PrepareToOpenClanScreen(delegate
					{
						CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.OpenClanScreenAction(CS$<>8__locals3.settlement);
					});
					return;
				}
				ClanNavigationElement.<>c__DisplayClass15_3 CS$<>8__locals4 = new ClanNavigationElement.<>c__DisplayClass15_3();
				CS$<>8__locals4.CS$<>8__locals3 = CS$<>8__locals3;
				if ((CS$<>8__locals4.workshop = obj as Workshop) != null)
				{
					this.PrepareToOpenClanScreen(delegate
					{
						CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.OpenClanScreenAction(CS$<>8__locals4.workshop);
					});
					return;
				}
				Alley alley;
				if ((alley = obj as Alley) != null)
				{
					this.PrepareToOpenClanScreen(delegate
					{
						CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.OpenClanScreenAction(alley);
					});
					return;
				}
				Debug.FailedAssert(string.Format("Invalid parameter type when opening the clan screen from navigation: {0}", obj.GetType()), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.View\\Map\\Navigation\\NavigationElements\\ClanNavigationElement.cs", "OpenView", 110);
			}
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00025840 File Offset: 0x00023A40
		public override void GoToLink()
		{
			Campaign.Current.EncyclopediaManager.GoToLink(Hero.MainHero.Clan.EncyclopediaLink);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00025860 File Offset: 0x00023A60
		public void OnClanScreenPermission(bool isAvailable, TextObject reasonString)
		{
			if (!isAvailable)
			{
				this._mostRecentClanScreenPermission = new NavigationPermissionItem?(new NavigationPermissionItem(isAvailable, reasonString));
			}
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00025878 File Offset: 0x00023A78
		private void PrepareToOpenClanScreen(Action openClanScreenAction)
		{
			if (base.Permission.IsAuthorized)
			{
				IChangeableScreen changeableScreen;
				if ((changeableScreen = ScreenManager.TopScreen as IChangeableScreen) != null && changeableScreen.AnyUnsavedChanges())
				{
					InformationManager.ShowInquiry(changeableScreen.CanChangesBeApplied() ? MapNavigationHelper.GetUnsavedChangedInquiry(openClanScreenAction) : MapNavigationHelper.GetUnapplicableChangedInquiry(), false, false);
					return;
				}
				MapNavigationHelper.SwitchToANewScreen(openClanScreenAction);
			}
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x000258D0 File Offset: 0x00023AD0
		private void OpenClanScreenAction()
		{
			ClanState clanState = base._game.GameStateManager.CreateState<ClanState>();
			base._game.GameStateManager.PushState(clanState, 0);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00025900 File Offset: 0x00023B00
		private void OpenClanScreenAction(Hero hero)
		{
			ClanState clanState = base._game.GameStateManager.CreateState<ClanState>(new object[] { hero });
			base._game.GameStateManager.PushState(clanState, 0);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0002593C File Offset: 0x00023B3C
		private void OpenClanScreenAction(PartyBase party)
		{
			ClanState clanState = base._game.GameStateManager.CreateState<ClanState>(new object[] { party });
			base._game.GameStateManager.PushState(clanState, 0);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00025978 File Offset: 0x00023B78
		private void OpenClanScreenAction(Settlement settlement)
		{
			ClanState clanState = base._game.GameStateManager.CreateState<ClanState>(new object[] { settlement });
			base._game.GameStateManager.PushState(clanState, 0);
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x000259B4 File Offset: 0x00023BB4
		private void OpenClanScreenAction(Workshop workshop)
		{
			ClanState clanState = base._game.GameStateManager.CreateState<ClanState>(new object[] { workshop });
			base._game.GameStateManager.PushState(clanState, 0);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x000259F0 File Offset: 0x00023BF0
		private void OpenClanScreenAction(Alley alley)
		{
			ClanState clanState = base._game.GameStateManager.CreateState<ClanState>(new object[] { alley });
			base._game.GameStateManager.PushState(clanState, 0);
		}

		// Token: 0x0400023B RID: 571
		private readonly ClanScreenPermissionEvent _clanScreenPermissionEvent;

		// Token: 0x0400023C RID: 572
		private NavigationPermissionItem? _mostRecentClanScreenPermission;
	}
}
