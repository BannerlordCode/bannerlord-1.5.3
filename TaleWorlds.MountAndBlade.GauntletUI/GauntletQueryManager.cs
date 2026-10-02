using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.ViewModelCollection.Inquiries;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x02000018 RID: 24
	public class GauntletQueryManager : GlobalLayer
	{
		// Token: 0x060000E2 RID: 226 RVA: 0x00007460 File Offset: 0x00005660
		public void Initialize()
		{
			if (!this._isInitialized)
			{
				this._isInitialized = true;
				this._inquiryQueue = new Queue<Tuple<Type, object, bool, bool>>();
				InformationManager.OnShowInquiry += this.CreateQuery;
				InformationManager.OnShowTextInquiry += this.CreateTextQuery;
				MBInformationManager.OnShowMultiSelectionInquiry += this.CreateMultiSelectionQuery;
				InformationManager.OnHideInquiry += this.CloseQuery;
				InformationManager.IsAnyInquiryActiveInternal = (Func<bool>)Delegate.Combine(InformationManager.IsAnyInquiryActiveInternal, new Func<bool>(this.OnIsAnyInquiryActiveQuery));
				this._singleQueryPopupVM = new SingleQueryPopUpVM(new Action(this.CloseQuery));
				this._multiSelectionQueryPopUpVM = new MultiSelectionQueryPopUpVM(new Action(this.CloseQuery));
				this._textQueryPopUpVM = new TextQueryPopUpVM(new Action(this.CloseQuery));
				this._gauntletLayer = new GauntletLayer("QueryManager", 19300, false);
				this._createQueryActions = new Dictionary<Type, Action<object, bool, bool>>
				{
					{
						typeof(InquiryData),
						delegate(object data, bool pauseState, bool prioritize)
						{
							this.CreateQuery((InquiryData)data, pauseState, prioritize);
						}
					},
					{
						typeof(TextInquiryData),
						delegate(object data, bool pauseState, bool prioritize)
						{
							this.CreateTextQuery((TextInquiryData)data, pauseState, prioritize);
						}
					},
					{
						typeof(MultiSelectionInquiryData),
						delegate(object data, bool pauseState, bool prioritize)
						{
							this.CreateMultiSelectionQuery((MultiSelectionInquiryData)data, pauseState, prioritize);
						}
					}
				};
				base.Layer = this._gauntletLayer;
				this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
				ScreenManager.AddGlobalLayer(this, true);
			}
			ScreenManager.SetSuspendLayer(base.Layer, true);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000075E1 File Offset: 0x000057E1
		private bool OnIsAnyInquiryActiveQuery()
		{
			return GauntletQueryManager._activeDataSource != null;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000075EC File Offset: 0x000057EC
		internal void InitializeKeyVisuals()
		{
			this._singleQueryPopupVM.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this._singleQueryPopupVM.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this._multiSelectionQueryPopUpVM.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this._multiSelectionQueryPopUpVM.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this._textQueryPopUpVM.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this._textQueryPopUpVM.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000076B4 File Offset: 0x000058B4
		protected override void OnEarlyTick(float dt)
		{
			base.OnEarlyTick(dt);
			if (GauntletQueryManager._activeDataSource != null && !this._isSuspendedBySceneNotification)
			{
				if (ScreenManager.FocusedLayer != base.Layer && (ScreenManager.FocusedLayer == null || ScreenManager.FocusedLayer.InputRestrictions.Order <= base.Layer.InputRestrictions.Order))
				{
					this.SetLayerFocus(true);
				}
				if (GauntletQueryManager._activeDataSource.IsButtonOkShown && GauntletQueryManager._activeDataSource.IsButtonOkEnabled && this._gauntletLayer.Input.IsHotKeyReleased("Confirm"))
				{
					UISoundsHelper.PlayUISound("event:/ui/panels/next");
					GauntletQueryManager._activeDataSource.ExecuteAffirmativeAction();
					return;
				}
				if (GauntletQueryManager._activeDataSource.IsButtonCancelShown && this._gauntletLayer.Input.IsHotKeyReleased("Exit"))
				{
					UISoundsHelper.PlayUISound("event:/ui/panels/next");
					GauntletQueryManager._activeDataSource.ExecuteNegativeAction();
				}
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00007794 File Offset: 0x00005994
		protected override void OnLateTick(float dt)
		{
			base.OnLateTick(dt);
			if (GauntletQueryManager._activeDataSource != null)
			{
				bool? isAnySceneNotificationActive = MBInformationManager.GetIsAnySceneNotificationActive();
				bool flag = true;
				bool flag2 = (isAnySceneNotificationActive.GetValueOrDefault() == flag) & (isAnySceneNotificationActive != null);
				if (flag2 != this._isSuspendedBySceneNotification)
				{
					this._isSuspendedBySceneNotification = flag2;
					this.SetLayerFocus(!this._isSuspendedBySceneNotification);
				}
				if (!this._isSuspendedBySceneNotification)
				{
					GauntletQueryManager._activeDataSource.OnTick(dt);
				}
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000077FC File Offset: 0x000059FC
		private void CreateQuery(InquiryData data, bool pauseGameActiveState, bool prioritize)
		{
			if (GauntletQueryManager._activeDataSource == null)
			{
				this._singleQueryPopupVM.SetData(data);
				this._movie = this._gauntletLayer.LoadMovie("SingleQueryPopup", this._singleQueryPopupVM);
				GauntletQueryManager._activeDataSource = this._singleQueryPopupVM;
				GauntletQueryManager._activeQueryData = data;
				this.HandleQueryCreated(data.SoundEventPath, pauseGameActiveState);
				return;
			}
			if (data == null)
			{
				Debug.FailedAssert("Trying to create query with null data!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\GauntletQueryManager.cs", "CreateQuery", 143);
				return;
			}
			if (GauntletQueryManager.CheckIfQueryDataIsEqual(GauntletQueryManager._activeQueryData, data) || this._inquiryQueue.Any<Tuple<Type, object, bool, bool>>((Tuple<Type, object, bool, bool> x) => GauntletQueryManager.CheckIfQueryDataIsEqual(x.Item2, data)))
			{
				Debug.FailedAssert("Trying to create query but it is already present! Title: " + data.TitleText, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\GauntletQueryManager.cs", "CreateQuery", 148);
				return;
			}
			if (prioritize)
			{
				this.QueueAndShowNewData(data, pauseGameActiveState, prioritize);
				return;
			}
			this._inquiryQueue.Enqueue(new Tuple<Type, object, bool, bool>(typeof(InquiryData), data, pauseGameActiveState, prioritize));
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00007924 File Offset: 0x00005B24
		private void CreateTextQuery(TextInquiryData data, bool pauseGameActiveState, bool prioritize)
		{
			if (GauntletQueryManager._activeDataSource == null)
			{
				this._textQueryPopUpVM.SetData(data);
				this._movie = this._gauntletLayer.LoadMovie("TextQueryPopup", this._textQueryPopUpVM);
				GauntletQueryManager._activeDataSource = this._textQueryPopUpVM;
				GauntletQueryManager._activeQueryData = data;
				this.HandleQueryCreated(data.SoundEventPath, pauseGameActiveState);
				return;
			}
			if (data == null)
			{
				Debug.FailedAssert("Trying to create textQuery with null data!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\GauntletQueryManager.cs", "CreateTextQuery", 178);
				return;
			}
			if (GauntletQueryManager.CheckIfQueryDataIsEqual(GauntletQueryManager._activeQueryData, data) || this._inquiryQueue.Any<Tuple<Type, object, bool, bool>>((Tuple<Type, object, bool, bool> x) => GauntletQueryManager.CheckIfQueryDataIsEqual(x.Item2, data)))
			{
				Debug.FailedAssert("Trying to create textQuery but it is already present! Title: " + data.TitleText, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\GauntletQueryManager.cs", "CreateTextQuery", 183);
				return;
			}
			if (prioritize)
			{
				this.QueueAndShowNewData(data, pauseGameActiveState, prioritize);
				return;
			}
			this._inquiryQueue.Enqueue(new Tuple<Type, object, bool, bool>(typeof(TextInquiryData), data, pauseGameActiveState, prioritize));
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00007A4C File Offset: 0x00005C4C
		private void CreateMultiSelectionQuery(MultiSelectionInquiryData data, bool pauseGameActiveState, bool prioritize)
		{
			if (GauntletQueryManager._activeDataSource == null)
			{
				this._multiSelectionQueryPopUpVM.SetData(data);
				this._movie = this._gauntletLayer.LoadMovie("MultiSelectionQueryPopup", this._multiSelectionQueryPopUpVM);
				GauntletQueryManager._activeDataSource = this._multiSelectionQueryPopUpVM;
				GauntletQueryManager._activeQueryData = data;
				this.HandleQueryCreated(data.SoundEventPath, pauseGameActiveState);
				return;
			}
			if (data == null)
			{
				Debug.FailedAssert("Trying to create multiSelectionQuery with null data!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\GauntletQueryManager.cs", "CreateMultiSelectionQuery", 213);
				return;
			}
			if (GauntletQueryManager.CheckIfQueryDataIsEqual(GauntletQueryManager._activeQueryData, data) || this._inquiryQueue.Any<Tuple<Type, object, bool, bool>>((Tuple<Type, object, bool, bool> x) => GauntletQueryManager.CheckIfQueryDataIsEqual(x.Item2, data)))
			{
				Debug.FailedAssert("Trying to create multiSelectionQuery but it is already present! Title: " + data.TitleText, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\GauntletQueryManager.cs", "CreateMultiSelectionQuery", 218);
				return;
			}
			if (prioritize)
			{
				this.QueueAndShowNewData(data, pauseGameActiveState, prioritize);
				return;
			}
			this._inquiryQueue.Enqueue(new Tuple<Type, object, bool, bool>(typeof(MultiSelectionInquiryData), data, pauseGameActiveState, prioritize));
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00007B74 File Offset: 0x00005D74
		private void QueueAndShowNewData(object newInquiryData, bool pauseGameActiveState, bool prioritize)
		{
			Queue<Tuple<Type, object, bool, bool>> queue = new Queue<Tuple<Type, object, bool, bool>>();
			queue.Enqueue(new Tuple<Type, object, bool, bool>(newInquiryData.GetType(), newInquiryData, pauseGameActiveState, prioritize));
			queue.Enqueue(new Tuple<Type, object, bool, bool>(GauntletQueryManager._activeQueryData.GetType(), GauntletQueryManager._activeQueryData, this._isLastActiveGameStatePaused, false));
			this._inquiryQueue = GauntletQueryManager.CombineQueues<Tuple<Type, object, bool, bool>>(queue, this._inquiryQueue);
			this.CloseQuery();
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00007BD4 File Offset: 0x00005DD4
		private void HandleQueryCreated(string soundEventPath, bool pauseGameActiveState)
		{
			InformationManager.HideTooltip();
			GauntletQueryManager._activeDataSource.ForceRefreshKeyVisuals();
			this.SetLayerFocus(true);
			this._isLastActiveGameStatePaused = pauseGameActiveState;
			if (this._isLastActiveGameStatePaused)
			{
				GameStateManager.Current.RegisterActiveStateDisableRequest(this);
				MBCommon.PauseGameEngine();
			}
			if (!string.IsNullOrEmpty(soundEventPath))
			{
				SoundEvent.PlaySound2D(soundEventPath);
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00007C28 File Offset: 0x00005E28
		private void CloseQuery()
		{
			if (GauntletQueryManager._activeDataSource == null)
			{
				return;
			}
			this.SetLayerFocus(false);
			this._isSuspendedBySceneNotification = false;
			if (this._isLastActiveGameStatePaused)
			{
				GameStateManager.Current.UnregisterActiveStateDisableRequest(this);
				MBCommon.UnPauseGameEngine();
			}
			if (this._movie != null)
			{
				this._gauntletLayer.ReleaseMovie(this._movie);
				this._movie = null;
			}
			GauntletQueryManager._activeDataSource.OnClearData();
			GauntletQueryManager._activeDataSource = null;
			GauntletQueryManager._activeQueryData = null;
			if (this._inquiryQueue.Count > 0)
			{
				Tuple<Type, object, bool, bool> tuple = this._inquiryQueue.Dequeue();
				Action<object, bool, bool> action;
				if (this._createQueryActions.TryGetValue(tuple.Item1, out action))
				{
					action(tuple.Item2, tuple.Item3, tuple.Item4);
					return;
				}
				Debug.FailedAssert("Invalid data type for query", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\GauntletQueryManager.cs", "CloseQuery", 311);
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00007CFC File Offset: 0x00005EFC
		private void SetLayerFocus(bool isFocused)
		{
			if (isFocused)
			{
				ScreenManager.SetSuspendLayer(base.Layer, false);
				base.Layer.IsFocusLayer = true;
				ScreenManager.TrySetFocus(base.Layer);
				base.Layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
				return;
			}
			base.Layer.InputRestrictions.ResetInputRestrictions();
			ScreenManager.SetSuspendLayer(base.Layer, true);
			base.Layer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(base.Layer);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00007D78 File Offset: 0x00005F78
		private static Queue<T> CombineQueues<T>(Queue<T> t1, Queue<T> t2)
		{
			Queue<T> queue = new Queue<T>();
			int num = t1.Count;
			for (int i = 0; i < num; i++)
			{
				queue.Enqueue(t1.Dequeue());
			}
			num = t2.Count;
			for (int j = 0; j < num; j++)
			{
				queue.Enqueue(t2.Dequeue());
			}
			return queue;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00007DCC File Offset: 0x00005FCC
		private static bool CheckIfQueryDataIsEqual(object queryData1, object queryData2)
		{
			InquiryData inquiryData;
			if ((inquiryData = queryData1 as InquiryData) != null)
			{
				return inquiryData.HasSameContentWith(queryData2);
			}
			TextInquiryData textInquiryData;
			if ((textInquiryData = queryData1 as TextInquiryData) != null)
			{
				return textInquiryData.HasSameContentWith(queryData2);
			}
			MultiSelectionInquiryData multiSelectionInquiryData;
			return (multiSelectionInquiryData = queryData1 as MultiSelectionInquiryData) != null && multiSelectionInquiryData.HasSameContentWith(queryData2);
		}

		// Token: 0x0400008E RID: 142
		private bool _isInitialized;

		// Token: 0x0400008F RID: 143
		private Queue<Tuple<Type, object, bool, bool>> _inquiryQueue;

		// Token: 0x04000090 RID: 144
		private bool _isLastActiveGameStatePaused;

		// Token: 0x04000091 RID: 145
		private bool _isSuspendedBySceneNotification;

		// Token: 0x04000092 RID: 146
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000093 RID: 147
		private SingleQueryPopUpVM _singleQueryPopupVM;

		// Token: 0x04000094 RID: 148
		private MultiSelectionQueryPopUpVM _multiSelectionQueryPopUpVM;

		// Token: 0x04000095 RID: 149
		private TextQueryPopUpVM _textQueryPopUpVM;

		// Token: 0x04000096 RID: 150
		private static PopUpBaseVM _activeDataSource;

		// Token: 0x04000097 RID: 151
		private static object _activeQueryData;

		// Token: 0x04000098 RID: 152
		private GauntletMovieIdentifier _movie;

		// Token: 0x04000099 RID: 153
		private Dictionary<Type, Action<object, bool, bool>> _createQueryActions;
	}
}
