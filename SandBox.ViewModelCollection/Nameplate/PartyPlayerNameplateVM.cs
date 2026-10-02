using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.ViewModelCollection.Nameplate
{
	// Token: 0x0200001B RID: 27
	public class PartyPlayerNameplateVM : PartyNameplateVM
	{
		// Token: 0x060002AD RID: 685 RVA: 0x0000BF6F File Offset: 0x0000A16F
		public PartyPlayerNameplateVM()
		{
			this.IsMainParty = true;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000BF8C File Offset: 0x0000A18C
		public void InitializePlayerNameplate(Action resetCamera)
		{
			this._isPartyHeroVisualDirty = true;
			this._resetCamera = resetCamera;
			bool flag;
			if (this.IsMainParty && base.Party.LeaderHero == null)
			{
				Hero mainHero = Hero.MainHero;
				flag = mainHero != null && mainHero.IsAlive;
			}
			else
			{
				flag = false;
			}
			this._isPrisonerBind = flag;
			this.MainHeroVisual = new CharacterImageIdentifierVM(CampaignUIHelper.GetCharacterCode(Hero.MainHero.CharacterObject, false));
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000BFF1 File Offset: 0x0000A1F1
		public override void Clear()
		{
			base.Clear();
			base.IsInSettlement = true;
			base.IsVisibleOnMap = false;
			base.IsShipBannerVisible = false;
			this.MainHeroVisual = null;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000C018 File Offset: 0x0000A218
		public override void RefreshDynamicProperties(bool forceUpdate)
		{
			base.RefreshDynamicProperties(forceUpdate);
			if ((this.IsMainParty && MathF.Abs(Hero.MainHero.Age - this._latestMainHeroAge) >= 1f) || forceUpdate)
			{
				this._latestMainHeroAge = Hero.MainHero.Age;
				this._isPartyHeroVisualDirty = true;
			}
			if (this._isPartyHeroVisualDirty || forceUpdate)
			{
				this._mainHeroVisualBind = new CharacterImageIdentifierVM(SandBoxUIHelper.GetCharacterCode(Hero.MainHero.CharacterObject, false));
				this._isPartyHeroVisualDirty = false;
			}
			bool flag;
			if (this.IsMainParty && base.Party.LeaderHero == null)
			{
				Hero mainHero = Hero.MainHero;
				flag = mainHero != null && mainHero.IsAlive;
			}
			else
			{
				flag = false;
			}
			this._isPrisonerBind = flag;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000C0CD File Offset: 0x0000A2CD
		public override void RefreshBinding()
		{
			base.RefreshBinding();
			this.IsPrisoner = this._isPrisonerBind;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000C0E4 File Offset: 0x0000A2E4
		public override void RefreshPosition()
		{
			this._latestX = 0f;
			this._latestY = 0f;
			this._latestW = 0f;
			Vec3 vec = (base.Party.Position + base.Party.EventPositionAdder).AsVec3();
			MapEvent mapEvent = base.Party.MapEvent;
			bool flag;
			if (mapEvent == null)
			{
				flag = false;
			}
			else
			{
				Settlement mapEventSettlement = mapEvent.MapEventSettlement;
				bool? flag2 = ((mapEventSettlement != null) ? new bool?(mapEventSettlement.IsVillage) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			bool flag4 = flag && base.Party.IsCurrentlyAtSea;
			if (flag4)
			{
				MBWindowManager.WorldToScreenInsideUsableArea(this._mapCamera, vec, ref this._latestX, ref this._latestY, ref this._latestW);
				this._shipBannerPositionBind = new Vec2(this._latestX, this._latestY);
				vec = base.Party.MapEvent.MapEventSettlement.GatePosition.AsVec3();
				vec += new Vec3(base.Party.RandomFloatWithSeed((uint)base.Party.RandomValue, -0.3f, 0.3f), base.Party.RandomFloatWithSeed((uint)base.Party.RandomValue, -0.3f, 0.3f), 0f, -1f);
			}
			Vec3 vec2 = vec + new Vec3(0f, 0f, 0.8f, -1f);
			MBWindowManager.WorldToScreenInsideUsableArea(this._mapCamera, vec, ref this._latestX, ref this._latestY, ref this._latestW);
			this._partyPositionBind = new Vec2(this._latestX, this._latestY);
			this._isHighBind = this._mapCamera.Position.Distance(vec) >= 110f;
			this._isBehindBind = this._latestW < 0f;
			MBWindowManager.WorldToScreenInsideUsableArea(this._mapCamera, vec2, ref this._latestX, ref this._latestY, ref this._latestW);
			this._headPositionBind = new Vec2(this._latestX, this._latestY);
			this._isShipBannerVisibleBind = flag4 && !this._isHighBind && !this._isBehindBind;
			base.DistanceToCamera = vec.Distance(this._mapCamera.Position);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000C339 File Offset: 0x0000A539
		public void ExecuteSetCameraPosition()
		{
			this._resetCamera();
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x0000C346 File Offset: 0x0000A546
		// (set) Token: 0x060002B5 RID: 693 RVA: 0x0000C34E File Offset: 0x0000A54E
		[DataSourceProperty]
		public bool IsMainParty
		{
			get
			{
				return this._isMainParty;
			}
			set
			{
				if (value != this._isMainParty)
				{
					this._isMainParty = value;
					base.OnPropertyChangedWithValue(value, "IsMainParty");
				}
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000C36C File Offset: 0x0000A56C
		// (set) Token: 0x060002B7 RID: 695 RVA: 0x0000C374 File Offset: 0x0000A574
		[DataSourceProperty]
		public bool IsPrisoner
		{
			get
			{
				return this._isPrisoner;
			}
			set
			{
				if (value != this._isPrisoner)
				{
					this._isPrisoner = value;
					base.OnPropertyChangedWithValue(value, "IsPrisoner");
				}
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x0000C392 File Offset: 0x0000A592
		// (set) Token: 0x060002B9 RID: 697 RVA: 0x0000C39A File Offset: 0x0000A59A
		[DataSourceProperty]
		public CharacterImageIdentifierVM MainHeroVisual
		{
			get
			{
				return this._mainHeroVisual;
			}
			set
			{
				if (value != this._mainHeroVisual)
				{
					this._mainHeroVisual = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "MainHeroVisual");
				}
			}
		}

		// Token: 0x04000158 RID: 344
		private float _latestMainHeroAge = -1f;

		// Token: 0x04000159 RID: 345
		private bool _isPartyHeroVisualDirty;

		// Token: 0x0400015A RID: 346
		private Action _resetCamera;

		// Token: 0x0400015B RID: 347
		private CharacterImageIdentifierVM _mainHeroVisualBind;

		// Token: 0x0400015C RID: 348
		private bool _isPrisonerBind;

		// Token: 0x0400015D RID: 349
		private bool _isMainParty;

		// Token: 0x0400015E RID: 350
		private bool _isPrisoner;

		// Token: 0x0400015F RID: 351
		private CharacterImageIdentifierVM _mainHeroVisual;
	}
}
