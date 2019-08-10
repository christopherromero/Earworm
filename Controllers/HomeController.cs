using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Earworm.Models;
using Microsoft.AspNetCore.Authentication;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using Earworm.Services;

namespace Earworm.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect(Url.Content("~/"));
        }

        [HttpGet]
        public JsonResult NowPlaying()
        {
            Dictionary<string, string> nowPlaying = new Dictionary<string, string>();
            if (User.Identity.IsAuthenticated)
            {

                string accessToken = HttpContext.GetTokenAsync("access_token").Result;

                try
                {
                    // For usage visit: https://github.com/JohnnyCrazy/SpotifyAPI-NET/
                    SpotifyWebAPI spotifyAPI = new SpotifyWebAPI
                    {
                        AccessToken = accessToken,
                        TokenType = "Bearer"
                    };

                    PrivateProfile profile = spotifyAPI.GetPrivateProfileAsync().Result;
                    PlaybackContext context = spotifyAPI.GetPlayback();
                    if (!profile.HasError())
                    {
                        nowPlaying.Add("SpotifyName", profile.DisplayName);
                        nowPlaying.Add("SpotifyAvatar", profile.Images.FirstOrDefault().Url);
                        nowPlaying.Add("SpotifyProfileUrl", "https://open.spotify.com/user/" + profile.Id);
                    }

                    if (context.Item != null && (context.IsPlaying))
                    {
                        nowPlaying.Add("SongName", context.Item.Name);
                        nowPlaying.Add("SongUrl", context.Item.Href);
                        nowPlaying.Add("ArtistName", context.Item.Artists.FirstOrDefault().Name);
                        nowPlaying.Add("ArtistUrl", context.Item.Artists.FirstOrDefault().Href);
                        nowPlaying.Add("NowPlaying", nowPlaying["SongName"] + " by " + nowPlaying["ArtistName"]);
                        nowPlaying.Add("AlbumArt", context.Item.Album.Images.FirstOrDefault().Url);
                        nowPlaying.Add("AlbumUrl", context.Item.Album.ExternalUrls["spotify"]);
                        nowPlaying.Add("IsPlaying", "true");
                    }
                    else
                    {
                        nowPlaying.Add("SongName", null);
                        nowPlaying.Add("SongUrl", null);
                        nowPlaying.Add("ArtistName", null);
                        nowPlaying.Add("ArtistUrl", null);
                        nowPlaying.Add("NowPlaying", null);
                        nowPlaying.Add("AlbumArt", null);
                        nowPlaying.Add("AlbumUrl", null);
                        nowPlaying.Add("IsPlaying", "false");
                    }
                    GeniusService geniusService = new GeniusService(nowPlaying["SongName"], nowPlaying["ArtistName"]);
                    nowPlaying.Add("Lyrics", geniusService.Lyrics);
                    nowPlaying.Add("LyricsContribution", geniusService.Url);
                }
                catch (Exception ex)
                {
                    // show the users some error
                }
            }
            return new JsonResult(nowPlaying);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
