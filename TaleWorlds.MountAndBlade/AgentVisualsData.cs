using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002CD RID: 717
	public class AgentVisualsData
	{
		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06002962 RID: 10594 RVA: 0x0009D458 File Offset: 0x0009B658
		// (set) Token: 0x06002963 RID: 10595 RVA: 0x0009D460 File Offset: 0x0009B660
		public MBActionSet ActionSetData { get; private set; }

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x06002964 RID: 10596 RVA: 0x0009D469 File Offset: 0x0009B669
		// (set) Token: 0x06002965 RID: 10597 RVA: 0x0009D471 File Offset: 0x0009B671
		public MatrixFrame FrameData { get; private set; }

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06002966 RID: 10598 RVA: 0x0009D47A File Offset: 0x0009B67A
		// (set) Token: 0x06002967 RID: 10599 RVA: 0x0009D482 File Offset: 0x0009B682
		public BodyProperties BodyPropertiesData { get; private set; }

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06002968 RID: 10600 RVA: 0x0009D48B File Offset: 0x0009B68B
		// (set) Token: 0x06002969 RID: 10601 RVA: 0x0009D493 File Offset: 0x0009B693
		public Equipment EquipmentData { get; private set; }

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x0600296A RID: 10602 RVA: 0x0009D49C File Offset: 0x0009B69C
		// (set) Token: 0x0600296B RID: 10603 RVA: 0x0009D4A4 File Offset: 0x0009B6A4
		public int RightWieldedItemIndexData { get; private set; }

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x0600296C RID: 10604 RVA: 0x0009D4AD File Offset: 0x0009B6AD
		// (set) Token: 0x0600296D RID: 10605 RVA: 0x0009D4B5 File Offset: 0x0009B6B5
		public int LeftWieldedItemIndexData { get; private set; }

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x0600296E RID: 10606 RVA: 0x0009D4BE File Offset: 0x0009B6BE
		// (set) Token: 0x0600296F RID: 10607 RVA: 0x0009D4C6 File Offset: 0x0009B6C6
		public SkeletonType SkeletonTypeData { get; private set; }

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x06002970 RID: 10608 RVA: 0x0009D4CF File Offset: 0x0009B6CF
		// (set) Token: 0x06002971 RID: 10609 RVA: 0x0009D4D7 File Offset: 0x0009B6D7
		public Banner BannerData { get; private set; }

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x06002972 RID: 10610 RVA: 0x0009D4E0 File Offset: 0x0009B6E0
		// (set) Token: 0x06002973 RID: 10611 RVA: 0x0009D4E8 File Offset: 0x0009B6E8
		public GameEntity CachedWeaponSlot0Entity { get; private set; }

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x06002974 RID: 10612 RVA: 0x0009D4F1 File Offset: 0x0009B6F1
		// (set) Token: 0x06002975 RID: 10613 RVA: 0x0009D4F9 File Offset: 0x0009B6F9
		public GameEntity CachedWeaponSlot1Entity { get; private set; }

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x06002976 RID: 10614 RVA: 0x0009D502 File Offset: 0x0009B702
		// (set) Token: 0x06002977 RID: 10615 RVA: 0x0009D50A File Offset: 0x0009B70A
		public GameEntity CachedWeaponSlot2Entity { get; private set; }

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06002978 RID: 10616 RVA: 0x0009D513 File Offset: 0x0009B713
		// (set) Token: 0x06002979 RID: 10617 RVA: 0x0009D51B File Offset: 0x0009B71B
		public GameEntity CachedWeaponSlot3Entity { get; private set; }

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x0600297A RID: 10618 RVA: 0x0009D524 File Offset: 0x0009B724
		// (set) Token: 0x0600297B RID: 10619 RVA: 0x0009D52C File Offset: 0x0009B72C
		public GameEntity CachedWeaponSlot4Entity { get; private set; }

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x0600297C RID: 10620 RVA: 0x0009D535 File Offset: 0x0009B735
		// (set) Token: 0x0600297D RID: 10621 RVA: 0x0009D53D File Offset: 0x0009B73D
		public Scene SceneData { get; private set; }

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x0600297E RID: 10622 RVA: 0x0009D546 File Offset: 0x0009B746
		// (set) Token: 0x0600297F RID: 10623 RVA: 0x0009D54E File Offset: 0x0009B74E
		public Monster MonsterData { get; private set; }

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x06002980 RID: 10624 RVA: 0x0009D557 File Offset: 0x0009B757
		// (set) Token: 0x06002981 RID: 10625 RVA: 0x0009D55F File Offset: 0x0009B75F
		public bool PrepareImmediatelyData { get; private set; }

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06002982 RID: 10626 RVA: 0x0009D568 File Offset: 0x0009B768
		// (set) Token: 0x06002983 RID: 10627 RVA: 0x0009D570 File Offset: 0x0009B770
		public bool UseScaledWeaponsData { get; private set; }

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06002984 RID: 10628 RVA: 0x0009D579 File Offset: 0x0009B779
		// (set) Token: 0x06002985 RID: 10629 RVA: 0x0009D581 File Offset: 0x0009B781
		public bool UseTranslucencyData { get; private set; }

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06002986 RID: 10630 RVA: 0x0009D58A File Offset: 0x0009B78A
		// (set) Token: 0x06002987 RID: 10631 RVA: 0x0009D592 File Offset: 0x0009B792
		public bool UseTesselationData { get; private set; }

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06002988 RID: 10632 RVA: 0x0009D59B File Offset: 0x0009B79B
		// (set) Token: 0x06002989 RID: 10633 RVA: 0x0009D5A3 File Offset: 0x0009B7A3
		public bool UseMorphAnimsData { get; private set; }

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x0600298A RID: 10634 RVA: 0x0009D5AC File Offset: 0x0009B7AC
		// (set) Token: 0x0600298B RID: 10635 RVA: 0x0009D5B4 File Offset: 0x0009B7B4
		public uint ClothColor1Data { get; private set; }

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x0600298C RID: 10636 RVA: 0x0009D5BD File Offset: 0x0009B7BD
		// (set) Token: 0x0600298D RID: 10637 RVA: 0x0009D5C5 File Offset: 0x0009B7C5
		public uint ClothColor2Data { get; private set; }

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x0600298E RID: 10638 RVA: 0x0009D5CE File Offset: 0x0009B7CE
		// (set) Token: 0x0600298F RID: 10639 RVA: 0x0009D5D6 File Offset: 0x0009B7D6
		public float ScaleData { get; private set; }

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06002990 RID: 10640 RVA: 0x0009D5DF File Offset: 0x0009B7DF
		// (set) Token: 0x06002991 RID: 10641 RVA: 0x0009D5E7 File Offset: 0x0009B7E7
		public string CharacterObjectStringIdData { get; private set; }

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06002992 RID: 10642 RVA: 0x0009D5F0 File Offset: 0x0009B7F0
		// (set) Token: 0x06002993 RID: 10643 RVA: 0x0009D5F8 File Offset: 0x0009B7F8
		public ActionIndexCache ActionCodeData { get; private set; } = ActionIndexCache.act_none;

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06002994 RID: 10644 RVA: 0x0009D601 File Offset: 0x0009B801
		// (set) Token: 0x06002995 RID: 10645 RVA: 0x0009D609 File Offset: 0x0009B809
		public GameEntity EntityData { get; private set; }

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x06002996 RID: 10646 RVA: 0x0009D612 File Offset: 0x0009B812
		// (set) Token: 0x06002997 RID: 10647 RVA: 0x0009D61A File Offset: 0x0009B81A
		public bool HasClippingPlaneData { get; private set; }

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x06002998 RID: 10648 RVA: 0x0009D623 File Offset: 0x0009B823
		// (set) Token: 0x06002999 RID: 10649 RVA: 0x0009D62B File Offset: 0x0009B82B
		public string MountCreationKeyData { get; private set; }

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x0600299A RID: 10650 RVA: 0x0009D634 File Offset: 0x0009B834
		// (set) Token: 0x0600299B RID: 10651 RVA: 0x0009D63C File Offset: 0x0009B83C
		public bool AddColorRandomnessData { get; private set; }

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x0600299C RID: 10652 RVA: 0x0009D645 File Offset: 0x0009B845
		// (set) Token: 0x0600299D RID: 10653 RVA: 0x0009D64D File Offset: 0x0009B84D
		public int RaceData { get; private set; }

		// Token: 0x0600299E RID: 10654 RVA: 0x0009D658 File Offset: 0x0009B858
		public AgentVisualsData(AgentVisualsData agentVisualsData)
		{
			this.AgentVisuals = agentVisualsData.AgentVisuals;
			this.ActionSetData = agentVisualsData.ActionSetData;
			this.FrameData = agentVisualsData.FrameData;
			this.BodyPropertiesData = agentVisualsData.BodyPropertiesData;
			this.EquipmentData = agentVisualsData.EquipmentData;
			this.RightWieldedItemIndexData = agentVisualsData.RightWieldedItemIndexData;
			this.LeftWieldedItemIndexData = agentVisualsData.LeftWieldedItemIndexData;
			this.SkeletonTypeData = agentVisualsData.SkeletonTypeData;
			this.BannerData = agentVisualsData.BannerData;
			this.CachedWeaponSlot0Entity = agentVisualsData.CachedWeaponSlot0Entity;
			this.CachedWeaponSlot1Entity = agentVisualsData.CachedWeaponSlot1Entity;
			this.CachedWeaponSlot2Entity = agentVisualsData.CachedWeaponSlot2Entity;
			this.CachedWeaponSlot3Entity = agentVisualsData.CachedWeaponSlot3Entity;
			this.CachedWeaponSlot4Entity = agentVisualsData.CachedWeaponSlot4Entity;
			this.SceneData = agentVisualsData.SceneData;
			this.MonsterData = agentVisualsData.MonsterData;
			this.PrepareImmediatelyData = agentVisualsData.PrepareImmediatelyData;
			this.UseScaledWeaponsData = agentVisualsData.UseScaledWeaponsData;
			this.UseTranslucencyData = agentVisualsData.UseTranslucencyData;
			this.UseTesselationData = agentVisualsData.UseTesselationData;
			this.UseMorphAnimsData = agentVisualsData.UseMorphAnimsData;
			this.ClothColor1Data = agentVisualsData.ClothColor1Data;
			this.ClothColor2Data = agentVisualsData.ClothColor2Data;
			this.ScaleData = agentVisualsData.ScaleData;
			this.ActionCodeData = agentVisualsData.ActionCodeData;
			this.EntityData = agentVisualsData.EntityData;
			this.CharacterObjectStringIdData = agentVisualsData.CharacterObjectStringIdData;
			this.HasClippingPlaneData = agentVisualsData.HasClippingPlaneData;
			this.MountCreationKeyData = agentVisualsData.MountCreationKeyData;
			this.AddColorRandomnessData = agentVisualsData.AddColorRandomnessData;
			this.RaceData = agentVisualsData.RaceData;
		}

		// Token: 0x0600299F RID: 10655 RVA: 0x0009D7EA File Offset: 0x0009B9EA
		public AgentVisualsData()
		{
			this.ClothColor1Data = uint.MaxValue;
			this.ClothColor2Data = uint.MaxValue;
			this.RightWieldedItemIndexData = -1;
			this.LeftWieldedItemIndexData = -1;
			this.ScaleData = 0f;
		}

		// Token: 0x060029A0 RID: 10656 RVA: 0x0009D824 File Offset: 0x0009BA24
		public AgentVisualsData Equipment(Equipment equipment)
		{
			this.EquipmentData = equipment;
			return this;
		}

		// Token: 0x060029A1 RID: 10657 RVA: 0x0009D82E File Offset: 0x0009BA2E
		public AgentVisualsData BodyProperties(BodyProperties bodyProperties)
		{
			this.BodyPropertiesData = bodyProperties;
			return this;
		}

		// Token: 0x060029A2 RID: 10658 RVA: 0x0009D838 File Offset: 0x0009BA38
		public AgentVisualsData Frame(MatrixFrame frame)
		{
			this.FrameData = frame;
			return this;
		}

		// Token: 0x060029A3 RID: 10659 RVA: 0x0009D842 File Offset: 0x0009BA42
		public AgentVisualsData ActionSet(MBActionSet actionSet)
		{
			this.ActionSetData = actionSet;
			return this;
		}

		// Token: 0x060029A4 RID: 10660 RVA: 0x0009D84C File Offset: 0x0009BA4C
		public AgentVisualsData Scene(Scene scene)
		{
			this.SceneData = scene;
			return this;
		}

		// Token: 0x060029A5 RID: 10661 RVA: 0x0009D856 File Offset: 0x0009BA56
		public AgentVisualsData Monster(Monster monster)
		{
			this.MonsterData = monster;
			return this;
		}

		// Token: 0x060029A6 RID: 10662 RVA: 0x0009D860 File Offset: 0x0009BA60
		public AgentVisualsData PrepareImmediately(bool prepareImmediately)
		{
			this.PrepareImmediatelyData = prepareImmediately;
			return this;
		}

		// Token: 0x060029A7 RID: 10663 RVA: 0x0009D86A File Offset: 0x0009BA6A
		public AgentVisualsData UseScaledWeapons(bool useScaledWeapons)
		{
			this.UseScaledWeaponsData = useScaledWeapons;
			return this;
		}

		// Token: 0x060029A8 RID: 10664 RVA: 0x0009D874 File Offset: 0x0009BA74
		public AgentVisualsData SkeletonType(SkeletonType skeletonType)
		{
			this.SkeletonTypeData = skeletonType;
			return this;
		}

		// Token: 0x060029A9 RID: 10665 RVA: 0x0009D87E File Offset: 0x0009BA7E
		public AgentVisualsData UseMorphAnims(bool useMorphAnims)
		{
			this.UseMorphAnimsData = useMorphAnims;
			return this;
		}

		// Token: 0x060029AA RID: 10666 RVA: 0x0009D888 File Offset: 0x0009BA88
		public AgentVisualsData ClothColor1(uint clothColor1)
		{
			this.ClothColor1Data = clothColor1;
			return this;
		}

		// Token: 0x060029AB RID: 10667 RVA: 0x0009D892 File Offset: 0x0009BA92
		public AgentVisualsData ClothColor2(uint clothColor2)
		{
			this.ClothColor2Data = clothColor2;
			return this;
		}

		// Token: 0x060029AC RID: 10668 RVA: 0x0009D89C File Offset: 0x0009BA9C
		public AgentVisualsData Banner(Banner banner)
		{
			this.BannerData = banner;
			return this;
		}

		// Token: 0x060029AD RID: 10669 RVA: 0x0009D8A6 File Offset: 0x0009BAA6
		public AgentVisualsData Race(int race)
		{
			this.RaceData = race;
			return this;
		}

		// Token: 0x060029AE RID: 10670 RVA: 0x0009D8B0 File Offset: 0x0009BAB0
		public GameEntity GetCachedWeaponEntity(EquipmentIndex slotIndex)
		{
			switch (slotIndex)
			{
			case EquipmentIndex.WeaponItemBeginSlot:
				return this.CachedWeaponSlot0Entity;
			case EquipmentIndex.Weapon1:
				return this.CachedWeaponSlot1Entity;
			case EquipmentIndex.Weapon2:
				return this.CachedWeaponSlot2Entity;
			case EquipmentIndex.Weapon3:
				return this.CachedWeaponSlot3Entity;
			case EquipmentIndex.ExtraWeaponSlot:
				return this.CachedWeaponSlot4Entity;
			default:
				return null;
			}
		}

		// Token: 0x060029AF RID: 10671 RVA: 0x0009D900 File Offset: 0x0009BB00
		public AgentVisualsData CachedWeaponEntity(EquipmentIndex slotIndex, GameEntity cachedWeaponEntity)
		{
			switch (slotIndex)
			{
			case EquipmentIndex.WeaponItemBeginSlot:
				this.CachedWeaponSlot0Entity = cachedWeaponEntity;
				break;
			case EquipmentIndex.Weapon1:
				this.CachedWeaponSlot1Entity = cachedWeaponEntity;
				break;
			case EquipmentIndex.Weapon2:
				this.CachedWeaponSlot2Entity = cachedWeaponEntity;
				break;
			case EquipmentIndex.Weapon3:
				this.CachedWeaponSlot3Entity = cachedWeaponEntity;
				break;
			case EquipmentIndex.ExtraWeaponSlot:
				this.CachedWeaponSlot4Entity = cachedWeaponEntity;
				break;
			}
			return this;
		}

		// Token: 0x060029B0 RID: 10672 RVA: 0x0009D955 File Offset: 0x0009BB55
		public AgentVisualsData Entity(GameEntity entity)
		{
			this.EntityData = entity;
			return this;
		}

		// Token: 0x060029B1 RID: 10673 RVA: 0x0009D95F File Offset: 0x0009BB5F
		public AgentVisualsData UseTranslucency(bool useTranslucency)
		{
			this.UseTranslucencyData = useTranslucency;
			return this;
		}

		// Token: 0x060029B2 RID: 10674 RVA: 0x0009D969 File Offset: 0x0009BB69
		public AgentVisualsData UseTesselation(bool useTesselation)
		{
			this.UseTesselationData = useTesselation;
			return this;
		}

		// Token: 0x060029B3 RID: 10675 RVA: 0x0009D973 File Offset: 0x0009BB73
		public AgentVisualsData ActionCode(in ActionIndexCache actionCode)
		{
			this.ActionCodeData = actionCode;
			return this;
		}

		// Token: 0x060029B4 RID: 10676 RVA: 0x0009D982 File Offset: 0x0009BB82
		public AgentVisualsData RightWieldedItemIndex(int rightWieldedItemIndex)
		{
			this.RightWieldedItemIndexData = rightWieldedItemIndex;
			return this;
		}

		// Token: 0x060029B5 RID: 10677 RVA: 0x0009D98C File Offset: 0x0009BB8C
		public AgentVisualsData LeftWieldedItemIndex(int leftWieldedItemIndex)
		{
			this.LeftWieldedItemIndexData = leftWieldedItemIndex;
			return this;
		}

		// Token: 0x060029B6 RID: 10678 RVA: 0x0009D996 File Offset: 0x0009BB96
		public AgentVisualsData Scale(float scale)
		{
			this.ScaleData = scale;
			return this;
		}

		// Token: 0x060029B7 RID: 10679 RVA: 0x0009D9A0 File Offset: 0x0009BBA0
		public AgentVisualsData CharacterObjectStringId(string characterObjectStringId)
		{
			this.CharacterObjectStringIdData = characterObjectStringId;
			return this;
		}

		// Token: 0x060029B8 RID: 10680 RVA: 0x0009D9AA File Offset: 0x0009BBAA
		public AgentVisualsData HasClippingPlane(bool hasClippingPlane)
		{
			this.HasClippingPlaneData = hasClippingPlane;
			return this;
		}

		// Token: 0x060029B9 RID: 10681 RVA: 0x0009D9B4 File Offset: 0x0009BBB4
		public AgentVisualsData MountCreationKey(string mountCreationKey)
		{
			this.MountCreationKeyData = mountCreationKey;
			return this;
		}

		// Token: 0x060029BA RID: 10682 RVA: 0x0009D9BE File Offset: 0x0009BBBE
		public AgentVisualsData AddColorRandomness(bool addColorRandomness)
		{
			this.AddColorRandomnessData = addColorRandomness;
			return this;
		}

		// Token: 0x04000FE1 RID: 4065
		public MBAgentVisuals AgentVisuals;
	}
}
