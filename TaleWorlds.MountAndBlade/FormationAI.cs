using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000140 RID: 320
	public class FormationAI
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000F5C RID: 3932 RVA: 0x00029404 File Offset: 0x00027604
		// (remove) Token: 0x06000F5D RID: 3933 RVA: 0x0002943C File Offset: 0x0002763C
		public event Action<Formation> OnActiveBehaviorChanged;

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000F5E RID: 3934 RVA: 0x00029471 File Offset: 0x00027671
		// (set) Token: 0x06000F5F RID: 3935 RVA: 0x0002947C File Offset: 0x0002767C
		public BehaviorComponent ActiveBehavior
		{
			get
			{
				return this._activeBehavior;
			}
			private set
			{
				if (this._activeBehavior != value)
				{
					BehaviorComponent activeBehavior = this._activeBehavior;
					if (activeBehavior != null)
					{
						activeBehavior.OnBehaviorCanceled();
					}
					BehaviorComponent activeBehavior2 = this._activeBehavior;
					this._activeBehavior = value;
					this._activeBehavior.OnBehaviorActivated();
					this.ActiveBehavior.PreserveExpireTime = Mission.Current.CurrentTime + 10f;
					if (this.OnActiveBehaviorChanged != null && (activeBehavior2 == null || !activeBehavior2.Equals(value)))
					{
						this.OnActiveBehaviorChanged(this._formation);
					}
				}
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000F60 RID: 3936 RVA: 0x000294FC File Offset: 0x000276FC
		// (set) Token: 0x06000F61 RID: 3937 RVA: 0x00029504 File Offset: 0x00027704
		public FormationAI.BehaviorSide Side
		{
			get
			{
				return this._side;
			}
			set
			{
				if (this._side != value)
				{
					this._side = value;
					if (this._side != FormationAI.BehaviorSide.BehaviorSideNotSet)
					{
						foreach (BehaviorComponent behaviorComponent in this._behaviors)
						{
							behaviorComponent.OnValidBehaviorSideChanged();
						}
					}
				}
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000F62 RID: 3938 RVA: 0x00029570 File Offset: 0x00027770
		// (set) Token: 0x06000F63 RID: 3939 RVA: 0x00029578 File Offset: 0x00027778
		public bool IsMainFormation { get; set; }

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000F64 RID: 3940 RVA: 0x00029581 File Offset: 0x00027781
		public int BehaviorCount
		{
			get
			{
				return this._behaviors.Count;
			}
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x00029590 File Offset: 0x00027790
		public FormationAI(Formation formation)
		{
			this._formation = formation;
			float num = 0f;
			if (formation.Team != null)
			{
				float num2 = 0.1f * (float)formation.FormationIndex;
				float num3 = 0f;
				if (formation.Team.TeamIndex >= 0)
				{
					num3 = (float)formation.Team.TeamIndex * 0.5f * 0.1f;
				}
				num = num2 + num3;
			}
			this._tickTimer = new Timer(Mission.Current.CurrentTime + 0.5f * num, 0.5f, true);
			this._specialBehaviorData = new List<FormationAI.BehaviorData>();
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x00029638 File Offset: 0x00027838
		public T SetBehaviorWeight<T>(float w) where T : BehaviorComponent
		{
			using (List<BehaviorComponent>.Enumerator enumerator = this._behaviors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T t;
					if ((t = enumerator.Current as T) != null)
					{
						t.WeightFactor = w;
						return t;
					}
				}
			}
			throw new MBException("Behavior weight could not be set.");
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x000296B4 File Offset: 0x000278B4
		public void AddAiBehavior(BehaviorComponent behaviorComponent)
		{
			this._behaviors.Add(behaviorComponent);
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x000296C4 File Offset: 0x000278C4
		public T GetBehavior<T>() where T : BehaviorComponent
		{
			using (List<BehaviorComponent>.Enumerator enumerator = this._behaviors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T t;
					if ((t = enumerator.Current as T) != null)
					{
						return t;
					}
				}
			}
			using (List<FormationAI.BehaviorData>.Enumerator enumerator2 = this._specialBehaviorData.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					T t2;
					if ((t2 = enumerator2.Current.Behavior as T) != null)
					{
						return t2;
					}
				}
			}
			return default(T);
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x0002978C File Offset: 0x0002798C
		public void AddSpecialBehavior(BehaviorComponent behavior, bool purgePreviousSpecialBehaviors = false)
		{
			if (purgePreviousSpecialBehaviors)
			{
				this._specialBehaviorData.Clear();
			}
			this._specialBehaviorData.Add(new FormationAI.BehaviorData
			{
				Behavior = behavior
			});
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x000297B4 File Offset: 0x000279B4
		private bool FindBestBehavior()
		{
			BehaviorComponent behaviorComponent = null;
			float num = float.MinValue;
			foreach (BehaviorComponent behaviorComponent2 in this._behaviors)
			{
				if (behaviorComponent2.WeightFactor > 1E-07f)
				{
					float num2 = behaviorComponent2.GetAIWeight() * behaviorComponent2.WeightFactor;
					if (behaviorComponent2 == this.ActiveBehavior)
					{
						num2 *= MBMath.Lerp(1.2f, 2f, MBMath.ClampFloat((behaviorComponent2.PreserveExpireTime - Mission.Current.CurrentTime) / 5f, 0f, 1f), float.MinValue);
					}
					if (num2 > num)
					{
						if (behaviorComponent2.NavmeshlessTargetPositionPenalty > 0f)
						{
							num2 /= behaviorComponent2.NavmeshlessTargetPositionPenalty;
						}
						behaviorComponent2.PrecalculateMovementOrder();
						num2 *= behaviorComponent2.NavmeshlessTargetPositionPenalty;
						if (num2 > num)
						{
							behaviorComponent = behaviorComponent2;
							num = num2;
						}
					}
				}
			}
			if (behaviorComponent != null)
			{
				this.ActiveBehavior = behaviorComponent;
				if (behaviorComponent != this._behaviors[0])
				{
					this._behaviors.Remove(behaviorComponent);
					this._behaviors.Insert(0, behaviorComponent);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x000298E4 File Offset: 0x00027AE4
		private void PreprocessBehaviors()
		{
			if (this._formation.HasAnyEnemyFormationsThatIsNotEmpty())
			{
				FormationAI.BehaviorData behaviorData = this._specialBehaviorData.FirstOrDefault<FormationAI.BehaviorData>((FormationAI.BehaviorData sd) => !sd.IsPreprocessed);
				if (behaviorData != null)
				{
					behaviorData.Behavior.TickOccasionally();
					float num = behaviorData.Behavior.GetAIWeight();
					if (behaviorData.Behavior == this.ActiveBehavior)
					{
						num *= MBMath.Lerp(1.01f, 1.5f, MBMath.ClampFloat((behaviorData.Behavior.PreserveExpireTime - Mission.Current.CurrentTime) / 5f, 0f, 1f), float.MinValue);
					}
					behaviorData.Weight = num * behaviorData.Preference;
					behaviorData.IsPreprocessed = true;
				}
			}
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x000299AC File Offset: 0x00027BAC
		public void Tick()
		{
			if (Mission.Current.AllowAiTicking && (Mission.Current.ForceTickOccasionally || this._tickTimer.Check(Mission.Current.CurrentTime)))
			{
				this.TickOccasionally(this._tickTimer.PreviousDeltaTime);
			}
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x000299FC File Offset: 0x00027BFC
		private void TickOccasionally(float dt)
		{
			this._formation.IsAITickedAfterSplit = true;
			if (this.FindBestBehavior())
			{
				if (!this._formation.IsAIControlled)
				{
					if (GameNetwork.IsMultiplayer && Mission.Current.MainAgent != null && !this._formation.Team.IsPlayerGeneral && this._formation.Team.IsPlayerSergeant && this._formation.PlayerOwner == Agent.Main)
					{
						this.ActiveBehavior.RemindSergeantPlayer();
						return;
					}
				}
				else
				{
					this.ActiveBehavior.TickOccasionally();
				}
				return;
			}
			BehaviorComponent behaviorComponent = this.ActiveBehavior;
			if (this._formation.HasAnyEnemyFormationsThatIsNotEmpty())
			{
				this.PreprocessBehaviors();
				foreach (FormationAI.BehaviorData behaviorData in this._specialBehaviorData)
				{
					behaviorData.IsPreprocessed = false;
				}
				if (behaviorComponent is BehaviorStop && this._specialBehaviorData.Count > 0)
				{
					IEnumerable<FormationAI.BehaviorData> enumerable = this._specialBehaviorData.Where<FormationAI.BehaviorData>((FormationAI.BehaviorData sbd) => sbd.Weight > 0f);
					if (enumerable.Any<FormationAI.BehaviorData>())
					{
						behaviorComponent = enumerable.MaxBy<FormationAI.BehaviorData, float>((FormationAI.BehaviorData abd) => abd.Weight).Behavior;
					}
				}
				bool isAIControlled = this._formation.IsAIControlled;
				bool flag = false;
				if (this.ActiveBehavior != behaviorComponent)
				{
					BehaviorComponent activeBehavior = this.ActiveBehavior;
					this.ActiveBehavior = behaviorComponent;
					flag = true;
				}
				if (flag || (behaviorComponent != null && behaviorComponent.IsCurrentOrderChanged))
				{
					if (this._formation.IsAIControlled)
					{
						this._formation.SetMovementOrder(behaviorComponent.CurrentOrder);
					}
					behaviorComponent.IsCurrentOrderChanged = false;
				}
			}
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x00029BBC File Offset: 0x00027DBC
		public void OnDeploymentFinished()
		{
			foreach (BehaviorComponent behaviorComponent in this._behaviors)
			{
				behaviorComponent.OnDeploymentFinished();
			}
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x00029C0C File Offset: 0x00027E0C
		public void OnAgentRemoved(Agent agent)
		{
			foreach (BehaviorComponent behaviorComponent in this._behaviors)
			{
				behaviorComponent.OnAgentRemoved(agent);
			}
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x00029C60 File Offset: 0x00027E60
		public BehaviorComponent GetBehaviorAtIndex(int index)
		{
			if (index >= 0 && index < this._behaviors.Count)
			{
				return this._behaviors[index];
			}
			return null;
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x00029C84 File Offset: 0x00027E84
		[Conditional("DEBUG")]
		public void DebugMore()
		{
			if (!MBDebug.IsDisplayingHighLevelAI)
			{
				return;
			}
			foreach (FormationAI.BehaviorData behaviorData in this._specialBehaviorData.OrderBy<FormationAI.BehaviorData, string>((FormationAI.BehaviorData d) => d.Behavior.GetType().ToString()))
			{
				behaviorData.Behavior.GetType().ToString().Replace("MBModule.Behavior", "");
				behaviorData.Weight.ToString("0.00");
				behaviorData.Preference.ToString("0.00");
			}
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x00029D38 File Offset: 0x00027F38
		[Conditional("DEBUG")]
		public void DebugScores()
		{
			if (this._formation.PhysicalClass.IsRanged())
			{
				MBDebug.Print("Ranged", 0, Debug.DebugColor.White, 17592186044416UL);
			}
			else if (this._formation.PhysicalClass.IsMeleeCavalry())
			{
				MBDebug.Print("Cavalry", 0, Debug.DebugColor.White, 17592186044416UL);
			}
			else
			{
				MBDebug.Print("Infantry", 0, Debug.DebugColor.White, 17592186044416UL);
			}
			foreach (FormationAI.BehaviorData behaviorData in this._specialBehaviorData.OrderBy<FormationAI.BehaviorData, string>((FormationAI.BehaviorData d) => d.Behavior.GetType().ToString()))
			{
				string text = behaviorData.Behavior.GetType().ToString().Replace("MBModule.Behavior", "");
				string text2 = behaviorData.Weight.ToString("0.00");
				string text3 = behaviorData.Preference.ToString("0.00");
				MBDebug.Print(string.Concat(new string[] { text, " \t\t w:", text2, "\t p:", text3 }), 0, Debug.DebugColor.White, 17592186044416UL);
			}
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x00029E88 File Offset: 0x00028088
		public void ResetBehaviorWeights()
		{
			foreach (BehaviorComponent behaviorComponent in this._behaviors)
			{
				behaviorComponent.ResetBehavior();
			}
		}

		// Token: 0x040003C9 RID: 969
		private const float BehaviorPreserveTime = 5f;

		// Token: 0x040003CB RID: 971
		private readonly Formation _formation;

		// Token: 0x040003CC RID: 972
		private readonly List<FormationAI.BehaviorData> _specialBehaviorData;

		// Token: 0x040003CD RID: 973
		private readonly List<BehaviorComponent> _behaviors = new List<BehaviorComponent>();

		// Token: 0x040003CE RID: 974
		private BehaviorComponent _activeBehavior;

		// Token: 0x040003CF RID: 975
		private FormationAI.BehaviorSide _side = FormationAI.BehaviorSide.Middle;

		// Token: 0x040003D0 RID: 976
		private readonly Timer _tickTimer;

		// Token: 0x0200045D RID: 1117
		public class BehaviorData
		{
			// Token: 0x04001A36 RID: 6710
			public BehaviorComponent Behavior;

			// Token: 0x04001A37 RID: 6711
			public float Preference = 1f;

			// Token: 0x04001A38 RID: 6712
			public float Weight;

			// Token: 0x04001A39 RID: 6713
			public bool IsRemovedOnCancel;

			// Token: 0x04001A3A RID: 6714
			public bool IsPreprocessed;
		}

		// Token: 0x0200045E RID: 1118
		public enum BehaviorSide
		{
			// Token: 0x04001A3C RID: 6716
			Left,
			// Token: 0x04001A3D RID: 6717
			Middle,
			// Token: 0x04001A3E RID: 6718
			Right,
			// Token: 0x04001A3F RID: 6719
			BehaviorSideNotSet,
			// Token: 0x04001A40 RID: 6720
			ValidBehaviorSideCount = 3
		}
	}
}
