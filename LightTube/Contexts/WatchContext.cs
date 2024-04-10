﻿using InnerTube;
using LightTube.Database.Models;

namespace LightTube.Contexts;

public class WatchContext : BaseContext
{
    public PlayerContext Player;
    public InnerTubeNextResponse Video;
    public InnerTubePlaylistInfo? Playlist;
    public InnerTubeContinuationResponse? Comments;
    public int Dislikes;
    public int Likes;
    public SponsorBlockSegment[] Sponsors;

    public WatchContext(HttpContext context, InnerTubePlayer innerTubePlayer,
        InnerTubeNextResponse innerTubeNextResponse,
        InnerTubeContinuationResponse? comments,
        bool compatibility, int dislikes, int likes, SponsorBlockSegment[] sponsors) : base(context)
    {
        Player = new PlayerContext(context, innerTubePlayer, innerTubeNextResponse, "embed", compatibility,
            context.Request.Query["q"], sponsors);
        Video = innerTubeNextResponse;
        Playlist = Video.Playlist;
        Comments = comments;
        Dislikes = dislikes;
        Likes = likes;
        Sponsors = sponsors;
        GuideHidden = true;

        AddMeta("description", Video.Description);
        AddMeta("author", Video.Channel.Title);
        AddMeta("og:title", Video.Title);
        AddMeta("og:description", Video.Description);
        AddMeta("og:url",
            $"{context.Request.Scheme}://{context.Request.Host}/{context.Request.Path}{context.Request.QueryString}");
        AddMeta("og:image", $"{context.Request.Scheme}://{context.Request.Host}/proxy/thumbnail/{Video.Id}/-1");
        AddMeta("og:video:url", $"https://{context.Request.Host}/proxy/media/{Video.Id}/18.mp4");
        AddMeta("og:video:width", "640");
        AddMeta("og:video:height", "360");
        AddMeta("og:type", "video.other");
        AddMeta("twitter:card", $"{context.Request.Scheme}://{context.Request.Host}/proxy/thumbnail/{Video.Id}/-1");
        AddMeta("twitter:player", $"https://{context.Request.Host}/embed/{Video.Id}");
        AddMeta("twitter:player:stream", $"https://{context.Request.Host}/proxy/media/{Video.Id}/18.mp4");

        AddStylesheet("/lib/ltplayer.css");

        AddScript("/lib/ltplayer.js");
        AddScript("/lib/hls.js");
        AddScript("/js/player.js");
    }

    public WatchContext(HttpContext context, Exception e, InnerTubeNextResponse innerTubeNextResponse,
        InnerTubeContinuationResponse? comments, int dislikes, int likes) : base(context)
    {
        Player = new PlayerContext(context, e);
        Video = innerTubeNextResponse;
        Playlist = Video.Playlist;
        Comments = comments;
        Dislikes = dislikes;
        Likes = likes;
        Sponsors = [];
        GuideHidden = true;

        AddMeta("description", Video.Description);
        AddMeta("author", Video.Channel.Title);
        AddMeta("og:title", Video.Title);
        AddMeta("og:description", Video.Description);
        AddMeta("og:url",
            $"{context.Request.Scheme}://{context.Request.Host}/{context.Request.Path}{context.Request.QueryString}");
        AddMeta("og:image", $"{context.Request.Scheme}://{context.Request.Host}/proxy/thumbnail/{Video.Id}/-1");
        AddMeta("og:video:url", $"https://{context.Request.Host}/proxy/media/{Video.Id}/18.mp4");
        AddMeta("og:video:width", "640");
        AddMeta("og:video:height", "360");
        AddMeta("og:type", "video.other");
        AddMeta("twitter:card", $"{context.Request.Scheme}://{context.Request.Host}/proxy/thumbnail/{Video.Id}/-1");
        AddMeta("twitter:player", $"https://{context.Request.Host}/embed/{Video.Id}");
        AddMeta("twitter:player:stream", $"https://{context.Request.Host}/proxy/media/{Video.Id}/18.mp4");
    }

    public WatchContext(HttpContext context, InnerTubePlayer innerTubePlayer,
        InnerTubeNextResponse innerTubeNextResponse, DatabasePlaylist? playlist,
        InnerTubeContinuationResponse? comments,
        bool compatibility, int dislikes, int likes, SponsorBlockSegment[] sponsors) : base(context)
    {
        Player = new PlayerContext(context, innerTubePlayer, innerTubeNextResponse, "embed", compatibility,
            context.Request.Query["q"], sponsors);
        Video = innerTubeNextResponse;
        Playlist = playlist?.GetInnerTubePlaylistInfo(innerTubePlayer.Details.Id);
        if (playlist != null && playlist.Visibility == PlaylistVisibility.PRIVATE)
            if (playlist.Author != User?.UserID)
                Playlist = null;
        Comments = comments;
        Dislikes = dislikes;
        Likes = likes;
        Sponsors = sponsors;
        GuideHidden = true;

        AddMeta("description", Video.Description);
        AddMeta("author", Video.Channel.Title);
        AddMeta("og:title", Video.Title);
        AddMeta("og:description", Video.Description);
        AddMeta("og:url",
            $"{context.Request.Scheme}://{context.Request.Host}/{context.Request.Path}{context.Request.QueryString}");
        AddMeta("og:image", $"{context.Request.Scheme}://{context.Request.Host}/proxy/thumbnail/{Video.Id}/-1");
        AddMeta("og:video:url", $"https://{context.Request.Host}/proxy/media/{Video.Id}/18.mp4");
        AddMeta("og:video:width", "640");
        AddMeta("og:video:height", "360");
        AddMeta("og:type", "video.other");
        AddMeta("twitter:card", $"{context.Request.Scheme}://{context.Request.Host}/proxy/thumbnail/{Video.Id}/-1");
        AddMeta("twitter:player", $"https://{context.Request.Host}/embed/{Video.Id}");
        AddMeta("twitter:player:stream", $"https://{context.Request.Host}/proxy/media/{Video.Id}/18.mp4");

        AddStylesheet("/lib/ltplayer.css");

        AddScript("/lib/ltplayer.js");
        AddScript("/lib/hls.js");
        AddScript("/js/player.js");
    }

    public WatchContext(HttpContext context, Exception e, InnerTubeNextResponse innerTubeNextResponse,
        DatabasePlaylist? playlist,
        InnerTubeContinuationResponse? comments, int dislikes, int likes) : base(context)
    {
        Player = new PlayerContext(context, e);
        Video = innerTubeNextResponse;
        Playlist = playlist?.GetInnerTubePlaylistInfo(innerTubeNextResponse.Id);
        if (playlist != null && playlist.Visibility == PlaylistVisibility.PRIVATE)
            if (playlist.Author != User?.UserID)
                Playlist = null;
        Comments = comments;
        Dislikes = dislikes;
        Likes = likes;
        Sponsors = [];
        GuideHidden = true;

        AddMeta("description", Video.Description);
        AddMeta("author", Video.Channel.Title);
        AddMeta("og:title", Video.Title);
        AddMeta("og:description", Video.Description);
        AddMeta("og:url",
            $"{context.Request.Scheme}://{context.Request.Host}/{context.Request.Path}{context.Request.QueryString}");
        AddMeta("og:image", $"{context.Request.Scheme}://{context.Request.Host}/proxy/thumbnail/{Video.Id}/-1");
        AddMeta("og:video:url", $"https://{context.Request.Host}/proxy/media/{Video.Id}/18.mp4");
        AddMeta("og:video:width", "640");
        AddMeta("og:video:height", "360");
        AddMeta("og:type", "video.other");
        AddMeta("twitter:card", $"{context.Request.Scheme}://{context.Request.Host}/proxy/thumbnail/{Video.Id}/-1");
        AddMeta("twitter:player", $"https://{context.Request.Host}/embed/{Video.Id}");
        AddMeta("twitter:player:stream", $"https://{context.Request.Host}/proxy/media/{Video.Id}/18.mp4");
    }
}