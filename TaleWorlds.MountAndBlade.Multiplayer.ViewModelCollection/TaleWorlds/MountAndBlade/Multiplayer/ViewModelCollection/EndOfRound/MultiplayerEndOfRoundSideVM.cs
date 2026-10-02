using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.EndOfRound
{
	// Token: 0x020000A4 RID: 164
	public class MultiplayerEndOfRoundSideVM : ViewModel
	{
		// Token: 0x06000FDE RID: 4062 RVA: 0x00031804 File Offset: 0x0002FA04
		public void SetData(BasicCultureObject culture, int score, bool isWinner, MultiplayerBattleColors.MultiplayerCultureColorInfo cultureColors)
		{
			this._culture = culture;
			this.CultureID = culture.StringId;
			this.Score = score;
			this.IsWinner = isWinner;
			this.CultureColor1 = cultureColors.Color1;
			this.CultureColor2 = cultureColors.Color2;
			this.RefreshValues();
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x00031852 File Offset: 0x0002FA52
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CultureName = this._culture.Name.ToString();
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06000FE0 RID: 4064 RVA: 0x00031870 File Offset: 0x0002FA70
		// (set) Token: 0x06000FE1 RID: 4065 RVA: 0x00031878 File Offset: 0x0002FA78
		[DataSourceProperty]
		public bool IsWinner
		{
			get
			{
				return this._isWinner;
			}
			set
			{
				if (value != this._isWinner)
				{
					this._isWinner = value;
					base.OnPropertyChangedWithValue(value, "IsWinner");
				}
			}
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06000FE2 RID: 4066 RVA: 0x00031896 File Offset: 0x0002FA96
		// (set) Token: 0x06000FE3 RID: 4067 RVA: 0x0003189E File Offset: 0x0002FA9E
		[DataSourceProperty]
		public Color CultureColor1
		{
			get
			{
				return this._cultureColor1;
			}
			set
			{
				if (value != this._cultureColor1)
				{
					this._cultureColor1 = value;
					base.OnPropertyChangedWithValue(value, "CultureColor1");
				}
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06000FE4 RID: 4068 RVA: 0x000318C1 File Offset: 0x0002FAC1
		// (set) Token: 0x06000FE5 RID: 4069 RVA: 0x000318C9 File Offset: 0x0002FAC9
		[DataSourceProperty]
		public Color CultureColor2
		{
			get
			{
				return this._cultureColor2;
			}
			set
			{
				if (value != this._cultureColor2)
				{
					this._cultureColor2 = value;
					base.OnPropertyChangedWithValue(value, "CultureColor2");
				}
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x000318EC File Offset: 0x0002FAEC
		// (set) Token: 0x06000FE7 RID: 4071 RVA: 0x000318F4 File Offset: 0x0002FAF4
		[DataSourceProperty]
		public string CultureID
		{
			get
			{
				return this._cultureID;
			}
			set
			{
				if (value != this._cultureID)
				{
					this._cultureID = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureID");
				}
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06000FE8 RID: 4072 RVA: 0x00031917 File Offset: 0x0002FB17
		// (set) Token: 0x06000FE9 RID: 4073 RVA: 0x0003191F File Offset: 0x0002FB1F
		[DataSourceProperty]
		public string CultureName
		{
			get
			{
				return this._cultureName;
			}
			set
			{
				if (value != this._cultureName)
				{
					this._cultureName = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureName");
				}
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06000FEA RID: 4074 RVA: 0x00031942 File Offset: 0x0002FB42
		// (set) Token: 0x06000FEB RID: 4075 RVA: 0x0003194A File Offset: 0x0002FB4A
		[DataSourceProperty]
		public int Score
		{
			get
			{
				return this._score;
			}
			set
			{
				if (value != this._score)
				{
					this._score = value;
					base.OnPropertyChangedWithValue(value, "Score");
				}
			}
		}

		// Token: 0x04000764 RID: 1892
		private BasicCultureObject _culture;

		// Token: 0x04000765 RID: 1893
		private bool _isWinner;

		// Token: 0x04000766 RID: 1894
		private string _cultureID;

		// Token: 0x04000767 RID: 1895
		private Color _cultureColor1;

		// Token: 0x04000768 RID: 1896
		private Color _cultureColor2;

		// Token: 0x04000769 RID: 1897
		private string _cultureName;

		// Token: 0x0400076A RID: 1898
		private int _score;
	}
}
