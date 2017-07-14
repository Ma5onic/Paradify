(function () {
    var defaults = {
        playPauseClass: '.play-pause'
    }
    $(document).ready(function () {
        $(".number-playback-pic[playback^='http']").each(function () {
            var url = $(this).attr("playback");

            if (url != undefined && url != '') {
                var first = $(this).find(defaults.playPauseClass)[0];
                setBackgroud(first,'play');
                initClick(first,url);
                initHover(first);
            }
        });
    });

    var setBackgroud = function (elem, className) {
        $(elem).addClass(className);
    }

    var initClick = function(elem, preview_url){
        $(elem).click(function(){

            if($(elem).hasClass('play'))
            {
                play(preview_url);
                $(elem).removeClass('play');
                $(elem).addClass('pause');
            }
            else    {
                pause(preview_url);
                $(elem).removeClass('pause');
                $(elem).addClass('play');
            }
        });
    }

    var initHover = function(elem)    {
        $(elem).mouseover(function(){
            $(elem).css('background-color','black');
            $(elem).fadeTo( "fast" , 0.5, function() {

            });
        });

        $(elem).mouseout(function(){
                $(elem).css('background-color','none');
                $(elem).fadeTo( "fast" , 0, function() {
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