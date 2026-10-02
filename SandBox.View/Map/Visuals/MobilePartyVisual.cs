using System;
using System.Threading;
using Helpers;
using SandBox.View.Map.Managers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;

namespace SandBox.View.Map.Visuals
{
	// Token: 0x02000066 RID: 102
	public class MobilePartyVisual : MapEntityVisual<PartyBase>
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x0001F537 File Offset: 0x0001D737
		public override float BearingRotation
		{
			get
			{
				return this._bearingRotation;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x0001F540 File Offset: 0x0001D740
		private Scene MapScene
		{
			get
			{
				if (this._mapScene == null && Campaign.Current != null && Campaign.Current.MapSceneWrapper != null)
				{
					this._mapScene = ((MapScene)Campaign.Current.MapSceneWrapper).Scene;
				}
				return this._mapScene;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x0001F58E File Offset: 0x0001D78E
		public override MapEntityVisual AttachedTo
		{
			get
			{
				MobileParty mobileParty = base.MapEntity.MobileParty;
				if (((mobileParty != null) ? mobileParty.AttachedTo : null) != null)
				{
					return MobilePartyVisualManager.Current.GetVisualOfEntity(base.MapEntity.MobileParty.AttachedTo.Party);
				}
				return null;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x0001F5CA File Offset: 0x0001D7CA
		public override CampaignVec2 InteractionPositionForPlayer
		{
			get
			{
				return ((IInteractablePoint)base.MapEntity).GetInteractionPosition(MobileParty.MainParty);
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x0001F5DC File Offset: 0x0001D7DC
		public override bool IsMobileEntity
		{
			get
			{
				return base.MapEntity.IsMobile;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x0001F5E9 File Offset: 0x0001D7E9
		public override bool IsMainEntity
		{
			get
			{
				return base.MapEntity == PartyBase.MainParty;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x0001F5F8 File Offset: 0x0001D7F8
		// (set) Token: 0x06000418 RID: 1048 RVA: 0x0001F600 File Offset: 0x0001D800
		public GameEntity StrategicEntity { get; private set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x0001F609 File Offset: 0x0001D809
		// (set) Token: 0x0600041A RID: 1050 RVA: 0x0001F611 File Offset: 0x0001D811
		public AgentVisuals HumanAgentVisuals { get; private set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x0001F61A File Offset: 0x0001D81A
		// (set) Token: 0x0600041C RID: 1052 RVA: 0x0001F622 File Offset: 0x0001D822
		public AgentVisuals MountAgentVisuals { get; private set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x0001F62B File Offset: 0x0001D82B
		// (set) Token: 0x0600041E RID: 1054 RVA: 0x0001F633 File Offset: 0x0001D833
		public AgentVisuals CaravanMountAgentVisuals { get; private set; }

		// Token: 0x0600041F RID: 1055 RVA: 0x0001F63C File Offset: 0x0001D83C
		public MobilePartyVisual(PartyBase partyBase)
			: base(partyBase)
		{
			this.CircleLocalFrame = MatrixFrame.Identity;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0001F650 File Offset: 0x0001D850
		public override bool IsEnemyOf(IFaction faction)
		{
			return FactionManager.IsAtWarAgainstFaction(base.MapEntity.MapFaction, faction.MapFaction);
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0001F668 File Offset: 0x0001D868
		public override bool IsInSameFaction(IFaction faction)
		{
			return DiplomacyHelper.IsSameFactionAndNotEliminated(base.MapEntity.MapFaction, faction.MapFaction);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0001F680 File Offset: 0x0001D880
		public override bool IsAllyOf(IFaction faction)
		{
			return DiplomacyHelper.HasAllianceWithFaction(base.MapEntity.MapFaction, faction.MapFaction);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0001F698 File Offset: 0x0001D898
		internal void OnPartyRemoved()
		{
			if (this.StrategicEntity != null)
			{
				this.RemoveVisualFromVisualsOfEntities();
				this.ReleaseResources();
				this.StrategicEntity.Remove(111);
			}
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0001F6C4 File Offset: 0x0001D8C4
		public override void OnTrackAction()
		{
			MobileParty mobileParty = base.MapEntity.MobileParty;
			if (mobileParty != null)
			{
				if (Campaign.Current.VisualTrackerManager.CheckTracked(mobileParty))
				{
					Campaign.Current.VisualTrackerManager.RemoveTrackedObject(mobileParty, false);
					return;
				}
				Campaign.Current.VisualTrackerManager.RegisterObject(mobileParty);
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0001F714 File Offset: 0x0001D914
		public override bool OnMapClick(bool followModifierUsed)
		{
			MobileParty.NavigationType navigationType;
			if (this.IsMainEntity)
			{
				MobileParty.MainParty.SetMoveModeHold();
			}
			else if (base.MapEntity.MobileParty.IsCurrentlyAtSea == MobileParty.MainParty.IsCurrentlyAtSea && NavigationHelper.CanPlayerNavigateToPosition(base.MapEntity.MobileParty.Position, out navigationType))
			{
				if (followModifierUsed)
				{
					MobileParty.MainParty.SetMoveEscortParty(base.MapEntity.MobileParty, navigationType, false);
				}
				else
				{
					MobileParty.MainParty.SetMoveEngageParty(base.MapEntity.MobileParty, navigationType);
				}
			}
			return true;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0001F7A0 File Offset: 0x0001D9A0
		public override void OnHover()
		{
			if (base.MapEntity.MapEvent != null)
			{
				InformationManager.ShowTooltip(typeof(MapEvent), new object[] { base.MapEntity.MapEvent });
				return;
			}
			if (base.MapEntity.IsMobile && base.MapEntity.IsVisible)
			{
				if (base.MapEntity.MobileParty.Army != null && base.MapEntity.MobileParty.Army.DoesLeaderPartyAndAttachedPartiesContain(base.MapEntity.MobileParty))
				{
					if (base.MapEntity.MobileParty.Army.LeaderParty.SiegeEvent != null)
					{
						InformationManager.ShowTooltip(typeof(SiegeEvent), new object[] { base.MapEntity.MobileParty.Army.LeaderParty.SiegeEvent });
						return;
					}
					InformationManager.ShowTooltip(typeof(Army), new object[]
					{
						base.MapEntity.MobileParty.Army,
						false,
						true
					});
					return;
				}
				else
				{
					if (base.MapEntity.MobileParty.SiegeEvent != null)
					{
						InformationManager.ShowTooltip(typeof(SiegeEvent), new object[] { base.MapEntity.MobileParty.SiegeEvent });
						return;
					}
					InformationManager.ShowTooltip(typeof(MobileParty), new object[]
					{
						base.MapEntity.MobileParty,
						false,
						true
					});
				}
			}
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0001F934 File Offset: 0x0001DB34
		public override Vec3 GetVisualPosition()
		{
			return base.MapEntity.MobileParty.VisualPosition2DWithoutError.ToVec3(base.MapEntity.Position.AsVec3().Z);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0001F974 File Offset: 0x0001DB74
		public override void ReleaseResources()
		{
			this.ResetPartyIcon();
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0001F97C File Offset: 0x0001DB7C
		public override bool IsVisibleOrFadingOut()
		{
			return this._entityAlpha > 0f;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0001F98C File Offset: 0x0001DB8C
		public override void OnOpenEncyclopedia()
		{
			if (base.MapEntity.MobileParty.IsLordParty && base.MapEntity.MobileParty.LeaderHero != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(base.MapEntity.MobileParty.LeaderHero.EncyclopediaLink);
			}
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0001F9E4 File Offset: 0x0001DBE4
		internal void Tick(float dt, float realDt, ref int dirtyPartiesCount, ref MobilePartyVisual[] dirtyPartiesList)
		{
			if (this.StrategicEntity == null)
			{
				return;
			}
			if (base.MapEntity.IsVisualDirty && (this._entityAlpha > 0f || base.MapEntity.IsVisible))
			{
				int num = Interlocked.Increment(ref dirtyPartiesCount);
				dirtyPartiesList[num] = this;
			}
			if (this.IsVisibleOrFadingOut() && this.StrategicEntity != null && (!base.MapEntity.MobileParty.IsCurrentlyAtSea || base.MapEntity.MobileParty.IsTransitionInProgress))
			{
				this.UpdateBearingRotation(realDt, dt);
				this._speed = (base.MapEntity.MobileParty.IsActive ? base.MapEntity.MobileParty.Speed : 0f);
				float num2 = ((this.MountAgentVisuals != null) ? 1.3f : 1f);
				float num3 = MathF.Min(0.25f * num2 * this._speed / 0.3f, 20f);
				bool flag = this.IsEntityMovingVisually();
				AgentVisuals humanAgentVisuals = this.HumanAgentVisuals;
				if (humanAgentVisuals != null)
				{
					humanAgentVisuals.Tick(this.MountAgentVisuals, dt, flag, num3);
				}
				AgentVisuals mountAgentVisuals = this.MountAgentVisuals;
				if (mountAgentVisuals != null)
				{
					mountAgentVisuals.Tick(null, dt, flag, num3);
				}
				AgentVisuals caravanMountAgentVisuals = this.CaravanMountAgentVisuals;
				if (caravanMountAgentVisuals != null)
				{
					caravanMountAgentVisuals.Tick(null, dt, flag, num3);
				}
				MobileParty mobileParty = base.MapEntity.MobileParty;
				MatrixFrame identity = MatrixFrame.Identity;
				identity.origin = this.GetVisualPosition();
				if (mobileParty.Army != null && mobileParty.AttachedTo == mobileParty.Army.LeaderParty && (base.MapEntity.MapEvent == null || !base.MapEntity.MapEvent.IsFieldBattle))
				{
					MatrixFrame frame = this.StrategicEntity.GetFrame();
					Vec2 vec = identity.origin.AsVec2 - frame.origin.AsVec2;
					if (vec.Length / dt > 20f)
					{
						identity.rotation.RotateAboutUp(this._bearingRotation);
					}
					else if (mobileParty.CurrentSettlement == null)
					{
						float num4 = MBMath.LerpRadians(frame.rotation.f.AsVec2.RotationInRadians, (vec + Vec2.FromRotation(this._bearingRotation) * 0.01f).RotationInRadians, Math.Min(6f * dt, 1f), 0.03f * dt, 10f * dt);
						identity.rotation.RotateAboutUp(num4);
					}
					else
					{
						float rotationInRadians = frame.rotation.f.AsVec2.RotationInRadians;
						identity.rotation.RotateAboutUp(rotationInRadians);
					}
				}
				else if (mobileParty.CurrentSettlement == null)
				{
					identity.rotation.RotateAboutUp(this.GetVisualRotation());
				}
				MatrixFrame matrixFrame = this.StrategicEntity.GetFrame();
				if (!matrixFrame.NearlyEquals(identity, 1E-05f))
				{
					this.StrategicEntity.SetFrame(ref identity, true);
					if (this.HumanAgentVisuals != null)
					{
						MatrixFrame matrixFrame2 = identity;
						matrixFrame2.rotation.ApplyScaleLocal(this.HumanAgentVisuals.GetScale());
						this.HumanAgentVisuals.GetWeakEntity().SetFrame(ref matrixFrame2, true);
					}
					if (this.MountAgentVisuals != null)
					{
						MatrixFrame matrixFrame3 = identity;
						matrixFrame3.rotation.ApplyScaleLocal(this.MountAgentVisuals.GetScale());
						this.MountAgentVisuals.GetWeakEntity().SetFrame(ref matrixFrame3, true);
					}
					if (this.CaravanMountAgentVisuals != null)
					{
						matrixFrame = this.CaravanMountAgentVisuals.GetFrame();
						MatrixFrame matrixFrame4 = identity.TransformToParent(in matrixFrame);
						matrixFrame4.rotation.ApplyScaleLocal(this.CaravanMountAgentVisuals.GetScale());
						this.CaravanMountAgentVisuals.GetWeakEntity().SetFrame(ref matrixFrame4, true);
					}
				}
				this.ApplyWindEffect();
			}
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0001FDA4 File Offset: 0x0001DFA4
		private void ApplyWindEffect()
		{
			if (this.HumanAgentVisuals != null && !this.HumanAgentVisuals.GetEquipment()[EquipmentIndex.ExtraWeaponSlot].IsEmpty)
			{
				this.HumanAgentVisuals.SetClothWindToWeaponAtIndex(-this.StrategicEntity.GetGlobalFrame().rotation.f, false, EquipmentIndex.ExtraWeaponSlot);
			}
			ClothSimulatorComponent clothSimulatorComponent;
			if (this._cachedBannerComponent.Item2 != null && (clothSimulatorComponent = this._cachedBannerComponent.Item2 as ClothSimulatorComponent) != null)
			{
				clothSimulatorComponent.SetForcedWind(-this.StrategicEntity.GetGlobalFrame().rotation.f, false);
			}
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0001FE44 File Offset: 0x0001E044
		internal void OnStartup()
		{
			bool flag = false;
			if (base.MapEntity.IsMobile)
			{
				this.StrategicEntity = GameEntity.CreateEmpty(this.MapScene, true, true, true);
				if (!base.MapEntity.IsVisible)
				{
					this.StrategicEntity.EntityFlags |= EntityFlags.DoNotTick;
				}
			}
			CharacterObject visualPartyLeader = PartyBaseHelper.GetVisualPartyLeader(base.MapEntity);
			if (!flag)
			{
				this.CircleLocalFrame = MatrixFrame.Identity;
				if ((visualPartyLeader != null && visualPartyLeader.HasMount()) || base.MapEntity.MobileParty.IsCaravan)
				{
					MatrixFrame circleLocalFrame = this.CircleLocalFrame;
					Mat3 rotation = circleLocalFrame.rotation;
					rotation.ApplyScaleLocal(0.4625f);
					circleLocalFrame.rotation = rotation;
					this.CircleLocalFrame = circleLocalFrame;
				}
				else
				{
					MatrixFrame circleLocalFrame2 = this.CircleLocalFrame;
					Mat3 rotation2 = circleLocalFrame2.rotation;
					rotation2.ApplyScaleLocal(0.3725f);
					circleLocalFrame2.rotation = rotation2;
					this.CircleLocalFrame = circleLocalFrame2;
				}
			}
			this._bearingRotation = base.MapEntity.MobileParty.Bearing.RotationInRadians;
			this.StrategicEntity.SetVisibilityExcludeParents(base.MapEntity.IsVisible);
			if (this.HumanAgentVisuals != null)
			{
				WeakGameEntity weakEntity = this.HumanAgentVisuals.GetWeakEntity();
				if (weakEntity != WeakGameEntity.Invalid)
				{
					weakEntity.SetVisibilityExcludeParents(base.MapEntity.IsVisible);
				}
			}
			if (this.MountAgentVisuals != null)
			{
				WeakGameEntity weakEntity2 = this.MountAgentVisuals.GetWeakEntity();
				if (weakEntity2 != WeakGameEntity.Invalid)
				{
					weakEntity2.SetVisibilityExcludeParents(base.MapEntity.IsVisible);
				}
			}
			if (this.CaravanMountAgentVisuals != null)
			{
				WeakGameEntity weakEntity3 = this.CaravanMountAgentVisuals.GetWeakEntity();
				if (weakEntity3 != WeakGameEntity.Invalid)
				{
					weakEntity3.SetVisibilityExcludeParents(base.MapEntity.IsVisible);
				}
			}
			this.StrategicEntity.SetReadyToRender(true);
			this.StrategicEntity.SetEntityEnvMapVisibility(false);
			this._entityAlpha = 0f;
			if (base.MapEntity.IsVisible)
			{
				if (base.MapEntity.MobileParty.IsTransitionInProgress)
				{
					this.TickFadingState(0.1f, 0.1f);
				}
				else
				{
					this._entityAlpha = 1f;
				}
			}
			this.AddVisualToVisualsOfEntities();
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0002005C File Offset: 0x0001E25C
		internal void TickFadingState(float realDt, float dt)
		{
			if ((!base.MapEntity.MobileParty.IsTransitionInProgress || !base.MapEntity.IsVisible) && ((this._entityAlpha < 1f && base.MapEntity.IsVisible) || (this._entityAlpha > 0f && !base.MapEntity.IsVisible)))
			{
				if (base.MapEntity.IsVisible)
				{
					if (this._entityAlpha <= 0f)
					{
						this.StrategicEntity.SetVisibilityExcludeParents(true);
						if (this.HumanAgentVisuals != null)
						{
							WeakGameEntity weakEntity = this.HumanAgentVisuals.GetWeakEntity();
							if (weakEntity != WeakGameEntity.Invalid)
							{
								weakEntity.SetVisibilityExcludeParents(true);
							}
						}
						if (this.MountAgentVisuals != null)
						{
							WeakGameEntity weakEntity2 = this.MountAgentVisuals.GetWeakEntity();
							if (weakEntity2 != WeakGameEntity.Invalid)
							{
								weakEntity2.SetVisibilityExcludeParents(true);
							}
						}
						if (this.CaravanMountAgentVisuals != null)
						{
							WeakGameEntity weakEntity3 = this.CaravanMountAgentVisuals.GetWeakEntity();
							if (weakEntity3 != WeakGameEntity.Invalid)
							{
								weakEntity3.SetVisibilityExcludeParents(true);
							}
						}
					}
					this._entityAlpha = MathF.Min(this._entityAlpha + MathF.Max(realDt, 1E-05f), 1f);
					this.StrategicEntity.SetAlpha(this._entityAlpha);
					if (this.HumanAgentVisuals != null)
					{
						WeakGameEntity weakEntity4 = this.HumanAgentVisuals.GetWeakEntity();
						if (weakEntity4 != WeakGameEntity.Invalid)
						{
							weakEntity4.SetAlpha(this._entityAlpha);
						}
					}
					if (this.MountAgentVisuals != null)
					{
						WeakGameEntity weakEntity5 = this.MountAgentVisuals.GetWeakEntity();
						if (weakEntity5 != WeakGameEntity.Invalid)
						{
							weakEntity5.SetAlpha(this._entityAlpha);
						}
					}
					if (this.CaravanMountAgentVisuals != null)
					{
						WeakGameEntity weakEntity6 = this.CaravanMountAgentVisuals.GetWeakEntity();
						if (weakEntity6 != WeakGameEntity.Invalid)
						{
							weakEntity6.SetAlpha(this._entityAlpha);
						}
					}
					this.StrategicEntity.EntityFlags &= ~EntityFlags.DoNotTick;
					return;
				}
				this._entityAlpha = MathF.Max(this._entityAlpha - MathF.Max(realDt, 1E-05f), 0f);
				this.StrategicEntity.SetAlpha(this._entityAlpha);
				if (this.HumanAgentVisuals != null)
				{
					WeakGameEntity weakEntity7 = this.HumanAgentVisuals.GetWeakEntity();
					if (weakEntity7 != WeakGameEntity.Invalid)
					{
						weakEntity7.SetAlpha(this._entityAlpha);
					}
				}
				if (this.MountAgentVisuals != null)
				{
					WeakGameEntity weakEntity8 = this.MountAgentVisuals.GetWeakEntity();
					if (weakEntity8 != WeakGameEntity.Invalid)
					{
						weakEntity8.SetAlpha(this._entityAlpha);
					}
				}
				if (this.CaravanMountAgentVisuals != null)
				{
					WeakGameEntity weakEntity9 = this.CaravanMountAgentVisuals.GetWeakEntity();
					if (weakEntity9 != WeakGameEntity.Invalid)
					{
						weakEntity9.SetAlpha(this._entityAlpha);
					}
				}
				if (this._entityAlpha <= 0f)
				{
					this.StrategicEntity.SetVisibilityExcludeParents(false);
					if (this.HumanAgentVisuals != null)
					{
						WeakGameEntity weakEntity10 = this.HumanAgentVisuals.GetWeakEntity();
						if (weakEntity10 != WeakGameEntity.Invalid)
						{
							weakEntity10.SetVisibilityExcludeParents(false);
						}
					}
					if (this.MountAgentVisuals != null)
					{
						WeakGameEntity weakEntity11 = this.MountAgentVisuals.GetWeakEntity();
						if (weakEntity11 != WeakGameEntity.Invalid)
						{
							weakEntity11.SetVisibilityExcludeParents(false);
						}
					}
					if (this.CaravanMountAgentVisuals != null)
					{
						WeakGameEntity weakEntity12 = this.CaravanMountAgentVisuals.GetWeakEntity();
						if (weakEntity12 != WeakGameEntity.Invalid)
						{
							weakEntity12.SetVisibilityExcludeParents(false);
						}
					}
					this.StrategicEntity.EntityFlags |= EntityFlags.DoNotTick;
					return;
				}
			}
			else if (base.MapEntity.MobileParty.IsTransitionInProgress)
			{
				if ((base.MapEntity.MobileParty.Army == null || base.MapEntity.MobileParty.Army.LeaderParty == base.MapEntity.MobileParty || base.MapEntity.MobileParty.AttachedTo == null) && this.IsMobileEntity && this.GetTransitionProgress() < 1f)
				{
					this.TickTransitionFadeState(dt);
					return;
				}
			}
			else
			{
				MobilePartyVisualManager.Current.UnRegisterFadingVisual(this);
			}
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00020444 File Offset: 0x0001E644
		private void UpdateBearingRotation(float realDt, float dt)
		{
			float num = MBMath.WrapAngle(base.MapEntity.MobileParty.Bearing.RotationInRadians - this._bearingRotation);
			float num2 = ((base.MapEntity.MapEvent != null) ? realDt : dt);
			this._bearingRotation += num * MathF.Min(num2 * 30f, 1f);
			this._bearingRotation = MBMath.WrapAngle(this._bearingRotation);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x000204BC File Offset: 0x0001E6BC
		private void TickTransitionFadeState(float dt)
		{
			float transitionProgress = this.GetTransitionProgress();
			if (base.MapEntity.MobileParty.IsCurrentlyAtSea)
			{
				this._entityAlpha = transitionProgress;
				AgentVisuals humanAgentVisuals = this.HumanAgentVisuals;
				if (humanAgentVisuals != null)
				{
					GameEntity entity = humanAgentVisuals.GetEntity();
					if (entity != null)
					{
						entity.SetAlpha(this._entityAlpha);
					}
				}
				AgentVisuals mountAgentVisuals = this.MountAgentVisuals;
				if (mountAgentVisuals != null)
				{
					GameEntity entity2 = mountAgentVisuals.GetEntity();
					if (entity2 != null)
					{
						entity2.SetAlpha(this._entityAlpha);
					}
				}
				AgentVisuals caravanMountAgentVisuals = this.CaravanMountAgentVisuals;
				if (caravanMountAgentVisuals != null)
				{
					GameEntity entity3 = caravanMountAgentVisuals.GetEntity();
					if (entity3 != null)
					{
						entity3.SetAlpha(this._entityAlpha);
					}
				}
				if (this.HumanAgentVisuals != null)
				{
					MatrixFrame frame = this.HumanAgentVisuals.GetEntity().GetFrame();
					CampaignVec2 campaignVec = base.MapEntity.MobileParty.EndPositionForNavigationTransition + base.MapEntity.MobileParty.ArmyPositionAdder;
					float num = MathF.Lerp(frame.origin.X, campaignVec.X, dt, 1E-05f);
					float num2 = MathF.Lerp(frame.origin.Y, campaignVec.Y, dt, 1E-05f);
					float num3 = MathF.Lerp(frame.origin.z, campaignVec.AsVec3().Z, dt, 1E-05f);
					frame.origin = new Vec3(num, num2, num3, -1f);
					GameEntity entity4 = this.HumanAgentVisuals.GetEntity();
					if (entity4 != null)
					{
						entity4.SetFrame(ref frame, false);
					}
					AgentVisuals mountAgentVisuals2 = this.MountAgentVisuals;
					if (mountAgentVisuals2 != null)
					{
						GameEntity entity5 = mountAgentVisuals2.GetEntity();
						if (entity5 != null)
						{
							entity5.SetFrame(ref frame, false);
						}
					}
					AgentVisuals caravanMountAgentVisuals2 = this.CaravanMountAgentVisuals;
					if (caravanMountAgentVisuals2 == null)
					{
						return;
					}
					GameEntity entity6 = caravanMountAgentVisuals2.GetEntity();
					if (entity6 == null)
					{
						return;
					}
					entity6.SetFrame(ref frame, false);
					return;
				}
			}
			else
			{
				this._entityAlpha = 1f - transitionProgress;
				AgentVisuals humanAgentVisuals2 = this.HumanAgentVisuals;
				if (humanAgentVisuals2 != null)
				{
					GameEntity entity7 = humanAgentVisuals2.GetEntity();
					if (entity7 != null)
					{
						entity7.SetAlpha(this._entityAlpha);
					}
				}
				AgentVisuals mountAgentVisuals3 = this.MountAgentVisuals;
				if (mountAgentVisuals3 != null)
				{
					GameEntity entity8 = mountAgentVisuals3.GetEntity();
					if (entity8 != null)
					{
						entity8.SetAlpha(this._entityAlpha);
					}
				}
				AgentVisuals caravanMountAgentVisuals3 = this.CaravanMountAgentVisuals;
				if (caravanMountAgentVisuals3 == null)
				{
					return;
				}
				GameEntity entity9 = caravanMountAgentVisuals3.GetEntity();
				if (entity9 == null)
				{
					return;
				}
				entity9.SetAlpha(this._entityAlpha);
			}
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x000206D8 File Offset: 0x0001E8D8
		internal void ValidateIsDirty()
		{
			if (base.MapEntity.MemberRoster.TotalManCount != 0)
			{
				this.RefreshPartyIcon();
				if ((this._entityAlpha < 1f && base.MapEntity.IsVisible) || (this._entityAlpha > 0f && !base.MapEntity.IsVisible))
				{
					MobilePartyVisualManager.Current.RegisterFadingVisual(this);
					return;
				}
			}
			else
			{
				this.ResetPartyIcon();
			}
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00020744 File Offset: 0x0001E944
		private void RefreshPartyIcon()
		{
			if (base.MapEntity.IsVisualDirty)
			{
				base.MapEntity.OnVisualsUpdated();
				bool flag = true;
				bool flag2 = true;
				this.ResetPartyIcon();
				MatrixFrame circleLocalFrame = this.CircleLocalFrame;
				circleLocalFrame.origin = Vec3.Zero;
				this.CircleLocalFrame = circleLocalFrame;
				MobileParty mobileParty = base.MapEntity.MobileParty;
				if (((mobileParty != null) ? mobileParty.CurrentSettlement : null) != null)
				{
					this.AddVisualToVisualsOfEntities();
					if (!base.MapEntity.MobileParty.MapFaction.IsAtWarWith(base.MapEntity.MobileParty.CurrentSettlement.MapFaction))
					{
						Hero leaderHero = base.MapEntity.LeaderHero;
						if (((leaderHero != null) ? leaderHero.ClanBanner : null) != null)
						{
							string bannerCode = base.MapEntity.LeaderHero.ClanBanner.BannerCode;
							if (string.IsNullOrEmpty(bannerCode))
							{
								goto IL_03FB;
							}
							MatrixFrame matrixFrame = MatrixFrame.Identity;
							Vec3 bannerPositionForParty = SettlementVisualManager.Current.GetSettlementVisual(base.MapEntity.MobileParty.CurrentSettlement).GetBannerPositionForParty(base.MapEntity.MobileParty);
							if (!bannerPositionForParty.IsValid)
							{
								goto IL_03FB;
							}
							matrixFrame.origin = bannerPositionForParty;
							MatrixFrame matrixFrame2 = this.StrategicEntity.GetGlobalFrame();
							matrixFrame.origin = matrixFrame2.TransformToLocal(in matrixFrame.origin);
							float num = MBMath.Map((float)base.MapEntity.NumberOfAllMembers / 400f * ((base.MapEntity.MobileParty.Army != null && base.MapEntity.MobileParty.Army.LeaderParty == base.MapEntity.MobileParty) ? 1.25f : 1f), 0f, 1f, 0.2f, 0.5f);
							matrixFrame = matrixFrame.Elevate(-num);
							matrixFrame.rotation.ApplyScaleLocal(num);
							matrixFrame2 = this.StrategicEntity.GetGlobalFrame();
							matrixFrame.rotation = matrixFrame2.rotation.TransformToLocal(in matrixFrame.rotation);
							this.StrategicEntity.AddSphereAsBody(matrixFrame.origin + Vec3.Up * 0.3f, 0.15f, BodyFlags.None);
							flag = false;
							string text = "campaign_flag";
							if (this._cachedBannerComponent.Item1 == bannerCode + text)
							{
								this._cachedBannerComponent.Item2.GetFirstMetaMesh().Frame = matrixFrame;
								this.StrategicEntity.AddComponent(this._cachedBannerComponent.Item2);
								goto IL_03FB;
							}
							MetaMesh banner = SandBoxViewHelpers.BannerVisualHelper.GetBanner(new Banner(bannerCode), text);
							banner.Frame = matrixFrame;
							int componentCount = this.StrategicEntity.GetComponentCount(GameEntity.ComponentType.ClothSimulator);
							this.StrategicEntity.AddMultiMesh(banner, true);
							if (this.StrategicEntity.GetComponentCount(GameEntity.ComponentType.ClothSimulator) > componentCount)
							{
								this._cachedBannerComponent.Item1 = bannerCode + text;
								this._cachedBannerComponent.Item2 = this.StrategicEntity.GetComponentAtIndex(componentCount, GameEntity.ComponentType.ClothSimulator);
								goto IL_03FB;
							}
							goto IL_03FB;
						}
					}
					this.StrategicEntity.RemovePhysics(false);
				}
				else if (base.MapEntity.MobileParty != null && (base.MapEntity.MobileParty.IsCurrentlyAtSea || base.MapEntity.MobileParty.IsTransitionInProgress))
				{
					this.RemoveVisualFromVisualsOfEntities();
					if (base.MapEntity.MobileParty.IsTransitionInProgress)
					{
						if (base.MapEntity.MobileParty.Army == null || base.MapEntity.MobileParty.Army.LeaderParty == base.MapEntity.MobileParty || base.MapEntity.MobileParty.AttachedTo == null)
						{
							this.AddMobileIconComponents(base.MapEntity, ref flag, ref flag2);
						}
						if (!this._isInTransitionProgressCached)
						{
							this.AddVisualToVisualsOfEntities();
							this.OnTransitionStarted();
						}
					}
					if (base.MapEntity.MobileParty.IsTransitionInProgress != this._isInTransitionProgressCached)
					{
						if (this._isInTransitionProgressCached)
						{
							this.OnTransitionEnded();
						}
						else
						{
							this.OnTransitionStarted();
						}
					}
				}
				else
				{
					this.AddVisualToVisualsOfEntities();
					this.InitializePartyCollider(base.MapEntity);
					this.AddMobileIconComponents(base.MapEntity, ref flag, ref flag2);
				}
				IL_03FB:
				if (flag)
				{
					this._cachedBannerComponent = new ValueTuple<string, GameEntityComponent>(null, null);
				}
				if (flag2)
				{
					this._cachedBannerEntity = new ValueTuple<string, GameEntity>(null, null);
				}
				this.StrategicEntity.CheckResources(true, false);
				if (this.IsMobileEntity)
				{
					this._isInTransitionProgressCached = base.MapEntity.MobileParty.IsTransitionInProgress;
				}
			}
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00020B98 File Offset: 0x0001ED98
		private void AddMobileIconComponents(PartyBase party, ref bool clearBannerComponentCache, ref bool clearBannerEntityCache)
		{
			uint num = (FactionManager.IsAtWarAgainstFaction(party.MapFaction, Hero.MainHero.MapFaction) ? 4294905856U : 4278206719U);
			if (this.IsPartOfBesiegerCamp(party))
			{
				this.AddTentEntityForParty(this.StrategicEntity, party, ref clearBannerComponentCache);
				return;
			}
			if (PartyBaseHelper.GetVisualPartyLeader(party) != null)
			{
				ActionIndexCache act_none = ActionIndexCache.act_none;
				ActionIndexCache act_none2 = ActionIndexCache.act_none;
				MapEvent mapEvent = ((party.MobileParty.Army != null && party.MobileParty.Army.DoesLeaderPartyAndAttachedPartiesContain(party.MobileParty)) ? party.MobileParty.Army.LeaderParty.MapEvent : party.MapEvent);
				int num2;
				SandBoxViewHelpers.MobilePartyVisualHelper.GetMeleeWeaponToWield(party, out num2);
				if (mapEvent != null && (mapEvent.EventType == MapEvent.BattleTypes.FieldBattle || mapEvent.EventType == MapEvent.BattleTypes.Raid || mapEvent.EventType == MapEvent.BattleTypes.SiegeOutside || mapEvent.EventType == MapEvent.BattleTypes.SallyOut))
				{
					MobilePartyVisual.GetPartyBattleAnimation(party, num2, out act_none, out act_none2);
				}
				this.AddCharacterToPartyIcon(party, PartyBaseHelper.GetVisualPartyLeader(party), num, in act_none, in act_none2, MBRandom.NondeterministicRandomFloat * 0.7f, ref clearBannerEntityCache);
				if (party.IsMobile)
				{
					string text;
					string text2;
					this.GetMountAndHarnessVisualIdsForPartyIcon(out text, out text2);
					if (!string.IsNullOrEmpty(text))
					{
						this.AddMountToPartyIcon(new Vec3(0.3f, -0.25f, 0f, -1f), text, text2, num, PartyBaseHelper.GetVisualPartyLeader(party), party);
					}
				}
			}
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00020CDC File Offset: 0x0001EEDC
		private void AddMountToPartyIcon(Vec3 positionOffset, string mountItemId, string harnessItemId, uint contourColor, CharacterObject character, PartyBase party)
		{
			ItemObject @object = Game.Current.ObjectManager.GetObject<ItemObject>(mountItemId);
			Monster monster = @object.HorseComponent.Monster;
			ItemObject itemObject = null;
			if (!string.IsNullOrEmpty(harnessItemId))
			{
				itemObject = Game.Current.ObjectManager.GetObject<ItemObject>(harnessItemId);
			}
			Equipment equipment = new Equipment();
			equipment[EquipmentIndex.ArmorItemEndSlot] = new EquipmentElement(@object, null, null, false);
			equipment[EquipmentIndex.HorseHarness] = new EquipmentElement(itemObject, null, null, false);
			AgentVisualsData agentVisualsData = new AgentVisualsData().Equipment(equipment).Scale(@object.ScaleFactor * 0.3f);
			Mat3 identity = Mat3.Identity;
			AgentVisualsData agentVisualsData2 = agentVisualsData.Frame(new MatrixFrame(in identity, in positionOffset)).ActionSet(MBGlobals.GetActionSet(monster.ActionSetCode + "_map")).Scene(this.MapScene)
				.Monster(monster)
				.PrepareImmediately(false)
				.UseScaledWeapons(true)
				.HasClippingPlane(true);
			IFaction mapFaction = party.MapFaction;
			AgentVisualsData agentVisualsData3 = agentVisualsData2.ClothColor1((mapFaction != null) ? mapFaction.Color : 4291609515U);
			IFaction mapFaction2 = party.MapFaction;
			AgentVisualsData agentVisualsData4 = agentVisualsData3.ClothColor2((mapFaction2 != null) ? mapFaction2.Color2 : 4291609515U).MountCreationKey(MountCreationKey.GetRandomMountKeyString(@object, character.GetMountKeySeed()));
			this.CaravanMountAgentVisuals = AgentVisuals.Create(agentVisualsData4, "PartyIcon " + mountItemId, false, false, false);
			this.CaravanMountAgentVisuals.GetEntity().SetContourColor(new uint?(contourColor), false);
			MatrixFrame matrixFrame = this.CaravanMountAgentVisuals.GetFrame();
			matrixFrame.rotation.ApplyScaleLocal(this.CaravanMountAgentVisuals.GetScale());
			matrixFrame = this.StrategicEntity.GetFrame().TransformToParent(in matrixFrame);
			this.CaravanMountAgentVisuals.GetEntity().SetFrame(ref matrixFrame, true);
			float num = MathF.Min(0.325f * this._speed / 0.3f, 20f);
			this.CaravanMountAgentVisuals.Tick(null, 0.0001f, this.IsEntityMovingVisually(), num);
			this.CaravanMountAgentVisuals.GetEntity().Skeleton.ForceUpdateBoneFrames();
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00020ED4 File Offset: 0x0001F0D4
		private void AddCharacterToPartyIcon(PartyBase party, CharacterObject characterObject, uint contourColor, in ActionIndexCache leaderAction, in ActionIndexCache mountAction, float animationStartDuration, ref bool clearBannerEntityCache)
		{
			if (!party.MobileParty.IsCurrentlyAtSea || party.MobileParty.IsTransitionInProgress)
			{
				float num;
				this.HumanAgentVisuals = SandBoxViewHelpers.MobilePartyVisualHelper.GetHumanAgentPartyVisual(this.MapScene, this.StrategicEntity.GetFrame(), party, contourColor, leaderAction, ref clearBannerEntityCache, ref this._cachedBannerEntity, out num);
				if (this.HumanAgentVisuals != null && leaderAction != ActionIndexCache.act_none)
				{
					if (num < 1f)
					{
						this.HumanAgentVisuals.GetVisuals().GetSkeleton().SetAgentActionChannel(0, in leaderAction, animationStartDuration, -0.2f, true, 0f);
					}
					else
					{
						this.HumanAgentVisuals.GetVisuals().GetSkeleton().SetAgentActionChannel(0, in leaderAction, animationStartDuration / num, -0.2f, true, 0f);
					}
				}
			}
			if (characterObject.HasMount() && (!party.MobileParty.IsCurrentlyAtSea || party.MobileParty.IsTransitionInProgress))
			{
				Monster monster = characterObject.Equipment[EquipmentIndex.ArmorItemEndSlot].Item.HorseComponent.Monster;
				MBActionSet actionSet = MBGlobals.GetActionSet(monster.ActionSetCode + "_map");
				AgentVisualsData agentVisualsData = new AgentVisualsData().Equipment(characterObject.Equipment).Scale(characterObject.Equipment[EquipmentIndex.ArmorItemEndSlot].Item.ScaleFactor * 0.3f).Frame(MatrixFrame.Identity)
					.ActionSet(actionSet)
					.Scene(this.MapScene)
					.Monster(monster)
					.PrepareImmediately(false)
					.UseScaledWeapons(true)
					.HasClippingPlane(true);
				IFaction mapFaction = party.MapFaction;
				AgentVisualsData agentVisualsData2 = agentVisualsData.ClothColor1((mapFaction != null) ? mapFaction.Color : 4291609515U);
				IFaction mapFaction2 = party.MapFaction;
				AgentVisualsData agentVisualsData3 = agentVisualsData2.ClothColor2((mapFaction2 != null) ? mapFaction2.Color2 : 4291609515U).MountCreationKey(MountCreationKey.GetRandomMountKeyString(characterObject.Equipment[EquipmentIndex.ArmorItemEndSlot].Item, characterObject.GetMountKeySeed()));
				this.MountAgentVisuals = AgentVisuals.Create(agentVisualsData3, "PartyIcon " + characterObject.Name + " mount", false, false, false);
				if (mountAction != ActionIndexCache.act_none)
				{
					float actionAnimationDuration = MBActionSet.GetActionAnimationDuration(actionSet, in mountAction);
					if (actionAnimationDuration < 1f)
					{
						this.MountAgentVisuals.GetWeakEntity().Skeleton.SetAgentActionChannel(0, in mountAction, animationStartDuration, -0.2f, true, 0f);
					}
					else
					{
						this.MountAgentVisuals.GetWeakEntity().Skeleton.SetAgentActionChannel(0, in mountAction, animationStartDuration / actionAnimationDuration, -0.2f, true, 0f);
					}
				}
				this.MountAgentVisuals.GetWeakEntity().SetContourColor(new uint?(contourColor), false);
				MatrixFrame frame = this.StrategicEntity.GetFrame();
				frame.rotation.ApplyScaleLocal(agentVisualsData3.ScaleData);
				this.MountAgentVisuals.GetWeakEntity().SetFrame(ref frame, true);
			}
			float num2 = ((this.MountAgentVisuals != null) ? 1.3f : 1f);
			float num3 = MathF.Min(0.25f * num2 * this._speed / 0.3f, 20f);
			if (this.MountAgentVisuals != null)
			{
				this.MountAgentVisuals.Tick(null, 0.0001f, this.IsEntityMovingVisually(), num3);
				this.MountAgentVisuals.GetWeakEntity().Skeleton.ForceUpdateBoneFrames();
			}
			if (this.HumanAgentVisuals != null)
			{
				WeakGameEntity weakEntity = this.HumanAgentVisuals.GetWeakEntity();
				weakEntity.SetContourColor(new uint?(contourColor), false);
				MatrixFrame frame2 = this.StrategicEntity.GetFrame();
				frame2.rotation.ApplyScaleLocal(0.3f);
				weakEntity.SetFrame(ref frame2, true);
				this.HumanAgentVisuals.Tick(this.MountAgentVisuals, 0.0001f, this.IsEntityMovingVisually(), num3);
				weakEntity.Skeleton.ForceUpdateBoneFrames();
			}
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0002129C File Offset: 0x0001F49C
		private bool IsEntityMovingVisually()
		{
			if (base.MapEntity.IsMobile && base.MapEntity.MapEvent != null)
			{
				this._isEntityMovingCache = false;
			}
			else
			{
				if (Campaign.Current.CampaignDt <= 0f)
				{
					MobileParty mobileParty = base.MapEntity.MobileParty;
					if (mobileParty == null || !mobileParty.IsMainParty || !Campaign.Current.IsMainPartyWaiting)
					{
						goto IL_00AF;
					}
				}
				this._isEntityMovingCache = false;
				MobileParty mobileParty2 = base.MapEntity.MobileParty;
				if (mobileParty2 != null && !mobileParty2.VisualPosition2DWithoutError.NearlyEquals(this._lastFrameVisualPositionWithoutError, 1E-05f))
				{
					this._lastFrameVisualPositionWithoutError = base.MapEntity.MobileParty.VisualPosition2DWithoutError;
					this._isEntityMovingCache = true;
				}
			}
			IL_00AF:
			if (this._isInTransitionProgressCached)
			{
				this._isEntityMovingCache = true;
			}
			return this._isEntityMovingCache;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00021370 File Offset: 0x0001F570
		public void AddTentEntityForParty(GameEntity strategicEntity, PartyBase party, ref bool clearBannerComponentCache)
		{
			GameEntity gameEntity = GameEntity.CreateEmpty(strategicEntity.Scene, true, true, true);
			gameEntity.AddMultiMesh(MetaMesh.GetCopy("map_icon_siege_camp_tent", true, false), true);
			MatrixFrame identity = MatrixFrame.Identity;
			identity.rotation.ApplyScaleLocal(1.2f);
			gameEntity.SetFrame(ref identity, true);
			string text = null;
			Hero leaderHero = party.LeaderHero;
			if (((leaderHero != null) ? leaderHero.ClanBanner : null) != null)
			{
				text = party.LeaderHero.ClanBanner.BannerCode;
			}
			bool flag = party.MobileParty.Army != null && party.MobileParty.Army.LeaderParty == party.MobileParty;
			MatrixFrame identity2 = MatrixFrame.Identity;
			identity2.origin.z = identity2.origin.z + (flag ? 0.2f : 0.15f);
			float num = MBMath.Map(party.CalculateCurrentStrength() / 500f * ((party.MobileParty.Army != null && flag) ? 1f : 0.8f), 0f, 1f, 0.15f, 0.5f);
			identity2.rotation.ApplyScaleLocal(num);
			if (!string.IsNullOrEmpty(text))
			{
				clearBannerComponentCache = false;
				string text2 = "campaign_flag";
				if (this._cachedBannerComponent.Item1 == text + text2)
				{
					this._cachedBannerComponent.Item2.GetFirstMetaMesh().Frame = identity2;
					gameEntity.AddComponent(this._cachedBannerComponent.Item2);
				}
				else
				{
					MetaMesh banner = SandBoxViewHelpers.BannerVisualHelper.GetBanner(new Banner(text), text2);
					banner.Frame = identity2;
					int componentCount = gameEntity.GetComponentCount(GameEntity.ComponentType.ClothSimulator);
					gameEntity.AddMultiMesh(banner, true);
					if (gameEntity.GetComponentCount(GameEntity.ComponentType.ClothSimulator) > componentCount)
					{
						this._cachedBannerComponent.Item1 = text + text2;
						this._cachedBannerComponent.Item2 = gameEntity.GetComponentAtIndex(componentCount, GameEntity.ComponentType.ClothSimulator);
					}
				}
			}
			strategicEntity.AddChild(gameEntity, false);
			gameEntity.SetVisibilityExcludeParents(true);
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0002154D File Offset: 0x0001F74D
		internal void ClearVisualMemory()
		{
			this.ResetPartyIcon();
			base.MapEntity.SetVisualAsDirty();
			this._cachedBannerEntity = new ValueTuple<string, GameEntity>(null, null);
			this._cachedBannerComponent = new ValueTuple<string, GameEntityComponent>(null, null);
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0002157C File Offset: 0x0001F77C
		private static void GetPartyBattleAnimation(PartyBase party, int wieldedItemIndex, out ActionIndexCache leaderAction, out ActionIndexCache mountAction)
		{
			leaderAction = ActionIndexCache.act_none;
			mountAction = ActionIndexCache.act_none;
			if (party.MobileParty.Army == null || !party.MobileParty.Army.DoesLeaderPartyAndAttachedPartiesContain(party.MobileParty))
			{
				MapEvent mapEvent = party.MapEvent;
			}
			else
			{
				MapEvent mapEvent2 = party.MobileParty.Army.LeaderParty.MapEvent;
			}
			CharacterObject visualPartyLeader = PartyBaseHelper.GetVisualPartyLeader(party);
			MapEvent mapEvent3 = party.MapEvent;
			if (((mapEvent3 != null) ? mapEvent3.MapEventSettlement : null) != null && visualPartyLeader != null && !visualPartyLeader.HasMount())
			{
				leaderAction = ActionIndexCache.act_map_raid;
				return;
			}
			if (wieldedItemIndex > -1 && ((visualPartyLeader != null) ? visualPartyLeader.Equipment[wieldedItemIndex].Item : null) != null)
			{
				WeaponComponent weaponComponent = visualPartyLeader.Equipment[wieldedItemIndex].Item.WeaponComponent;
				if (weaponComponent != null && weaponComponent.PrimaryWeapon.IsMeleeWeapon)
				{
					if (visualPartyLeader.HasMount())
					{
						if (visualPartyLeader.Equipment[10].Item.HorseComponent.Monster.MonsterUsage == "camel")
						{
							if (weaponComponent.GetItemType() == ItemObject.ItemTypeEnum.OneHandedWeapon || weaponComponent.GetItemType() == ItemObject.ItemTypeEnum.TwoHandedWeapon)
							{
								leaderAction = ActionIndexCache.act_map_rider_camel_attack_1h;
								mountAction = ActionIndexCache.act_map_mount_attack_1h;
							}
							else if (weaponComponent.GetItemType() == ItemObject.ItemTypeEnum.Polearm)
							{
								if (weaponComponent.PrimaryWeapon.SwingDamageType == DamageTypes.Invalid)
								{
									leaderAction = ActionIndexCache.act_map_rider_camel_attack_1h_spear;
									mountAction = ActionIndexCache.act_map_mount_attack_spear;
								}
								else if (weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.TwoHandedPolearm)
								{
									leaderAction = ActionIndexCache.act_map_rider_camel_attack_1h_swing;
									mountAction = ActionIndexCache.act_map_mount_attack_swing;
								}
								else
								{
									leaderAction = ActionIndexCache.act_map_rider_camel_attack_2h_swing;
									mountAction = ActionIndexCache.act_map_mount_attack_swing;
								}
							}
						}
						else if (weaponComponent.GetItemType() == ItemObject.ItemTypeEnum.OneHandedWeapon || weaponComponent.GetItemType() == ItemObject.ItemTypeEnum.TwoHandedWeapon)
						{
							leaderAction = ActionIndexCache.act_map_rider_horse_attack_1h;
							mountAction = ActionIndexCache.act_map_mount_attack_1h;
						}
						else if (weaponComponent.GetItemType() == ItemObject.ItemTypeEnum.Polearm)
						{
							if (weaponComponent.PrimaryWeapon.SwingDamageType == DamageTypes.Invalid)
							{
								leaderAction = ActionIndexCache.act_map_rider_horse_attack_1h_spear;
								mountAction = ActionIndexCache.act_map_mount_attack_spear;
							}
							else if (weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.TwoHandedPolearm)
							{
								leaderAction = ActionIndexCache.act_map_rider_horse_attack_1h_swing;
								mountAction = ActionIndexCache.act_map_mount_attack_swing;
							}
							else
							{
								leaderAction = ActionIndexCache.act_map_rider_horse_attack_2h_swing;
								mountAction = ActionIndexCache.act_map_mount_attack_swing;
							}
						}
					}
					else if (weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.OneHandedAxe || weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.Mace || weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.OneHandedSword)
					{
						leaderAction = ActionIndexCache.act_map_attack_1h;
					}
					else if (weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.TwoHandedAxe || weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.TwoHandedMace || weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.TwoHandedSword)
					{
						leaderAction = ActionIndexCache.act_map_attack_2h;
					}
					else if (weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.OneHandedPolearm || weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.TwoHandedPolearm)
					{
						leaderAction = ActionIndexCache.act_map_attack_spear_1h_or_2h;
					}
				}
			}
			if (leaderAction == ActionIndexCache.act_none)
			{
				if (visualPartyLeader.HasMount())
				{
					HorseComponent horseComponent = visualPartyLeader.Equipment[10].Item.HorseComponent;
					leaderAction = ((horseComponent.Monster.MonsterUsage == "camel") ? ActionIndexCache.act_map_rider_camel_attack_unarmed : ActionIndexCache.act_map_rider_horse_attack_unarmed);
					mountAction = ActionIndexCache.act_map_mount_attack_unarmed;
					return;
				}
				leaderAction = ActionIndexCache.act_map_attack_unarmed;
			}
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00021903 File Offset: 0x0001FB03
		private void GetMountAndHarnessVisualIdsForPartyIcon(out string mountStringId, out string harnessStringId)
		{
			mountStringId = "";
			harnessStringId = "";
			if (base.MapEntity.IsMobile)
			{
				PartyComponent partyComponent = base.MapEntity.MobileParty.PartyComponent;
				if (partyComponent == null)
				{
					return;
				}
				partyComponent.GetMountAndHarnessVisualIdsForPartyIcon(base.MapEntity, out mountStringId, out harnessStringId);
			}
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00021944 File Offset: 0x0001FB44
		private void InitializePartyCollider(PartyBase party)
		{
			if (this.StrategicEntity != null && party.IsMobile)
			{
				this.StrategicEntity.AddSphereAsBody(new Vec3(0f, 0f, 0f, -1f), 0.5f, BodyFlags.Moveable | BodyFlags.OnlyCollideWithRaycast);
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00021998 File Offset: 0x0001FB98
		private void ResetPartyIcon()
		{
			if (this.HumanAgentVisuals != null)
			{
				this.HumanAgentVisuals.Reset();
				this.HumanAgentVisuals = null;
			}
			if (this.MountAgentVisuals != null)
			{
				this.MountAgentVisuals.Reset();
				this.MountAgentVisuals = null;
			}
			if (this.CaravanMountAgentVisuals != null)
			{
				this.CaravanMountAgentVisuals.Reset();
				this.CaravanMountAgentVisuals = null;
			}
			if (this.StrategicEntity != null)
			{
				if ((this.StrategicEntity.EntityFlags & EntityFlags.Ignore) != (EntityFlags)0U)
				{
					this.StrategicEntity.RemoveFromPredisplayEntity();
				}
				this.StrategicEntity.ClearComponents();
			}
			this._bearingRotation = base.MapEntity.MobileParty.Bearing.RotationInRadians;
			MobilePartyVisualManager.Current.UnRegisterFadingVisual(this);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00021A54 File Offset: 0x0001FC54
		private float GetTransitionProgress()
		{
			if (this.IsMobileEntity && base.MapEntity.MobileParty.IsTransitionInProgress && base.MapEntity.MobileParty.NavigationTransitionDuration != CampaignTime.Zero)
			{
				float num = (float)base.MapEntity.MobileParty.NavigationTransitionDuration.ToHours;
				Army army = base.MapEntity.MobileParty.Army;
				if (((army != null) ? army.LeaderParty : null) == base.MapEntity.MobileParty && base.MapEntity.MobileParty.AttachedParties.Count > 0)
				{
					float num2 = base.MapEntity.MobileParty.AttachedParties.MaxQ<MobileParty>((MobileParty x) => (float)x.NavigationTransitionDuration.ToHours);
					num = Math.Max(num, num2);
				}
				return MBMath.ClampFloat(base.MapEntity.MobileParty.NavigationTransitionStartTime.ElapsedHoursUntilNow / num, 0f, 1f);
			}
			return 1f;
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00021B68 File Offset: 0x0001FD68
		private void OnTransitionStarted()
		{
			MobilePartyVisualManager.Current.RegisterFadingVisual(this);
			this._transitionStartRotation = (base.MapEntity.MobileParty.EndPositionForNavigationTransition.ToVec2() - base.MapEntity.Position.ToVec2()).RotationInRadians;
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00021BBE File Offset: 0x0001FDBE
		private void OnTransitionEnded()
		{
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00021BC0 File Offset: 0x0001FDC0
		private float GetVisualRotation()
		{
			if (base.MapEntity.IsMobile && base.MapEntity.MapEvent != null && base.MapEntity.MapEvent.IsFieldBattle)
			{
				return this.GetMapEventVisualRotation();
			}
			if (base.MapEntity.IsMobile && base.MapEntity.MobileParty.IsTransitionInProgress)
			{
				return this._transitionStartRotation;
			}
			return this._bearingRotation;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00021C2C File Offset: 0x0001FE2C
		private float GetMapEventVisualRotation()
		{
			if (base.MapEntity.MapEventSide.OtherSide.LeaderParty != null && base.MapEntity.MapEventSide.OtherSide.LeaderParty.IsMobile && base.MapEntity.MapEventSide.OtherSide.LeaderParty.IsMobile)
			{
				return (base.MapEntity.MapEventSide.OtherSide.LeaderParty.MobileParty.VisualPosition2DWithoutError - base.MapEntity.MobileParty.VisualPosition2DWithoutError).Normalized().RotationInRadians;
			}
			return this._bearingRotation;
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00021CD3 File Offset: 0x0001FED3
		private void AddVisualToVisualsOfEntities()
		{
			if (!MapScreen.VisualsOfEntities.ContainsKey(this.StrategicEntity.Pointer))
			{
				MapScreen.VisualsOfEntities.Add(this.StrategicEntity.Pointer, this);
			}
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00021D04 File Offset: 0x0001FF04
		private void RemoveVisualFromVisualsOfEntities()
		{
			MapScreen.VisualsOfEntities.Remove(this.StrategicEntity.Pointer);
			foreach (GameEntity gameEntity in this.StrategicEntity.GetChildren())
			{
				MapScreen.VisualsOfEntities.Remove(gameEntity.Pointer);
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00021D78 File Offset: 0x0001FF78
		private bool IsPartOfBesiegerCamp(PartyBase party)
		{
			Settlement besiegedSettlement = party.MobileParty.BesiegedSettlement;
			return ((besiegedSettlement != null) ? besiegedSettlement.SiegeEvent : null) != null && party.MobileParty.BesiegedSettlement.SiegeEvent.BesiegerCamp.HasInvolvedPartyForEventType(party, MapEvent.BattleTypes.Siege);
		}

		// Token: 0x0400020A RID: 522
		private const float PartyScale = 0.3f;

		// Token: 0x0400020B RID: 523
		private const float HorseAnimationSpeedFactor = 1.3f;

		// Token: 0x0400020C RID: 524
		private float _speed;

		// Token: 0x0400020D RID: 525
		private float _entityAlpha;

		// Token: 0x0400020E RID: 526
		private float _transitionStartRotation;

		// Token: 0x0400020F RID: 527
		private Vec2 _lastFrameVisualPositionWithoutError;

		// Token: 0x04000210 RID: 528
		private bool _isEntityMovingCache;

		// Token: 0x04000211 RID: 529
		private bool _isInTransitionProgressCached;

		// Token: 0x04000212 RID: 530
		private float _bearingRotation;

		// Token: 0x04000213 RID: 531
		private ValueTuple<string, GameEntityComponent> _cachedBannerComponent;

		// Token: 0x04000214 RID: 532
		private ValueTuple<string, GameEntity> _cachedBannerEntity;

		// Token: 0x04000215 RID: 533
		private Scene _mapScene;
	}
}
