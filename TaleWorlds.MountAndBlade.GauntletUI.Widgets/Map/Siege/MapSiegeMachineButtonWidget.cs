using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.Siege
{
	// Token: 0x0200011F RID: 287
	public class MapSiegeMachineButtonWidget : ButtonWidget
	{
		// Token: 0x06000F3B RID: 3899 RVA: 0x0002A43A File Offset: 0x0002863A
		public MapSiegeMachineButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x0002A458 File Offset: 0x00028658
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.ColoredImageWidget != null && !this._machineSpritesUpdated)
			{
				this.SetStylesSprite(this.ColoredImageWidget, "SPGeneral\\Siege\\" + this.MachineID);
				this._machineSpritesUpdated = true;
			}
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x0002A494 File Offset: 0x00028694
		private void SetStylesSprite(Widget widget, string spriteName)
		{
			widget.Sprite = base.Context.SpriteData.GetSprite(spriteName);
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06000F3E RID: 3902 RVA: 0x0002A4AD File Offset: 0x000286AD
		// (set) Token: 0x06000F3F RID: 3903 RVA: 0x0002A4B5 File Offset: 0x000286B5
		[Editor(false)]
		public Widget ColoredImageWidget
		{
			get
			{
				return this._coloredImageWidget;
			}
			set
			{
				if (value != this._coloredImageWidget)
				{
					this._coloredImageWidget = value;
					base.OnPropertyChanged<Widget>(value, "ColoredImageWidget");
				}
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06000F40 RID: 3904 RVA: 0x0002A4D3 File Offset: 0x000286D3
		// (set) Token: 0x06000F41 RID: 3905 RVA: 0x0002A4DB File Offset: 0x000286DB
		[Editor(false)]
		public bool IsDeploymentTarget
		{
			get
			{
				return this._isDeploymentTarget;
			}
			set
			{
				if (value != this._isDeploymentTarget)
				{
					this._isDeploymentTarget = value;
					base.OnPropertyChanged(value, "IsDeploymentTarget");
				}
			}
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06000F42 RID: 3906 RVA: 0x0002A4F9 File Offset: 0x000286F9
		// (set) Token: 0x06000F43 RID: 3907 RVA: 0x0002A501 File Offset: 0x00028701
		[Editor(false)]
		public string MachineID
		{
			get
			{
				return this._machineID;
			}
			set
			{
				if (value != this._machineID)
				{
					this._machineID = value;
					base.OnPropertyChanged<string>(value, "MachineID");
					this._machineSpritesUpdated = false;
				}
			}
		}

		// Token: 0x040006F8 RID: 1784
		private Vec2 _orgClipSize = new Vec2(-1f, -1f);

		// Token: 0x040006F9 RID: 1785
		private bool _machineSpritesUpdated;

		// Token: 0x040006FA RID: 1786
		private Widget _coloredImageWidget;

		// Token: 0x040006FB RID: 1787
		private bool _isDeploymentTarget;

		// Token: 0x040006FC RID: 1788
		private string _machineID;
	}
}
