using System;
using TaleWorlds.Engine.Options;

namespace TaleWorlds.MountAndBlade.Options.ManagedOptions
{
	// Token: 0x020003A5 RID: 933
	public abstract class ManagedOptionData : IOptionData
	{
		// Token: 0x0600358D RID: 13709 RVA: 0x000DD624 File Offset: 0x000DB824
		protected ManagedOptionData(ManagedOptions.ManagedOptionsType type)
		{
			this.Type = type;
			this._value = ManagedOptions.GetConfig(type);
		}

		// Token: 0x0600358E RID: 13710 RVA: 0x000DD63F File Offset: 0x000DB83F
		public virtual float GetDefaultValue()
		{
			return ManagedOptions.GetDefaultConfig(this.Type);
		}

		// Token: 0x0600358F RID: 13711 RVA: 0x000DD64C File Offset: 0x000DB84C
		public void Commit()
		{
			if (this._value != ManagedOptions.GetConfig(this.Type))
			{
				ManagedOptions.SetConfig(this.Type, this._value);
			}
		}

		// Token: 0x06003590 RID: 13712 RVA: 0x000DD672 File Offset: 0x000DB872
		public float GetValue(bool forceRefresh)
		{
			if (forceRefresh)
			{
				this._value = ManagedOptions.GetConfig(this.Type);
			}
			return this._value;
		}

		// Token: 0x06003591 RID: 13713 RVA: 0x000DD68E File Offset: 0x000DB88E
		public void SetValue(float value)
		{
			this._value = value;
		}

		// Token: 0x06003592 RID: 13714 RVA: 0x000DD697 File Offset: 0x000DB897
		public object GetOptionType()
		{
			return this.Type;
		}

		// Token: 0x06003593 RID: 13715 RVA: 0x000DD6A4 File Offset: 0x000DB8A4
		public bool IsNative()
		{
			return false;
		}

		// Token: 0x06003594 RID: 13716 RVA: 0x000DD6A7 File Offset: 0x000DB8A7
		public bool IsAction()
		{
			return false;
		}

		// Token: 0x06003595 RID: 13717 RVA: 0x000DD6AC File Offset: 0x000DB8AC
		public ValueTuple<string, bool> GetIsDisabledAndReasonID()
		{
			ManagedOptions.ManagedOptionsType type = this.Type;
			if (type - ManagedOptions.ManagedOptionsType.ControlBlockDirection <= 1 && BannerlordConfig.GyroOverrideForAttackDefend)
			{
				return new ValueTuple<string, bool>("str_gyro_overrides_attack_block_direction", true);
			}
			return new ValueTuple<string, bool>(string.Empty, false);
		}

		// Token: 0x040016D5 RID: 5845
		public readonly ManagedOptions.ManagedOptionsType Type;

		// Token: 0x040016D6 RID: 5846
		private float _value;
	}
}
