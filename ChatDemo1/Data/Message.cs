using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatDemo1.Data
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Send_Time { get; set; }
        public bool IsReaded { get; set; }
        public string Key { get; set; }
        //public int chatId { get; set; }
        //[ForeignKey("chatId")]
        //public Chat chat { get; set; }
        public string  SenderId { get; set; }
        public string receiverId { get; set; }
    }
}