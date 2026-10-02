using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000A1 RID: 161
	public class MultiplayerLobbyCosmeticAnimationControllerWidget : Widget
	{
		// Token: 0x060008B0 RID: 2224 RVA: 0x00019373 File Offset: 0x00017573
		private double GetRandomDoubleBetween(double min, double max)
		{
			return base.Context.UIRandom.NextDouble() * (max - min) + max;
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0001938C File Offset: 0x0001758C
		public MultiplayerLobbyCosmeticAnimationControllerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x000193E2 File Offset: 0x000175E2
		private void RestartAllAnimations()
		{
			this.SetAllAnimationPartColors();
			this.StopAllAnimations();
			this.StartAllAnimations();
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x000193F6 File Offset: 0x000175F6
		private void SetAllAnimationPartColors()
		{
			this.ApplyActionOnAllAnimations(new Action<MultiplayerLobbyCosmeticAnimationPartWidget>(this.SetColorOfPart));
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x0001940A File Offset: 0x0001760A
		private void StartAllAnimations()
		{
			this.ApplyActionOnAllAnimations(new Action<MultiplayerLobbyCosmeticAnimationPartWidget>(this.StartAnimationOfPart));
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0001941E File Offset: 0x0001761E
		private void StopAllAnimations()
		{
			this.ApplyActionOnAllAnimations(new Action<MultiplayerLobbyCosmeticAnimationPartWidget>(this.StopAnimationOfPart));
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x00019434 File Offset: 0x00017634
		private void StartAnimationOfPart(MultiplayerLobbyCosmeticAnimationPartWidget part)
		{
			double randomDoubleBetween = this.GetRandomDoubleBetween((double)this.MinAlphaChangeDuration, (double)this.MaxAlphaChangeDuration);
			double randomDoubleBetween2 = this.GetRandomDoubleBetween((double)this.MinAlphaLowerBound, (double)this.MinAlphaUpperBound);
			double randomDoubleBetween3 = this.GetRandomDoubleBetween((double)this.MaxAlphaLowerBound, (double)this.MaxAlphaUpperBound);
			part.StartAnimation((float)randomDoubleBetween, (float)randomDoubleBetween2, (float)randomDoubleBetween3);
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0001948C File Offset: 0x0001768C
		private void StopAnimationOfPart(MultiplayerLobbyCosmeticAnimationPartWidget part)
		{
			part.StopAnimation();
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x00019494 File Offset: 0x00017694
		private void SetColorOfPart(MultiplayerLobbyCosmeticAnimationPartWidget part)
		{
			switch (this.CosmeticRarity)
			{
			case 0:
			case 1:
				part.Color = this.RarityCommonColor;
				return;
			case 2:
				part.Color = this.RarityRareColor;
				return;
			case 3:
				part.Color = this.RarityUniqueColor;
				return;
			default:
				part.Color = MultiplayerLobbyCosmeticAnimationControllerWidget.DefaultColor;
				return;
			}
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x000194F4 File Offset: 0x000176F4
		private void ApplyActionOnAllAnimations(Action<MultiplayerLobbyCosmeticAnimationPartWidget> action)
		{
			BasicContainer animationPartContainer = this.AnimationPartContainer;
			if (animationPartContainer == null)
			{
				return;
			}
			animationPartContainer.Children.ForEach(delegate(Widget c)
			{
				action(c as MultiplayerLobbyCosmeticAnimationPartWidget);
			});
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x00019530 File Offset: 0x00017730
		private void OnAnimationPartAdded(Widget parent, Widget child)
		{
			MultiplayerLobbyCosmeticAnimationPartWidget multiplayerLobbyCosmeticAnimationPartWidget = child as MultiplayerLobbyCosmeticAnimationPartWidget;
			this.SetColorOfPart(multiplayerLobbyCosmeticAnimationPartWidget);
			this.StartAnimationOfPart(multiplayerLobbyCosmeticAnimationPartWidget);
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x060008BB RID: 2235 RVA: 0x00019552 File Offset: 0x00017752
		// (set) Token: 0x060008BC RID: 2236 RVA: 0x0001955A File Offset: 0x0001775A
		[Editor(false)]
		public int CosmeticRarity
		{
			get
			{
				return this._cosmeticRarity;
			}
			set
			{
				if (value != this._cosmeticRarity)
				{
					this._cosmeticRarity = value;
					base.OnPropertyChanged(value, "CosmeticRarity");
					this.RestartAllAnimations();
				}
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x0001957E File Offset: 0x0001777E
		// (set) Token: 0x060008BE RID: 2238 RVA: 0x00019586 File Offset: 0x00017786
		[Editor(false)]
		public float MinAlphaChangeDuration
		{
			get
			{
				return this._minAlphaChangeDuration;
			}
			set
			{
				if (this._minAlphaChangeDuration != value)
				{
					this._minAlphaChangeDuration = value;
					base.OnPropertyChanged(value, "MinAlphaChangeDuration");
					this.RestartAllAnimations();
				}
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x060008BF RID: 2239 RVA: 0x000195AA File Offset: 0x000177AA
		// (set) Token: 0x060008C0 RID: 2240 RVA: 0x000195B2 File Offset: 0x000177B2
		[Editor(false)]
		public float MaxAlphaChangeDuration
		{
			get
			{
				return this._maxAlphaChangeDuration;
			}
			set
			{
				if (this._maxAlphaChangeDuration != value)
				{
					this._maxAlphaChangeDuration = value;
					base.OnPropertyChanged(value, "MaxAlphaChangeDuration");
					this.RestartAllAnimations();
				}
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x060008C1 RID: 2241 RVA: 0x000195D6 File Offset: 0x000177D6
		// (set) Token: 0x060008C2 RID: 2242 RVA: 0x000195DE File Offset: 0x000177DE
		[Editor(false)]
		public float MinAlphaLowerBound
		{
			get
			{
				return this._minAlphaLowerBound;
			}
			set
			{
				if (this._minAlphaLowerBound != value)
				{
					this._minAlphaLowerBound = value;
					base.OnPropertyChanged(value, "MinAlphaLowerBound");
					this.RestartAllAnimations();
				}
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x060008C3 RID: 2243 RVA: 0x00019602 File Offset: 0x00017802
		// (set) Token: 0x060008C4 RID: 2244 RVA: 0x0001960A File Offset: 0x0001780A
		[Editor(false)]
		public float MinAlphaUpperBound
		{
			get
			{
				return this._minAlphaUpperBound;
			}
			set
			{
				if (this._minAlphaUpperBound != value)
				{
					this._minAlphaUpperBound = value;
					base.OnPropertyChanged(value, "MinAlphaUpperBound");
					this.RestartAllAnimations();
				}
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x0001962E File Offset: 0x0001782E
		// (set) Token: 0x060008C6 RID: 2246 RVA: 0x00019636 File Offset: 0x00017836
		[Editor(false)]
		public float MaxAlphaLowerBound
		{
			get
			{
				return this._maxAlphaLowerBound;
			}
			set
			{
				if (this._maxAlphaLowerBound != value)
				{
					this._maxAlphaLowerBound = value;
					base.OnPropertyChanged(value, "MaxAlphaLowerBound");
					this.RestartAllAnimations();
				}
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x0001965A File Offset: 0x0001785A
		// (set) Token: 0x060008C8 RID: 2248 RVA: 0x00019662 File Offset: 0x00017862
		[Editor(false)]
		public float MaxAlphaUpperBound
		{
			get
			{
				return this._maxAlphaUpperBound;
			}
			set
			{
				if (this._maxAlphaUpperBound != value)
				{
					this._maxAlphaUpperBound = value;
					base.OnPropertyChanged(value, "MaxAlphaUpperBound");
					this.RestartAllAnimations();
				}
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x00019686 File Offset: 0x00017886
		// (set) Token: 0x060008CA RID: 2250 RVA: 0x0001968E File Offset: 0x0001788E
		[Editor(false)]
		public Color RarityCommonColor
		{
			get
			{
				return this._rarityCommonColor;
			}
			set
			{
				if (this._rarityCommonColor != value)
				{
					this._rarityCommonColor = value;
					base.OnPropertyChanged(value, "RarityCommonColor");
					this.RestartAllAnimations();
				}
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x000196B7 File Offset: 0x000178B7
		// (set) Token: 0x060008CC RID: 2252 RVA: 0x000196BF File Offset: 0x000178BF
		[Editor(false)]
		public Color RarityRareColor
		{
			get
			{
				return this._rarityRareColor;
			}
			set
			{
				if (this._rarityRareColor != value)
				{
					this._rarityRareColor = value;
					base.OnPropertyChanged(value, "RarityRareColor");
					this.RestartAllAnimations();
				}
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x060008CD RID: 2253 RVA: 0x000196E8 File Offset: 0x000178E8
		// (set) Token: 0x060008CE RID: 2254 RVA: 0x000196F0 File Offset: 0x000178F0
		[Editor(false)]
		public Color RarityUniqueColor
		{
			get
			{
				return this._rarityUniqueColor;
			}
			set
			{
				if (this._rarityUniqueColor != value)
				{
					this._rarityUniqueColor = value;
					base.OnPropertyChanged(value, "RarityUniqueColor");
					this.RestartAllAnimations();
				}
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x060008CF RID: 2255 RVA: 0x00019719 File Offset: 0x00017919
		// (set) Token: 0x060008D0 RID: 2256 RVA: 0x00019724 File Offset: 0x00017924
		[Editor(false)]
		public BasicContainer AnimationPartContainer
		{
			get
			{
				return this._animationPartContainer;
			}
			set
			{
				if (value != this._animationPartContainer)
				{
					if (this._animationPartContainer != null)
					{
						this._animationPartContainer.ItemAddEventHandlers.Remove(new Action<Widget, Widget>(this.OnAnimationPartAdded));
					}
					this._animationPartContainer = value;
					if (this._animationPartContainer != null)
					{
						this._animationPartContainer.ItemAddEventHandlers.Add(new Action<Widget, Widget>(this.OnAnimationPartAdded));
					}
					base.OnPropertyChanged<BasicContainer>(value, "AnimationPartContainer");
					this.RestartAllAnimations();
				}
			}
		}

		// Token: 0x040003EF RID: 1007
		private static readonly Color DefaultColor = Color.FromUint(0U);

		// Token: 0x040003F0 RID: 1008
		private int _cosmeticRarity;

		// Token: 0x040003F1 RID: 1009
		private float _minAlphaChangeDuration = 1.5f;

		// Token: 0x040003F2 RID: 1010
		private float _maxAlphaChangeDuration = 2.5f;

		// Token: 0x040003F3 RID: 1011
		private float _minAlphaLowerBound = 0.4f;

		// Token: 0x040003F4 RID: 1012
		private float _minAlphaUpperBound = 0.6f;

		// Token: 0x040003F5 RID: 1013
		private float _maxAlphaLowerBound = 0.6f;

		// Token: 0x040003F6 RID: 1014
		private float _maxAlphaUpperBound = 0.8f;

		// Token: 0x040003F7 RID: 1015
		private Color _rarityCommonColor;

		// Token: 0x040003F8 RID: 1016
		private Color _rarityRareColor;

		// Token: 0x040003F9 RID: 1017
		private Color _rarityUniqueColor;

		// Token: 0x040003FA RID: 1018
		private BasicContainer _animationPartContainer;
	}
}
