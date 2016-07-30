﻿using Newtonsoft.Json.Linq;
using PizzaApp.Data.Persistence;
using PizzaApp.Data.ServerEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Data.Providers
{
    public class ShopCartProvider
    {
        public static async Task<bool> AddProduct(DBConnection dbc, int productID, int amount)
        {
            string url = "http://lakmoes-001-site1.etempurl.com/ShopCart/AddProduct";
            var token = dbc.GetToken();
            if (token == null)
                return false;
            DateTime now = DateTime.Now;
            if (token.expTime.Date == now.Date &&
                token.expTime.TimeOfDay.Hours == now.TimeOfDay.Hours &&
                token.expTime.TimeOfDay.Minutes - now.TimeOfDay.Minutes <= 5)
            {
                Token t = await AuthProvider.RenewToken(token.token_hash);
                if (t != null)
                    dbc.SaveToken(t);
            }

            var values = new Dictionary<string, string>
            {
                { "token", token.token_hash },
                { "productID", productID.ToString()},
                { "amount", amount.ToString() }
            };

            string content = await Requests.PostAsync(url, values);
            if (content != null && content.Equals("\"wrong token\""))
            {
                try
                {
                    var user = dbc.GetUser();
                    token = await AuthProvider.Login(user.username, user.password);
                    if (token != null)
                    {
                        dbc.SaveToken(token);
                        content = await Requests.PostAsync(url, values);
                    }
                }
                catch { }
            }
            if (content == null)
                return false;
            if (content.Equals("\"ok\""))
                return true;
            else
                return false;
        }

        public static async Task<List<ShopCartProduct>> Show(DBConnection dbc, string promocode)
        {
            string url = "http://lakmoes-001-site1.etempurl.com/ShopCart/Show";
            var token = dbc.GetToken();
            if (token == null)
                return null;
            DateTime now = DateTime.Now;
            if (token.expTime.Date == now.Date &&
                token.expTime.TimeOfDay.Hours == now.TimeOfDay.Hours &&
                token.expTime.TimeOfDay.Minutes - now.TimeOfDay.Minutes <= 5)
            {
                Token t = await AuthProvider.RenewToken(token.token_hash);
                if (t != null)
                    dbc.SaveToken(t);
            }

            var values = new Dictionary<string, string>
            {
                { "token", token.token_hash }
            };
            if (promocode != null)
                values.Add("promocode", promocode);

            string content = await Requests.PostAsync(url, values);
            if (content != null && content.Equals("\"wrong token\""))
            {
                try
                {
                    var user = dbc.GetUser();
                    token = await AuthProvider.Login(user.username, user.password);
                    if (token != null)
                    {
                        dbc.SaveToken(token);
                        content = await Requests.PostAsync(url, values);
                    }
                }
                catch { }
            }
            if (content == null)
                return null;
            JArray jArray;
            try
            {
                jArray = JArray.Parse(content);
            }
            catch { return null; }
            var shopCartProductList = jArray.ToObject<List<ShopCartProduct>>();

            return shopCartProductList;
        }
        public static async Task<int> ProductExists(DBConnection dbc, int productID)
        {
            string url = "http://lakmoes-001-site1.etempurl.com/ShopCart/ProductExists";
            var token = dbc.GetToken();
            if (token == null)
                return 0;
            DateTime now = DateTime.Now;
            if (token.expTime.Date == now.Date &&
                token.expTime.TimeOfDay.Hours == now.TimeOfDay.Hours &&
                token.expTime.TimeOfDay.Minutes - now.TimeOfDay.Minutes <= 5)
            {
                Token t = await AuthProvider.RenewToken(token.token_hash);
                if (t != null)
                    dbc.SaveToken(t);
            }

            var values = new Dictionary<string, string>
            {
                { "token", token.token_hash },
                { "productID", productID.ToString()}
            };

            string content = await Requests.PostAsync(url, values);
            if (content != null && content.Equals("\"wrong token\""))
            {
                try
                {
                    var user = dbc.GetUser();
                    token = await AuthProvider.Login(user.username, user.password);
                    if (token != null)
                    {
                        dbc.SaveToken(token);
                        content = await Requests.PostAsync(url, values);
                    }
                }
                catch { }
            }
            if (content == null)
                return 0;

            int count;
            if (!Int32.TryParse(content, out count))
                return 0;
            else
                return count;
        }
    }
}
