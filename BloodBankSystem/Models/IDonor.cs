using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloodBankSystem.Models
{
    public interface IDonor
    {
        bool save(Donor donor);
        bool remove(Donor donor);
    }
}