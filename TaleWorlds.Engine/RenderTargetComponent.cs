using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x0200007E RID: 126
	public sealed class RenderTargetComponent : DotNetObject
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x0000B12E File Offset: 0x0000932E
		public Texture RenderTarget
		{
			get
			{
				return (Texture)this._renderTargetWeakReference.GetNativeObject();
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x0000B140 File Offset: 0x00009340
		// (set) Token: 0x06000AC8 RID: 2760 RVA: 0x0000B148 File Offset: 0x00009348
		public object UserData { get; internal set; }

		// Token: 0x06000AC9 RID: 2761 RVA: 0x0000B151 File Offset: 0x00009351
		internal RenderTargetComponent(Texture renderTarget)
		{
			this._renderTargetWeakReference = new WeakNativeObjectReference(renderTarget);
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x0000B165 File Offset: 0x00009365
		internal void OnTargetReleased()
		{
			this.PaintNeeded = null;
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x0000B16E File Offset: 0x0000936E
		[EngineCallback(null, false)]
		internal static RenderTargetComponent CreateRenderTargetComponent(Texture renderTarget)
		{
			return new RenderTargetComponent(renderTarget);
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000ACC RID: 2764 RVA: 0x0000B178 File Offset: 0x00009378
		// (remove) Token: 0x06000ACD RID: 2765 RVA: 0x0000B1B0 File Offset: 0x000093B0
		internal event RenderTargetComponent.TextureUpdateEventHandler PaintNeeded;

		// Token: 0x06000ACE RID: 2766 RVA: 0x0000B1E5 File Offset: 0x000093E5
		[EngineCallback(null, false)]
		internal void OnPaintNeeded()
		{
			RenderTargetComponent.TextureUpdateEventHandler paintNeeded = this.PaintNeeded;
			if (paintNeeded == null)
			{
				return;
			}
			paintNeeded(this.RenderTarget, EventArgs.Empty);
		}

		// Token: 0x04000197 RID: 407
		private readonly WeakNativeObjectReference _renderTargetWeakReference;

		// Token: 0x020000D0 RID: 208
		// (Invoke) Token: 0x06001030 RID: 4144
		public delegate void TextureUpdateEventHandler(Texture sender, EventArgs e);
	}
}
