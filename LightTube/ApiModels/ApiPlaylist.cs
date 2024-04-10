using InnerTube;
using InnerTube.Renderers;
using LightTube.Database;
using LightTube.Database.Models;

namespace LightTube.ApiModels;

public class ApiPlaylist
{
    public string Id { get; }
    public IEnumerable<string> Alerts { get; }
    public string Title { get; }
    public string Description { get; }
    public IEnumerable<Badge> Badges { get; }
    public Channel Channel { get; }
    public IEnumerable<Thumbnail> Thumbnails { get; }
    public string LastUpdated { get; }
    public string VideoCountText { get; }
    public string ViewCountText { get; }
    public PlaylistContinuationInfo? Continuation { get; }
    public IEnumerable<PlaylistVideoRenderer> Videos { get; }

    public ApiPlaylist(InnerTubePlaylist playlist)
    {
        Id = playlist.Id;
        Alerts = playlist.Alerts;
        Title = playlist.Sidebar.Title;
        Description = playlist.Sidebar.Description;
        Badges = playlist.Sidebar.Badges;
        Channel = playlist.Sidebar.Channel;
        Thumbnails = playlist.Sidebar.Thumbnails;
        LastUpdated = playlist.Sidebar.LastUpdated;
        VideoCountText = playlist.Sidebar.VideoCountText;
        ViewCountText = playlist.Sidebar.ViewCountText;
        Continuation = playlist.Continuation;
        Videos = playlist.Videos;
    }

    public ApiPlaylist(InnerTubeContinuationResponse playlist)
    {
        Id = "";
        Alerts = [];
        Title = "";
        Description = "";
        Badges = [];
        Channel = new Channel();
        Thumbnails = [];
        LastUpdated = "";
        VideoCountText = "";
        ViewCountText = "";
        Continuation = playlist.Continuation is not null ? InnerTube.Utils.UnpackPlaylistContinuation(playlist.Continuation) : null;
        Videos = playlist.Contents.Cast<PlaylistVideoRenderer>();
    }

    public ApiPlaylist(DatabasePlaylist playlist)
    {
        Id = playlist.Id;
        Alerts = [];
        Title = playlist.Name;
        Description = playlist.Description;
        Badges = [];
        DatabaseUser user = DatabaseManager.Users.GetUserFromId(playlist.Author).Result!;
        Channel = new Channel
        {
            Id = user.LTChannelID,
            Title = user.UserID,
            Avatar = null,
            Subscribers = null,
            Badges = []
        };
        Thumbnails =
        [
            new Thumbnail
            {
                Width = null,
                Height = null,
                Url = new Uri($"https://i.ytimg.com/vi/{playlist.VideoIds.FirstOrDefault()}/hqdefault.jpg")
            }
        ];
        LastUpdated = $"Last updated on {playlist.LastUpdated:MMM d, yyyy}";
        VideoCountText = playlist.VideoIds.Count switch
        {
            0 => "No videos",
            1 => "1 video",
            _ => $"{playlist.VideoIds.Count} videos"
        };
        ViewCountText = "LightTube playlist";
        Continuation = null;
        Videos = DatabaseManager.Playlists.GetPlaylistVideos(playlist.Id, false);
    }
}