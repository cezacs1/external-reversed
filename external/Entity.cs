using System;
using System.Collections.Generic;

// Token: 0x02000007 RID: 7
public class Entity
{
	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000014 RID: 20 RVA: 0x00002380 File Offset: 0x00000580
	// (set) Token: 0x06000015 RID: 21 RVA: 0x00002388 File Offset: 0x00000588
	public bool IsDefuser { get; set; }

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000016 RID: 22 RVA: 0x00002394 File Offset: 0x00000594
	// (set) Token: 0x06000017 RID: 23 RVA: 0x0000239C File Offset: 0x0000059C
	public bool IsBomb { get; set; }

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000018 RID: 24 RVA: 0x000023A8 File Offset: 0x000005A8
	// (set) Token: 0x06000019 RID: 25 RVA: 0x000023B0 File Offset: 0x000005B0
	public int Hp { get; set; }

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x0600001A RID: 26 RVA: 0x000023BC File Offset: 0x000005BC
	// (set) Token: 0x0600001B RID: 27 RVA: 0x000023C4 File Offset: 0x000005C4
	public bool IsMyTeam { get; set; }

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x0600001C RID: 28 RVA: 0x000023D0 File Offset: 0x000005D0
	// (set) Token: 0x0600001D RID: 29 RVA: 0x000023D8 File Offset: 0x000005D8
	public Vector2 ScreenHead { get; set; }

	// Token: 0x0600001E RID: 30 RVA: 0x000023E4 File Offset: 0x000005E4
	public Entity(long pawnOffset)
	{
		this.PawnOffset = pawnOffset;
		int num = Memory.Read<int>(this.PawnOffset + 1980L);
		this.controllerPawnPtr = (long)Memory.Read<int>(this.PawnOffset + 1500L);
		long num2 = Memory.Read<long>(EntityManager.Instance.EntityList + (8L * ((this.controllerPawnPtr & 32767L) >> 9) + 16L));
		this._playerpawn = Memory.Read<long>(num2 + 120L * (this.controllerPawnPtr & 511L));
		this._listEntry2 = Memory.ReadByPtr(EntityManager.Instance.EntityList + (long)(8 * ((num & 32767) >> 9)) + 16L);
		this.BasePlayerPawn = Memory.ReadByPtr(this._listEntry2 + (long)(120 * (num & 511)));
	}

	// Token: 0x0600001F RID: 31 RVA: 0x000024FC File Offset: 0x000006FC
	public bool IsVisibleOnScreen()
	{
		return ScreenProjection.Instance.IsVisibleOnScreen(this.ScreenHead);
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002510 File Offset: 0x00000710
	public long GetSpectatorTarget()
	{
		long handle = Memory.Read<long>(Memory.Read<long>(this._playerpawn + 4288L) + 68L);
		this.SpectatorTarget = Entity.GetEntityFromHandle(handle);
		this.IsSpectateMe = (this.SpectatorTarget == LocalPlayer.Instance.BasePlayerPawn);
		return this.SpectatorTarget;
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00002568 File Offset: 0x00000768
	public static long GetEntityFromHandle(long handle)
	{
		long num = Memory.Read<long>(EntityManager.Instance.EntityList + 8L * ((handle & 32767L) >> 9) + 16L);
		if (num == 0L)
		{
			return 0L;
		}
		long num2 = Memory.Read<long>(num + 120L * (handle & 511L));
		if (num2 == 0L)
		{
			return 0L;
		}
		return num2;
	}

	// Token: 0x06000022 RID: 34 RVA: 0x000025C4 File Offset: 0x000007C4
	public int GetHp()
	{
		return Memory.Read<int>(this.BasePlayerPawn + 812L);
	}

	// Token: 0x06000023 RID: 35 RVA: 0x000025D8 File Offset: 0x000007D8
	public string GetName()
	{
		return Memory.ReadString(Memory.ReadByPtr(new IntPtr(this.PawnOffset + 1824L)));
	}

	// Token: 0x06000024 RID: 36 RVA: 0x000025F8 File Offset: 0x000007F8
	public Vector3 GetWorldPosition()
	{
		return Memory.Read3DVector(this.BasePlayerPawn + 4628L);
	}

	// Token: 0x06000025 RID: 37 RVA: 0x0000260C File Offset: 0x0000080C
	public int GetTeam()
	{
		return Memory.Read<int>(this.BasePlayerPawn + 959L);
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00002620 File Offset: 0x00000820
	private long GetGameSceneNode()
	{
		return Memory.Read<long>(this.BasePlayerPawn + 784L);
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00002634 File Offset: 0x00000834
	public bool GetIsMyTeam()
	{
		int team = LocalPlayer.Instance.GetTeam();
		return this.GetTeam() == team;
	}

	// Token: 0x06000028 RID: 40 RVA: 0x0000265C File Offset: 0x0000085C
	public bool SpottedState()
	{
		return Memory.Read<bool>(this.BasePlayerPawn + 6512L + 8L);
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00002674 File Offset: 0x00000874
	public bool HasDefuser()
	{
		return Memory.Read<bool>(this.BasePlayerPawn + 2000L);
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00002688 File Offset: 0x00000888
	public bool HasBomb()
	{
		return this.GetActiveWeaponId() == 49;
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00002694 File Offset: 0x00000894
	public ushort GetActiveWeaponId()
	{
		return Memory.Read<ushort>(Memory.Read<long>(this.BasePlayerPawn + 4752L) + 4160L + 80L + 442L);
	}

	// Token: 0x0600002C RID: 44 RVA: 0x000026C0 File Offset: 0x000008C0
	public void UpdateDrawVariables()
	{
		this.IsBomb = this.HasBomb();
		this.Name = this.GetName();
		this.Hp = this.GetHp();
		this.IsMyTeam = this.GetIsMyTeam();
		this.WorldPosition = this.GetWorldPosition();
		ScreenProjection instance = ScreenProjection.Instance;
		this.UpdateBone();
		Vector2 vector2;
		Vector2 vector3;
		try
		{
			if (this.PlayerBoneArray != null)
			{
				BoneVo boneVo = this.PlayerBoneArray[6];
				if (boneVo == null)
				{
					return;
				}
				Vector2 vector;
				instance.WorldToScreen(boneVo.WorldPos, out vector);
				this.HeadBoneScreenPosition = vector;
				vector2 = vector;
				this.WorldPosition = new Vector3(boneVo.WorldPos.X, boneVo.WorldPos.Y, boneVo.WorldPos.Z - 75f);
				this.HeadWorldPosition = new Vector3(this.WorldPosition.X, this.WorldPosition.Y, this.WorldPosition.Z + 85f);
				instance.WorldToScreen(this.HeadWorldPosition, out vector2);
				instance.WorldToScreen(this.WorldPosition, out vector3);
			}
			else
			{
				this.HeadWorldPosition = new Vector3(this.WorldPosition.X, this.WorldPosition.Y, this.WorldPosition.Z + 75f);
				instance.WorldToScreen(this.HeadWorldPosition, out vector2);
				instance.WorldToScreen(this.WorldPosition, out vector3);
				this.HeadBoneScreenPosition = vector2;
				this.HeadBoneScreenPosition.Y = this.HeadBoneScreenPosition.Y + -10f;
			}
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			throw;
		}
		this.ScreenHead = vector2;
		this._height = vector3.Y - vector2.Y;
		this._width = this._height / 2.4f;
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x0600002D RID: 45 RVA: 0x00002898 File Offset: 0x00000A98
	// (set) Token: 0x0600002E RID: 46 RVA: 0x000028A0 File Offset: 0x00000AA0
	public bool IsSpectateMe { get; set; }

	// Token: 0x0600002F RID: 47 RVA: 0x000028AC File Offset: 0x00000AAC
	public void UpdateBone()
	{
		ref CSkeletonInstance ptr = Memory.Read<CSkeletonInstance>(this.GetGameSceneNode());
		int length = 30;
		CBoneData[] array = Memory.ReadStructArray<CBoneData>(ptr.m_modelState.m_boneArray, length);
		for (int i = 0; i < array.Length; i++)
		{
			CBoneData cboneData = array[i];
			if (!(cboneData.Location == Vector3.Zero))
			{
				Vector2 screenPos;
				bool visible = ScreenProjection.Instance.WorldToScreen(cboneData.Location, out screenPos);
				BoneVo boneVo = this.PlayerBoneArray[i];
				if (boneVo == null)
				{
					boneVo = new BoneVo();
				}
				boneVo.Visible = visible;
				boneVo.ScreenPos = screenPos;
				boneVo.WorldPos = cboneData.Location;
				this.PlayerBoneArray[i] = boneVo;
			}
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x0000296C File Offset: 0x00000B6C
	private float CalculateRadius()
	{
		return Math.Min(this._width, this._height) / 2f * 0.5f;
	}

	// Token: 0x06000031 RID: 49 RVA: 0x0000298C File Offset: 0x00000B8C
	private static float CalculateThickness(float radius)
	{
		float num = 0.5f;
		float num2 = 0.7f;
		return num - (radius - 2f) * ((num - num2) / 3f);
	}

	// Token: 0x06000032 RID: 50 RVA: 0x000029BC File Offset: 0x00000BBC
	public void DrawName()
	{
		Vector2 screenHead = this.ScreenHead;
		screenHead.Y -= 10f;
		string text = this.Name;
		Color color = Color.White;
		if (this.IsBomb)
		{
			text += " \ud83d\udd25";
			color = Color.Red;
		}
		DrawFactory.DrawFont(text, 2, screenHead, color);
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00002A18 File Offset: 0x00000C18
	public void DrawBone()
	{
		BoneVo boneVo = new BoneVo();
		float radius = this.CalculateRadius();
		float w = Entity.CalculateThickness(radius);
		Color fuchsia = Color.Fuchsia;
		DrawFactory.DrawCircle((int)this.HeadBoneScreenPosition.X, (int)this.HeadBoneScreenPosition.Y, w, radius, 7, fuchsia);
		foreach (BoneJointList boneJointList in this.BoneList)
		{
			boneVo.WorldPos = Vector3.Zero;
			foreach (int num in boneJointList.bones)
			{
				BoneVo boneVo2 = this.PlayerBoneArray[num];
				if (boneVo2 != null && !(boneVo2.WorldPos == Vector3.Zero))
				{
					if (boneVo.WorldPos == Vector3.Zero)
					{
						boneVo = boneVo2;
					}
					else
					{
						if (boneVo.Visible && boneVo2.Visible)
						{
							DrawFactory.DrawLine(boneVo.ScreenPos.X, boneVo.ScreenPos.Y, boneVo2.ScreenPos.X, boneVo2.ScreenPos.Y, w, fuchsia);
						}
						boneVo = boneVo2;
					}
				}
			}
		}
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00002B78 File Offset: 0x00000D78
	public void DrawBox()
	{
		DrawFactory.DrawBox(this.ScreenHead.X - this._width / 2f, this.ScreenHead.Y, this._width, this._height, Color.Red);
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00002BC4 File Offset: 0x00000DC4
	public void DrawHealth()
	{
		float num = (float)this.Hp / 100f;
		Color color = new Color(1f - num, num, 0f);
		DrawFactory.DrawFilledBox(this.ScreenHead.X - (this._width / 2f + 5f), this.ScreenHead.Y + this._height * (float)(100 - this.Hp) / 100f, 2f, this._height - this._height * (float)(100 - this.Hp) / 100f, color);
	}

	// Token: 0x0400001F RID: 31
	public long PawnOffset;

	// Token: 0x04000020 RID: 32
	public long BasePlayerPawn;

	// Token: 0x04000021 RID: 33
	private float _width;

	// Token: 0x04000022 RID: 34
	private float _height;

	// Token: 0x04000023 RID: 35
	public Vector3 WorldPosition;

	// Token: 0x04000024 RID: 36
	public Vector2 HeadBoneScreenPosition;

	// Token: 0x04000025 RID: 37
	public Vector3 HeadWorldPosition;

	// Token: 0x04000026 RID: 38
	public BoneVo[] PlayerBoneArray = new BoneVo[116];

	// Token: 0x04000029 RID: 41
	private BoneJointList[] BoneList = new BoneJointList[]
	{
		Entity.Trunk,
		Entity.LeftArm,
		Entity.RightArm,
		Entity.LeftLeg,
		Entity.RightLeg
	};

	// Token: 0x0400002D RID: 45
	public string Name;

	// Token: 0x0400002E RID: 46
	private long _playerpawn;

	// Token: 0x0400002F RID: 47
	private long controllerPawnPtr;

	// Token: 0x04000030 RID: 48
	private long _listEntry2;

	// Token: 0x04000031 RID: 49
	public long SpectatorTarget;

	// Token: 0x04000033 RID: 51
	private static readonly BoneJointList Trunk = new BoneJointList
	{
		bones = new List<int>
		{
			6,
			5,
			2,
			0
		}
	};

	// Token: 0x04000034 RID: 52
	private static readonly BoneJointList LeftArm = new BoneJointList
	{
		bones = new List<int>
		{
			5,
			8,
			9,
			10
		}
	};

	// Token: 0x04000035 RID: 53
	private static readonly BoneJointList RightArm = new BoneJointList
	{
		bones = new List<int>
		{
			5,
			13,
			14,
			15
		}
	};

	// Token: 0x04000036 RID: 54
	private static readonly BoneJointList LeftLeg = new BoneJointList
	{
		bones = new List<int>
		{
			0,
			22,
			23,
			24
		}
	};

	// Token: 0x04000037 RID: 55
	private static readonly BoneJointList RightLeg = new BoneJointList
	{
		bones = new List<int>
		{
			0,
			25,
			26,
			27
		}
	};
}
