using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x02000026 RID: 38
	public static class CombatStatCalculator
	{
		// Token: 0x060001CF RID: 463 RVA: 0x000079E0 File Offset: 0x00005BE0
		public static float CalculateStrikeMagnitudeForSwing(float swingSpeed, float impactPointAsPercent, float weaponWeight, float weaponLength, float weaponInertia, float weaponCoM, float extraLinearSpeed)
		{
			float num = weaponLength * impactPointAsPercent - weaponCoM;
			float num2 = swingSpeed * (0.5f + weaponCoM) + extraLinearSpeed;
			float num3 = 0.5f * weaponWeight * num2 * num2;
			float num4 = 0.5f * weaponInertia * swingSpeed * swingSpeed;
			float num5 = num3 + num4;
			float num6 = (num2 + swingSpeed * num) / (1f / weaponWeight + num * num / weaponInertia);
			float num7 = num2 - num6 / weaponWeight;
			float num8 = swingSpeed - num6 * num / weaponInertia;
			float num9 = 0.5f * weaponWeight * num7 * num7;
			float num10 = 0.5f * weaponInertia * num8 * num8;
			float num11 = num9 + num10;
			float num12 = num5 - num11 + 0.5f;
			return 0.067f * num12;
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00007A7C File Offset: 0x00005C7C
		public static float CalculateStrikeMagnitudeForThrust(float thrustWeaponSpeed, float weaponWeight, float extraLinearSpeed, bool isThrown)
		{
			float num = thrustWeaponSpeed + extraLinearSpeed;
			if (num > 0f)
			{
				if (!isThrown)
				{
					weaponWeight += 2.5f;
				}
				float num2 = 0.5f * weaponWeight * num * num;
				return 0.125f * num2;
			}
			return 0f;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00007ABC File Offset: 0x00005CBC
		private static float CalculateStrikeMagnitudeForPassiveUsage(float weaponWeight, float extraLinearSpeed)
		{
			float num = 20f / ((extraLinearSpeed > 0f) ? MathF.Pow(extraLinearSpeed, 0.1f) : 1f) + weaponWeight;
			float num2 = CombatStatCalculator.CalculateStrikeMagnitudeForThrust(0f, num, extraLinearSpeed * 0.83f, false);
			if (num2 < 10f)
			{
				return 0f;
			}
			return num2;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00007B10 File Offset: 0x00005D10
		public static float CalculateBaseBlowMagnitudeForSwing(float angularSpeed, float weaponReach, float weaponWeight, float weaponInertia, float weaponCoM, float impactPoint, float exraLinearSpeed)
		{
			impactPoint = MathF.Min(impactPoint, 0.93f);
			float num = MBMath.ClampFloat(0.4f / weaponReach, 0f, 1f);
			float num2 = 0f;
			for (int i = 0; i < 5; i++)
			{
				float num3 = impactPoint + (float)i / 4f * num;
				if (num3 >= 1f)
				{
					break;
				}
				float num4 = CombatStatCalculator.CalculateStrikeMagnitudeForSwing(angularSpeed, num3, weaponWeight, weaponReach, weaponInertia, weaponCoM, exraLinearSpeed);
				if (num2 < num4)
				{
					num2 = num4;
				}
			}
			return num2;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00007B83 File Offset: 0x00005D83
		public static float CalculateBaseBlowMagnitudeForThrust(float linearSpeed, float weaponWeight, float exraLinearSpeed)
		{
			return CombatStatCalculator.CalculateStrikeMagnitudeForThrust(linearSpeed, weaponWeight, exraLinearSpeed, false);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00007B8E File Offset: 0x00005D8E
		public static float CalculateBaseBlowMagnitudeForPassiveUsage(float weaponWeight, float extraLinearSpeed)
		{
			return CombatStatCalculator.CalculateStrikeMagnitudeForPassiveUsage(weaponWeight, extraLinearSpeed);
		}

		// Token: 0x04000189 RID: 393
		public const float ReferenceSwingSpeed = 22f;

		// Token: 0x0400018A RID: 394
		public const float ReferenceThrustSpeed = 8.5f;

		// Token: 0x0400018B RID: 395
		public const float SwingSpeedConst = 4.5454545f;

		// Token: 0x0400018C RID: 396
		public const float ThrustSpeedConst = 11.764706f;

		// Token: 0x0400018D RID: 397
		public const float DefaultImpactDistanceFromTip = 0.07f;

		// Token: 0x0400018E RID: 398
		public const float ArmLength = 0.5f;

		// Token: 0x0400018F RID: 399
		public const float ArmWeight = 2.5f;
	}
}
