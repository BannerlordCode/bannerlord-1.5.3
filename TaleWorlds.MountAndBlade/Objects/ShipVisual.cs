using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Objects
{
	// Token: 0x020003AD RID: 941
	public class ShipVisual : ScriptComponentBehavior
	{
		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x060035C3 RID: 13763 RVA: 0x000DDF9A File Offset: 0x000DC19A
		// (set) Token: 0x060035C4 RID: 13764 RVA: 0x000DDFA2 File Offset: 0x000DC1A2
		public int Seed { get; private set; }

		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x060035C5 RID: 13765 RVA: 0x000DDFAB File Offset: 0x000DC1AB
		// (set) Token: 0x060035C6 RID: 13766 RVA: 0x000DDFB3 File Offset: 0x000DC1B3
		public string CustomSailPatternId { get; private set; }

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x060035C7 RID: 13767 RVA: 0x000DDFBC File Offset: 0x000DC1BC
		// (set) Token: 0x060035C8 RID: 13768 RVA: 0x000DDFC4 File Offset: 0x000DC1C4
		public List<ScriptComponentBehavior> SailVisuals { get; private set; }

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x060035C9 RID: 13769 RVA: 0x000DDFCD File Offset: 0x000DC1CD
		// (set) Token: 0x060035CA RID: 13770 RVA: 0x000DDFD5 File Offset: 0x000DC1D5
		public float Health
		{
			get
			{
				return this._health;
			}
			private set
			{
				this._health = MathF.Clamp(value, 0f, 1f);
			}
		}

		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x060035CB RID: 13771 RVA: 0x000DDFED File Offset: 0x000DC1ED
		// (set) Token: 0x060035CC RID: 13772 RVA: 0x000DDFF5 File Offset: 0x000DC1F5
		[TupleElementNames(new string[] { "sailColor1", "sailColor2" })]
		public ValueTuple<uint, uint> SailColors
		{
			[return: TupleElementNames(new string[] { "sailColor1", "sailColor2" })]
			get;
			[param: TupleElementNames(new string[] { "sailColor1", "sailColor2" })]
			private set;
		}

		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x060035CD RID: 13773 RVA: 0x000DDFFE File Offset: 0x000DC1FE
		// (set) Token: 0x060035CE RID: 13774 RVA: 0x000DE006 File Offset: 0x000DC206
		public float FloatingForceMultiplier { get; private set; }

		// Token: 0x060035CF RID: 13775 RVA: 0x000DE010 File Offset: 0x000DC210
		public void Initialize(int seed, string customSailPatternId = "", float? health = null, [TupleElementNames(new string[] { "sailColor1", "sailColor2" })] ValueTuple<uint, uint>? sailColors = null, float? floatingForceMultiplier = null)
		{
			this.Seed = seed;
			this.CustomSailPatternId = customSailPatternId;
			this.SailVisuals = new List<ScriptComponentBehavior>();
			this.Health = ((health != null) ? health.Value : 1f);
			this.SailColors = ((sailColors != null) ? sailColors.Value : new ValueTuple<uint, uint>(Colors.White.ToUnsignedInteger(), Colors.White.ToUnsignedInteger()));
			this.FloatingForceMultiplier = ((floatingForceMultiplier != null) ? floatingForceMultiplier.Value : 1f);
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x000DE0A8 File Offset: 0x000DC2A8
		public void UpdateParameters(float? health = null, [TupleElementNames(new string[] { "sailColor1", "sailColor2" })] ValueTuple<uint, uint>? sailColors = null, float? floatingForceMultiplier = null)
		{
			if (health != null)
			{
				this.Health = health.Value;
			}
			if (sailColors != null)
			{
				this.SailColors = sailColors.Value;
			}
			if (floatingForceMultiplier != null)
			{
				this.FloatingForceMultiplier = floatingForceMultiplier.Value;
			}
		}

		// Token: 0x040016F0 RID: 5872
		private float _health;
	}
}
