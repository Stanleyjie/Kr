using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ServiceExt
{
    public class WebSocketHelper
    {
        private static ConcurrentDictionary<string, WebSocket> _socketConnectUsers;
        /// <summary>
        /// 用于存储在线的websocket用户
        /// </summary>
        public static ConcurrentDictionary<string, WebSocket> SocketConnectUsers
        {
            get
            {
                return _socketConnectUsers;
            }
        }
        WebSocket socket;
        private string _sockSenderId = "";

        public WebSocketHelper(WebSocket socket)
        {
            this.socket = socket;
        }
        async Task EchoLoop()
        {

            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await this.socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (_socketConnectUsers == null)
            {
                _socketConnectUsers = new ConcurrentDictionary<string, WebSocket>();
            }

            if (!result.CloseStatus.HasValue)//第一次连接 
            {
                WSMessageHelper OneModel = null;
                try
                {
                    OneModel = JsonConvert.DeserializeObject<WSMessageHelper>(ReceiveString(buffer, result));
                    _sockSenderId = OneModel.SenderId;
                }
                catch (Exception)
                {
                }
                if (string.IsNullOrEmpty(_sockSenderId))
                {
                    _sockSenderId = Guid.NewGuid().ToString();
                }

                if (_socketConnectUsers.TryAdd(_sockSenderId, socket))
                {
                    await SendAsync(new WSMessageHelper
                    {//连接成功时返回用户id
                        SenderId = "Service",
                        ReceiverId = _sockSenderId,
                        MessageType = "text",
                        Content = "open"
                    }, WebSocketMessageType.Text, result.EndOfMessage);
                }
            }
            while (!result.CloseStatus.HasValue)
            {
                string sendMsg = ReceiveString(buffer, result);
                WSMessageHelper wSMessage = null;
                if (!string.IsNullOrWhiteSpace(sendMsg))
                {
                    try
                    {
                        wSMessage = JsonConvert.DeserializeObject<WSMessageHelper>(sendMsg);
                    }
                    catch (Exception)
                    {
                        wSMessage = new WSMessageHelper //，失败时回发用户消息
                        {
                            SenderId = "Service",
                            ReceiverId = _sockSenderId,
                            MessageType = "text",
                            Content = sendMsg
                        };
                    }

                }
                if (wSMessage != null && !string.IsNullOrWhiteSpace(wSMessage.ReceiverId) && !wSMessage.ReceiverId.ToLower().Equals("service"))
                {
                    if (!await SendAsync(wSMessage, WebSocketMessageType.Text, true))//false表示发送失败
                    {
                        await SendAsync(new WSMessageHelper //，失败时回发用户消息
                        {
                            SenderId = "Service",
                            ReceiverId = _sockSenderId,
                            MessageType = "text",
                            Content = "消息发送不成功，用户已下线"
                        }, WebSocketMessageType.Text, result.EndOfMessage);
                    }
                }
                result = await this.socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            _socketConnectUsers.TryRemove(_sockSenderId, out socket);
            await this.socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息对象</param>
        /// <param name="messageType">消息类型</param>
        /// <param name="endOfMessage">是否结束消息</param>
        /// <returns></returns>
        public async Task<bool> SendAsync(WSMessageHelper message, WebSocketMessageType messageType, bool endOfMessage)
        {
            var buffer1 = Encoding.Default.GetBytes(JsonConvert.SerializeObject(message));
            var outgoing = new ArraySegment<byte>(buffer1, 0, buffer1.Length);
            WebSocket socket = null;
            if (_socketConnectUsers.TryGetValue(message.ReceiverId, out socket))
            {
                await socket.SendAsync(outgoing, messageType, endOfMessage, CancellationToken.None);
                return true;
            }
            return false;

        }

        private string ReceiveString(ArraySegment<byte> buffer, WebSocketReceiveResult result)
        {
            using (var ms = new MemoryStream())
            {
                do
                {
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);
                if (result.MessageType != WebSocketMessageType.Text)
                {
                    return null;
                }

                using (var reader = new StreamReader(ms, Encoding.Default))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        static async Task Acceptor(HttpContext hc, Func<Task> n)
        {
            if (!hc.WebSockets.IsWebSocketRequest)
                return;
            var socket = await hc.WebSockets.AcceptWebSocketAsync();
            var h = new WebSocketHelper(socket);
            await h.EchoLoop();
        }
        /// <summary>
        /// 路由绑定处理
        /// </summary>
        /// <param name="app"></param>
        public static void Map(IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.Use(WebSocketHelper.Acceptor);
        }
    }

    public class WSMessageHelper
    {
        /// <summary>
        /// 发送者Id
        /// </summary>
        public string SenderId { get; set; }

        /// <summary>
        /// 接受者id
        /// </summary>
        public string ReceiverId { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string MessageType { get; set; }

        public object Content { get; set; }
    }
}
