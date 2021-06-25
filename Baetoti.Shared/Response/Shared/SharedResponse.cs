using Newtonsoft.Json;

namespace Baetoti.Shared.Response.Shared
{
    public class SharedResponse
    {
        public bool IsSuccess { get; set; }

        public int Status { get; set; }

        public string Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Record { get; set; }

        public string Path { get; set; }

        public string FileName { get; set; }

        public string PathwithFileName { get; set; }

        public SharedResponse()
        {

        }

        public SharedResponse(bool isSuccess, int status = 200, string msg = "")
        {
            IsSuccess = isSuccess;
            Status = status;
            Message = msg;
        }

        public SharedResponse(bool isSuccess, int status = 200, string msg = "", object data = null)
        {
            IsSuccess = isSuccess;
            Status = status;
            Message = msg;
            Record = data;
        }

        public SharedResponse(bool isSuccess,int status=200,string msg = "", string path = "", string fileName = "", string pathFilename = "")
        {
            IsSuccess = isSuccess;
            Status = status;
            Message = msg;
            Path = path;
            FileName = fileName;
            PathwithFileName = pathFilename;
        }
    }

}
