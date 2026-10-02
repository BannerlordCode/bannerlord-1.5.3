using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000313 RID: 787
	public class MPConditionalEffect
	{
		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06002D47 RID: 11591 RVA: 0x000AEB86 File Offset: 0x000ACD86
		public MBReadOnlyList<MPPerkCondition> Conditions { get; }

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06002D48 RID: 11592 RVA: 0x000AEB8E File Offset: 0x000ACD8E
		public MBReadOnlyList<MPPerkEffectBase> Effects { get; }

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06002D49 RID: 11593 RVA: 0x000AEB98 File Offset: 0x000ACD98
		public MPPerkCondition.PerkEventFlags EventFlags
		{
			get
			{
				MPPerkCondition.PerkEventFlags perkEventFlags = MPPerkCondition.PerkEventFlags.None;
				foreach (MPPerkCondition mpperkCondition in this.Conditions)
				{
					perkEventFlags |= mpperkCondition.EventFlags;
				}
				return perkEventFlags;
			}
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06002D4A RID: 11594 RVA: 0x000AEBF0 File Offset: 0x000ACDF0
		public bool IsTickRequired
		{
			get
			{
				using (List<MPPerkEffectBase>.Enumerator enumerator = this.Effects.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.IsTickRequired)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06002D4B RID: 11595 RVA: 0x000AEC4C File Offset: 0x000ACE4C
		public MPConditionalEffect(List<string> gameModes, XmlNode node)
		{
			MBList<MPPerkCondition> mblist = new MBList<MPPerkCondition>();
			MBList<MPPerkEffectBase> mblist2 = new MBList<MPPerkEffectBase>();
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "Conditions")
				{
					using (IEnumerator enumerator2 = xmlNode.ChildNodes.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							XmlNode xmlNode2 = (XmlNode)obj2;
							if (xmlNode2.NodeType == XmlNodeType.Element)
							{
								mblist.Add(MPPerkCondition.CreateFrom(gameModes, xmlNode2));
							}
						}
						continue;
					}
				}
				if (xmlNode.Name == "Effects")
				{
					foreach (object obj3 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode3 = (XmlNode)obj3;
						if (xmlNode3.NodeType == XmlNodeType.Element)
						{
							MPPerkEffect mpperkEffect = MPPerkEffect.CreateFrom(xmlNode3);
							mblist2.Add(mpperkEffect);
						}
					}
				}
			}
			this.Conditions = mblist;
			this.Effects = mblist2;
		}

		// Token: 0x06002D4C RID: 11596 RVA: 0x000AEDB0 File Offset: 0x000ACFB0
		public bool Check(MissionPeer peer)
		{
			using (List<MPPerkCondition>.Enumerator enumerator = this.Conditions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Check(peer))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06002D4D RID: 11597 RVA: 0x000AEE0C File Offset: 0x000AD00C
		public bool Check(Agent agent)
		{
			using (List<MPPerkCondition>.Enumerator enumerator = this.Conditions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Check(agent))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06002D4E RID: 11598 RVA: 0x000AEE68 File Offset: 0x000AD068
		public void OnEvent(bool isWarmup, MissionPeer peer, MPConditionalEffect.ConditionalEffectContainer container)
		{
			if (MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) <= 0)
			{
				if (peer == null)
				{
					return;
				}
				Agent controlledAgent = peer.ControlledAgent;
				bool? flag = ((controlledAgent != null) ? new bool?(controlledAgent.IsActive()) : null);
				bool flag2 = true;
				if (!((flag.GetValueOrDefault() == flag2) & (flag != null)))
				{
					return;
				}
			}
			bool flag3 = true;
			foreach (MPPerkCondition mpperkCondition in this.Conditions)
			{
				if (mpperkCondition.IsPeerCondition && !mpperkCondition.Check(peer))
				{
					flag3 = false;
					break;
				}
			}
			if (!flag3)
			{
				if (MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) > 0)
				{
					MBReadOnlyList<IFormationUnit> mbreadOnlyList;
					if (peer == null)
					{
						mbreadOnlyList = null;
					}
					else
					{
						Formation controlledFormation = peer.ControlledFormation;
						mbreadOnlyList = ((controlledFormation != null) ? controlledFormation.Arrangement.GetAllUnits() : null);
					}
					MBReadOnlyList<IFormationUnit> mbreadOnlyList2 = mbreadOnlyList;
					if (mbreadOnlyList2 == null)
					{
						return;
					}
					using (List<IFormationUnit>.Enumerator enumerator2 = mbreadOnlyList2.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Agent agent;
							if ((agent = enumerator2.Current as Agent) != null && agent.IsActive())
							{
								this.UpdateAgentState(isWarmup, container, agent, false);
							}
						}
						return;
					}
				}
				this.UpdateAgentState(isWarmup, container, peer.ControlledAgent, false);
				return;
			}
			if (MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) > 0)
			{
				MBReadOnlyList<IFormationUnit> mbreadOnlyList3;
				if (peer == null)
				{
					mbreadOnlyList3 = null;
				}
				else
				{
					Formation controlledFormation2 = peer.ControlledFormation;
					mbreadOnlyList3 = ((controlledFormation2 != null) ? controlledFormation2.Arrangement.GetAllUnits() : null);
				}
				MBReadOnlyList<IFormationUnit> mbreadOnlyList4 = mbreadOnlyList3;
				if (mbreadOnlyList4 == null)
				{
					return;
				}
				using (List<IFormationUnit>.Enumerator enumerator2 = mbreadOnlyList4.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Agent agent2;
						if ((agent2 = enumerator2.Current as Agent) != null && agent2.IsActive())
						{
							bool flag4 = true;
							foreach (MPPerkCondition mpperkCondition2 in this.Conditions)
							{
								if (!mpperkCondition2.IsPeerCondition && !mpperkCondition2.Check(agent2))
								{
									flag4 = false;
									break;
								}
							}
							this.UpdateAgentState(isWarmup, container, agent2, flag4);
						}
					}
					return;
				}
			}
			bool flag5 = true;
			foreach (MPPerkCondition mpperkCondition3 in this.Conditions)
			{
				if (!mpperkCondition3.IsPeerCondition && !mpperkCondition3.Check(peer.ControlledAgent))
				{
					flag5 = false;
					break;
				}
			}
			this.UpdateAgentState(isWarmup, container, peer.ControlledAgent, flag5);
		}

		// Token: 0x06002D4F RID: 11599 RVA: 0x000AF108 File Offset: 0x000AD308
		public void OnEvent(bool isWarmup, Agent agent, MPConditionalEffect.ConditionalEffectContainer container)
		{
			if (agent != null)
			{
				bool flag = true;
				using (List<MPPerkCondition>.Enumerator enumerator = this.Conditions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.Check(agent))
						{
							flag = false;
							break;
						}
					}
				}
				this.UpdateAgentState(isWarmup, container, agent, flag);
			}
		}

		// Token: 0x06002D50 RID: 11600 RVA: 0x000AF170 File Offset: 0x000AD370
		public void OnTick(bool isWarmup, MissionPeer peer, int tickCount)
		{
			if (MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) <= 0)
			{
				if (peer == null)
				{
					return;
				}
				Agent controlledAgent = peer.ControlledAgent;
				bool? flag = ((controlledAgent != null) ? new bool?(controlledAgent.IsActive()) : null);
				bool flag2 = true;
				if (!((flag.GetValueOrDefault() == flag2) & (flag != null)))
				{
					return;
				}
			}
			bool flag3 = true;
			foreach (MPPerkCondition mpperkCondition in this.Conditions)
			{
				if (mpperkCondition.IsPeerCondition && !mpperkCondition.Check(peer))
				{
					flag3 = false;
					break;
				}
			}
			if (flag3)
			{
				if (MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) > 0)
				{
					MBReadOnlyList<IFormationUnit> mbreadOnlyList;
					if (peer == null)
					{
						mbreadOnlyList = null;
					}
					else
					{
						Formation controlledFormation = peer.ControlledFormation;
						mbreadOnlyList = ((controlledFormation != null) ? controlledFormation.Arrangement.GetAllUnits() : null);
					}
					MBReadOnlyList<IFormationUnit> mbreadOnlyList2 = mbreadOnlyList;
					if (mbreadOnlyList2 == null)
					{
						return;
					}
					using (List<IFormationUnit>.Enumerator enumerator2 = mbreadOnlyList2.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Agent agent;
							if ((agent = enumerator2.Current as Agent) != null && agent.IsActive())
							{
								bool flag4 = true;
								foreach (MPPerkCondition mpperkCondition2 in this.Conditions)
								{
									if (!mpperkCondition2.IsPeerCondition && !mpperkCondition2.Check(agent))
									{
										flag4 = false;
										break;
									}
								}
								if (flag4)
								{
									foreach (MPPerkEffectBase mpperkEffectBase in this.Effects)
									{
										if ((!isWarmup || !mpperkEffectBase.IsDisabledInWarmup) && mpperkEffectBase.IsTickRequired)
										{
											mpperkEffectBase.OnTick(agent, tickCount);
										}
									}
								}
							}
						}
						return;
					}
				}
				bool flag5 = true;
				foreach (MPPerkCondition mpperkCondition3 in this.Conditions)
				{
					if (!mpperkCondition3.IsPeerCondition && !mpperkCondition3.Check(peer.ControlledAgent))
					{
						flag5 = false;
						break;
					}
				}
				if (flag5)
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in this.Effects)
					{
						if ((!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup) && mpperkEffectBase2.IsTickRequired)
						{
							mpperkEffectBase2.OnTick(peer.ControlledAgent, tickCount);
						}
					}
				}
			}
		}

		// Token: 0x06002D51 RID: 11601 RVA: 0x000AF430 File Offset: 0x000AD630
		private void UpdateAgentState(bool isWarmup, MPConditionalEffect.ConditionalEffectContainer container, Agent agent, bool state)
		{
			if (container.GetState(this, agent) != state)
			{
				container.SetState(this, agent, state);
				foreach (MPPerkEffectBase mpperkEffectBase in this.Effects)
				{
					if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
					{
						mpperkEffectBase.OnUpdate(agent, state);
					}
				}
			}
		}

		// Token: 0x02000602 RID: 1538
		public class ConditionalEffectContainer : List<MPConditionalEffect>
		{
			// Token: 0x06004023 RID: 16419 RVA: 0x000FAE04 File Offset: 0x000F9004
			public ConditionalEffectContainer()
			{
			}

			// Token: 0x06004024 RID: 16420 RVA: 0x000FAE0C File Offset: 0x000F900C
			public ConditionalEffectContainer(IEnumerable<MPConditionalEffect> conditionalEffects)
				: base(conditionalEffects)
			{
			}

			// Token: 0x06004025 RID: 16421 RVA: 0x000FAE18 File Offset: 0x000F9018
			public bool GetState(MPConditionalEffect conditionalEffect, Agent agent)
			{
				ConditionalWeakTable<Agent, MPConditionalEffect.ConditionalEffectContainer.ConditionState> conditionalWeakTable;
				MPConditionalEffect.ConditionalEffectContainer.ConditionState conditionState;
				return this._states != null && this._states.TryGetValue(conditionalEffect, out conditionalWeakTable) && conditionalWeakTable.TryGetValue(agent, out conditionState) && conditionState.IsSatisfied;
			}

			// Token: 0x06004026 RID: 16422 RVA: 0x000FAE50 File Offset: 0x000F9050
			public void SetState(MPConditionalEffect conditionalEffect, Agent agent, bool state)
			{
				if (this._states == null)
				{
					this._states = new Dictionary<MPConditionalEffect, ConditionalWeakTable<Agent, MPConditionalEffect.ConditionalEffectContainer.ConditionState>>();
					ConditionalWeakTable<Agent, MPConditionalEffect.ConditionalEffectContainer.ConditionState> conditionalWeakTable = new ConditionalWeakTable<Agent, MPConditionalEffect.ConditionalEffectContainer.ConditionState>();
					conditionalWeakTable.Add(agent, new MPConditionalEffect.ConditionalEffectContainer.ConditionState
					{
						IsSatisfied = state
					});
					this._states.Add(conditionalEffect, conditionalWeakTable);
					return;
				}
				ConditionalWeakTable<Agent, MPConditionalEffect.ConditionalEffectContainer.ConditionState> conditionalWeakTable2;
				if (!this._states.TryGetValue(conditionalEffect, out conditionalWeakTable2))
				{
					conditionalWeakTable2 = new ConditionalWeakTable<Agent, MPConditionalEffect.ConditionalEffectContainer.ConditionState>();
					conditionalWeakTable2.Add(agent, new MPConditionalEffect.ConditionalEffectContainer.ConditionState
					{
						IsSatisfied = state
					});
					this._states.Add(conditionalEffect, conditionalWeakTable2);
					return;
				}
				MPConditionalEffect.ConditionalEffectContainer.ConditionState conditionState;
				if (!conditionalWeakTable2.TryGetValue(agent, out conditionState))
				{
					conditionalWeakTable2.Add(agent, new MPConditionalEffect.ConditionalEffectContainer.ConditionState
					{
						IsSatisfied = state
					});
					return;
				}
				conditionState.IsSatisfied = state;
			}

			// Token: 0x06004027 RID: 16423 RVA: 0x000FAEF4 File Offset: 0x000F90F4
			public void ResetStates()
			{
				this._states = null;
			}

			// Token: 0x0400209D RID: 8349
			private Dictionary<MPConditionalEffect, ConditionalWeakTable<Agent, MPConditionalEffect.ConditionalEffectContainer.ConditionState>> _states;

			// Token: 0x020006D4 RID: 1748
			private class ConditionState
			{
				// Token: 0x17000B36 RID: 2870
				// (get) Token: 0x06004326 RID: 17190 RVA: 0x001015AB File Offset: 0x000FF7AB
				// (set) Token: 0x06004327 RID: 17191 RVA: 0x001015B3 File Offset: 0x000FF7B3
				public bool IsSatisfied { get; set; }
			}
		}
	}
}
