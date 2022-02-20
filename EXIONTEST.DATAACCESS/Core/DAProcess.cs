using Dapper;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace EXIONTEST.DATAACCESS.Core
{
    public class DAProcess<TEntity, UEntity> : DADefinition<TEntity, UEntity> where TEntity : class where UEntity : class
    {
        public override IDbConnection creaconexion()
        {
            try
            {
                string strconnectionstring = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
                SqlConnection dbconn = new SqlConnection(strconnectionstring);
                //TODO: Descomentar solamente para verificar el tiempo de respuesta de la conexión a base de datos.
                //Stopwatch sw = new Stopwatch();
                //sw.Start();

                dbconn.Open();

                //sw.Stop();
                //TimeSpan ts = sw.Elapsed;
                //string str = string.Format("{0:N5} minutes - {1,5} seconds - {2,5} miliseconds", ts.TotalMinutes, ts.Seconds, ts.Milliseconds);

                return dbconn;
            }
            catch
            { throw; }
        }

        public override TEntity ObtieneItem(string strprocedure)
        {
            try
            {
                TEntity objEntidad;
                using (IDbConnection conn = creaconexion())
                {
                    DynamicParameters dp = DefineParametros(null);
                    objEntidad = conn.QueryFirst<TEntity>(strprocedure, dp, commandType: CommandType.StoredProcedure);
                }

                return objEntidad;
            }
            catch
            { throw; }
        }

        public override TEntity ObtieneItem(string strprocedure, UEntity obj)
        {
            try
            {
                TEntity objEntidad;
                using (IDbConnection conn = creaconexion())
                {
                    DynamicParameters dp = DefineParametros(obj);
                    objEntidad = conn.QueryFirst<TEntity>(strprocedure, dp, commandType: CommandType.StoredProcedure);
                }

                return objEntidad;
            }
            catch
            { throw; }
        }

        public override IEnumerable ObtieneLista(string strprocedure)
        {
            try
            {
                IEnumerable lst;
                using (IDbConnection conn = creaconexion())
                {
                    DynamicParameters dp = DefineParametros(null);
                    lst = conn.Query<TEntity>(strprocedure, dp, commandType: CommandType.StoredProcedure);
                }

                return lst;
            }
            catch
            { throw; }
        }

        public override IEnumerable ObtieneLista(string strprocedure, UEntity obj)
        {
            try
            {
                IEnumerable lst;
                using (IDbConnection conn = creaconexion())
                {
                    DynamicParameters dp = DefineParametros(obj);
                    lst = conn.Query<TEntity>(strprocedure, dp, commandType: CommandType.StoredProcedure);
                }

                return lst;
            }
            catch
            { throw; }
        }

        public override bool EjecutaProceso(string strprocedure, UEntity obj)
        {
            try
            {
                int ireturn = 0;
                using (IDbConnection conn = creaconexion())
                {
                    DynamicParameters dp = DefineParametros(obj);
                    ireturn = conn.ExecuteScalar<int>(strprocedure, dp, commandType: CommandType.StoredProcedure);
                }

                return !ireturn.Equals(0);
            }
            catch
            { throw; }
        }

        DynamicParameters DefineParametros(UEntity obj)
        {
            DynamicParameters lst = new DynamicParameters();
            if (obj != null)
            {
                List<PropertyInfo> lstProperty = obj.GetType().GetProperties().Where(w => w.GetValue(obj, null) != null).ToList();
                foreach (PropertyInfo item in lstProperty)
                {
                    var value = item.GetValue(obj, null);
                    lst.Add(name: item.Name, value: value);
                }
            }

            return lst;
        }
    }
}