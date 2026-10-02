using System;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ScreenSystem;

namespace SandBox.View.Map.Navigation.NavigationElements
{
	// Token: 0x02000072 RID: 114
	public class PartyNavigationElement : MapNavigationElementBase
	{
		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x000262DA File Offset: 0x000244DA
		public override string StringId
		{
			get
			{
				return "party";
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060004EE RID: 1262 RVA: 0x000262E1 File Offset: 0x000244E1
		public override bool IsActive
		{
			get
			{
				return base._game.GameStateManager.ActiveState is PartyState;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x000262FC File Offset: 0x000244FC
		public override bool IsLockingNavigation
		{
			get
			{
				GameStateManager gameStateManager = GameStateManager.Current;
				PartyState partyState;
				return (partyState = ((gameStateManager != null) ? gameStateManager.ActiveState : null) as PartyState) != null && partyState.PartyScreenLogic != null && partyState.PartyScreenMode != PartyScreenHelper.PartyScreenMode.Normal;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x00026336 File Offset: 0x00024536
		public override bool HasAlert
		{
			get
			{
				return this._viewDataTracker.IsPartyNotificationActive;
			}
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00026343 File Offset: 0x00024543
		public PartyNavigationElement(MapNavigationHandler handler)
			: base(handler)
		{
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0002634C File Offset: 0x0002454C
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
			if (MobileParty.MainParty.IsInNavalAutoTravel || Hero.MainHero.HeroState == Hero.CharacterStates.Prisoner)
			{
				return new NavigationPermissionItem(false, null);
			}
			if (MobileParty.MainParty.MapEvent != null)
			{
				return new NavigationPermissionItem(false, null);
			}
			Mission mission = Mission.Current;
			if (mission != null && !mission.IsPartyWindowAccessAllowed)
			{
				return new NavigationPermissionItem(false, null);
			}
			return new NavigationPermissionItem(true, null);
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x000263D8 File Offset: 0x000245D8
		protected override TextObject GetTooltip()
		{
			if (!Input.IsGamepadActive && (base.Permission.IsAuthorized || this.IsActive))
			{
				string text = Game.Current.GameTextManager.GetHotKeyGameText("GenericCampaignPanelsGameKeyCategory", 43).ToString();
				TextObject textObject = GameTexts.FindText("str_hotkey_with_hint", null);
				textObject.SetTextVariable("TEXT", GameTexts.FindText("str_party", null).ToString());
				textObject.SetTextVariable("HOTKEY", text);
				return textObject;
			}
			return GameTexts.FindText("str_party", null);
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00026460 File Offset: 0x00024660
		protected override TextObject GetAlertTooltip()
		{
			if (this.HasAlert)
			{
				return this._viewDataTracker.GetPartyNotificationText();
			}
			return TextObject.GetEmpty();
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0002647C File Offset: 0x0002467C
		public override void OpenView()
		{
			if (base.Permission.IsAuthorized)
			{
				IChangeableScreen changeableScreen;
				if ((changeableScreen = ScreenManager.TopScreen as IChangeableScreen) != null && changeableScreen.AnyUnsavedChanges())
				{
					InformationManager.ShowInquiry(changeableScreen.CanChangesBeApplied() ? MapNavigationHelper.GetUnsavedChangedInquiry(new Action(PartyScreenHelper.OpenScreenAsNormal)) : MapNavigationHelper.GetUnapplicableChangedInquiry(), false, false);
					return;
				}
				MapNavigationHelper.SwitchToANewScreen(new Action(PartyScreenHelper.OpenScreenAsNormal));
			}
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x000264E8 File Offset: 0x000246E8
		public override void OpenView(params object[] parameters)
		{
			Debug.FailedAssert("Party screen shouldn't be opened with parameters from navigation", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.View\\Map\\Navigation\\NavigationElements\\PartyNavigationElement.cs", "OpenView", 119);
			this.OpenView();
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00026506 File Offset: 0x00024706
		public override void GoToLink()
		{
		}
	}
}
