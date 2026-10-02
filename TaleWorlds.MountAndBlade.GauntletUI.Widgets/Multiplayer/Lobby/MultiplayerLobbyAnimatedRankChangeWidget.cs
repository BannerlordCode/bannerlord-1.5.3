using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x0200009D RID: 157
	public class MultiplayerLobbyAnimatedRankChangeWidget : Widget
	{
		// Token: 0x06000887 RID: 2183 RVA: 0x000189F0 File Offset: 0x00016BF0
		public MultiplayerLobbyAnimatedRankChangeWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x00018A10 File Offset: 0x00016C10
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this.IsAnimationRequested)
			{
				return;
			}
			if (this._preAnimationTimeElapsed < this._animationDelay)
			{
				this._preAnimationTimeElapsed += dt;
				return;
			}
			if (this._animationTimeElapsed >= this._animationDuration)
			{
				this.NewRankName.SetGlobalAlphaRecursively(1f);
				this.NewRankSprite.SetGlobalAlphaRecursively(1f);
				this.OldRankName.SetGlobalAlphaRecursively(0f);
				this.OldRankSprite.SetGlobalAlphaRecursively(0f);
				this.NewRankSprite.ScaledSuggestedWidth = base.ScaledSuggestedWidth / 2f;
				this.NewRankSprite.ScaledSuggestedHeight = base.ScaledSuggestedHeight / 2f;
				return;
			}
			float num = MathF.Lerp(0f, 1f, this._animationTimeElapsed / this._animationDuration, 1E-05f);
			this.OldRankSprite.SetGlobalAlphaRecursively(1f - num);
			this.OldRankName.SetGlobalAlphaRecursively(1f - num);
			this.NewRankSprite.SetGlobalAlphaRecursively(num);
			this.NewRankName.SetGlobalAlphaRecursively(num);
			this.NewRankSprite.ScaledSuggestedWidth = MathF.Lerp(base.ScaledSuggestedWidth, base.ScaledSuggestedWidth / 2f, this._animationTimeElapsed / this._animationDuration, 1E-05f);
			this.NewRankSprite.ScaledSuggestedHeight = MathF.Lerp(base.ScaledSuggestedHeight, base.ScaledSuggestedHeight / 2f, this._animationTimeElapsed / this._animationDuration, 1E-05f);
			this._animationTimeElapsed += dt;
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00018B9C File Offset: 0x00016D9C
		private void StartAnimation()
		{
			this.NewRankName.SetGlobalAlphaRecursively(0f);
			this.NewRankSprite.SetGlobalAlphaRecursively(0f);
			this.OldRankName.SetGlobalAlphaRecursively(1f);
			this.OldRankSprite.SetGlobalAlphaRecursively(1f);
			this.OldRankSprite.ScaledSuggestedWidth = base.ScaledSuggestedWidth / 2f;
			this.OldRankSprite.ScaledSuggestedHeight = base.ScaledSuggestedHeight / 2f;
			this._preAnimationTimeElapsed = 0f;
			this._animationTimeElapsed = 0f;
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x00018C2D File Offset: 0x00016E2D
		// (set) Token: 0x0600088B RID: 2187 RVA: 0x00018C35 File Offset: 0x00016E35
		[Editor(false)]
		public bool IsAnimationRequested
		{
			get
			{
				return this._isAnimationRequested;
			}
			set
			{
				if (value != this._isAnimationRequested)
				{
					this._isAnimationRequested = value;
					base.OnPropertyChanged(value, "IsAnimationRequested");
					this.StartAnimation();
				}
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x00018C59 File Offset: 0x00016E59
		// (set) Token: 0x0600088D RID: 2189 RVA: 0x00018C61 File Offset: 0x00016E61
		[Editor(false)]
		public bool IsPromoted
		{
			get
			{
				return this._isPromoted;
			}
			set
			{
				if (value != this._isPromoted)
				{
					this._isPromoted = value;
					base.OnPropertyChanged(value, "IsPromoted");
				}
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x00018C7F File Offset: 0x00016E7F
		// (set) Token: 0x0600088F RID: 2191 RVA: 0x00018C87 File Offset: 0x00016E87
		[Editor(false)]
		public TextWidget OldRankName
		{
			get
			{
				return this._oldRankName;
			}
			set
			{
				if (value != this._oldRankName)
				{
					this._oldRankName = value;
					base.OnPropertyChanged<TextWidget>(value, "OldRankName");
				}
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x00018CA5 File Offset: 0x00016EA5
		// (set) Token: 0x06000891 RID: 2193 RVA: 0x00018CAD File Offset: 0x00016EAD
		[Editor(false)]
		public TextWidget NewRankName
		{
			get
			{
				return this._newRankName;
			}
			set
			{
				if (value != this._newRankName)
				{
					this._newRankName = value;
					base.OnPropertyChanged<TextWidget>(value, "NewRankName");
				}
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000892 RID: 2194 RVA: 0x00018CCB File Offset: 0x00016ECB
		// (set) Token: 0x06000893 RID: 2195 RVA: 0x00018CD3 File Offset: 0x00016ED3
		[Editor(false)]
		public MultiplayerLobbyRankItemButtonWidget OldRankSprite
		{
			get
			{
				return this._oldRankSprite;
			}
			set
			{
				if (value != this._oldRankSprite)
				{
					this._oldRankSprite = value;
					base.OnPropertyChanged<MultiplayerLobbyRankItemButtonWidget>(value, "OldRankSprite");
				}
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x00018CF1 File Offset: 0x00016EF1
		// (set) Token: 0x06000895 RID: 2197 RVA: 0x00018CF9 File Offset: 0x00016EF9
		[Editor(false)]
		public MultiplayerLobbyRankItemButtonWidget NewRankSprite
		{
			get
			{
				return this._newRankSprite;
			}
			set
			{
				if (value != this._newRankSprite)
				{
					this._newRankSprite = value;
					base.OnPropertyChanged<MultiplayerLobbyRankItemButtonWidget>(value, "NewRankSprite");
				}
			}
		}

		// Token: 0x040003CB RID: 971
		private float _animationTimeElapsed;

		// Token: 0x040003CC RID: 972
		private float _animationDuration = 0.25f;

		// Token: 0x040003CD RID: 973
		private float _preAnimationTimeElapsed;

		// Token: 0x040003CE RID: 974
		private float _animationDelay = 0.5f;

		// Token: 0x040003CF RID: 975
		private bool _isAnimationRequested;

		// Token: 0x040003D0 RID: 976
		private bool _isPromoted;

		// Token: 0x040003D1 RID: 977
		private TextWidget _oldRankName;

		// Token: 0x040003D2 RID: 978
		private TextWidget _newRankName;

		// Token: 0x040003D3 RID: 979
		private MultiplayerLobbyRankItemButtonWidget _oldRankSprite;

		// Token: 0x040003D4 RID: 980
		private MultiplayerLobbyRankItemButtonWidget _newRankSprite;
	}
}
