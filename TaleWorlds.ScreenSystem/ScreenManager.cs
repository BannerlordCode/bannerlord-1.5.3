using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.ScreenSystem
{
	// Token: 0x02000009 RID: 9
	public static class ScreenManager
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00003006 File Offset: 0x00001206
		public static IScreenManagerEngineConnection EngineInterface
		{
			get
			{
				return ScreenManager._engineInterface;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000093 RID: 147 RVA: 0x0000300D File Offset: 0x0000120D
		// (set) Token: 0x06000094 RID: 148 RVA: 0x00003014 File Offset: 0x00001214
		public static float Scale { get; private set; } = 1f;

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000095 RID: 149 RVA: 0x0000301C File Offset: 0x0000121C
		// (set) Token: 0x06000096 RID: 150 RVA: 0x00003023 File Offset: 0x00001223
		public static Vec2 UsableArea
		{
			get
			{
				return ScreenManager._usableArea;
			}
			private set
			{
				if (value != ScreenManager._usableArea)
				{
					ScreenManager._usableArea = value;
					ScreenManager.OnUsableAreaChanged(ScreenManager._usableArea);
				}
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00003042 File Offset: 0x00001242
		public static bool IsEnterButtonRDown
		{
			get
			{
				return ScreenManager._engineInterface.GetIsEnterButtonRDown();
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000098 RID: 152 RVA: 0x0000304E File Offset: 0x0000124E
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00003055 File Offset: 0x00001255
		public static bool IsLateTickInProgress { get; private set; }

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600009A RID: 154 RVA: 0x00003060 File Offset: 0x00001260
		// (remove) Token: 0x0600009B RID: 155 RVA: 0x00003094 File Offset: 0x00001294
		public static event ScreenManager.OnPushScreenEvent OnPushScreen;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600009C RID: 156 RVA: 0x000030C8 File Offset: 0x000012C8
		// (remove) Token: 0x0600009D RID: 157 RVA: 0x000030FC File Offset: 0x000012FC
		public static event ScreenManager.OnPopScreenEvent OnPopScreen;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600009E RID: 158 RVA: 0x00003130 File Offset: 0x00001330
		// (remove) Token: 0x0600009F RID: 159 RVA: 0x00003164 File Offset: 0x00001364
		public static event ScreenManager.OnControllerDisconnectedEvent OnControllerDisconnected;

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00003198 File Offset: 0x00001398
		public static List<ScreenLayer> SortedLayers
		{
			get
			{
				if (!ScreenManager._isSortedActiveLayersDirty)
				{
					int count = ScreenManager._sortedLayers.Count;
					ScreenBase topScreen = ScreenManager.TopScreen;
					int? num = ((topScreen != null) ? new int?(topScreen.Layers.Count) : null);
					ObservableCollection<GlobalLayer> globalLayers = ScreenManager._globalLayers;
					int? num2 = num + ((globalLayers != null) ? new int?(globalLayers.Count) : null);
					if ((count == num2.GetValueOrDefault()) & (num2 != null))
					{
						goto IL_013F;
					}
				}
				ScreenManager._sortedLayers.Clear();
				if (ScreenManager.TopScreen != null)
				{
					for (int i = 0; i < ScreenManager.TopScreen.Layers.Count; i++)
					{
						ScreenLayer screenLayer = ScreenManager.TopScreen.Layers[i];
						if (screenLayer != null)
						{
							ScreenManager._sortedLayers.Add(screenLayer);
						}
					}
				}
				foreach (GlobalLayer globalLayer in ScreenManager._globalLayers)
				{
					ScreenManager._sortedLayers.Add(globalLayer.Layer);
				}
				ScreenManager._sortedLayers.Sort();
				ScreenManager._isSortedActiveLayersDirty = false;
				IL_013F:
				return ScreenManager._sortedLayers;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x000032FC File Offset: 0x000014FC
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00003303 File Offset: 0x00001503
		public static ScreenBase TopScreen { get; private set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x0000330B File Offset: 0x0000150B
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00003312 File Offset: 0x00001512
		public static ScreenLayer FocusedLayer { get; private set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x0000331A File Offset: 0x0000151A
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x00003321 File Offset: 0x00001521
		public static ScreenLayer FirstHitLayer { get; private set; }

		// Token: 0x060000A7 RID: 167 RVA: 0x0000332C File Offset: 0x0000152C
		static ScreenManager()
		{
			ScreenManager._screenList.CollectionChanged += ScreenManager.OnScreenListChanged;
			ScreenManager._globalLayers.CollectionChanged += ScreenManager.OnGlobalListChanged;
			ScreenLayer.OnLayerActiveStateChanged += ScreenManager.OnLayerActiveStateChanged;
			ScreenManager.FocusedLayer = null;
			ScreenManager.FirstHitLayer = null;
			ScreenManager._isWindowFocused = true;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000033EE File Offset: 0x000015EE
		private static void OnLayerActiveStateChanged(ScreenLayer layer)
		{
			ScreenManager.SetSortedLayersDirty();
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000033F5 File Offset: 0x000015F5
		public static void Initialize(IScreenManagerEngineConnection engineInterface)
		{
			ScreenManager._engineInterface = engineInterface;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003400 File Offset: 0x00001600
		internal static void RefreshGlobalOrder()
		{
			if (!ScreenManager._isRefreshActive)
			{
				ScreenManager._isRefreshActive = true;
				int num = -2000;
				int num2 = 10000;
				for (int i = 0; i < ScreenManager.SortedLayers.Count; i++)
				{
					if (ScreenManager.SortedLayers[i] != null)
					{
						if (!ScreenManager.SortedLayers[i].IsFinalized)
						{
							ScreenLayer screenLayer = ScreenManager.SortedLayers[i];
							if (screenLayer != null && screenLayer.IsActive)
							{
								ScreenLayer screenLayer2 = ScreenManager.SortedLayers[i];
								if (screenLayer2 != null)
								{
									screenLayer2.RefreshGlobalOrder(ref num);
								}
							}
							else
							{
								ScreenLayer screenLayer3 = ScreenManager.SortedLayers[i];
								if (screenLayer3 != null)
								{
									screenLayer3.RefreshGlobalOrder(ref num2);
								}
							}
						}
						ScreenManager._globalOrderDirty = false;
					}
				}
				ScreenManager._isRefreshActive = false;
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000034B8 File Offset: 0x000016B8
		public static void RemoveGlobalLayer(GlobalLayer layer, bool finalizeLayer = true)
		{
			Debug.Print("RemoveGlobalLayer", 0, Debug.DebugColor.White, 17592186044416UL);
			ScreenManager._globalLayers.Remove(layer);
			layer.Layer.HandleDeactivate();
			if (finalizeLayer)
			{
				layer.Layer.HandleFinalize();
			}
			ScreenManager._globalOrderDirty = true;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003508 File Offset: 0x00001708
		public static void AddGlobalLayer(GlobalLayer layer, bool isFocusable)
		{
			Debug.Print("AddGlobalLayer", 0, Debug.DebugColor.White, 17592186044416UL);
			int num = ScreenManager._globalLayers.Count;
			for (int i = 0; i < ScreenManager._globalLayers.Count; i++)
			{
				if (ScreenManager._globalLayers[i].Layer.InputRestrictions.Order >= layer.Layer.InputRestrictions.Order)
				{
					num = i;
					break;
				}
			}
			ScreenManager._globalLayers.Insert(num, layer);
			layer.Layer.HandleActivate();
			ScreenManager._globalOrderDirty = true;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00003598 File Offset: 0x00001798
		public static void OnConstrainStateChanged(bool isConstrained)
		{
			Debug.Print("OnConstrainStateChanged: " + isConstrained.ToString(), 0, Debug.DebugColor.White, 17592186044416UL);
			ScreenManager.OnGameWindowFocusChange(!isConstrained);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000035C8 File Offset: 0x000017C8
		public static bool ScreenTypeExistsAtList(ScreenBase screen)
		{
			Type type = screen.GetType();
			using (IEnumerator<ScreenBase> enumerator = ScreenManager._screenList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetType() == type)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003628 File Offset: 0x00001828
		public static void UpdateLayout()
		{
			foreach (GlobalLayer globalLayer in ScreenManager._globalLayers)
			{
				globalLayer.UpdateLayout();
			}
			foreach (ScreenBase screenBase in ScreenManager._screenList)
			{
				screenBase.UpdateLayout();
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000036AC File Offset: 0x000018AC
		public static void SetSuspendLayer(ScreenLayer layer, bool isSuspended)
		{
			if (isSuspended)
			{
				layer.HandleDeactivate();
			}
			else
			{
				layer.HandleActivate();
			}
			layer.LastActiveState = !isSuspended;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000036CC File Offset: 0x000018CC
		public static void OnFinalize()
		{
			ScreenManager.DeactivateAndFinalizeAllScreens();
			ScreenManager._screenList.CollectionChanged -= ScreenManager.OnScreenListChanged;
			ScreenManager._globalLayers.CollectionChanged -= ScreenManager.OnGlobalListChanged;
			ScreenLayer.OnLayerActiveStateChanged -= ScreenManager.OnLayerActiveStateChanged;
			ScreenManager._screenList = null;
			ScreenManager._globalLayers = null;
			ScreenManager.FocusedLayer = null;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003730 File Offset: 0x00001930
		private static void DeactivateAndFinalizeAllScreens()
		{
			if (!TWParallel.IsMainThread())
			{
				Debug.FailedAssert("Screen should be changed from main thread", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.ScreenSystem\\ScreenManager.cs", "DeactivateAndFinalizeAllScreens", 296);
			}
			Debug.Print("DeactivateAndFinalizeAllScreens", 0, Debug.DebugColor.White, 17592186044416UL);
			for (int i = ScreenManager._screenList.Count - 1; i >= 0; i--)
			{
				ScreenManager._screenList[i].HandlePause();
				ScreenManager._screenList[i].HandleDeactivate();
				ScreenManager._screenList[i].HandleFinalize();
				ScreenManager.OnPopScreenEvent onPopScreen = ScreenManager.OnPopScreen;
				if (onPopScreen != null)
				{
					onPopScreen(ScreenManager._screenList[i]);
				}
				ScreenManager._screenList.RemoveAt(i);
			}
			Common.MemoryCleanupGC(false);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000037E8 File Offset: 0x000019E8
		public static void Tick(float dt)
		{
			if (ScreenManager.DisableScreenManagerTicks)
			{
				return;
			}
			for (int i = 0; i < ScreenManager._globalLayers.Count; i++)
			{
				GlobalLayer globalLayer = ScreenManager._globalLayers[i];
				if (globalLayer != null)
				{
					globalLayer.EarlyTick(dt);
				}
			}
			ScreenManager.Update();
			if (ScreenManager.TopScreen != null)
			{
				ScreenManager.TopScreen.FrameTick(dt);
				ScreenBase screenBase = ScreenManager.FindPredecessor(ScreenManager.TopScreen);
				if (screenBase != null)
				{
					screenBase.IdleTick(dt);
				}
			}
			for (int j = 0; j < ScreenManager.SortedLayers.Count; j++)
			{
				ScreenLayer screenLayer = ScreenManager.SortedLayers[j];
				if (screenLayer != null && screenLayer.IsActive && !screenLayer.IsFinalized)
				{
					screenLayer.Tick(dt);
				}
			}
			for (int k = 0; k < ScreenManager._globalLayers.Count; k++)
			{
				GlobalLayer globalLayer2 = ScreenManager._globalLayers[k];
				if (globalLayer2 != null)
				{
					globalLayer2.Tick(dt);
				}
			}
			ScreenManager.LateUpdate(dt);
			for (int l = 0; l < ScreenManager._globalLayers.Count; l++)
			{
				GlobalLayer globalLayer3 = ScreenManager._globalLayers[l];
				if (globalLayer3 != null)
				{
					globalLayer3.LateTick(dt);
				}
			}
			if (ScreenManager.TopScreen != null)
			{
				ScreenManager.TopScreen.PostFrameTick(dt);
			}
			ScreenManager.ShowScreenDebugInformation();
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003910 File Offset: 0x00001B10
		public static void LateTick(float dt)
		{
			ScreenManager.IsLateTickInProgress = true;
			for (int i = 0; i < ScreenManager.SortedLayers.Count; i++)
			{
				ScreenLayer screenLayer = ScreenManager.SortedLayers[i];
				if (screenLayer != null && screenLayer.IsActive && !screenLayer.IsFinalized)
				{
					screenLayer.RenderTick(dt);
				}
			}
			for (int j = 0; j < ScreenManager.SortedLayers.Count; j++)
			{
				ScreenLayer screenLayer2 = ScreenManager.SortedLayers[j];
				if (screenLayer2 != null && screenLayer2.IsFocusLayer)
				{
					screenLayer2.Input.UnregisterReleasedKeys();
				}
			}
			ScreenManager.IsLateTickInProgress = false;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000399B File Offset: 0x00001B9B
		public static bool OnPlatformScreenKeyboardRequested(string initialText, string descriptionText, int maxLength, int keyboardTypeEnum)
		{
			ScreenManager.OnPlatformTextRequestedDelegate platformTextRequested = ScreenManager.PlatformTextRequested;
			return platformTextRequested != null && platformTextRequested(initialText, descriptionText, maxLength, keyboardTypeEnum);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000039B1 File Offset: 0x00001BB1
		public static void OnOnscreenKeyboardDone(string inputText)
		{
			Input.IsOnScreenKeyboardActive = false;
			ScreenLayer focusedLayer = ScreenManager.FocusedLayer;
			if (focusedLayer == null)
			{
				return;
			}
			focusedLayer.OnOnScreenKeyboardDone(inputText);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000039C9 File Offset: 0x00001BC9
		public static void OnOnscreenKeyboardCanceled()
		{
			Input.IsOnScreenKeyboardActive = false;
			ScreenLayer focusedLayer = ScreenManager.FocusedLayer;
			if (focusedLayer == null)
			{
				return;
			}
			focusedLayer.OnOnScreenKeyboardCanceled();
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000039E0 File Offset: 0x00001BE0
		public static void OnGameWindowFocusChange(bool focusGained)
		{
			ScreenManager._isWindowFocused = focusGained;
			if (ScreenManager._isWindowFocused)
			{
				ScreenManager._activeMouseVisible = ScreenManager.EngineInterface.GetMouseVisible();
			}
			Debug.Print("OnGameWindowFocusChange: " + ScreenManager._isWindowFocused.ToString(), 0, Debug.DebugColor.White, 17592186044416UL);
			string text = "TopScreen: ";
			ScreenBase topScreen = ScreenManager.TopScreen;
			string text2;
			if (topScreen == null)
			{
				text2 = null;
			}
			else
			{
				Type type = topScreen.GetType();
				text2 = ((type != null) ? type.Name : null);
			}
			Debug.Print(text + text2, 0, Debug.DebugColor.White, 17592186044416UL);
			bool flag = false;
			if (!Debugger.IsAttached && !flag)
			{
				ScreenBase topScreen2 = ScreenManager.TopScreen;
				if (topScreen2 != null)
				{
					topScreen2.OnFocusChangeOnGameWindow(focusGained);
				}
			}
			if (focusGained)
			{
				Action focusGained2 = ScreenManager.FocusGained;
				if (focusGained2 != null)
				{
					focusGained2();
				}
			}
			ScreenLayer focusedLayer = ScreenManager.FocusedLayer;
			if (focusedLayer == null)
			{
				return;
			}
			focusedLayer.Input.ResetLastDownKeys();
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060000B9 RID: 185 RVA: 0x00003AAC File Offset: 0x00001CAC
		// (remove) Token: 0x060000BA RID: 186 RVA: 0x00003AE0 File Offset: 0x00001CE0
		public static event Action FocusGained;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060000BB RID: 187 RVA: 0x00003B14 File Offset: 0x00001D14
		// (remove) Token: 0x060000BC RID: 188 RVA: 0x00003B48 File Offset: 0x00001D48
		public static event ScreenManager.OnPlatformTextRequestedDelegate PlatformTextRequested;

		// Token: 0x060000BD RID: 189 RVA: 0x00003B7C File Offset: 0x00001D7C
		public static void ReplaceTopScreen(ScreenBase screen)
		{
			Debug.Print("ReplaceToTopScreen", 0, Debug.DebugColor.White, 17592186044416UL);
			if (ScreenManager._screenList.Count > 0)
			{
				ScreenManager.TopScreen.HandlePause();
				ScreenManager.TopScreen.HandleDeactivate();
				ScreenManager.TopScreen.HandleFinalize();
				ScreenManager.OnPopScreenEvent onPopScreen = ScreenManager.OnPopScreen;
				if (onPopScreen != null)
				{
					onPopScreen(ScreenManager.TopScreen);
				}
				ScreenManager._screenList.Remove(ScreenManager.TopScreen);
			}
			ScreenManager._screenList.Add(screen);
			screen.HandleInitialize();
			screen.HandleActivate();
			screen.HandleResume();
			ScreenManager._globalOrderDirty = true;
			ScreenManager.OnPushScreenEvent onPushScreen = ScreenManager.OnPushScreen;
			if (onPushScreen == null)
			{
				return;
			}
			onPushScreen(screen);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00003C24 File Offset: 0x00001E24
		public static List<ScreenLayer> GetPersistentInputRestrictions()
		{
			List<ScreenLayer> list = new List<ScreenLayer>();
			foreach (GlobalLayer globalLayer in ScreenManager._globalLayers)
			{
				list.Add(globalLayer.Layer);
			}
			return list;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00003C7C File Offset: 0x00001E7C
		public static void SetAndActivateRootScreen(ScreenBase screen)
		{
			Debug.Print("SetAndActivateRootScreen", 0, Debug.DebugColor.White, 17592186044416UL);
			if (ScreenManager.TopScreen != null)
			{
				throw new Exception("TopScreen is not null.");
			}
			ScreenManager._screenList.Add(screen);
			screen.HandleInitialize();
			screen.HandleActivate();
			screen.HandleResume();
			ScreenManager._globalOrderDirty = true;
			ScreenManager.OnPushScreenEvent onPushScreen = ScreenManager.OnPushScreen;
			if (onPushScreen == null)
			{
				return;
			}
			onPushScreen(screen);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00003CE4 File Offset: 0x00001EE4
		public static void CleanAndPushScreen(ScreenBase screen)
		{
			if (!TWParallel.IsMainThread())
			{
				Debug.FailedAssert("Screen should be changed from main thread", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.ScreenSystem\\ScreenManager.cs", "CleanAndPushScreen", 522);
			}
			Debug.Print("CleanAndPushScreen", 0, Debug.DebugColor.White, 17592186044416UL);
			ScreenManager.DeactivateAndFinalizeAllScreens();
			ScreenManager._screenList.Add(screen);
			screen.HandleInitialize();
			screen.HandleActivate();
			screen.HandleResume();
			ScreenManager._globalOrderDirty = true;
			ScreenManager.OnPushScreenEvent onPushScreen = ScreenManager.OnPushScreen;
			if (onPushScreen == null)
			{
				return;
			}
			onPushScreen(screen);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00003D60 File Offset: 0x00001F60
		[CommandLineFunctionality.CommandLineArgumentFunction("cb_clear_siege_machine_selection", "ui")]
		public static string ClearSiegeMachineSelection(List<string> args)
		{
			ScreenBase screenBase = ScreenManager._screenList.FirstOrDefault<ScreenBase>((ScreenBase x) => x.GetType().GetMethod("ClearSiegeMachineSelections") != null);
			if (screenBase != null)
			{
				screenBase.GetType().GetMethod("ClearSiegeMachineSelections").Invoke(screenBase, null);
			}
			return "Siege machine selections have been cleared.";
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00003DB8 File Offset: 0x00001FB8
		[CommandLineFunctionality.CommandLineArgumentFunction("cb_copy_battle_layout_to_clipboard", "ui")]
		public static string CopyCustomBattle(List<string> args)
		{
			ScreenBase screenBase = ScreenManager._screenList.FirstOrDefault<ScreenBase>((ScreenBase x) => x.GetType().GetMethod("CopyBattleLayoutToClipboard") != null);
			if (screenBase != null)
			{
				screenBase.GetType().GetMethod("CopyBattleLayoutToClipboard").Invoke(screenBase, null);
				return "Custom battle layout has been copied to clipboard as text.";
			}
			return "Something went wrong";
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00003E18 File Offset: 0x00002018
		[CommandLineFunctionality.CommandLineArgumentFunction("cb_apply_battle_layout_from_string", "ui")]
		public static string ApplyCustomBattleLayout(List<string> args)
		{
			ScreenBase screenBase = ScreenManager._screenList.FirstOrDefault<ScreenBase>((ScreenBase x) => x.GetType().GetMethod("ApplyCustomBattleLayout") != null);
			if (screenBase == null || args.Count <= 0)
			{
				return "Something went wrong.";
			}
			string text = args.Aggregate<string>((string i, string j) => i + " " + j);
			if (text.Count<char>() > 5)
			{
				screenBase.GetType().GetMethod("ApplyCustomBattleLayout").Invoke(screenBase, new object[] { text });
				return "Applied new layout from text.";
			}
			return "Argument is not right.";
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00003EBC File Offset: 0x000020BC
		public static void PushScreen(ScreenBase screen)
		{
			if (!TWParallel.IsMainThread())
			{
				Debug.FailedAssert("Screen should be changed from main thread", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.ScreenSystem\\ScreenManager.cs", "PushScreen", 599);
			}
			Debug.Print("PushScreen", 0, Debug.DebugColor.White, 17592186044416UL);
			if (ScreenManager._screenList.Count > 0)
			{
				ScreenManager.TopScreen.HandlePause();
				if (ScreenManager.TopScreen.IsActive)
				{
					ScreenManager.TopScreen.HandleDeactivate();
				}
			}
			ScreenManager._screenList.Add(screen);
			screen.HandleInitialize();
			screen.HandleActivate();
			screen.HandleResume();
			ScreenManager._globalOrderDirty = true;
			ScreenManager.OnPushScreenEvent onPushScreen = ScreenManager.OnPushScreen;
			if (onPushScreen == null)
			{
				return;
			}
			onPushScreen(screen);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00003F60 File Offset: 0x00002160
		public static void PopScreen()
		{
			if (!TWParallel.IsMainThread())
			{
				Debug.FailedAssert("Screen should be changed from main thread", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.ScreenSystem\\ScreenManager.cs", "PopScreen", 630);
			}
			Debug.Print("PopScreen", 0, Debug.DebugColor.White, 17592186044416UL);
			if (ScreenManager._screenList.Count > 0)
			{
				ScreenManager.TopScreen.HandlePause();
				ScreenManager.TopScreen.HandleDeactivate();
				ScreenManager.TopScreen.HandleFinalize();
				Debug.Print("PopScreen - " + ScreenManager.TopScreen.GetType().ToString(), 0, Debug.DebugColor.White, 17592186044416UL);
				ScreenManager.OnPopScreenEvent onPopScreen = ScreenManager.OnPopScreen;
				if (onPopScreen != null)
				{
					onPopScreen(ScreenManager.TopScreen);
				}
				ScreenManager._screenList.Remove(ScreenManager.TopScreen);
			}
			if (ScreenManager._screenList.Count > 0)
			{
				ScreenBase topScreen = ScreenManager.TopScreen;
				ScreenManager.TopScreen.HandleActivate();
				if (topScreen == ScreenManager.TopScreen)
				{
					ScreenManager.TopScreen.HandleResume();
				}
			}
			ScreenManager._globalOrderDirty = true;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00004050 File Offset: 0x00002250
		public static void CleanScreens()
		{
			if (!TWParallel.IsMainThread())
			{
				Debug.FailedAssert("Screen should be changed from main thread", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.ScreenSystem\\ScreenManager.cs", "CleanScreens", 664);
			}
			Debug.Print("CleanScreens", 0, Debug.DebugColor.White, 17592186044416UL);
			while (ScreenManager._screenList.Count > 0)
			{
				ScreenManager.TopScreen.HandlePause();
				ScreenManager.TopScreen.HandleDeactivate();
				ScreenManager.TopScreen.HandleFinalize();
				ScreenManager.OnPopScreenEvent onPopScreen = ScreenManager.OnPopScreen;
				if (onPopScreen != null)
				{
					onPopScreen(ScreenManager.TopScreen);
				}
				ScreenManager._screenList.Remove(ScreenManager.TopScreen);
			}
			ScreenManager._globalOrderDirty = true;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000040EC File Offset: 0x000022EC
		private static ScreenBase FindPredecessor(ScreenBase screen)
		{
			ScreenBase screenBase = null;
			int num = ScreenManager._screenList.IndexOf(screen);
			if (num > 0)
			{
				screenBase = ScreenManager._screenList[num - 1];
			}
			return screenBase;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000411C File Offset: 0x0000231C
		public static void Update(IReadOnlyList<int> lastKeysPressed)
		{
			ScreenManager._lastPressedKeys = lastKeysPressed;
			ScreenBase topScreen = ScreenManager.TopScreen;
			if (topScreen != null && topScreen.IsActive)
			{
				ScreenManager.TopScreen.Update(ScreenManager._lastPressedKeys);
			}
			for (int i = 0; i < ScreenManager._globalLayers.Count; i++)
			{
				GlobalLayer globalLayer = ScreenManager._globalLayers[i];
				if (globalLayer.Layer.IsActive)
				{
					globalLayer.Update(ScreenManager._lastPressedKeys);
				}
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000418C File Offset: 0x0000238C
		private static bool? GetMouseInput()
		{
			bool? flag = null;
			List<InputKey> activeMouseKeys = ScreenManager.GetActiveMouseKeys();
			if (ScreenManager._lastMouseActiveKeys.Count != activeMouseKeys.Count || !ScreenManager._lastMouseActiveKeys.SequenceEqual<InputKey>(activeMouseKeys))
			{
				flag = new bool?(activeMouseKeys.Count > 0);
			}
			ScreenManager._lastMouseActiveKeys = activeMouseKeys;
			return flag;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000041E0 File Offset: 0x000023E0
		private static List<InputKey> GetActiveMouseKeys()
		{
			List<InputKey> list = new List<InputKey>();
			InputKey inputKey = (ScreenManager.IsEnterButtonRDown ? InputKey.ControllerRDown : InputKey.ControllerRRight);
			InputKey[] array = new InputKey[]
			{
				InputKey.LeftMouseButton,
				InputKey.RightMouseButton,
				InputKey.MiddleMouseButton,
				InputKey.X1MouseButton,
				InputKey.X2MouseButton,
				(InputKey)0
			};
			array[5] = inputKey;
			InputKey[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				if (Input.IsKeyDown(array2[i]))
				{
					list.Add(array2[i]);
				}
			}
			return list;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004240 File Offset: 0x00002440
		public static void EarlyUpdate(Vec2 usableArea)
		{
			ScreenManager.UsableArea = usableArea;
			ScreenManager.RefreshGlobalOrder();
			InputType inputType = InputType.None;
			bool? mouseInput = ScreenManager.GetMouseInput();
			bool? flag = mouseInput;
			bool flag2 = false;
			if ((flag.GetValueOrDefault() == flag2) & (flag != null))
			{
				ScreenManager._mouseDownLayer = null;
			}
			for (int i = ScreenManager.SortedLayers.Count - 1; i >= 0; i--)
			{
				ScreenLayer screenLayer = ScreenManager.SortedLayers[i];
				if (screenLayer != null && screenLayer.IsActive && !screenLayer.IsFinalized)
				{
					InputType inputType2 = InputType.None;
					InputUsageMask inputUsageMask = screenLayer.InputUsageMask;
					screenLayer.ScreenOrderInLastFrame = i;
					bool isHitThisFrame = screenLayer.IsHitThisFrame;
					screenLayer.IsHitThisFrame = false;
					if (screenLayer.HitTest())
					{
						if (ScreenManager._mouseDownLayer != null && ScreenManager._mouseDownLayer != screenLayer && mouseInput != null && !inputType.HasAnyFlag(InputType.MouseButton) && inputUsageMask.HasAnyFlag(InputUsageMask.MouseButtons))
						{
							ScreenManager._mouseDownLayer = null;
						}
						if (ScreenManager.FirstHitLayer == null)
						{
							ScreenManager.FirstHitLayer = screenLayer;
							ScreenManager._engineInterface.ActivateMouseCursor(screenLayer.ActiveCursor);
						}
						if (ScreenManager._mouseDownLayer == screenLayer || (ScreenManager._mouseDownLayer == null && !inputType.HasAnyFlag(InputType.MouseButton) && inputUsageMask.HasAnyFlag(InputUsageMask.MouseButtons)))
						{
							inputType2 |= InputType.MouseButton;
							inputType |= InputType.MouseButton;
							screenLayer.IsHitThisFrame = true;
							flag = mouseInput;
							flag2 = true;
							if ((flag.GetValueOrDefault() == flag2) & (flag != null))
							{
								ScreenManager._mouseDownLayer = screenLayer;
							}
						}
						if (!inputType.HasAnyFlag(InputType.MouseWheel) && inputUsageMask.HasAnyFlag(InputUsageMask.MouseWheels))
						{
							inputType2 |= InputType.MouseWheel;
							inputType |= InputType.MouseWheel;
							screenLayer.IsHitThisFrame = true;
						}
					}
					if (!inputType.HasAnyFlag(InputType.Key) && ScreenManager.FocusTest(screenLayer))
					{
						inputType2 |= InputType.Key;
						inputType |= InputType.Key;
					}
					screenLayer.EarlyProcessEvents(inputType2);
				}
				if (ScreenManager._mouseDownLayer == screenLayer)
				{
					screenLayer.IsHitThisFrame = true;
					screenLayer.Input.MouseOnMe = true;
				}
				else
				{
					screenLayer.Input.MouseOnMe = screenLayer.IsActive && screenLayer.IsHitThisFrame;
				}
			}
			for (int j = ScreenManager._sortedLayers.Count - 1; j >= 0; j--)
			{
				ScreenLayer screenLayer2 = ScreenManager._sortedLayers[j];
				if (screenLayer2.IsFocusLayer)
				{
					screenLayer2.Input.RegisterDownKeys();
				}
				else
				{
					screenLayer2.Input.ResetLastDownKeys();
				}
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004478 File Offset: 0x00002678
		private static void Update()
		{
			int num = 0;
			for (int i = 0; i < ScreenManager.SortedLayers.Count; i++)
			{
				if (ScreenManager.SortedLayers[i].IsActive)
				{
					num++;
				}
			}
			if (ScreenManager._sortedActiveLayersCopyForUpdate.Length < num)
			{
				ScreenManager._sortedActiveLayersCopyForUpdate = new ScreenLayer[num];
			}
			int num2 = 0;
			for (int j = 0; j < ScreenManager.SortedLayers.Count; j++)
			{
				ScreenLayer screenLayer = ScreenManager.SortedLayers[j];
				if (screenLayer.IsActive)
				{
					ScreenManager._sortedActiveLayersCopyForUpdate[num2] = screenLayer;
					num2++;
				}
			}
			for (int k = num2 - 1; k >= 0; k--)
			{
				ScreenLayer screenLayer2 = ScreenManager._sortedActiveLayersCopyForUpdate[k];
				if (!screenLayer2.IsFinalized)
				{
					screenLayer2.ProcessEvents();
				}
			}
			for (int l = 0; l < ScreenManager._sortedActiveLayersCopyForUpdate.Length; l++)
			{
				ScreenManager._sortedActiveLayersCopyForUpdate[l] = null;
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0000454C File Offset: 0x0000274C
		private static void LateUpdate(float dt)
		{
			for (int i = 0; i < ScreenManager.SortedLayers.Count; i++)
			{
				ScreenLayer screenLayer = ScreenManager.SortedLayers[i];
				if (screenLayer != null && screenLayer.IsActive && !screenLayer.IsFinalized)
				{
					screenLayer.LateUpdate(dt);
				}
			}
			ScreenManager.FirstHitLayer = null;
			ScreenManager.UpdateMouseVisibility();
			if (ScreenManager._globalOrderDirty)
			{
				ScreenManager.RefreshGlobalOrder();
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000045AC File Offset: 0x000027AC
		internal static void UpdateMouseVisibility()
		{
			if (!ScreenManager._isWindowFocused)
			{
				ScreenManager._activeMouseVisible = ScreenManager.EngineInterface.GetMouseVisible();
			}
			for (int i = 0; i < ScreenManager.SortedLayers.Count; i++)
			{
				ScreenLayer screenLayer = ScreenManager.SortedLayers[i];
				if (screenLayer.IsActive && screenLayer.InputRestrictions.MouseVisibility)
				{
					if (!ScreenManager._activeMouseVisible)
					{
						ScreenManager.SetMouseVisible(true);
					}
					return;
				}
			}
			if (ScreenManager._activeMouseVisible)
			{
				ScreenManager.SetMouseVisible(false);
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004620 File Offset: 0x00002820
		public static bool IsControllerActive()
		{
			return Input.IsControllerConnected && Input.IsGamepadActive && !Input.IsMouseActive && ScreenManager._engineInterface.GetMouseVisible();
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00004643 File Offset: 0x00002843
		public static bool IsMouseCursorHidden()
		{
			return !Input.IsMouseActive && ScreenManager._engineInterface.GetMouseVisible();
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00004658 File Offset: 0x00002858
		public static bool IsMouseCursorActive()
		{
			return Input.IsMouseActive && ScreenManager._engineInterface.GetMouseVisible();
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00004670 File Offset: 0x00002870
		public static bool IsLayerBlockedAtPosition(ScreenLayer layer, Vector2 position)
		{
			for (int i = ScreenManager.SortedLayers.Count - 1; i >= 0; i--)
			{
				ScreenLayer screenLayer = ScreenManager.SortedLayers[i];
				if (layer == screenLayer)
				{
					return false;
				}
				if (screenLayer != null && screenLayer.IsActive && !screenLayer.IsFinalized && screenLayer.HitTest(position))
				{
					if (screenLayer.InputUsageMask.HasAnyFlag(InputUsageMask.MouseButtons))
					{
						return layer != ScreenManager.SortedLayers[i];
					}
					if (screenLayer.InputUsageMask.HasAnyFlag(InputUsageMask.MouseWheels))
					{
						return layer != ScreenManager.SortedLayers[i];
					}
				}
			}
			return false;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00004703 File Offset: 0x00002903
		private static void SetMouseVisible(bool value)
		{
			ScreenManager._activeMouseVisible = value;
			ScreenManager._engineInterface.SetMouseVisible(value);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004716 File Offset: 0x00002916
		public static bool GetMouseVisibility()
		{
			return ScreenManager._activeMouseVisible;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00004720 File Offset: 0x00002920
		public static void TrySetFocus(ScreenLayer layer)
		{
			if (ScreenManager.FocusedLayer != null && ScreenManager.FocusedLayer.InputRestrictions.Order > layer.InputRestrictions.Order && layer.IsActive)
			{
				return;
			}
			if (!layer.IsFocusLayer && !layer.FocusTest())
			{
				return;
			}
			if (ScreenManager.FocusedLayer != layer)
			{
				ScreenLayer focusedLayer = ScreenManager.FocusedLayer;
				if (focusedLayer != null)
				{
					focusedLayer.HandleLoseFocus();
				}
				ScreenManager.FocusedLayer = layer;
				ScreenLayer focusedLayer2 = ScreenManager.FocusedLayer;
				if (focusedLayer2 == null)
				{
					return;
				}
				focusedLayer2.HandleGainFocus();
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004798 File Offset: 0x00002998
		public static void TryLoseFocus(ScreenLayer layer)
		{
			if (ScreenManager.FocusedLayer != layer)
			{
				return;
			}
			ScreenLayer focusedLayer = ScreenManager.FocusedLayer;
			if (focusedLayer != null)
			{
				focusedLayer.HandleLoseFocus();
			}
			for (int i = ScreenManager.SortedLayers.Count - 1; i >= 0; i--)
			{
				ScreenLayer screenLayer = ScreenManager.SortedLayers[i];
				if (screenLayer.IsActive && screenLayer.IsFocusLayer && layer != screenLayer)
				{
					ScreenLayer focusedLayer2 = ScreenManager.FocusedLayer;
					if (focusedLayer2 != null)
					{
						focusedLayer2.HandleGainFocus();
					}
					ScreenManager.FocusedLayer = screenLayer;
					return;
				}
			}
			ScreenManager.FocusedLayer = null;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004812 File Offset: 0x00002A12
		private static bool FocusTest(ScreenLayer layer)
		{
			return ScreenManager.FocusedLayer == layer;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000481C File Offset: 0x00002A1C
		public static void OnScaleChange(float newScale)
		{
			ScreenManager.Scale = newScale;
			foreach (GlobalLayer globalLayer in ScreenManager._globalLayers)
			{
				globalLayer.UpdateLayout();
			}
			foreach (ScreenBase screenBase in ScreenManager._screenList)
			{
				screenBase.UpdateLayout();
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000048A4 File Offset: 0x00002AA4
		public static void OnControllerDisconnect()
		{
			ScreenManager.OnControllerDisconnectedEvent onControllerDisconnected = ScreenManager.OnControllerDisconnected;
			if (onControllerDisconnected == null)
			{
				return;
			}
			onControllerDisconnected();
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000048B5 File Offset: 0x00002AB5
		private static void SetSortedLayersDirty()
		{
			ScreenManager._isSortedActiveLayersDirty = true;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000048C0 File Offset: 0x00002AC0
		private static void OnScreenListChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			Debug.Print("OnScreenListChanged", 0, Debug.DebugColor.White, 17592186044416UL);
			ScreenManager.SetSortedLayersDirty();
			ObservableCollection<ScreenBase> screenList = ScreenManager._screenList;
			if (screenList != null && screenList.Count > 0)
			{
				if (ScreenManager.TopScreen != null)
				{
					ScreenManager.TopScreen.OnAddLayer -= ScreenManager.OnLayerAddedToTopLayer;
					ScreenManager.TopScreen.OnRemoveLayer -= ScreenManager.OnLayerRemovedFromTopLayer;
				}
				ScreenManager.TopScreen = ScreenManager._screenList[ScreenManager._screenList.Count - 1];
				if (ScreenManager.TopScreen != null)
				{
					ScreenManager.TopScreen.OnAddLayer += ScreenManager.OnLayerAddedToTopLayer;
					ScreenManager.TopScreen.OnRemoveLayer += ScreenManager.OnLayerRemovedFromTopLayer;
				}
			}
			else
			{
				if (ScreenManager.TopScreen != null)
				{
					ScreenManager.TopScreen.OnAddLayer -= ScreenManager.OnLayerAddedToTopLayer;
					ScreenManager.TopScreen.OnRemoveLayer -= ScreenManager.OnLayerRemovedFromTopLayer;
				}
				ScreenManager.TopScreen = null;
			}
			ScreenManager.SetSortedLayersDirty();
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000049C2 File Offset: 0x00002BC2
		private static void OnLayerAddedToTopLayer(ScreenLayer layer)
		{
			ScreenManager.SetSortedLayersDirty();
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000049C9 File Offset: 0x00002BC9
		private static void OnLayerRemovedFromTopLayer(ScreenLayer layer)
		{
			ScreenManager.SetSortedLayersDirty();
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000049D0 File Offset: 0x00002BD0
		private static void OnGlobalListChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			ScreenManager.SetSortedLayersDirty();
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000049D8 File Offset: 0x00002BD8
		[CommandLineFunctionality.CommandLineArgumentFunction("set_screen_debug_information_enabled", "ui")]
		public static string SetScreenDebugInformationEnabled(List<string> args)
		{
			string text = "Usage: ui.set_screen_debug_information_enabled [True/False]";
			if (args.Count != 1)
			{
				return text;
			}
			bool flag;
			if (bool.TryParse(args[0], out flag))
			{
				ScreenManager.SetScreenDebugInformationEnabled(flag);
				return "Success.";
			}
			return text;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004A15 File Offset: 0x00002C15
		public static void SetScreenDebugInformationEnabled(bool isEnabled)
		{
			ScreenManager._isScreenDebugInformationEnabled = isEnabled;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00004A20 File Offset: 0x00002C20
		private static void ShowScreenDebugInformation()
		{
			if (ScreenManager._isScreenDebugInformationEnabled)
			{
				ScreenManager._engineInterface.BeginDebugPanel("Screen Debug Information");
				for (int i = 0; i < ScreenManager.SortedLayers.Count; i++)
				{
					ScreenLayer screenLayer = ScreenManager.SortedLayers[i];
					List<string> list = new List<string>();
					InputUsageMask inputUsageMask = screenLayer.InputRestrictions.InputUsageMask;
					if (screenLayer.IsFocusLayer && ScreenManager.FocusedLayer == screenLayer)
					{
						list.Add("(FocusLayer)");
					}
					list.Add(screenLayer.Name);
					if (screenLayer.InputRestrictions.MouseVisibility)
					{
						list.Add("MouseVisibile");
					}
					if (screenLayer.InputRestrictions.InputUsageMask != InputUsageMask.Invalid)
					{
						list.Add("Input");
					}
					string text = string.Join(" - ", list);
					if (ScreenManager._engineInterface.DrawDebugTreeNode(string.Format("{0}###{1}.{2}.{3}", new object[]
					{
						text,
						screenLayer.Name,
						i,
						screenLayer.Name.GetDeterministicHashCode()
					})))
					{
						screenLayer.DrawDebugInfo();
						ScreenManager._engineInterface.PopDebugTreeNode();
					}
				}
				ScreenManager._engineInterface.EndDebugPanel();
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00004B41 File Offset: 0x00002D41
		private static void OnUsableAreaChanged(Vec2 newUsableArea)
		{
			ScreenManager.UpdateLayout();
		}

		// Token: 0x04000029 RID: 41
		private static IScreenManagerEngineConnection _engineInterface;

		// Token: 0x0400002B RID: 43
		private static Vec2 _usableArea = new Vec2(1f, 1f);

		// Token: 0x04000030 RID: 48
		private static ObservableCollection<ScreenBase> _screenList = new ObservableCollection<ScreenBase>();

		// Token: 0x04000031 RID: 49
		private static ObservableCollection<GlobalLayer> _globalLayers = new ObservableCollection<GlobalLayer>();

		// Token: 0x04000032 RID: 50
		private static List<ScreenLayer> _sortedLayers = new List<ScreenLayer>(16);

		// Token: 0x04000033 RID: 51
		private static ScreenLayer[] _sortedActiveLayersCopyForUpdate = new ScreenLayer[16];

		// Token: 0x04000034 RID: 52
		private static bool _isSortedActiveLayersDirty = true;

		// Token: 0x04000035 RID: 53
		private static bool _isScreenDebugInformationEnabled;

		// Token: 0x04000036 RID: 54
		private static List<InputKey> _lastMouseActiveKeys = new List<InputKey>();

		// Token: 0x04000037 RID: 55
		public static bool DisableScreenManagerTicks = false;

		// Token: 0x04000039 RID: 57
		private static bool _activeMouseVisible;

		// Token: 0x0400003A RID: 58
		private static IReadOnlyList<int> _lastPressedKeys;

		// Token: 0x0400003B RID: 59
		private static bool _globalOrderDirty;

		// Token: 0x0400003E RID: 62
		private static ScreenLayer _mouseDownLayer;

		// Token: 0x0400003F RID: 63
		private static bool _isWindowFocused;

		// Token: 0x04000040 RID: 64
		private static bool _isRefreshActive = false;

		// Token: 0x0200000D RID: 13
		// (Invoke) Token: 0x060000EC RID: 236
		public delegate void OnPushScreenEvent(ScreenBase pushedScreen);

		// Token: 0x0200000E RID: 14
		// (Invoke) Token: 0x060000F0 RID: 240
		public delegate void OnPopScreenEvent(ScreenBase poppedScreen);

		// Token: 0x0200000F RID: 15
		// (Invoke) Token: 0x060000F4 RID: 244
		public delegate void OnControllerDisconnectedEvent();

		// Token: 0x02000010 RID: 16
		// (Invoke) Token: 0x060000F8 RID: 248
		public delegate bool OnPlatformTextRequestedDelegate(string initialText, string descriptionText, int maxLength, int keyboardTypeEnum);
	}
}
