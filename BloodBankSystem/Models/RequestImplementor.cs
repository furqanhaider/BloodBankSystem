using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloodBankSystem.Models
{
    public class RequestImplementor: IRequest
    {
        public bool save()
        {
            return true;
        }
    }
}