using System.Net;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using SpotifyAPI.Web;
using System.Text;
using System.Text.RegularExpressions;
using Songlyrics.Models;
using SpotifyAPI.Web.Http;

namespace Songlyrics.Data
{
    public class SpotifyClient
    {
        private string? _apiToken;

        public SpotifyClient()
        {
            _apiToken = GetSpotifyAccessToken("7726e03f7fc141099f28f2a4a4c887b6", "593b9a5df77742c39666c7773da5654e");
        }

        public async Task<string[]> GetLyricsFromARandomTrackInPlaylist(string playlist)
        {
            var r = new Random();
            var tracks = await GetAllTracksFromAPlaylist(playlist);
            var track = tracks.ElementAt(r.Next(0, tracks.Count() - 1));
            var unformattedLyrics = await GetLyricsFromATrack(track.Id);
            var lyricsObject = JsonConvert.DeserializeObject<LyricsResponse>(unformattedLyrics)!.Lines;
            var lyricsString = lyricsObject.Select(l => l.Words);
            var cleanedLyrics = string.Join(" ", lyricsString.Select(s => Regex.Replace(s, @"[^0-9a-zA-Z\s]", "")));
            var wordsList = cleanedLyrics.Split(' ').ToList().Where(s => !string.IsNullOrWhiteSpace(s));
            var wordsListWithArtistAndTrack = wordsList.ToList();
            wordsListWithArtistAndTrack.Add(track.Artists[0].Name);
            wordsListWithArtistAndTrack.Add(track.Name);
            return wordsListWithArtistAndTrack.ToArray();
        }

        private string GetSpotifyAccessToken(string clientId, string clientSecret)
        {
            var authUrl = "https://accounts.spotify.com/api/token";

            var authParams = new System.Collections.Specialized.NameValueCollection
            {
                { "grant_type", "client_credentials" },
                { "client_id", clientId },
                { "client_secret", clientSecret }
            };

            using (var client = new WebClient())
            {
                var response = client.UploadValues(authUrl, authParams);
                var responseString = System.Text.Encoding.Default.GetString(response);

                // Deserialize into a Dictionary
                var responseObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);

                // Check if the key exists and return the value
                if (responseObject != null && responseObject.TryGetValue("access_token", out var token))
                {
                    return token;
                }

                throw new Exception("Access token not found in response.");
            }
        }


        private async Task<IEnumerable<Track>> GetAllTracksFromAPlaylist(string playlistId)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiToken}");

            var tracks = new List<Track>();
            var nextUrl = $"https://api.spotify.com/v1/playlists/{playlistId}/tracks";

            while (!string.IsNullOrEmpty(nextUrl))
            {
                var response = await client.GetAsync(nextUrl);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var page = JsonConvert.DeserializeObject<Page<PlaylistTrack>>(result);

                    tracks.AddRange(page.Items.Select(i => i.Track));
                    nextUrl = page.Next;
                }
                else
                {
                    throw new Exception("Status code is not success");
                }
            }

            return tracks;
        }

        public class LyricsResponse
        {
            public bool Error { get; set; }
            public string SyncType { get; set; }
            public IList<Line> Lines { get; set; }
        }

        public class Line
        {
            public long StartTimeMs { get; set; }
            public string Words { get; set; }
            public IList<object> Syllables { get; set; } // Adjust this if syllables structure is known
            public string EndTimeMs { get; set; }
        }


        public class Page<T>
        {
            public string Next { get; set; }
            public List<T> Items { get; set; }
        }

        public class PlaylistTrack
        {
            public Track Track { get; set; }
        }

        private static async Task<string> GetLyricsFromATrack(string trackId)
        {
            var client = new HttpClient();
            // Define the request URL
            var url = $"https://spotify-lyric-api.herokuapp.com/?url=https://open.spotify.com/track/{trackId}?autoplay=true";

            // Send GET request
            var response = await client.GetAsync(url);

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            // Read and print the response body
            return await response.Content.ReadAsStringAsync();
        }
    }
}
