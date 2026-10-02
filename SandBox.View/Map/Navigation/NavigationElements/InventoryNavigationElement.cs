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
	// Token: 0x02000070 RID: 112
	public class InventoryNavigationElement : MapNavigationElementBase
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060004CE RID: 1230 RVA: 0x00025BC4 File Offset: 0x00023DC4
		public override string StringId
		{
			get
			{
				return "inventory";
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x00025BCB File Offset: 0x00023DCB
		public override bool IsActive
		{
			get
			{
				return base._game.GameStateManager.ActiveState is InventoryState;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060004D0 RID: 1232 RVA: 0x00025BE8 File Offset: 0x00023DE8
		public override bool IsLockingNavigation
		{
			get
			{
				GameStateManager gameStateManager = GameStateManager.Current;
				InventoryState inventoryState;
				return (inventoryState = ((gameStateManager != null) ? gameStateManager.ActiveState : null) as InventoryState) != null && inventoryState.InventoryLogic != null && inventoryState.InventoryMode != InventoryScreenHelper.InventoryMode.Default;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x00025C22 File Offset: 0x00023E22
		public override bool HasAlert
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00025C25 File Offset: 0x00023E25
		public InventoryNavigationElement(MapNavigationHandler handler)
			: base(handler)
		{
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00025C30 File Offset: 0x00023E30
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
			Mission mission = Mission.Current;
			if (mission != null && !mission.IsInventoryAccessAllowed)
			{
				return new NavigationPermissionItem(false, null);
			}
			return new NavigationPermissionItem(true, null);
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00025CA8 File Offset: 0x00023EA8
		protected override TextObject GetTooltip()
		{
			if (!Input.IsGamepadActive && (base.Permission.IsAuthorized || this.IsActive))
			{
				string text = Game.Current.GameTextManager.GetHotKeyGameText("GenericCampaignPanelsGameKeyCategory", 38).ToString();
				TextObject textObject = GameTexts.FindText("str_hotkey_with_hint", null);
				textObject.SetTextVariable("TEXT", GameTexts.FindText("str_inventory", null).ToString());
				textObject.SetTextVariable("HOTKEY", text);
				return textObject;
			}
			return GameTexts.FindText("str_inventory", null);
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00025D30 File Offset: 0x00023F30
		protected override TextObject GetAlertTooltip()
		{
			return TextObject.GetEmpty();
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00025D38 File Offset: 0x00023F38
		public override void OpenView()
		{
			if (base.Permission.IsAuthorized)
			{
				IChangeableScreen changeableScreen;
				if ((changeableScreen = ScreenManager.TopScreen as IChangeableScreen) != null && changeableScreen.AnyUnsavedChanges())
				{
					InquiryData inquiryData;
					if (!changeableScreen.CanChangesBeApplied())
					{
						inquiryData = MapNavigationHelper.GetUnapplicableChangedInquiry();
					}
					else
					{
						inquiryData = MapNavigationHelper.GetUnsavedChangedInquiry(delegate
						{
							InventoryScreenHelper.OpenScreenAsInventory(null);
						});
					}
					InformationManager.ShowInquiry(inquiryData, false, false);
					return;
				}
				MapNavigationHelper.SwitchToANewScreen(delegate
				{
					InventoryScreenHelper.OpenScreenAsInventory(null);
				});
			}
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00025DCA File Offset: 0x00023FCA
		public override void OpenView(params object[] parameters)
		{
			Debug.FailedAssert("Inventory screen shouldn't be opened with parameters from navigation", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.View\\Map\\Navigation\\NavigationElements\\InventoryNavigationElement.cs", "OpenView", 107);
			this.OpenView();
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00025DE8 File Offset: 0x00023FE8
		public override void GoToLink()
		{
		}
	}
}
