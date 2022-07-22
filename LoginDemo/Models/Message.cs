using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginDemo.Models
{
    public class Message
    {
        //错误状态标识
        public static string STATUS_ERROR = "0";

        //正确状态标识
        public static string STATUS_SUCCESS = "1";
        private string status;//状态，成果或失败
        private string reply;//返回的提示消息

        public string Status { get => status; set => status = value; }
        public string Reply { get => reply; set => reply = value; }

    }
}
