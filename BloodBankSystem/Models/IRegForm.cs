using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloodBankSystem.Models
{
    public interface IRegForm
    {
        bool save(RegForm form);
        bool remove(RegForm form);
    }
}