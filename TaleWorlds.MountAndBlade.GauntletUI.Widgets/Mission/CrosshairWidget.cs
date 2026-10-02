using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000DD RID: 221
	public class CrosshairWidget : Widget
	{
		// Token: 0x06000B3F RID: 2879 RVA: 0x0001FB39 File Offset: 0x0001DD39
		public CrosshairWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x0001FB44 File Offset: 0x0001DD44
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (base.IsVisible)
			{
				base.SuggestedWidth = (float)((int)(74.0 + this.CrosshairAccuracy * 300.0));
				base.SuggestedHeight = (float)((int)(74.0 + this.CrosshairAccuracy * 300.0));
			}
			this.LeftArrow.Brush.AlphaFactor = (float)this.LeftArrowOpacity;
			this.RightArrow.Brush.AlphaFactor = (float)this.RightArrowOpacity;
			this.TopArrow.Brush.AlphaFactor = (float)this.TopArrowOpacity;
			this.BottomArrow.Brush.AlphaFactor = (float)this.BottomArrowOpacity;
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x0001FC00 File Offset: 0x0001DE00
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			child.AddState("Invalid");
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x0001FC14 File Offset: 0x0001DE14
		private void HitMarkerUpdated()
		{
			if (this.HitMarker != null)
			{
				this.HitMarker.AddState("Show");
			}
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x0001FC2E File Offset: 0x0001DE2E
		private void HeadshotMarkerUpdated()
		{
			if (this.HeadshotMarker != null)
			{
				this.HitMarker.AddState("Show");
			}
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x0001FC48 File Offset: 0x0001DE48
		private void ShowHitMarkerChanged()
		{
			if (this.HitMarker == null)
			{
				return;
			}
			string text = (this.IsVictimDead ? "ShowDeath" : "Show");
			if (this.HitMarker.CurrentState != text)
			{
				this.HitMarker.SetState(text);
				return;
			}
			this.HitMarker.BrushRenderer.RestartAnimation();
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x0001FCA4 File Offset: 0x0001DEA4
		private void ShowHeadshotMarkerChanged()
		{
			if (this.HeadshotMarker == null)
			{
				return;
			}
			string text = (this.IsHumanoidHeadshot ? "Show" : "Default");
			if (this.HeadshotMarker.CurrentState != text)
			{
				this.HeadshotMarker.SetState(text);
			}
			this.HeadshotMarker.BrushRenderer.RestartAnimation();
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06000B46 RID: 2886 RVA: 0x0001FCFE File Offset: 0x0001DEFE
		// (set) Token: 0x06000B47 RID: 2887 RVA: 0x0001FD06 File Offset: 0x0001DF06
		[Editor(false)]
		public double TopArrowOpacity
		{
			get
			{
				return this._topArrowOpacity;
			}
			set
			{
				if (value != this._topArrowOpacity)
				{
					this._topArrowOpacity = value;
					base.OnPropertyChanged(value, "TopArrowOpacity");
				}
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06000B48 RID: 2888 RVA: 0x0001FD24 File Offset: 0x0001DF24
		// (set) Token: 0x06000B49 RID: 2889 RVA: 0x0001FD2C File Offset: 0x0001DF2C
		[Editor(false)]
		public double BottomArrowOpacity
		{
			get
			{
				return this._bottomArrowOpacity;
			}
			set
			{
				if (value != this._bottomArrowOpacity)
				{
					this._bottomArrowOpacity = value;
					base.OnPropertyChanged(value, "BottomArrowOpacity");
				}
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06000B4A RID: 2890 RVA: 0x0001FD4A File Offset: 0x0001DF4A
		// (set) Token: 0x06000B4B RID: 2891 RVA: 0x0001FD52 File Offset: 0x0001DF52
		[Editor(false)]
		public double RightArrowOpacity
		{
			get
			{
				return this._rightArrowOpacity;
			}
			set
			{
				if (value != this._rightArrowOpacity)
				{
					this._rightArrowOpacity = value;
					base.OnPropertyChanged(value, "RightArrowOpacity");
				}
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06000B4C RID: 2892 RVA: 0x0001FD70 File Offset: 0x0001DF70
		// (set) Token: 0x06000B4D RID: 2893 RVA: 0x0001FD78 File Offset: 0x0001DF78
		[Editor(false)]
		public double LeftArrowOpacity
		{
			get
			{
				return this._leftArrowOpacity;
			}
			set
			{
				if (value != this._leftArrowOpacity)
				{
					this._leftArrowOpacity = value;
					base.OnPropertyChanged(value, "LeftArrowOpacity");
				}
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06000B4E RID: 2894 RVA: 0x0001FD96 File Offset: 0x0001DF96
		// (set) Token: 0x06000B4F RID: 2895 RVA: 0x0001FDA0 File Offset: 0x0001DFA0
		[Editor(false)]
		public bool IsTargetInvalid
		{
			get
			{
				return this._isTargetInvalid;
			}
			set
			{
				if (value != this._isTargetInvalid)
				{
					this._isTargetInvalid = value;
					base.OnPropertyChanged(value, "IsTargetInvalid");
					base.ApplyActionToAllChildrenRecursive(delegate(Widget child)
					{
						child.SetState(value ? "Invalid" : "Default");
					});
				}
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06000B50 RID: 2896 RVA: 0x0001FDF7 File Offset: 0x0001DFF7
		// (set) Token: 0x06000B51 RID: 2897 RVA: 0x0001FDFF File Offset: 0x0001DFFF
		[Editor(false)]
		public double CrosshairAccuracy
		{
			get
			{
				return this._crosshairAccuracy;
			}
			set
			{
				if (value != this._crosshairAccuracy)
				{
					this._crosshairAccuracy = value;
					base.OnPropertyChanged(value, "CrosshairAccuracy");
				}
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06000B52 RID: 2898 RVA: 0x0001FE1D File Offset: 0x0001E01D
		// (set) Token: 0x06000B53 RID: 2899 RVA: 0x0001FE25 File Offset: 0x0001E025
		[Editor(false)]
		public double CrosshairScale
		{
			get
			{
				return this._crosshairScale;
			}
			set
			{
				if (value != this._crosshairScale)
				{
					this._crosshairScale = value;
					base.OnPropertyChanged(value, "CrosshairScale");
				}
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06000B54 RID: 2900 RVA: 0x0001FE43 File Offset: 0x0001E043
		// (set) Token: 0x06000B55 RID: 2901 RVA: 0x0001FE4B File Offset: 0x0001E04B
		[Editor(false)]
		public bool IsVictimDead
		{
			get
			{
				return this._isVictimDead;
			}
			set
			{
				if (value != this._isVictimDead)
				{
					this._isVictimDead = value;
					base.OnPropertyChanged(value, "IsVictimDead");
				}
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06000B56 RID: 2902 RVA: 0x0001FE69 File Offset: 0x0001E069
		// (set) Token: 0x06000B57 RID: 2903 RVA: 0x0001FE71 File Offset: 0x0001E071
		[Editor(false)]
		public bool IsHumanoidHeadshot
		{
			get
			{
				return this._isHumanoidHeadshot;
			}
			set
			{
				if (value != this._isHumanoidHeadshot)
				{
					this._isHumanoidHeadshot = value;
					base.OnPropertyChanged(value, "IsHumanoidHeadshot");
					this.ShowHeadshotMarkerChanged();
				}
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06000B58 RID: 2904 RVA: 0x0001FE95 File Offset: 0x0001E095
		// (set) Token: 0x06000B59 RID: 2905 RVA: 0x0001FE9D File Offset: 0x0001E09D
		[Editor(false)]
		public bool ShowHitMarker
		{
			get
			{
				return this._showHitMarker;
			}
			set
			{
				if (value != this._showHitMarker)
				{
					this._showHitMarker = value;
					base.OnPropertyChanged(value, "ShowHitMarker");
					this.ShowHitMarkerChanged();
				}
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06000B5A RID: 2906 RVA: 0x0001FEC1 File Offset: 0x0001E0C1
		// (set) Token: 0x06000B5B RID: 2907 RVA: 0x0001FEC9 File Offset: 0x0001E0C9
		[Editor(false)]
		public BrushWidget LeftArrow
		{
			get
			{
				return this._leftArrow;
			}
			set
			{
				if (value != this._leftArrow)
				{
					this._leftArrow = value;
					base.OnPropertyChanged<BrushWidget>(value, "LeftArrow");
				}
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06000B5C RID: 2908 RVA: 0x0001FEE7 File Offset: 0x0001E0E7
		// (set) Token: 0x06000B5D RID: 2909 RVA: 0x0001FEEF File Offset: 0x0001E0EF
		[Editor(false)]
		public BrushWidget RightArrow
		{
			get
			{
				return this._rightArrow;
			}
			set
			{
				if (value != this._rightArrow)
				{
					this._rightArrow = value;
					base.OnPropertyChanged<BrushWidget>(value, "RightArrow");
				}
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06000B5E RID: 2910 RVA: 0x0001FF0D File Offset: 0x0001E10D
		// (set) Token: 0x06000B5F RID: 2911 RVA: 0x0001FF15 File Offset: 0x0001E115
		[Editor(false)]
		public BrushWidget TopArrow
		{
			get
			{
				return this._topArrow;
			}
			set
			{
				if (value != this._topArrow)
				{
					this._topArrow = value;
					base.OnPropertyChanged<BrushWidget>(value, "TopArrow");
				}
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06000B60 RID: 2912 RVA: 0x0001FF33 File Offset: 0x0001E133
		// (set) Token: 0x06000B61 RID: 2913 RVA: 0x0001FF3B File Offset: 0x0001E13B
		[Editor(false)]
		public BrushWidget BottomArrow
		{
			get
			{
				return this._bottomArrow;
			}
			set
			{
				if (value != this._bottomArrow)
				{
					this._bottomArrow = value;
					base.OnPropertyChanged<BrushWidget>(value, "BottomArrow");
				}
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06000B62 RID: 2914 RVA: 0x0001FF59 File Offset: 0x0001E159
		// (set) Token: 0x06000B63 RID: 2915 RVA: 0x0001FF61 File Offset: 0x0001E161
		[Editor(false)]
		public BrushWidget HitMarker
		{
			get
			{
				return this._hitMarker;
			}
			set
			{
				if (value != this._hitMarker)
				{
					this._hitMarker = value;
					base.OnPropertyChanged<BrushWidget>(value, "HitMarker");
					this.HitMarkerUpdated();
				}
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06000B64 RID: 2916 RVA: 0x0001FF85 File Offset: 0x0001E185
		// (set) Token: 0x06000B65 RID: 2917 RVA: 0x0001FF8D File Offset: 0x0001E18D
		[Editor(false)]
		public BrushWidget HeadshotMarker
		{
			get
			{
				return this._headshotMarker;
			}
			set
			{
				if (value != this._headshotMarker)
				{
					this._headshotMarker = value;
					base.OnPropertyChanged<BrushWidget>(value, "HeadshotMarker");
					this.HeadshotMarkerUpdated();
				}
			}
		}

		// Token: 0x04000516 RID: 1302
		private double _crosshairAccuracy;

		// Token: 0x04000517 RID: 1303
		private double _crosshairScale;

		// Token: 0x04000518 RID: 1304
		private bool _isTargetInvalid;

		// Token: 0x04000519 RID: 1305
		private double _topArrowOpacity;

		// Token: 0x0400051A RID: 1306
		private double _bottomArrowOpacity;

		// Token: 0x0400051B RID: 1307
		private double _rightArrowOpacity;

		// Token: 0x0400051C RID: 1308
		private double _leftArrowOpacity;

		// Token: 0x0400051D RID: 1309
		private bool _isVictimDead;

		// Token: 0x0400051E RID: 1310
		private bool _showHitMarker;

		// Token: 0x0400051F RID: 1311
		private bool _isHumanoidHeadshot;

		// Token: 0x04000520 RID: 1312
		private BrushWidget _leftArrow;

		// Token: 0x04000521 RID: 1313
		private BrushWidget _rightArrow;

		// Token: 0x04000522 RID: 1314
		private BrushWidget _topArrow;

		// Token: 0x04000523 RID: 1315
		private BrushWidget _bottomArrow;

		// Token: 0x04000524 RID: 1316
		private BrushWidget _hitMarker;

		// Token: 0x04000525 RID: 1317
		private BrushWidget _headshotMarker;
	}
}
