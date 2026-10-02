using System;
using TaleWorlds.CampaignSystem.Incidents;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Map.Incidents
{
	// Token: 0x0200004B RID: 75
	public class MapIncidentHintVM : ViewModel
	{
		// Token: 0x060004AF RID: 1199 RVA: 0x00012A0C File Offset: 0x00010C0C
		public MapIncidentHintVM(IncidentHint hint, bool showChanceVisually = false)
		{
			this._hintText = MapIncidentHintVM.GetDisplayedText(hint);
			this._chancePercentage = MathF.Round(hint.Chance * 100f);
			this.ChildHints = new MBBindingList<MapIncidentHintVM>();
			bool flag = hint.Type == IncidentHintType.Select;
			foreach (IncidentHint incidentHint in hint.Children)
			{
				this.ChildHints.Add(new MapIncidentHintVM(incidentHint, flag));
			}
			this.HasText = this._hintText != null;
			this.HasChildHints = this.ChildHints.Count > 0;
			this.HasChance = showChanceVisually && this.HasChildHints;
			this.HasIndent = this.HasChildHints && (this.HasChance || (this.HasText && !flag));
			this.RefreshValues();
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00012AEC File Offset: 0x00010CEC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Text = (this.HasText ? ("• " + this._hintText.ToString()) : null);
			this.ChanceText = (this.HasChance ? GameTexts.FindText("str_NUMBER_percent", null).SetTextVariable("NUMBER", this._chancePercentage).ToString() : null);
			this.ChildHints.ApplyActionOnAllItems(delegate(MapIncidentHintVM h)
			{
				h.RefreshValues();
			});
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00012B80 File Offset: 0x00010D80
		private static TextObject GetDisplayedText(IncidentHint hint)
		{
			IncidentHintType type = hint.Type;
			if (type == IncidentHintType.Select)
			{
				return new TextObject("{=ABZbb0xQ}One of the following happens:", null);
			}
			if (type == IncidentHintType.SelectBranch)
			{
				return null;
			}
			return hint.Text;
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x00012BB0 File Offset: 0x00010DB0
		// (set) Token: 0x060004B3 RID: 1203 RVA: 0x00012BB8 File Offset: 0x00010DB8
		[DataSourceProperty]
		public bool HasText
		{
			get
			{
				return this._hasText;
			}
			set
			{
				if (value != this._hasText)
				{
					this._hasText = value;
					base.OnPropertyChangedWithValue(value, "HasText");
				}
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x00012BD6 File Offset: 0x00010DD6
		// (set) Token: 0x060004B5 RID: 1205 RVA: 0x00012BDE File Offset: 0x00010DDE
		[DataSourceProperty]
		public bool HasChance
		{
			get
			{
				return this._hasChance;
			}
			set
			{
				if (value != this._hasChance)
				{
					this._hasChance = value;
					base.OnPropertyChangedWithValue(value, "HasChance");
				}
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x00012BFC File Offset: 0x00010DFC
		// (set) Token: 0x060004B7 RID: 1207 RVA: 0x00012C04 File Offset: 0x00010E04
		[DataSourceProperty]
		public bool HasIndent
		{
			get
			{
				return this._hasIndent;
			}
			set
			{
				if (value != this._hasIndent)
				{
					this._hasIndent = value;
					base.OnPropertyChangedWithValue(value, "HasIndent");
				}
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x00012C22 File Offset: 0x00010E22
		// (set) Token: 0x060004B9 RID: 1209 RVA: 0x00012C2A File Offset: 0x00010E2A
		[DataSourceProperty]
		public bool HasChildHints
		{
			get
			{
				return this._hasChildHints;
			}
			set
			{
				if (value != this._hasChildHints)
				{
					this._hasChildHints = value;
					base.OnPropertyChangedWithValue(value, "HasChildHints");
				}
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060004BA RID: 1210 RVA: 0x00012C48 File Offset: 0x00010E48
		// (set) Token: 0x060004BB RID: 1211 RVA: 0x00012C50 File Offset: 0x00010E50
		[DataSourceProperty]
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				if (value != this._text)
				{
					this._text = value;
					base.OnPropertyChangedWithValue<string>(value, "Text");
				}
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x00012C73 File Offset: 0x00010E73
		// (set) Token: 0x060004BD RID: 1213 RVA: 0x00012C7B File Offset: 0x00010E7B
		[DataSourceProperty]
		public string ChanceText
		{
			get
			{
				return this._chanceText;
			}
			set
			{
				if (value != this._chanceText)
				{
					this._chanceText = value;
					base.OnPropertyChangedWithValue<string>(value, "ChanceText");
				}
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x00012C9E File Offset: 0x00010E9E
		// (set) Token: 0x060004BF RID: 1215 RVA: 0x00012CA6 File Offset: 0x00010EA6
		[DataSourceProperty]
		public MBBindingList<MapIncidentHintVM> ChildHints
		{
			get
			{
				return this._childHints;
			}
			set
			{
				if (value != this._childHints)
				{
					this._childHints = value;
					base.OnPropertyChangedWithValue<MBBindingList<MapIncidentHintVM>>(value, "ChildHints");
				}
			}
		}

		// Token: 0x0400025C RID: 604
		private readonly TextObject _hintText;

		// Token: 0x0400025D RID: 605
		private readonly int _chancePercentage;

		// Token: 0x0400025E RID: 606
		private bool _hasText;

		// Token: 0x0400025F RID: 607
		private bool _hasChance;

		// Token: 0x04000260 RID: 608
		private bool _hasIndent;

		// Token: 0x04000261 RID: 609
		private bool _hasChildHints;

		// Token: 0x04000262 RID: 610
		private string _text;

		// Token: 0x04000263 RID: 611
		private string _chanceText;

		// Token: 0x04000264 RID: 612
		private MBBindingList<MapIncidentHintVM> _childHints;
	}
}
