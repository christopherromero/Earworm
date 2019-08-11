var currentAlbumArt = null;
var currentAvatar = null;
var firstLoad = true;

//new Vue({
//    el: '#droplets',
//    data: {
//        quantity: 10,
//    }
//});

function loadNowPlaying() {
    console.log('loadNowPlaying');
    $('#result').html('Loading...');
    $.ajax({
        type: "GET",
        url: "home/nowplaying",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var albumArt;
            var spotifyAvatar;
            var lyricsContributions;
            var lyrics;
            var songName;

            // Check if there is any music playing
            if (response.IsPlaying === "false") {
                // If not, set lyrics body to this message
                lyrics = `It doesn't look like anything is playing right now.</br> Go to <a href="https://open.spotify.com" target="_blank">Spotify</a> so we can show some lyrics.`;
                albumArt = "";
            }
            else {
                // If yes, set to now playing properties
                albumArt = `<a id="art" href="${response.AlbumUrl}" target="_blank"><img src="${response.AlbumArt}" class="img-thumbnail" /></a>`;
                lyricsContributions = `Lyrics from <a id="geniusContributions" href="${response.LyricsContribution}" target="_blank">Genius.com</a>`;
                lyrics = response.Lyrics;
                songName = "You're Listening to: " + response.NowPlaying;
            }

            $('#lyrics').html(lyrics);
            $('#lyricsContributions').html(lyricsContributions);
            $('#songName').html(songName);
            spotifyAvatar = `<a id="avatar" href="${response.SpotifyProfileUrl}" target="_blank"><img src="${response.SpotifyAvatar}" alt"" class="img-spotify-avatar img-thumbnail" /></a>`;

            if (!firstLoad) {
                if (currentAlbumArt !== response.AlbumArt) {
                    currentAlbumArt = response.AlbumArt;
                    $('#albumArt').html(albumArt);
                }

                if (currentAvatar !== response.SpotifyAvatar) {
                    currentAvatar = response.SpotifyAvatar;
                    $('#spotifyAvatar').html(spotifyAvatar);
                }
            }
            else {
                $('#spotifyAvatar').html(spotifyAvatar);
                $('#albumArt').html(albumArt);
                firstLoad = false;
            }


            var title;
            if (response.NowPlaying !== null) {
                title = `<title>${response.NowPlaying} - Earworm</title>`;
                $('#nowPlayingTitle').html(title);
            }
            else {
                title = `<title>Earworm</title>`;
                $('#nowPlayingTitle').html(title);
            }

            $('#spotifyUsername').html("Hello,<br/>" + response.SpotifyName + "!");



        },
        failure: function (response) {
            alert(response);
        }
    });
}

function nowPlaying() {
    loadNowPlaying(); // This will run on page load
    setInterval(function () {
        loadNowPlaying() // this will run after every 10 seconds
    }, 10000);
}