using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
namespace AlmonifExchange
{
public static class DatabaseHelper
{
private static string _connectionString = null;
private static string _dbName = "2024";       // الاسم الافتراضي
private static string _server = ".";          // السيرفر المحلي الافتراضي
private static int _currentUserId = -1;
private static string _currentUserName = "";
private static string _currentAgentName = "";
private static bool _isLoggedIn = false;
private static bool _settingsLoaded = false;

    public static bool IsLoggedIn { get { return _isLoggedIn; } }
     public static int CurrentUserId { get { return _currentUserId; } }
     public static string CurrentUserName { get { return _currentUserName; } }
     public static string CurrentAgentName { get { return _currentAgentName; } }
     public static void SetCurrentUser(int userId, string userName, string agentName)
     {
         _currentUserId = userId;
         _currentUserName = userName;
         _currentAgentName = agentName;
         _isLoggedIn = true;
     }
     public static void ClearCurrentUser()
     {
         _currentUserId = -1;
         _currentUserName = "";
         _currentAgentName = "";
         _isLoggedIn = false;
     }
     private static string CleanValue(string temp)
     {
         temp = temp.Trim();
         int semi = temp.IndexOf(';');
         if (semi >= 0) temp = temp.Substring(0, semi);
         return temp.Trim();
     }
     private static void LoadSettings()
     {
         if (_settingsLoaded) return;
         _settingsLoaded = true;
         try
         {
             string agentIdPath = Path.Combine(Application.StartupPath, "AgentID.txt");
             if (File.Exists(agentIdPath))
             {
                 string[] lines = File.ReadAllLines(agentIdPath);
                 foreach (string line in lines)
                 {
                     if (line.IndexOf("DatabaseName", StringComparison.OrdinalIgnoreCase) >= 0)
                     {
                         int eq = line.IndexOf('=');
                         if (eq >= 0)
                         {
                             string v = CleanValue(line.Substring(eq + 1));
                             if (v.Length > 0) _dbName = v;
                         }
                     }
                 }
             }
         }
         catch { }
     }
     public static string GetConnectionString()
     {
         if (_connectionString != null) return _connectionString;
         LoadSettings();
         // ✅ سلسلة الاتصال المتوافقة مع SQL Server المحلي (Default Instance)
         // مطابقة لسكريبت PowerShell الخاص بك
         _connectionString = string.Format(
             "Server={0};Database={1};Integrated Security=True;Encrypt=False;",
             _server, _dbName);
         return _connectionString;
     }
     public static void LoadApplicationIcon(Form form)
     {
         try
         {
             string appIcon = Path.Combine(Application.StartupPath, "ايقونة النظام.ico");
             if (File.Exists(appIcon))
             {
                 form.Icon = new Icon(appIcon);
                 return;
             }
             form.Icon = SystemIcons.Application;
         }
         catch { form.Icon = SystemIcons.Application; }
     }
 }
}