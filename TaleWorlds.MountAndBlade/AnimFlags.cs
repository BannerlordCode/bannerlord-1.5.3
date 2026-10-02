using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001A4 RID: 420
	[EngineStruct("Anim_flags", false, null, FirstCharacterUppercase = false)]
	[Flags]
	public enum AnimFlags : ulong
	{
		// Token: 0x0400074B RID: 1867
		amf_priority_continue = 1UL,
		// Token: 0x0400074C RID: 1868
		amf_priority_jump = 2UL,
		// Token: 0x0400074D RID: 1869
		amf_priority_ride = 2UL,
		// Token: 0x0400074E RID: 1870
		amf_priority_crouch = 2UL,
		// Token: 0x0400074F RID: 1871
		amf_priority_attack = 10UL,
		// Token: 0x04000750 RID: 1872
		amf_priority_cancel = 12UL,
		// Token: 0x04000751 RID: 1873
		amf_priority_defend = 14UL,
		// Token: 0x04000752 RID: 1874
		amf_priority_defend_parry = 15UL,
		// Token: 0x04000753 RID: 1875
		amf_priority_throw = 15UL,
		// Token: 0x04000754 RID: 1876
		amf_priority_blocked = 15UL,
		// Token: 0x04000755 RID: 1877
		amf_priority_parried = 15UL,
		// Token: 0x04000756 RID: 1878
		amf_priority_kick = 33UL,
		// Token: 0x04000757 RID: 1879
		amf_priority_reload = 60UL,
		// Token: 0x04000758 RID: 1880
		amf_priority_mount = 64UL,
		// Token: 0x04000759 RID: 1881
		amf_priority_equip = 70UL,
		// Token: 0x0400075A RID: 1882
		amf_priority_rear = 74UL,
		// Token: 0x0400075B RID: 1883
		amf_priority_upperbody_while_kick = 75UL,
		// Token: 0x0400075C RID: 1884
		amf_priority_striked = 80UL,
		// Token: 0x0400075D RID: 1885
		amf_priority_fall_from_horse = 81UL,
		// Token: 0x0400075E RID: 1886
		amf_priority_jump_loop = 81UL,
		// Token: 0x0400075F RID: 1887
		amf_priority_jump_end = 82UL,
		// Token: 0x04000760 RID: 1888
		amf_priority_die = 95UL,
		// Token: 0x04000761 RID: 1889
		amf_priority_mask = 255UL,
		// Token: 0x04000762 RID: 1890
		anf_disable_agent_agent_collisions = 256UL,
		// Token: 0x04000763 RID: 1891
		anf_ignore_all_collisions = 512UL,
		// Token: 0x04000764 RID: 1892
		anf_ignore_static_body_collisions = 1024UL,
		// Token: 0x04000765 RID: 1893
		anf_use_last_step_point_as_data = 2048UL,
		// Token: 0x04000766 RID: 1894
		anf_make_bodyfall_sound = 4096UL,
		// Token: 0x04000767 RID: 1895
		anf_client_prediction = 8192UL,
		// Token: 0x04000768 RID: 1896
		anf_keep = 16384UL,
		// Token: 0x04000769 RID: 1897
		anf_restart = 32768UL,
		// Token: 0x0400076A RID: 1898
		anf_client_owner_prediction = 65536UL,
		// Token: 0x0400076B RID: 1899
		anf_make_walk_sound = 131072UL,
		// Token: 0x0400076C RID: 1900
		anf_disable_hand_ik = 262144UL,
		// Token: 0x0400076D RID: 1901
		anf_stick_item_to_left_hand = 524288UL,
		// Token: 0x0400076E RID: 1902
		anf_blends_according_to_look_slope = 1048576UL,
		// Token: 0x0400076F RID: 1903
		anf_synch_with_horse = 2097152UL,
		// Token: 0x04000770 RID: 1904
		anf_use_left_hand_during_attack = 4194304UL,
		// Token: 0x04000771 RID: 1905
		anf_lock_camera = 8388608UL,
		// Token: 0x04000772 RID: 1906
		anf_lock_movement = 16777216UL,
		// Token: 0x04000773 RID: 1907
		anf_synch_with_movement = 33554432UL,
		// Token: 0x04000774 RID: 1908
		anf_enable_hand_spring_ik = 67108864UL,
		// Token: 0x04000775 RID: 1909
		anf_enable_hand_blend_ik = 134217728UL,
		// Token: 0x04000776 RID: 1910
		anf_synch_with_ladder_movement = 268435456UL,
		// Token: 0x04000777 RID: 1911
		anf_do_not_keep_track_of_sound = 536870912UL,
		// Token: 0x04000778 RID: 1912
		anf_reset_camera_height = 1073741824UL,
		// Token: 0x04000779 RID: 1913
		anf_disable_alternative_randomization = 2147483648UL,
		// Token: 0x0400077A RID: 1914
		anf_disable_auto_increment_progress = 4294967296UL,
		// Token: 0x0400077B RID: 1915
		anf_switch_item_between_hands = 8589934592UL,
		// Token: 0x0400077C RID: 1916
		anf_attach_sound_to_agent = 17179869184UL,
		// Token: 0x0400077D RID: 1917
		anf_spawn_particle = 34359738368UL,
		// Token: 0x0400077E RID: 1918
		anf_enforce_lowerbody = 68719476736UL,
		// Token: 0x0400077F RID: 1919
		anf_enforce_all = 137438953472UL,
		// Token: 0x04000780 RID: 1920
		anf_cyclic = 274877906944UL,
		// Token: 0x04000781 RID: 1921
		anf_enforce_root_rotation = 549755813888UL,
		// Token: 0x04000782 RID: 1922
		anf_allow_head_movement = 1099511627776UL,
		// Token: 0x04000783 RID: 1923
		anf_disable_foot_ik = 2199023255552UL,
		// Token: 0x04000784 RID: 1924
		anf_affected_by_movement = 4398046511104UL,
		// Token: 0x04000785 RID: 1925
		anf_update_bounding_volume = 8796093022208UL,
		// Token: 0x04000786 RID: 1926
		anf_align_with_ground = 17592186044416UL,
		// Token: 0x04000787 RID: 1927
		anf_ignore_slope = 35184372088832UL,
		// Token: 0x04000788 RID: 1928
		anf_displace_position = 70368744177664UL,
		// Token: 0x04000789 RID: 1929
		anf_enable_left_hand_ik = 140737488355328UL,
		// Token: 0x0400078A RID: 1930
		anf_ignore_scale_on_root_position = 281474976710656UL,
		// Token: 0x0400078B RID: 1931
		anf_blend_main_item_bone_entitially = 562949953421312UL,
		// Token: 0x0400078C RID: 1932
		anf_enforce_weapon_tip_with_rope_stretched = 1125899906842624UL,
		// Token: 0x0400078D RID: 1933
		anf_enforce_weapon_tip_with_rope_relaxed = 2251799813685248UL,
		// Token: 0x0400078E RID: 1934
		anf_animation_layer_flags_mask = 4503530907893760UL,
		// Token: 0x0400078F RID: 1935
		anf_animation_layer_flags_bits = 36UL,
		// Token: 0x04000790 RID: 1936
		anf_randomization_weight_1 = 1152921504606846976UL,
		// Token: 0x04000791 RID: 1937
		anf_randomization_weight_2 = 2305843009213693952UL,
		// Token: 0x04000792 RID: 1938
		anf_randomization_weight_4 = 4611686018427387904UL,
		// Token: 0x04000793 RID: 1939
		anf_randomization_weight_8 = 9223372036854775808UL,
		// Token: 0x04000794 RID: 1940
		anf_randomization_weight_mask = 17293822569102704640UL
	}
}
