using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloodBankSystem.Models
{
    public class DonorImplementor : IDonor
    {
        BloodBankDbEntities db = new BloodBankDbEntities();

        public bool save(Donor donor)
        {
            db.Donors.Add(donor);
            if (db.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool remove(Donor donor)
        {
            db.Donors.Remove(db.Donors.Find(donor.donorID));
            if (db.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}