using System;
using TaleWorlds.Library;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x02000013 RID: 19
	public class BrushLayerAnimation
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00007247 File Offset: 0x00005447
		// (set) Token: 0x06000147 RID: 327 RVA: 0x0000724F File Offset: 0x0000544F
		public string LayerName { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00007258 File Offset: 0x00005458
		public MBReadOnlyList<BrushAnimationProperty> Collections
		{
			get
			{
				return this._collections;
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00007260 File Offset: 0x00005460
		public BrushLayerAnimation()
		{
			this.LayerName = null;
			this._collections = new MBList<BrushAnimationProperty>();
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000727A File Offset: 0x0000547A
		internal void RemoveAnimationProperty(BrushAnimationProperty property)
		{
			this._collections.Remove(property);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00007289 File Offset: 0x00005489
		public void AddAnimationProperty(BrushAnimationProperty property)
		{
			this._collections.Add(property);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00007298 File Offset: 0x00005498
		private void FillFrom(BrushLayerAnimation brushLayerAnimation)
		{
			this.LayerName = brushLayerAnimation.LayerName;
			this._collections = new MBList<BrushAnimationProperty>();
			foreach (BrushAnimationProperty brushAnimationProperty in brushLayerAnimation._collections)
			{
				BrushAnimationProperty brushAnimationProperty2 = brushAnimationProperty.Clone();
				this._collections.Add(brushAnimationProperty2);
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000730C File Offset: 0x0000550C
		public BrushLayerAnimation Clone()
		{
			BrushLayerAnimation brushLayerAnimation = new BrushLayerAnimation();
			brushLayerAnimation.FillFrom(this);
			return brushLayerAnimation;
		}

		// Token: 0x04000072 RID: 114
		private MBList<BrushAnimationProperty> _collections;
	}
}
