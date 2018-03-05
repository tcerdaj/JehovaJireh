using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using JehovaJireh.Web.UI.Models;
using System.Web.Helpers;

namespace JehovaJireh.Web.UI.Hubs
{
    public class ChatHub : Hub
    {
        static List<UserConnection> SignalRUsers = new List<UserConnection>();
        const string roomName = "chat";
        static string userObject;
        static dynamic user;
        public void Hello()
        {
            Clients.All.hello();
        }

       
        public void Send(string json, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            //user = Json.Decode(json);
            //user.Members = SignalRUsers.ToList();
            Clients.Group(roomName).addNewMessageToPage(json, message);
        }

        public void SendChatMessage(string who, string jsonObject, string message)
        {
            Clients.Group(who).addChatMessage(who, jsonObject, message);
        }

        public void WhoIsTyping(string who, string message)
        {
            Clients.Group(who).WhoIsTypingMessage(message);
        }

        public override Task OnConnected()
        {
            userObject = Context.QueryString["userobject"];
            user = Json.Decode(userObject);
            user.ConnectionId = Context.ConnectionId;
            userObject = Json.Encode(user);

            if (!string.IsNullOrEmpty(userObject))
            {
                JoinRoom(roomName);
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            LeaveRoom(roomName);
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            userObject = Context.QueryString["userobject"];
            user = Json.Decode(userObject);
            user.ConnectionId = Context.ConnectionId;
            userObject = Json.Encode(user);

            if (!string.IsNullOrEmpty(userObject))
            {
                JoinRoom(roomName);
            }
            return base.OnReconnected();
        }

        protected override void Dispose(bool disposing)
        {

        }

        public void JoinRoom(string roomName)
        {
            user = Json.Decode(userObject);
            var memberName = GetProperty(user, "Name");
            var userName = GetProperty(user, "UserName");
            Groups.Add(Context.ConnectionId, userName);
            Groups.Add(Context.ConnectionId, roomName);

            var count = SignalRUsers.RemoveAll(x => GetProperty(Json.Decode(x.UserObject), "UserName") == userName);
            var userAlreadyConnected = SignalRUsers.Any(x =>x.ConnectionId == Context.ConnectionId);

            if (!userAlreadyConnected)
                SignalRUsers.Add(new UserConnection { ConnectionId = Context.ConnectionId, UserObject = userObject });

            string msg = "has entered the room.";
            Clients.Group(roomName).addNewMessageToPage(userObject, msg);
            Clients.Group(roomName).messageReceived(userName, msg);
        }

        public void LeaveRoom(string roomName)
        {
            var userLeave = Json.Decode(Context.QueryString["userobject"]);
            var userName  = GetProperty(userLeave, "UserName");

            foreach (var user in SignalRUsers)
            {
                if (GetProperty(Json.Decode(user.UserObject), "UserName") == userName)
                {
                    Groups.Remove(user.ConnectionId, userName);
                    Groups.Remove(Context.ConnectionId, roomName);
                }
            }
            
            var count = SignalRUsers.RemoveAll(x =>GetProperty(Json.Decode(x.UserObject), "UserName") == userName);

            string msg = "has left the room.";
            Clients.Group(roomName).addNewMessageToPage(userObject, msg);
            Clients.Group(roomName).messageReceived(userName, msg);
        }

        public List<UserConnection> GetAllActiveConnections()
        {
            return SignalRUsers.ToList();
        }

        public static object GetProperty(object target, string name)
        {
            var site = System.Runtime.CompilerServices.CallSite<Func<System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(0, name, target.GetType(), new[] { Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(0, null) }));
            return site.Target(site, target);
        }
    }
}