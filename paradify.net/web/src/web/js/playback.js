(function () {
    var defaults = {
        playPauseClass: '.play-pause'
    }
    $(document).ready(function () {
        initPlayback();
    });

    this.initPlayback = function () {
        $(".number-playback-pic[playback^='http']").each(function () {
            var url = $(this).attr("playback");
            var trackName = $(this).attr("trackName");

            if (url != undefined && url != '') {
                var first = $(this).find(defaults.playPauseClass)[0];
                initClick(first, url, trackName);
                initHover(first);
            }
        });
    }

    var setBackgroud = function (elem, className) {
        $(elem).addClass(className);
    }

    var initClick = function (elem, preview_url, trackName) {
        $(elem).unbind("click");

        $(elem).click(function () {

            if ($(elem).hasClass('pause')) {
                pause(preview_url);
                $(elem).removeClass('pause');
                $(elem).addClass('play');
            }
            else {
                play(preview_url);
                $(elem).removeClass('play');
                $(elem).addClass('pause');
                gaEvent.track.play(trackName);
            }
        });
    }

    var initHover = function (elem) {
        
        $(elem).mouseover(function () {
            if ($(elem).hasClass('play') == false) {
                $(elem).addClass('play');
            }

            $(elem).css('background-color', 'black');
            $(elem).fadeTo("fast", 1, function () {

            });
        });

        $(elem).mouseout(function () {
            $(elem).css('background-color', 'none');
            $(elem).fadeTo("fast", 0, function () {
            });
        });
    }

     
}());

var p;
function play(url) {

    if (p != null) {
        if (p.src != url) {
            p.src = url;
        }
    } else {
        p = new Audio();
        p.src = url;

    }

    p.play();
}


function pause(url) {
    if (p != null && p.src == url) {
        p.pause();
    }
}