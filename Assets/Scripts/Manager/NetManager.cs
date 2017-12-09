using WebSocket4Net;
using SuperSocket.ClientEngine;
using Utils;
using System;
namespace Manager
{
    public class NetManager : Singleton<NetManager>
    {
        private static readonly string WEBSOCKET_PATH = "/websocket";

        private static readonly string URI = "ws://127.0.0.1:8080/" + WEBSOCKET_PATH;

        private WebSocket webSocket = null;

        public override void Initialize()
        {
            base.Initialize();

            webSocket = new WebSocket(URI);
            webSocket.Opened += new EventHandler(OnWebSocketOpened);
            webSocket.Error += new EventHandler<ErrorEventArgs>(OnWebSocketError);
            webSocket.Closed += new EventHandler(OnWebSocketClosed);
            webSocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(OnWebSocketMessageReceived);
            
        }

        public void Open()
        {
            webSocket.Open();
        }

        public void Send()
        {
          
        }

        public void Close()
        {
            webSocket.Close();
        }

        private void OnWebSocketOpened(object sender, EventArgs e)
        {
            LogUtils.LogDebug("OnWebSocketOpened");
        }

        private void OnWebSocketError(object sender, ErrorEventArgs e)
        {
            LogUtils.LogDebug("OnWebSocketError",e.Exception.Message);
        }

        private void OnWebSocketClosed(object sender, EventArgs e)
        {
            LogUtils.LogDebug("OnWebSocketClosed");
        }

        private void OnWebSocketMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            LogUtils.LogDebug("OnWebSocketMessageReceived");
        }

        public override void Release()
        {
            base.Release();
            
            webSocket.Close();
        }
    }
}