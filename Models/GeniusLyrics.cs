using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Earworm.Models
{

    public class GeniusLyrics
    {
        RootObject rootObject { get; set; }
        public Meta metaData { get; set; }
        public Response response { get; set; }
        public List<Hit> hits { get; set; }
        public List<Result> results = new List<Result>();
        public List<Stats> stats = new List<Stats>();
        public List<Primary_Artist> primaryArtists = new List<Primary_Artist>();
        public string LyricsUrl;
        public bool LyricsFound = false;

        public GeniusLyrics(string jsonString, string songSpecifics)
        {
            rootObject = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(jsonString);
            response = rootObject.response;
            hits = response.hits;
            foreach (Hit h in hits)
            {
                results.Add(h.result);
            }
            foreach (Result r in results)
            {
                stats.Add(r.stats);
                primaryArtists.Add(r.primary_artist);
                if (!LyricsFound)
                {
                    string ft = r.full_title;
                    ft = Regex.Replace(ft, @" ?\(.*?\)", string.Empty);
                    songSpecifics = Regex.Replace(songSpecifics, @" ?\(.*?\)", string.Empty);                    
                    if (ft.Length == songSpecifics.Length)
                    {
                        LyricsUrl = r.url;
                        LyricsFound = true;
                    }
                }
            }
        }
    }


    public class RootObject
    {
        public Meta meta { get; set; }
        public Response response { get; set; }
    }

    public class Meta
    {
        public int status { get; set; }
    }

    public class Response
    {
        public List<Hit> hits { get; set; }
    }

    public class Hit
    {
        public List<object> highlights { get; set; }
        public string index { get; set; }
        public string type { get; set; }
        public Result result { get; set; }
    }

    public class Result
    {
        public int annotation_count { get; set; }
        public string api_path { get; set; }
        public string full_title { get; set; }
        public string header_image_thumbnail_url { get; set; }
        public string header_image_url { get; set; }
        public int id { get; set; }
        public int lyrics_owner_id { get; set; }
        public string lyrics_state { get; set; }
        public string path { get; set; }
        public int? pyongs_count { get; set; }
        public string song_art_image_thumbnail_url { get; set; }
        public string song_art_image_url { get; set; }
        public Stats stats { get; set; }
        public string title { get; set; }
        public string title_with_featured { get; set; }
        public string url { get; set; }
        public Primary_Artist primary_artist { get; set; }
    }

    public class Stats
    {
        public int unreviewed_annotations { get; set; }
        public int concurrents { get; set; }
        public bool hot { get; set; }
        public int pageviews { get; set; }
    }

    public class Primary_Artist
    {
        public string api_path { get; set; }
        public string header_image_url { get; set; }
        public int id { get; set; }
        public string image_url { get; set; }
        public bool is_meme_verified { get; set; }
        public bool is_verified { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public int iq { get; set; }
    }

}
