var currentAlbumArt = null;
var currentAvatar = null;
var firstLoad = true;

function loadNowPlaying() {
    console.log('loadNowPlaying');
    $('#result').html('Loading...');
    $.ajax({
        type: "GET",
        url: "home/nowplaying",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var albumArt = `<a id="art" href="${response.AlbumUrl}" target="_blank"><img src="${response.AlbumArt}" class="img-thumbnail" /></a>`;
            var spotifyAvatar = `<a id="avatar" href="${response.SpotifyProfileUrl}" target="_blank"><img src="${response.SpotifyAvatar}" alt"" class="img-spotify-avatar img-thumbnail" /></a>`;
            var lyricsContributions = `Lyrics from <a id="geniusContributions" href="${response.LyricsContribution}" target="_blank">Genius.com</a>`;
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
            
            $('#lyrics').html(response.Lyrics);
            $('#lyricsContributions').html(lyricsContributions);
            $('#songName').html(response.NowPlaying);
            var title;
            if (response.NowPlaying !== null) {
                title = `<title>${response.NowPlaying} - Earworm</title>`;
                $('#nowPlayingTitle').html(title);
            }
            else {
                title = `<title>Earworm</title>`;
                $('#nowPlayingTitle').html(title);
            }
            
            $('#spotifyUsername').html(response.SpotifyName);
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