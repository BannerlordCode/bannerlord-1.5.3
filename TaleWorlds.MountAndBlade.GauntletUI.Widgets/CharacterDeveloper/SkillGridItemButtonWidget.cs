using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterDeveloper
{
	// Token: 0x02000186 RID: 390
	public class SkillGridItemButtonWidget : ButtonWidget
	{
		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x0600146A RID: 5226 RVA: 0x00037DA6 File Offset: 0x00035FA6
		// (set) Token: 0x0600146B RID: 5227 RVA: 0x00037DAE File Offset: 0x00035FAE
		public Brush CannotLearnBrush { get; set; }

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x0600146C RID: 5228 RVA: 0x00037DB7 File Offset: 0x00035FB7
		// (set) Token: 0x0600146D RID: 5229 RVA: 0x00037DBF File Offset: 0x00035FBF
		public Brush CanLearnBrush { get; set; }

		// Token: 0x0600146E RID: 5230 RVA: 0x00037DC8 File Offset: 0x00035FC8
		public SkillGridItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x00037DD8 File Offset: 0x00035FD8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			Widget focusLevelWidget = this.FocusLevelWidget;
			if (focusLevelWidget != null)
			{
				focusLevelWidget.SetState(this.CurrentFocusLevel.ToString());
			}
			if (this._isVisualsDirty)
			{
				base.Brush = (this.CanLearnSkill ? this.CanLearnBrush : this.CannotLearnBrush);
				this._isVisualsDirty = false;
			}
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06001470 RID: 5232 RVA: 0x00037E36 File Offset: 0x00036036
		// (set) Token: 0x06001471 RID: 5233 RVA: 0x00037E3E File Offset: 0x0003603E
		public Widget FocusLevelWidget
		{
			get
			{
				return this._focusLevelWidget;
			}
			set
			{
				if (this._focusLevelWidget != value)
				{
					this._focusLevelWidget = value;
					base.OnPropertyChanged<Widget>(value, "FocusLevelWidget");
				}
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06001472 RID: 5234 RVA: 0x00037E5C File Offset: 0x0003605C
		// (set) Token: 0x06001473 RID: 5235 RVA: 0x00037E64 File Offset: 0x00036064
		public bool CanLearnSkill
		{
			get
			{
				return this._canLearnSkill;
			}
			set
			{
				if (this._canLearnSkill != value)
				{
					this._canLearnSkill = value;
					base.OnPropertyChanged(value, "CanLearnSkill");
					this._isVisualsDirty = true;
				}
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06001474 RID: 5236 RVA: 0x00037E89 File Offset: 0x00036089
		// (set) Token: 0x06001475 RID: 5237 RVA: 0x00037E91 File Offset: 0x00036091
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

		// Token: 0x04000950 RID: 2384
		private bool _isVisualsDirty = true;

		// Token: 0x04000951 RID: 2385
		private Widget _focusLevelWidget;

		// Token: 0x04000952 RID: 2386
		private int _currentFocusLevel;

		// Token: 0x04000953 RID: 2387
		private bool _canLearnSkill;
	}
}
