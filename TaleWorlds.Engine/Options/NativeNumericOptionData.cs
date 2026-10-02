using System;

namespace TaleWorlds.Engine.Options
{
	// Token: 0x020000AB RID: 171
	public class NativeNumericOptionData : NativeOptionData, INumericOptionData, IOptionData
	{
		// Token: 0x06000F76 RID: 3958 RVA: 0x000123CF File Offset: 0x000105CF
		public NativeNumericOptionData(NativeOptions.NativeOptionsType type)
			: base(type)
		{
			this._minValue = NativeNumericOptionData.GetLimitValue(this.Type, true);
			this._maxValue = NativeNumericOptionData.GetLimitValue(this.Type, false);
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x000123FC File Offset: 0x000105FC
		public float GetMinValue()
		{
			return this._minValue;
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x00012404 File Offset: 0x00010604
		public float GetMaxValue()
		{
			return this._maxValue;
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x0001240C File Offset: 0x0001060C
		private static float GetLimitValue(NativeOptions.NativeOptionsType type, bool isMin)
		{
			if (type <= NativeOptions.NativeOptionsType.Brightness)
			{
				switch (type)
				{
				case NativeOptions.NativeOptionsType.MouseSensitivity:
					if (!isMin)
					{
						return 1f;
					}
					return 0.3f;
				case NativeOptions.NativeOptionsType.InvertMouseYAxis:
				case NativeOptions.NativeOptionsType.EnableVibration:
				case NativeOptions.NativeOptionsType.EnableGyroAssistedAim:
					break;
				case NativeOptions.NativeOptionsType.MouseYMovementScale:
					if (!isMin)
					{
						return 4f;
					}
					return 0.25f;
				case NativeOptions.NativeOptionsType.TrailAmount:
					if (!isMin)
					{
						return 1f;
					}
					return 0f;
				case NativeOptions.NativeOptionsType.GyroAimSensitivity:
					if (!isMin)
					{
						return 1f;
					}
					return 0f;
				default:
					switch (type)
					{
					case NativeOptions.NativeOptionsType.ResolutionScale:
						if (!isMin)
						{
							return 100f;
						}
						return 50f;
					case NativeOptions.NativeOptionsType.FrameLimiter:
						if (!isMin)
						{
							return 360f;
						}
						return 30f;
					case NativeOptions.NativeOptionsType.Brightness:
						if (!isMin)
						{
							return 100f;
						}
						return 0f;
					}
					break;
				}
			}
			else if (type != NativeOptions.NativeOptionsType.SharpenAmount)
			{
				switch (type)
				{
				case NativeOptions.NativeOptionsType.BrightnessMin:
					if (!isMin)
					{
						return 0.3f;
					}
					return 0f;
				case NativeOptions.NativeOptionsType.BrightnessMax:
					if (!isMin)
					{
						return 1f;
					}
					return 0.7f;
				case NativeOptions.NativeOptionsType.ExposureCompensation:
					if (!isMin)
					{
						return 2f;
					}
					return -2f;
				case NativeOptions.NativeOptionsType.DynamicResolutionTarget:
					if (!isMin)
					{
						return 240f;
					}
					return 30f;
				}
			}
			else
			{
				if (!isMin)
				{
					return 100f;
				}
				return 0f;
			}
			if (!isMin)
			{
				return 1f;
			}
			return 0f;
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x00012554 File Offset: 0x00010754
		public bool GetIsDiscrete()
		{
			NativeOptions.NativeOptionsType type = this.Type;
			if (type <= NativeOptions.NativeOptionsType.Brightness)
			{
				if (type - NativeOptions.NativeOptionsType.ResolutionScale > 1 && type != NativeOptions.NativeOptionsType.Brightness)
				{
					return false;
				}
			}
			else if (type != NativeOptions.NativeOptionsType.SharpenAmount)
			{
				switch (type)
				{
				case NativeOptions.NativeOptionsType.BrightnessMin:
				case NativeOptions.NativeOptionsType.BrightnessMax:
				case NativeOptions.NativeOptionsType.ExposureCompensation:
				case NativeOptions.NativeOptionsType.DynamicResolutionTarget:
					break;
				case NativeOptions.NativeOptionsType.BrightnessCalibrated:
				case NativeOptions.NativeOptionsType.DynamicResolution:
					return false;
				default:
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x000125A8 File Offset: 0x000107A8
		public int GetDiscreteIncrementInterval()
		{
			NativeOptions.NativeOptionsType type = this.Type;
			if (type == NativeOptions.NativeOptionsType.SharpenAmount)
			{
				return 5;
			}
			return 1;
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x000125C4 File Offset: 0x000107C4
		public bool GetShouldUpdateContinuously()
		{
			return true;
		}

		// Token: 0x0400021F RID: 543
		private readonly float _minValue;

		// Token: 0x04000220 RID: 544
		private readonly float _maxValue;
	}
}
