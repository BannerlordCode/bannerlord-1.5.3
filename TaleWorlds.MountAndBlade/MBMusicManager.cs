using System;
using System.Collections.Generic;
using System.Threading;
using psai.net;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001DB RID: 475
	public class MBMusicManager
	{
		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06001C52 RID: 7250 RVA: 0x00061968 File Offset: 0x0005FB68
		// (set) Token: 0x06001C53 RID: 7251 RVA: 0x0006196F File Offset: 0x0005FB6F
		public static MBMusicManager Current { get; private set; }

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001C54 RID: 7252 RVA: 0x00061977 File Offset: 0x0005FB77
		// (set) Token: 0x06001C55 RID: 7253 RVA: 0x0006197F File Offset: 0x0005FB7F
		public MusicMode CurrentMode { get; private set; }

		// Token: 0x06001C56 RID: 7254 RVA: 0x00061988 File Offset: 0x0005FB88
		private MBMusicManager()
		{
			if (!NativeConfig.DisableSound)
			{
				List<string> list = new List<string>();
				foreach (MbObjectXmlInformation mbObjectXmlInformation in XmlResource.MbprojXmls)
				{
					if (mbObjectXmlInformation.Id == "soln_soundtrack")
					{
						string moduleName = mbObjectXmlInformation.ModuleName;
						list.Add(moduleName);
					}
				}
				this._soundtrackLoadResult = PsaiCore.Instance.LoadSoundtrackFromProjectFile(list);
			}
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x00061A28 File Offset: 0x0005FC28
		public static bool IsCreationCompleted()
		{
			return MBMusicManager._creationCompleted;
		}

		// Token: 0x06001C58 RID: 7256 RVA: 0x00061A2F File Offset: 0x0005FC2F
		private static void ProcessCreation(object callback)
		{
			MBMusicManager.Current = new MBMusicManager();
			MusicParameters.LoadFromXml();
			MBMusicManager._creationCompleted = true;
		}

		// Token: 0x06001C59 RID: 7257 RVA: 0x00061A46 File Offset: 0x0005FC46
		public static void Create()
		{
			ThreadPool.QueueUserWorkItem(new WaitCallback(MBMusicManager.ProcessCreation));
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x00061A5C File Offset: 0x0005FC5C
		public static void Initialize()
		{
			if (!MBMusicManager._initialized)
			{
				MBMusicManager.Current._battleMode = new MBMusicManager.BattleMusicMode();
				MBMusicManager.Current._campaignMode = new MBMusicManager.CampaignMusicMode();
				MBMusicManager.Current.CurrentMode = MusicMode.Paused;
				MBMusicManager.Current._menuModeActivationTimer = 0.5f;
				MBMusicManager._initialized = true;
				if (MBMusicManager.Current._soundtrackLoadResult != PsaiResult.OK)
				{
					MBDebug.ShowMessageBox("Music files could not be loaded. The game will run without music. Please verify your game files.", "Warning", 3U);
				}
				Debug.Print("MusicManager Initialize completed.", 0, Debug.DebugColor.Green, 281474976710656UL);
			}
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x00061AE1 File Offset: 0x0005FCE1
		public void OnCampaignMusicHandlerInit(IMusicHandler campaignMusicHandler)
		{
			this._campaignMusicHandler = campaignMusicHandler;
			this._activeMusicHandler = this._campaignMusicHandler;
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x00061AF6 File Offset: 0x0005FCF6
		public void OnCampaignMusicHandlerFinalize()
		{
			this._campaignMusicHandler = null;
			this.CheckActiveHandler();
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x00061B05 File Offset: 0x0005FD05
		public void OnBattleMusicHandlerInit(IMusicHandler battleMusicHandler)
		{
			this._battleMusicHandler = battleMusicHandler;
			this._activeMusicHandler = this._battleMusicHandler;
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x00061B1A File Offset: 0x0005FD1A
		public void OnBattleMusicHandlerFinalize()
		{
			this._battleMusicHandler = null;
			this.CheckActiveHandler();
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x00061B29 File Offset: 0x0005FD29
		public void OnSilencedMusicHandlerInit(IMusicHandler silencedMusicHandler)
		{
			this._silencedMusicHandler = silencedMusicHandler;
			this._activeMusicHandler = this._silencedMusicHandler;
		}

		// Token: 0x06001C60 RID: 7264 RVA: 0x00061B3E File Offset: 0x0005FD3E
		public void OnSilencedMusicHandlerFinalize()
		{
			this._silencedMusicHandler = null;
			this.CheckActiveHandler();
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x00061B4D File Offset: 0x0005FD4D
		private void CheckActiveHandler()
		{
			IMusicHandler musicHandler;
			if ((musicHandler = this._battleMusicHandler) == null)
			{
				musicHandler = this._silencedMusicHandler ?? this._campaignMusicHandler;
			}
			this._activeMusicHandler = musicHandler;
		}

		// Token: 0x06001C62 RID: 7266 RVA: 0x00061B70 File Offset: 0x0005FD70
		private void ActivateMenuMode()
		{
			if (!this._systemPaused)
			{
				this.CurrentMode = MusicMode.Menu;
				MusicTheme musicTheme = (ModuleHelper.IsModuleActive("NavalDLC") ? MusicTheme.NavalMainTheme : MusicTheme.MainTheme);
				PsaiCore.Instance.MenuModeEnter((int)musicTheme, 0.5f);
			}
		}

		// Token: 0x06001C63 RID: 7267 RVA: 0x00061BB2 File Offset: 0x0005FDB2
		private void DeactivateMenuMode()
		{
			PsaiCore.Instance.MenuModeLeave();
			this.CurrentMode = MusicMode.Paused;
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x00061BC6 File Offset: 0x0005FDC6
		public void ActivateBattleMode()
		{
			if (!this._systemPaused)
			{
				this.CurrentMode = MusicMode.Battle;
			}
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x00061BD7 File Offset: 0x0005FDD7
		public void DeactivateBattleMode()
		{
			PsaiCore.Instance.StopMusic(true, 3f);
			this.CurrentMode = MusicMode.Paused;
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x00061BF1 File Offset: 0x0005FDF1
		public void ActivateCampaignMode()
		{
			if (!this._systemPaused)
			{
				this.CurrentMode = MusicMode.Campaign;
			}
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x00061C02 File Offset: 0x0005FE02
		public void DeactivateCampaignMode()
		{
			PsaiCore.Instance.StopMusic(true, 3f);
			this.CurrentMode = MusicMode.Paused;
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x00061C1C File Offset: 0x0005FE1C
		public void DeactivateCurrentMode()
		{
			switch (this.CurrentMode)
			{
			case MusicMode.Menu:
				break;
			case MusicMode.Campaign:
				this.DeactivateCampaignMode();
				return;
			case MusicMode.Battle:
				this.DeactivateBattleMode();
				break;
			default:
				return;
			}
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x00061C52 File Offset: 0x0005FE52
		private bool CheckMenuModeActivationTimer()
		{
			return this._menuModeActivationTimer <= 0f;
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x00061C64 File Offset: 0x0005FE64
		public void UnpauseMusicManagerSystem()
		{
			if (this._systemPaused)
			{
				this._systemPaused = false;
				this._menuModeActivationTimer = 1f;
			}
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x00061C80 File Offset: 0x0005FE80
		public void PauseMusicManagerSystem()
		{
			if (!this._systemPaused)
			{
				if (this.CurrentMode == MusicMode.Menu)
				{
					this.DeactivateMenuMode();
				}
				this._systemPaused = true;
			}
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x00061CA0 File Offset: 0x0005FEA0
		public void StartTheme(MusicTheme theme, float startIntensity, bool queueEndSegment = false)
		{
			PsaiCore.Instance.TriggerMusicTheme((int)theme, startIntensity);
			if (queueEndSegment)
			{
				PsaiCore.Instance.StopMusic(false, 3f);
			}
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x00061CC3 File Offset: 0x0005FEC3
		public void StartThemeWithConstantIntensity(MusicTheme theme, bool queueEndSegment = false)
		{
			PsaiCore.Instance.HoldCurrentIntensity(true);
			this.StartTheme(theme, 0f, queueEndSegment);
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x00061CDE File Offset: 0x0005FEDE
		public void ForceStopThemeWithFadeOut()
		{
			PsaiCore.Instance.StopMusic(true, 3f);
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x00061CF1 File Offset: 0x0005FEF1
		public void ChangeCurrentThemeIntensity(float deltaIntensity)
		{
			PsaiCore.Instance.AddToCurrentIntensity(deltaIntensity);
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x00061D00 File Offset: 0x0005FF00
		public void Update(float dt)
		{
			if (Utilities.EngineFrameNo == this._latestFrameUpdatedNo)
			{
				return;
			}
			this._latestFrameUpdatedNo = Utilities.EngineFrameNo;
			if (this._menuModeActivationTimer > 0f)
			{
				this._menuModeActivationTimer -= dt;
			}
			if (!this._systemPaused)
			{
				if (GameStateManager.Current != null && GameStateManager.Current.ActiveState != null)
				{
					GameState activeState = GameStateManager.Current.ActiveState;
					MusicMode currentMode = this.CurrentMode;
					if (currentMode != MusicMode.Paused)
					{
						if (currentMode == MusicMode.Menu)
						{
							if (!activeState.IsMusicMenuState)
							{
								this.DeactivateMenuMode();
							}
						}
					}
					else if (activeState.IsMusicMenuState && this.CheckMenuModeActivationTimer())
					{
						this.ActivateMenuMode();
					}
				}
				if (this._activeMusicHandler != null)
				{
					this._activeMusicHandler.OnUpdated(dt);
				}
			}
			PsaiCore.Instance.Update();
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x00061DBC File Offset: 0x0005FFBC
		public MusicTheme GetSiegeTheme(BasicCultureObject culture)
		{
			return this._battleMode.GetSiegeTheme(culture);
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x00061DCA File Offset: 0x0005FFCA
		public MusicTheme GetBattleTheme(BasicCultureObject culture, int battleSize, out bool isPaganBattle)
		{
			return this._battleMode.GetBattleTheme(culture, battleSize, out isPaganBattle);
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x00061DDA File Offset: 0x0005FFDA
		public MusicTheme GetBattleEndTheme(BasicCultureObject culture, bool isVictory)
		{
			return this._battleMode.GetBattleEndTheme(culture, isVictory);
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x00061DE9 File Offset: 0x0005FFE9
		public MusicTheme GetBattleTurnsOneSideTheme(BasicCultureObject culture, bool isPositive, bool isPaganBattle)
		{
			if (isPaganBattle)
			{
				if (!isPositive)
				{
					return MusicTheme.PaganTurnsNegative;
				}
				return MusicTheme.PaganTurnsPositive;
			}
			else
			{
				if (!isPositive)
				{
					return MusicTheme.BattleTurnsNegative;
				}
				return MusicTheme.BattleTurnsPositive;
			}
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x00061E00 File Offset: 0x00060000
		public MusicTheme GetCampaignMusicTheme(BasicCultureObject culture, bool isDark, bool isWarMode, bool isAtSea)
		{
			MusicTheme musicTheme = MusicTheme.None;
			if (!isDark && isWarMode)
			{
				musicTheme = this._campaignMode.GetCampaignDramaticThemeWithCulture(culture);
			}
			if (isAtSea)
			{
				musicTheme = this._campaignMode.GetSeaCampignMusic(culture);
			}
			if (musicTheme != MusicTheme.None)
			{
				return musicTheme;
			}
			return this._campaignMode.GetCampaignTheme(culture, isDark);
		}

		// Token: 0x04000978 RID: 2424
		private const string CultureEmpire = "empire";

		// Token: 0x04000979 RID: 2425
		private const string CultureSturgia = "sturgia";

		// Token: 0x0400097A RID: 2426
		private const string CultureAserai = "aserai";

		// Token: 0x0400097B RID: 2427
		private const string CultureVlandia = "vlandia";

		// Token: 0x0400097C RID: 2428
		private const string CultureBattania = "battania";

		// Token: 0x0400097D RID: 2429
		private const string CultureKhuzait = "khuzait";

		// Token: 0x0400097E RID: 2430
		private const string CultureNord = "nord";

		// Token: 0x0400097F RID: 2431
		private const float DefaultFadeOutDurationInSeconds = 3f;

		// Token: 0x04000980 RID: 2432
		private const float MenuModeActivationTimerInSeconds = 0.5f;

		// Token: 0x04000983 RID: 2435
		private MBMusicManager.BattleMusicMode _battleMode;

		// Token: 0x04000984 RID: 2436
		private MBMusicManager.CampaignMusicMode _campaignMode;

		// Token: 0x04000985 RID: 2437
		private IMusicHandler _campaignMusicHandler;

		// Token: 0x04000986 RID: 2438
		private IMusicHandler _battleMusicHandler;

		// Token: 0x04000987 RID: 2439
		private IMusicHandler _silencedMusicHandler;

		// Token: 0x04000988 RID: 2440
		private IMusicHandler _activeMusicHandler;

		// Token: 0x04000989 RID: 2441
		private static bool _initialized;

		// Token: 0x0400098A RID: 2442
		private static bool _creationCompleted;

		// Token: 0x0400098B RID: 2443
		private float _menuModeActivationTimer;

		// Token: 0x0400098C RID: 2444
		private bool _systemPaused;

		// Token: 0x0400098D RID: 2445
		private int _latestFrameUpdatedNo = -1;

		// Token: 0x0400098E RID: 2446
		private PsaiResult _soundtrackLoadResult = PsaiResult.OK;

		// Token: 0x02000516 RID: 1302
		private class CampaignMusicMode
		{
			// Token: 0x06003C9A RID: 15514 RVA: 0x000F35F2 File Offset: 0x000F17F2
			public CampaignMusicMode()
			{
				this._factionSpecificCampaignThemeSelectionFactor = 0.35f;
				this._factionSpecificCampaignDramaticThemeSelectionFactor = 0.35f;
			}

			// Token: 0x06003C9B RID: 15515 RVA: 0x000F3610 File Offset: 0x000F1810
			public MusicTheme GetCampaignTheme(BasicCultureObject culture, bool isDark)
			{
				if (isDark)
				{
					return MusicTheme.CampaignDark;
				}
				MusicTheme campaignThemeWithCulture = this.GetCampaignThemeWithCulture(culture);
				MusicTheme musicTheme;
				if (campaignThemeWithCulture == MusicTheme.None)
				{
					musicTheme = MusicTheme.CampaignStandard;
					this._factionSpecificCampaignThemeSelectionFactor += 0.1f;
					MBMath.ClampUnit(ref this._factionSpecificCampaignThemeSelectionFactor);
				}
				else
				{
					musicTheme = campaignThemeWithCulture;
					this._factionSpecificCampaignThemeSelectionFactor -= 0.1f;
					MBMath.ClampUnit(ref this._factionSpecificCampaignThemeSelectionFactor);
				}
				return musicTheme;
			}

			// Token: 0x06003C9C RID: 15516 RVA: 0x000F3670 File Offset: 0x000F1870
			private MusicTheme GetCampaignThemeWithCulture(BasicCultureObject culture)
			{
				if (MBRandom.NondeterministicRandomFloat <= this._factionSpecificCampaignThemeSelectionFactor)
				{
					this._factionSpecificCampaignThemeSelectionFactor -= 0.1f;
					MBMath.ClampUnit(ref this._factionSpecificCampaignThemeSelectionFactor);
					if (culture.StringId == "empire")
					{
						if (MBRandom.NondeterministicRandomFloat >= 0.5f)
						{
							return MusicTheme.EmpireCampaignB;
						}
						return MusicTheme.EmpireCampaignA;
					}
					else
					{
						if (culture.StringId == "sturgia")
						{
							return MusicTheme.SturgiaCampaignA;
						}
						if (culture.StringId == "aserai")
						{
							return MusicTheme.AseraiCampaignA;
						}
						if (culture.StringId == "vlandia")
						{
							return MusicTheme.VlandiaCampaignA;
						}
						if (culture.StringId == "khuzait")
						{
							return MusicTheme.KhuzaitCampaignA;
						}
						if (culture.StringId == "battania")
						{
							return MusicTheme.BattaniaCampaignA;
						}
						if (culture.StringId == "nord")
						{
							return MusicTheme.NordCampaign;
						}
					}
				}
				return MusicTheme.None;
			}

			// Token: 0x06003C9D RID: 15517 RVA: 0x000F3750 File Offset: 0x000F1950
			public MusicTheme GetCampaignDramaticThemeWithCulture(BasicCultureObject culture)
			{
				if (MBRandom.NondeterministicRandomFloat <= this._factionSpecificCampaignDramaticThemeSelectionFactor)
				{
					this._factionSpecificCampaignDramaticThemeSelectionFactor -= 0.1f;
					MBMath.ClampUnit(ref this._factionSpecificCampaignDramaticThemeSelectionFactor);
					if (culture.StringId == "empire")
					{
						return MusicTheme.EmpireCampaignDramatic;
					}
					if (culture.StringId == "sturgia")
					{
						return MusicTheme.SturgiaCampaignDramatic;
					}
					if (culture.StringId == "aserai")
					{
						return MusicTheme.AseraiCampaignDramatic;
					}
					if (culture.StringId == "vlandia")
					{
						return MusicTheme.VlandiaCampaignDramatic;
					}
					if (culture.StringId == "khuzait")
					{
						return MusicTheme.KhuzaitCampaignDramatic;
					}
					if (culture.StringId == "battania")
					{
						return MusicTheme.BattaniaCampaignDramatic;
					}
					if (culture.StringId == "nord")
					{
						return MusicTheme.NordCampaign;
					}
				}
				this._factionSpecificCampaignDramaticThemeSelectionFactor += 0.1f;
				MBMath.ClampUnit(ref this._factionSpecificCampaignDramaticThemeSelectionFactor);
				return MusicTheme.None;
			}

			// Token: 0x06003C9E RID: 15518 RVA: 0x000F3840 File Offset: 0x000F1A40
			public MusicTheme GetSeaCampignMusic(BasicCultureObject culture)
			{
				if (culture.StringId == "sturgia" || culture.StringId == "battania" || culture.StringId == "nord")
				{
					return MusicTheme.SeaCampaignNorthern;
				}
				if (culture.StringId == "aserai" || culture.StringId == "vlandia" || culture.StringId == "khuzait" || culture.StringId == "empire")
				{
					return MusicTheme.SeaCampaignSouthern;
				}
				return MusicTheme.None;
			}

			// Token: 0x04001D5B RID: 7515
			private const float DefaultSelectionFactorForFactionSpecificCampaignTheme = 0.35f;

			// Token: 0x04001D5C RID: 7516
			private const float SelectionFactorDecayAmountForFactionSpecificCampaignTheme = 0.1f;

			// Token: 0x04001D5D RID: 7517
			private const float SelectionFactorGrowthAmountForFactionSpecificCampaignTheme = 0.1f;

			// Token: 0x04001D5E RID: 7518
			private float _factionSpecificCampaignThemeSelectionFactor;

			// Token: 0x04001D5F RID: 7519
			private float _factionSpecificCampaignDramaticThemeSelectionFactor;
		}

		// Token: 0x02000517 RID: 1303
		private class BattleMusicMode
		{
			// Token: 0x06003C9F RID: 15519 RVA: 0x000F38D8 File Offset: 0x000F1AD8
			public BattleMusicMode()
			{
				this._factionSpecificBattleThemeSelectionFactor = 0.35f;
				this._factionSpecificSiegeThemeSelectionFactor = 0.35f;
			}

			// Token: 0x06003CA0 RID: 15520 RVA: 0x000F38F8 File Offset: 0x000F1AF8
			private MusicTheme GetBattleThemeWithCulture(BasicCultureObject culture, out bool isPaganBattle)
			{
				isPaganBattle = false;
				MusicTheme musicTheme = MusicTheme.None;
				if (MBRandom.NondeterministicRandomFloat <= this._factionSpecificBattleThemeSelectionFactor)
				{
					this._factionSpecificBattleThemeSelectionFactor -= 0.1f;
					MBMath.ClampUnit(ref this._factionSpecificBattleThemeSelectionFactor);
					if (culture.StringId == "sturgia" || culture.StringId == "aserai" || culture.StringId == "khuzait" || culture.StringId == "battania")
					{
						isPaganBattle = true;
						musicTheme = ((MBRandom.NondeterministicRandomFloat < 0.5f) ? MusicTheme.BattlePaganA : MusicTheme.BattlePaganB);
					}
					else if (culture.StringId == "nord")
					{
						musicTheme = MusicTheme.BattleNord;
					}
					else
					{
						musicTheme = ((MBRandom.NondeterministicRandomFloat < 0.5f) ? MusicTheme.CombatA : MusicTheme.CombatB);
					}
				}
				return musicTheme;
			}

			// Token: 0x06003CA1 RID: 15521 RVA: 0x000F39C4 File Offset: 0x000F1BC4
			private MusicTheme GetSiegeThemeWithCulture(BasicCultureObject culture)
			{
				MusicTheme musicTheme = MusicTheme.None;
				if (MBRandom.NondeterministicRandomFloat <= this._factionSpecificSiegeThemeSelectionFactor)
				{
					this._factionSpecificSiegeThemeSelectionFactor -= 0.1f;
					MBMath.ClampUnit(ref this._factionSpecificSiegeThemeSelectionFactor);
					if (culture.StringId == "sturgia" || culture.StringId == "aserai" || culture.StringId == "khuzait" || culture.StringId == "battania")
					{
						musicTheme = MusicTheme.PaganSiege;
					}
				}
				return musicTheme;
			}

			// Token: 0x06003CA2 RID: 15522 RVA: 0x000F3A4C File Offset: 0x000F1C4C
			private MusicTheme GetVictoryThemeForCulture(BasicCultureObject culture)
			{
				if (MBRandom.NondeterministicRandomFloat <= 0.65f)
				{
					if (culture.StringId == "empire")
					{
						return MusicTheme.EmpireVictory;
					}
					if (culture.StringId == "sturgia")
					{
						return MusicTheme.SturgiaVictory;
					}
					if (culture.StringId == "nord")
					{
						return MusicTheme.NordVictory;
					}
					if (culture.StringId == "aserai")
					{
						return MusicTheme.AseraiVictory;
					}
					if (culture.StringId == "vlandia")
					{
						return MusicTheme.VlandiaVictory;
					}
					if (culture.StringId == "khuzait")
					{
						return MusicTheme.KhuzaitVictory;
					}
					if (culture.StringId == "battania")
					{
						return MusicTheme.BattaniaVictory;
					}
				}
				return MusicTheme.None;
			}

			// Token: 0x06003CA3 RID: 15523 RVA: 0x000F3B00 File Offset: 0x000F1D00
			public MusicTheme GetBattleTheme(BasicCultureObject culture, int battleSize, out bool isPaganBattle)
			{
				MusicTheme battleThemeWithCulture = this.GetBattleThemeWithCulture(culture, out isPaganBattle);
				MusicTheme musicTheme;
				if (battleThemeWithCulture == MusicTheme.None)
				{
					musicTheme = (((float)battleSize < (float)MusicParameters.SmallBattleTreshold - (float)MusicParameters.SmallBattleTreshold * 0.2f * MBRandom.NondeterministicRandomFloat) ? MusicTheme.BattleSmall : MusicTheme.BattleMedium);
					this._factionSpecificBattleThemeSelectionFactor += 0.1f;
					MBMath.ClampUnit(ref this._factionSpecificBattleThemeSelectionFactor);
				}
				else
				{
					musicTheme = battleThemeWithCulture;
					this._factionSpecificBattleThemeSelectionFactor -= 0.1f;
					MBMath.ClampUnit(ref this._factionSpecificBattleThemeSelectionFactor);
				}
				return musicTheme;
			}

			// Token: 0x06003CA4 RID: 15524 RVA: 0x000F3B80 File Offset: 0x000F1D80
			public MusicTheme GetSiegeTheme(BasicCultureObject culture)
			{
				MusicTheme siegeThemeWithCulture = this.GetSiegeThemeWithCulture(culture);
				MusicTheme musicTheme;
				if (siegeThemeWithCulture == MusicTheme.None)
				{
					musicTheme = MusicTheme.BattleSiege;
					this._factionSpecificSiegeThemeSelectionFactor += 0.1f;
					MBMath.ClampUnit(ref this._factionSpecificSiegeThemeSelectionFactor);
				}
				else
				{
					musicTheme = siegeThemeWithCulture;
					this._factionSpecificSiegeThemeSelectionFactor -= 0.1f;
					MBMath.ClampUnit(ref this._factionSpecificSiegeThemeSelectionFactor);
				}
				return musicTheme;
			}

			// Token: 0x06003CA5 RID: 15525 RVA: 0x000F3BDC File Offset: 0x000F1DDC
			public MusicTheme GetBattleEndTheme(BasicCultureObject culture, bool isVictorious)
			{
				MusicTheme musicTheme;
				if (isVictorious)
				{
					MusicTheme victoryThemeForCulture = this.GetVictoryThemeForCulture(culture);
					if (victoryThemeForCulture == MusicTheme.None)
					{
						musicTheme = MusicTheme.BattleVictory;
					}
					else
					{
						musicTheme = victoryThemeForCulture;
					}
				}
				else
				{
					musicTheme = MusicTheme.BattleDefeat;
				}
				return musicTheme;
			}

			// Token: 0x04001D60 RID: 7520
			private const float DefaultSelectionFactorForFactionSpecificBattleTheme = 0.35f;

			// Token: 0x04001D61 RID: 7521
			private const float SelectionFactorDecayAmountForFactionSpecificBattleTheme = 0.1f;

			// Token: 0x04001D62 RID: 7522
			private const float SelectionFactorGrowthAmountForFactionSpecificBattleTheme = 0.1f;

			// Token: 0x04001D63 RID: 7523
			private const float DefaultSelectionFactorForFactionSpecificVictoryTheme = 0.65f;

			// Token: 0x04001D64 RID: 7524
			private float _factionSpecificBattleThemeSelectionFactor;

			// Token: 0x04001D65 RID: 7525
			private float _factionSpecificSiegeThemeSelectionFactor;
		}
	}
}
