using System;

// Token: 0x02000013 RID: 19
public class OffsetManager
{
	// Token: 0x1700000A RID: 10
	// (get) Token: 0x06000063 RID: 99 RVA: 0x0000305C File Offset: 0x0000125C
	// (set) Token: 0x06000064 RID: 100 RVA: 0x00003090 File Offset: 0x00001290
	public static long BaseClient
	{
		get
		{
			if (OffsetManager.baseClient == 0L)
			{
				OffsetManager.baseClient = Utils.GetClientAddress().ToInt64();
			}
			return OffsetManager.baseClient;
		}
		set
		{
			OffsetManager.baseClient = value;
		}
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000065 RID: 101 RVA: 0x00003098 File Offset: 0x00001298
	// (set) Token: 0x06000066 RID: 102 RVA: 0x000030CC File Offset: 0x000012CC
	public static long BaseEngine
	{
		get
		{
			if (OffsetManager.baseEngine == 0L)
			{
				OffsetManager.baseEngine = Utils.GetEngineAddress().ToInt64();
			}
			return OffsetManager.baseEngine;
		}
		set
		{
			OffsetManager.baseEngine = value;
		}
	}

	// Token: 0x04000059 RID: 89
	public static long baseClient;

	// Token: 0x0400005A RID: 90
	public static long baseEngine;

	// Token: 0x0200026C RID: 620
	public static class engine2_dll
	{
		// Token: 0x04001DB2 RID: 7602
		public const long dwBuildNumber = 4764964L;

		// Token: 0x04001DB3 RID: 7603
		public const long dwWindowHeight = 5512732L;

		// Token: 0x04001DB4 RID: 7604
		public const long dwWindowWidth = 5512728L;
	}

	// Token: 0x0200026D RID: 621
	public static class client_dll
	{
		// Token: 0x04001DB5 RID: 7605
		public const long dwEntityList = 24828640L;

		// Token: 0x04001DB6 RID: 7606
		public const long dwLocalPlayerController = 25152952L;

		// Token: 0x04001DB7 RID: 7607
		public const long dwViewMatrix = 25215200L;
	}

	// Token: 0x0200026E RID: 622
	public static class CCSPlayerController
	{
		// Token: 0x04001DB8 RID: 7608
		public const long m_sSanitizedPlayerName = 1824L;

		// Token: 0x04001DB9 RID: 7609
		public const long m_hPlayerPawn = 1980L;

		// Token: 0x04001DBA RID: 7610
		public const long m_iHealth = 812L;

		// Token: 0x04001DBB RID: 7611
		public const long m_bPawnHasDefuser = 2000L;

		// Token: 0x04001DBC RID: 7612
		public const long m_hPawn = 1500L;
	}

	// Token: 0x0200026F RID: 623
	public static class C_BaseEntity
	{
		// Token: 0x04001DBD RID: 7613
		public const long m_pGameSceneNode = 784L;

		// Token: 0x04001DBE RID: 7614
		public const long m_iTeamNum = 959L;
	}

	// Token: 0x02000270 RID: 624
	public static class C_BasePlayerPawn
	{
		// Token: 0x04001DBF RID: 7615
		public const long m_vOldOrigin = 4628L;

		// Token: 0x04001DC0 RID: 7616
		public const long m_pWeaponServices = 4264L;

		// Token: 0x04001DC1 RID: 7617
		public const long m_pObserverServices = 4288L;
	}

	// Token: 0x02000271 RID: 625
	public static class CPlayer_ObserverServices
	{
		// Token: 0x04001DC2 RID: 7618
		public const long m_hObserverTarget = 68L;
	}

	// Token: 0x02000272 RID: 626
	public static class C_CSPlayerPawnBase
	{
		// Token: 0x04001DC3 RID: 7619
		public const long m_entitySpottedState = 6512L;

		// Token: 0x04001DC4 RID: 7620
		public const long m_pClippingWeapon = 4752L;
	}

	// Token: 0x02000273 RID: 627
	public static class EntitySpottedState_t
	{
		// Token: 0x04001DC5 RID: 7621
		public const long m_bSpotted = 8L;
	}
}
