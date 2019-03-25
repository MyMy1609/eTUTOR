(function () {
    "use strict";
    const chars = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
    const ws_url = location.origin.replace(/^http/, 'ws');
    var player = null;
    var peer = null;
    var conn = null;
    var curPeer = 0;

    function getParameterByName(name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, '\\$&');
        let regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'), results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, ' '));
    }

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

    function status(data, call) {
        if (typeof data === 'string') {
            var decoded = JSON.parse(data);
            switch (decoded.type) {
                case "OPEN":
                    conn = call;
                    console.log(decoded.payload.msg);
                    break;
                case "ERROR":
                    conn = call;
                    call.close(decoded.payload.msg);
                    break;
                default: break;
            }
        }
    }

    const course_id = getParameterByName('p');

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

    const initAjax = (call) => {
        const canvas = document.getElementById(`streamVideo_${call.peer}`);
        openAudio()
            .then(oAudio => {
                call.answer(oAudio);
                playStream(`localAudio_${call.peer}`, oAudio);
                call.on('stream', remoteAudio => {
                    playStream(`remoteAudio_${call.peer}`, remoteAudio);
                    player = new JSMpeg.Player(`ws://www.bigprotech.vn:7000/stream?id=${course_id}${randomToken()}&course=${course_id}${call.peer}&type=tutor&token=${peer.options.token}`, { canvas: canvas });
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
            })
            .catch(err => console.log(err));
        $(".stream_zoom").click(function () {
            $(this).toggleClass("show");
            $(this).children("canvas").toggleClass("zoom");
        });
    };

    function initialize() {

        peer = new Peer(course_id, {
            host: "www.bigprotech.vn",
            port: "7000",
            path: "/",
            secure: false
        });

        peer.on('open', id => makePeerHeartbeater(peer).start());

        //Answer
        peer.on('call', call => {
            curPeer++;
            console.log(call);
            console.log(peer);
            //create canvas
            $(document).ready(function () {
                const child_HD = `<div id="cam_${call.peer}" data-id="${curPeer}"><h3>
                                Hình ảnh
                                <span id="current_connect" class="circle__item circle__green"></span>
                                <span id="current_connect_text">  (Online) </span>
                            </h3>
                            <div class="stream_zoom mr-5">
                                <canvas id="streamVideo_${call.peer}"></canvas>
                            </div></div>`;
                const child_Audio = `<div id="audio_${call.peer}" data-id="${curPeer}"><audio id="localAudio_${call.peer}" autoplay></audio>
                 <audio id="remoteAudio_${call.peer}" autoplay></audio></div>`;
                $("#container-cam").append(child_HD);
                $("#container-audio").append(child_Audio);
                initAjax(call);
            });
            call.on('close', () => {
                $(`#cam_${call.peer}`).remove();
                $(`#audio_${call.peer}`).remove();
                makePeerHeartbeater(peer).stop();
            });
        });
        peer.on('disconnected', function () {
            console.log('Connection lost. Please reconnect');
        });
        peer.on('close', function () {
            console.log(conn.peer + ' has left the room');
        });
        peer.on('error', function (err) {
            console.log('Err ' + err);
        });

        if (window.navigator.onLine) {
            console.log("online");
        } else {
            console.log("offline");
        }
    };

    //$(document).ready(function () {
    //    $(document).on("keydown", disableF5);
    //});

    window.onunload = window.onbeforeunload = function (e) {
        if (!!peer && !peer.destroyed) {
            conn.close();
            peer.destroy();
            player.stop();
        }
        e.preventDefault();
        return "Reload ?"
    };

    initialize();
})();