using Microsoft.Data.SqlClient;

namespace GaleriaDeArtes.Data
{
    public static class Conexion
    {
        private static readonly string _cadenaConexion =
           "Server=BICHITO;Database=GaleriaArte;User Id=sa;Password=Fregoso03;TrustServerCertificate=True;";

        public static SqlConnection ObtenerConexion()
        {
            return new SqlConnection(_cadenaConexion);
        }

        public static bool ProbarConexion()
        {
            try
            {
                using var con = ObtenerConexion();
                con.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
