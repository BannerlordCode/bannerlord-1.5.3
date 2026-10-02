using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x02000213 RID: 531
	public class CharacterCreationManager
	{
		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x0600208C RID: 8332 RVA: 0x00093360 File Offset: 0x00091560
		public MBReadOnlyList<NarrativeMenu> NarrativeMenus
		{
			get
			{
				return this._narrativeMenus;
			}
		}

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x0600208D RID: 8333 RVA: 0x00093368 File Offset: 0x00091568
		// (set) Token: 0x0600208E RID: 8334 RVA: 0x00093370 File Offset: 0x00091570
		public CharacterCreationContent CharacterCreationContent { get; private set; }

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x0600208F RID: 8335 RVA: 0x00093379 File Offset: 0x00091579
		// (set) Token: 0x06002090 RID: 8336 RVA: 0x00093381 File Offset: 0x00091581
		public NarrativeMenu CurrentMenu { get; private set; }

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x06002091 RID: 8337 RVA: 0x0009338A File Offset: 0x0009158A
		public int CharacterCreationMenuCount
		{
			get
			{
				return this.NarrativeMenus.Count;
			}
		}

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x06002092 RID: 8338 RVA: 0x00093397 File Offset: 0x00091597
		// (set) Token: 0x06002093 RID: 8339 RVA: 0x0009339F File Offset: 0x0009159F
		public CharacterCreationStageBase CurrentStage { get; private set; }

		// Token: 0x06002094 RID: 8340 RVA: 0x000933A8 File Offset: 0x000915A8
		public CharacterCreationManager(CharacterCreationState state)
		{
			this._state = state;
			this._stages = new MBList<CharacterCreationStageBase>();
			this.FaceGenHistory = new FaceGenHistory(new List<UndoRedoKey>(100), new List<UndoRedoKey>(100), new Dictionary<string, float>());
			this._narrativeMenus = new MBList<NarrativeMenu>();
			this.SelectedOptions = new Dictionary<NarrativeMenu, NarrativeMenuOption>();
			this.CharacterCreationContent = new CharacterCreationContent();
			CampaignEventDispatcher.Instance.OnCharacterCreationInitialized(this);
			foreach (KeyValuePair<int, ICharacterCreationContentHandler> keyValuePair in this._handlers)
			{
				keyValuePair.Value.InitializeContent(this);
			}
			foreach (KeyValuePair<int, ICharacterCreationContentHandler> keyValuePair2 in this._handlers)
			{
				keyValuePair2.Value.AfterInitializeContent(this);
			}
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x000934B4 File Offset: 0x000916B4
		public void RegisterCharacterCreationContentHandler(ICharacterCreationContentHandler characterCreationContentHandler, int priority)
		{
			this._handlers.Add(priority, characterCreationContentHandler);
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x000934C3 File Offset: 0x000916C3
		public void AddStage(CharacterCreationStageBase stage)
		{
			this._stages.Add(stage);
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x000934D4 File Offset: 0x000916D4
		public bool RemoveStage<T>() where T : CharacterCreationStageBase
		{
			for (int i = 0; i < this._stages.Count; i++)
			{
				if (this._stages[i] is T)
				{
					this._stages.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x0009351C File Offset: 0x0009171C
		public T GetStage<T>() where T : CharacterCreationStageBase
		{
			for (int i = 0; i < this._stages.Count; i++)
			{
				T t;
				if ((t = this._stages[i] as T) != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06002099 RID: 8345 RVA: 0x0009356C File Offset: 0x0009176C
		public void NextStage()
		{
			this._stageIndex++;
			if (this.CurrentStage != null)
			{
				CharacterCreationStageBase currentStage = this.CurrentStage;
				if (currentStage != null)
				{
					currentStage.OnFinalize();
				}
				foreach (KeyValuePair<int, ICharacterCreationContentHandler> keyValuePair in this._handlers)
				{
					keyValuePair.Value.OnStageCompleted(this.CurrentStage);
				}
			}
			this._furthestStageIndex = MathF.Max(this._furthestStageIndex, this._stageIndex);
			if (this._stageIndex == this._stages.Count)
			{
				this.ApplyFinalEffects();
				this._state.FinalizeCharacterCreationState();
				return;
			}
			this.ActivateStage(this._stages[this._stageIndex]);
			this._state.Refresh();
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x0009364C File Offset: 0x0009184C
		public void PreviousStage()
		{
			CharacterCreationStageBase currentStage = this.CurrentStage;
			if (currentStage != null)
			{
				currentStage.OnFinalize();
			}
			this._stageIndex--;
			this.ActivateStage(this._stages[this._stageIndex]);
			this._state.Refresh();
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x0009369C File Offset: 0x0009189C
		public void GoToStage(int stageIndex)
		{
			if (stageIndex >= 0 && stageIndex < this._stages.Count && stageIndex != this._stageIndex && stageIndex <= this._furthestStageIndex)
			{
				CharacterCreationStageBase currentStage = this.CurrentStage;
				if (currentStage != null)
				{
					currentStage.OnFinalize();
				}
				this._stageIndex = stageIndex;
				this.ActivateStage(this._stages[this._stageIndex]);
				this._state.Refresh();
			}
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x00093707 File Offset: 0x00091907
		private void ActivateStage(CharacterCreationStageBase stage)
		{
			this.CurrentStage = stage;
			if (this._stageIndex == 0)
			{
				this.FaceGenHistory.ClearHistory();
			}
			this._state.OnStageActivated(this.CurrentStage);
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x00093734 File Offset: 0x00091934
		internal void OnStateActivated()
		{
			if (this._stageIndex == -1)
			{
				this.NextStage();
			}
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x00093745 File Offset: 0x00091945
		public int GetIndexOfCurrentStage()
		{
			return this._stageIndex;
		}

		// Token: 0x0600209F RID: 8351 RVA: 0x0009374D File Offset: 0x0009194D
		public int GetTotalStagesCount()
		{
			return this._stages.Count;
		}

		// Token: 0x060020A0 RID: 8352 RVA: 0x0009375A File Offset: 0x0009195A
		public int GetFurthestIndex()
		{
			return this._furthestStageIndex;
		}

		// Token: 0x060020A1 RID: 8353 RVA: 0x00093762 File Offset: 0x00091962
		public void AddNewMenu(NarrativeMenu menu)
		{
			this._narrativeMenus.Add(menu);
		}

		// Token: 0x060020A2 RID: 8354 RVA: 0x00093770 File Offset: 0x00091970
		public NarrativeMenu GetCurrentMenu(int index)
		{
			if (index >= 0 && index < this.NarrativeMenus.Count)
			{
				return this.NarrativeMenus[index];
			}
			return null;
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x00093792 File Offset: 0x00091992
		public IEnumerable<NarrativeMenuOption> GetCurrentMenuOptions(int index)
		{
			NarrativeMenu currentMenu = this.GetCurrentMenu(index);
			if (currentMenu == null)
			{
				return null;
			}
			return currentMenu.CharacterCreationMenuOptions;
		}

		// Token: 0x060020A4 RID: 8356 RVA: 0x000937A8 File Offset: 0x000919A8
		public NarrativeMenu GetNarrativeMenuWithId(string stringId)
		{
			return this.NarrativeMenus.FirstOrDefault<NarrativeMenu>((NarrativeMenu m) => m.StringId.Equals(stringId));
		}

		// Token: 0x060020A5 RID: 8357 RVA: 0x000937DC File Offset: 0x000919DC
		public void DeleteNarrativeMenuWithId(string stringId)
		{
			NarrativeMenu narrativeMenu = null;
			foreach (NarrativeMenu narrativeMenu2 in this.NarrativeMenus)
			{
				if (narrativeMenu2.StringId.Equals(stringId))
				{
					narrativeMenu = narrativeMenu2;
					break;
				}
			}
			if (narrativeMenu != null)
			{
				this._narrativeMenus.Remove(narrativeMenu);
			}
		}

		// Token: 0x060020A6 RID: 8358 RVA: 0x0009384C File Offset: 0x00091A4C
		public void ResetNarrativeMenus()
		{
			this._narrativeMenus.Clear();
			this.ResetMenuOptions();
		}

		// Token: 0x060020A7 RID: 8359 RVA: 0x0009385F File Offset: 0x00091A5F
		public void ResetMenuOptions()
		{
			this.SelectedOptions.Clear();
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x0009386C File Offset: 0x00091A6C
		public void StartNarrativeStage()
		{
			NarrativeMenu narrativeMenu = this.NarrativeMenus.FirstOrDefault<NarrativeMenu>((NarrativeMenu m) => m.InputMenuId == "start");
			this.CurrentMenu = narrativeMenu;
			this.ModifyMenuCharacters();
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x000938B4 File Offset: 0x00091AB4
		public bool TrySwitchToNextMenu()
		{
			string stringId = this.CurrentMenu.StringId;
			this.SelectedOptions[this.CurrentMenu].OnConsequence(this);
			foreach (NarrativeMenu narrativeMenu in this.NarrativeMenus)
			{
				if (narrativeMenu.InputMenuId.Equals(stringId))
				{
					this.CurrentMenu = narrativeMenu;
					this.ModifyMenuCharacters();
					return true;
				}
			}
			return false;
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x00093948 File Offset: 0x00091B48
		private void ModifyMenuCharacters()
		{
			List<NarrativeMenuCharacter> characters = this.CurrentMenu.Characters;
			foreach (NarrativeMenuCharacterArgs narrativeMenuCharacterArgs in this.CurrentMenu.GetNarrativeMenuCharacterArgs(this.CharacterCreationContent.SelectedCulture, this.CharacterCreationContent.SelectedTitleType, this))
			{
				foreach (NarrativeMenuCharacter narrativeMenuCharacter in characters)
				{
					if (narrativeMenuCharacter.StringId == narrativeMenuCharacterArgs.CharacterId)
					{
						if (narrativeMenuCharacterArgs.IsHuman)
						{
							MBEquipmentRoster mbequipmentRoster = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(narrativeMenuCharacterArgs.EquipmentId);
							if (mbequipmentRoster == null)
							{
								Debug.FailedAssert("character creation menu character equipment should not be null! Equipment id: " + narrativeMenuCharacterArgs.EquipmentId, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CharacterCreationContent\\CharacterCreationManager.cs", "ModifyMenuCharacters", 305);
								mbequipmentRoster = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>("player_char_creation_default");
							}
							narrativeMenuCharacter.SetEquipment(mbequipmentRoster);
							narrativeMenuCharacter.SetLeftHandItem(narrativeMenuCharacterArgs.LeftHandItemId);
							narrativeMenuCharacter.SetRightHandItem(narrativeMenuCharacterArgs.RightHandItemId);
							narrativeMenuCharacter.ChangeAge((float)narrativeMenuCharacterArgs.Age);
							narrativeMenuCharacter.IsFemale = narrativeMenuCharacterArgs.IsFemale;
						}
						else
						{
							narrativeMenuCharacter.SetMountCreationKey(narrativeMenuCharacterArgs.MountCreationKey);
							narrativeMenuCharacter.SetHorseItemId(narrativeMenuCharacterArgs.LeftHandItemId);
							narrativeMenuCharacter.SetHarnessItemId(narrativeMenuCharacterArgs.RightHandItemId);
						}
						narrativeMenuCharacter.SetAnimationId(narrativeMenuCharacterArgs.AnimationId);
						narrativeMenuCharacter.SetSpawnPointEntityId(narrativeMenuCharacterArgs.SpawnPointEntityId);
						break;
					}
				}
			}
		}

		// Token: 0x060020AB RID: 8363 RVA: 0x00093B18 File Offset: 0x00091D18
		public bool TrySwitchToPreviousMenu()
		{
			string inputMenuId = this.CurrentMenu.InputMenuId;
			foreach (NarrativeMenu narrativeMenu in this.NarrativeMenus)
			{
				if (narrativeMenu.StringId.Equals(inputMenuId))
				{
					this.CurrentMenu = narrativeMenu;
					this.ModifyMenuCharacters();
					return true;
				}
			}
			return false;
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x00093B94 File Offset: 0x00091D94
		public void OnNarrativeMenuOptionSelected(NarrativeMenuOption option)
		{
			this.SelectedOptions[this.CurrentMenu] = option;
			option.OnSelect(this);
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x00093BAF File Offset: 0x00091DAF
		public IEnumerable<NarrativeMenuOption> GetSuitableNarrativeMenuOptions()
		{
			return this.CurrentMenu.CharacterCreationMenuOptions.Where<NarrativeMenuOption>((NarrativeMenuOption o) => o.OnCondition(this));
		}

		// Token: 0x060020AE RID: 8366 RVA: 0x00093BD0 File Offset: 0x00091DD0
		public void ApplyFinalEffects()
		{
			Clan.PlayerClan.Renown = 0f;
			this.CharacterCreationContent.ApplyCulture(this);
			foreach (KeyValuePair<NarrativeMenu, NarrativeMenuOption> keyValuePair in this.SelectedOptions)
			{
				keyValuePair.Value.ApplyFinalEffects(this.CharacterCreationContent);
			}
			TraitLevelingHelper.UpdateTraitXPAccordingToTraitLevels();
			CultureObject culture = CharacterObject.PlayerCharacter.Culture;
			if (culture.StartingPoint.IsNonZero())
			{
				if (NavigationHelper.IsPositionValidForNavigationType(culture.StartingPoint, MobileParty.MainParty.NavigationCapability))
				{
					MobileParty.MainParty.Position = culture.StartingPoint;
				}
				else
				{
					Debug.FailedAssert("Selected culture start pos is invalid!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CharacterCreationContent\\CharacterCreationManager.cs", "ApplyFinalEffects", 382);
					CampaignVec2 closestNavMeshFaceCenterPositionForPosition = NavigationHelper.GetClosestNavMeshFaceCenterPositionForPosition(culture.StartingPoint, Campaign.Current.Models.PartyNavigationModel.GetInvalidTerrainTypesForNavigationType(MobileParty.MainParty.NavigationCapability));
					MobileParty.MainParty.Position = closestNavMeshFaceCenterPositionForPosition;
				}
			}
			MapState mapState;
			if ((mapState = GameStateManager.Current.ActiveState as MapState) != null)
			{
				mapState.Handler.ResetCamera(true, true);
				mapState.Handler.TeleportCameraToMainParty();
			}
			foreach (KeyValuePair<int, ICharacterCreationContentHandler> keyValuePair2 in this._handlers)
			{
				keyValuePair2.Value.OnCharacterCreationFinalize(this);
			}
		}

		// Token: 0x04000979 RID: 2425
		private readonly MBList<CharacterCreationStageBase> _stages;

		// Token: 0x0400097A RID: 2426
		private readonly MBList<NarrativeMenu> _narrativeMenus;

		// Token: 0x0400097B RID: 2427
		public readonly Dictionary<NarrativeMenu, NarrativeMenuOption> SelectedOptions;

		// Token: 0x0400097E RID: 2430
		private SortedList<int, ICharacterCreationContentHandler> _handlers = new SortedList<int, ICharacterCreationContentHandler>();

		// Token: 0x0400097F RID: 2431
		private readonly CharacterCreationState _state;

		// Token: 0x04000980 RID: 2432
		private int _stageIndex = -1;

		// Token: 0x04000982 RID: 2434
		public readonly FaceGenHistory FaceGenHistory;

		// Token: 0x04000983 RID: 2435
		private int _furthestStageIndex;
	}
}
