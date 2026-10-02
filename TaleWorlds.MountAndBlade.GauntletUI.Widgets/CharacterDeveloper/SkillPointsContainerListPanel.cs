using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterDeveloper
{
	// Token: 0x02000187 RID: 391
	public class SkillPointsContainerListPanel : ListPanel
	{
		// Token: 0x06001476 RID: 5238 RVA: 0x00037EAF File Offset: 0x000360AF
		public SkillPointsContainerListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x00037EB8 File Offset: 0x000360B8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			for (int i = 0; i < base.ChildCount; i++)
			{
				if (!this._initialized)
				{
					base.GetChild(i).RegisterBrushStatesOfWidget();
				}
				bool flag = this.CurrentFocusLevel >= i + 1;
				base.GetChild(i).SetState(flag ? "Full" : "Empty");
			}
			this._initialized = true;
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06001478 RID: 5240 RVA: 0x00037F22 File Offset: 0x00036122
		// (set) Token: 0x06001479 RID: 5241 RVA: 0x00037F2A File Offset: 0x0003612A
		public int CurrentFocusLevel
		{
			get
			{
				return this._currentFocusLevel;
			}
			set
			{
				if (this._currentFocusLevel != value)
				{
					this._currentFocusLevel = value;
					base.OnPropertyChanged(value, "CurrentFocusLevel");
				}
			}
		}

		// Token: 0x04000954 RID: 2388
		private bool _initialized;

		// Token: 0x04000955 RID: 2389
		private int _currentFocusLevel;
	}
}
