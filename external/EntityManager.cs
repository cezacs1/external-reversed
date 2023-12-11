using System;
using System.Linq;

// Token: 0x02000010 RID: 16
public class EntityManager
{
	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000037 RID: 55 RVA: 0x00002D78 File Offset: 0x00000F78
	// (set) Token: 0x06000038 RID: 56 RVA: 0x00002D80 File Offset: 0x00000F80
	public long EntityList
	{
		get
		{
			return this._entityList;
		}
		set
		{
			this._entityList = value;
		}
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00002D8C File Offset: 0x00000F8C
	public void Init(MatchType matchType)
	{
		this._matchType = matchType;
		this.EntityList = Memory.ReadByPtr(OffsetManager.BaseClient + 24828640L);
		this.Entities = new Entity[64];
		for (int i = 0; i < 64; i++)
		{
			Entity entity = new Entity(this.GetPlayer(i));
			if (!string.IsNullOrEmpty(entity.GetName()))
			{
				this.Entities[i] = entity;
			}
		}
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00002E04 File Offset: 0x00001004
	public Entity[] GetEnemies()
	{
		if (this._matchType == MatchType.Dead)
		{
			return (from entity in this.Entities
			where entity != null && entity.BasePlayerPawn != LocalPlayer.Instance.BasePlayerPawn
			select entity).ToArray<Entity>();
		}
		return (from entity in this.Entities
		where entity != null && !entity.GetIsMyTeam()
		select entity).ToArray<Entity>();
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00002E88 File Offset: 0x00001088
	public string[] GetSpectatorList()
	{
		return (from entity in this.Entities
		where entity != null && entity.IsSpectateMe
		select entity.GetName()).ToArray<string>();
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00002EF4 File Offset: 0x000010F4
	public Entity[] GetDrawEnemies()
	{
		Entity[] enemies = this.GetEnemies();
		this.DrawEntities = enemies;
		return (from entity in this.DrawEntities
		where entity.Hp > 0 && entity.IsVisibleOnScreen()
		select entity).ToArray<Entity>();
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00002F48 File Offset: 0x00001148
	public long GetPlayer(int i)
	{
		return Memory.ReadByPtr(Memory.ReadByPtr(this.EntityList + (long)(8 * (i & 32767) >> 9) + 16L) + (long)(120 * (i & 511)));
	}

	// Token: 0x04000049 RID: 73
	public static EntityManager Instance;

	// Token: 0x0400004A RID: 74
	private long _entityList;

	// Token: 0x0400004B RID: 75
	public Entity[] DrawEntities = new Entity[64];

	// Token: 0x0400004C RID: 76
	public Entity[] Entities = new Entity[64];

	// Token: 0x0400004D RID: 77
	private MatchType _matchType;
}
