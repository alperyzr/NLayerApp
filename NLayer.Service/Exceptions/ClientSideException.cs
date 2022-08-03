using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Exceptions
{
    public class ClientSideException:Exception
    {
        //Constraction parametresindeki mesajı Exceptin base classındaki methoda gönderiliyor
        public ClientSideException(string message) : base(message)
        {

        }
    }
}
