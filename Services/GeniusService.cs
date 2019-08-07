using Earworm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Earworm.Services
{
    public class GeniusService
    {
        public string Lyrics { get; set; }
        public string Url { get; set; }
        public string SongName { get; set; }
        public string SongArtist { get; set; }

        public GeniusService(string songName, string songArtist)
        {
            SongName = songName;
            SongArtist = songArtist;
            List<string> LyricsInfo = GetSongLyrics(SongName, SongArtist);
            Lyrics = LyricsInfo[0];
            Url = LyricsInfo[1];
        }

        public List<string> GetSongLyrics(string songName, string songArtists)
        {
            ParseHtml formatter = new ParseHtml();
            List<string> LyricsInfo = new List<string>();
            string lyrics = "Hmm. Couldn't find the lyrics for this song...";
            string lyricsUrl = "https://Genius.com";
            string songSpecifics = SongName + " by " + SongArtist;
            string query = formatter.Urlify(songSpecifics);
            string url = "https://api.genius.com/search?q=" + query;
            string token = "[GENIUS_API_TOKEN]";


            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetStringAsync(new Uri(url)).Result;
                GeniusLyrics geniusLyrics = new GeniusLyrics(response, songSpecifics);

                if (geniusLyrics.LyricsFound)
                {
                    lyrics = formatter.ParseFromWeb(geniusLyrics.LyricsUrl);
                    lyricsUrl = geniusLyrics.LyricsUrl;
                }
            }
            LyricsInfo.Add(lyrics);
            LyricsInfo.Add(lyricsUrl);

            return LyricsInfo;
        }
    }

}
