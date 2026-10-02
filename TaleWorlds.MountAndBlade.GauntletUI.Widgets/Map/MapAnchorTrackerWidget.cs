using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map
{
	// Token: 0x02000119 RID: 281
	public class MapAnchorTrackerWidget : Widget
	{
		// Token: 0x06000EFA RID: 3834 RVA: 0x0002956A File Offset: 0x0002776A
		public MapAnchorTrackerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x00029574 File Offset: 0x00027774
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.IsRecursivelyVisible())
			{
				float num = this.PositionX - base.Size.X * 0.5f;
				float num2 = this.PositionY - base.Size.Y * 0.5f;
				if (num + base.Size.X >= 0f && num <= base.EventManager.PageSize.X && num2 + base.Size.Y >= 0f && num2 <= base.EventManager.PageSize.Y)
				{
					base.ScaledPositionXOffset = MathF.Clamp(num, 0f, base.EventManager.PageSize.X - base.Size.X);
					base.ScaledPositionYOffset = MathF.Clamp(num2, 0f, base.EventManager.PageSize.Y - base.Size.Y - 50f * base._scaleToUse);
					return;
				}
				base.ScaledPositionXOffset = -5000f;
				base.ScaledPositionYOffset = -5000f;
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06000EFC RID: 3836 RVA: 0x00029697 File Offset: 0x00027897
		// (set) Token: 0x06000EFD RID: 3837 RVA: 0x0002969F File Offset: 0x0002789F
		[Editor(false)]
		public float PositionX
		{
			get
			{
				return this._positionX;
			}
			set
			{
				if (value != this._positionX)
				{
					this._positionX = value;
					base.OnPropertyChanged(value, "PositionX");
				}
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06000EFE RID: 3838 RVA: 0x000296BD File Offset: 0x000278BD
		// (set) Token: 0x06000EFF RID: 3839 RVA: 0x000296C5 File Offset: 0x000278C5
		[Editor(false)]
		public float PositionY
		{
			get
			{
				return this._positionY;
			}
			set
			{
				if (value != this._positionY)
				{
					this._positionY = value;
					base.OnPropertyChanged(value, "PositionY");
				}
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06000F00 RID: 3840 RVA: 0x000296E3 File Offset: 0x000278E3
		// (set) Token: 0x06000F01 RID: 3841 RVA: 0x000296EB File Offset: 0x000278EB
		[Editor(false)]
		public float PositionW
		{
			get
			{
				return this._positionW;
			}
			set
			{
				if (value != this._positionW)
				{
					this._positionW = value;
					base.OnPropertyChanged(value, "PositionW");
				}
			}
		}

		// Token: 0x040006D6 RID: 1750
		private float _positionX;

		// Token: 0x040006D7 RID: 1751
		private float _positionY;

		// Token: 0x040006D8 RID: 1752
		private float _positionW;
	}
}
