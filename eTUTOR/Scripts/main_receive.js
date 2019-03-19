(function () {
    "use strict";
    const chars = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
    const ws_url = location.origin.replace(/^http/, 'ws');
    let player = null;
    let peer = null;
    let conn = null;
    let curPeer = {};

    function getParameterByName(name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, '\\$&');
        let regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'), results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, ' '));
    }

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

    function checkClose(data, call) {
        //data
        if (typeof data === ('object' || 'arraybuffer' || 'blob')) {
            var decoded = String.fromCharCode.apply(null, new Uint8Array(data));
            if (decoded.indexOf('"type":') !== -1) {
                conn = call;
                delete curPeer[`${conn.peer}`];
                $(`#${call.peer}`).remove();
                call.close();
            }
        }
    }

    const course_id = getParameterByName('p');

    const initAjax = (call) => {
        const canvas = document.getElementById(`streamVideo_${call.peer}`);
        openAudio()
            .then(oAudio => {
                call.answer(oAudio);
                playStream(`localAudio_${call.peer}`, oAudio);
                call.on('stream', remoteAudio => {
                    playStream(`remoteAudio_${call.peer}`, remoteAudio);
                    player = new JSMpeg.Player(`ws://www.bigprotech.vn:7000/stream?id=${course_id}${call.customID}&course=${course_id}${call.peer}&token=${randomToken()}`, { canvas: canvas });
                    setTimeout(function () {
                        let socket = player.source.socket;
                        let thisIs = player.source;
                        socket.onmessage = function (evt) {
                            var data = evt.data;
                            var I = !thisIs.established;
                            thisIs.established = !0,
                                I && thisIs.onEstablishedCallback && thisIs.onEstablishedCallback(thisIs),
                                thisIs.destination && thisIs.destination.write(data)
                            checkClose(data, call);
                        }
                    }, 300);
                });
            })
            .catch(err => console.log(err));
    };

    function initialize() {

        peer = new Peer(course_id, {
            host: "www.bigprotech.vn",
            port: "7000",
            path: "/",
            secure: false
        });

        peer.on('open', id => console.log('ID: ' + id));

        //Answer
        peer.on('call', call => {
            curPeer[call.peer] = call;
            //create canvas
            $(document).ready(function () {
                //if (curPeerIdDOM.indexOf(c.random) === -1 && c.random) {
                call.customID = randomToken();
                const child = `<div id="${call.peer}"><h3>
                                Hình ảnh
                                <span id="current_connect" class="circle__item circle__green"></span>
                                <span id="current_connect_text">  (Online) </span>
                            </h3>
                            <div class="mr-5">
                                <canvas id="streamVideo_${call.peer}"></canvas>
                                <audio id="localAudio_${call.peer}" autoplay></audio>
                                <audio id="remoteAudio_${call.peer}" autoplay></audio>
                            </div></div>`;
                $("#container-cam").append(child);
                initAjax(call);
                //curPeerIdDOM.push(c.random);
            });
            call.on('close', () => {
                $(`#${call.peer}`).remove();
            });
        });
        peer.on('disconnected', function () {
            console.log('Connection lost. Please reconnect');
        });
        peer.on('close', function () {
            console.log(curPeer[`${conn.peer}`] + ' has left the room');
            delete curPeer[`${conn.peer}`];
        });
        peer.on('error', function (err) {
            console.log('Err ' + err);
        });

        var heartbeat = makePeerHeartbeater(peer);
        //aaaa
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

    window.onunload = window.onbeforeunload = function (e) {
        if (!!peer && !peer.destroyed) {
            conn.close();
            peer.destroy();
            player.stop();
            player.destroy();
        }
        e.preventDefault();
    };

    initialize();
})();