using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.GamepadNavigation;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterDeveloper
{
	// Token: 0x02000180 RID: 384
	public class CharacterDeveloperPerksContainerWidget : Widget
	{
		// Token: 0x06001418 RID: 5144 RVA: 0x00036D51 File Offset: 0x00034F51
		public CharacterDeveloperPerksContainerWidget(UIContext context)
			: base(context)
		{
			this._perkWidgets = new List<PerkItemButtonWidget>();
			this._navigationScopes = new List<GamepadNavigationScope>();
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x00036D78 File Offset: 0x00034F78
		private void RefreshScopes()
		{
			foreach (GamepadNavigationScope gamepadNavigationScope in this._navigationScopes)
			{
				base.GamepadNavigationContext.RemoveNavigationScope(gamepadNavigationScope);
			}
			this._navigationScopes.Clear();
			GamepadNavigationScope gamepadNavigationScope2 = this.BuildNewScope(this.FirstScopeID);
			this._navigationScopes.Add(gamepadNavigationScope2);
			base.GamepadNavigationContext.AddNavigationScope(gamepadNavigationScope2, true);
			int num = -1;
			if (this._perkWidgets.Count > 0)
			{
				num = this._perkWidgets[0].AlternativeType;
			}
			for (int i = 0; i < this._perkWidgets.Count; i++)
			{
				if (this._perkWidgets[i].AlternativeType == 0 || num == 0)
				{
					GamepadNavigationScope gamepadNavigationScope3 = this.BuildNewScope("Scope-" + i);
					this._navigationScopes.Add(gamepadNavigationScope3);
					base.GamepadNavigationContext.AddNavigationScope(gamepadNavigationScope3, true);
				}
				this._perkWidgets[i].GamepadNavigationIndex = 0;
				this._navigationScopes[this._navigationScopes.Count - 1].AddWidget(this._perkWidgets[i]);
				num = this._perkWidgets[i].AlternativeType;
			}
			for (int j = 0; j < this._navigationScopes.Count; j++)
			{
				List<Widget> list = this._navigationScopes[j].NavigatableWidgets.ToList<Widget>();
				list = list.OrderBy<Widget, int>((Widget w) => ((PerkItemButtonWidget)w).AlternativeType).ToList<Widget>();
				this._navigationScopes[j].ClearNavigatableWidgets();
				for (int k = 0; k < list.Count; k++)
				{
					list[k].GamepadNavigationIndex = k;
					this._navigationScopes[j].AddWidget(list[k]);
				}
				if (this._navigationScopes[j].NavigatableWidgets.Count > 1)
				{
					this._navigationScopes[j].AlternateMovementStepSize = MathF.Round((float)this._navigationScopes[j].NavigatableWidgets.Count / 2f);
					this._navigationScopes[j].AlternateScopeMovements = GamepadNavigationTypes.Vertical;
				}
				this._navigationScopes[j].DownNavigationScopeID = this.DownScopeID;
				this._navigationScopes[j].UpNavigationScopeID = this.UpScopeID;
				if (j == 0)
				{
					this._navigationScopes[j].LeftNavigationScopeID = this.LeftScopeID;
					if (this._navigationScopes.Count > 1)
					{
						this._navigationScopes[j].RightNavigationScopeID = this._navigationScopes[j + 1].ScopeID;
					}
				}
				else if (j == this._navigationScopes.Count - 1)
				{
					if (this._navigationScopes.Count > 1)
					{
						this._navigationScopes[j].LeftNavigationScopeID = this._navigationScopes[j - 1].ScopeID;
					}
					this._navigationScopes[j].RightNavigationScopeID = this.RightScopeID;
				}
				else if (j > 0 && j < this._navigationScopes.Count - 1)
				{
					this._navigationScopes[j].LeftNavigationScopeID = this._navigationScopes[j - 1].ScopeID;
					this._navigationScopes[j].RightNavigationScopeID = this._navigationScopes[j + 1].ScopeID;
				}
			}
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x00037144 File Offset: 0x00035344
		protected override void OnLateUpdate(float dt)
		{
			if (!this._initialized || this._lastPerkCount != this._perkWidgets.Count)
			{
				this.RefreshScopes();
				this._initialized = true;
				this._lastPerkCount = this._perkWidgets.Count;
			}
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x0003717F File Offset: 0x0003537F
		private GamepadNavigationScope BuildNewScope(string scopeID)
		{
			return new GamepadNavigationScope
			{
				ScopeID = scopeID,
				ParentWidget = this,
				ScopeMovements = GamepadNavigationTypes.Horizontal,
				DoNotAutomaticallyFindChildren = true
			};
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x000371A4 File Offset: 0x000353A4
		protected override void OnChildAdded(Widget child)
		{
			PerkItemButtonWidget perkItemButtonWidget;
			if ((perkItemButtonWidget = child as PerkItemButtonWidget) != null)
			{
				this._perkWidgets.Add(perkItemButtonWidget);
				this._initialized = false;
			}
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x000371D0 File Offset: 0x000353D0
		protected override void OnBeforeChildRemoved(Widget child)
		{
			PerkItemButtonWidget perkItemButtonWidget;
			if ((perkItemButtonWidget = child as PerkItemButtonWidget) != null)
			{
				this._perkWidgets.Remove(perkItemButtonWidget);
				this._initialized = false;
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x0600141E RID: 5150 RVA: 0x000371FB File Offset: 0x000353FB
		// (set) Token: 0x0600141F RID: 5151 RVA: 0x00037203 File Offset: 0x00035403
		public string LeftScopeID { get; set; }

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06001420 RID: 5152 RVA: 0x0003720C File Offset: 0x0003540C
		// (set) Token: 0x06001421 RID: 5153 RVA: 0x00037214 File Offset: 0x00035414
		public string RightScopeID { get; set; }

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06001422 RID: 5154 RVA: 0x0003721D File Offset: 0x0003541D
		// (set) Token: 0x06001423 RID: 5155 RVA: 0x00037225 File Offset: 0x00035425
		public string DownScopeID { get; set; }

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06001424 RID: 5156 RVA: 0x0003722E File Offset: 0x0003542E
		// (set) Token: 0x06001425 RID: 5157 RVA: 0x00037236 File Offset: 0x00035436
		public string UpScopeID { get; set; }

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06001426 RID: 5158 RVA: 0x0003723F File Offset: 0x0003543F
		// (set) Token: 0x06001427 RID: 5159 RVA: 0x00037247 File Offset: 0x00035447
		public string FirstScopeID { get; set; }

		// Token: 0x04000927 RID: 2343
		private List<GamepadNavigationScope> _navigationScopes;

		// Token: 0x04000928 RID: 2344
		private List<PerkItemButtonWidget> _perkWidgets;

		// Token: 0x04000929 RID: 2345
		private bool _initialized;

		// Token: 0x0400092A RID: 2346
		private int _lastPerkCount = -1;
	}
}
