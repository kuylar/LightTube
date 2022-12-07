﻿using LightTube.Chores;
using LightTube.Database.Models;
using MongoDB.Driver;

namespace LightTube.Database;

public static class DatabaseManager
{
	public static IMongoDatabase Database { get; private set; }
	public static IMongoCollection<DatabaseUser> UserCollection { get; private set; }
	public static IMongoCollection<DatabaseLogin> TokensCollection { get; private set; }
	public static IMongoCollection<DatabaseVideo> VideoCacheCollection { get; private set; }
	public static IMongoCollection<DatabasePlaylist> PlaylistCollection { get; private set; }
	public static IMongoCollection<DatabaseChannel> ChannelCacheCollection { get; private set; }

	public static void Init(string connstr)
	{
		MongoClient client = new(connstr);
		Database = client.GetDatabase(Configuration.GetVariable("LIGHTTUBE_MONGODB_DATABASE", "lighttube"));
		UserCollection = Database.GetCollection<DatabaseUser>("users");
		TokensCollection = Database.GetCollection<DatabaseLogin>("tokens");
		VideoCacheCollection = Database.GetCollection<DatabaseVideo>("videoCache");
		PlaylistCollection = Database.GetCollection<DatabasePlaylist>("playlists");
		ChannelCacheCollection = Database.GetCollection<DatabaseChannel>("channelCache");

		ChoreManager.QueueChore("MigrateData");
		ChoreManager.QueueChore("DatabaseCleanup");
	}
}