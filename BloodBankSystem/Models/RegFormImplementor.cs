using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloodBankSystem.Models
{
    public class RegFormImplementor: IRegForm
    {
        BloodBankDbEntities db = new BloodBankDbEntities();

        public bool save(RegForm form)
        {
            db.RegForms.Add(form);
            if (db.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool remove(RegForm form)
        {
            db.RegForms.Remove(db.RegForms.Find(form.formID));
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