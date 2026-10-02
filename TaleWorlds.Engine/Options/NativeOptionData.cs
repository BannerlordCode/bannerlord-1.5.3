using System;

namespace TaleWorlds.Engine.Options
{
	// Token: 0x020000AC RID: 172
	public abstract class NativeOptionData : IOptionData
	{
		// Token: 0x06000F7D RID: 3965 RVA: 0x000125C7 File Offset: 0x000107C7
		protected NativeOptionData(NativeOptions.NativeOptionsType type)
		{
			this.Type = type;
			this._value = NativeOptions.GetConfig(type);
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x000125E2 File Offset: 0x000107E2
		public virtual float GetDefaultValue()
		{
			return NativeOptions.GetDefaultConfig(this.Type);
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x000125EF File Offset: 0x000107EF
		public void Commit()
		{
			NativeOptions.SetConfig(this.Type, this._value);
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x00012602 File Offset: 0x00010802
		public float GetValue(bool forceRefresh)
		{
			if (forceRefresh)
			{
				this._value = NativeOptions.GetConfig(this.Type);
			}
			return this._value;
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x0001261E File Offset: 0x0001081E
		public void SetValue(float value)
		{
			this._value = value;
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x00012627 File Offset: 0x00010827
		public object GetOptionType()
		{
			return this.Type;
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x00012634 File Offset: 0x00010834
		public bool IsNative()
		{
			return true;
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x00012637 File Offset: 0x00010837
		public bool IsAction()
		{
			return false;
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x0001263C File Offset: 0x0001083C
		public ValueTuple<string, bool> GetIsDisabledAndReasonID()
		{
			NativeOptions.NativeOptionsType type = this.Type;
			if (type <= NativeOptions.NativeOptionsType.ResolutionScale)
			{
				if (type != NativeOptions.NativeOptionsType.GyroAimSensitivity)
				{
					if (type == NativeOptions.NativeOptionsType.ResolutionScale)
					{
						if (NativeOptions.GetConfig(NativeOptions.NativeOptionsType.DLSS) != 0f)
						{
							return new ValueTuple<string, bool>("str_dlss_enabled", true);
						}
						if (NativeOptions.GetConfig(NativeOptions.NativeOptionsType.DynamicResolution) != 0f)
						{
							return new ValueTuple<string, bool>("str_dynamic_resolution_enabled", true);
						}
					}
				}
				else if (NativeOptions.GetConfig(NativeOptions.NativeOptionsType.EnableGyroAssistedAim) != 1f)
				{
					return new ValueTuple<string, bool>("str_gyro_disabled", true);
				}
			}
			else if (type != NativeOptions.NativeOptionsType.DLSS)
			{
				if (type != NativeOptions.NativeOptionsType.DynamicResolution)
				{
					if (type == NativeOptions.NativeOptionsType.DynamicResolutionTarget)
					{
						if (NativeOptions.GetConfig(NativeOptions.NativeOptionsType.DynamicResolution) == 0f)
						{
							return new ValueTuple<string, bool>("str_dynamic_resolution_disabled", true);
						}
						if (NativeOptions.GetConfig(NativeOptions.NativeOptionsType.DLSS) != 0f)
						{
							return new ValueTuple<string, bool>("str_dlss_enabled", true);
						}
					}
				}
				else if (NativeOptions.GetConfig(NativeOptions.NativeOptionsType.DLSS) != 0f)
				{
					return new ValueTuple<string, bool>("str_dlss_enabled", true);
				}
			}
			else if (!NativeOptions.GetIsDLSSAvailable())
			{
				return new ValueTuple<string, bool>("str_dlss_not_available", true);
			}
			return new ValueTuple<string, bool>(string.Empty, false);
		}

		// Token: 0x04000221 RID: 545
		public readonly NativeOptions.NativeOptionsType Type;

		// Token: 0x04000222 RID: 546
		private float _value;
	}
}
