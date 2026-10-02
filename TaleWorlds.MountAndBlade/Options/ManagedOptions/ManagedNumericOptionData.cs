using System;
using TaleWorlds.Engine.Options;

namespace TaleWorlds.MountAndBlade.Options.ManagedOptions
{
	// Token: 0x020003A4 RID: 932
	public class ManagedNumericOptionData : ManagedOptionData, INumericOptionData, IOptionData
	{
		// Token: 0x06003586 RID: 13702 RVA: 0x000DD504 File Offset: 0x000DB704
		public ManagedNumericOptionData(ManagedOptions.ManagedOptionsType type)
			: base(type)
		{
			this._minValue = ManagedNumericOptionData.GetLimitValue(this.Type, true);
			this._maxValue = ManagedNumericOptionData.GetLimitValue(this.Type, false);
		}

		// Token: 0x06003587 RID: 13703 RVA: 0x000DD531 File Offset: 0x000DB731
		public float GetMinValue()
		{
			return this._minValue;
		}

		// Token: 0x06003588 RID: 13704 RVA: 0x000DD539 File Offset: 0x000DB739
		public float GetMaxValue()
		{
			return this._maxValue;
		}

		// Token: 0x06003589 RID: 13705 RVA: 0x000DD544 File Offset: 0x000DB744
		private static float GetLimitValue(ManagedOptions.ManagedOptionsType type, bool isMin)
		{
			if (type <= ManagedOptions.ManagedOptionsType.AutoSaveInterval)
			{
				if (type == ManagedOptions.ManagedOptionsType.BattleSize)
				{
					return (float)(isMin ? BannerlordConfig.MinBattleSize : BannerlordConfig.MaxBattleSize);
				}
				if (type == ManagedOptions.ManagedOptionsType.AutoSaveInterval)
				{
					if (!isMin)
					{
						return 60f;
					}
					return 4f;
				}
			}
			else if (type != ManagedOptions.ManagedOptionsType.FirstPersonFov)
			{
				if (type != ManagedOptions.ManagedOptionsType.CombatCameraDistance)
				{
					if (type == ManagedOptions.ManagedOptionsType.UIScale)
					{
						if (!isMin)
						{
							return 1f;
						}
						return 0.75f;
					}
				}
				else
				{
					if (!isMin)
					{
						return 2.4f;
					}
					return 0.7f;
				}
			}
			else
			{
				if (!isMin)
				{
					return 100f;
				}
				return 45f;
			}
			if (!isMin)
			{
				return 1f;
			}
			return 0f;
		}

		// Token: 0x0600358A RID: 13706 RVA: 0x000DD5D0 File Offset: 0x000DB7D0
		public bool GetIsDiscrete()
		{
			ManagedOptions.ManagedOptionsType type = this.Type;
			if (type <= ManagedOptions.ManagedOptionsType.AutoSaveInterval)
			{
				if (type != ManagedOptions.ManagedOptionsType.BattleSize && type != ManagedOptions.ManagedOptionsType.AutoSaveInterval)
				{
					return false;
				}
			}
			else if (type != ManagedOptions.ManagedOptionsType.FirstPersonFov)
			{
				if (type != ManagedOptions.ManagedOptionsType.UIScale)
				{
					return false;
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600358B RID: 13707 RVA: 0x000DD603 File Offset: 0x000DB803
		public int GetDiscreteIncrementInterval()
		{
			return 1;
		}

		// Token: 0x0600358C RID: 13708 RVA: 0x000DD608 File Offset: 0x000DB808
		public bool GetShouldUpdateContinuously()
		{
			ManagedOptions.ManagedOptionsType type = this.Type;
			return type != ManagedOptions.ManagedOptionsType.UIScale;
		}

		// Token: 0x040016D3 RID: 5843
		private readonly float _minValue;

		// Token: 0x040016D4 RID: 5844
		private readonly float _maxValue;
	}
}
