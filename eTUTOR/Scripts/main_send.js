(function () {

    var lastPeerId = null;
    var peer = null;
    var conn = null;
    let player_send = null;
    const canvas_send = document.getElementById("streamVideo");
    const stopStream = document.getElementById("stopStream");
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
        peer = new Peer(null, {
            host: "localhost",
            port: "9000",
            path: "/",
            secure: false,
        });

        peer.on('open', function (id) {
            const whiteboard = document.getElementById("whiteBoard");
            const imageTemp = document.getElementById("imageTemp");
            // Workaround for peer.reconnect deleting previous id
            if (peer.id === null) {
                console.log('Received null id from peer open');
                peer.id = lastPeerId;
            } else {
                lastPeerId = peer.id;
            }
            whiteboard.style.cursor = "not-allowed";
            imageTemp.style.cursor = "not-allowed";
            whiteboard.style.zIndex = "-1";
            imageTemp.style.zIndex = "-1";

            console.log('ID: ' + peer.id);
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
            console.log('Connection destroyed');
        });
        peer.on('error', function (err) {
            console.log(err);
        });
    };

    /**
     * Create the connection between the two Peers.
     *
     * Sets up callbacks that handle any events related to the
     * connection and data received on it.
     */
    function join() {
        // Close old connection
        if (conn) {
            conn.close();
        }

        // debugger
        // Create connection to destination peer specified in the input field
        conn = peer.connect(course_id, {
            reliable: true
        });

        conn.on('open', function () {
            // status.innerHTML = "Connected to: " + conn.peer;
            console.log("Connected to: " + conn.peer);
            startStream();
            // Check URL params for comamnds that should be sent immediately
            var command = getUrlParam("command");
            if (command)
                conn.send(command);
        });
        // Handle incoming data (messages only since this is the signal sender)
        conn.on('data', function (data) {
            console.log(data);
        });
        conn.on('close', function () {
            // status.innerHTML = "Connection closed";
        });
    };

    /**
     * Get first "GET style" parameter from href.
     * This enables delivering an initial command upon page load.
     *
     * Would have been easier to use location.hash.
     */
    function getUrlParam(name) {
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(window.location.href);
        if (results == null)
            return null;
        else
            return results[1];
    };

    function stopStreams() {
        player_send.stop();
        conn.close();
        window.history.back();
    };

    // Send message
    function startStream() {
        if (conn.open) {
            const client_send = new WebSocket(`ws://localhost:8081/live?cam=0&course=${course_id}`);
            player_send = new jsmpeg(client_send, { canvas: canvas_send });
            client_send.onmessage = (event) => {
                conn.send(event.data);
                player_send.receiveSocketMessage(event);
            }
            // Caller
            openAudio()
                .then(oAudio => {
                    playStream('localAudio', oAudio);
                    const call = peer.call(conn.peer, oAudio);
                    call.on('stream', remoteAudio => playStream('remoteAudio', remoteAudio));
                })
                .catch(err => console.log(err));
        }
    };
    // stop stream
    stopStream.onclick = function () {
        stopStreams();
    };


    window.onunload = window.onbeforeunload = function (e) {
        if (!!peer && !peer.destroyed) {
            peer.destroy();
        }
    };

    // Since all our callbacks are setup, start the process of obtaining an ID
    initialize();
    join();
})();