namespace on_screen_keylogger.Handlers
{
    /*public static class AppUpdateHandler
    {
        //========================================================
        public const string ApiHost    = "api.github.com";
        public const string Username   = "TheCSDev";
        public const string Repository = "on-screen-keylogger";
        //--------------------------------------------------------
        public static string ReleaseApiPath => "/repos/{$user}/{$repo}/releases"
            .Replace("{$user}", Username).Replace("{$repo}", Repository);
        //========================================================
        public static void CheckForUpdates(MainWindow window) =>
            window.Dispatcher.InvokeAsync(async () => await CheckForUpdatesAsync(window));
        //
        private static async Task CheckForUpdatesAsync(MainWindow window)
        {
            //wait for core web view
            await window.WaitForCoreWebViewAsync();

            //post and get response
            string response = HttpPost(window);
            Console.WriteLine(response);

            //okay so no matter how hard i try to get this working, it wont.
            //i tried many methods of sending POST requests, and all of them
            //end up in the thread freezing/awaiting indefinitely, so I
            //think i will give up on this one.
        }
        //========================================================
        private static string HttpPost(MainWindow window)
        {
            //define the post message that will be sent
            string content = "per_page=1";

            string request_message =
                "POST " + ReleaseApiPath + " HTTP/1.1\n" +
                "Host: " + ApiHost + "\n" +
                "User-Agent: " + window.webBrowser.CoreWebView2.Settings.UserAgent + "\n" +
                "Accept-Language: en-US,en;q=1\n" +
                "Content-Type: text/plain\n" +
                "Content-Length: " + content.Length + "\n" +
                "\n" +
                content;
            
            //get remote end point
            IPAddress hostIp = Dns.GetHostEntry(ApiHost).AddressList[0];
            IPEndPoint remoteEndPoint = new IPEndPoint(hostIp, 443);

            //connect
            Socket socket = new Socket(hostIp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(remoteEndPoint);

            //send message
            socket.Send(Encoding.ASCII.GetBytes(request_message));

            //recieve response
            byte[] buffer = new byte[1024];
            socket.Receive(buffer);

            //return
            return Encoding.ASCII.GetString(buffer);
        }
        //========================================================
    }*/
}
