using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloodBankSystem.Models
{
    public interface IRequest
    {
        bool save();
    }
}