﻿using Kooboo.IndexedDB;
using Kooboo.Sites.Models;

namespace Kooboo.Sites.Repository
{
    public class TableRelationRepository : SiteRepositoryBase<TableRelation>
    {
        public override ObjectStoreParameters StoreParameters
        {
            get
            {
                ObjectStoreParameters para = new ObjectStoreParameters();
                para.SetPrimaryKeyField<TableRelation>(o => o.Id);
                return base.StoreParameters;
            }
        }

        public TableRelation GetRelation(string currentTableName, string key)
        {
            //Relation is in cache, can get all.
            var all = this.List();
            var matchkey = all.Find(o => o.Name == key);
            if (matchkey != null)
            {
                if (matchkey.TableA == currentTableName || matchkey.TableB == currentTableName)
                {
                    return matchkey;
                }
            }

            // as table A.
            var asTableA = all.Find(o => o.TableA == currentTableName && o.TableB == key);

            if (asTableA != null)
            {
                return asTableA;
            }

            var asTableB = all.Find(o => o.TableB == currentTableName && o.TableA == key);

            return asTableB;
        }
    }
}