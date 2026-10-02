using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.View.Map.Navigation.NavigationElements
{
	// Token: 0x0200006F RID: 111
	public class EscapeMenuNavigationElement : MapNavigationElementBase
	{
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x00025A52 File Offset: 0x00023C52
		public override string StringId
		{
			get
			{
				return "escape_menu";
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x00025A59 File Offset: 0x00023C59
		public override bool IsActive
		{
			get
			{
				if (base._game.GameStateManager.ActiveState is MapState)
				{
					MapScreen instance = MapScreen.Instance;
					return instance != null && instance.IsEscapeMenuOpened;
				}
				return false;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00025A84 File Offset: 0x00023C84
		public override bool IsLockingNavigation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00025A87 File Offset: 0x00023C87
		public override bool HasAlert
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00025A8A File Offset: 0x00023C8A
		public EscapeMenuNavigationElement(MapNavigationHandler handler)
			: base(handler)
		{
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00025A94 File Offset: 0x00023C94
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
			return new NavigationPermissionItem(base._game.GameStateManager.ActiveState is MapState, null);
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00025AE4 File Offset: 0x00023CE4
		protected override TextObject GetTooltip()
		{
			if (!Input.IsGamepadActive && (base.Permission.IsAuthorized || this.IsActive))
			{
				string text = Game.Current.GameTextManager.GetHotKeyGameText("GenericPanelGameKeyCategory", "ToggleEscapeMenu").ToString();
				TextObject textObject = GameTexts.FindText("str_hotkey_with_hint", null);
				textObject.SetTextVariable("TEXT", GameTexts.FindText("str_escape_menu", null).ToString());
				textObject.SetTextVariable("HOTKEY", text);
				return textObject;
			}
			return GameTexts.FindText("str_escape_menu", null);
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x00025B6F File Offset: 0x00023D6F
		protected override TextObject GetAlertTooltip()
		{
			return TextObject.GetEmpty();
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00025B78 File Offset: 0x00023D78
		public override void OpenView()
		{
			if (base.Permission.IsAuthorized)
			{
				MapScreen instance = MapScreen.Instance;
				if (instance == null)
				{
					return;
				}
				instance.OpenEscapeMenu();
			}
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00025BA4 File Offset: 0x00023DA4
		public override void OpenView(params object[] parameters)
		{
			Debug.FailedAssert("Escape menu shouldn't be opened with parameters from navigation", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.View\\Map\\Navigation\\NavigationElements\\EscapeMenuNavigationElement.cs", "OpenView", 70);
			this.OpenView();
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00025BC2 File Offset: 0x00023DC2
		public override void GoToLink()
		{
		}
	}
}
