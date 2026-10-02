using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.FormationMarker
{
	// Token: 0x02000061 RID: 97
	public class MissionFormationMarkerTargetVM : ViewModel
	{
		// Token: 0x1700023B RID: 571
		// (get) Token: 0x0600079C RID: 1948 RVA: 0x0001ABD6 File Offset: 0x00018DD6
		// (set) Token: 0x0600079D RID: 1949 RVA: 0x0001ABDE File Offset: 0x00018DDE
		public Formation Formation { get; private set; }

		// Token: 0x0600079E RID: 1950 RVA: 0x0001ABE8 File Offset: 0x00018DE8
		public MissionFormationMarkerTargetVM(Formation formation)
		{
			this.Formation = formation;
			this.FormationType = MissionFormationMarkerTargetVM.GetFormationType(this.Formation.RepresentativeClass);
			if (this.Formation.Team.IsPlayerTeam)
			{
				this.TeamType = 0;
				return;
			}
			if (this.Formation.Team.IsPlayerAlly)
			{
				this.TeamType = 1;
				return;
			}
			this.TeamType = 2;
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0001AC9C File Offset: 0x00018E9C
		public void Refresh()
		{
			this.Size = this.Formation.CountOfUnits;
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0001ACAF File Offset: 0x00018EAF
		public void SetTargetedState(bool isFocused, bool isTargetingAFormation)
		{
			this.IsCenterOfFocus = isFocused;
			this.IsTargetingAFormation = isTargetingAFormation;
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0001ACC0 File Offset: 0x00018EC0
		public static string GetFormationType(FormationClass formationType)
		{
			switch (formationType)
			{
			case FormationClass.Infantry:
				return "Infantry_Light";
			case FormationClass.Ranged:
				return "Archer_Light";
			case FormationClass.Cavalry:
				return "Cavalry_Light";
			case FormationClass.HorseArcher:
				return "HorseArcher_Light";
			case FormationClass.NumberOfDefaultFormations:
			case FormationClass.HeavyInfantry:
			case FormationClass.NumberOfRegularFormations:
			case FormationClass.Bodyguard:
			case FormationClass.NumberOfAllFormations:
				return "Infantry_Heavy";
			case FormationClass.LightCavalry:
				return "Cavalry_Light";
			case FormationClass.HeavyCavalry:
				return "Cavalry_Heavy";
			default:
				return "None";
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x0001AD30 File Offset: 0x00018F30
		// (set) Token: 0x060007A3 RID: 1955 RVA: 0x0001AD38 File Offset: 0x00018F38
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (this._isEnabled != value)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x0001AD56 File Offset: 0x00018F56
		// (set) Token: 0x060007A5 RID: 1957 RVA: 0x0001AD5E File Offset: 0x00018F5E
		[DataSourceProperty]
		public bool IsCenterOfFocus
		{
			get
			{
				return this._isCenterOfFocus;
			}
			set
			{
				if (this._isCenterOfFocus != value)
				{
					this._isCenterOfFocus = value;
					base.OnPropertyChangedWithValue(value, "IsCenterOfFocus");
				}
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x0001AD7C File Offset: 0x00018F7C
		// (set) Token: 0x060007A7 RID: 1959 RVA: 0x0001AD84 File Offset: 0x00018F84
		[DataSourceProperty]
		public bool IsFormationTargetRelevant
		{
			get
			{
				return this._isFormationTargetRelevant;
			}
			set
			{
				if (this._isFormationTargetRelevant != value)
				{
					this._isFormationTargetRelevant = value;
					base.OnPropertyChangedWithValue(value, "IsFormationTargetRelevant");
				}
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x0001ADA2 File Offset: 0x00018FA2
		// (set) Token: 0x060007A9 RID: 1961 RVA: 0x0001ADAA File Offset: 0x00018FAA
		[DataSourceProperty]
		public bool IsTargetingAFormation
		{
			get
			{
				return this._isTargetingAFormation;
			}
			set
			{
				if (this._isTargetingAFormation != value)
				{
					this._isTargetingAFormation = value;
					base.OnPropertyChangedWithValue(value, "IsTargetingAFormation");
				}
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x0001ADC8 File Offset: 0x00018FC8
		// (set) Token: 0x060007AB RID: 1963 RVA: 0x0001ADD0 File Offset: 0x00018FD0
		[DataSourceProperty]
		public bool ShowDistanceTexts
		{
			get
			{
				return this._showDistanceTexts;
			}
			set
			{
				if (this._showDistanceTexts != value)
				{
					this._showDistanceTexts = value;
					base.OnPropertyChangedWithValue(value, "ShowDistanceTexts");
				}
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x060007AC RID: 1964 RVA: 0x0001ADEE File Offset: 0x00018FEE
		// (set) Token: 0x060007AD RID: 1965 RVA: 0x0001ADF6 File Offset: 0x00018FF6
		[DataSourceProperty]
		public string FormationType
		{
			get
			{
				return this._formationType;
			}
			set
			{
				if (this._formationType != value)
				{
					this._formationType = value;
					base.OnPropertyChangedWithValue<string>(value, "FormationType");
				}
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x060007AE RID: 1966 RVA: 0x0001AE19 File Offset: 0x00019019
		// (set) Token: 0x060007AF RID: 1967 RVA: 0x0001AE21 File Offset: 0x00019021
		[DataSourceProperty]
		public int TeamType
		{
			get
			{
				return this._teamType;
			}
			set
			{
				if (this._teamType != value)
				{
					this._teamType = value;
					base.OnPropertyChangedWithValue(value, "TeamType");
				}
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x0001AE3F File Offset: 0x0001903F
		// (set) Token: 0x060007B1 RID: 1969 RVA: 0x0001AE47 File Offset: 0x00019047
		[DataSourceProperty]
		public Vec2 ScreenPosition
		{
			get
			{
				return this._screenPosition;
			}
			set
			{
				if (value.x != this._screenPosition.x || value.y != this._screenPosition.y)
				{
					this._screenPosition = value;
					base.OnPropertyChangedWithValue(value, "ScreenPosition");
				}
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x0001AE82 File Offset: 0x00019082
		// (set) Token: 0x060007B3 RID: 1971 RVA: 0x0001AE8A File Offset: 0x0001908A
		[DataSourceProperty]
		public float Distance
		{
			get
			{
				return this._distance;
			}
			set
			{
				if (this._distance != value && !float.IsNaN(value))
				{
					this._distance = value;
					base.OnPropertyChangedWithValue(value, "Distance");
				}
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x0001AEB0 File Offset: 0x000190B0
		// (set) Token: 0x060007B5 RID: 1973 RVA: 0x0001AEB8 File Offset: 0x000190B8
		[DataSourceProperty]
		public string DistanceText
		{
			get
			{
				return this._distanceText;
			}
			set
			{
				if (this._distanceText != value)
				{
					this._distanceText = value;
					base.OnPropertyChangedWithValue<string>(value, "DistanceText");
				}
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x0001AEDB File Offset: 0x000190DB
		// (set) Token: 0x060007B7 RID: 1975 RVA: 0x0001AEE3 File Offset: 0x000190E3
		[DataSourceProperty]
		public int Size
		{
			get
			{
				return this._size;
			}
			set
			{
				if (this._size != value)
				{
					this._size = value;
					base.OnPropertyChangedWithValue(value, "Size");
				}
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x060007B8 RID: 1976 RVA: 0x0001AF01 File Offset: 0x00019101
		// (set) Token: 0x060007B9 RID: 1977 RVA: 0x0001AF09 File Offset: 0x00019109
		[DataSourceProperty]
		public int WSign
		{
			get
			{
				return this._wSign;
			}
			set
			{
				if (this._wSign != value)
				{
					this._wSign = value;
					base.OnPropertyChangedWithValue(value, "WSign");
				}
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x0001AF27 File Offset: 0x00019127
		// (set) Token: 0x060007BB RID: 1979 RVA: 0x0001AF2F File Offset: 0x0001912F
		[DataSourceProperty]
		public float FarAlphaTarget
		{
			get
			{
				return this._farAlphaTarget;
			}
			set
			{
				if (this._farAlphaTarget != value)
				{
					this._farAlphaTarget = value;
					base.OnPropertyChangedWithValue(value, "FarAlphaTarget");
				}
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x060007BC RID: 1980 RVA: 0x0001AF4D File Offset: 0x0001914D
		// (set) Token: 0x060007BD RID: 1981 RVA: 0x0001AF55 File Offset: 0x00019155
		[DataSourceProperty]
		public float FarDistanceCutoff
		{
			get
			{
				return this._farDistanceCutoff;
			}
			set
			{
				if (this._farDistanceCutoff != value)
				{
					this._farDistanceCutoff = value;
					base.OnPropertyChangedWithValue(value, "FarDistanceCutoff");
				}
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x060007BE RID: 1982 RVA: 0x0001AF73 File Offset: 0x00019173
		// (set) Token: 0x060007BF RID: 1983 RVA: 0x0001AF7B File Offset: 0x0001917B
		[DataSourceProperty]
		public float CloseDistanceCutoff
		{
			get
			{
				return this._closeDistanceCutoff;
			}
			set
			{
				if (this._closeDistanceCutoff != value)
				{
					this._closeDistanceCutoff = value;
					base.OnPropertyChangedWithValue(value, "CloseDistanceCutoff");
				}
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x0001AF99 File Offset: 0x00019199
		// (set) Token: 0x060007C1 RID: 1985 RVA: 0x0001AFA1 File Offset: 0x000191A1
		[DataSourceProperty]
		public float ClosestFadeoutRange
		{
			get
			{
				return this._closestFadeoutRange;
			}
			set
			{
				if (this._closestFadeoutRange != value)
				{
					this._closestFadeoutRange = value;
					base.OnPropertyChangedWithValue(value, "ClosestFadeoutRange");
				}
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x060007C2 RID: 1986 RVA: 0x0001AFBF File Offset: 0x000191BF
		// (set) Token: 0x060007C3 RID: 1987 RVA: 0x0001AFC7 File Offset: 0x000191C7
		[DataSourceProperty]
		public float VisibilityRatio
		{
			get
			{
				return this._visibilityRatio;
			}
			set
			{
				if (!this._visibilityRatio.ApproximatelyEqualsTo(value, 1E-05f))
				{
					this._visibilityRatio = value;
					base.OnPropertyChangedWithValue(value, "VisibilityRatio");
				}
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x0001AFEF File Offset: 0x000191EF
		// (set) Token: 0x060007C5 RID: 1989 RVA: 0x0001AFF7 File Offset: 0x000191F7
		[DataSourceProperty]
		public float AlwaysOnDistance
		{
			get
			{
				return this._alwaysOnDistance;
			}
			set
			{
				if (!this._alwaysOnDistance.ApproximatelyEqualsTo(value, 1E-05f))
				{
					this._alwaysOnDistance = value;
					base.OnPropertyChangedWithValue(value, "AlwaysOnDistance");
				}
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x0001B01F File Offset: 0x0001921F
		// (set) Token: 0x060007C7 RID: 1991 RVA: 0x0001B027 File Offset: 0x00019227
		[DataSourceProperty]
		public int VisibilityState
		{
			get
			{
				return this._visibilityState;
			}
			set
			{
				if (value != this._visibilityState)
				{
					this._visibilityState = value;
					base.OnPropertyChangedWithValue(value, "VisibilityState");
				}
			}
		}

		// Token: 0x04000365 RID: 869
		private Vec2 _screenPosition;

		// Token: 0x04000366 RID: 870
		private float _distance;

		// Token: 0x04000367 RID: 871
		private string _distanceText;

		// Token: 0x04000368 RID: 872
		private bool _isEnabled;

		// Token: 0x04000369 RID: 873
		private bool _isCenterOfFocus;

		// Token: 0x0400036A RID: 874
		private bool _isFormationTargetRelevant;

		// Token: 0x0400036B RID: 875
		private bool _isTargetingAFormation;

		// Token: 0x0400036C RID: 876
		private bool _showDistanceTexts;

		// Token: 0x0400036D RID: 877
		private int _teamType;

		// Token: 0x0400036E RID: 878
		private int _size;

		// Token: 0x0400036F RID: 879
		private int _wSign;

		// Token: 0x04000370 RID: 880
		private string _formationType;

		// Token: 0x04000371 RID: 881
		private float _farAlphaTarget = 0.7f;

		// Token: 0x04000372 RID: 882
		private float _farDistanceCutoff = 500f;

		// Token: 0x04000373 RID: 883
		private float _closeDistanceCutoff = 10f;

		// Token: 0x04000374 RID: 884
		private float _closestFadeoutRange = 5f;

		// Token: 0x04000375 RID: 885
		private float _visibilityRatio = 1f;

		// Token: 0x04000376 RID: 886
		private float _alwaysOnDistance = 25f;

		// Token: 0x04000377 RID: 887
		private int _visibilityState = -1;

		// Token: 0x020000F1 RID: 241
		public enum TeamTypes
		{
			// Token: 0x0400068E RID: 1678
			PlayerTeam,
			// Token: 0x0400068F RID: 1679
			PlayerAllyTeam,
			// Token: 0x04000690 RID: 1680
			EnemyTeam
		}
	}
}
