using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Irony.Parsing;

namespace ado.Hubs
{
    [HubName("Chat")]
    public class signalrChat : Hub
    {
       

        public void sendMessage(string fromUser,string toUserId, string message)
        {
            string ID=DocUsers(toUserId).ToString();
            string IDGui=DocUsers(fromUser).ToString();
            if (managerConnection.IsUserConnected(toUserId) ==true)
            {
                
                Clients.Client(ID).receiveMessage(fromUser, message);
            }
            else
            {
                            // Người dùng không kết nối, xử lý theo nhu cầu của bạn
            }

        }
        public void testChat(string message)
        {
            Clients.All.tinTest(message);
        }
        public void trangThai(string fromUser,string toUser)
        {
            string ID = DocUsers(toUser);
            string IDGui = DocUsers(fromUser).ToString();
            if (managerConnection.IsUserConnected(ID) == true)
            {
                Clients.Client(IDGui).thongBao("Đang hoạt động");
            }
            else
            {
                Clients.Client(IDGui).thongBao("Người này hiện tại không họat động");
            }
        }
        public void taoketNoi(string userID)
        {
            if (managerConnection.IsUserConnected(userID) == false)
            {
                managerConnection.AddConnection(userID.ToString(), Context.ConnectionId);
            }
            else
            {
                string ID = DocUsers(userID).ToString();
                Clients.Client(ID).canhBao("Có người đang đăng nhập");
                
            }
          
        }
        public string DocUsers(string userId)
        {
            
            var check = managerConnection.GetConnectionId(userId);
            if (check == null)
            {
                return "";
            }
            return check;
        }
    }
}