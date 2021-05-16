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

            // Data initial generation can be here
        }
    }
}
