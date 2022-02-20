using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;

using EXIONTEST.ENTITIES.Models;

namespace EXIONTEST.BUSINESS.Common
{
    public class BaseExecution<TEntity, UEntity> where TEntity : class where UEntity : class, new()
    {
        #region "Variables..."
        public delegate BaseItem DExcecute(string strprocedure, BaseItem request);
        #endregion

        public TEntity LocalExcecute(DExcecute del, string strprocedure, UEntity obj)
        {
            try
            {
                BaseItem request = CreateRequest(obj);

                BaseItem response = del.Invoke(strprocedure, request);

                TEntity objResult = CreateEntity(response.Response);

                return objResult;
            }
            catch
            { throw; }
        }

        public TEntity LocalExcecute(DExcecute del, string strprocedure, List<UEntity> lst)
        {
            try
            {
                BaseItem request = CreateRequest(lst);

                BaseItem response = del.Invoke(strprocedure, request);

                TEntity objResult = CreateEntity(response.Response);

                return objResult;
            }
            catch
            { throw; }
        }

        #region "Private Methods..."
        BaseItem CreateRequest(UEntity obj)
        {
            BaseItem objResult = new BaseItem();
            objResult.Request = JsonConvert.SerializeObject(obj);

            return objResult;
        }

        BaseItem CreateRequest(List<UEntity> lst)
        {
            BaseItem objResult = new BaseItem();
            objResult.Request = JsonConvert.SerializeObject(lst);

            return objResult;
        }

        TEntity CreateEntity(string response)
        {
            TEntity objResult = JsonConvert.DeserializeObject<TEntity>(response);

            return objResult;
        }
        #endregion
    }
}