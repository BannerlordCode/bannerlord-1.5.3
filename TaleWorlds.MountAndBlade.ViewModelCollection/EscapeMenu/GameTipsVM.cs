using System;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu
{
	// Token: 0x02000081 RID: 129
	public class GameTipsVM : ViewModel
	{
		// Token: 0x06000AC4 RID: 2756 RVA: 0x0002664D File Offset: 0x0002484D
		public GameTipsVM(bool isAutoChangeEnabled, bool navigationButtonsEnabled)
		{
			this._navigationButtonsEnabled = navigationButtonsEnabled;
			this._isAutoChangeEnabled = isAutoChangeEnabled;
			this.RefreshValues();
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x00026674 File Offset: 0x00024874
		public override void RefreshValues()
		{
			base.RefreshValues();
			this._allTips = new MBList<string>();
			this.GameTipTitle = GameTexts.FindText("str_game_tip_title", null).ToString();
			float num = 0.8f;
			string keyHyperlinkText = HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("Generic", 4), num);
			GameTexts.SetVariable("LEAVE_AREA_KEY", keyHyperlinkText);
			string keyHyperlinkText2 = HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("Generic", 5), num);
			GameTexts.SetVariable("MISSION_INDICATORS_KEY", keyHyperlinkText2);
			GameTexts.SetVariable("EXTEND_KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("MapHotKeyCategory", "MapFollowModifier"), num));
			GameTexts.SetVariable("ENCYCLOPEDIA_SHORTCUT", HyperlinkTexts.GetKeyHyperlinkText("RightMouseButton", num));
			if (Input.IsMouseActive)
			{
				foreach (TextObject textObject in GameTexts.FindAllTextVariations("str_game_tip_pc"))
				{
					this._allTips.Add(textObject.ToString());
				}
			}
			foreach (TextObject textObject2 in GameTexts.FindAllTextVariations("str_game_tip"))
			{
				this._allTips.Add(textObject2.ToString());
			}
			this.NavigationButtonsEnabled = this._allTips.Count > 1;
			this.CurrentTip = ((this._allTips.Count == 0) ? string.Empty : this._allTips.GetRandomElement<string>());
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x000267F8 File Offset: 0x000249F8
		public void ExecutePreviousTip()
		{
			this._currentTipIndex--;
			if (this._currentTipIndex < 0)
			{
				this._currentTipIndex = this._allTips.Count - 1;
			}
			this.CurrentTip = this._allTips[this._currentTipIndex];
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x00026846 File Offset: 0x00024A46
		public void ExecuteNextTip()
		{
			this._currentTipIndex = (this._currentTipIndex + 1) % this._allTips.Count;
			this.CurrentTip = this._allTips[this._currentTipIndex];
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x00026879 File Offset: 0x00024A79
		public void OnTick(float dt)
		{
			if (this._isAutoChangeEnabled)
			{
				this._totalDt += dt;
				if (this._totalDt > this._tipTimeInterval)
				{
					this.ExecuteNextTip();
					this._totalDt = 0f;
				}
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x000268B0 File Offset: 0x00024AB0
		// (set) Token: 0x06000ACA RID: 2762 RVA: 0x000268B8 File Offset: 0x00024AB8
		[DataSourceProperty]
		public string CurrentTip
		{
			get
			{
				return this._currentTip;
			}
			set
			{
				if (value != this._currentTip)
				{
					this._currentTip = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentTip");
				}
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000ACB RID: 2763 RVA: 0x000268DB File Offset: 0x00024ADB
		// (set) Token: 0x06000ACC RID: 2764 RVA: 0x000268E3 File Offset: 0x00024AE3
		[DataSourceProperty]
		public string GameTipTitle
		{
			get
			{
				return this._gameTipTitle;
			}
			set
			{
				if (value != this._gameTipTitle)
				{
					this._gameTipTitle = value;
					base.OnPropertyChangedWithValue<string>(value, "GameTipTitle");
				}
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000ACD RID: 2765 RVA: 0x00026906 File Offset: 0x00024B06
		// (set) Token: 0x06000ACE RID: 2766 RVA: 0x0002690E File Offset: 0x00024B0E
		[DataSourceProperty]
		public bool NavigationButtonsEnabled
		{
			get
			{
				return this._navigationButtonsEnabled;
			}
			set
			{
				if (value != this._navigationButtonsEnabled)
				{
					this._navigationButtonsEnabled = value;
					base.OnPropertyChangedWithValue(value, "NavigationButtonsEnabled");
				}
			}
		}

		// Token: 0x040004F8 RID: 1272
		private MBList<string> _allTips;

		// Token: 0x040004F9 RID: 1273
		private readonly float _tipTimeInterval = 5f;

		// Token: 0x040004FA RID: 1274
		private readonly bool _isAutoChangeEnabled;

		// Token: 0x040004FB RID: 1275
		private int _currentTipIndex;

		// Token: 0x040004FC RID: 1276
		private float _totalDt;

		// Token: 0x040004FD RID: 1277
		private string _currentTip;

		// Token: 0x040004FE RID: 1278
		private string _gameTipTitle;

		// Token: 0x040004FF RID: 1279
		private bool _navigationButtonsEnabled;
	}
}
