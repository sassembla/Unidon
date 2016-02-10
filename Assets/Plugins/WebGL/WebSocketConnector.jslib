var WebSocketConnector = {
    Connect: function() {
		wsConnectioonBasement = {
			connected: false,
			uri: 'ws://127.0.0.1:4649/',
			init : function (e) {
				wsConnectioonBasement.reinit(e);
			},
			reinit : function (e) {
				wsConnectioonBasement.socket = new WebSocket(wsConnectioonBasement.uri);
				wsConnectioonBasement.socket.onopen = function (e) { wsConnectioonBasement.onOpen(e); };
				wsConnectioonBasement.socket.onclose = function (e) { wsConnectioonBasement.onClose(e); };
				wsConnectioonBasement.socket.onmessage = function (e) { wsConnectioonBasement.onMessage(e); };
				wsConnectioonBasement.socket.onerror = function (e) { wsConnectioonBasement.onError(e); };
			},

			onOpen: function () {
				console.log("opened1");
				wsConnectioonBasement.socket.send("connected from client.");
				console.log("opened2");
				wsConnectioonBasement.connected = true;
				console.log("opened3");
			},

			onClose: function () {
				console.log("closed!");
				wsConnectioonBasement.connected = false;
				
				wsConnectioonBasement.socket.onopen = function () {};
				wsConnectioonBasement.socket.onclose = function () {};
				wsConnectioonBasement.socket.onmessage = function () {};
				wsConnectioonBasement.socket.onerror = function () {};

				// reconnection
				// setTimeout(basement.reinit, 1000);
			},

			onMessage: function (e) {
				console.log("received:" + e.data);
			},

			onError: function (e) {
				console.log("error:" + e.data);
			}
		};

		wsConnectioonBasement.init();
		
		console.log("hello!!!");
    },
    SendLog: function(message) {
		console.log("sending..");
		var messageStr = Pointer_stringify(message);
        if (wsConnectioonBasement.connected) {
			wsConnectioonBasement.socket.send("client:" + messageStr);
		} else {
			console.log("not yet connected.");
		}
    }
};


mergeInto(LibraryManager.library, WebSocketConnector);