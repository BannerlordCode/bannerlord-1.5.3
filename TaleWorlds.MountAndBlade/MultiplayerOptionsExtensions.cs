using System;
using System.Linq;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000311 RID: 785
	public static class MultiplayerOptionsExtensions
	{
		// Token: 0x06002D32 RID: 11570 RVA: 0x000AE924 File Offset: 0x000ACB24
		public static string GetValueText(this MultiplayerOptions.OptionType optionType, MultiplayerOptions.MultiplayerOptionsAccessMode mode = MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)
		{
			switch (optionType.GetOptionProperty().OptionValueType)
			{
			case MultiplayerOptions.OptionValueType.Bool:
				return optionType.GetBoolValue(mode).ToString();
			case MultiplayerOptions.OptionValueType.Integer:
			case MultiplayerOptions.OptionValueType.Enum:
				return optionType.GetIntValue(mode).ToString();
			case MultiplayerOptions.OptionValueType.String:
				return optionType.GetStrValue(mode);
			default:
				Debug.FailedAssert("Missing OptionValueType for optionType: " + optionType, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Network\\Gameplay\\MultiplayerOptions.cs", "GetValueText", 1097);
				return null;
			}
		}

		// Token: 0x06002D33 RID: 11571 RVA: 0x000AE9A4 File Offset: 0x000ACBA4
		public static bool GetBoolValue(this MultiplayerOptions.OptionType optionType, MultiplayerOptions.MultiplayerOptionsAccessMode mode = MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)
		{
			int num;
			MultiplayerOptions.Instance.GetOptionFromOptionType(optionType, mode).GetValue(out num);
			return num == 1;
		}

		// Token: 0x06002D34 RID: 11572 RVA: 0x000AE9C8 File Offset: 0x000ACBC8
		public static int GetIntValue(this MultiplayerOptions.OptionType optionType, MultiplayerOptions.MultiplayerOptionsAccessMode mode = MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)
		{
			int num;
			MultiplayerOptions.Instance.GetOptionFromOptionType(optionType, mode).GetValue(out num);
			return num;
		}

		// Token: 0x06002D35 RID: 11573 RVA: 0x000AE9EC File Offset: 0x000ACBEC
		public static string GetStrValue(this MultiplayerOptions.OptionType optionType, MultiplayerOptions.MultiplayerOptionsAccessMode mode = MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)
		{
			string text;
			MultiplayerOptions.Instance.GetOptionFromOptionType(optionType, mode).GetValue(out text);
			return text;
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x000AEA0D File Offset: 0x000ACC0D
		public static void SetValue(this MultiplayerOptions.OptionType optionType, bool value, MultiplayerOptions.MultiplayerOptionsAccessMode mode = MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)
		{
			MultiplayerOptions.Instance.GetOptionFromOptionType(optionType, mode).UpdateValue(value ? 1 : 0);
		}

		// Token: 0x06002D37 RID: 11575 RVA: 0x000AEA28 File Offset: 0x000ACC28
		public static void SetValue(this MultiplayerOptions.OptionType optionType, int value, MultiplayerOptions.MultiplayerOptionsAccessMode mode = MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)
		{
			MultiplayerOptions.Instance.GetOptionFromOptionType(optionType, mode).UpdateValue(value);
		}

		// Token: 0x06002D38 RID: 11576 RVA: 0x000AEA3D File Offset: 0x000ACC3D
		public static void SetValue(this MultiplayerOptions.OptionType optionType, string value, MultiplayerOptions.MultiplayerOptionsAccessMode mode = MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)
		{
			MultiplayerOptions.Instance.GetOptionFromOptionType(optionType, mode).UpdateValue(value);
		}

		// Token: 0x06002D39 RID: 11577 RVA: 0x000AEA52 File Offset: 0x000ACC52
		public static int GetMinimumValue(this MultiplayerOptions.OptionType optionType)
		{
			return optionType.GetOptionProperty().BoundsMin;
		}

		// Token: 0x06002D3A RID: 11578 RVA: 0x000AEA5F File Offset: 0x000ACC5F
		public static int GetMaximumValue(this MultiplayerOptions.OptionType optionType)
		{
			return optionType.GetOptionProperty().BoundsMax;
		}

		// Token: 0x06002D3B RID: 11579 RVA: 0x000AEA6C File Offset: 0x000ACC6C
		public static MultiplayerOptionsProperty GetOptionProperty(this MultiplayerOptions.OptionType optionType)
		{
			return (MultiplayerOptionsProperty)optionType.GetType().GetField(optionType.ToString()).GetCustomAttributesSafe(typeof(MultiplayerOptionsProperty), false)
				.Single<object>();
		}
	}
}
