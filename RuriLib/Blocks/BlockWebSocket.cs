using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Windows.Media;
using AngleSharp.Text;
using RuriLib.LS;
using WebSocketSharp;

namespace RuriLib
{
    /// <summary>
    /// Available commands for the WebSocket.
    /// </summary>
    public enum WSCommand
    {
        /// <summary>Connects the client to a url.</summary>
        Connect,

        /// <summary>Disconnects the client from the connected url.</summary>
        Disconnect,

        /// <summary>Sends a message to the connected url.</summary>
        Send
    }

    /// <summary>
    /// A block that websocket connect to url
    /// </summary>
    public class BlockWebSocket : BlockBase
    {

        #region Variables

        private string variableName = "";
        /// <summary>The name of the output variable where the TCP response will be stored.</summary>
        public string VariableName { get { return variableName; } set { variableName = value; OnPropertyChanged(); } }

        private bool isCapture = false;
        /// <summary>Whether the output variable should be marked for Capture.</summary>
        public bool IsCapture { get { return isCapture; } set { isCapture = value; OnPropertyChanged(); } }

        private string url = string.Empty;
        /// <summary>
        /// WebSocket URL to connect.
        /// </summary>
        public string Url { get { return url; } set { url = value; OnPropertyChanged(); } }

        private CompressionMethod compression;
        /// <summary>
        /// Gets or sets the compression method used to compress a message on the WebSocket connection.
        /// </summary>
        public CompressionMethod Compression { get { return compression; } set { compression = value; OnPropertyChanged(); } }

        private string origin;
        /// <summary>
        ///   Gets or sets the value of the HTTP Origin header to send with the WebSocket handshake request to the server.
        /// </summary>
        public string Origin { get { return origin; } set { origin = value; OnPropertyChanged(); } }

        private TimeSpan waitTime = TimeSpan.FromSeconds(5);
        /// <summary>
        /// Gets or sets the wait time for the response to the Ping or Close.
        /// </summary>
        public TimeSpan WaitTime { get { return waitTime; } set { waitTime = value; OnPropertyChanged(); } }

        private bool emitOnPing;
        /// <summary>
        ///  Gets or sets a value indicating whether the WebSocketSharp.WebSocket emits a WebSocketSharp.WebSocket.OnMessage event when receives a ping.
        /// </summary>
        public bool EmitOnPing { get { return emitOnPing; } set { emitOnPing = value; OnPropertyChanged(); } }

        private bool redirection;
        /// <summary>
        /// Gets or sets a value indicating whether the WebSocketSharp.WebSocket redirects
        /// the handshake request to the new URL located in the handshake response.
        /// </summary>
        public bool Redirection
        {
            get { return redirection; }
            set { redirection = value; OnPropertyChanged(); }
        }

        private WSCommand command;
        /// <summary>
        /// Websocket command
        /// </summary>
        public WSCommand Command
        {
            get { return command; }
            set { command = value; OnPropertyChanged(); }
        }

        private string message;
        /// <summary>
        /// Message for send the WebSocket connection.
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged(); }
        }

        private SslProtocols sslProtocols = SslProtocols.Default;
        /// <summary>
        /// Gets or sets the SSL protocols used for authentication.
        /// </summary>
        public SslProtocols SslProtocols
        {
            get { return sslProtocols; }
            set { sslProtocols = value; OnPropertyChanged(); }
        }

        /// <summary>The custom cookies that</summary>
        public Dictionary<string, string> CustomCookies { get; set; } = new Dictionary<string, string>() { };

        private bool credentials;
        public bool Credentials { get { return credentials; } set { credentials = value; OnPropertyChanged(); } }

        private string username;
        public string Username { get { return username; } set { username = value; OnPropertyChanged(); } }

        private string password;
        public string Password { get { return password; } set { password = value; OnPropertyChanged(); } }

        private bool preAuth;
        public bool PreAuth { get { return preAuth; } set { preAuth = value; OnPropertyChanged(); } }

        #endregion Vars

        /// <summary>
        /// Creates a WebSocket block.
        /// </summary>
        public BlockWebSocket()
        {
            Label = "WS";
        }

        /// <inheritdoc/>
        public override void Process(BotData data)
        {
            base.Process(data);

            var ws = data.GetCustomObject(nameof(WebSocket)) as WebSocket;
            var receivedMsg = string.Empty;
            bool onMsg = false;

            switch (Command)
            {
                case WSCommand.Connect:
                    {
                        var inputs = ReplaceValues(Url, data);
                        try { ws?.Close(CloseStatusCode.NoStatus); } catch { }
                        ws = new WebSocket(inputs)
                        {
                            Compression = Compression,
                            Origin = Origin,
                            WaitTime = WaitTime,
                            EmitOnPing = EmitOnPing,
                            EnableRedirection = Redirection,
                        };
                        ws.SslConfiguration.EnabledSslProtocols = SslProtocols;
                        //ws.SetProxy()

                        if (Credentials)
                            ws.SetCredentials(ReplaceValues(Username, data), ReplaceValues(Password, data), PreAuth);

                        // Set cookies
                        data.Log(new LogEntry("Sent Cookies:", Colors.MediumTurquoise));

                        foreach (var cookie in CustomCookies) // Add new user-defined custom cookies to the bot's cookie jar
                        {
                            ws.SetCookie(new WebSocketSharp.Net.Cookie(ReplaceValues(cookie.Key, data), ReplaceValues(cookie.Value, data), "/"));
                            data.LogBuffer.Add(new LogEntry($"{cookie.Key}: {cookie.Value}", Colors.MediumTurquoise));
                        }
                        foreach (var cookie in data.Cookies)
                        {
                            ws.SetCookie(new WebSocketSharp.Net.Cookie(ReplaceValues(cookie.Key, data), ReplaceValues(cookie.Value, data), "/"));
                            data.LogBuffer.Add(new LogEntry($"{cookie.Key}: {cookie.Value}", Colors.MediumTurquoise));
                        }
#if DEBUG
                        ws.Log.Level = WebSocketSharp.LogLevel.Trace;
#endif

                        ws.OnMessage += new EventHandler<MessageEventArgs>((s, e) =>
                        {
                            onMsg = true;
                            var msg = string.Empty;
                            if (e.IsText)
                                receivedMsg += $"{msg = e.Data}\n";
                            else if (e.IsBinary)
                                receivedMsg += $"{msg = Encoding.ASCII.GetString(e.RawData)}\n";

                            data.Log(new LogEntry($"On Message Ev: {msg}", Colors.Yellow));
                        });

                        ws.ConnectAsync();

                        if (!WaitForConnect(ws))
                        {
                            throw new Exception($"Connection Status: {ws.ReadyState}");
                        }

                        data.CustomObjects[nameof(WebSocket)] = ws;
                        data.Log(new LogEntry($"Succesfully connected to url: {inputs}.", Colors.LimeGreen));
                        data.Log(new LogEntry($"Connection Status: {ws.ReadyState}", Colors.LimeGreen));
                        data.Log(new LogEntry(receivedMsg, Colors.GreenYellow));
                    }
                    break;
                case WSCommand.Disconnect:
                    {
                        if (ws == null)
                        {
                            throw new Exception("Make a connection first!");
                        }
                        ws.CloseAsync(CloseStatusCode.Normal);
                        ws = null;
                        data.Log(new LogEntry($"Succesfully closed", Colors.GreenYellow));
                        data.CustomObjects[nameof(WebSocket)] = null;
                    }
                    break;
                case WSCommand.Send:
                    {
                        if (ws == null)
                        {
                            throw new Exception("Make a connection first!");
                        }
                        var msg = ReplaceValues(Message, data);
                        var bytes = Encoding.ASCII.GetBytes(msg.Unescape());
                        bool? wsSendReplied = null;
                        data.Log(new LogEntry($"Sending {Message}", Colors.GreenYellow));
                        ws.SendAsync(bytes, (completed) =>
                         {
                             wsSendReplied = completed;
                             if (completed)
                             {
                                 data.Log(new LogEntry("Success to send Message", Colors.GreenYellow));
                             }
                             else
                             {
                                 data.Log(new LogEntry("Failure to send Message", Colors.Red));
                             }
                         });
                        TaskExtensions.WaitUntil(() => wsSendReplied.HasValue, timeout: 100).Wait();
                    }
                    break;
            }
            try
            {
                TaskExtensions.WaitUntil(() => onMsg,
                timeout: 500)
                .Wait();
            }
            catch { }

            if (!string.IsNullOrEmpty(VariableName))
                InsertVariable(data, IsCapture, receivedMsg, VariableName);
        }

        private bool WaitForConnect(WebSocket ws)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            while (ws.ReadyState == WebSocketState.Connecting)
            {
                if (stopWatch.Elapsed.TotalSeconds >= WaitTime.TotalSeconds)
                {
                    return OpenState(ws);
                }
            }
            stopWatch.Stop();
            return OpenState(ws);
        }

        private bool OpenState(WebSocket ws)
        {
            return ws.ReadyState == WebSocketState.Open;
        }

        /// <inheritdoc/>
        public override BlockBase FromLS(string line)
        {
            // Trim the line
            var input = line.Trim();

            // Parse the label
            if (input.StartsWith("#"))
                Label = LineParser.ParseLabel(ref input);

            // Parse the function
            Command = (WSCommand)LineParser.ParseEnum(ref input, nameof(Command), typeof(WSCommand));

            switch (Command)
            {
                case WSCommand.Connect:
                    Url = LineParser.ParseLiteral(ref input, nameof(Url));

                    while (LineParser.Lookahead(ref input) == TokenType.Boolean)
                        LineParser.SetBool(ref input, this);

                    ParseVarOrCap(ref input);

                    while (input != "" && LineParser.Lookahead(ref input) == TokenType.Parameter)
                    {
                        var parsed = LineParser.ParseToken(ref input, TokenType.Parameter, true).ToUpper();
                        switch (parsed)
                        {
                            case "USERNAME":
                                Username = LineParser.ParseToken(ref input, TokenType.Parameter, false);
                                break;

                            case "PASSWORD":
                                Password = LineParser.ParseToken(ref input, TokenType.Parameter, false);
                                break;

                            case "PREAUTH":
                                PreAuth = LineParser.ParseToken(ref input, TokenType.Parameter, false).ToBoolean();
                                break;

                            case "SSLPROTO":
                                SslProtocols = LineParser.ParseEnum(ref input, "SSL Protocols", typeof(SslProtocols));
                                break;

                            case "COMPRESSION":
                                Compression = LineParser.ParseEnum(ref input, "Compression Method", typeof(CompressionMethod));
                                break;

                            case "COOKIE":
                                var cookiePair = ParseString(LineParser.ParseLiteral(ref input, "COOKIE VALUE"), ':', 2);
                                CustomCookies[cookiePair[0]] = cookiePair[1];
                                break;
                        }
                    }

                    break;
                case WSCommand.Send:
                    Message = LineParser.ParseLiteral(ref input, nameof(Message));
                    if (LineParser.Lookahead(ref input) == TokenType.Boolean)
                        LineParser.SetBool(ref input, this);
                    ParseVarOrCap(ref input);
                    break;
                default:
                    ParseVarOrCap(ref input);
                    break;
            }

            return this;
        }

        private void ParseVarOrCap(ref string input)
        {
            // Try to parse the arrow, otherwise just return the block as is with default var name and var / cap choice
            if (LineParser.ParseToken(ref input, TokenType.Arrow, false) == string.Empty)
                return;

            // Parse the VAR / CAP
            try
            {
                var varType = LineParser.ParseToken(ref input, TokenType.Parameter, true);
                if (varType.ToUpper() == "VAR" || varType.ToUpper() == "CAP")
                    IsCapture = varType.ToUpper() == "CAP";
            }
            catch { throw new ArgumentException("Invalid or missing variable type"); }

            // Parse the variable/capture name
            try { VariableName = LineParser.ParseToken(ref input, TokenType.Literal, true); }
            catch { throw new ArgumentException("Variable name not specified"); }
        }

        private void WriteVarOrCap(BlockWriter writer)
        {
            if (!writer.CheckDefault(VariableName, nameof(VariableName)))
                writer.Arrow()
                    .Token(IsCapture ? "CAP" : "VAR")
                    .Literal(VariableName);
        }

        /// <inheritdoc/>
        public override string ToLS(bool indent = true)
        {
            BlockWriter writer = new BlockWriter(GetType(), indent, Disabled);

            writer.Label(Label)
                .Token("WS")
                .Token(Command);

            switch (Command)
            {
                case WSCommand.Connect:
                    {
                        writer.Literal(Url);
                        if (Redirection)
                            writer.Boolean(Redirection, nameof(Redirection));

                        if (EmitOnPing)
                            writer.Boolean(EmitOnPing, nameof(EmitOnPing));

                        if (Credentials)
                            writer.Boolean(Credentials, nameof(Credentials));

                        WriteVarOrCap(writer);

                        if (Credentials)
                        {
                            writer.Indent()
                                .Token("USERNAME")
                                .Token(Username, nameof(Username));
                            writer.Indent()
                               .Token("PASSWORD")
                               .Token(Password, nameof(Password));
                            if (PreAuth)
                                writer.Indent()
                                   .Token("PREAUTH")
                                   .Token(PreAuth, nameof(PreAuth));
                        }

                        if (SslProtocols != SslProtocols.Default)
                            writer.Indent()
                                .Token("SSLPROTO")
                                .Token(SslProtocols, nameof(SslProtocols));

                        if (Compression != CompressionMethod.None)
                            writer.Indent()
                                .Token("COMPRESSION")
                                .Token(Compression, nameof(Compression));

                        foreach (var c in CustomCookies)
                        {
                            writer.Indent()
                                .Token("COOKIE")
                                .Literal($"{c.Key}: {c.Value}");
                        }
                    }
                    break;
                case WSCommand.Send:
                    {
                        writer.Literal(Message);
                        WriteVarOrCap(writer);
                    }
                    break;
                default:
                    WriteVarOrCap(writer);
                    break;
            }

            return writer.ToString();
        }

        /// <summary>
        /// Sets custom headers from an array of lines.
        /// </summary>
        /// <param name="lines">The lines containing the colon-separated name and value of the headers</param>
        public void SetCustomCookies(string[] lines)
        {
            CustomCookies.Clear();
            foreach (var line in lines)
            {
                if (line.Contains(':'))
                {
                    var split = line.Split(new[] { ':' }, 2);
                    CustomCookies[split[0].Trim()] = split[1].Trim();
                }
            }
        }

        /// <summary>
        /// Builds a string containing custom headers.
        /// </summary>
        /// <returns>One header per line, with name and value separated by a colon</returns>
        public string GetCustomHeaders()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var pair in CustomCookies)
            {
                sb.Append($"{pair.Key}: {pair.Value}");
                if (!pair.Equals(CustomCookies.Last())) sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Parses values from a string.
        /// </summary>
        /// <param name="input">The string to parse</param>
        /// <param name="separator">The character that separates the elements</param>
        /// <param name="count">The number of elements to return</param>
        /// <returns>The array of the parsed elements.</returns>
        public static string[] ParseString(string input, char separator, int count)
        {
            return input.Split(new[] { separator }, count).Select(s => s.Trim()).ToArray();
        }
    }
}
