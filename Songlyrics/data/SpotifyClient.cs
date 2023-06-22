namespace Songlyrics.Data
{
    public class SpotifyClient
    {
        private static async Task<string> GetLyricsFromATrack(int trackId)
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
