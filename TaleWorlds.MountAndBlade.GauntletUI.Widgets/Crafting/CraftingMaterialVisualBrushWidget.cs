using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Crafting
{
	// Token: 0x0200016A RID: 362
	public class CraftingMaterialVisualBrushWidget : BrushWidget
	{
		// Token: 0x06001336 RID: 4918 RVA: 0x00034D03 File Offset: 0x00032F03
		public CraftingMaterialVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x00034D13 File Offset: 0x00032F13
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._visualDirty)
			{
				this.UpdateVisual();
				this._visualDirty = false;
			}
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x00034D34 File Offset: 0x00032F34
		private void UpdateVisual()
		{
			this.RegisterBrushStatesOfWidget();
			string text = this.MaterialType;
			if (this.IsBig)
			{
				text += "Big";
			}
			this.SetState(text);
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06001339 RID: 4921 RVA: 0x00034D69 File Offset: 0x00032F69
		// (set) Token: 0x0600133A RID: 4922 RVA: 0x00034D71 File Offset: 0x00032F71
		public string MaterialType
		{
			get
			{
				return this._materialType;
			}
			set
			{
				if (this._materialType != value)
				{
					this._materialType = value;
					this._visualDirty = true;
				}
			}
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x0600133B RID: 4923 RVA: 0x00034D8F File Offset: 0x00032F8F
		// (set) Token: 0x0600133C RID: 4924 RVA: 0x00034D97 File Offset: 0x00032F97
		public bool IsBig
		{
			get
			{
				return this._isBig;
			}
			set
			{
				if (this._isBig != value)
				{
					this._isBig = value;
					this._visualDirty = true;
				}
			}
		}

		// Token: 0x040008C2 RID: 2242
		private bool _visualDirty = true;

		// Token: 0x040008C3 RID: 2243
		private string _materialType;

		// Token: 0x040008C4 RID: 2244
		private bool _isBig;
	}
}
