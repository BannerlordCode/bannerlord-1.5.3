using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x020000CF RID: 207
	public sealed class SkeletonScale : MBObjectBase
	{
		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06000B1C RID: 2844 RVA: 0x000240B3 File Offset: 0x000222B3
		// (set) Token: 0x06000B1D RID: 2845 RVA: 0x000240BB File Offset: 0x000222BB
		public string SkeletonModel { get; private set; }

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06000B1E RID: 2846 RVA: 0x000240C4 File Offset: 0x000222C4
		// (set) Token: 0x06000B1F RID: 2847 RVA: 0x000240CC File Offset: 0x000222CC
		public Vec3 MountSitBoneScale { get; private set; }

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06000B20 RID: 2848 RVA: 0x000240D5 File Offset: 0x000222D5
		// (set) Token: 0x06000B21 RID: 2849 RVA: 0x000240DD File Offset: 0x000222DD
		public float MountRadiusAdder { get; private set; }

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06000B22 RID: 2850 RVA: 0x000240E6 File Offset: 0x000222E6
		// (set) Token: 0x06000B23 RID: 2851 RVA: 0x000240EE File Offset: 0x000222EE
		public Vec3[] Scales { get; private set; }

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06000B24 RID: 2852 RVA: 0x000240F7 File Offset: 0x000222F7
		// (set) Token: 0x06000B25 RID: 2853 RVA: 0x000240FF File Offset: 0x000222FF
		public List<string> BoneNames { get; private set; }

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06000B26 RID: 2854 RVA: 0x00024108 File Offset: 0x00022308
		// (set) Token: 0x06000B27 RID: 2855 RVA: 0x00024110 File Offset: 0x00022310
		public sbyte[] BoneIndices { get; private set; }

		// Token: 0x06000B28 RID: 2856 RVA: 0x00024119 File Offset: 0x00022319
		public SkeletonScale()
		{
			this.BoneNames = null;
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x00024128 File Offset: 0x00022328
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			this.SkeletonModel = node.Attributes["skeleton"].InnerText;
			XmlAttribute xmlAttribute = node.Attributes["mount_sit_bone_scale"];
			Vec3 vec = new Vec3(1f, 1f, 1f, -1f);
			if (xmlAttribute != null)
			{
				string[] array = xmlAttribute.Value.Split(new char[] { ',' });
				if (array.Length == 3)
				{
					float.TryParse(array[0], out vec.x);
					float.TryParse(array[1], out vec.y);
					float.TryParse(array[2], out vec.z);
				}
			}
			this.MountSitBoneScale = vec;
			XmlAttribute xmlAttribute2 = node.Attributes["mount_radius_adder"];
			if (xmlAttribute2 != null)
			{
				this.MountRadiusAdder = float.Parse(xmlAttribute2.Value);
			}
			this.BoneNames = new List<string>();
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				string text = xmlNode.Name;
				if (text == "BoneScales")
				{
					List<Vec3> list = new List<Vec3>();
					foreach (object obj2 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode2 = (XmlNode)obj2;
						if (xmlNode2.Attributes != null)
						{
							text = xmlNode2.Name;
							if (text == "BoneScale")
							{
								XmlAttribute xmlAttribute3 = xmlNode2.Attributes["scale"];
								Vec3 vec2 = default(Vec3);
								if (xmlAttribute3 != null)
								{
									string[] array2 = xmlAttribute3.Value.Split(new char[] { ',' });
									if (array2.Length == 3)
									{
										float.TryParse(array2[0], out vec2.x);
										float.TryParse(array2[1], out vec2.y);
										float.TryParse(array2[2], out vec2.z);
									}
								}
								this.BoneNames.Add(xmlNode2.Attributes["bone_name"].InnerText);
								list.Add(vec2);
							}
						}
					}
					this.Scales = list.ToArray();
				}
			}
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x000243B8 File Offset: 0x000225B8
		public void SetBoneIndices(sbyte[] boneIndices)
		{
			this.BoneIndices = boneIndices;
			this.BoneNames = null;
		}
	}
}
