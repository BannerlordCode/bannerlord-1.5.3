using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000159 RID: 345
	public struct FormOrder
	{
		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06001212 RID: 4626 RVA: 0x00037457 File Offset: 0x00035657
		// (set) Token: 0x06001213 RID: 4627 RVA: 0x0003745F File Offset: 0x0003565F
		public float CustomFlankWidth
		{
			get
			{
				return this._customFlankWidth;
			}
			set
			{
				this._customFlankWidth = value;
			}
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x00037468 File Offset: 0x00035668
		private FormOrder(FormOrder.FormOrderEnum orderEnum, float customFlankWidth = -1f)
		{
			this.OrderEnum = orderEnum;
			this._customFlankWidth = customFlankWidth;
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x00037478 File Offset: 0x00035678
		public static FormOrder FormOrderCustom(float customWidth)
		{
			return new FormOrder(FormOrder.FormOrderEnum.Custom, customWidth);
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06001216 RID: 4630 RVA: 0x00037484 File Offset: 0x00035684
		public OrderType OrderType
		{
			get
			{
				switch (this.OrderEnum)
				{
				case FormOrder.FormOrderEnum.Wide:
					return OrderType.FormWide;
				case FormOrder.FormOrderEnum.Wider:
					return OrderType.FormWider;
				case FormOrder.FormOrderEnum.Custom:
					return OrderType.FormCustom;
				default:
					return OrderType.FormDeep;
				}
			}
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x000374B9 File Offset: 0x000356B9
		public void OnApply(Formation formation)
		{
			this.OnApplyToArrangement(formation, formation.Arrangement);
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x000374C8 File Offset: 0x000356C8
		public static int GetUnitCountOf(Formation formation)
		{
			if (formation.OverridenUnitCount == null)
			{
				return formation.CountOfUnitsWithoutDetachedOnes;
			}
			return formation.OverridenUnitCount.Value;
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x000374FA File Offset: 0x000356FA
		public bool OnApplyToCustomArrangement(Formation formation, IFormationArrangement arrangement)
		{
			return false;
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x00037500 File Offset: 0x00035700
		private void OnApplyToArrangement(Formation formation, IFormationArrangement arrangement)
		{
			if (!this.OnApplyToCustomArrangement(formation, arrangement))
			{
				if (arrangement is ColumnFormation)
				{
					ColumnFormation columnFormation = arrangement as ColumnFormation;
					if (FormOrder.GetUnitCountOf(formation) > 0)
					{
						columnFormation.FormFromWidth((float)this.GetRankVerticalFormFileCount(formation));
					}
					if (this.OrderEnum == FormOrder.FormOrderEnum.Custom && MathF.Abs(this.CustomFlankWidth - arrangement.FlankWidth) > 0.01f)
					{
						ArrangementOrder.TransposeLineFormation(formation);
						formation.OnTick += formation.TickForColumnArrangementInitialPositioning;
						return;
					}
				}
				else
				{
					if (arrangement is RectilinearSchiltronFormation)
					{
						(arrangement as RectilinearSchiltronFormation).Form();
						return;
					}
					if (arrangement is CircularSchiltronFormation)
					{
						(arrangement as CircularSchiltronFormation).Form();
						return;
					}
					if (arrangement is CircularFormation)
					{
						CircularFormation circularFormation = arrangement as CircularFormation;
						int unitCountOf = FormOrder.GetUnitCountOf(formation);
						int? maxFileCount = this.GetMaxFileCount(unitCountOf);
						float num2;
						if (maxFileCount != null)
						{
							int num = MathF.Max(1, MathF.Ceiling((float)unitCountOf * 1f / (float)maxFileCount.Value));
							num2 = circularFormation.GetCircumferenceFromRankCount(num);
						}
						else
						{
							num2 = 3.1415927f * this.CustomFlankWidth;
						}
						circularFormation.FormFromCircumference(num2);
						return;
					}
					if (arrangement is SquareFormation)
					{
						SquareFormation squareFormation = arrangement as SquareFormation;
						int unitCountOf2 = FormOrder.GetUnitCountOf(formation);
						int? maxFileCount2 = this.GetMaxFileCount(unitCountOf2);
						if (maxFileCount2 != null)
						{
							int num3 = MathF.Max(1, MathF.Ceiling((float)unitCountOf2 * 1f / (float)maxFileCount2.Value));
							squareFormation.FormFromRankCount(num3);
							return;
						}
						squareFormation.FormFromBorderSideWidth(this.CustomFlankWidth);
						return;
					}
					else if (arrangement is SkeinFormation)
					{
						SkeinFormation skeinFormation = arrangement as SkeinFormation;
						int unitCountOf3 = FormOrder.GetUnitCountOf(formation);
						int? maxFileCount3 = this.GetMaxFileCount(unitCountOf3);
						if (maxFileCount3 != null)
						{
							skeinFormation.FormFromFlankWidth(maxFileCount3.Value, false);
							return;
						}
						skeinFormation.FlankWidth = this.CustomFlankWidth;
						return;
					}
					else if (arrangement is WedgeFormation)
					{
						WedgeFormation wedgeFormation = arrangement as WedgeFormation;
						int unitCountOf4 = FormOrder.GetUnitCountOf(formation);
						int? maxFileCount4 = this.GetMaxFileCount(unitCountOf4);
						if (maxFileCount4 != null)
						{
							wedgeFormation.FormFromFlankWidth(maxFileCount4.Value, false);
							return;
						}
						wedgeFormation.FlankWidth = this.CustomFlankWidth;
						return;
					}
					else if (arrangement is TransposedLineFormation)
					{
						TransposedLineFormation transposedLineFormation = arrangement as TransposedLineFormation;
						int unitCountOf5 = FormOrder.GetUnitCountOf(formation);
						if (unitCountOf5 > 0)
						{
							int? maxFileCount5 = this.GetMaxFileCount(unitCountOf5);
							if (maxFileCount5 == null)
							{
								maxFileCount5 = new int?(transposedLineFormation.GetFileCountFromWidth(this.CustomFlankWidth));
							}
							MathF.Ceiling((float)unitCountOf5 * 1f / (float)maxFileCount5.Value);
							transposedLineFormation.FormFromFlankWidth(this.GetRankVerticalFormFileCount(formation), false);
							return;
						}
					}
					else if (arrangement is LineFormation)
					{
						LineFormation lineFormation = arrangement as LineFormation;
						int unitCountOf6 = FormOrder.GetUnitCountOf(formation);
						int? maxFileCount6 = this.GetMaxFileCount(unitCountOf6);
						if (maxFileCount6 != null)
						{
							lineFormation.FormFromFlankWidth(maxFileCount6.Value, unitCountOf6 > 40);
							return;
						}
						lineFormation.FlankWidth = this.CustomFlankWidth;
						return;
					}
					else
					{
						Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Orders\\FormOrder.cs", "OnApplyToArrangement", 230);
					}
				}
			}
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x000377E2 File Offset: 0x000359E2
		private int? GetMaxFileCount(int unitCount)
		{
			return FormOrder.GetMaxFileCountStatic(this.OrderEnum, unitCount);
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x000377F0 File Offset: 0x000359F0
		public static int? GetMaxFileCountStatic(FormOrder.FormOrderEnum order, int unitCount)
		{
			return FormOrder.GetMaxFileCountAux(order, unitCount);
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x000377FC File Offset: 0x000359FC
		private int GetRankVerticalFormFileCount(IFormation formation)
		{
			int arrangementAspectRatio = ColumnFormation.ArrangementAspectRatio;
			int countOfUnitsWithoutLooseDetachedOnes = (formation as Formation).CountOfUnitsWithoutLooseDetachedOnes;
			switch (this.OrderEnum)
			{
			case FormOrder.FormOrderEnum.Deep:
				return MathF.Max(MathF.Round(MathF.Sqrt((float)countOfUnitsWithoutLooseDetachedOnes / ((float)arrangementAspectRatio * 2f))), 1);
			case FormOrder.FormOrderEnum.Wide:
				return MathF.Max(MathF.Round(MathF.Sqrt((float)countOfUnitsWithoutLooseDetachedOnes / ((float)arrangementAspectRatio * 1f))), 1);
			case FormOrder.FormOrderEnum.Wider:
				return MathF.Max(MathF.Round(MathF.Sqrt((float)countOfUnitsWithoutLooseDetachedOnes / ((float)arrangementAspectRatio * 0.5f))), 1);
			case FormOrder.FormOrderEnum.Custom:
				return MathF.Floor((this._customFlankWidth + formation.Interval) / (formation.UnitDiameter + formation.Interval));
			default:
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Orders\\FormOrder.cs", "GetRankVerticalFormFileCount", 274);
				return 1;
			}
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x000378CC File Offset: 0x00035ACC
		private static int? GetMaxFileCountAux(FormOrder.FormOrderEnum order, int unitCount)
		{
			if (order == FormOrder.FormOrderEnum.Custom)
			{
				return null;
			}
			int num = 0;
			switch (order)
			{
			case FormOrder.FormOrderEnum.Deep:
				num = MathF.Max(MathF.Round(MathF.Sqrt((float)unitCount / 4f)), 1) * 4;
				break;
			case FormOrder.FormOrderEnum.Wide:
				num = MathF.Max(MathF.Round(MathF.Sqrt((float)unitCount / 16f)), 1) * 16;
				break;
			case FormOrder.FormOrderEnum.Wider:
				num = MathF.Max(MathF.Round(MathF.Sqrt((float)unitCount / 64f)), 1) * 64;
				break;
			}
			return new int?(num);
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x0003795C File Offset: 0x00035B5C
		public override bool Equals(object obj)
		{
			if (obj is FormOrder)
			{
				FormOrder formOrder = (FormOrder)obj;
				return formOrder == this;
			}
			return false;
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x00037988 File Offset: 0x00035B88
		public override int GetHashCode()
		{
			return (int)this.OrderEnum;
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x00037990 File Offset: 0x00035B90
		public static bool operator !=(FormOrder f1, FormOrder f2)
		{
			return f1.OrderEnum != f2.OrderEnum;
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x000379A3 File Offset: 0x00035BA3
		public static bool operator ==(FormOrder f1, FormOrder f2)
		{
			return f1.OrderEnum == f2.OrderEnum;
		}

		// Token: 0x0400045F RID: 1119
		private float _customFlankWidth;

		// Token: 0x04000460 RID: 1120
		public readonly FormOrder.FormOrderEnum OrderEnum;

		// Token: 0x04000461 RID: 1121
		public static readonly FormOrder FormOrderDeep = new FormOrder(FormOrder.FormOrderEnum.Deep, -1f);

		// Token: 0x04000462 RID: 1122
		public static readonly FormOrder FormOrderWide = new FormOrder(FormOrder.FormOrderEnum.Wide, -1f);

		// Token: 0x04000463 RID: 1123
		public static readonly FormOrder FormOrderWider = new FormOrder(FormOrder.FormOrderEnum.Wider, -1f);

		// Token: 0x02000480 RID: 1152
		public enum FormOrderEnum
		{
			// Token: 0x04001AF1 RID: 6897
			Deep,
			// Token: 0x04001AF2 RID: 6898
			Wide,
			// Token: 0x04001AF3 RID: 6899
			Wider,
			// Token: 0x04001AF4 RID: 6900
			Custom
		}
	}
}
