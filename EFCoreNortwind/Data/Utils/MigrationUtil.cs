using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreEFApp.Database.Utils
{
    public static class MigrationUtil
    {
        public static string ReadSql(Type migrationType, string sqlFileName)
        {
            var assembly = migrationType.Assembly;
            // ilgili katmanın hangisi olduğunu bilmem lazım onun için kullanıldı. DAC, Persistance Layer
            string resourceName = $"{migrationType.Namespace}.{sqlFileName}";
            // ilgili katmandaki dosyayı oku demek.
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException("Unable to find the SQL file from an embedded resource", resourceName);
                }

                using (var reader = new StreamReader(stream))
                {
                    string content = reader.ReadToEnd();
                    return content;
                }
            }
        }
    }
}
