using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MVC.Models
{
    public class GetAllFiles
    {
        public static async Task<List<userFilesCollection>> GetAllFilesByUserId(int? userId)
        {
            List<userFilesCollection> files = new List<userFilesCollection>();
            using (DataquadEntities db = new DataquadEntities())
            {
                files = await db.userFilesCollections.Where(x => x.UserId == userId).ToListAsync();
            }
            return files;
        }
    }
    
}