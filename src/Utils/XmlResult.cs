using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Relay.Utils
{
    public class XmlResult<T> : ActionResult
    {
        private T Data { get; set; }
        
        public static implicit operator XmlResult<T>(T data)
        {
            return new XmlResult<T>()
            {
                Data = data
            };
        }

        public override void ExecuteResult(ActionContext context)
        {
            using (var body = context.HttpContext.Response.Body)
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(body, Data, new XmlSerializerNamespaces());
            }
        }
    }
}