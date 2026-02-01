using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewApi.Data
{
    public class SeniorDbContext : IdentityDbContext
    {
        public SeniorDbContext(DbContextOptions<SeniorDbContext> options) : base(options)
        {

        }
    }
}
