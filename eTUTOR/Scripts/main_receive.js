
(function () {
    "use strict";

    var lastPeerId = null;
    var peer = null; 
    var conn = null;
    var client_receive = null;
    var player_receive = null;
    var current_connect_cam = document.getElementById("current_connect");
    var current_connect_text = document.getElementById("current_connect_text");
    const canvas_receive = document.getElementById("streamVideo");
    const ws_url = location.origin.replace(/^http/, 'ws');

    function getParameterByName(name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, '\\$&');
        let regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'), results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, ' '));
    }

    const course_id = getParameterByName('p');

    navigator.mediaDevices.getUserMedia = navigator.mediaDevices.getUserMedia ||
        navigator.mediaDevices.webkitGetUserMedia ||
        navigator.mediaDevices.mozGetUserMedia;

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

    function initialize() {
        
        peer = new Peer(course_id, {
            host: "localhost",
            port: "9000",
            path: "/",
            secure: false,
        });

        peer.on('open', function (id) {
            
            if (peer.id === null) {
                console.log('Received null id from peer open');
                peer.id = lastPeerId;
            } else {
                lastPeerId = peer.id;
            }

            client_receive = new WebSocket(`ws://localhost:8081/live`);
            player_receive = new jsmpeg(client_receive, { canvas: canvas_receive });

            console.log('ID: ' + peer.id);
            // recvId.innerHTML = "ID: " + peer.id;
            // status.innerHTML = "Awaiting connection...";
        });
        peer.on('connection', function (c) {
            // Allow only a single connection
            if (conn) {
                c.on('open', function () {
                    c.send("Already connected to another client");
                    setTimeout(function () { c.close(); }, 500);
                });
                return;
            }

            conn = c;
            console.log("Connected to: " + conn.peer);
            // status.innerHTML = "Connected"
            ready();
        });
        peer.on('disconnected', function () {
            // status.innerHTML = "Connection lost. Please reconnect";
            console.log('Connection lost. Please reconnect');

            // Workaround for peer.reconnect deleting previous id
            peer.id = lastPeerId;
            peer._lastServerId = lastPeerId;
            peer.reconnect();
        });
        peer.on('close', function () {
            conn = null;
            // status.innerHTML = "Connection destroyed. Please refresh";
            alert(conn.peer + ' has left the room');
        });
        peer.on('error', function (err) {
            console.log(err);
            alert('' + err);
        });
    };

    /**
     * Triggered once a connection has been achieved.
     * Defines callbacks to handle incoming data and connection events.
     */
    function ready() {
        current_connect_cam.classList.add("circle__green");
        current_connect_text.innerText = " (Online) ";
        conn.on('data', function (data) {
            const obj = new Object();
            obj.data = data;
            player_receive.receiveSocketMessage(obj);
        });

        //Answer
        peer.on('call', call => {
            openAudio()
                .then(oAudio => {
                    call.answer(oAudio);
                    playStream('localAudio', oAudio);
                    call.on('stream', remoteAudio => playStream('remoteAudio', remoteAudio));
                })
                .catch(err => console.log(err));
        });

        conn.on('close', function () {
            current_connect_cam.classList.remove("circle__green");
            current_connect_text.innerText = " (Offline) ";
            //alert(conn.peer + ' has left the room');
            conn = null;
            ImgForClose();
        });
    }

    function ImgForClose() {
        //context = canvas1.getContext("2d");
        //let img = new Image();
        //img.src = "./Images/capture.png";
        //img.onload = function () {
        //    context.drawImage(img,0,0);
        //}
    }

    window.onunload = window.onbeforeunload = function (e) {
        if (!!peer && !peer.destroyed) {
            conn.close();
            peer.destroy();
        }
    };

    initialize();
})();