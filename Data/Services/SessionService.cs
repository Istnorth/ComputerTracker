using System.Collections.Generic;
using System.Linq;
using ComputerTracker.Data.DbModel;
using Microsoft.EntityFrameworkCore;

namespace ComputerTracker.Data.Services
{
    public class SessionService
    {
        public List<UsageSession> GetAllSessions()
        {
            using (var context = new AppDbContext())
            {
                return context.UsageSessions
                              .Include(s => s.Employee)
                              .Include(s => s.Computer)
                              .Include(s => s.SoftwareUsages)
                                    .ThenInclude(su => su.Software)
                              .ToList();
            }
        }

        public void AddSession(UsageSession session)
        {
            using (var context = new AppDbContext())
            {
                context.UsageSessions.Add(session);
                context.SaveChanges();
            }
        }

        public void UpdateSession(UsageSession session)
        {
            using (var context = new AppDbContext())
            {
                context.UsageSessions.Update(session);
                context.SaveChanges();
            }
        }

        public void DeleteSession(int sessionID)
        {
            using (var context = new AppDbContext())
            {
                var session = context.UsageSessions.Find(sessionID);
                if (session != null)
                {
                    context.UsageSessions.Remove(session);
                    context.SaveChanges();
                }
            }
        }
    }
}
