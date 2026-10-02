using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.AdvancedStartOptions;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.AdvancedStartOptions
{
	// Token: 0x02000112 RID: 274
	public class AdvancedStartOptions
	{
		// Token: 0x06000D95 RID: 3477 RVA: 0x00062217 File Offset: 0x00060417
		public AdvancedStartOptions()
		{
			this._options = new List<AdvancedStartOption>();
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x0006222C File Offset: 0x0006042C
		public void Add(AdvancedStartOption option)
		{
			for (int i = 0; i < this._options.Count; i++)
			{
				if (this._options[i].StringId == option.StringId)
				{
					Debug.Print("Overriding start option id: " + option.StringId, 0, Debug.DebugColor.White, 17592186044416UL);
					this._options[i] = option;
					return;
				}
			}
			this._options.Add(option);
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x000622A8 File Offset: 0x000604A8
		public bool RemoveOption(string key)
		{
			for (int i = 0; i < this._options.Count; i++)
			{
				if (this._options[i].StringId == key)
				{
					this._options.RemoveAt(i);
					return true;
				}
			}
			Debug.FailedAssert("Trying to remove nonexistent option", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\AdvancedStartOptions\\AdvancedStartOptions.cs", "RemoveOption", 51);
			return false;
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x0006230C File Offset: 0x0006050C
		public AdvancedStartOption GetOption(string key)
		{
			for (int i = 0; i < this._options.Count; i++)
			{
				if (this._options[i].StringId == key)
				{
					return this._options[i];
				}
			}
			return null;
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00062358 File Offset: 0x00060558
		public TextObject GetOptionName(string key)
		{
			AdvancedStartOption advancedStartOption;
			if (this.HasValue(key, out advancedStartOption))
			{
				TextObject name = advancedStartOption.Value.GetName(Module.CurrentModule.GlobalTextManager);
				this.SetTextVariables(key, name);
				return name;
			}
			return TextObject.GetEmpty();
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x00062398 File Offset: 0x00060598
		public TextObject GetOptionDescription(string key)
		{
			AdvancedStartOption advancedStartOption;
			if (this.HasValue(key, out advancedStartOption))
			{
				TextObject description = advancedStartOption.Value.GetDescription(Module.CurrentModule.GlobalTextManager);
				this.SetTextVariables(key, description);
				return description;
			}
			return TextObject.GetEmpty();
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x000623D8 File Offset: 0x000605D8
		public TextObject GetItemDescription(string key)
		{
			AdvancedStartOption advancedStartOption;
			ListAdvancedStartOption listAdvancedStartOption;
			if (this.HasValue(key, out advancedStartOption) && (listAdvancedStartOption = advancedStartOption as ListAdvancedStartOption) != null)
			{
				TextObject listItemDescription = listAdvancedStartOption.GetListItemDescription(listAdvancedStartOption.GetValue<string>());
				this.SetTextVariables(key, listItemDescription);
				return listItemDescription;
			}
			return TextObject.GetEmpty();
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x00062418 File Offset: 0x00060618
		public TextObject GetItemName(string key, string identifier)
		{
			AdvancedStartOption advancedStartOption;
			ListAdvancedStartOption listAdvancedStartOption;
			if (this.HasValue(key, out advancedStartOption) && (listAdvancedStartOption = advancedStartOption as ListAdvancedStartOption) != null)
			{
				return listAdvancedStartOption.GetListItemName(identifier);
			}
			return TextObject.GetEmpty();
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00062448 File Offset: 0x00060648
		private void SetTextVariables(string key, TextObject text)
		{
			foreach (AdvancedStartOption advancedStartOption in this._options)
			{
				text.SetTextVariable(advancedStartOption.StringId, advancedStartOption.GetItemName());
			}
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x000624A8 File Offset: 0x000606A8
		private bool HasValue(string key, out AdvancedStartOption option)
		{
			option = this.GetOption(key);
			return option != null;
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x000624B8 File Offset: 0x000606B8
		public bool HasValue(string key)
		{
			AdvancedStartOption advancedStartOption;
			return this.HasValue(key, out advancedStartOption);
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x000624CE File Offset: 0x000606CE
		public T GetOption<T>(string key) where T : AdvancedStartOption
		{
			return this.GetOption(key) as T;
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x000624E4 File Offset: 0x000606E4
		public bool HasAnyChange()
		{
			foreach (AdvancedStartOption advancedStartOption in this._options)
			{
				if (advancedStartOption.HasValueChanged() && !advancedStartOption.GetIsHidden(this))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00062548 File Offset: 0x00060748
		public bool IsEmpty()
		{
			return this._options.Count == 0;
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x00062558 File Offset: 0x00060758
		public IReadOnlyList<AdvancedStartOption> GetAllOptions()
		{
			return this._options;
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x00062560 File Offset: 0x00060760
		public AdvancedStartOptionsData GetChangedOptions()
		{
			List<AdvancedStartData> list = new List<AdvancedStartData>();
			foreach (AdvancedStartOption advancedStartOption in this._options)
			{
				if (advancedStartOption.HasValueChanged() && !advancedStartOption.GetIsHidden(this))
				{
					list.Add(advancedStartOption.Value);
				}
			}
			return new AdvancedStartOptionsData(list);
		}

		// Token: 0x040005CC RID: 1484
		private readonly List<AdvancedStartOption> _options;
	}
}
