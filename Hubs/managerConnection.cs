using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ado.Hubs
{
    public class managerConnection
    {
        
        private static readonly ConcurrentDictionary<string, string> UserConnections = new ConcurrentDictionary<string, string>();

        public static void AddConnection(string userId, string connectionId)
        {
           
                UserConnections.TryAdd(userId, connectionId);
            
        }

        public static void RemoveConnection(string userId)
        {
           
                UserConnections.TryRemove(userId, out _);
            
        }

        public static bool IsUserConnected(string userId)
        {
          
                UserConnections.TryGetValue(userId, out string connectionId);
                if (connectionId != null)
                {
                    return true;
                }
                return false;
            
        }
        public static string GetConnectionId(string userId)
        {
           
                UserConnections.TryGetValue(userId, out string connectionId);
                return connectionId;
            
        }
    }
}