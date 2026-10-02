using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin.Internal
{
	// Token: 0x0200007B RID: 123
	internal class AdminPanelNumericOption : AdminPanelOption<int>, IAdminPanelNumericOption, IAdminPanelOption<int>, IAdminPanelOption
	{
		// Token: 0x060003CD RID: 973 RVA: 0x0001075E File Offset: 0x0000E95E
		public AdminPanelNumericOption(string uniqueId)
			: base(uniqueId)
		{
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00010767 File Offset: 0x0000E967
		protected override bool AreEqualValues(int first, int second)
		{
			return first == second;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0001076D File Offset: 0x0000E96D
		public AdminPanelNumericOption SetMinimumValue(int value)
		{
			this._minimumValue = new int?(value);
			return this;
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0001077C File Offset: 0x0000E97C
		public AdminPanelNumericOption SetMaximumValue(int value)
		{
			this._maximumValue = new int?(value);
			return this;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0001078C File Offset: 0x0000E98C
		public AdminPanelNumericOption SetMinimumAndMaximumFrom(MultiplayerOptions.OptionType optionType)
		{
			MultiplayerOptionsProperty optionProperty = optionType.GetOptionProperty();
			if (optionProperty != null && optionProperty.HasBounds)
			{
				this._minimumValue = new int?(optionType.GetMinimumValue());
				this._maximumValue = new int?(optionType.GetMaximumValue());
				base.SetValue(MBMath.ClampInt(base.CurrentValue, this._minimumValue.Value, this._maximumValue.Value));
			}
			return this;
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x000107F7 File Offset: 0x0000E9F7
		public int? GetMinimumValue()
		{
			return this._minimumValue;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x000107FF File Offset: 0x0000E9FF
		public int? GetMaximumValue()
		{
			return this._maximumValue;
		}

		// Token: 0x04000121 RID: 289
		private int? _minimumValue;

		// Token: 0x04000122 RID: 290
		private int? _maximumValue;
	}
}
