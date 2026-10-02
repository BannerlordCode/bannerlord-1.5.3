using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ScreenSystem;

namespace SandBox.View.Map.Navigation.NavigationElements
{
	// Token: 0x02000071 RID: 113
	public class KingdomNavigationElement : MapNavigationElementBase
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x00025DEA File Offset: 0x00023FEA
		public override string StringId
		{
			get
			{
				return "kingdom";
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060004DA RID: 1242 RVA: 0x00025DF1 File Offset: 0x00023FF1
		public override bool IsActive
		{
			get
			{
				return base._game.GameStateManager.ActiveState is KingdomState;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x00025E0B File Offset: 0x0002400B
		public override bool IsLockingNavigation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x00025E0E File Offset: 0x0002400E
		public override bool HasAlert
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x00025E11 File Offset: 0x00024011
		public KingdomNavigationElement(MapNavigationHandler handler)
			: base(handler)
		{
			this._needToBeInKingdomText = GameTexts.FindText("str_need_to_be_a_part_of_kingdom", null);
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00025E2C File Offset: 0x0002402C
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
			if (!Hero.MainHero.MapFaction.IsKingdomFaction)
			{
				return new NavigationPermissionItem(false, this._needToBeInKingdomText);
			}
			Mission mission = Mission.Current;
			if (mission != null && !mission.IsKingdomWindowAccessAllowed)
			{
				return new NavigationPermissionItem(false, null);
			}
			return new NavigationPermissionItem(true, null);
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00025EA4 File Offset: 0x000240A4
		protected override TextObject GetTooltip()
		{
			if (!Input.IsGamepadActive && (base.Permission.IsAuthorized || this.IsActive))
			{
				string text = Game.Current.GameTextManager.GetHotKeyGameText("GenericCampaignPanelsGameKeyCategory", 40).ToString();
				TextObject textObject = GameTexts.FindText("str_hotkey_with_hint", null);
				textObject.SetTextVariable("TEXT", GameTexts.FindText("str_kingdom", null).ToString());
				textObject.SetTextVariable("HOTKEY", text);
				return textObject;
			}
			return GameTexts.FindText("str_kingdom", null);
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00025F2C File Offset: 0x0002412C
		protected override TextObject GetAlertTooltip()
		{
			return TextObject.GetEmpty();
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00025F33 File Offset: 0x00024133
		public override void OpenView()
		{
			this.PrepareToOpenKingdomScreen(delegate
			{
				this.OpenKingdomAction();
			});
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00025F48 File Offset: 0x00024148
		public override void OpenView(params object[] parameters)
		{
			if (parameters.Length != 0)
			{
				KingdomNavigationElement.<>c__DisplayClass14_0 CS$<>8__locals1 = new KingdomNavigationElement.<>c__DisplayClass14_0();
				CS$<>8__locals1.<>4__this = this;
				object obj = parameters[0];
				if ((CS$<>8__locals1.army = obj as Army) != null)
				{
					this.PrepareToOpenKingdomScreen(delegate
					{
						CS$<>8__locals1.<>4__this.OpenKingdomAction(CS$<>8__locals1.army);
					});
					return;
				}
				KingdomNavigationElement.<>c__DisplayClass14_1 CS$<>8__locals2 = new KingdomNavigationElement.<>c__DisplayClass14_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				if ((CS$<>8__locals2.settlement = obj as Settlement) != null)
				{
					this.PrepareToOpenKingdomScreen(delegate
					{
						CS$<>8__locals2.CS$<>8__locals1.<>4__this.OpenKingdomAction(CS$<>8__locals2.settlement);
					});
					return;
				}
				KingdomNavigationElement.<>c__DisplayClass14_2 CS$<>8__locals3 = new KingdomNavigationElement.<>c__DisplayClass14_2();
				CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals2;
				if ((CS$<>8__locals3.clan = obj as Clan) != null)
				{
					this.PrepareToOpenKingdomScreen(delegate
					{
						CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.OpenKingdomAction(CS$<>8__locals3.clan);
					});
					return;
				}
				KingdomNavigationElement.<>c__DisplayClass14_3 CS$<>8__locals4 = new KingdomNavigationElement.<>c__DisplayClass14_3();
				CS$<>8__locals4.CS$<>8__locals3 = CS$<>8__locals3;
				if ((CS$<>8__locals4.policy = obj as PolicyObject) != null)
				{
					this.PrepareToOpenKingdomScreen(delegate
					{
						CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.OpenKingdomAction(CS$<>8__locals4.policy);
					});
					return;
				}
				KingdomNavigationElement.<>c__DisplayClass14_4 CS$<>8__locals5 = new KingdomNavigationElement.<>c__DisplayClass14_4();
				CS$<>8__locals5.CS$<>8__locals4 = CS$<>8__locals4;
				if ((CS$<>8__locals5.faction = obj as IFaction) != null)
				{
					this.PrepareToOpenKingdomScreen(delegate
					{
						CS$<>8__locals5.CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.OpenKingdomAction(CS$<>8__locals5.faction);
					});
					return;
				}
				KingdomDecision decision;
				if ((decision = obj as KingdomDecision) != null)
				{
					this.PrepareToOpenKingdomScreen(delegate
					{
						CS$<>8__locals5.CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.OpenKingdomAction(decision);
					});
					return;
				}
				Debug.FailedAssert(string.Format("Invalid parameter type when opening the kingdom screen from navigation: {0}", obj.GetType()), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.View\\Map\\Navigation\\NavigationElements\\KindomNavigationElement.cs", "OpenView", 113);
			}
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x000260C4 File Offset: 0x000242C4
		public override void GoToLink()
		{
			Campaign.Current.EncyclopediaManager.GoToLink(Hero.MainHero.MapFaction.EncyclopediaLink);
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x000260E4 File Offset: 0x000242E4
		private void PrepareToOpenKingdomScreen(Action openKingdomAction)
		{
			if (base.Permission.IsAuthorized)
			{
				IChangeableScreen changeableScreen;
				if ((changeableScreen = ScreenManager.TopScreen as IChangeableScreen) != null && changeableScreen.AnyUnsavedChanges())
				{
					InformationManager.ShowInquiry(changeableScreen.CanChangesBeApplied() ? MapNavigationHelper.GetUnsavedChangedInquiry(openKingdomAction) : MapNavigationHelper.GetUnapplicableChangedInquiry(), false, false);
					return;
				}
				MapNavigationHelper.SwitchToANewScreen(openKingdomAction);
			}
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0002613C File Offset: 0x0002433C
		private void OpenKingdomAction()
		{
			KingdomState kingdomState = base._game.GameStateManager.CreateState<KingdomState>();
			base._game.GameStateManager.PushState(kingdomState, 0);
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0002616C File Offset: 0x0002436C
		private void OpenKingdomAction(Army army)
		{
			KingdomState kingdomState = base._game.GameStateManager.CreateState<KingdomState>(new object[] { army });
			base._game.GameStateManager.PushState(kingdomState, 0);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x000261A8 File Offset: 0x000243A8
		private void OpenKingdomAction(Settlement settlement)
		{
			KingdomState kingdomState = base._game.GameStateManager.CreateState<KingdomState>(new object[] { settlement });
			base._game.GameStateManager.PushState(kingdomState, 0);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x000261E4 File Offset: 0x000243E4
		private void OpenKingdomAction(Clan clan)
		{
			KingdomState kingdomState = base._game.GameStateManager.CreateState<KingdomState>(new object[] { clan });
			base._game.GameStateManager.PushState(kingdomState, 0);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00026220 File Offset: 0x00024420
		private void OpenKingdomAction(PolicyObject policy)
		{
			KingdomState kingdomState = base._game.GameStateManager.CreateState<KingdomState>(new object[] { policy });
			base._game.GameStateManager.PushState(kingdomState, 0);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0002625C File Offset: 0x0002445C
		private void OpenKingdomAction(IFaction faction)
		{
			KingdomState kingdomState = base._game.GameStateManager.CreateState<KingdomState>(new object[] { faction });
			base._game.GameStateManager.PushState(kingdomState, 0);
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00026298 File Offset: 0x00024498
		private void OpenKingdomAction(KingdomDecision decision)
		{
			KingdomState kingdomState = base._game.GameStateManager.CreateState<KingdomState>(new object[] { decision });
			base._game.GameStateManager.PushState(kingdomState, 0);
		}

		// Token: 0x0400023E RID: 574
		private readonly TextObject _needToBeInKingdomText;
	}
}
