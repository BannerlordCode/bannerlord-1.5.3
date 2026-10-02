using System;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu
{
	// Token: 0x020000A1 RID: 161
	public class GameMenuItemProgressVM : ViewModel
	{
		// Token: 0x06000F3B RID: 3899 RVA: 0x0003F7FE File Offset: 0x0003D9FE
		public void InitializeWith(MenuContext context, int virtualIndex)
		{
			this._context = context;
			this._virtualIndex = virtualIndex;
			this._gameMenuManager = Campaign.Current.GameMenuManager;
			this.RefreshValues();
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x0003F824 File Offset: 0x0003DA24
		public override void RefreshValues()
		{
			base.RefreshValues();
			this._text1 = Campaign.Current.GameMenuManager.GetVirtualMenuOptionText(this._context, this._virtualIndex).ToString();
			this._text2 = Campaign.Current.GameMenuManager.GetVirtualMenuOptionText2(this._context, this._virtualIndex).ToString();
			this.Refresh();
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x0003F88C File Offset: 0x0003DA8C
		private void Refresh()
		{
			switch (this._gameMenuManager.GetVirtualMenuAndOptionType(this._context))
			{
			case GameMenu.MenuAndOptionType.WaitMenuShowProgressAndHoursOption:
			{
				float num = Campaign.Current.GameMenuManager.GetVirtualMenuTargetWaitHours(this._context);
				num = (float)MathF.Round(num);
				if (num > 1f)
				{
					GameTexts.SetVariable("PLURAL_HOURS", 1);
				}
				else
				{
					GameTexts.SetVariable("PLURAL_HOURS", 0);
				}
				GameTexts.SetVariable("HOUR", num.ToString());
				this.ProgressText = GameTexts.FindText("str_hours", null).ToString();
				goto IL_00BB;
			}
			case GameMenu.MenuAndOptionType.WaitMenuShowOnlyProgressOption:
				this.ProgressText = "";
				goto IL_00BB;
			}
			Debug.FailedAssert("Shouldn't create game menu progress for normal options", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\GameMenu\\GameMenuItemProgressVM.cs", "Refresh", 69);
			return;
			IL_00BB:
			this.Text = (Campaign.Current.GameMenuManager.GetVirtualMenuIsWaitActive(this._context) ? this._text2 : this._text1);
			float virtualMenuProgress = Campaign.Current.GameMenuManager.GetVirtualMenuProgress(this._context);
			this.Progress = (float)MathF.Round(virtualMenuProgress * 100f);
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x0003F9AA File Offset: 0x0003DBAA
		public void OnTick()
		{
			this.Refresh();
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06000F3F RID: 3903 RVA: 0x0003F9B2 File Offset: 0x0003DBB2
		// (set) Token: 0x06000F40 RID: 3904 RVA: 0x0003F9BA File Offset: 0x0003DBBA
		[DataSourceProperty]
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				if (value != this._text)
				{
					this._text = value;
					base.OnPropertyChangedWithValue<string>(value, "Text");
				}
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06000F41 RID: 3905 RVA: 0x0003F9DD File Offset: 0x0003DBDD
		// (set) Token: 0x06000F42 RID: 3906 RVA: 0x0003F9E5 File Offset: 0x0003DBE5
		[DataSourceProperty]
		public string ProgressText
		{
			get
			{
				return this._progressText;
			}
			set
			{
				if (value != this._progressText)
				{
					this._progressText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProgressText");
				}
			}
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06000F43 RID: 3907 RVA: 0x0003FA08 File Offset: 0x0003DC08
		// (set) Token: 0x06000F44 RID: 3908 RVA: 0x0003FA10 File Offset: 0x0003DC10
		[DataSourceProperty]
		public float Progress
		{
			get
			{
				return this._progress;
			}
			set
			{
				if (value != this._progress)
				{
					this._progress = value;
					base.OnPropertyChangedWithValue(value, "Progress");
				}
			}
		}

		// Token: 0x040006D5 RID: 1749
		private MenuContext _context;

		// Token: 0x040006D6 RID: 1750
		private GameMenuManager _gameMenuManager;

		// Token: 0x040006D7 RID: 1751
		private int _virtualIndex;

		// Token: 0x040006D8 RID: 1752
		private string _text1 = "";

		// Token: 0x040006D9 RID: 1753
		private string _text2 = "";

		// Token: 0x040006DA RID: 1754
		private string _text;

		// Token: 0x040006DB RID: 1755
		private string _progressText;

		// Token: 0x040006DC RID: 1756
		private float _progress;
	}
}
