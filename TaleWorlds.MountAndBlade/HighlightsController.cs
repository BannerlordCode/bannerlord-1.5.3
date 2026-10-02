using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200028C RID: 652
	public class HighlightsController : MissionLogic
	{
		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x0600247C RID: 9340 RVA: 0x00083B3B File Offset: 0x00081D3B
		// (set) Token: 0x0600247D RID: 9341 RVA: 0x00083B42 File Offset: 0x00081D42
		private protected static List<HighlightsController.HighlightType> HighlightTypes { protected get; private set; }

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x0600247E RID: 9342 RVA: 0x00083B4A File Offset: 0x00081D4A
		// (set) Token: 0x0600247F RID: 9343 RVA: 0x00083B51 File Offset: 0x00081D51
		public static bool IsHighlightsInitialized { get; private set; }

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06002480 RID: 9344 RVA: 0x00083B59 File Offset: 0x00081D59
		public bool IsAnyHighlightSaved
		{
			get
			{
				return this._savedHighlightGroups.Count > 0;
			}
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x00083B6C File Offset: 0x00081D6C
		public static void RemoveHighlights()
		{
			if (HighlightsController.IsHighlightsInitialized)
			{
				foreach (HighlightsController.HighlightType highlightType in HighlightsController.HighlightTypes)
				{
					Highlights.RemoveHighlight(highlightType.Id);
				}
			}
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x00083BCC File Offset: 0x00081DCC
		public HighlightsController.HighlightType GetHighlightTypeWithId(string highlightId)
		{
			return HighlightsController.HighlightTypes.First<HighlightsController.HighlightType>((HighlightsController.HighlightType h) => h.Id == highlightId);
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x00083BFC File Offset: 0x00081DFC
		private void SaveVideo(string highlightID, string groupID, int startDelta, int endDelta)
		{
			Highlights.SaveVideo(highlightID, groupID, startDelta, endDelta);
			if (!this._savedHighlightGroups.Contains(groupID))
			{
				this._savedHighlightGroups.Add(groupID);
			}
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x00083C24 File Offset: 0x00081E24
		public override void AfterStart()
		{
			if (!HighlightsController.IsHighlightsInitialized)
			{
				HighlightsController.HighlightTypes = new List<HighlightsController.HighlightType>
				{
					new HighlightsController.HighlightType("hlid_killing_spree", "Killing Spree", "grpid_incidents", -2010, 3000, 0.25f, float.MaxValue, true),
					new HighlightsController.HighlightType("hlid_high_ranged_shot_difficulty", "Sharpshooter", "grpid_incidents", -5000, 3000, 0.25f, float.MaxValue, true),
					new HighlightsController.HighlightType("hlid_archer_salvo_kills", "Death from Above", "grpid_incidents", -5004, 3000, 0.5f, 150f, false),
					new HighlightsController.HighlightType("hlid_couched_lance_against_mounted_opponent", "Lance A Lot", "grpid_incidents", -5000, 3000, 0.25f, float.MaxValue, true),
					new HighlightsController.HighlightType("hlid_cavalry_charge_first_impact", "Cavalry Charge First Impact", "grpid_incidents", -5000, 5000, 0.25f, float.MaxValue, false),
					new HighlightsController.HighlightType("hlid_headshot_kill", "Headshot!", "grpid_incidents", -5000, 3000, 0.25f, 150f, true),
					new HighlightsController.HighlightType("hlid_burning_ammunition_kill", "Burn Baby", "grpid_incidents", -5000, 3000, 0.25f, 100f, true),
					new HighlightsController.HighlightType("hlid_throwing_weapon_kill_against_charging_enemy", "Throwing Weapon Kill Against Charging Enemy", "grpid_incidents", -5000, 3000, 0.25f, 150f, true)
				};
				Highlights.Initialize();
				foreach (HighlightsController.HighlightType highlightType in HighlightsController.HighlightTypes)
				{
					Highlights.AddHighlight(highlightType.Id, highlightType.Description);
				}
				HighlightsController.IsHighlightsInitialized = true;
			}
			foreach (string text in this._highlightGroupIds)
			{
				Highlights.OpenGroup(text);
			}
			this._highlightSaveQueue = new List<HighlightsController.Highlight>();
			this._playerKillTimes = new List<float>();
			this._archerSalvoKillTimes = new List<float>();
			this._cavalryChargeHitTimes = new List<float>();
			this._savedHighlightGroups = new List<string>();
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x00083E90 File Offset: 0x00082090
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			if (affectorAgent != null && affectedAgent != null && affectorAgent.IsHuman && affectedAgent.IsHuman && (agentState == AgentState.Killed || agentState == AgentState.Unconscious))
			{
				bool flag = affectorAgent.Team != null && affectorAgent.Team.IsPlayerTeam;
				bool isMainAgent = affectorAgent.IsMainAgent;
				if ((((isMainAgent || flag) && !affectedAgent.Team.IsPlayerAlly && killingBlow.WeaponClass == 12) || killingBlow.WeaponClass == 13) && this.CanSaveHighlight(this.GetHighlightTypeWithId("hlid_archer_salvo_kills"), affectedAgent.Position))
				{
					if (!this._isArcherSalvoHappening)
					{
						this._archerSalvoKillTimes.RemoveAll((float ht) => ht + 4f < Mission.Current.CurrentTime);
					}
					this._archerSalvoKillTimes.Add(Mission.Current.CurrentTime);
					if (this._archerSalvoKillTimes.Count >= 5)
					{
						this._isArcherSalvoHappening = true;
					}
				}
				if (isMainAgent && this.CanSaveHighlight(this.GetHighlightTypeWithId("hlid_killing_spree"), affectedAgent.Position))
				{
					if (!this._isKillingSpreeHappening)
					{
						this._playerKillTimes.RemoveAll((float ht) => ht + 10f < Mission.Current.CurrentTime);
					}
					this._playerKillTimes.Add(Mission.Current.CurrentTime);
					if (this._playerKillTimes.Count >= 4)
					{
						this._isKillingSpreeHappening = true;
					}
				}
				HighlightsController.Highlight highlight = default(HighlightsController.Highlight);
				highlight.Start = Mission.Current.CurrentTime;
				highlight.End = Mission.Current.CurrentTime;
				bool flag2 = false;
				if (isMainAgent && killingBlow.WeaponRecordWeaponFlags.HasAllFlags(WeaponFlags.Burning))
				{
					highlight.HighlightType = this.GetHighlightTypeWithId("hlid_burning_ammunition_kill");
					flag2 = true;
				}
				if (isMainAgent && killingBlow.IsMissile && killingBlow.IsHeadShot())
				{
					highlight.HighlightType = this.GetHighlightTypeWithId("hlid_headshot_kill");
					flag2 = true;
				}
				if (isMainAgent && killingBlow.IsMissile && affectedAgent.HasMount && affectedAgent.IsDoingPassiveAttack && (killingBlow.WeaponClass == 21 || killingBlow.WeaponClass == 22))
				{
					highlight.HighlightType = this.GetHighlightTypeWithId("hlid_throwing_weapon_kill_against_charging_enemy");
					flag2 = true;
				}
				if (this._isFirstImpact && affectorAgent.Formation != null && affectorAgent.Formation.PhysicalClass.IsMeleeCavalry() && affectorAgent.Formation.GetReadonlyMovementOrderReference() == MovementOrder.MovementOrderCharge && this.CanSaveHighlight(this.GetHighlightTypeWithId("hlid_cavalry_charge_first_impact"), affectedAgent.Position))
				{
					this._cavalryChargeHitTimes.RemoveAll((float ht) => ht + 3f < Mission.Current.CurrentTime);
					this._cavalryChargeHitTimes.Add(Mission.Current.CurrentTime);
					if (this._cavalryChargeHitTimes.Count >= 5)
					{
						highlight.HighlightType = this.GetHighlightTypeWithId("hlid_cavalry_charge_first_impact");
						highlight.Start = this._cavalryChargeHitTimes[0];
						highlight.End = this._cavalryChargeHitTimes[this._cavalryChargeHitTimes.Count - 1];
						flag2 = true;
						this._isFirstImpact = false;
						this._cavalryChargeHitTimes.Clear();
					}
				}
				if (flag2)
				{
					this.SaveHighlight(highlight, affectedAgent.Position);
				}
			}
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x000841DC File Offset: 0x000823DC
		public override void OnScoreHit(Agent affectedAgent, Agent affectorAgent, WeaponComponentData attackerWeapon, bool isBlocked, bool isSiegeEngineHit, in Blow blow, in AttackCollisionData collisionData, float damagedHp, float hitDistance, float shotDifficulty)
		{
			if (affectorAgent != null && affectedAgent != null && affectorAgent.IsHuman && affectedAgent.IsHuman)
			{
				bool isMainAgent = affectorAgent.IsMainAgent;
				HighlightsController.Highlight highlight = default(HighlightsController.Highlight);
				highlight.Start = Mission.Current.CurrentTime;
				highlight.End = Mission.Current.CurrentTime;
				bool flag = false;
				if (isMainAgent && shotDifficulty >= 7.5f)
				{
					highlight.HighlightType = this.GetHighlightTypeWithId("hlid_high_ranged_shot_difficulty");
					flag = true;
				}
				if (isMainAgent && affectedAgent.HasMount && blow.AttackType == AgentAttackType.Standard && affectorAgent.HasMount && affectorAgent.IsDoingPassiveAttack)
				{
					highlight.HighlightType = this.GetHighlightTypeWithId("hlid_couched_lance_against_mounted_opponent");
					flag = true;
				}
				if (this._isFirstImpact && affectorAgent.Formation != null && affectorAgent.Formation.PhysicalClass.IsMeleeCavalry() && affectorAgent.Formation.GetReadonlyMovementOrderReference() == MovementOrder.MovementOrderCharge && this.CanSaveHighlight(this.GetHighlightTypeWithId("hlid_cavalry_charge_first_impact"), affectedAgent.Position))
				{
					this._cavalryChargeHitTimes.RemoveAll((float ht) => ht + 3f < Mission.Current.CurrentTime);
					this._cavalryChargeHitTimes.Add(Mission.Current.CurrentTime);
					if (this._cavalryChargeHitTimes.Count >= 5)
					{
						highlight.HighlightType = this.GetHighlightTypeWithId("hlid_cavalry_charge_first_impact");
						highlight.Start = this._cavalryChargeHitTimes[0];
						highlight.End = this._cavalryChargeHitTimes[this._cavalryChargeHitTimes.Count - 1];
						flag = true;
						this._isFirstImpact = false;
						this._cavalryChargeHitTimes.Clear();
					}
				}
				if (flag)
				{
					this.SaveHighlight(highlight, affectedAgent.Position);
				}
			}
		}

		// Token: 0x06002487 RID: 9351 RVA: 0x000843AC File Offset: 0x000825AC
		public override void OnMissionTick(float dt)
		{
			if (this._isArcherSalvoHappening && this._archerSalvoKillTimes[0] + 4f < Mission.Current.CurrentTime)
			{
				HighlightsController.Highlight highlight;
				highlight.HighlightType = this.GetHighlightTypeWithId("hlid_archer_salvo_kills");
				highlight.Start = this._archerSalvoKillTimes[0];
				highlight.End = this._archerSalvoKillTimes[this._archerSalvoKillTimes.Count - 1];
				this.SaveHighlight(highlight);
				this._isArcherSalvoHappening = false;
				this._archerSalvoKillTimes.Clear();
			}
			if (this._isKillingSpreeHappening && this._playerKillTimes[0] + 10f < Mission.Current.CurrentTime)
			{
				HighlightsController.Highlight highlight2;
				highlight2.HighlightType = this.GetHighlightTypeWithId("hlid_killing_spree");
				highlight2.Start = this._playerKillTimes[0];
				highlight2.End = this._playerKillTimes[this._playerKillTimes.Count - 1];
				this.SaveHighlight(highlight2);
				this._isKillingSpreeHappening = false;
				this._playerKillTimes.Clear();
			}
			this.TickHighlightsToBeSaved();
		}

		// Token: 0x06002488 RID: 9352 RVA: 0x000844C8 File Offset: 0x000826C8
		protected override void OnEndMission()
		{
			base.OnEndMission();
			foreach (string text in this._highlightGroupIds)
			{
				Highlights.CloseGroup(text, false);
			}
			this._highlightSaveQueue = null;
			this._lastSavedHighlightData = null;
			this._playerKillTimes = null;
			this._archerSalvoKillTimes = null;
			this._cavalryChargeHitTimes = null;
		}

		// Token: 0x06002489 RID: 9353 RVA: 0x00084544 File Offset: 0x00082744
		public static void AddHighlightType(HighlightsController.HighlightType highlightType)
		{
			if (!HighlightsController.HighlightTypes.Any<HighlightsController.HighlightType>((HighlightsController.HighlightType h) => h.Id == highlightType.Id))
			{
				if (HighlightsController.IsHighlightsInitialized)
				{
					Highlights.AddHighlight(highlightType.Id, highlightType.Description);
				}
				HighlightsController.HighlightTypes.Add(highlightType);
			}
		}

		// Token: 0x0600248A RID: 9354 RVA: 0x000845A8 File Offset: 0x000827A8
		public void SaveHighlight(HighlightsController.Highlight highlight)
		{
			this._highlightSaveQueue.Add(highlight);
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x000845B6 File Offset: 0x000827B6
		public void SaveHighlight(HighlightsController.Highlight highlight, Vec3 position)
		{
			if (this.CanSaveHighlight(highlight.HighlightType, position))
			{
				this._highlightSaveQueue.Add(highlight);
			}
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x000845D4 File Offset: 0x000827D4
		public bool CanSaveHighlight(HighlightsController.HighlightType highlightType, Vec3 position)
		{
			return highlightType.MaxHighlightDistance >= Mission.Current.Scene.LastFinalRenderCameraFrame.origin.Distance(position) && highlightType.MinVisibilityScore <= this.GetPlayerIsLookingAtPositionScore(position) && (!highlightType.IsVisibilityRequired || this.CanSeePosition(position));
		}

		// Token: 0x0600248D RID: 9357 RVA: 0x0008462C File Offset: 0x0008282C
		public float GetPlayerIsLookingAtPositionScore(Vec3 position)
		{
			Vec3 vec = -Mission.Current.Scene.LastFinalRenderCameraFrame.rotation.u;
			Vec3 origin = Mission.Current.Scene.LastFinalRenderCameraFrame.origin;
			return MathF.Max(Vec3.DotProduct(vec.NormalizedCopy(), (position - origin).NormalizedCopy()), 0f);
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x00084694 File Offset: 0x00082894
		public bool CanSeePosition(Vec3 position)
		{
			Vec3 origin = Mission.Current.Scene.LastFinalRenderCameraFrame.origin;
			float num;
			return !Mission.Current.Scene.RayCastForClosestEntityOrTerrain(origin, position, out num, 0.01f, BodyFlags.CameraCollisionRayCastExludeFlags) || MathF.Abs(position.Distance(origin) - num) < 0.1f;
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x000846ED File Offset: 0x000828ED
		public void ShowSummary()
		{
			if (this.IsAnyHighlightSaved)
			{
				Highlights.OpenSummary(this._savedHighlightGroups);
			}
		}

		// Token: 0x06002490 RID: 9360 RVA: 0x00084704 File Offset: 0x00082904
		private void TickHighlightsToBeSaved()
		{
			if (this._highlightSaveQueue != null)
			{
				if (this._lastSavedHighlightData != null && this._highlightSaveQueue.Count > 0)
				{
					float item = this._lastSavedHighlightData.Item1;
					float item2 = this._lastSavedHighlightData.Item2;
					float num = item2 - (item2 - item) * 0.5f;
					for (int i = 0; i < this._highlightSaveQueue.Count; i++)
					{
						float start = this._highlightSaveQueue[i].Start;
						HighlightsController.Highlight highlight = this._highlightSaveQueue[i];
						if (start - (float)highlight.HighlightType.StartDelta < num)
						{
							this._highlightSaveQueue.Remove(this._highlightSaveQueue[i]);
							i--;
						}
					}
				}
				if (this._highlightSaveQueue.Count > 0)
				{
					float start2 = this._highlightSaveQueue[0].Start;
					HighlightsController.Highlight highlight = this._highlightSaveQueue[0];
					float num2 = start2 + (float)(highlight.HighlightType.StartDelta / 1000);
					float end = this._highlightSaveQueue[0].End;
					highlight = this._highlightSaveQueue[0];
					float num3 = end + (float)(highlight.HighlightType.EndDelta / 1000);
					for (int j = 1; j < this._highlightSaveQueue.Count; j++)
					{
						float start3 = this._highlightSaveQueue[j].Start;
						highlight = this._highlightSaveQueue[j];
						float num4 = start3 + (float)(highlight.HighlightType.StartDelta / 1000);
						float end2 = this._highlightSaveQueue[j].End;
						highlight = this._highlightSaveQueue[j];
						float num5 = end2 + (float)(highlight.HighlightType.EndDelta / 1000);
						if (num4 < num2)
						{
							num2 = num4;
						}
						if (num5 > num3)
						{
							num3 = num5;
						}
					}
					highlight = this._highlightSaveQueue[0];
					string id = highlight.HighlightType.Id;
					highlight = this._highlightSaveQueue[0];
					this.SaveVideo(id, highlight.HighlightType.GroupId, (int)(num2 - Mission.Current.CurrentTime) * 1000, (int)(num3 - Mission.Current.CurrentTime) * 1000);
					this._lastSavedHighlightData = new Tuple<float, float>(num2, num3);
					this._highlightSaveQueue.Clear();
				}
			}
		}

		// Token: 0x04000E03 RID: 3587
		private bool _isKillingSpreeHappening;

		// Token: 0x04000E04 RID: 3588
		private List<float> _playerKillTimes;

		// Token: 0x04000E05 RID: 3589
		private const int MinKillingSpreeKills = 4;

		// Token: 0x04000E06 RID: 3590
		private const float MaxKillingSpreeDuration = 10f;

		// Token: 0x04000E07 RID: 3591
		private const float HighShotDifficultyThreshold = 7.5f;

		// Token: 0x04000E08 RID: 3592
		private bool _isArcherSalvoHappening;

		// Token: 0x04000E09 RID: 3593
		private List<float> _archerSalvoKillTimes;

		// Token: 0x04000E0A RID: 3594
		private const int MinArcherSalvoKills = 5;

		// Token: 0x04000E0B RID: 3595
		private const float MaxArcherSalvoDuration = 4f;

		// Token: 0x04000E0C RID: 3596
		private bool _isFirstImpact = true;

		// Token: 0x04000E0D RID: 3597
		private List<float> _cavalryChargeHitTimes;

		// Token: 0x04000E0E RID: 3598
		private const float CavalryChargeImpactTimeFrame = 3f;

		// Token: 0x04000E0F RID: 3599
		private const int MinCavalryChargeHits = 5;

		// Token: 0x04000E10 RID: 3600
		private Tuple<float, float> _lastSavedHighlightData;

		// Token: 0x04000E11 RID: 3601
		private List<HighlightsController.Highlight> _highlightSaveQueue;

		// Token: 0x04000E12 RID: 3602
		private const float IgnoreIfOverlapsLastVideoPercent = 0.5f;

		// Token: 0x04000E13 RID: 3603
		private List<string> _savedHighlightGroups;

		// Token: 0x04000E14 RID: 3604
		private List<string> _highlightGroupIds = new List<string> { "grpid_incidents", "grpid_achievements" };

		// Token: 0x02000567 RID: 1383
		public struct HighlightType
		{
			// Token: 0x17000A85 RID: 2693
			// (get) Token: 0x06003DD0 RID: 15824 RVA: 0x000F6D4B File Offset: 0x000F4F4B
			// (set) Token: 0x06003DD1 RID: 15825 RVA: 0x000F6D53 File Offset: 0x000F4F53
			public string Id { get; private set; }

			// Token: 0x17000A86 RID: 2694
			// (get) Token: 0x06003DD2 RID: 15826 RVA: 0x000F6D5C File Offset: 0x000F4F5C
			// (set) Token: 0x06003DD3 RID: 15827 RVA: 0x000F6D64 File Offset: 0x000F4F64
			public string Description { get; private set; }

			// Token: 0x17000A87 RID: 2695
			// (get) Token: 0x06003DD4 RID: 15828 RVA: 0x000F6D6D File Offset: 0x000F4F6D
			// (set) Token: 0x06003DD5 RID: 15829 RVA: 0x000F6D75 File Offset: 0x000F4F75
			public string GroupId { get; private set; }

			// Token: 0x17000A88 RID: 2696
			// (get) Token: 0x06003DD6 RID: 15830 RVA: 0x000F6D7E File Offset: 0x000F4F7E
			// (set) Token: 0x06003DD7 RID: 15831 RVA: 0x000F6D86 File Offset: 0x000F4F86
			public int StartDelta { get; private set; }

			// Token: 0x17000A89 RID: 2697
			// (get) Token: 0x06003DD8 RID: 15832 RVA: 0x000F6D8F File Offset: 0x000F4F8F
			// (set) Token: 0x06003DD9 RID: 15833 RVA: 0x000F6D97 File Offset: 0x000F4F97
			public int EndDelta { get; private set; }

			// Token: 0x17000A8A RID: 2698
			// (get) Token: 0x06003DDA RID: 15834 RVA: 0x000F6DA0 File Offset: 0x000F4FA0
			// (set) Token: 0x06003DDB RID: 15835 RVA: 0x000F6DA8 File Offset: 0x000F4FA8
			public float MinVisibilityScore { get; private set; }

			// Token: 0x17000A8B RID: 2699
			// (get) Token: 0x06003DDC RID: 15836 RVA: 0x000F6DB1 File Offset: 0x000F4FB1
			// (set) Token: 0x06003DDD RID: 15837 RVA: 0x000F6DB9 File Offset: 0x000F4FB9
			public float MaxHighlightDistance { get; private set; }

			// Token: 0x17000A8C RID: 2700
			// (get) Token: 0x06003DDE RID: 15838 RVA: 0x000F6DC2 File Offset: 0x000F4FC2
			// (set) Token: 0x06003DDF RID: 15839 RVA: 0x000F6DCA File Offset: 0x000F4FCA
			public bool IsVisibilityRequired { get; private set; }

			// Token: 0x06003DE0 RID: 15840 RVA: 0x000F6DD3 File Offset: 0x000F4FD3
			public HighlightType(string id, string description, string groupId, int startDelta, int endDelta, float minVisibilityScore, float maxHighlightDistance, bool isVisibilityRequired)
			{
				this.Id = id;
				this.Description = description;
				this.GroupId = groupId;
				this.StartDelta = startDelta;
				this.EndDelta = endDelta;
				this.MinVisibilityScore = minVisibilityScore;
				this.MaxHighlightDistance = maxHighlightDistance;
				this.IsVisibilityRequired = isVisibilityRequired;
			}
		}

		// Token: 0x02000568 RID: 1384
		public struct Highlight
		{
			// Token: 0x04001E83 RID: 7811
			public HighlightsController.HighlightType HighlightType;

			// Token: 0x04001E84 RID: 7812
			public float Start;

			// Token: 0x04001E85 RID: 7813
			public float End;
		}
	}
}
