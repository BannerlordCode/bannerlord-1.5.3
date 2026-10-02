using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000372 RID: 882
	public class TrainingIcon : UsableMachine
	{
		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x060032CF RID: 13007 RVA: 0x000D0017 File Offset: 0x000CE217
		// (set) Token: 0x060032D0 RID: 13008 RVA: 0x000D001F File Offset: 0x000CE21F
		public bool Focused { get; private set; }

		// Token: 0x060032D1 RID: 13009 RVA: 0x000D0028 File Offset: 0x000CE228
		protected internal override void OnInit()
		{
			base.OnInit();
			this._markerBeam = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity.GetFirstChildEntityWithTag("highlight_beam"));
			this._weaponIcons = (from x in TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity).GetChildren()
				where !x.GetScriptComponents().Any<ScriptComponentBehavior>() && x != this._markerBeam
				select x).ToList<GameEntity>();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x060032D2 RID: 13010 RVA: 0x000D0091 File Offset: 0x000CE291
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x060032D3 RID: 13011 RVA: 0x000D009C File Offset: 0x000CE29C
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._markerBeam != null)
			{
				if (MathF.Abs(this._markerAlpha - this._targetMarkerAlpha) > dt * 110f)
				{
					this._markerAlpha += dt * 110f * (float)MathF.Sign(this._targetMarkerAlpha - this._markerAlpha);
					this._markerBeam.GetChild(0).GetFirstMesh().SetVectorArgument(this._markerAlpha, 1f, 0.49f, 11.65f);
				}
				else
				{
					this._markerAlpha = this._targetMarkerAlpha;
					if (this._targetMarkerAlpha == 0f)
					{
						GameEntity markerBeam = this._markerBeam;
						if (markerBeam != null)
						{
							markerBeam.SetVisibilityExcludeParents(false);
						}
					}
				}
			}
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				if (standingPoint.HasUser)
				{
					Agent userAgent = standingPoint.UserAgent;
					ActionIndexCache currentAction = userAgent.GetCurrentAction(0);
					ActionIndexCache currentAction2 = userAgent.GetCurrentAction(1);
					if (!(currentAction2 == ActionIndexCache.act_none) || (!(currentAction == ActionIndexCache.act_pickup_middle_begin) && !(currentAction == ActionIndexCache.act_pickup_middle_begin_left_stance)))
					{
						if (currentAction2 == ActionIndexCache.act_none && (currentAction == ActionIndexCache.act_pickup_middle_end || currentAction == ActionIndexCache.act_pickup_middle_end_left_stance))
						{
							this._activated = true;
							userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
						}
						else
						{
							if (!(currentAction2 != ActionIndexCache.act_none))
							{
								Agent agent = userAgent;
								int num = 0;
								ActionIndexCache actionIndexCache = (userAgent.GetIsLeftStance() ? ActionIndexCache.act_pickup_middle_begin_left_stance : ActionIndexCache.act_pickup_middle_begin);
								if (agent.SetActionChannel(num, in actionIndexCache, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true))
								{
									continue;
								}
							}
							userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
						}
					}
				}
			}
		}

		// Token: 0x060032D4 RID: 13012 RVA: 0x000D0294 File Offset: 0x000CE494
		public void SetMarked(bool highlight)
		{
			if (!highlight)
			{
				this._targetMarkerAlpha = 0f;
				return;
			}
			this._targetMarkerAlpha = 75f;
			this._markerBeam.GetChild(0).GetFirstMesh().SetVectorArgument(this._markerAlpha, 1f, 0.49f, 11.65f);
			GameEntity markerBeam = this._markerBeam;
			if (markerBeam == null)
			{
				return;
			}
			markerBeam.SetVisibilityExcludeParents(true);
		}

		// Token: 0x060032D5 RID: 13013 RVA: 0x000D02F7 File Offset: 0x000CE4F7
		public bool GetIsActivated()
		{
			bool activated = this._activated;
			this._activated = false;
			return activated;
		}

		// Token: 0x060032D6 RID: 13014 RVA: 0x000D0306 File Offset: 0x000CE506
		public string GetTrainingSubTypeTag()
		{
			return this._trainingSubTypeTag;
		}

		// Token: 0x060032D7 RID: 13015 RVA: 0x000D0310 File Offset: 0x000CE510
		public void DisableIcon()
		{
			foreach (GameEntity gameEntity in this._weaponIcons)
			{
				gameEntity.SetVisibilityExcludeParents(false);
			}
		}

		// Token: 0x060032D8 RID: 13016 RVA: 0x000D0364 File Offset: 0x000CE564
		public void EnableIcon()
		{
			foreach (GameEntity gameEntity in this._weaponIcons)
			{
				gameEntity.SetVisibilityExcludeParents(true);
			}
		}

		// Token: 0x060032D9 RID: 13017 RVA: 0x000D03B8 File Offset: 0x000CE5B8
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			TextObject textObject = new TextObject("{=!}{TRAINING_TYPE}", null);
			textObject.SetTextVariable("TRAINING_TYPE", GameTexts.FindText("str_tutorial_" + this._descriptionTextOfIcon, null));
			return textObject;
		}

		// Token: 0x060032DA RID: 13018 RVA: 0x000D03E7 File Offset: 0x000CE5E7
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject = null)
		{
			TextObject textObject = new TextObject("{=wY1qP2qj}{KEY} Select", null);
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			return textObject;
		}

		// Token: 0x060032DB RID: 13019 RVA: 0x000D0416 File Offset: 0x000CE616
		public override void OnFocusGain(Agent userAgent)
		{
			base.OnFocusGain(userAgent);
			this.Focused = true;
		}

		// Token: 0x060032DC RID: 13020 RVA: 0x000D0426 File Offset: 0x000CE626
		public override void OnFocusLose(Agent userAgent)
		{
			base.OnFocusLose(userAgent);
			this.Focused = false;
		}

		// Token: 0x040015A0 RID: 5536
		private const string HighlightBeamTag = "highlight_beam";

		// Token: 0x040015A1 RID: 5537
		private const float MarkerAlphaChangeAmount = 110f;

		// Token: 0x040015A3 RID: 5539
		private bool _activated;

		// Token: 0x040015A4 RID: 5540
		private float _markerAlpha;

		// Token: 0x040015A5 RID: 5541
		private float _targetMarkerAlpha;

		// Token: 0x040015A6 RID: 5542
		private List<GameEntity> _weaponIcons = new List<GameEntity>();

		// Token: 0x040015A7 RID: 5543
		private GameEntity _markerBeam;

		// Token: 0x040015A8 RID: 5544
		[EditableScriptComponentVariable(true, "")]
		private string _descriptionTextOfIcon = "";

		// Token: 0x040015A9 RID: 5545
		[EditableScriptComponentVariable(true, "")]
		private string _trainingSubTypeTag = "";
	}
}
