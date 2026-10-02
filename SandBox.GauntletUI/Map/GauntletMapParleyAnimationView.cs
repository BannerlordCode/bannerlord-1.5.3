using System;
using SandBox.View;
using SandBox.View.Map;
using SandBox.View.Map.Managers;
using SandBox.View.Map.Visuals;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x0200003D RID: 61
	[OverrideView(typeof(MapParleyAnimationView))]
	public class GauntletMapParleyAnimationView : MapParleyAnimationView
	{
		// Token: 0x060002E0 RID: 736 RVA: 0x000115BC File Offset: 0x0000F7BC
		public GauntletMapParleyAnimationView(PartyBase parleyedParty)
		{
			this._parleyedParty = parleyedParty;
			this._behavior = Campaign.Current.GetCampaignBehavior<IParleyCampaignBehavior>();
			foreach (EntityVisualManagerBase<PartyBase> entityVisualManagerBase in SandBoxViewSubModule.SandBoxViewVisualManager.GetComponents<EntityVisualManagerBase<PartyBase>>())
			{
				MapEntityVisual<PartyBase> visualOfEntity = entityVisualManagerBase.GetVisualOfEntity(PartyBase.MainParty);
				MapEntityVisual<PartyBase> visualOfEntity2 = entityVisualManagerBase.GetVisualOfEntity(this._parleyedParty);
				if (visualOfEntity != null)
				{
					this._mainPartyVisual = visualOfEntity;
				}
				if (visualOfEntity2 != null)
				{
					this._parleyedPartyVisual = visualOfEntity2;
				}
			}
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00011654 File Offset: 0x0000F854
		protected override void CreateLayout()
		{
			base.CreateLayout();
			this._remainingAnimationDuration = 1f;
			this.CreateBanners();
			MBInformationManager.AddQuickInformation(new TextObject("{=LZbHWkCB}Parleying with {PARTY_NAME}", null).SetTextVariable("PARTY_NAME", this._parleyedParty.Name), -750, null, null, "");
			this._previousTimeControlMode = Campaign.Current.TimeControlMode;
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			Campaign.Current.SetTimeControlModeLock(true);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x000116D0 File Offset: 0x0000F8D0
		private void CreateBanners()
		{
			this._playerBannerEntity = this.CreateAnimationBannerEntity(PartyBase.MainParty, this._mainPartyVisual);
			this._targetBannerEntity = this.CreateAnimationBannerEntity(this._parleyedParty, this._parleyedPartyVisual);
			if (this._parleyedParty.IsSettlement)
			{
				this._bannerTargetPosition = this._targetBannerEntity.GetFrame().origin;
			}
			else
			{
				this._bannerTargetPosition = Vec3.Lerp(this._playerBannerEntity.GetFrame().origin, this._targetBannerEntity.GetFrame().origin, 0.5f);
			}
			this.RotateBannersTowardsEachother(this._playerBannerEntity, this._targetBannerEntity, this._bannerTargetPosition);
			float num = 0.7f;
			Vec3 vec = new Vec3(num, num, num, -1f);
			this.ScaleBanner(this._playerBannerEntity, vec);
			this.ScaleBanner(this._targetBannerEntity, vec);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x000117A8 File Offset: 0x0000F9A8
		private GameEntity CreateAnimationBannerEntity(PartyBase party, MapEntityVisual<PartyBase> partyVisual)
		{
			GameEntity gameEntity = GameEntity.CreateEmpty(base.MapScreen.MapScene, false, true, true);
			MetaMesh copy = MetaMesh.GetCopy("map_banner", true, false);
			gameEntity.AddMultiMesh(copy, true);
			MatrixFrame identity = MatrixFrame.Identity;
			identity.origin = partyVisual.GetVisualPosition();
			gameEntity.SetFrame(ref identity, true);
			return gameEntity;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x000117FC File Offset: 0x0000F9FC
		private void RotateBannersTowardsEachother(GameEntity playerBanner, GameEntity targetBanner, Vec3 bannerTargetPosition)
		{
			MatrixFrame frame = playerBanner.GetFrame();
			MatrixFrame frame2 = targetBanner.GetFrame();
			Vec3 vec = bannerTargetPosition - frame.origin;
			frame.rotation.f = vec;
			frame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
			frame.rotation.RotateAboutUp(3.1415927f);
			frame2.rotation = frame.rotation;
			frame2.rotation.RotateAboutUp(3.1415927f);
			playerBanner.SetFrame(ref frame, true);
			targetBanner.SetFrame(ref frame2, true);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00011880 File Offset: 0x0000FA80
		private void ScaleBanner(GameEntity bannerEntity, Vec3 scaleVector)
		{
			MatrixFrame frame = bannerEntity.GetFrame();
			frame.Scale(in scaleVector);
			bannerEntity.SetFrame(ref frame, true);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x000118A6 File Offset: 0x0000FAA6
		private void DestroyAnimationBannerEntities()
		{
			GameEntity playerBannerEntity = this._playerBannerEntity;
			if (playerBannerEntity != null)
			{
				playerBannerEntity.Remove(0);
			}
			GameEntity targetBannerEntity = this._targetBannerEntity;
			if (targetBannerEntity != null)
			{
				targetBannerEntity.Remove(0);
			}
			this._playerBannerEntity = null;
			this._targetBannerEntity = null;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x000118DA File Offset: 0x0000FADA
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			this.Tick(dt);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x000118EA File Offset: 0x0000FAEA
		protected override void OnIdleTick(float dt)
		{
			base.OnIdleTick(dt);
			this.Tick(dt);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x000118FC File Offset: 0x0000FAFC
		private void Tick(float dt)
		{
			if (this._remainingAnimationDuration > 0f)
			{
				float num = MathF.Clamp((1f - this._remainingAnimationDuration) / 1f, 0f, 1f);
				Vec3 visualPosition = this._mainPartyVisual.GetVisualPosition();
				Vec3 visualPosition2 = this._parleyedPartyVisual.GetVisualPosition();
				MatrixFrame frame = this._playerBannerEntity.GetFrame();
				MatrixFrame frame2 = this._targetBannerEntity.GetFrame();
				frame.origin = Vec3.Lerp(visualPosition, this._bannerTargetPosition, num);
				frame2.origin = Vec3.Lerp(visualPosition2, this._bannerTargetPosition, num);
				this._playerBannerEntity.SetFrame(ref frame, true);
				this._targetBannerEntity.SetFrame(ref frame2, true);
				this._remainingAnimationDuration -= dt;
				return;
			}
			base.MapScreen.RemoveMapView(this);
			IParleyCampaignBehavior behavior = this._behavior;
			if (behavior == null)
			{
				return;
			}
			behavior.StartParley(this._parleyedParty);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x000119DE File Offset: 0x0000FBDE
		protected override void OnFinalize()
		{
			base.OnFinalize();
			this.DestroyAnimationBannerEntities();
			Campaign.Current.SetTimeControlModeLock(false);
			Campaign.Current.TimeControlMode = this._previousTimeControlMode;
		}

		// Token: 0x04000113 RID: 275
		private readonly PartyBase _parleyedParty;

		// Token: 0x04000114 RID: 276
		private CampaignTimeControlMode _previousTimeControlMode;

		// Token: 0x04000115 RID: 277
		private const float _animationDuration = 1f;

		// Token: 0x04000116 RID: 278
		private float _remainingAnimationDuration;

		// Token: 0x04000117 RID: 279
		private readonly IParleyCampaignBehavior _behavior;

		// Token: 0x04000118 RID: 280
		private GameEntity _playerBannerEntity;

		// Token: 0x04000119 RID: 281
		private GameEntity _targetBannerEntity;

		// Token: 0x0400011A RID: 282
		private Vec3 _bannerTargetPosition;

		// Token: 0x0400011B RID: 283
		private MapEntityVisual<PartyBase> _mainPartyVisual;

		// Token: 0x0400011C RID: 284
		private MapEntityVisual<PartyBase> _parleyedPartyVisual;
	}
}
