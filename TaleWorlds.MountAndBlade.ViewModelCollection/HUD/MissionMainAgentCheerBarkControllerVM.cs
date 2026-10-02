using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics.CosmeticTypes;
using TaleWorlds.MountAndBlade.Diamond.Lobby;
using TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD
{
	// Token: 0x02000052 RID: 82
	public class MissionMainAgentCheerBarkControllerVM : ViewModel
	{
		// Token: 0x0600069F RID: 1695 RVA: 0x00017D38 File Offset: 0x00015F38
		public MissionMainAgentCheerBarkControllerVM(Action<int> onSelectCheer, Action<int> onSelectBark)
		{
			this._onSelectCheer = onSelectCheer;
			this._onSelectBark = onSelectBark;
			this.Nodes = new MBBindingList<CheerBarkNodeItemVM>();
			if (GameNetwork.IsMultiplayer)
			{
				this._ownedTauntCosmetics = NetworkMain.GameClient.OwnedCosmetics.ToList<string>();
				this.UpdatePlayerTauntIndices();
			}
			CheerBarkNodeItemVM.OnSelection += this.OnNodeFocused;
			CheerBarkNodeItemVM.OnNodeFocused += this.OnNodeTooltipToggled;
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x00017DA8 File Offset: 0x00015FA8
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.Nodes.ApplyActionOnAllItems(delegate(CheerBarkNodeItemVM n)
			{
				n.OnFinalize();
			});
			CheerBarkNodeItemVM.OnSelection -= this.OnNodeFocused;
			CheerBarkNodeItemVM.OnNodeFocused -= this.OnNodeTooltipToggled;
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00017E08 File Offset: 0x00016008
		private void PopulateList()
		{
			bool isClient = GameNetwork.IsClient;
			this.IsNodesCategories = isClient;
			this.Nodes.Clear();
			GameKeyContext category = HotKeyManager.GetCategory("CombatHotKeyCategory");
			HotKey hotKey = category.GetHotKey("CheerBarkCloseMenu");
			SkinVoiceManager.SkinVoiceType[] mpBarks = SkinVoiceManager.VoiceType.MpBarks;
			if (isClient)
			{
				HotKey hotKey2 = category.GetHotKey("CheerBarkSelectFirstCategory");
				CheerBarkNodeItemVM cheerBarkNodeItemVM = new CheerBarkNodeItemVM(new TextObject("{=KxH4VVU3}Taunt", null), "cheer", hotKey2, false, TauntUsageManager.TauntUsage.TauntUsageFlag.None);
				this.Nodes.Add(cheerBarkNodeItemVM);
				TauntCosmeticElement[] array = new TauntCosmeticElement[TauntCosmeticElement.MaxNumberOfTaunts];
				foreach (TauntIndexData tauntIndexData in this._playerTauntsWithIndices)
				{
					string tauntId = tauntIndexData.TauntId;
					int tauntIndex = tauntIndexData.TauntIndex;
					TauntCosmeticElement tauntCosmeticElement = CosmeticsManager.GetCosmeticElement(tauntId) as TauntCosmeticElement;
					if (!tauntCosmeticElement.IsFree && !this._ownedTauntCosmetics.Contains(tauntId))
					{
						Debug.FailedAssert("Taunt list have invalid taunt: " + tauntId, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\HUD\\MissionMainAgentCheerBarkControllerVM.cs", "PopulateList", 86);
					}
					else if (tauntIndex >= 0 && tauntIndex < TauntCosmeticElement.MaxNumberOfTaunts)
					{
						array[tauntIndex] = tauntCosmeticElement;
					}
				}
				for (int i = 0; i < array.Length; i++)
				{
					TauntCosmeticElement tauntCosmeticElement2 = array[i];
					if (tauntCosmeticElement2 != null)
					{
						int indexOfAction = TauntUsageManager.Instance.GetIndexOfAction(tauntCosmeticElement2.Id);
						TauntUsageManager.TauntUsage.TauntUsageFlag actionNotUsableReason = CosmeticsManagerHelper.GetActionNotUsableReason(Agent.Main, indexOfAction);
						cheerBarkNodeItemVM.AddSubNode(new CheerBarkNodeItemVM(tauntCosmeticElement2.Id, new TextObject("{=!}" + tauntCosmeticElement2.Name, null), tauntCosmeticElement2.Id, this.GetCheerShortcut(i), true, actionNotUsableReason));
					}
					else
					{
						cheerBarkNodeItemVM.AddSubNode(new CheerBarkNodeItemVM(string.Empty, TextObject.GetEmpty(), string.Empty, null, true, TauntUsageManager.TauntUsage.TauntUsageFlag.None));
					}
				}
				HotKey hotKey3 = category.GetHotKey("CheerBarkSelectSecondCategory");
				CheerBarkNodeItemVM cheerBarkNodeItemVM2 = new CheerBarkNodeItemVM(new TextObject("{=5Xoilj6r}Shout", null), "bark", hotKey3, false, TauntUsageManager.TauntUsage.TauntUsageFlag.None);
				this.Nodes.Add(cheerBarkNodeItemVM2);
				cheerBarkNodeItemVM2.AddSubNode(new CheerBarkNodeItemVM(new TextObject("{=koX9okuG}None", null), "none", hotKey, true, TauntUsageManager.TauntUsage.TauntUsageFlag.None));
				for (int j = 0; j < mpBarks.Length; j++)
				{
					cheerBarkNodeItemVM2.AddSubNode(new CheerBarkNodeItemVM(mpBarks[j].GetName(), "bark" + j, this.GetCheerShortcut(j), true, TauntUsageManager.TauntUsage.TauntUsageFlag.None));
				}
			}
			else
			{
				ActionIndexCache[] array2 = Agent.DefaultTauntActions.ToArray<ActionIndexCache>();
				this.Nodes.Add(new CheerBarkNodeItemVM(new TextObject("{=koX9okuG}None", null), "none", hotKey, true, TauntUsageManager.TauntUsage.TauntUsageFlag.None));
				for (int k = 0; k < array2.Length; k++)
				{
					this.Nodes.Add(new CheerBarkNodeItemVM(new TextObject("{=!}" + (k + 1), null), array2[k].GetName(), this.GetCheerShortcut(k), true, TauntUsageManager.TauntUsage.TauntUsageFlag.None));
				}
			}
			this.DisabledReasonText = string.Empty;
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0001810C File Offset: 0x0001630C
		private void UpdatePlayerTauntIndices()
		{
			LobbyClient gameClient = NetworkMain.GameClient;
			if (((gameClient != null) ? gameClient.PlayerData : null) != null)
			{
				string text = NetworkMain.GameClient.PlayerData.UserId.ToString();
				this._playerTauntsWithIndices = MultiplayerLocalDataManager.Instance.TauntSlotData.GetTauntIndicesForPlayer(text);
			}
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0001815C File Offset: 0x0001635C
		private HotKey GetCheerShortcut(int cheerIndex)
		{
			GameKeyContext category = HotKeyManager.GetCategory("CombatHotKeyCategory");
			switch (cheerIndex)
			{
			case 0:
				return category.GetHotKey("CheerBarkItem1");
			case 1:
				return category.GetHotKey("CheerBarkItem2");
			case 2:
				return category.GetHotKey("CheerBarkItem3");
			case 3:
				return category.GetHotKey("CheerBarkItem4");
			default:
				return null;
			}
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x000181C0 File Offset: 0x000163C0
		public void SelectItem(int itemIndex, int subNodeIndex = -1)
		{
			if (subNodeIndex == -1)
			{
				for (int i = 0; i < this.Nodes.Count; i++)
				{
					this.Nodes[i].IsSelected = itemIndex == i;
				}
				return;
			}
			if (itemIndex >= 0 && itemIndex < this.Nodes.Count)
			{
				for (int j = 0; j < this.Nodes[itemIndex].SubNodes.Count; j++)
				{
					this.Nodes[itemIndex].SubNodes[j].IsSelected = subNodeIndex == j;
				}
			}
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00018250 File Offset: 0x00016450
		public void ExecuteActivate()
		{
			this.IsActive = true;
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0001825C File Offset: 0x0001645C
		public void ExecuteDeactivate(bool applySelection)
		{
			if (applySelection)
			{
				CheerBarkNodeItemVM cheerBarkNodeItemVM = this.Nodes.FirstOrDefault<CheerBarkNodeItemVM>((CheerBarkNodeItemVM c) => c.IsSelected);
				if (cheerBarkNodeItemVM != null)
				{
					if (this.IsNodesCategories)
					{
						bool flag = cheerBarkNodeItemVM.TypeAsString == "bark";
						CheerBarkNodeItemVM cheerBarkNodeItemVM2;
						if (cheerBarkNodeItemVM == null)
						{
							cheerBarkNodeItemVM2 = null;
						}
						else
						{
							cheerBarkNodeItemVM2 = cheerBarkNodeItemVM.SubNodes.FirstOrDefault<CheerBarkNodeItemVM>((CheerBarkNodeItemVM c) => c.IsSelected);
						}
						CheerBarkNodeItemVM cheerBarkNodeItemVM3 = cheerBarkNodeItemVM2;
						if (cheerBarkNodeItemVM3 != null && cheerBarkNodeItemVM3.TypeAsString != "none")
						{
							if (flag)
							{
								Action<int> onSelectBark = this._onSelectBark;
								if (onSelectBark != null)
								{
									onSelectBark(cheerBarkNodeItemVM.SubNodes.IndexOf(cheerBarkNodeItemVM3) - 1);
								}
							}
							else
							{
								int indexOfAction = TauntUsageManager.Instance.GetIndexOfAction(cheerBarkNodeItemVM3.TypeAsString);
								Action<int> onSelectCheer = this._onSelectCheer;
								if (onSelectCheer != null)
								{
									onSelectCheer(indexOfAction);
								}
							}
						}
					}
					else if (cheerBarkNodeItemVM.TypeAsString != "none")
					{
						int num = TauntUsageManager.Instance.GetIndexOfAction(cheerBarkNodeItemVM.TypeAsString);
						if (num == -1)
						{
							ActionIndexCache[] defaultTauntActions = Agent.DefaultTauntActions;
							for (int i = 0; i < defaultTauntActions.Length; i++)
							{
								string name = defaultTauntActions[i].GetName();
								if (cheerBarkNodeItemVM.TypeAsString == name)
								{
									num = i;
									break;
								}
							}
						}
						Action<int> onSelectCheer2 = this._onSelectCheer;
						if (onSelectCheer2 != null)
						{
							onSelectCheer2(num);
						}
					}
				}
			}
			this.Nodes.ApplyActionOnAllItems(delegate(CheerBarkNodeItemVM n)
			{
				n.IsSelected = false;
			});
			this.IsActive = false;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00018408 File Offset: 0x00016608
		public void OnNodeFocused(CheerBarkNodeItemVM focusedNode)
		{
			string text = ((focusedNode != null) ? focusedNode.CheerNameText : null) ?? string.Empty;
			if (this.IsNodesCategories)
			{
				bool flag = focusedNode != null && focusedNode.TypeAsString.Contains("bark");
				string typeId = (flag ? "bark" : "cheer");
				this.Nodes.First<CheerBarkNodeItemVM>((CheerBarkNodeItemVM c) => c.TypeAsString == typeId).SelectedNodeText = text;
				return;
			}
			this.SelectedNodeText = text;
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0001848A File Offset: 0x0001668A
		public void OnNodeTooltipToggled(CheerBarkNodeItemVM node)
		{
			if (node != null && node.TauntUsageDisabledReason != TauntUsageManager.TauntUsage.TauntUsageFlag.None)
			{
				this.DisabledReasonText = TauntUsageManager.GetActionDisabledReasonText(node.TauntUsageDisabledReason);
				return;
			}
			this.DisabledReasonText = string.Empty;
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x000184B4 File Offset: 0x000166B4
		// (set) Token: 0x060006AA RID: 1706 RVA: 0x000184BC File Offset: 0x000166BC
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChangedWithValue(value, "IsActive");
					if (this._isActive)
					{
						this.PopulateList();
					}
				}
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x000184E8 File Offset: 0x000166E8
		// (set) Token: 0x060006AC RID: 1708 RVA: 0x000184F0 File Offset: 0x000166F0
		[DataSourceProperty]
		public string DisabledReasonText
		{
			get
			{
				return this._disabledReasonText;
			}
			set
			{
				if (value != this._disabledReasonText)
				{
					this._disabledReasonText = value;
					base.OnPropertyChangedWithValue<string>(value, "DisabledReasonText");
				}
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x00018513 File Offset: 0x00016713
		// (set) Token: 0x060006AE RID: 1710 RVA: 0x0001851B File Offset: 0x0001671B
		[DataSourceProperty]
		public string SelectedNodeText
		{
			get
			{
				return this._selectedNodeText;
			}
			set
			{
				if (value != this._selectedNodeText)
				{
					this._selectedNodeText = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectedNodeText");
				}
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x0001853E File Offset: 0x0001673E
		// (set) Token: 0x060006B0 RID: 1712 RVA: 0x00018546 File Offset: 0x00016746
		[DataSourceProperty]
		public bool IsNodesCategories
		{
			get
			{
				return this._isNodesCategories;
			}
			set
			{
				if (value != this._isNodesCategories)
				{
					this._isNodesCategories = value;
					base.OnPropertyChangedWithValue(value, "IsNodesCategories");
				}
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x00018564 File Offset: 0x00016764
		// (set) Token: 0x060006B2 RID: 1714 RVA: 0x0001856C File Offset: 0x0001676C
		[DataSourceProperty]
		public MBBindingList<CheerBarkNodeItemVM> Nodes
		{
			get
			{
				return this._nodes;
			}
			set
			{
				if (value != this._nodes)
				{
					this._nodes = value;
					base.OnPropertyChangedWithValue<MBBindingList<CheerBarkNodeItemVM>>(value, "Nodes");
				}
			}
		}

		// Token: 0x040002F4 RID: 756
		private const string CheerId = "cheer";

		// Token: 0x040002F5 RID: 757
		private const string BarkId = "bark";

		// Token: 0x040002F6 RID: 758
		private const string NoneId = "none";

		// Token: 0x040002F7 RID: 759
		private readonly Action<int> _onSelectCheer;

		// Token: 0x040002F8 RID: 760
		private readonly Action<int> _onSelectBark;

		// Token: 0x040002F9 RID: 761
		private List<string> _ownedTauntCosmetics;

		// Token: 0x040002FA RID: 762
		private IEnumerable<TauntIndexData> _playerTauntsWithIndices;

		// Token: 0x040002FB RID: 763
		private bool _isActive;

		// Token: 0x040002FC RID: 764
		private bool _isNodesCategories;

		// Token: 0x040002FD RID: 765
		private string _disabledReasonText;

		// Token: 0x040002FE RID: 766
		private string _selectedNodeText;

		// Token: 0x040002FF RID: 767
		private MBBindingList<CheerBarkNodeItemVM> _nodes;
	}
}
