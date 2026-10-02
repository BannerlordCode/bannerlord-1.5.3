using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Scoreboard
{
	// Token: 0x02000059 RID: 89
	public class ScoreboardSideMoraleListPanel : ListPanel
	{
		// Token: 0x060004EF RID: 1263 RVA: 0x0000F672 File Offset: 0x0000D872
		public ScoreboardSideMoraleListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0000F688 File Offset: 0x0000D888
		private void OnMoraleUpdated()
		{
			if (base.ChildCount > 0)
			{
				float num = ((this.MaxMorale != 0f) ? (this.Morale / this.MaxMorale * 100f) : 0f);
				num = MathF.Clamp(num, 0f, 100f);
				num = (float)MathF.Round(num);
				float num2 = (float)(100 / base.ChildCount);
				for (int i = 0; i < base.ChildCount; i++)
				{
					float num3 = (num - (float)i * num2) / num2;
					num3 = MathF.Clamp(num3, 0f, 1f);
					Widget child = base.GetChild(i);
					if (num3 > 0f)
					{
						child.SetState("Default");
					}
					else
					{
						child.SetState("Disabled");
					}
					(child as FillBarWidget).InitialAmountAsFloat = num3;
				}
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x0000F750 File Offset: 0x0000D950
		// (set) Token: 0x060004F2 RID: 1266 RVA: 0x0000F758 File Offset: 0x0000D958
		[Editor(false)]
		public float Morale
		{
			get
			{
				return this._morale;
			}
			set
			{
				if (this._morale != value)
				{
					this._morale = value;
					base.OnPropertyChanged(value, "Morale");
					this.OnMoraleUpdated();
				}
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x0000F77C File Offset: 0x0000D97C
		// (set) Token: 0x060004F4 RID: 1268 RVA: 0x0000F785 File Offset: 0x0000D985
		[Editor(false)]
		public int MoraleInt
		{
			get
			{
				return (int)this._morale;
			}
			set
			{
				this.Morale = (float)value;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x0000F78F File Offset: 0x0000D98F
		// (set) Token: 0x060004F6 RID: 1270 RVA: 0x0000F797 File Offset: 0x0000D997
		[Editor(false)]
		public float MaxMorale
		{
			get
			{
				return this._maxMorale;
			}
			set
			{
				if (this._maxMorale != value)
				{
					this._maxMorale = value;
					base.OnPropertyChanged(value, "MaxMorale");
					this.OnMoraleUpdated();
				}
			}
		}

		// Token: 0x0400021B RID: 539
		private float _morale;

		// Token: 0x0400021C RID: 540
		private float _maxMorale = 100f;
	}
}
