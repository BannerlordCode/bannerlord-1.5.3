using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001F3 RID: 499
	public static class CombatLogManager
	{
		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06001D11 RID: 7441 RVA: 0x00062EE8 File Offset: 0x000610E8
		// (remove) Token: 0x06001D12 RID: 7442 RVA: 0x00062F1C File Offset: 0x0006111C
		public static event Action<CombatLogData> OnGenerateCombatLog;

		// Token: 0x06001D13 RID: 7443 RVA: 0x00062F50 File Offset: 0x00061150
		public static void PrintDebugLogForInfo(Agent attackerAgent, Agent victimAgent, DamageTypes damageType, int speedBonus, int armorAmount, int inflictedDamage, int absorbedByArmor, sbyte collisionBone, float lostHpPercentage)
		{
			TextObject textObject = TextObject.GetEmpty();
			CombatLogColor combatLogColor = CombatLogColor.White;
			bool isMine = attackerAgent.IsMine;
			bool isMine2 = victimAgent.IsMine;
			GameTexts.SetVariable("AMOUNT", inflictedDamage);
			GameTexts.SetVariable("DAMAGE_TYPE", damageType.ToString().ToLower());
			GameTexts.SetVariable("LOST_HP_PERCENTAGE", lostHpPercentage);
			if (isMine2)
			{
				GameTexts.SetVariable("ATTACKER_NAME", attackerAgent.NameTextObject);
				textObject = GameTexts.FindText("combat_log_player_attacked", null);
				combatLogColor = CombatLogColor.Red;
			}
			else if (isMine)
			{
				GameTexts.SetVariable("VICTIM_NAME", victimAgent.NameTextObject);
				textObject = GameTexts.FindText("combat_log_player_attacker", null);
				combatLogColor = CombatLogColor.Green;
			}
			CombatLogManager.Print(textObject, combatLogColor);
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "PrintDebugLogForInfo");
			if (armorAmount > 0)
			{
				GameTexts.SetVariable("ABSORBED_AMOUNT", absorbedByArmor);
				GameTexts.SetVariable("ARMOR_AMOUNT", armorAmount);
				mbstringBuilder.AppendLine<string>(GameTexts.FindText("combat_log_damage_absorbed", null).ToString());
			}
			if (victimAgent.IsHuman)
			{
				GameTexts.SetVariable("BONE", collisionBone.ToString());
				mbstringBuilder.AppendLine<string>(GameTexts.FindText("combat_log_hit_bone", null).ToString());
			}
			if (speedBonus != 0)
			{
				GameTexts.SetVariable("SPEED_BONUS", speedBonus);
				mbstringBuilder.AppendLine<string>(GameTexts.FindText("combat_log_speed_bonus", null).ToString());
			}
			CombatLogManager.Print(new TextObject(mbstringBuilder.ToStringAndRelease(), null), CombatLogColor.White);
		}

		// Token: 0x06001D14 RID: 7444 RVA: 0x000630A8 File Offset: 0x000612A8
		private static void Print(TextObject message, CombatLogColor logColor = CombatLogColor.White)
		{
			Debug.Print(message.ToString(), 0, (Debug.DebugColor)logColor, 562949953421312UL);
		}

		// Token: 0x06001D15 RID: 7445 RVA: 0x000630D0 File Offset: 0x000612D0
		public static void GenerateCombatLog(CombatLogData logData)
		{
			Action<CombatLogData> onGenerateCombatLog = CombatLogManager.OnGenerateCombatLog;
			if (onGenerateCombatLog != null)
			{
				onGenerateCombatLog(logData);
			}
			foreach (ValueTuple<string, uint> valueTuple in logData.GetLogString())
			{
				InformationManager.DisplayMessage(new InformationMessage(valueTuple.Item1, Color.FromUint(valueTuple.Item2), "Combat"));
			}
		}
	}
}
