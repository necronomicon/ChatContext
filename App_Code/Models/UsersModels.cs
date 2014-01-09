﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

/// <summary>
/// Summary description for UsersModels
/// </summary>
public class UsersModels
{
    public static string COOKIE_NAME = "ism";

    public static UsersModels instance { get; set; }
    private static object syncRoot = new Object();

    private static List<Users> UserDB;

    private UsersModels()
    {
        UserDB = new List<Users>();
    }
    static UsersModels() { }



    public static UsersModels Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new UsersModels();
                }
            }

            return instance;
        }
    }

    public string ClientID()
    {
        return UserDB.GetHashCode().ToString();
    }

    public void Add(Users user)
    {
        UserDB.Add(user);
    }

    public static void AddUser(Users user)
    {
        UserDB.Add(user);

    }

    public void Remove(string ID)
    {
        var user = UserDB.FirstOrDefault(n => n.ID == ID);

        UserDB.Remove(user);
    }
    public static bool isUserExist(HttpRequestWrapper request)
    {
        if (request == null)
        {
            throw new ArgumentNullException("request");
        }
        HttpCookie cookie = request.Cookies[COOKIE_NAME];

        if (cookie == null)
        {
            return false;
        }

        byte[] bytes = Convert.FromBase64String(cookie.Value);

        string input = Encoding.UTF8.GetString(bytes);

        var user = new JavaScriptSerializer().Deserialize<Users>(input);

        return Convert.ToBoolean(UserDB.Where(a => a.ID == user.ID && a.UserName == user.UserName).Any());

    }

    
}