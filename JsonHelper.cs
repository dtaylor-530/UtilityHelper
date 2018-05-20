//using Supremes;
//using Supremes.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace WebScrape.ESPN
{
    public static class JsonHelper
    {




        public static void WalkNode(JToken node,
                                Action<JObject> objectAction = null,
                                Action<JProperty> propertyAction = null)
        {
            if (node.Type == JTokenType.Object)
            {
                if (objectAction != null) objectAction((JObject)node);

                foreach (JProperty child in node.Children<JProperty>())
                {
                    if (propertyAction != null) propertyAction(child);
                    WalkNode(child.Value, objectAction, propertyAction);
                }
            }
            else if (node.Type == JTokenType.Array)
            {
                foreach (JToken child in node.Children())
                {
                    WalkNode(child, objectAction, propertyAction);
                }
            }
        }

        public static string Traverse(JToken parsed, string s)
        {

            foreach (var pair in parsed)
            {
                s = pair.ToString();

                try
                {
                    Traverse(pair, s);
                }
                catch { }


            }

            return s;
        }


        public static void JsonTest()

        {


            string json = "{\"FullName\":\"Mr Mahesh Sharma\",\"Miles\":0,\"TierStatus\":\"IO\"," +
               "\"TierMiles\":0,\"MilesExpiry\":0,\"ExpiryDate\":\"30/03/2012 00:00:00\"," +
               "\"AccessToken\":\"106C9FD143AFA6198A9EBDC8B9CC0FB2CE867356222D21D45B16BEE" +
               "B9A7F390B5E226665851D6DB9\",\"ActiveCardNo\":\"00300452124\",\"PersonID\":8654110}";

            JObject parsed = JObject.Parse(json);

            foreach (var pair in parsed)
            {
                Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
            }
        }
    }





   


}
