using System;
using TaleWorlds.CampaignSystem.AdvancedStartOptions;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.AdvancedStartOptions
{
	// Token: 0x02000111 RID: 273
	public abstract class AdvancedStartOption
	{
		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000D8A RID: 3466 RVA: 0x00062057 File Offset: 0x00060257
		public string StringId
		{
			get
			{
				return this._defaultValue.StringId;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000D8B RID: 3467 RVA: 0x00062064 File Offset: 0x00060264
		public string CategoryId
		{
			get
			{
				return this._defaultValue.CategoryId;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000D8C RID: 3468 RVA: 0x00062071 File Offset: 0x00060271
		public AdvancedStartData Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000D8D RID: 3469 RVA: 0x00062079 File Offset: 0x00060279
		public AdvancedStartData DefaultValue
		{
			get
			{
				return this._defaultValue;
			}
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x00062081 File Offset: 0x00060281
		public virtual bool HasValueChanged()
		{
			return !object.Equals(this._value.GetData(), this._defaultValue.GetData());
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x000620A4 File Offset: 0x000602A4
		public void SetValue<T>(T data)
		{
			AdvancedStartData<T> advancedStartData;
			if ((advancedStartData = this._value as AdvancedStartData<T>) != null)
			{
				advancedStartData.Value = data;
				return;
			}
			Debug.FailedAssert("Wrong generic type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\AdvancedStartOptions\\AdvancedStartOption.cs", "SetValue", 38);
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x000620E0 File Offset: 0x000602E0
		public T GetValue<T>()
		{
			AdvancedStartData<T> advancedStartData;
			if ((advancedStartData = this._value as AdvancedStartData<T>) != null)
			{
				return advancedStartData.Value;
			}
			Debug.FailedAssert("Wrong generic type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\AdvancedStartOptions\\AdvancedStartOption.cs", "GetValue", 50);
			return default(T);
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x00062124 File Offset: 0x00060324
		public T GetDefaultValue<T>()
		{
			AdvancedStartData<T> advancedStartData;
			if ((advancedStartData = this._defaultValue as AdvancedStartData<T>) != null)
			{
				return advancedStartData.Value;
			}
			Debug.FailedAssert("Wrong generic type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\AdvancedStartOptions\\AdvancedStartOption.cs", "GetDefaultValue", 63);
			return default(T);
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x00062168 File Offset: 0x00060368
		internal TextObject GetItemName()
		{
			object data = this.Value.GetData();
			if (data == null)
			{
				return TextObject.GetEmpty();
			}
			string text;
			if ((text = data as string) != null)
			{
				return Module.CurrentModule.GlobalTextManager.FindText("str_campaign_starting_options_item_name", text);
			}
			object obj;
			if ((obj = data) is bool)
			{
				return new TextObject(((bool)obj) ? "1" : "0", null);
			}
			return new TextObject(data.ToString(), null);
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x000621DD File Offset: 0x000603DD
		protected AdvancedStartOption(AdvancedStartData defaultValue, AdvancedStartOption.AdvancedStartOptionCondition onCondition)
		{
			this._onCondition = onCondition;
			this._value = defaultValue.Clone();
			this._defaultValue = defaultValue;
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x000621FF File Offset: 0x000603FF
		public bool GetIsHidden(AdvancedStartOptions options)
		{
			return this._onCondition != null && this._onCondition(options);
		}

		// Token: 0x040005C8 RID: 1480
		private const string ItemTextId = "str_campaign_starting_options_item_name";

		// Token: 0x040005C9 RID: 1481
		private readonly AdvancedStartOption.AdvancedStartOptionCondition _onCondition;

		// Token: 0x040005CA RID: 1482
		private readonly AdvancedStartData _value;

		// Token: 0x040005CB RID: 1483
		private readonly AdvancedStartData _defaultValue;

		// Token: 0x0200023D RID: 573
		// (Invoke) Token: 0x0600147F RID: 5247
		public delegate bool AdvancedStartOptionCondition(AdvancedStartOptions options);
	}
}
