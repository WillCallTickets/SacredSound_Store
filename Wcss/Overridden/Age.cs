using System;
using System.Xml.Serialization;

using System.IO;

namespace Wcss
{
    public partial class Age
    {        
    }

    public partial class AgeCollection
    {
        public bool DeleteAgeFromCollection(int idx)
        {
            Age entity = (Age)this.Find(idx);

            if (entity != null)
            {
                try
                {
                    Age.Delete(idx);

                    this.Remove(entity);
                    return true;
                }
                catch (Exception e)
                {
                    _Error.LogException(e);
                    throw e;
                }
            }

            return false;
        }

        public Age AddItemToCollection(string name)
        {
            Age newItem = new Age();
            newItem.ApplicationId = _Config.APPLICATION_ID;
            newItem.DtStamp = DateTime.Now;
            newItem.Name = name.Trim();

            try
            {
                newItem.Save();
                this.Add(newItem);
                return newItem;
            }
            catch (Exception e)
            {
                _Error.LogException(e);
                throw e;
            }
        }
    }
}
