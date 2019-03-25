(function () {
    "use strict";
    var chars = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
    var canvas = document.getElementById("streamVideo");
    var stopStream = document.getElementById("stopStream");
    var ws_url = location.origin.replace(/^http/, 'ws');
    var stt = $("#stt").attr("data-id");
    var player = null;
    var peer = null;
    var call = null;

    function getParameterByName(name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, '\\$&');
        let regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'), results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, ' '));
    }

    const course_id = getParameterByName('p');

    //function disableF5(e) { if ((e.which || e.keyCode) == 116 || (e.which || e.keyCode) == 82) e.preventDefault(); };

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

    function makePeerHeartbeater(peer) {
        var timeoutId = 0;
        function heartbeat() {
            timeoutId = setTimeout(heartbeat, 20000);
            if (peer.socket._wsOpen()) {
                peer.socket.send({ type: 'HEARTBEAT' });
            }
        }
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

    function status(data, call) {
        if (typeof data === 'string') {
            var decoded = JSON.parse(data);
            switch (decoded.type) {
                case "OPEN":
                    console.log("OPEN");
                    break;
                case "DUPLICATES":
                    alert(decoded.payload.msg);
                    call.close();
                    window.close();
                    break;
                case "ERROR":
                    alert(decoded.payload.msg);
                    call.close();
                    break;
                default: break;
            }
        }
    }

    function disableDom() {
        var whiteboard = document.getElementById("whiteBoard");
        var imageTemp = document.getElementById("imageTemp");
        whiteboard.style.cursor = "not-allowed";
        imageTemp.style.cursor = "not-allowed";
        whiteboard.style.zIndex = "-1";
        imageTemp.style.zIndex = "-1";
    }

    const getCam = (id) => {
        //const rtsp_URL = prompt("Please enter your url RTSP IP Camera", "");
        if (id) {
            // Caller
            openAudio()
                .then(oAudio => {
                    playStream('localAudio', oAudio);
                    call = peer.call(course_id, oAudio);
                    call.on('stream', remoteAudio => {
                        disableDom();
                        makePeerHeartbeater(peer).start();
                        playStream('remoteAudio', remoteAudio);
                        player = new JSMpeg.Player(`ws://www.bigprotech.vn:7000/stream?id=${id}&course=${course_id}${id}&type=student&token=${peer.options.token}`, { canvas: canvas });
                        setTimeout(function () {
                            var thisIs = player.source;
                            thisIs.socket.onmessage = function (evt) {
                                var data = evt.data;
                                var I = !thisIs.established;
                                thisIs.established = !0,
                                    I && thisIs.onEstablishedCallback && thisIs.onEstablishedCallback(thisIs),
                                    thisIs.destination && thisIs.destination.write(data)
                                status(data, call);
                            }
                        }, 300);

                    });
                    call.on('close', () => {
                        makePeerHeartbeater(peer).stop();
                    });
                })
                .catch(err => console.log(err));
            // End
        } else {
            getCam(null);
        }
    }

    function initialize() {
        peer = new Peer(stt, {
            host: "www.bigprotech.vn",
            port: "7000",
            path: "/",
            secure: false
        });

        peer.on('open', function (id) {
            //start
            console.log('ID: ' + id);
            getCam(id);
        });
        peer.on('disconnected', function () {
            call.close();
            console.log('Connection lost. Please reconnect');
            peer.reconnect();
        });
        peer.on('close', function () {
            call.close();
            call = null;
            console.log('Connection destroyed');
        });
        peer.on('error', function (err) {
            alert("Chưa tới giờ học");
            window.close();
        });
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

    //$(document).ready(function () {
    //    $(document).on("keydown", disableF5);
    //});

    window.onunload = window.onbeforeunload = function (e) {
        if (!!peer && !peer.destroyed) {
            call.close();
            peer.destroy();
            player.stop();
        }
        call.close();
        e.preventDefault();
        return "Reload ?"
    };

    // Strigger function
    initialize();
})();