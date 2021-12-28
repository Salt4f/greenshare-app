using System;
using System.Collections.Generic;
using System.Text;

namespace greenshare_app.Models
{
    public class PendingPost
    {
        public String UserNickName { get; set; }
        public String UserId { get; set; }
        public String PostName { get; set; }
        public bool AcceptButtonsVisible { get; set; }//En caso de que sea una pending offer creada por nosotros y no esté completada será True. En los demás, False. Lo calculamos nosotros
        public int PostId { get; set; }//Id del post al que aplico
        public int OwnPostId { get; set; }//Id de mi propio post

    }
}
