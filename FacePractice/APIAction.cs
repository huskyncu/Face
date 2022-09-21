using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Documents;

namespace FacePractice
{
    class APIAction
    {
        private const string Subscription_Key = "d96057bda40c4af5ac740c047647172f";
        private const string Group_ID = "face_practice";
        private const string regModle = "recognition_03";
        private static HttpClient client;
        private static NameValueCollection queryString;
        private static bool isSuccessed = false;


        public bool IsSuccessed { get => isSuccessed; }

        public APIAction()
        {
            client = new HttpClient();
            queryString = HttpUtility.ParseQueryString(string.Empty);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Subscription_Key);
            isSuccessed = false;
        }

        public static async void CreatePersonGroup()
        {
            isSuccessed = false;
            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Subscription_Key);
            var uri = "https://eastasia.api.cognitive.microsoft.com/face/v1.0/persongroups/" + Group_ID + "?" + queryString;

            HttpResponseMessage response;

            var jobj = new
            {
                name = "IU",
                recognitionModel = regModle
            };
            Console.WriteLine("jsonData : " + jobj.ToString());
            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jobj));
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PutAsync(uri, content);
                Console.WriteLine("CreatePersonGroup response : " + await response.Content.ReadAsStringAsync());
                isSuccessed = true;
            }
        }

        public async Task<bool> CreatePerson(string inputname)
        {
            isSuccessed = false;
            var uri = "https://eastasia.api.cognitive.microsoft.com/face/v1.0/persongroups/" + Group_ID + "/persons?" + queryString;
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Subscription_Key);
            HttpResponseMessage response;

            // Request body
            var jobj = new { name = inputname };
            Console.WriteLine("jsonData : " + jobj.ToString());
            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jobj));

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);
                Console.WriteLine("CreatePerson response : " + await response.Content.ReadAsStringAsync());
                isSuccessed = true;
            }
            return isSuccessed;
        }
        public async Task<bool> AddFace(string personId, string ImgUrl)
        {
            isSuccessed = false;
            // Request parameters
            string targetFaceStr = await GetTargetFaceRange(ImgUrl);
            if (!targetFaceStr.Equals(""))
            {
                queryString["userData"] = ImgUrl;
                queryString["targetFace"] = targetFaceStr;
                queryString["detectionModel"] = "detection_01";
                var uri = "https://eastasia.api.cognitive.microsoft.com/face/v1.0/persongroups/" + Group_ID + "/persons/" + personId + "/persistedFaces?" + queryString;

                HttpResponseMessage response;

                // Request body
                var jobj = new { url = ImgUrl };
                byte[] byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jobj));

                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = await client.PostAsync(uri, content);
                    Console.WriteLine("AddFace response : " + await response.Content.ReadAsStringAsync());

                    isSuccessed = await Train(client);
                }
            }
            Console.WriteLine("AddFace isSuccessed : " + isSuccessed);
            return isSuccessed;
        }



        public async Task<List<PersonObj>> IdentifyFace(string ImgUrl)
        {
            var uri = "https://eastasia.api.cognitive.microsoft.com/face/v1.0/identify?" + queryString;
            string targetFaceStr = await GetFaceID(ImgUrl);
            string[] faceIdsArr = targetFaceStr.Split(',');
            var jobj = new
            {
                personGroupId = Group_ID,
                faceIds = from id in faceIdsArr select id,
                maxNumOfCandidatesReturned = 3,
                confidenceThreshold = 0.8
            };
            HttpResponseMessage response;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jobj));
            List<PersonObj> resultList = new List<PersonObj>();
            string result = "";
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);
                result = await response.Content.ReadAsStringAsync();
                result = result.Substring(1, result.Length - 2);
                if (!result.Equals(""))
                {
                    Console.WriteLine("IdentifyFace response : " + result);
                    JObject take = JObject.Parse(result);
                    JArray jsonArray = JArray.Parse(take.GetValue("candidates").ToString());
                    PersonObj personObj;
                    foreach (JToken item in jsonArray)
                    {
                        string personId = item.Value<string>("personId");
                        string nameAndFaceIds = await GetPersonByID(personId, true);
                        string confidence = item.Value<string>("confidence");
                        string[] genderAndAge = (await GetFaceGenderAndAge(ImgUrl)).Split(',');
                        string gender = genderAndAge[0];
                        string age = genderAndAge[1];
                        personObj = new PersonObj(personId, gender, age, PersonObj.formatConfidence(confidence));
                        personObj.SetNameAndFaceIds(nameAndFaceIds);
                        resultList.Add(personObj);
                    }
                }
            }
            return resultList;
        }

        public async Task<string> GetPersonByID(string personId, bool includePersistedFaceIds)
        {
            string resultStr = "";
            var uri = "https://eastasia.api.cognitive.microsoft.com/face/v1.0/persongroups/" + Group_ID + "/persons/" + personId + "?" + queryString;
            var response = await client.GetAsync(uri);
            string result = await response.Content.ReadAsStringAsync();
            if (!result.Equals(""))
            {
                Console.WriteLine("GetPersonByID response : " + result);
                JObject take = JObject.Parse(result);
                resultStr = take.Value<string>("name");
                if (includePersistedFaceIds)
                {
                    StringBuilder sb = new StringBuilder();
                    JArray jsonArray = JArray.Parse(take.GetValue("persistedFaceIds").ToString());
                    foreach (JToken item in jsonArray)
                    {
                        sb.Append(item.ToString() + ",");
                    }
                    resultStr += "," + sb.ToString();
                }
            }
            return resultStr;
        }

        public async Task<string> GetTargetFaceRange(string ImgUrl)
        {
            return await DetectFace(ImgUrl, 0);
        }

        public async Task<string> GetFaceID(string ImgUrl)
        {
            return await DetectFace(ImgUrl, 1);
        }
        public async Task<string> GetFaceGenderAndAge(string ImgUrl)
        {
            return await DetectFace(ImgUrl, 2);
        }

        private async Task<string> DetectFace(string ImgUrl, int targerInfo)
        {
            var jobj = new { url = ImgUrl };
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Subscription_Key);
            // Request parameters
            queryString["returnFaceId"] = "true";
            queryString["returnFaceLandmarks"] = "false";
            queryString["returnFaceAttributes"] = "age,gender";
            queryString["recognitionModel"] = regModle;
            queryString["returnRecognitionModel"] = "false";
            queryString["detectionModel"] = "detection_01";
            var uri = "https://eastasia.api.cognitive.microsoft.com/face/v1.0/detect?" + queryString;

            HttpResponseMessage response;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jobj));
            string resultStr = "";
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);
                string result = await response.Content.ReadAsStringAsync();
                result = result.Substring(1, result.Length - 2);
                if (!result.Equals(""))
                {
                    Console.WriteLine("DecteFace response : " + result);

                    JObject take = JObject.Parse(result);

                    switch (targerInfo)
                    {
                        case 0://targetFace
                            JToken JTfaceRectangle = take.GetValue("faceRectangle");
                            resultStr = JTfaceRectangle.Value<string>("left") + "," + JTfaceRectangle.Value<string>("top") + "," + JTfaceRectangle.Value<string>("width") + "," + JTfaceRectangle.Value<string>("height");
                            break;
                        case 1://faceID
                            resultStr = take.GetValue("faceId").Value<string>();
                            break;
                        case 2://Gender & Age
                            JToken JFaceAttributes = take.GetValue("faceAttributes");
                            resultStr = JFaceAttributes.Value<string>("gender") + "," + JFaceAttributes.Value<string>("age");
                            break;
                    }

                }
            }
            Console.WriteLine("DecteFace resultStr : " + resultStr);
            return resultStr;
        }



        public async Task<List<PersonObj>> ListAllPersons()
        {
            List<PersonObj> resultList = new List<PersonObj>();
            var uri = "https://eastasia.api.cognitive.microsoft.com/face/v1.0/persongroups/" + Group_ID + "/persons?";
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Subscription_Key);

            var response = await client.GetAsync(uri);
            string result = await response.Content.ReadAsStringAsync();
            if (!result.Equals(""))
            {
                Console.WriteLine("ListAllPersons response : " + result);
                JArray jArray = JArray.Parse(result);
                foreach (JToken item in jArray)
                {
                    string name = item.Value<string>("name");
                    string id = item.Value<string>("personId");
                    JArray faceids = item.Value<JArray>("persistedFaceIds");
                    List<string> faceList = new List<string>();
                    foreach (JToken faceId in faceids)
                    {
                        faceList.Add(faceId.ToString());
                    }
                    resultList.Add(new PersonObj(id, name, faceList.ToArray()));
                }
            }
            return resultList;
        }




        public async Task<bool> DeletePerson(string personid)
        {
            isSuccessed = false;
            var uri = "https://eastasia.api.cognitive.microsoft.com/face/v1.0/persongroups/" + Group_ID + "/persons/" + personid + "?" + queryString;
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Subscription_Key);
            var response = await client.DeleteAsync(uri);
            Console.WriteLine("DeletePerson response : " + await response.Content.ReadAsStringAsync());
            isSuccessed = true;
            return isSuccessed;
        }


        public async Task<string> GetFaceUrlByID(string personid, string inputPersistedFaceId)
        {
            var uri = "https://eastasia.api.cognitive.microsoft.com/face/v1.0/persongroups/" + Group_ID + "/persons/" + personid + "/persistedFaces/" + inputPersistedFaceId + "?" + queryString;
            var response = await client.GetAsync(uri);
            string result = await response.Content.ReadAsStringAsync();
            if (!result.Equals(""))
            {
                Console.WriteLine("GetFaceByID response : " + await response.Content.ReadAsStringAsync());
                JObject take = JObject.Parse(result);
                result = take.GetValue("userData").ToString();
            }
            return result;
        }

        public async Task<bool> DeletePersonsFace(string personid, string inputPersistedFaceId)
        {
            isSuccessed = false;
            var uri = "https://eastasia.api.cognitive.microsoft.com/face/v1.0/persongroups/" + Group_ID + "/persons/" + personid + "/persistedFaces/" + inputPersistedFaceId + "?" + queryString;
            var response = await client.DeleteAsync(uri);
            Console.WriteLine("DeletePersonsFace response : " + await response.Content.ReadAsStringAsync());
            isSuccessed = true;
            return isSuccessed;
        }

        public async void GetPersonGroupStatus()
        {
            // Request parameters
            queryString["returnRecognitionModel"] = "true";
            var uri = "https://eastasia.api.cognitive.microsoft.com/face/v1.0/persongroups/" + Group_ID + "?" + queryString;
            var response = await client.GetAsync(uri);
            Console.WriteLine("GetPersonGroupStatus response : " + await response.Content.ReadAsStringAsync());
        }
        public static async Task<bool> Train(HttpClient inputClient)
        {
            isSuccessed = false;
            var uri = "https://eastasia.api.cognitive.microsoft.com/face/v1.0/persongroups/" + Group_ID + "/train?" + queryString;

            HttpResponseMessage response;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes("");

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await inputClient.PostAsync(uri, content);
                string result = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Train response : " + result);
                if (result.Equals(""))
                {
                    isSuccessed = true;
                }
            }
            return isSuccessed;
        }
    }
}