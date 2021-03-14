using System;
//using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
//using System.Net.Http;
//using System.Text;
//using System.Text.RegularExpressions;
using System.Threading.Tasks;
//using System.Web;
using System.Web.Mvc;
using SEOAnalyzer.Common;
using SEOAnalyzer.Models;

namespace SEOAnalyzer.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index(string Sorting_Order)
        {
            ContentModel model = new ContentModel();


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ContentModel model)
        {
            CalculateFunctions CalculateFunction = new CalculateFunctions();
            string[] ListofStopWords = System.IO.File.ReadAllLines(Server.MapPath(@"~/App_Data/StopWords.txt")); //ref:https://www.ranks.nl/stopwords

            model.ValidationChecked = !(model.FilterStopWord ||
                model.CalculateNumberWordOccurence || model.CalculateNumberWordOccurenceInMetaTag ||
                model.CalculateNumberExternalLink);

            if (model.ValidationChecked)
            {
                ModelState.AddModelError(nameof(model.ValidationChecked), "Required one validation Atleast!");
            }

            model.IsValidModel = ModelState.IsValid;

            if (model.IsValidModel)
            {
                try
                {
                    ProcessUrl(model);
                    CalculateFunction.FilterStopWords(model, ListofStopWords);
                    CalculateFunction.CalculateWordOccurences(model);
                    CalculateFunction.CalculateWordOccurencesInMetaTag(model);
                    CalculateFunction.CountNumberOfExternalLinks(model);
                }
                catch (AggregateException ex)
                {
                    model.IsValidModel = false;
                    if (ex.InnerException != null && ex.InnerException.InnerException != null)
                    {
                        ModelState.AddModelError(nameof(model.HasError), ex.InnerException.InnerException.Message);
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.HasError), ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    model.IsValidModel = false;
                    if (ex.InnerException != null)
                    {
                        ModelState.AddModelError(nameof(model.HasError), ex.InnerException);
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.HasError), ex.Message);
                    }
                }
            }

            return View(model);
        }

        private void ProcessUrl(ContentModel model)
        {
            if (ValidateURL(model.Input))
            {
                model.Content = GetAsyncRequestResult(model.Input).Result;
            }
            else
            {
                model.Content = model.Input;
            }
        }

        private static Task<string> GetAsyncRequestResult(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Http.Get;
            request.Timeout = 20000;
            request.Proxy = null;


            Task<WebResponse> task = Task.Factory.FromAsync(
                    request.BeginGetResponse,
                    asyncResult => request.EndGetResponse(asyncResult),
                    (object)null);

            return task.ContinueWith(t => ReadStreamFromResponse(t.Result));
          
        }

        private static string ReadStreamFromResponse(WebResponse response)
        {
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader sr = new StreamReader(responseStream))
            {
                //Need to return this response 
                string strContent = sr.ReadToEnd();
                return strContent;
            }
        }

        private bool ValidateURL(string url)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }
    }
}