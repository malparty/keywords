using KeywordsApp.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace KeywordsApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(KeywordContext context)
        {
            // Create Db if not exists, perform migrations
            context.Database.Migrate();

            // Look for any keyword.
            if (context.Keywords.Any())
            {
                return;   // DB has been seeded
            }

            var keywords = new Keyword[]
            {
            new Keyword{Name="Carson"},
            new Keyword{Name="Ville"},
            new Keyword{Name="Jambon"},
            new Keyword{Name="Xuận-đĩ-đưởng"},
            new Keyword{Name="123"},
            new Keyword{Name="IamALongKeyWordForTestingPurposeAndMore"},
            };
            foreach (Keyword k in keywords)
            {
                context.Keywords.Add(k);
            }
            context.SaveChanges();
        }
    }
}