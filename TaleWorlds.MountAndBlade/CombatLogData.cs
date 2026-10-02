using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001F4 RID: 500
	public struct CombatLogData
	{
		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06001D16 RID: 7446 RVA: 0x00063150 File Offset: 0x00061350
		private bool IsValidForPlayer
		{
			get
			{
				return this.IsImportant && (this.IsAttackerPlayer || this.IsVictimPlayer);
			}
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06001D17 RID: 7447 RVA: 0x0006316C File Offset: 0x0006136C
		private bool IsImportant
		{
			get
			{
				return this.TotalDamage > 0 || this.TotalFireDamage > 0 || this.CrushedThrough || this.Chamber;
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001D18 RID: 7448 RVA: 0x00063190 File Offset: 0x00061390
		private bool IsSpecialSelfDamage
		{
			get
			{
				return this.IsSpecialDamage && this.IsVictimAgentSameAsAttackerAgent;
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06001D19 RID: 7449 RVA: 0x000631A2 File Offset: 0x000613A2
		private bool IsAttackerPlayer
		{
			get
			{
				if (!this.IsAttackerAgentHuman)
				{
					return this.DoesAttackerAgentHaveRiderAgent && this.IsAttackerAgentRiderAgentMine;
				}
				return this.IsAttackerAgentMine;
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06001D1A RID: 7450 RVA: 0x000631C3 File Offset: 0x000613C3
		private bool IsVictimPlayer
		{
			get
			{
				if (!this.IsVictimAgentHuman)
				{
					return this.DoesVictimAgentHaveRiderAgent && this.IsVictimAgentRiderAgentMine;
				}
				return this.IsVictimAgentMine;
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06001D1B RID: 7451 RVA: 0x000631E4 File Offset: 0x000613E4
		private bool IsAttackerMount
		{
			get
			{
				return this.IsAttackerAgentMount;
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06001D1C RID: 7452 RVA: 0x000631EC File Offset: 0x000613EC
		private bool IsVictimMount
		{
			get
			{
				return this.IsVictimAgentMount;
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001D1D RID: 7453 RVA: 0x000631F4 File Offset: 0x000613F4
		public int TotalDamage
		{
			get
			{
				return this.InflictedDamage + this.ModifiedDamage;
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001D1E RID: 7454 RVA: 0x00063203 File Offset: 0x00061403
		public int TotalFireDamage
		{
			get
			{
				return this.InflictedFireDamage + this.ModifiedFireDamage;
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001D1F RID: 7455 RVA: 0x00063212 File Offset: 0x00061412
		// (set) Token: 0x06001D20 RID: 7456 RVA: 0x0006321A File Offset: 0x0006141A
		public float AttackProgress { get; internal set; }

		// Token: 0x06001D21 RID: 7457 RVA: 0x00063224 File Offset: 0x00061424
		public List<ValueTuple<string, uint>> GetLogString()
		{
			CombatLogData._logStringCache.Clear();
			if (this.IsValidForPlayer && !this.IsSpecialSelfDamage && ManagedOptions.GetConfig(ManagedOptions.ManagedOptionsType.ReportDamage) > 0f)
			{
				if (this.IsSneakAttack && this.IsAttackerPlayer)
				{
					CombatLogData._logStringCache.Add(ValueTuple.Create<string, uint>(GameTexts.FindText("combat_log_sneak_attack", null).ToString(), 4289612505U));
				}
				if (this.IsRangedAttack && this.IsAttackerPlayer && this.BodyPartHit == BoneBodyPartType.Head)
				{
					CombatLogData._logStringCache.Add(ValueTuple.Create<string, uint>(GameTexts.FindText("ui_head_shot", null).ToString(), 4289612505U));
				}
				if (this.IsFriendlyFire)
				{
					CombatLogData._logStringCache.Add(ValueTuple.Create<string, uint>(GameTexts.FindText("combat_log_friendly_fire", null).ToString(), 4289612505U));
				}
				if (this.CrushedThrough && !this.IsFriendlyFire)
				{
					if (this.IsAttackerPlayer)
					{
						CombatLogData._logStringCache.Add(ValueTuple.Create<string, uint>(GameTexts.FindText("combat_log_crushed_through_attacker", null).ToString(), 4289612505U));
					}
					else
					{
						CombatLogData._logStringCache.Add(ValueTuple.Create<string, uint>(GameTexts.FindText("combat_log_crushed_through_victim", null).ToString(), 4289612505U));
					}
				}
				if (this.Chamber)
				{
					CombatLogData._logStringCache.Add(ValueTuple.Create<string, uint>(GameTexts.FindText("combat_log_chamber_blocked", null).ToString(), 4289612505U));
				}
				uint num = 4290563554U;
				GameTexts.SetVariable("DAMAGE", this.TotalDamage);
				string text = "DAMAGE_TYPE";
				string text2 = "combat_log_damage_type";
				int num2 = (int)this.DamageType;
				GameTexts.SetVariable(text, GameTexts.FindText(text2, num2.ToString()));
				MBStringBuilder mbstringBuilder = default(MBStringBuilder);
				mbstringBuilder.Initialize(16, "GetLogString");
				TextObject textObject = null;
				if (this.IsEntityToEntityCollisionDamage)
				{
					if (this.IsAttackerPlayer)
					{
						if (this.IsSpecialDamage)
						{
							mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_ram_damage_delivered", null));
						}
						else
						{
							mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_collision_damage_delivered", null));
						}
					}
					else if (this.IsSpecialDamage)
					{
						mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_ram_damage_received", null));
					}
					else
					{
						mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_collision_damage_received", null));
					}
				}
				else if (this.IsVictimAgentSameAsAttackerAgent)
				{
					mbstringBuilder.Append<TextObject>(GameTexts.FindText("ui_received_number_damage_fall", null));
					num = 4292917946U;
				}
				else if (this.IsVictimMount)
				{
					if (this.IsVictimRiderAgentSameAsAttackerAgent)
					{
						mbstringBuilder.Append<TextObject>(GameTexts.FindText("ui_received_number_damage_fall_to_horse", null));
						num = 4292917946U;
					}
					else
					{
						mbstringBuilder.Append<TextObject>(GameTexts.FindText(this.IsAttackerPlayer ? "ui_delivered_number_damage_to_horse" : "ui_horse_received_number_damage", null));
						num = (this.IsAttackerPlayer ? 4210351871U : 4292917946U);
					}
				}
				else if (this.MissionObjectHit != null)
				{
					WeakGameEntity weakGameEntity = this.MissionObjectHit.GameEntity;
					textObject = this.MissionObjectHit.HitObjectName;
					while (weakGameEntity != null)
					{
						if (!TextObject.IsNullOrEmpty(textObject))
						{
							break;
						}
						int scriptCount = weakGameEntity.GetScriptCount();
						for (int i = 0; i < scriptCount; i++)
						{
							MissionObject missionObject;
							if ((missionObject = weakGameEntity.GetScriptAtIndex(i) as MissionObject) != null && TextObject.IsNullOrEmpty(textObject) && !TextObject.IsNullOrEmpty(missionObject.HitObjectName))
							{
								textObject = missionObject.HitObjectName;
								break;
							}
						}
						weakGameEntity = weakGameEntity.Parent;
					}
				}
				else if (this.IsAttackerMount)
				{
					mbstringBuilder.Append<TextObject>(GameTexts.FindText(this.IsAttackerPlayer ? "ui_horse_charged_for_number_damage" : "ui_received_number_damage", null));
					num = (this.IsAttackerPlayer ? 4210351871U : 4292917946U);
				}
				else if (this.TotalDamage > 0)
				{
					mbstringBuilder.Append<TextObject>(GameTexts.FindText(this.IsAttackerPlayer ? "ui_delivered_number_damage" : "ui_received_number_damage", null));
					num = (this.IsAttackerPlayer ? 4210351871U : 4292917946U);
				}
				if (this.MissionObjectHit != null && this.TotalDamage > 0)
				{
					mbstringBuilder.Append<TextObject>(GameTexts.FindText("ui_delivered_number_damage_to_entity", null));
				}
				if (this.BodyPartHit != BoneBodyPartType.None)
				{
					string text3 = "BODY_PART";
					string text4 = "body_part_type";
					num2 = (int)this.BodyPartHit;
					GameTexts.SetVariable(text3, GameTexts.FindText(text4, num2.ToString()));
					mbstringBuilder.Append<string>("<Detail>");
					mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_detail_body_part", null));
					mbstringBuilder.Append<string>("</Detail>");
				}
				if (this.HitSpeed > 1E-05f)
				{
					GameTexts.SetVariable("SPEED", MathF.Round(this.HitSpeed, 2));
					mbstringBuilder.Append<string>("<Detail>");
					mbstringBuilder.Append<TextObject>(this.IsRangedAttack ? GameTexts.FindText("combat_log_detail_missile_speed", null) : GameTexts.FindText("combat_log_detail_move_speed", null));
					mbstringBuilder.Append<string>("</Detail>");
				}
				if (this.IsRangedAttack)
				{
					GameTexts.SetVariable("DISTANCE", MathF.Round(this.Distance, 1));
					mbstringBuilder.Append<string>("<Detail>");
					mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_detail_distance", null));
					mbstringBuilder.Append<string>("</Detail>");
				}
				if (this.TotalDamage > 0)
				{
					if (this.AbsorbedDamage > 0)
					{
						GameTexts.SetVariable("ABSORBED_DAMAGE", this.AbsorbedDamage);
						mbstringBuilder.Append<string>("<Detail>");
						mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_detail_absorbed_damage", null));
						mbstringBuilder.Append<string>("</Detail>");
					}
					if (this.ModifiedDamage != 0)
					{
						GameTexts.SetVariable("MODIFIED_DAMAGE", MathF.Abs(this.ModifiedDamage));
						mbstringBuilder.Append<string>("<Detail>");
						if (this.ModifiedDamage > 0)
						{
							mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_detail_extra_damage", null));
						}
						else if (this.ModifiedDamage < 0)
						{
							mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_detail_reduced_damage", null));
						}
						mbstringBuilder.Append<string>("</Detail>");
					}
					if (this.ReflectedDamage > 0)
					{
						GameTexts.SetVariable("REFLECTED_DAMAGE", this.ReflectedDamage);
						mbstringBuilder.Append<string>("<Detail>");
						mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_detail_reflected_damage", null));
						mbstringBuilder.Append<string>("</Detail>");
					}
				}
				if (this.TotalFireDamage > 0)
				{
					if (this.TotalDamage > 0 && this.TotalFireDamage > 0)
					{
						mbstringBuilder.AppendLine();
					}
					GameTexts.SetVariable("FIRE_DAMAGE", this.TotalFireDamage);
					mbstringBuilder.Append<TextObject>(GameTexts.FindText("ui_delivered_number_fire_damage_to_entity", null));
					if (this.ModifiedFireDamage != 0)
					{
						GameTexts.SetVariable("MODIFIED_DAMAGE", MathF.Abs(this.ModifiedFireDamage));
						mbstringBuilder.Append<string>("<Detail>");
						if (this.ModifiedFireDamage > 0)
						{
							mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_detail_extra_damage", null));
						}
						else if (this.ModifiedFireDamage < 0)
						{
							mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_detail_reduced_damage", null));
						}
						mbstringBuilder.Append<string>("</Detail>");
					}
				}
				if (!TextObject.IsNullOrEmpty(textObject))
				{
					GameTexts.SetVariable("OBJECT_NAME", textObject.ToString());
					mbstringBuilder.Append<string>("<Detail>");
					mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_detail_entity_name", null));
					mbstringBuilder.Append<string>("</Detail>");
				}
				CombatLogData._logStringCache.Add(ValueTuple.Create<string, uint>(mbstringBuilder.ToStringAndRelease(), num));
			}
			return CombatLogData._logStringCache;
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x00063958 File Offset: 0x00061B58
		public CombatLogData(bool isVictimAgentSameAsAttackerAgent, bool isAttackerAgentHuman, bool isAttackerAgentMine, bool doesAttackerAgentHaveRiderAgent, bool isAttackerAgentRiderAgentMine, bool isAttackerAgentMount, bool isVictimAgentHuman, bool isVictimAgentMine, bool isVictimAgentDead, bool doesVictimAgentHaveRiderAgent, bool isVictimAgentRiderAgentIsMine, bool isVictimAgentMount, MissionObject missionObjectHit, bool isVictimRiderAgentSameAsAttackerAgent, bool crushedThrough, bool chamber, float distance)
		{
			this.IsVictimAgentSameAsAttackerAgent = isVictimAgentSameAsAttackerAgent;
			this.IsAttackerAgentHuman = isAttackerAgentHuman;
			this.IsAttackerAgentMine = isAttackerAgentMine;
			this.DoesAttackerAgentHaveRiderAgent = doesAttackerAgentHaveRiderAgent;
			this.IsAttackerAgentRiderAgentMine = isAttackerAgentRiderAgentMine;
			this.IsAttackerAgentMount = isAttackerAgentMount;
			this.IsVictimAgentHuman = isVictimAgentHuman;
			this.IsVictimAgentMine = isVictimAgentMine;
			this.DoesVictimAgentHaveRiderAgent = doesVictimAgentHaveRiderAgent;
			this.IsVictimAgentRiderAgentMine = isVictimAgentRiderAgentIsMine;
			this.IsVictimAgentMount = isVictimAgentMount;
			this.MissionObjectHit = missionObjectHit;
			this.IsVictimRiderAgentSameAsAttackerAgent = isVictimRiderAgentSameAsAttackerAgent;
			this.IsFatalDamage = isVictimAgentDead;
			this.IsEntityToEntityCollisionDamage = false;
			this.IsSpecialDamage = false;
			this.DamageType = DamageTypes.Blunt;
			this.CrushedThrough = crushedThrough;
			this.Chamber = chamber;
			this.IsRangedAttack = false;
			this.IsFriendlyFire = false;
			this.IsSneakAttack = false;
			this.VictimAgentName = null;
			this.HitSpeed = 0f;
			this.InflictedDamage = 0;
			this.AbsorbedDamage = 0;
			this.ModifiedDamage = 0;
			this.InflictedFireDamage = 0;
			this.ModifiedFireDamage = 0;
			this.ReflectedDamage = 0;
			this.AttackProgress = 0f;
			this.BodyPartHit = BoneBodyPartType.None;
			this.Distance = distance;
		}

		// Token: 0x06001D23 RID: 7459 RVA: 0x00063A62 File Offset: 0x00061C62
		public void SetVictimAgent(Agent victimAgent)
		{
			if (((victimAgent != null) ? victimAgent.MissionPeer : null) != null)
			{
				this.VictimAgentName = victimAgent.MissionPeer.DisplayedName;
				return;
			}
			this.VictimAgentName = ((victimAgent != null) ? victimAgent.Name : null);
		}

		// Token: 0x040009F4 RID: 2548
		private const string DetailTagStart = "<Detail>";

		// Token: 0x040009F5 RID: 2549
		private const string DetailTagEnd = "</Detail>";

		// Token: 0x040009F6 RID: 2550
		private const uint DamageReceivedColor = 4292917946U;

		// Token: 0x040009F7 RID: 2551
		private const uint DamageDealedColor = 4210351871U;

		// Token: 0x040009F8 RID: 2552
		private static List<ValueTuple<string, uint>> _logStringCache = new List<ValueTuple<string, uint>>();

		// Token: 0x040009F9 RID: 2553
		public readonly bool IsVictimAgentSameAsAttackerAgent;

		// Token: 0x040009FA RID: 2554
		public readonly bool IsVictimRiderAgentSameAsAttackerAgent;

		// Token: 0x040009FB RID: 2555
		public readonly bool IsAttackerAgentHuman;

		// Token: 0x040009FC RID: 2556
		public readonly bool IsAttackerAgentMine;

		// Token: 0x040009FD RID: 2557
		public readonly bool DoesAttackerAgentHaveRiderAgent;

		// Token: 0x040009FE RID: 2558
		public readonly bool IsAttackerAgentRiderAgentMine;

		// Token: 0x040009FF RID: 2559
		public readonly bool IsAttackerAgentMount;

		// Token: 0x04000A00 RID: 2560
		public readonly bool IsVictimAgentHuman;

		// Token: 0x04000A01 RID: 2561
		public readonly bool IsVictimAgentMine;

		// Token: 0x04000A02 RID: 2562
		public readonly bool DoesVictimAgentHaveRiderAgent;

		// Token: 0x04000A03 RID: 2563
		public readonly bool IsVictimAgentRiderAgentMine;

		// Token: 0x04000A04 RID: 2564
		public readonly bool IsVictimAgentMount;

		// Token: 0x04000A05 RID: 2565
		public MissionObject MissionObjectHit;

		// Token: 0x04000A06 RID: 2566
		public DamageTypes DamageType;

		// Token: 0x04000A07 RID: 2567
		public bool CrushedThrough;

		// Token: 0x04000A08 RID: 2568
		public bool Chamber;

		// Token: 0x04000A09 RID: 2569
		public bool IsRangedAttack;

		// Token: 0x04000A0A RID: 2570
		public bool IsFriendlyFire;

		// Token: 0x04000A0B RID: 2571
		public bool IsFatalDamage;

		// Token: 0x04000A0C RID: 2572
		public bool IsSpecialDamage;

		// Token: 0x04000A0D RID: 2573
		public bool IsEntityToEntityCollisionDamage;

		// Token: 0x04000A0E RID: 2574
		public bool IsSneakAttack;

		// Token: 0x04000A0F RID: 2575
		public BoneBodyPartType BodyPartHit;

		// Token: 0x04000A10 RID: 2576
		public string VictimAgentName;

		// Token: 0x04000A11 RID: 2577
		public float HitSpeed;

		// Token: 0x04000A12 RID: 2578
		public int InflictedDamage;

		// Token: 0x04000A13 RID: 2579
		public int AbsorbedDamage;

		// Token: 0x04000A14 RID: 2580
		public int ModifiedDamage;

		// Token: 0x04000A15 RID: 2581
		public int InflictedFireDamage;

		// Token: 0x04000A16 RID: 2582
		public int ModifiedFireDamage;

		// Token: 0x04000A17 RID: 2583
		public int ReflectedDamage;

		// Token: 0x04000A19 RID: 2585
		public float Distance;
	}
}
