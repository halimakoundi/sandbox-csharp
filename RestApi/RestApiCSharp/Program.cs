using System;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

/* What is REST
    REST is an architectural style that builds on the same principles that provide the foundation of the World Wide Web. 
    Clients interact with services by making HTTP requests, and services respond with HTTP responses, 
    which often include a representation of a resource that client code can use to drive an application.
*/

namespace RestApiTuto
{
    class MainClass{
        
        static void Main(string[] args)
        {
            /*
             * This is a simple how-to tutorial to make calls to Rest Api and parse data with C#
             * Check out the Git hub repo for info and samples about the Hacker news API
             * https://github.com/HackerNews/API
             * With 2.6 million unique visitors per month, Hacker News (HN) has a huge audience of programmers and entrepreneurs.
             * HN users submit interesting stories from around the web that are upvoted by other users.
             * When a post receives enough upvotes it makes it to the front page. 
            */
            string details = GethackerNewsTopStories ();
            //Uncomment below to print the raw result from the webRequest
            //Console.WriteLine(details); 
            //You can compare results : https://news.ycombinator.com/news

            //serialisation of the resut
            /*
             * https://msdn.microsoft.com/en-us/library/ms233843.aspx
             * Here we are using the powerfull and widespread Newtonsoft JSON library
             * http://www.newtonsoft.com/json/help/html/deserializeobject.htm
            */
            int[] topStoriesIds = JsonConvert.DeserializeObject<int[]>(details);
            GethackerNewsItem(topStoriesIds);
            Console.ReadLine();
        }
         
        public static string GethackerNewsTopStories()
        {
            string url = "https://hacker-news.firebaseio.com/v0/topstories.json?print=pretty";
            return CallRestMethod(url);
        }

        public static void GethackerNewsItem(int[] topStoriesIds)
        {            
            //Take() Method is a linq extension method method that allows us to query data on collections in a similar way to SQL data queries
            foreach(var story in topStoriesIds.Take(1)){                    
                string url = string.Format("https://hacker-news.firebaseio.com/v0/item/{0}.json?print=pretty", story); 
                //Uncomment below to print the raw result from the webRequest
                Console.WriteLine(CallRestMethod(url));    
                BuildObjectFromJsonData (CallRestMethod (url));
                                
            }
        }

        public static object BuildObjectFromJsonData(string jsonData )
        {            
            HackerNewsItem item = JsonConvert.DeserializeObject<HackerNewsItem>(jsonData);
            Console.WriteLine (item);
            item.GetComments();
            return item;
        }

        public static string CallRestMethod(string url)
        {
            //https://msdn.microsoft.com/en-us/library/system.net.httpwebrequest%28v=vs.110%29.aspx
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);

            //Request method to use to contact the Internet resource. (optional) The default value is GET.
            //can be set to any of the HTTP 1.1 protocol verbs: GET, HEAD, POST, PUT, DELETE, TRACE, or OPTIONS
            webrequest.Method = "GET";
            // Set the content type of the data being posted.
            webrequest.ContentType = "application/x-www-form-urlencoded";

            /* https://msdn.microsoft.com/en-us/library/system.net.httpwebresponse%28v=vs.110%29.aspx
             * https://msdn.microsoft.com/en-gb/library/system.net.webrequest.getresponse%28v=vs.110%29.aspx
             * This is a synchronous call to the API
             * Check this link out for asynchronous call : 
             * https://msdn.microsoft.com/en-gb/library/system.net.webrequest.begingetresponse%28v=vs.110%29.aspx
            */
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();

            //We can specify which encoding format to be used (optional)
            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");

            string result = string.Empty;
            //Gets the stream that is used to read the body of the response from the server
            //The using statement defines a scope at the end of which an object will be disposed (calls the Dispose method on the object) 
            using (StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(), enc))
            {
                //This allows you to do one Read operation.
                // Releases the resources of the Stream.
                result = responseStream.ReadToEnd();   
            }
            //We must close the stream and release the connection for reuse
            //Failure to close the stream will cause your application to run out of connections
            webresponse.Close();
            return result;
        }

        public static DateTime UnixTimeStampToDateTime( double unixTimeStamp )
        {
            //We have to convert the time provided in the response to a DateTime object
            //solution foud here : http://stackoverflow.com/questions/249760/how-to-convert-a-unix-timestamp-to-datetime-and-vice-versa
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970,1,1,0,0,0,0);
            dtDateTime = dtDateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
            return dtDateTime;
        }
    }

    public class HackerNewsItem {
        
        public string by {get;set;}
        public int descendants {get;set;}
        public int id {get;set;}
        public int[] kids {get;set;}
        public decimal score {get;set;}
        public double time {get{ return this.time;}set{ this.datePosted = MainClass.UnixTimeStampToDateTime(value);}}
        public DateTime datePosted {get;set;}
        public string title {get;set;}
        public string type {get;set;}
        public string url {get;set;}

        //Getting associated comments 
        public void GetComments(){
            //We only print the 5 first comments
            foreach (var kid in this.kids.Take(5)) {
                string url = string.Format("https://hacker-news.firebaseio.com/v0/item/{0}.json?print=pretty", kid); 
                Console.WriteLine(MainClass.CallRestMethod (url));
            }
        }

        //Building the override of the toString method to print the object
        public override string ToString ()
        {
            return string.Format("[{9}HackerNewsItem: {9} by= {0},{9} descendants= {1},{9} id= {2},{9} kids= there are {3} comments,{9} score= {4},{9} datePosted= {5},{9} title= {6},{9} type= {7},{9} url= {8} {9}]", by, descendants, id, kids.Length.ToString(), score, datePosted, title, type, url, Environment.NewLine);
        }

    }
}

/*
 * Going further :
 * 
 * 1. Build a C# Object for comments and populate with data retrieved from the GetComments method of HackerNewsItems
 * 
 * 2. Query all jobs, shows or asks listed on HackerNews ( https://hacker-news.firebaseio.com/v0/<job, show, or ask>stories.json?print=pretty)
 * 
 * 3. Get information about users that published the retrieved story 
 *  
 * 4. Parse the list of stories and lookout for your favorite domain (and integrate with twilio bit.ly/1Lu4RPw)
 * 
*/