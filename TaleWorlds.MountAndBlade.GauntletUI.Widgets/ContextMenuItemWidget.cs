using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000013 RID: 19
	public class ContextMenuItemWidget : Widget
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00004C26 File Offset: 0x00002E26
		// (set) Token: 0x06000101 RID: 257 RVA: 0x00004C2E File Offset: 0x00002E2E
		public Widget TypeIconWidget { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00004C37 File Offset: 0x00002E37
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00004C3F File Offset: 0x00002E3F
		public ButtonWidget ActionButtonWidget { get; set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00004C48 File Offset: 0x00002E48
		// (set) Token: 0x06000105 RID: 261 RVA: 0x00004C50 File Offset: 0x00002E50
		public string TypeIconState { get; set; }

		// Token: 0x06000106 RID: 262 RVA: 0x00004C59 File Offset: 0x00002E59
		public ContextMenuItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00004C6C File Offset: 0x00002E6C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._isInitialized)
			{
				if (this.TypeIconWidget != null && !string.IsNullOrEmpty(this.TypeIconState))
				{
					this.TypeIconWidget.RegisterBrushStatesOfWidget();
					this.TypeIconWidget.SetState(this.TypeIconState);
				}
				this._isInitialized = true;
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00004CC0 File Offset: 0x00002EC0
		protected override void RefreshState()
		{
			base.RefreshState();
			if (!this.CanBeUsed)
			{
				this.SetGlobalAlphaRecursively(0.5f);
				return;
			}
			this.SetGlobalAlphaRecursively(1f);
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00004CE7 File Offset: 0x00002EE7
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00004CEF File Offset: 0x00002EEF
		public bool CanBeUsed
		{
			get
			{
				return this._canBeUsed;
			}
			set
			{
				if (value != this._canBeUsed)
				{
					this._canBeUsed = value;
					base.OnPropertyChanged(value, "CanBeUsed");
					this.RefreshState();
				}
			}
		}

		// Token: 0x0400007B RID: 123
		private const float _disabledAlpha = 0.5f;

		// Token: 0x0400007C RID: 124
		private const float _enabledAlpha = 1f;

		// Token: 0x04000080 RID: 128
		private bool _isInitialized;

		// Token: 0x04000081 RID: 129
		private bool _canBeUsed = true;
	}
}
