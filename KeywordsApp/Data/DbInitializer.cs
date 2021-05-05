using KeywordsApp.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using KeywordsApp.Models.Keyword;

namespace KeywordsApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(KeywordContext context)
        {
            // Create Db if not exists, perform migrations
            context.Database.Migrate();

            // Can be used if some data need to be generated
            // // Look for any keyword.
            // if (context.Keywords.Any())
            // {
            //     return;   // DB has been seeded
            // }

            // var keywords = new KeywordEntity[]
            // {
            // new KeywordEntity{Name="Carson"},
            // new KeywordEntity{Name="Ville"},
            // new KeywordEntity{Name="Jambon"},
            // new KeywordEntity{Name="Xuận-đĩ-đưởng"},
            // new KeywordEntity{Name="123"},
            // new KeywordEntity{Name="IamALongKeyWordForTestingPurposeAndMore"},
            // };
            // foreach (KeywordEntity k in keywords)
            // {
            //     context.Keywords.Add(k);
            // }
            // context.SaveChanges();
        }
    }
}