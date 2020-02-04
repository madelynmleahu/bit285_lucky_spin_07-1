using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuckySpin.Models
{
    public class LuckySpinContext  : DbContext
    {
        public DbSet<Player> players { get; set; }
        public DbSet<Spin> spins { get; set; }
    }
}
