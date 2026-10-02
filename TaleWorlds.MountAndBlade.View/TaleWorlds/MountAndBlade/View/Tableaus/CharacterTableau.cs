using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Options;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;

namespace TaleWorlds.MountAndBlade.View.Tableaus
{
	// Token: 0x02000034 RID: 52
	public class CharacterTableau
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000191 RID: 401 RVA: 0x0000B2AB File Offset: 0x000094AB
		// (set) Token: 0x06000192 RID: 402 RVA: 0x0000B2B3 File Offset: 0x000094B3
		public Texture Texture { get; private set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000193 RID: 403 RVA: 0x0000B2BC File Offset: 0x000094BC
		public bool IsRunningCustomAnimation
		{
			get
			{
				return this._customAnimation != ActionIndexCache.act_none || this._customAnimationStartScheduled;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000194 RID: 404 RVA: 0x0000B2D8 File Offset: 0x000094D8
		// (set) Token: 0x06000195 RID: 405 RVA: 0x0000B2E0 File Offset: 0x000094E0
		public bool ShouldLoopCustomAnimation { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000196 RID: 406 RVA: 0x0000B2E9 File Offset: 0x000094E9
		// (set) Token: 0x06000197 RID: 407 RVA: 0x0000B2F1 File Offset: 0x000094F1
		public float CustomAnimationWaitDuration { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000198 RID: 408 RVA: 0x0000B2FA File Offset: 0x000094FA
		private TableauView View
		{
			get
			{
				if (this.Texture != null)
				{
					return this.Texture.TableauView;
				}
				return null;
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000B318 File Offset: 0x00009518
		public CharacterTableau()
		{
			this._leftHandEquipmentIndex = -1;
			this._rightHandEquipmentIndex = -1;
			this._isVisualsDirty = false;
			this._equipment = new Equipment();
			this.SetEnabled(true);
			this.FirstTimeInit();
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000B40C File Offset: 0x0000960C
		public void OnTick(float dt)
		{
			if (this._customAnimationStartScheduled)
			{
				this.StartCustomAnimation();
			}
			if (this._customAnimation != ActionIndexCache.act_none && this._characterActionSet.IsValid)
			{
				this._customAnimationTimer += dt;
				float actionAnimationDuration = MBActionSet.GetActionAnimationDuration(this._characterActionSet, in this._customAnimation);
				if (this._customAnimationTimer > actionAnimationDuration)
				{
					if (this._customAnimationTimer > actionAnimationDuration + this.CustomAnimationWaitDuration)
					{
						if (this.ShouldLoopCustomAnimation)
						{
							this.StartCustomAnimation();
						}
						else
						{
							this.StopCustomAnimationIfCantContinue();
						}
					}
					else
					{
						AgentVisuals agentVisuals = this._agentVisuals;
						if (agentVisuals != null)
						{
							ActionIndexCache idleAction = this.GetIdleAction();
							agentVisuals.SetAction(in idleAction, 0f, true);
						}
					}
				}
			}
			if (this._isEnabled && this._isRotatingCharacter)
			{
				this.UpdateCharacterRotation((int)Input.MouseMoveX);
			}
			if (this._animationFrequencyThreshold > this._animationGap)
			{
				this._animationGap += dt;
			}
			if (this._isEnabled)
			{
				AgentVisuals agentVisuals2 = this._agentVisuals;
				if (agentVisuals2 != null)
				{
					agentVisuals2.TickVisuals();
				}
				AgentVisuals oldAgentVisuals = this._oldAgentVisuals;
				if (oldAgentVisuals != null)
				{
					oldAgentVisuals.TickVisuals();
				}
				AgentVisuals mountVisuals = this._mountVisuals;
				if (mountVisuals != null)
				{
					mountVisuals.TickVisuals();
				}
				AgentVisuals oldMountVisuals = this._oldMountVisuals;
				if (oldMountVisuals != null)
				{
					oldMountVisuals.TickVisuals();
				}
			}
			if (this.View != null)
			{
				if (this._continuousRenderCamera == null)
				{
					this._continuousRenderCamera = Camera.CreateCamera();
				}
				this.View.SetDoNotRenderThisFrame(false);
			}
			if (this._isVisualsDirty)
			{
				this.RefreshCharacterTableau(this._oldEquipment);
				this._oldEquipment = null;
				this._isVisualsDirty = false;
			}
			if (this._agentVisualLoadingCounter > 0 && this._agentVisuals.GetEntity().CheckResources(true, true))
			{
				this._agentVisualLoadingCounter--;
			}
			if (this._mountVisualLoadingCounter > 0 && this._mountVisuals.GetEntity().CheckResources(true, true))
			{
				this._mountVisualLoadingCounter--;
			}
			if (this._mountVisualLoadingCounter == 0 && this._agentVisualLoadingCounter == 0)
			{
				AgentVisuals oldMountVisuals2 = this._oldMountVisuals;
				if (oldMountVisuals2 != null)
				{
					oldMountVisuals2.SetVisible(false);
				}
				AgentVisuals mountVisuals2 = this._mountVisuals;
				if (mountVisuals2 != null)
				{
					mountVisuals2.SetVisible(this._bodyProperties != BodyProperties.Default);
				}
				AgentVisuals oldAgentVisuals2 = this._oldAgentVisuals;
				if (oldAgentVisuals2 != null)
				{
					oldAgentVisuals2.SetVisible(false);
				}
				AgentVisuals agentVisuals3 = this._agentVisuals;
				if (agentVisuals3 != null)
				{
					agentVisuals3.SetVisible(this._bodyProperties != BodyProperties.Default);
				}
			}
			if (this._isEquipmentIndicesDirty)
			{
				this._agentVisuals.GetVisuals().SetWieldedWeaponIndices(this._rightHandEquipmentIndex, this._leftHandEquipmentIndex);
				this._isEquipmentIndicesDirty = false;
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000B688 File Offset: 0x00009888
		public float GetCustomAnimationProgressRatio()
		{
			if (!(this._customAnimation != ActionIndexCache.act_none) || !this._characterActionSet.IsValid)
			{
				return -1f;
			}
			float actionAnimationDuration = MBActionSet.GetActionAnimationDuration(this._characterActionSet, in this._customAnimation);
			if (actionAnimationDuration == 0f)
			{
				return -1f;
			}
			return this._customAnimationTimer / actionAnimationDuration;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000B6E4 File Offset: 0x000098E4
		private void StopCustomAnimationIfCantContinue()
		{
			bool flag = false;
			if (this._agentVisuals != null && this._customAnimation != ActionIndexCache.act_none)
			{
				ActionIndexCache actionAnimationContinueToAction = MBActionSet.GetActionAnimationContinueToAction(this._characterActionSet, in this._customAnimation);
				if (actionAnimationContinueToAction.Index >= 0)
				{
					this._customAnimationName = actionAnimationContinueToAction.GetName();
					this.StartCustomAnimation();
					flag = true;
				}
			}
			if (!flag)
			{
				this.StopCustomAnimation();
				this._customAnimationTimer = -1f;
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000B752 File Offset: 0x00009952
		public void SetEnabled(bool enabled)
		{
			this._isEnabled = enabled;
			TableauView view = this.View;
			if (view == null)
			{
				return;
			}
			view.SetEnable(this._isEnabled);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000B771 File Offset: 0x00009971
		public void SetLeftHandWieldedEquipmentIndex(int index)
		{
			this._leftHandEquipmentIndex = index;
			this._isEquipmentIndicesDirty = true;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000B781 File Offset: 0x00009981
		public void SetRightHandWieldedEquipmentIndex(int index)
		{
			this._rightHandEquipmentIndex = index;
			this._isEquipmentIndicesDirty = true;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000B794 File Offset: 0x00009994
		public void SetTargetSize(int width, int height)
		{
			this._isRotatingCharacter = false;
			this._latestWidth = width;
			this._latestHeight = height;
			if (width <= 0 || height <= 0)
			{
				this._tableauSizeX = 10;
				this._tableauSizeY = 10;
			}
			else
			{
				this._renderScale = NativeOptions.GetConfig(NativeOptions.NativeOptionsType.ResolutionScale) / 100f;
				this._tableauSizeX = (int)((float)width * this._customRenderScale * this._renderScale);
				this._tableauSizeY = (int)((float)height * this._customRenderScale * this._renderScale);
			}
			this._cameraRatio = (float)this._tableauSizeX / (float)this._tableauSizeY;
			TableauView view = this.View;
			if (view != null)
			{
				view.SetEnable(false);
			}
			TableauView view2 = this.View;
			if (view2 != null)
			{
				view2.AddClearTask(true);
			}
			Texture texture = this.Texture;
			if (texture != null)
			{
				texture.Release();
			}
			this.Texture = TableauView.AddTableau(string.Format("CharacterTableau_{0}", CharacterTableau._tableauIndex++), new RenderTargetComponent.TextureUpdateEventHandler(this.CharacterTableauContinuousRenderFunction), this._tableauScene, this._tableauSizeX, this._tableauSizeY);
			this.Texture.TableauView.SetSceneUsesContour(false);
			this.Texture.TableauView.SetFocusedShadowmap(true, ref this._initialSpawnFrame.origin, 2.55f);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000B8D2 File Offset: 0x00009AD2
		public void SetCharStringID(string charStringId)
		{
			this._charStringId = charStringId;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000B8DC File Offset: 0x00009ADC
		public void OnFinalize()
		{
			Camera continuousRenderCamera = this._continuousRenderCamera;
			if (continuousRenderCamera != null)
			{
				continuousRenderCamera.ReleaseCameraEntity();
				this._continuousRenderCamera = null;
			}
			AgentVisuals agentVisuals = this._agentVisuals;
			if (agentVisuals != null)
			{
				agentVisuals.ResetNextFrame();
			}
			this._agentVisuals = null;
			AgentVisuals mountVisuals = this._mountVisuals;
			if (mountVisuals != null)
			{
				mountVisuals.ResetNextFrame();
			}
			this._mountVisuals = null;
			AgentVisuals oldAgentVisuals = this._oldAgentVisuals;
			if (oldAgentVisuals != null)
			{
				oldAgentVisuals.ResetNextFrame();
			}
			this._oldAgentVisuals = null;
			AgentVisuals oldMountVisuals = this._oldMountVisuals;
			if (oldMountVisuals != null)
			{
				oldMountVisuals.ResetNextFrame();
			}
			this._oldMountVisuals = null;
			TableauView view = this.View;
			if (view != null)
			{
				view.SetEnable(false);
			}
			if (this._tableauScene != null)
			{
				if (this._bannerEntity != null)
				{
					this._tableauScene.RemoveEntity(this._bannerEntity, 0);
					this._bannerEntity = null;
				}
				if (this._agentRendererSceneController != null)
				{
					if (view != null)
					{
						view.SetEnable(false);
					}
					if (view != null)
					{
						view.AddClearTask(false);
					}
					MBAgentRendererSceneController.DestructAgentRendererSceneController(this._tableauScene, this._agentRendererSceneController, false);
					this._agentRendererSceneController = null;
					this._tableauScene.ManualInvalidate();
					this._tableauScene = null;
				}
				else
				{
					ThumbnailCacheManager.Current.ReturnCachedInventoryTableauScene();
					ThumbnailCacheManager.Current.ReturnCachedInventoryTableauScene();
					if (view != null)
					{
						view.AddClearTask(true);
					}
					Scene tableauScene = this._tableauScene;
					if (tableauScene != null)
					{
						tableauScene.ManualInvalidate();
					}
					this._tableauScene = null;
				}
			}
			Texture texture = this.Texture;
			if (texture != null)
			{
				texture.Release();
			}
			this.Texture = null;
			this._isFinalized = true;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000BA50 File Offset: 0x00009C50
		public void SetBodyProperties(string bodyPropertiesCode)
		{
			if (this._bodyPropertiesCode != bodyPropertiesCode)
			{
				this._bodyPropertiesCode = bodyPropertiesCode;
				BodyProperties bodyProperties;
				if (!string.IsNullOrEmpty(bodyPropertiesCode) && BodyProperties.FromString(bodyPropertiesCode, out bodyProperties))
				{
					this._bodyProperties = bodyProperties;
				}
				else
				{
					this._bodyProperties = BodyProperties.Default;
				}
				this._isVisualsDirty = true;
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000BA9F File Offset: 0x00009C9F
		public void SetStanceIndex(int index)
		{
			this._stanceIndex = (CharacterViewModel.StanceTypes)index;
			this._isVisualsDirty = true;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000BAAF File Offset: 0x00009CAF
		public void SetCustomRenderScale(float value)
		{
			if (!this._customRenderScale.ApproximatelyEqualsTo(value, 1E-05f))
			{
				this._customRenderScale = value;
				if (this._latestWidth != -1 && this._latestHeight != -1)
				{
					this.SetTargetSize(this._latestWidth, this._latestHeight);
				}
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000BAF0 File Offset: 0x00009CF0
		private void AdjustCharacterForStanceIndex()
		{
			switch (this._stanceIndex)
			{
			case CharacterViewModel.StanceTypes.None:
			{
				AgentVisuals agentVisuals = this._agentVisuals;
				if (agentVisuals != null)
				{
					ActionIndexCache actionIndexCache = this.GetIdleAction();
					agentVisuals.SetAction(in actionIndexCache, 0f, true);
				}
				AgentVisuals oldAgentVisuals = this._oldAgentVisuals;
				if (oldAgentVisuals != null)
				{
					ActionIndexCache actionIndexCache = this.GetIdleAction();
					oldAgentVisuals.SetAction(in actionIndexCache, 0f, true);
				}
				break;
			}
			case CharacterViewModel.StanceTypes.EmphasizeFace:
			{
				this._camPos = this._camPosGatheredFromScene;
				this._camPos.Elevate(-2f);
				this._camPos.Advance(0.5f);
				AgentVisuals agentVisuals2 = this._agentVisuals;
				if (agentVisuals2 != null)
				{
					ActionIndexCache actionIndexCache = this.GetIdleAction();
					agentVisuals2.SetAction(in actionIndexCache, 0f, true);
				}
				AgentVisuals oldAgentVisuals2 = this._oldAgentVisuals;
				if (oldAgentVisuals2 != null)
				{
					ActionIndexCache actionIndexCache = this.GetIdleAction();
					oldAgentVisuals2.SetAction(in actionIndexCache, 0f, true);
				}
				break;
			}
			case CharacterViewModel.StanceTypes.SideView:
			case CharacterViewModel.StanceTypes.OnMount:
				if (this._agentVisuals != null)
				{
					this._camPos = this._camPosGatheredFromScene;
					if (this._equipment[10].Item != null)
					{
						this._camPos.Advance(0.5f);
						AgentVisuals agentVisuals3 = this._agentVisuals;
						ActionIndexCache actionIndexCache = this._mountVisuals.GetEntity().Skeleton.GetActionAtChannel(0);
						agentVisuals3.SetAction(in actionIndexCache, this._mountVisuals.GetEntity().Skeleton.GetAnimationParameterAtChannel(0), true);
						AgentVisuals oldAgentVisuals3 = this._oldAgentVisuals;
						actionIndexCache = this._mountVisuals.GetEntity().Skeleton.GetActionAtChannel(0);
						oldAgentVisuals3.SetAction(in actionIndexCache, this._mountVisuals.GetEntity().Skeleton.GetAnimationParameterAtChannel(0), true);
					}
					else
					{
						this._camPos.Elevate(-2f);
						this._camPos.Advance(0.5f);
						AgentVisuals agentVisuals4 = this._agentVisuals;
						ActionIndexCache actionIndexCache = this.GetIdleAction();
						agentVisuals4.SetAction(in actionIndexCache, 0f, true);
						AgentVisuals oldAgentVisuals4 = this._oldAgentVisuals;
						actionIndexCache = this.GetIdleAction();
						oldAgentVisuals4.SetAction(in actionIndexCache, 0f, true);
					}
				}
				break;
			case CharacterViewModel.StanceTypes.CelebrateVictory:
			{
				AgentVisuals agentVisuals5 = this._agentVisuals;
				if (agentVisuals5 != null)
				{
					agentVisuals5.SetAction(in ActionIndexCache.act_cheer_1, 0f, true);
				}
				AgentVisuals oldAgentVisuals5 = this._oldAgentVisuals;
				if (oldAgentVisuals5 != null)
				{
					oldAgentVisuals5.SetAction(in ActionIndexCache.act_cheer_1, 0f, true);
				}
				break;
			}
			}
			if (this._agentVisuals != null)
			{
				GameEntity entity = this._agentVisuals.GetEntity();
				Skeleton skeleton = entity.Skeleton;
				skeleton.TickAnimations(0.01f, this._agentVisuals.GetVisuals().GetGlobalFrame(), true);
				if (!string.IsNullOrEmpty(this._idleFaceAnim))
				{
					skeleton.SetFacialAnimation(Agent.FacialAnimChannel.Mid, this._idleFaceAnim, false, true);
				}
				entity.ManualInvalidate();
				skeleton.ManualInvalidate();
			}
			if (this._oldAgentVisuals != null)
			{
				GameEntity entity2 = this._oldAgentVisuals.GetEntity();
				Skeleton skeleton2 = entity2.Skeleton;
				skeleton2.TickAnimations(0.01f, this._oldAgentVisuals.GetVisuals().GetGlobalFrame(), true);
				if (!string.IsNullOrEmpty(this._idleFaceAnim))
				{
					skeleton2.SetFacialAnimation(Agent.FacialAnimChannel.Mid, this._idleFaceAnim, false, true);
				}
				entity2.ManualInvalidate();
				skeleton2.ManualInvalidate();
			}
			if (this._mountVisuals != null)
			{
				GameEntity entity3 = this._mountVisuals.GetEntity();
				Skeleton skeleton3 = entity3.Skeleton;
				skeleton3.TickAnimations(0.01f, this._mountVisuals.GetVisuals().GetGlobalFrame(), true);
				if (!string.IsNullOrEmpty(this._idleFaceAnim))
				{
					skeleton3.SetFacialAnimation(Agent.FacialAnimChannel.Mid, this._idleFaceAnim, false, true);
				}
				entity3.ManualInvalidate();
				skeleton3.ManualInvalidate();
			}
			if (this._oldMountVisuals != null)
			{
				GameEntity entity4 = this._oldMountVisuals.GetEntity();
				Skeleton skeleton4 = entity4.Skeleton;
				skeleton4.TickAnimations(0.01f, this._oldMountVisuals.GetVisuals().GetGlobalFrame(), true);
				entity4.ManualInvalidate();
				skeleton4.ManualInvalidate();
			}
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000BE8C File Offset: 0x0000A08C
		private void ForceRefresh()
		{
			int stanceIndex = (int)this._stanceIndex;
			this._stanceIndex = CharacterViewModel.StanceTypes.None;
			this.SetStanceIndex(stanceIndex);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000BEAE File Offset: 0x0000A0AE
		public void SetIsFemale(bool isFemale)
		{
			this._isFemale = isFemale;
			this._isVisualsDirty = true;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000BEBE File Offset: 0x0000A0BE
		public void SetIsBannerShownInBackground(bool isBannerShownInBackground)
		{
			this._isBannerShownInBackground = isBannerShownInBackground;
			this._isVisualsDirty = true;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000BECE File Offset: 0x0000A0CE
		public void SetRace(int race)
		{
			this._race = race;
			this._isVisualsDirty = true;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000BEDE File Offset: 0x0000A0DE
		public void SetIdleAction(string idleAction)
		{
			this._idleAction = ActionIndexCache.Create(idleAction);
			this._isVisualsDirty = true;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000BEF3 File Offset: 0x0000A0F3
		public void SetCustomAnimation(string animation)
		{
			this._customAnimationName = animation;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000BEFC File Offset: 0x0000A0FC
		public void StartCustomAnimation()
		{
			if (this._isVisualsDirty || this._agentVisuals == null || string.IsNullOrEmpty(this._customAnimationName))
			{
				this._customAnimationStartScheduled = true;
				return;
			}
			this.StopCustomAnimation();
			this._customAnimation = ActionIndexCache.Create(this._customAnimationName);
			if (this._customAnimation.Index >= 0)
			{
				this._agentVisuals.SetAction(in this._customAnimation, 0f, true);
				this._customAnimationStartScheduled = false;
				this._customAnimationTimer = 0f;
				return;
			}
			Debug.FailedAssert("Invalid custom animation in character tableau: " + this._customAnimationName, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\Tableaus\\CharacterTableau.cs", "StartCustomAnimation", 593);
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000BFA4 File Offset: 0x0000A1A4
		public void StopCustomAnimation()
		{
			if (this._agentVisuals != null && this._customAnimation != ActionIndexCache.act_none)
			{
				if (MBActionSet.GetActionAnimationContinueToAction(this._characterActionSet, in this._customAnimation).Index < 0)
				{
					AgentVisuals agentVisuals = this._agentVisuals;
					ActionIndexCache idleAction = this.GetIdleAction();
					agentVisuals.SetAction(in idleAction, 0f, true);
				}
				this._customAnimation = ActionIndexCache.act_none;
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000C00C File Offset: 0x0000A20C
		public void SetIdleFaceAnim(string idleFaceAnim)
		{
			if (!string.IsNullOrEmpty(idleFaceAnim))
			{
				this._idleFaceAnim = idleFaceAnim;
				this._isVisualsDirty = true;
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000C024 File Offset: 0x0000A224
		public void SetEquipmentCode(string equipmentCode)
		{
			if (this._equipmentCode != equipmentCode && !string.IsNullOrEmpty(equipmentCode))
			{
				this._oldEquipment = Equipment.CreateFromEquipmentCode(this._equipmentCode);
				this._equipmentCode = equipmentCode;
				this._equipment = Equipment.CreateFromEquipmentCode(equipmentCode);
				this._bannerItem = this.GetAndRemoveBannerFromEquipment(ref this._equipment);
				this._isVisualsDirty = true;
			}
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000C084 File Offset: 0x0000A284
		public void SetIsEquipmentAnimActive(bool value)
		{
			this._isEquipmentAnimActive = value;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000C08D File Offset: 0x0000A28D
		public void SetMountCreationKey(string value)
		{
			if (this._mountCreationKey != value)
			{
				this._mountCreationKey = value;
				this._isVisualsDirty = true;
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000C0AB File Offset: 0x0000A2AB
		public void SetBannerCode(string value)
		{
			this._banner = (string.IsNullOrEmpty(value) ? null : new Banner(value));
			this._isVisualsDirty = true;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000C0CB File Offset: 0x0000A2CB
		public void SetArmorColor1(uint clothColor1)
		{
			if (this._clothColor1 != clothColor1)
			{
				this._clothColor1 = clothColor1;
				this._isVisualsDirty = true;
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000C0E4 File Offset: 0x0000A2E4
		public void SetArmorColor2(uint clothColor2)
		{
			if (this._clothColor2 != clothColor2)
			{
				this._clothColor2 = clothColor2;
				this._isVisualsDirty = true;
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000C0FD File Offset: 0x0000A2FD
		private ActionIndexCache GetIdleAction()
		{
			if (!(this._idleAction != ActionIndexCache.act_none))
			{
				return ActionIndexCache.act_inventory_idle_start;
			}
			return this._idleAction;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000C120 File Offset: 0x0000A320
		private void RefreshCharacterTableau(Equipment oldEquipment = null)
		{
			this.UpdateMount(this._stanceIndex == CharacterViewModel.StanceTypes.OnMount);
			this.UpdateBannerItem();
			if (this._mountVisuals == null && this._isCharacterMountPlacesSwapped)
			{
				this._isCharacterMountPlacesSwapped = false;
				this._mainCharacterRotation = 0f;
			}
			if (this._agentVisuals != null)
			{
				bool visibilityExcludeParents = this._oldAgentVisuals.GetEntity().GetVisibilityExcludeParents();
				AgentVisuals oldAgentVisuals = this._oldAgentVisuals;
				AgentVisuals agentVisuals = this._agentVisuals;
				this._agentVisuals = oldAgentVisuals;
				this._oldAgentVisuals = agentVisuals;
				this._agentVisualLoadingCounter = 1;
				AgentVisualsData copyAgentVisualsData = this._agentVisuals.GetCopyAgentVisualsData();
				MatrixFrame matrixFrame = (this._isCharacterMountPlacesSwapped ? this._characterMountPositionFrame : this._initialSpawnFrame);
				if (!this._isCharacterMountPlacesSwapped)
				{
					matrixFrame.rotation.RotateAboutUp(this._mainCharacterRotation);
				}
				this._characterActionSet = MBGlobals.GetActionSetWithSuffix(copyAgentVisualsData.MonsterData, this._isFemale, "_warrior");
				copyAgentVisualsData.BodyProperties(this._bodyProperties).SkeletonType(this._isFemale ? SkeletonType.Female : SkeletonType.Male).Frame(matrixFrame)
					.ActionSet(this._characterActionSet)
					.Equipment(this._equipment)
					.Banner(this._banner)
					.UseMorphAnims(true)
					.ClothColor1(this._clothColor1)
					.ClothColor2(this._clothColor2)
					.Race(this._race);
				if (this._initialLoadingCounter > 0)
				{
					this._initialLoadingCounter--;
				}
				this._agentVisuals.Refresh(false, copyAgentVisualsData, false);
				this._agentVisuals.SetVisible(false);
				if (this._initialLoadingCounter == 0)
				{
					this._oldAgentVisuals.SetVisible(visibilityExcludeParents);
				}
				if (oldEquipment != null && this._animationFrequencyThreshold <= this._animationGap && this._isEquipmentAnimActive)
				{
					if (this._equipment[EquipmentIndex.Gloves].Item != null && oldEquipment[EquipmentIndex.Gloves].Item != this._equipment[EquipmentIndex.Gloves].Item)
					{
						this._agentVisuals.GetVisuals().GetSkeleton().SetAgentActionChannel(0, in ActionIndexCache.act_inventory_glove_equip, 0f, -0.2f, true, 0f);
						this._animationGap = 0f;
					}
					else if (this._equipment[EquipmentIndex.Body].Item != null && oldEquipment[EquipmentIndex.Body].Item != this._equipment[EquipmentIndex.Body].Item)
					{
						this._agentVisuals.GetVisuals().GetSkeleton().SetAgentActionChannel(0, in ActionIndexCache.act_inventory_cloth_equip, 0f, -0.2f, true, 0f);
						this._animationGap = 0f;
					}
				}
				this._agentVisuals.GetEntity().CheckResources(true, true);
			}
			this.AdjustCharacterForStanceIndex();
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000C3D4 File Offset: 0x0000A5D4
		public void RotateCharacter(bool value)
		{
			this._isRotatingCharacter = value;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000C3DD File Offset: 0x0000A5DD
		public void TriggerCharacterMountPlacesSwap()
		{
			this._mainCharacterRotation = 0f;
			this._isCharacterMountPlacesSwapped = !this._isCharacterMountPlacesSwapped;
			this._isVisualsDirty = true;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000C400 File Offset: 0x0000A600
		public void OnCharacterTableauMouseMove(int mouseMoveX)
		{
			this.UpdateCharacterRotation(mouseMoveX);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000C40C File Offset: 0x0000A60C
		private void UpdateCharacterRotation(int mouseMoveX)
		{
			if (this._agentVisuals != null)
			{
				float num = (float)mouseMoveX * 0.005f;
				this._mainCharacterRotation += num;
				if (this._isCharacterMountPlacesSwapped)
				{
					MatrixFrame frame = this._mountVisuals.GetEntity().GetFrame();
					frame.rotation.RotateAboutUp(num);
					this._mountVisuals.GetEntity().SetFrame(ref frame, true);
					return;
				}
				MatrixFrame frame2 = this._agentVisuals.GetEntity().GetFrame();
				frame2.rotation.RotateAboutUp(num);
				this._agentVisuals.GetEntity().SetFrame(ref frame2, true);
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000C4A8 File Offset: 0x0000A6A8
		private void FirstTimeInit()
		{
			if (this._continuousRenderCamera == null)
			{
				this._continuousRenderCamera = Camera.CreateCamera();
			}
			if (this._equipment != null)
			{
				if (this._tableauScene == null)
				{
					if (ThumbnailCacheManager.Current.IsCachedInventoryTableauSceneUsed())
					{
						this._tableauScene = Scene.CreateNewScene(true, false, DecalAtlasGroup.All, "mono_renderscene");
						this._tableauScene.SetName("CharacterTableau");
						this._tableauScene.DisableStaticShadows(true);
						this._tableauScene.SetClothSimulationState(true);
						this._agentRendererSceneController = MBAgentRendererSceneController.CreateNewAgentRendererSceneController(this._tableauScene);
						SceneInitializationData sceneInitializationData = new SceneInitializationData(true);
						sceneInitializationData.InitPhysicsWorld = false;
						sceneInitializationData.DoNotUseLoadingScreen = true;
						this._tableauScene.Read("inventory_character_scene", ref sceneInitializationData, "");
					}
					else
					{
						this._tableauScene = ThumbnailCacheManager.Current.GetCachedInventoryTableauScene();
					}
					this._tableauScene.SetShadow(true);
					this._tableauScene.SetClothSimulationState(true);
					this._camPos = (this._camPosGatheredFromScene = ThumbnailCacheManager.Current.InventorySceneCameraFrame);
					this._mountSpawnPoint = this._tableauScene.FindEntityWithTag("horse_inv").GetGlobalFrame();
					this._bannerSpawnPoint = this._tableauScene.FindEntityWithTag("banner_inv").GetGlobalFrame();
					this._initialSpawnFrame = this._tableauScene.FindEntityWithTag("agent_inv").GetGlobalFrame();
					this._characterMountPositionFrame = new MatrixFrame(in this._initialSpawnFrame.rotation, in this._mountSpawnPoint.origin);
					this._characterMountPositionFrame.Strafe(-0.25f);
					this._mountCharacterPositionFrame = new MatrixFrame(in this._mountSpawnPoint.rotation, in this._initialSpawnFrame.origin);
					this._mountCharacterPositionFrame.Strafe(0.25f);
					if (this._agentRendererSceneController != null)
					{
						this._tableauScene.RemoveEntity(this._tableauScene.FindEntityWithTag("agent_inv"), 99);
						this._tableauScene.RemoveEntity(this._tableauScene.FindEntityWithTag("horse_inv"), 100);
						this._tableauScene.RemoveEntity(this._tableauScene.FindEntityWithTag("banner_inv"), 101);
					}
				}
				this.InitializeAgentVisuals();
				this._isVisualsDirty = true;
			}
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000C6D8 File Offset: 0x0000A8D8
		private void InitializeAgentVisuals()
		{
			Monster baseMonsterFromRace = FaceGen.GetBaseMonsterFromRace(this._race);
			this._characterActionSet = MBGlobals.GetActionSetWithSuffix(baseMonsterFromRace, this._isFemale, "_warrior");
			AgentVisualsData agentVisualsData = new AgentVisualsData().Banner(this._banner).Equipment(this._equipment).BodyProperties(this._bodyProperties)
				.Race(this._race)
				.Frame(this._initialSpawnFrame)
				.UseMorphAnims(true)
				.ActionSet(this._characterActionSet);
			ActionIndexCache actionIndexCache = this.GetIdleAction();
			this._oldAgentVisuals = AgentVisuals.Create(agentVisualsData.ActionCode(in actionIndexCache).Scene(this._tableauScene).Monster(baseMonsterFromRace)
				.PrepareImmediately(false)
				.SkeletonType(this._isFemale ? SkeletonType.Female : SkeletonType.Male)
				.ClothColor1(this._clothColor1)
				.ClothColor2(this._clothColor2)
				.CharacterObjectStringId(this._charStringId), "CharacterTableau", false, false, false);
			this._oldAgentVisuals.SetAgentLodZeroOrMaxExternal(true);
			this._oldAgentVisuals.SetVisible(false);
			AgentVisualsData agentVisualsData2 = new AgentVisualsData().Banner(this._banner).Equipment(this._equipment).BodyProperties(this._bodyProperties)
				.Race(this._race)
				.Frame(this._initialSpawnFrame)
				.UseMorphAnims(true)
				.ActionSet(this._characterActionSet);
			actionIndexCache = this.GetIdleAction();
			this._agentVisuals = AgentVisuals.Create(agentVisualsData2.ActionCode(in actionIndexCache).Scene(this._tableauScene).Monster(baseMonsterFromRace)
				.PrepareImmediately(false)
				.SkeletonType(this._isFemale ? SkeletonType.Female : SkeletonType.Male)
				.ClothColor1(this._clothColor1)
				.ClothColor2(this._clothColor2)
				.CharacterObjectStringId(this._charStringId), "CharacterTableau", false, false, false);
			this._agentVisuals.SetAgentLodZeroOrMaxExternal(true);
			this._agentVisuals.SetVisible(false);
			this._initialLoadingCounter = 2;
			if (!string.IsNullOrEmpty(this._idleFaceAnim))
			{
				this._agentVisuals.GetVisuals().GetSkeleton().SetFacialAnimation(Agent.FacialAnimChannel.Mid, this._idleFaceAnim, false, true);
				this._oldAgentVisuals.GetVisuals().GetSkeleton().SetFacialAnimation(Agent.FacialAnimChannel.Mid, this._idleFaceAnim, false, true);
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000C8F8 File Offset: 0x0000AAF8
		private void UpdateMount(bool isRiderAgentMounted = false)
		{
			ItemObject item = this._equipment[EquipmentIndex.ArmorItemEndSlot].Item;
			if (((item != null) ? item.HorseComponent : null) != null)
			{
				ItemObject item2 = this._equipment[EquipmentIndex.ArmorItemEndSlot].Item;
				Monster monster = item2.HorseComponent.Monster;
				Equipment equipment = new Equipment();
				equipment[EquipmentIndex.ArmorItemEndSlot] = this._equipment[EquipmentIndex.ArmorItemEndSlot];
				equipment[EquipmentIndex.HorseHarness] = this._equipment[EquipmentIndex.HorseHarness];
				Equipment equipment2 = equipment;
				MatrixFrame matrixFrame = (this._isCharacterMountPlacesSwapped ? this._mountCharacterPositionFrame : this._mountSpawnPoint);
				if (this._isCharacterMountPlacesSwapped)
				{
					matrixFrame.rotation.RotateAboutUp(this._mainCharacterRotation);
				}
				if (this._oldMountVisuals != null)
				{
					this._oldMountVisuals.ResetNextFrame();
				}
				this._oldMountVisuals = this._mountVisuals;
				this._mountVisualLoadingCounter = 3;
				AgentVisualsData agentVisualsData = new AgentVisualsData();
				AgentVisualsData agentVisualsData2 = agentVisualsData.Banner(this._banner).Equipment(equipment2).Frame(matrixFrame)
					.Scale(item2.ScaleFactor)
					.ActionSet(MBGlobals.GetActionSet(monster.ActionSetCode));
				ActionIndexCache actionIndexCache = (isRiderAgentMounted ? ((monster.MonsterUsage == "camel") ? ActionIndexCache.act_inventory_idle_start : ActionIndexCache.act_inventory_idle_start) : this.GetIdleAction());
				agentVisualsData2.ActionCode(in actionIndexCache).Scene(this._tableauScene).Monster(monster)
					.PrepareImmediately(false)
					.ClothColor1(this._clothColor1)
					.ClothColor2(this._clothColor2)
					.MountCreationKey(this._mountCreationKey);
				this._mountVisuals = AgentVisuals.Create(agentVisualsData, "MountTableau", false, false, false);
				this._mountVisuals.SetAgentLodZeroOrMaxExternal(true);
				this._mountVisuals.SetVisible(false);
				this._mountVisuals.SetClothingColors(this._clothColor1, this._clothColor2);
				this._mountVisuals.GetEntity().CheckResources(true, true);
				return;
			}
			if (this._mountVisuals != null)
			{
				this._mountVisuals.Reset();
				this._mountVisuals = null;
				this._mountVisualLoadingCounter = 0;
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000CAF8 File Offset: 0x0000ACF8
		private void UpdateBannerItem()
		{
			if (this._bannerEntity != null)
			{
				this._tableauScene.RemoveEntity(this._bannerEntity, 0);
				this._bannerEntity = null;
			}
			if (this._isBannerShownInBackground && this._bannerItem != null)
			{
				this._bannerEntity = GameEntity.CreateEmpty(this._tableauScene, true, true, true);
				this._bannerEntity.SetFrame(ref this._bannerSpawnPoint, true);
				this._bannerEntity.AddMultiMesh(this._bannerItem.GetMultiMeshCopy(), true);
				if (this._banner != null)
				{
					Banner banner = this._banner;
					BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual(base.GetType().Name);
					banner.GetTableauTextureLarge(in bannerDebugInfo, delegate(Texture t)
					{
						this.OnBannerTableauRenderDone(t);
					});
				}
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000CBAC File Offset: 0x0000ADAC
		private void OnBannerTableauRenderDone(Texture newTexture)
		{
			if (this._isFinalized)
			{
				return;
			}
			if (this._bannerEntity == null)
			{
				return;
			}
			foreach (Mesh mesh in this._bannerEntity.GetAllMeshesWithTag("banner_replacement_mesh"))
			{
				this.ApplyBannerTextureToMesh(mesh, newTexture);
			}
			Skeleton skeleton = this._bannerEntity.Skeleton;
			if (((skeleton != null) ? skeleton.GetAllMeshes() : null) != null)
			{
				Skeleton skeleton2 = this._bannerEntity.Skeleton;
				foreach (Mesh mesh2 in ((skeleton2 != null) ? skeleton2.GetAllMeshes() : null))
				{
					if (mesh2.HasTag("banner_replacement_mesh"))
					{
						this.ApplyBannerTextureToMesh(mesh2, newTexture);
					}
				}
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000CC90 File Offset: 0x0000AE90
		private void ApplyBannerTextureToMesh(Mesh bannerMesh, Texture bannerTexture)
		{
			if (bannerMesh != null)
			{
				Material material = bannerMesh.GetMaterial().CreateCopy();
				material.SetTexture(Material.MBTextureType.DiffuseMap2, bannerTexture);
				uint num = (uint)material.GetShader().GetMaterialShaderFlagMask("use_tableau_blending", true);
				ulong shaderFlags = material.GetShaderFlags();
				material.SetShaderFlags(shaderFlags | (ulong)num);
				bannerMesh.SetMaterial(material);
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000CCE8 File Offset: 0x0000AEE8
		private ItemObject GetAndRemoveBannerFromEquipment(ref Equipment equipment)
		{
			ItemObject itemObject = null;
			ItemObject item = equipment[EquipmentIndex.ExtraWeaponSlot].Item;
			if (item != null && item.IsBannerItem)
			{
				itemObject = equipment[EquipmentIndex.ExtraWeaponSlot].Item;
				equipment[EquipmentIndex.ExtraWeaponSlot] = EquipmentElement.Invalid;
			}
			return itemObject;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000CD34 File Offset: 0x0000AF34
		internal void CharacterTableauContinuousRenderFunction(Texture sender, EventArgs e)
		{
			Scene scene = (Scene)sender.UserData;
			TableauView tableauView = sender.TableauView;
			if (scene == null)
			{
				tableauView.SetContinuousRendering(false);
				tableauView.SetDeleteAfterRendering(true);
				return;
			}
			scene.EnsurePostfxSystem();
			scene.SetDofMode(false);
			scene.SetMotionBlurMode(false);
			scene.SetBloom(true);
			scene.SetDynamicShadowmapCascadesRadiusMultiplier(0.31f);
			tableauView.SetRenderWithPostfx(true);
			float cameraRatio = this._cameraRatio;
			MatrixFrame camPos = this._camPos;
			Camera continuousRenderCamera = this._continuousRenderCamera;
			if (continuousRenderCamera != null)
			{
				continuousRenderCamera.SetFovVertical(0.7853982f, cameraRatio, 0.2f, 200f);
				continuousRenderCamera.Frame = camPos;
				tableauView.SetCamera(continuousRenderCamera);
				tableauView.SetScene(scene);
				tableauView.SetSceneUsesSkybox(false);
				tableauView.SetDeleteAfterRendering(false);
				tableauView.SetContinuousRendering(true);
				tableauView.SetDoNotRenderThisFrame(true);
				tableauView.SetClearColor(0U);
				tableauView.SetFocusedShadowmap(true, ref this._initialSpawnFrame.origin, 1.55f);
			}
		}

		// Token: 0x040000B8 RID: 184
		private static int _tableauIndex;

		// Token: 0x040000BC RID: 188
		private bool _isFinalized;

		// Token: 0x040000BD RID: 189
		private MatrixFrame _mountSpawnPoint;

		// Token: 0x040000BE RID: 190
		private MatrixFrame _bannerSpawnPoint;

		// Token: 0x040000BF RID: 191
		private float _animationFrequencyThreshold = 2.5f;

		// Token: 0x040000C0 RID: 192
		private MatrixFrame _initialSpawnFrame;

		// Token: 0x040000C1 RID: 193
		private MatrixFrame _characterMountPositionFrame;

		// Token: 0x040000C2 RID: 194
		private MatrixFrame _mountCharacterPositionFrame;

		// Token: 0x040000C3 RID: 195
		private AgentVisuals _agentVisuals;

		// Token: 0x040000C4 RID: 196
		private AgentVisuals _mountVisuals;

		// Token: 0x040000C5 RID: 197
		private int _agentVisualLoadingCounter;

		// Token: 0x040000C6 RID: 198
		private int _mountVisualLoadingCounter;

		// Token: 0x040000C7 RID: 199
		private AgentVisuals _oldAgentVisuals;

		// Token: 0x040000C8 RID: 200
		private AgentVisuals _oldMountVisuals;

		// Token: 0x040000C9 RID: 201
		private int _initialLoadingCounter;

		// Token: 0x040000CA RID: 202
		private ActionIndexCache _idleAction = ActionIndexCache.act_none;

		// Token: 0x040000CB RID: 203
		private string _idleFaceAnim;

		// Token: 0x040000CC RID: 204
		private Scene _tableauScene;

		// Token: 0x040000CD RID: 205
		private MBAgentRendererSceneController _agentRendererSceneController;

		// Token: 0x040000CE RID: 206
		private Camera _continuousRenderCamera;

		// Token: 0x040000CF RID: 207
		private float _cameraRatio;

		// Token: 0x040000D0 RID: 208
		private MatrixFrame _camPos;

		// Token: 0x040000D1 RID: 209
		private MatrixFrame _camPosGatheredFromScene;

		// Token: 0x040000D2 RID: 210
		private string _charStringId;

		// Token: 0x040000D3 RID: 211
		private int _tableauSizeX;

		// Token: 0x040000D4 RID: 212
		private int _tableauSizeY;

		// Token: 0x040000D5 RID: 213
		private uint _clothColor1 = new Color(1f, 1f, 1f, 1f).ToUnsignedInteger();

		// Token: 0x040000D6 RID: 214
		private uint _clothColor2 = new Color(1f, 1f, 1f, 1f).ToUnsignedInteger();

		// Token: 0x040000D7 RID: 215
		private bool _isRotatingCharacter;

		// Token: 0x040000D8 RID: 216
		private bool _isCharacterMountPlacesSwapped;

		// Token: 0x040000D9 RID: 217
		private string _mountCreationKey = "";

		// Token: 0x040000DA RID: 218
		private string _equipmentCode = "";

		// Token: 0x040000DB RID: 219
		private bool _isEquipmentAnimActive;

		// Token: 0x040000DC RID: 220
		private float _animationGap;

		// Token: 0x040000DD RID: 221
		private float _mainCharacterRotation;

		// Token: 0x040000DE RID: 222
		private bool _isEnabled;

		// Token: 0x040000DF RID: 223
		private float _renderScale = 1f;

		// Token: 0x040000E0 RID: 224
		private float _customRenderScale = 1f;

		// Token: 0x040000E1 RID: 225
		private int _latestWidth = -1;

		// Token: 0x040000E2 RID: 226
		private int _latestHeight = -1;

		// Token: 0x040000E3 RID: 227
		private string _bodyPropertiesCode;

		// Token: 0x040000E4 RID: 228
		private BodyProperties _bodyProperties = BodyProperties.Default;

		// Token: 0x040000E5 RID: 229
		private bool _isFemale;

		// Token: 0x040000E6 RID: 230
		private CharacterViewModel.StanceTypes _stanceIndex;

		// Token: 0x040000E7 RID: 231
		private Equipment _equipment;

		// Token: 0x040000E8 RID: 232
		private Banner _banner;

		// Token: 0x040000E9 RID: 233
		private int _race;

		// Token: 0x040000EA RID: 234
		private bool _isBannerShownInBackground;

		// Token: 0x040000EB RID: 235
		private ItemObject _bannerItem;

		// Token: 0x040000EC RID: 236
		private GameEntity _bannerEntity;

		// Token: 0x040000ED RID: 237
		private int _leftHandEquipmentIndex;

		// Token: 0x040000EE RID: 238
		private int _rightHandEquipmentIndex;

		// Token: 0x040000EF RID: 239
		private bool _isEquipmentIndicesDirty;

		// Token: 0x040000F0 RID: 240
		private bool _customAnimationStartScheduled;

		// Token: 0x040000F1 RID: 241
		private float _customAnimationTimer;

		// Token: 0x040000F2 RID: 242
		private string _customAnimationName;

		// Token: 0x040000F3 RID: 243
		private ActionIndexCache _customAnimation = ActionIndexCache.act_none;

		// Token: 0x040000F4 RID: 244
		private MBActionSet _characterActionSet;

		// Token: 0x040000F5 RID: 245
		private bool _isVisualsDirty;

		// Token: 0x040000F6 RID: 246
		private Equipment _oldEquipment;
	}
}
