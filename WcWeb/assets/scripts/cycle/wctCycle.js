$(function() {
    //see foxt for advanced implementation
    //http://jquery.malsup.com/cycle/options.html
    //speed - speed of the transition effect / timeout - time between transitions / delay - extra delay for first image
    $('#cycle-wrapper').cycle({ cleartype: true, fx: 'fade', speed: 5000, timeout: 6800, delay: 0, cleartypeNoBg: true    });
});
