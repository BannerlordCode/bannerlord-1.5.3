using System;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI;
using TaleWorlds.MountAndBlade.View.Tableaus;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.TextureProviders
{
	// Token: 0x0200001F RID: 31
	public class CharacterTableauTextureProvider : TextureProvider
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00008985 File Offset: 0x00006B85
		public float CustomAnimationProgressRatio
		{
			get
			{
				return this._characterTableau.GetCustomAnimationProgressRatio();
			}
		}

		// Token: 0x1700002B RID: 43
		// (set) Token: 0x0600012A RID: 298 RVA: 0x00008992 File Offset: 0x00006B92
		public string BannerCodeText
		{
			set
			{
				this._characterTableau.SetBannerCode(value);
			}
		}

		// Token: 0x1700002C RID: 44
		// (set) Token: 0x0600012B RID: 299 RVA: 0x000089A0 File Offset: 0x00006BA0
		public string BodyProperties
		{
			set
			{
				this._characterTableau.SetBodyProperties(value);
			}
		}

		// Token: 0x1700002D RID: 45
		// (set) Token: 0x0600012C RID: 300 RVA: 0x000089AE File Offset: 0x00006BAE
		public int StanceIndex
		{
			set
			{
				this._characterTableau.SetStanceIndex(value);
			}
		}

		// Token: 0x1700002E RID: 46
		// (set) Token: 0x0600012D RID: 301 RVA: 0x000089BC File Offset: 0x00006BBC
		public bool IsFemale
		{
			set
			{
				this._characterTableau.SetIsFemale(value);
			}
		}

		// Token: 0x1700002F RID: 47
		// (set) Token: 0x0600012E RID: 302 RVA: 0x000089CA File Offset: 0x00006BCA
		public int Race
		{
			set
			{
				this._characterTableau.SetRace(value);
			}
		}

		// Token: 0x17000030 RID: 48
		// (set) Token: 0x0600012F RID: 303 RVA: 0x000089D8 File Offset: 0x00006BD8
		public bool IsBannerShownInBackground
		{
			set
			{
				this._characterTableau.SetIsBannerShownInBackground(value);
			}
		}

		// Token: 0x17000031 RID: 49
		// (set) Token: 0x06000130 RID: 304 RVA: 0x000089E6 File Offset: 0x00006BE6
		public bool IsEquipmentAnimActive
		{
			set
			{
				this._characterTableau.SetIsEquipmentAnimActive(value);
			}
		}

		// Token: 0x17000032 RID: 50
		// (set) Token: 0x06000131 RID: 305 RVA: 0x000089F4 File Offset: 0x00006BF4
		public string EquipmentCode
		{
			set
			{
				this._characterTableau.SetEquipmentCode(value);
			}
		}

		// Token: 0x17000033 RID: 51
		// (set) Token: 0x06000132 RID: 306 RVA: 0x00008A02 File Offset: 0x00006C02
		public string IdleAction
		{
			set
			{
				this._characterTableau.SetIdleAction(value);
			}
		}

		// Token: 0x17000034 RID: 52
		// (set) Token: 0x06000133 RID: 307 RVA: 0x00008A10 File Offset: 0x00006C10
		public string IdleFaceAnim
		{
			set
			{
				this._characterTableau.SetIdleFaceAnim(value);
			}
		}

		// Token: 0x17000035 RID: 53
		// (set) Token: 0x06000134 RID: 308 RVA: 0x00008A1E File Offset: 0x00006C1E
		public bool CurrentlyRotating
		{
			set
			{
				this._characterTableau.RotateCharacter(value);
			}
		}

		// Token: 0x17000036 RID: 54
		// (set) Token: 0x06000135 RID: 309 RVA: 0x00008A2C File Offset: 0x00006C2C
		public string MountCreationKey
		{
			set
			{
				this._characterTableau.SetMountCreationKey(value);
			}
		}

		// Token: 0x17000037 RID: 55
		// (set) Token: 0x06000136 RID: 310 RVA: 0x00008A3A File Offset: 0x00006C3A
		public uint ArmorColor1
		{
			set
			{
				this._characterTableau.SetArmorColor1(value);
			}
		}

		// Token: 0x17000038 RID: 56
		// (set) Token: 0x06000137 RID: 311 RVA: 0x00008A48 File Offset: 0x00006C48
		public uint ArmorColor2
		{
			set
			{
				this._characterTableau.SetArmorColor2(value);
			}
		}

		// Token: 0x17000039 RID: 57
		// (set) Token: 0x06000138 RID: 312 RVA: 0x00008A56 File Offset: 0x00006C56
		public string CharStringId
		{
			set
			{
				this._characterTableau.SetCharStringID(value);
			}
		}

		// Token: 0x1700003A RID: 58
		// (set) Token: 0x06000139 RID: 313 RVA: 0x00008A64 File Offset: 0x00006C64
		public bool TriggerCharacterMountPlacesSwap
		{
			set
			{
				this._characterTableau.TriggerCharacterMountPlacesSwap();
			}
		}

		// Token: 0x1700003B RID: 59
		// (set) Token: 0x0600013A RID: 314 RVA: 0x00008A71 File Offset: 0x00006C71
		public float CustomRenderScale
		{
			set
			{
				this._characterTableau.SetCustomRenderScale(value);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00008A7F File Offset: 0x00006C7F
		// (set) Token: 0x0600013C RID: 316 RVA: 0x00008A92 File Offset: 0x00006C92
		public bool IsPlayingCustomAnimations
		{
			get
			{
				CharacterTableau characterTableau = this._characterTableau;
				return characterTableau != null && characterTableau.IsRunningCustomAnimation;
			}
			set
			{
				if (value)
				{
					this._characterTableau.StartCustomAnimation();
					return;
				}
				this._characterTableau.StopCustomAnimation();
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00008AAE File Offset: 0x00006CAE
		// (set) Token: 0x0600013E RID: 318 RVA: 0x00008ABB File Offset: 0x00006CBB
		public bool ShouldLoopCustomAnimation
		{
			get
			{
				return this._characterTableau.ShouldLoopCustomAnimation;
			}
			set
			{
				this._characterTableau.ShouldLoopCustomAnimation = value;
			}
		}

		// Token: 0x1700003E RID: 62
		// (set) Token: 0x0600013F RID: 319 RVA: 0x00008AC9 File Offset: 0x00006CC9
		public int LeftHandWieldedEquipmentIndex
		{
			set
			{
				this._characterTableau.SetLeftHandWieldedEquipmentIndex(value);
			}
		}

		// Token: 0x1700003F RID: 63
		// (set) Token: 0x06000140 RID: 320 RVA: 0x00008AD7 File Offset: 0x00006CD7
		public int RightHandWieldedEquipmentIndex
		{
			set
			{
				this._characterTableau.SetRightHandWieldedEquipmentIndex(value);
			}
		}

		// Token: 0x17000040 RID: 64
		// (set) Token: 0x06000141 RID: 321 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public float CustomAnimationWaitDuration
		{
			set
			{
				this._characterTableau.CustomAnimationWaitDuration = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (set) Token: 0x06000142 RID: 322 RVA: 0x00008AF3 File Offset: 0x00006CF3
		public string CustomAnimation
		{
			set
			{
				this._characterTableau.SetCustomAnimation(value);
			}
		}

		// Token: 0x17000042 RID: 66
		// (set) Token: 0x06000143 RID: 323 RVA: 0x00008B01 File Offset: 0x00006D01
		public bool IsTableauEnabled
		{
			set
			{
				this._characterTableau.SetEnabled(value);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00008B21 File Offset: 0x00006D21
		// (set) Token: 0x06000144 RID: 324 RVA: 0x00008B0F File Offset: 0x00006D0F
		public bool IsHidden
		{
			get
			{
				return this._isHidden;
			}
			set
			{
				if (this._isHidden != value)
				{
					this._isHidden = value;
				}
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00008B29 File Offset: 0x00006D29
		public CharacterTableauTextureProvider()
		{
			this._characterTableau = new CharacterTableau();
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00008B3C File Offset: 0x00006D3C
		public override void Clear(bool clearNextFrame)
		{
			this._characterTableau.OnFinalize();
			base.Clear(clearNextFrame);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00008B50 File Offset: 0x00006D50
		private void CheckTexture()
		{
			if (this._texture != this._characterTableau.Texture)
			{
				this._texture = this._characterTableau.Texture;
				if (this._texture != null)
				{
					EngineTexture engineTexture = new EngineTexture(this._texture);
					this._providedTexture = new TaleWorlds.TwoDimension.Texture(engineTexture);
					return;
				}
				this._providedTexture = null;
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00008BB4 File Offset: 0x00006DB4
		protected override TaleWorlds.TwoDimension.Texture OnGetTextureForRender(TwoDimensionContext twoDimensionContext, string name)
		{
			this.CheckTexture();
			return this._providedTexture;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00008BC2 File Offset: 0x00006DC2
		public override void SetTargetSize(int width, int height)
		{
			base.SetTargetSize(width, height);
			this._characterTableau.SetTargetSize(width, height);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00008BD9 File Offset: 0x00006DD9
		public override void Tick(float dt)
		{
			base.Tick(dt);
			this.CheckTexture();
			this._characterTableau.OnTick(dt);
		}

		// Token: 0x040000B6 RID: 182
		private CharacterTableau _characterTableau;

		// Token: 0x040000B7 RID: 183
		private TaleWorlds.Engine.Texture _texture;

		// Token: 0x040000B8 RID: 184
		private TaleWorlds.TwoDimension.Texture _providedTexture;

		// Token: 0x040000B9 RID: 185
		private bool _isHidden;
	}
}
