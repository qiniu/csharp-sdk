using System.Net;

namespace Qiniu.Http
{
    public delegate HttpResult DNextSend(HttpWebRequest req);
    
    public interface IMiddleware
    {
        HttpResult Send(HttpWebRequest req, DNextSend next);
    }

}