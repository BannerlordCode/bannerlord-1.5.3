using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200030E RID: 782
	public class MultiplayerClassDivisions
	{
		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06002CE7 RID: 11495 RVA: 0x000ACB97 File Offset: 0x000AAD97
		// (set) Token: 0x06002CE8 RID: 11496 RVA: 0x000ACB9E File Offset: 0x000AAD9E
		public static List<MultiplayerClassDivisions.MPHeroClassGroup> MultiplayerHeroClassGroups { get; private set; }

		// Token: 0x06002CE9 RID: 11497 RVA: 0x000ACBA8 File Offset: 0x000AADA8
		public static IEnumerable<MultiplayerClassDivisions.MPHeroClass> GetMPHeroClasses(BasicCultureObject culture)
		{
			return from x in MBObjectManager.Instance.GetObjectTypeList<MultiplayerClassDivisions.MPHeroClass>()
				where x.Culture == culture
				select x;
		}

		// Token: 0x06002CEA RID: 11498 RVA: 0x000ACBDD File Offset: 0x000AADDD
		public static MBReadOnlyList<MultiplayerClassDivisions.MPHeroClass> GetMPHeroClasses()
		{
			return MBObjectManager.Instance.GetObjectTypeList<MultiplayerClassDivisions.MPHeroClass>();
		}

		// Token: 0x06002CEB RID: 11499 RVA: 0x000ACBEC File Offset: 0x000AADEC
		public static MultiplayerClassDivisions.MPHeroClass GetMPHeroClassForCharacter(BasicCharacterObject character)
		{
			return MBObjectManager.Instance.GetObjectTypeList<MultiplayerClassDivisions.MPHeroClass>().FirstOrDefault<MultiplayerClassDivisions.MPHeroClass>((MultiplayerClassDivisions.MPHeroClass x) => x.HeroCharacter == character || x.TroopCharacter == character);
		}

		// Token: 0x06002CEC RID: 11500 RVA: 0x000ACC24 File Offset: 0x000AAE24
		public static List<List<IReadOnlyPerkObject>> GetAllPerksForHeroClass(MultiplayerClassDivisions.MPHeroClass heroClass, string forcedForGameMode = null)
		{
			List<List<IReadOnlyPerkObject>> list = new List<List<IReadOnlyPerkObject>>();
			for (int i = 0; i < 3; i++)
			{
				list.Add(heroClass.GetAllAvailablePerksForListIndex(i, forcedForGameMode).ToList<IReadOnlyPerkObject>());
			}
			return list;
		}

		// Token: 0x06002CED RID: 11501 RVA: 0x000ACC58 File Offset: 0x000AAE58
		public static MultiplayerClassDivisions.MPHeroClass GetMPHeroClassForPeer(MissionPeer peer, bool skipTeamCheck = false)
		{
			Team team = peer.Team;
			if ((!skipTeamCheck && (team == null || team.Side == BattleSideEnum.None)) || (peer.SelectedTroopIndex < 0 && peer.ControlledAgent == null))
			{
				return null;
			}
			if (peer.ControlledAgent != null)
			{
				return MultiplayerClassDivisions.GetMPHeroClassForCharacter(peer.ControlledAgent.Character);
			}
			if (peer.SelectedTroopIndex >= 0)
			{
				return MultiplayerClassDivisions.GetMPHeroClasses(peer.Culture).ToList<MultiplayerClassDivisions.MPHeroClass>()[peer.SelectedTroopIndex];
			}
			Debug.FailedAssert("This should not be seen.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Network\\Gameplay\\MultiplayerClassDivisions.cs", "GetMPHeroClassForPeer", 255);
			return null;
		}

		// Token: 0x06002CEE RID: 11502 RVA: 0x000ACCE8 File Offset: 0x000AAEE8
		public static TargetIconType GetMPHeroClassForFormation(Formation formation)
		{
			switch (formation.PhysicalClass)
			{
			case FormationClass.Infantry:
				return TargetIconType.Infantry_Light;
			case FormationClass.Ranged:
				return TargetIconType.Archer_Light;
			case FormationClass.Cavalry:
				return TargetIconType.Cavalry_Light;
			default:
				return TargetIconType.HorseArcher_Light;
			}
		}

		// Token: 0x06002CEF RID: 11503 RVA: 0x000ACD18 File Offset: 0x000AAF18
		public static List<List<IReadOnlyPerkObject>> GetAvailablePerksForPeer(MissionPeer missionPeer)
		{
			if (((missionPeer != null) ? missionPeer.Team : null) != null)
			{
				return MultiplayerClassDivisions.GetAllPerksForHeroClass(MultiplayerClassDivisions.GetMPHeroClassForPeer(missionPeer, false), null);
			}
			return new List<List<IReadOnlyPerkObject>>();
		}

		// Token: 0x06002CF0 RID: 11504 RVA: 0x000ACD3C File Offset: 0x000AAF3C
		public static void Initialize()
		{
			MultiplayerClassDivisions.MultiplayerHeroClassGroups = new List<MultiplayerClassDivisions.MPHeroClassGroup>
			{
				new MultiplayerClassDivisions.MPHeroClassGroup("Infantry"),
				new MultiplayerClassDivisions.MPHeroClassGroup("Ranged"),
				new MultiplayerClassDivisions.MPHeroClassGroup("Cavalry"),
				new MultiplayerClassDivisions.MPHeroClassGroup("HorseArcher")
			};
			MultiplayerClassDivisions.AvailableCultures = from x in MBObjectManager.Instance.GetObjectTypeList<BasicCultureObject>().ToArray()
				where x.IsMainCulture
				select x;
		}

		// Token: 0x06002CF1 RID: 11505 RVA: 0x000ACDCB File Offset: 0x000AAFCB
		public static void Release()
		{
			MultiplayerClassDivisions.MultiplayerHeroClassGroups.Clear();
			MultiplayerClassDivisions.AvailableCultures = null;
		}

		// Token: 0x06002CF2 RID: 11506 RVA: 0x000ACDDD File Offset: 0x000AAFDD
		private static BasicCharacterObject GetMPCharacter(string stringId)
		{
			return MBObjectManager.Instance.GetObject<BasicCharacterObject>(stringId);
		}

		// Token: 0x06002CF3 RID: 11507 RVA: 0x000ACDEC File Offset: 0x000AAFEC
		public static int GetMinimumTroopCost(BasicCultureObject culture = null)
		{
			MBReadOnlyList<MultiplayerClassDivisions.MPHeroClass> mpheroClasses = MultiplayerClassDivisions.GetMPHeroClasses();
			if (culture != null)
			{
				return mpheroClasses.Where<MultiplayerClassDivisions.MPHeroClass>((MultiplayerClassDivisions.MPHeroClass c) => c.Culture == culture).Min<MultiplayerClassDivisions.MPHeroClass>((MultiplayerClassDivisions.MPHeroClass troop) => troop.TroopCost);
			}
			return mpheroClasses.Min<MultiplayerClassDivisions.MPHeroClass>((MultiplayerClassDivisions.MPHeroClass troop) => troop.TroopCost);
		}

		// Token: 0x040011BF RID: 4543
		public static IEnumerable<BasicCultureObject> AvailableCultures;

		// Token: 0x020005EF RID: 1519
		public class MPHeroClass : MBObjectBase
		{
			// Token: 0x17000AAF RID: 2735
			// (get) Token: 0x06003FA7 RID: 16295 RVA: 0x000FA323 File Offset: 0x000F8523
			// (set) Token: 0x06003FA8 RID: 16296 RVA: 0x000FA32B File Offset: 0x000F852B
			public BasicCharacterObject HeroCharacter { get; private set; }

			// Token: 0x17000AB0 RID: 2736
			// (get) Token: 0x06003FA9 RID: 16297 RVA: 0x000FA334 File Offset: 0x000F8534
			// (set) Token: 0x06003FAA RID: 16298 RVA: 0x000FA33C File Offset: 0x000F853C
			public BasicCharacterObject TroopCharacter { get; private set; }

			// Token: 0x17000AB1 RID: 2737
			// (get) Token: 0x06003FAB RID: 16299 RVA: 0x000FA345 File Offset: 0x000F8545
			// (set) Token: 0x06003FAC RID: 16300 RVA: 0x000FA34D File Offset: 0x000F854D
			public BasicCharacterObject BannerBearerCharacter { get; private set; }

			// Token: 0x17000AB2 RID: 2738
			// (get) Token: 0x06003FAD RID: 16301 RVA: 0x000FA356 File Offset: 0x000F8556
			// (set) Token: 0x06003FAE RID: 16302 RVA: 0x000FA35E File Offset: 0x000F855E
			public BasicCultureObject Culture { get; private set; }

			// Token: 0x17000AB3 RID: 2739
			// (get) Token: 0x06003FAF RID: 16303 RVA: 0x000FA367 File Offset: 0x000F8567
			// (set) Token: 0x06003FB0 RID: 16304 RVA: 0x000FA36F File Offset: 0x000F856F
			public MultiplayerClassDivisions.MPHeroClassGroup ClassGroup { get; private set; }

			// Token: 0x17000AB4 RID: 2740
			// (get) Token: 0x06003FB1 RID: 16305 RVA: 0x000FA378 File Offset: 0x000F8578
			// (set) Token: 0x06003FB2 RID: 16306 RVA: 0x000FA380 File Offset: 0x000F8580
			public string HeroIdleAnim { get; private set; }

			// Token: 0x17000AB5 RID: 2741
			// (get) Token: 0x06003FB3 RID: 16307 RVA: 0x000FA389 File Offset: 0x000F8589
			// (set) Token: 0x06003FB4 RID: 16308 RVA: 0x000FA391 File Offset: 0x000F8591
			public string HeroMountIdleAnim { get; private set; }

			// Token: 0x17000AB6 RID: 2742
			// (get) Token: 0x06003FB5 RID: 16309 RVA: 0x000FA39A File Offset: 0x000F859A
			// (set) Token: 0x06003FB6 RID: 16310 RVA: 0x000FA3A2 File Offset: 0x000F85A2
			public string TroopIdleAnim { get; private set; }

			// Token: 0x17000AB7 RID: 2743
			// (get) Token: 0x06003FB7 RID: 16311 RVA: 0x000FA3AB File Offset: 0x000F85AB
			// (set) Token: 0x06003FB8 RID: 16312 RVA: 0x000FA3B3 File Offset: 0x000F85B3
			public string TroopMountIdleAnim { get; private set; }

			// Token: 0x17000AB8 RID: 2744
			// (get) Token: 0x06003FB9 RID: 16313 RVA: 0x000FA3BC File Offset: 0x000F85BC
			// (set) Token: 0x06003FBA RID: 16314 RVA: 0x000FA3C4 File Offset: 0x000F85C4
			public int ArmorValue { get; private set; }

			// Token: 0x17000AB9 RID: 2745
			// (get) Token: 0x06003FBB RID: 16315 RVA: 0x000FA3CD File Offset: 0x000F85CD
			// (set) Token: 0x06003FBC RID: 16316 RVA: 0x000FA3D5 File Offset: 0x000F85D5
			public int Health { get; private set; }

			// Token: 0x17000ABA RID: 2746
			// (get) Token: 0x06003FBD RID: 16317 RVA: 0x000FA3DE File Offset: 0x000F85DE
			// (set) Token: 0x06003FBE RID: 16318 RVA: 0x000FA3E6 File Offset: 0x000F85E6
			public float HeroMovementSpeedMultiplier { get; private set; }

			// Token: 0x17000ABB RID: 2747
			// (get) Token: 0x06003FBF RID: 16319 RVA: 0x000FA3EF File Offset: 0x000F85EF
			// (set) Token: 0x06003FC0 RID: 16320 RVA: 0x000FA3F7 File Offset: 0x000F85F7
			public float HeroCombatMovementSpeedMultiplier { get; private set; }

			// Token: 0x17000ABC RID: 2748
			// (get) Token: 0x06003FC1 RID: 16321 RVA: 0x000FA400 File Offset: 0x000F8600
			// (set) Token: 0x06003FC2 RID: 16322 RVA: 0x000FA408 File Offset: 0x000F8608
			public float HeroTopSpeedReachDuration { get; private set; }

			// Token: 0x17000ABD RID: 2749
			// (get) Token: 0x06003FC3 RID: 16323 RVA: 0x000FA411 File Offset: 0x000F8611
			// (set) Token: 0x06003FC4 RID: 16324 RVA: 0x000FA419 File Offset: 0x000F8619
			public float TroopMovementSpeedMultiplier { get; private set; }

			// Token: 0x17000ABE RID: 2750
			// (get) Token: 0x06003FC5 RID: 16325 RVA: 0x000FA422 File Offset: 0x000F8622
			// (set) Token: 0x06003FC6 RID: 16326 RVA: 0x000FA42A File Offset: 0x000F862A
			public float TroopCombatMovementSpeedMultiplier { get; private set; }

			// Token: 0x17000ABF RID: 2751
			// (get) Token: 0x06003FC7 RID: 16327 RVA: 0x000FA433 File Offset: 0x000F8633
			// (set) Token: 0x06003FC8 RID: 16328 RVA: 0x000FA43B File Offset: 0x000F863B
			public float TroopTopSpeedReachDuration { get; private set; }

			// Token: 0x17000AC0 RID: 2752
			// (get) Token: 0x06003FC9 RID: 16329 RVA: 0x000FA444 File Offset: 0x000F8644
			// (set) Token: 0x06003FCA RID: 16330 RVA: 0x000FA44C File Offset: 0x000F864C
			public float TroopMultiplier { get; private set; }

			// Token: 0x17000AC1 RID: 2753
			// (get) Token: 0x06003FCB RID: 16331 RVA: 0x000FA455 File Offset: 0x000F8655
			// (set) Token: 0x06003FCC RID: 16332 RVA: 0x000FA45D File Offset: 0x000F865D
			public int TroopCost { get; private set; }

			// Token: 0x17000AC2 RID: 2754
			// (get) Token: 0x06003FCD RID: 16333 RVA: 0x000FA466 File Offset: 0x000F8666
			// (set) Token: 0x06003FCE RID: 16334 RVA: 0x000FA46E File Offset: 0x000F866E
			public int TroopCasualCost { get; private set; }

			// Token: 0x17000AC3 RID: 2755
			// (get) Token: 0x06003FCF RID: 16335 RVA: 0x000FA477 File Offset: 0x000F8677
			// (set) Token: 0x06003FD0 RID: 16336 RVA: 0x000FA47F File Offset: 0x000F867F
			public int TroopBattleCost { get; private set; }

			// Token: 0x17000AC4 RID: 2756
			// (get) Token: 0x06003FD1 RID: 16337 RVA: 0x000FA488 File Offset: 0x000F8688
			// (set) Token: 0x06003FD2 RID: 16338 RVA: 0x000FA490 File Offset: 0x000F8690
			public int MeleeAI { get; private set; }

			// Token: 0x17000AC5 RID: 2757
			// (get) Token: 0x06003FD3 RID: 16339 RVA: 0x000FA499 File Offset: 0x000F8699
			// (set) Token: 0x06003FD4 RID: 16340 RVA: 0x000FA4A1 File Offset: 0x000F86A1
			public int RangedAI { get; private set; }

			// Token: 0x17000AC6 RID: 2758
			// (get) Token: 0x06003FD5 RID: 16341 RVA: 0x000FA4AA File Offset: 0x000F86AA
			// (set) Token: 0x06003FD6 RID: 16342 RVA: 0x000FA4B2 File Offset: 0x000F86B2
			public TextObject HeroInformation { get; private set; }

			// Token: 0x17000AC7 RID: 2759
			// (get) Token: 0x06003FD7 RID: 16343 RVA: 0x000FA4BB File Offset: 0x000F86BB
			// (set) Token: 0x06003FD8 RID: 16344 RVA: 0x000FA4C3 File Offset: 0x000F86C3
			public TextObject TroopInformation { get; private set; }

			// Token: 0x17000AC8 RID: 2760
			// (get) Token: 0x06003FD9 RID: 16345 RVA: 0x000FA4CC File Offset: 0x000F86CC
			// (set) Token: 0x06003FDA RID: 16346 RVA: 0x000FA4D4 File Offset: 0x000F86D4
			public TargetIconType IconType { get; private set; }

			// Token: 0x17000AC9 RID: 2761
			// (get) Token: 0x06003FDB RID: 16347 RVA: 0x000FA4DD File Offset: 0x000F86DD
			public TextObject HeroName
			{
				get
				{
					return this.HeroCharacter.Name;
				}
			}

			// Token: 0x17000ACA RID: 2762
			// (get) Token: 0x06003FDC RID: 16348 RVA: 0x000FA4EA File Offset: 0x000F86EA
			public TextObject TroopName
			{
				get
				{
					return this.TroopCharacter.Name;
				}
			}

			// Token: 0x06003FDD RID: 16349 RVA: 0x000FA4F7 File Offset: 0x000F86F7
			public override bool Equals(object obj)
			{
				return obj is MultiplayerClassDivisions.MPHeroClass && ((MultiplayerClassDivisions.MPHeroClass)obj).StringId.Equals(base.StringId);
			}

			// Token: 0x06003FDE RID: 16350 RVA: 0x000FA519 File Offset: 0x000F8719
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x06003FDF RID: 16351 RVA: 0x000FA524 File Offset: 0x000F8724
			public List<IReadOnlyPerkObject> GetAllAvailablePerksForListIndex(int index, string forcedForGameMode = null)
			{
				string text = forcedForGameMode ?? MultiplayerOptions.OptionType.GameType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				List<IReadOnlyPerkObject> list = new List<IReadOnlyPerkObject>();
				foreach (IReadOnlyPerkObject readOnlyPerkObject in this._perks)
				{
					foreach (string text2 in readOnlyPerkObject.GameModes)
					{
						if ((text2.Equals(text, StringComparison.InvariantCultureIgnoreCase) || text2.Equals("all", StringComparison.InvariantCultureIgnoreCase)) && readOnlyPerkObject.PerkListIndex == index)
						{
							list.Add(readOnlyPerkObject);
							break;
						}
					}
				}
				return list;
			}

			// Token: 0x06003FE0 RID: 16352 RVA: 0x000FA5F0 File Offset: 0x000F87F0
			public override void Deserialize(MBObjectManager objectManager, XmlNode node)
			{
				base.Deserialize(objectManager, node);
				this.HeroCharacter = MultiplayerClassDivisions.GetMPCharacter(node.Attributes["hero"].Value);
				this.TroopCharacter = MultiplayerClassDivisions.GetMPCharacter(node.Attributes["troop"].Value);
				XmlAttribute xmlAttribute = node.Attributes["banner_bearer"];
				string text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				if (text != null)
				{
					this.BannerBearerCharacter = MultiplayerClassDivisions.GetMPCharacter(text);
				}
				XmlAttribute xmlAttribute2 = node.Attributes["hero_idle_anim"];
				this.HeroIdleAnim = ((xmlAttribute2 != null) ? xmlAttribute2.Value : null);
				XmlAttribute xmlAttribute3 = node.Attributes["hero_mount_idle_anim"];
				this.HeroMountIdleAnim = ((xmlAttribute3 != null) ? xmlAttribute3.Value : null);
				XmlAttribute xmlAttribute4 = node.Attributes["troop_idle_anim"];
				this.TroopIdleAnim = ((xmlAttribute4 != null) ? xmlAttribute4.Value : null);
				XmlAttribute xmlAttribute5 = node.Attributes["troop_mount_idle_anim"];
				this.TroopMountIdleAnim = ((xmlAttribute5 != null) ? xmlAttribute5.Value : null);
				this.Culture = this.HeroCharacter.Culture;
				this.ClassGroup = new MultiplayerClassDivisions.MPHeroClassGroup(this.HeroCharacter.DefaultFormationClass.GetName());
				this.TroopMultiplier = (float)Convert.ToDouble(node.Attributes["multiplier"].Value);
				this.TroopCost = Convert.ToInt32(node.Attributes["cost"].Value);
				this.ArmorValue = Convert.ToInt32(node.Attributes["armor"].Value);
				XmlAttribute xmlAttribute6 = node.Attributes["casual_cost"];
				XmlAttribute xmlAttribute7 = node.Attributes["battle_cost"];
				this.TroopCasualCost = ((xmlAttribute6 != null) ? Convert.ToInt32(node.Attributes["casual_cost"].Value) : this.TroopCost);
				this.TroopBattleCost = ((xmlAttribute7 != null) ? Convert.ToInt32(node.Attributes["battle_cost"].Value) : this.TroopCost);
				this.Health = 100;
				this.MeleeAI = 50;
				this.RangedAI = 50;
				XmlNode xmlNode = node.Attributes["hitpoints"];
				if (xmlNode != null)
				{
					this.Health = Convert.ToInt32(xmlNode.Value);
				}
				this.HeroMovementSpeedMultiplier = (float)Convert.ToDouble(node.Attributes["movement_speed"].Value);
				this.HeroCombatMovementSpeedMultiplier = (float)Convert.ToDouble(node.Attributes["combat_movement_speed"].Value);
				this.HeroTopSpeedReachDuration = (float)Convert.ToDouble(node.Attributes["acceleration"].Value);
				XmlAttribute xmlAttribute8 = node.Attributes["troop_movement_speed"];
				XmlAttribute xmlAttribute9 = node.Attributes["troop_combat_movement_speed"];
				XmlAttribute xmlAttribute10 = node.Attributes["troop_acceleration"];
				this.TroopMovementSpeedMultiplier = ((xmlAttribute8 != null) ? ((float)Convert.ToDouble(xmlAttribute8.Value)) : this.HeroMovementSpeedMultiplier);
				this.TroopCombatMovementSpeedMultiplier = ((xmlAttribute9 != null) ? ((float)Convert.ToDouble(xmlAttribute9.Value)) : this.HeroCombatMovementSpeedMultiplier);
				this.TroopTopSpeedReachDuration = ((xmlAttribute10 != null) ? ((float)Convert.ToDouble(xmlAttribute10.Value)) : this.HeroTopSpeedReachDuration);
				this.MeleeAI = Convert.ToInt32(node.Attributes["melee_ai"].Value);
				this.RangedAI = Convert.ToInt32(node.Attributes["ranged_ai"].Value);
				TargetIconType targetIconType;
				if (Enum.TryParse<TargetIconType>(node.Attributes["icon"].Value, true, out targetIconType))
				{
					this.IconType = targetIconType;
				}
				foreach (object obj in node.ChildNodes)
				{
					XmlNode xmlNode2 = (XmlNode)obj;
					if (xmlNode2.NodeType != XmlNodeType.Comment && xmlNode2.Name == "Perks")
					{
						this._perks = new List<IReadOnlyPerkObject>();
						foreach (object obj2 in xmlNode2.ChildNodes)
						{
							XmlNode xmlNode3 = (XmlNode)obj2;
							if (xmlNode3.NodeType != XmlNodeType.Comment)
							{
								this._perks.Add(MPPerkObject.Deserialize(xmlNode3));
							}
						}
					}
				}
			}

			// Token: 0x06003FE1 RID: 16353 RVA: 0x000FAA7C File Offset: 0x000F8C7C
			public bool IsTroopCharacter(BasicCharacterObject character)
			{
				return this.TroopCharacter == character;
			}

			// Token: 0x04002038 RID: 8248
			private List<IReadOnlyPerkObject> _perks = new List<IReadOnlyPerkObject>();
		}

		// Token: 0x020005F0 RID: 1520
		public class MPHeroClassGroup
		{
			// Token: 0x06003FE3 RID: 16355 RVA: 0x000FAA9A File Offset: 0x000F8C9A
			public MPHeroClassGroup(string stringId)
			{
				this.StringId = stringId;
				this.Name = GameTexts.FindText("str_troop_type_name", this.StringId);
			}

			// Token: 0x06003FE4 RID: 16356 RVA: 0x000FAABF File Offset: 0x000F8CBF
			public override bool Equals(object obj)
			{
				return obj is MultiplayerClassDivisions.MPHeroClassGroup && ((MultiplayerClassDivisions.MPHeroClassGroup)obj).StringId.Equals(this.StringId);
			}

			// Token: 0x06003FE5 RID: 16357 RVA: 0x000FAAE1 File Offset: 0x000F8CE1
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x04002039 RID: 8249
			public readonly string StringId;

			// Token: 0x0400203A RID: 8250
			public readonly TextObject Name;
		}
	}
}
