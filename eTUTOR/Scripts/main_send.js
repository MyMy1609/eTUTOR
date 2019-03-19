(function () {
    "use strict";
    const chars = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
    const canvas = document.getElementById("streamVideo");
    const stopStream = document.getElementById("stopStream");
    const ws_url = location.origin.replace(/^http/, 'ws');
    let player = null;
    let peer = null;
    let call = null;

    function getParameterByName(name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, '\\$&');
        let regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'), results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, ' '));
    }

    const course_id = getParameterByName('p');

    function openAudio() {
        if (navigator.mediaDevices.getUserMedia) {
            const config = { audio: true, video: false };
            return navigator.mediaDevices.getUserMedia(config);
        } else {
            alert("Browser not support");
        }
    }

    function playStream(idAudioTag, stream) {
        const audio = document.getElementById(idAudioTag);
        audio.srcObject = stream;
        audio.play();
    }

    function randomToken() {
        var res = '', i = 10, len = chars.length;
        while (i--) { res += chars[Math.floor(Math.random() * len)]; } return res;
    }

    const getCam = (id) => {
        const rtsp_URL = prompt("Please enter your url RTSP IP Camera", "");
        if (rtsp_URL && id) {
            // Caller
            openAudio()
                .then(oAudio => {
                    playStream('localAudio', oAudio);
                    call = peer.call(course_id, oAudio);
                    call.on('stream', remoteAudio => {
                        playStream('remoteAudio', remoteAudio);
                        player = new JSMpeg.Player(`ws://www.bigprotech.vn:7000/stream?id=${id}&cam=${rtsp_URL}&course=${course_id}${id}&token=${randomToken()}`, { canvas: canvas });
                    });
                    call.on('close', () => {
                        call.close();
                        peer.destroy();
                    });
                })
                .catch(err => console.log(err));
        } else {
            getCam(null);
        }
    }

    function initialize() {
        peer = new Peer(null, {
            host: "www.bigprotech.vn",
            port: "7000",
            path: "/",
            secure: false
        });

        peer.on('open', function (id) {
            //start
            const whiteboard = document.getElementById("whiteBoard");
            const imageTemp = document.getElementById("imageTemp");
            whiteboard.style.cursor = "not-allowed";
            imageTemp.style.cursor = "not-allowed";
            whiteboard.style.zIndex = "-1";
            imageTemp.style.zIndex = "-1";
            console.log('ID: ' + id);
            getCam(id);
        });
        peer.on('disconnected', function () {
            call.close();
            peer.destroy();
            console.log('Connection lost. Please reconnect');
            peer.reconnect();
        });
        peer.on('close', function () {
            call.close();
            peer.destroy();
            call = null;
            console.log('Connection destroyed');
        });
        peer.on('error', function (err) {
            console.log(err);
        });
        var heartbeat = makePeerHeartbeater(peer);
        //aaa
        function makePeerHeartbeater(peer) {
            var timeoutId = 0;
            function heartbeat() {
                timeoutId = setTimeout(heartbeat, 20000);
                if (peer.socket._wsOpen()) {
                    peer.socket.send({ type: 'HEARTBEAT' });
                }
            }
            heartbeat();
            // return
            return {
                start: function () {
                    if (timeoutId === 0) { heartbeat(); }
                },
                stop: function () {
                    clearTimeout(timeoutId);
                    timeoutId = 0;
                }
            };
        }
    };

    //canvas.addEventListener('click', function () {
    //    if (player.isPlaying) {
    //        player.pause();
    //    }
    //    else {
    //        player.play();
    //    }
    //});

    //stopStream.addEventListener('click', function () {
    //    player.stop();
    //    player.destroy();
    //});

    window.onunload = window.onbeforeunload = function (e) {
        if (!!peer && !peer.destroyed) {
            call.close();
            peer.destroy();
            player.stop();
            player.destroy();
        }
        call.close();
        e.preventDefault();
    };

    // Strigger function
    initialize();
    //join();
})();