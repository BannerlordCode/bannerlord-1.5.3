using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ScreenSystem;

namespace SandBox.View.Map.Navigation.NavigationElements
{
	// Token: 0x0200006C RID: 108
	public class CharacterDeveloperNavigationElement : MapNavigationElementBase
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x000252E6 File Offset: 0x000234E6
		public override string StringId
		{
			get
			{
				return "character_developer";
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600049E RID: 1182 RVA: 0x000252ED File Offset: 0x000234ED
		public override bool IsActive
		{
			get
			{
				return base._game.GameStateManager.ActiveState is CharacterDeveloperState;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x00025307 File Offset: 0x00023507
		public override bool IsLockingNavigation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060004A0 RID: 1184 RVA: 0x0002530A File Offset: 0x0002350A
		public override bool HasAlert
		{
			get
			{
				return this._viewDataTracker.IsCharacterNotificationActive;
			}
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00025317 File Offset: 0x00023517
		public CharacterDeveloperNavigationElement(MapNavigationHandler handler)
			: base(handler)
		{
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00025320 File Offset: 0x00023520
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
			if (mission != null && !mission.IsCharacterWindowAccessAllowed)
			{
				return new NavigationPermissionItem(false, null);
			}
			return new NavigationPermissionItem(true, null);
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00025378 File Offset: 0x00023578
		protected override TextObject GetTooltip()
		{
			if (!Input.IsGamepadActive && (base.Permission.IsAuthorized || this.IsActive))
			{
				string text = Game.Current.GameTextManager.GetHotKeyGameText("GenericCampaignPanelsGameKeyCategory", 37).ToString();
				TextObject textObject = GameTexts.FindText("str_hotkey_with_hint", null);
				textObject.SetTextVariable("TEXT", GameTexts.FindText("str_character", null).ToString());
				textObject.SetTextVariable("HOTKEY", text);
				return textObject;
			}
			return GameTexts.FindText("str_character", null);
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00025400 File Offset: 0x00023600
		protected override TextObject GetAlertTooltip()
		{
			if (this.HasAlert)
			{
				return this._viewDataTracker.GetCharacterNotificationText();
			}
			return TextObject.GetEmpty();
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0002541B File Offset: 0x0002361B
		public override void OpenView()
		{
			this.PrepareToOpenCharacterDeveloper(delegate
			{
				this.OpenCharacterDeveloperScreenAction();
			});
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00025430 File Offset: 0x00023630
		public override void OpenView(params object[] parameters)
		{
			if (parameters.Length != 0)
			{
				object obj = parameters[0];
				Hero hero;
				if ((hero = obj as Hero) != null)
				{
					this.PrepareToOpenCharacterDeveloper(delegate
					{
						this.OpenCharacterDeveloperScreenAction(hero);
					});
					return;
				}
				Debug.FailedAssert(string.Format("Invalid parameter type when opening the character developer screen from navigation: {0}", obj.GetType()), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.View\\Map\\Navigation\\NavigationElements\\CharacterDeveloperNavigationElement.cs", "OpenView", 90);
			}
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00025497 File Offset: 0x00023697
		public override void GoToLink()
		{
			Campaign.Current.EncyclopediaManager.GoToLink(Hero.MainHero.EncyclopediaLink);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x000254B4 File Offset: 0x000236B4
		private void PrepareToOpenCharacterDeveloper(Action openCharacterDeveloperAction)
		{
			if (base.Permission.IsAuthorized)
			{
				IChangeableScreen changeableScreen;
				if ((changeableScreen = ScreenManager.TopScreen as IChangeableScreen) != null && changeableScreen.AnyUnsavedChanges())
				{
					InformationManager.ShowInquiry(changeableScreen.CanChangesBeApplied() ? MapNavigationHelper.GetUnsavedChangedInquiry(openCharacterDeveloperAction) : MapNavigationHelper.GetUnapplicableChangedInquiry(), false, false);
					return;
				}
				MapNavigationHelper.SwitchToANewScreen(openCharacterDeveloperAction);
			}
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0002550C File Offset: 0x0002370C
		private void OpenCharacterDeveloperScreenAction()
		{
			CharacterDeveloperState characterDeveloperState = base._game.GameStateManager.CreateState<CharacterDeveloperState>();
			base._game.GameStateManager.PushState(characterDeveloperState, 0);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0002553C File Offset: 0x0002373C
		private void OpenCharacterDeveloperScreenAction(Hero hero)
		{
			CharacterDeveloperState characterDeveloperState = base._game.GameStateManager.CreateState<CharacterDeveloperState>(new object[] { hero });
			base._game.GameStateManager.PushState(characterDeveloperState, 0);
		}
	}
}
