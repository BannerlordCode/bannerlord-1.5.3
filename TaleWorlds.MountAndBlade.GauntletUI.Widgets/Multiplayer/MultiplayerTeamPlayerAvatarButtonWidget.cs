using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer
{
	// Token: 0x0200008F RID: 143
	public class MultiplayerTeamPlayerAvatarButtonWidget : ButtonWidget
	{
		// Token: 0x060007E6 RID: 2022 RVA: 0x00017091 File Offset: 0x00015291
		public MultiplayerTeamPlayerAvatarButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x000170A5 File Offset: 0x000152A5
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._isInitialized && this.AvatarImage != null)
			{
				this._originalAvatarImageAlpha = this.AvatarImage.ReadOnlyBrush.GlobalAlphaFactor;
				this.UpdateGlobalAlpha();
				this._isInitialized = true;
			}
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x000170E4 File Offset: 0x000152E4
		private void UpdateGlobalAlpha()
		{
			if (this._isInitialized)
			{
				float num = (this.IsDead ? this.DeathAlphaFactor : 1f);
				float num2 = num * this._originalAvatarImageAlpha;
				this.SetGlobalAlphaRecursively(num);
				this.AvatarImage.Brush.GlobalAlphaFactor = num2;
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x00017130 File Offset: 0x00015330
		// (set) Token: 0x060007EA RID: 2026 RVA: 0x00017138 File Offset: 0x00015338
		[DataSourceProperty]
		public bool IsDead
		{
			get
			{
				return this._isDead;
			}
			set
			{
				if (this._isDead != value)
				{
					this._isDead = value;
					base.OnPropertyChanged(value, "IsDead");
					this.UpdateGlobalAlpha();
				}
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x0001715C File Offset: 0x0001535C
		// (set) Token: 0x060007EC RID: 2028 RVA: 0x00017164 File Offset: 0x00015364
		[DataSourceProperty]
		public float DeathAlphaFactor
		{
			get
			{
				return this._deathAlphaFactor;
			}
			set
			{
				if (this._deathAlphaFactor != value)
				{
					this._deathAlphaFactor = value;
					base.OnPropertyChanged(value, "DeathAlphaFactor");
				}
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x00017182 File Offset: 0x00015382
		// (set) Token: 0x060007EE RID: 2030 RVA: 0x0001718A File Offset: 0x0001538A
		[DataSourceProperty]
		public ImageIdentifierWidget AvatarImage
		{
			get
			{
				return this._avatarImage;
			}
			set
			{
				if (this._avatarImage != value)
				{
					this._avatarImage = value;
					base.OnPropertyChanged<ImageIdentifierWidget>(value, "AvatarImage");
				}
			}
		}

		// Token: 0x04000377 RID: 887
		private bool _isInitialized;

		// Token: 0x04000378 RID: 888
		private float _originalAvatarImageAlpha = 1f;

		// Token: 0x04000379 RID: 889
		private bool _isDead;

		// Token: 0x0400037A RID: 890
		private float _deathAlphaFactor;

		// Token: 0x0400037B RID: 891
		private ImageIdentifierWidget _avatarImage;
	}
}
