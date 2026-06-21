using MongoDB.Driver;
using NexsolCrmBackendVersion2.Dtos.HeroSectionDtos;
using NexsolCrmBackendVersion2.Helpers;
using NexsolCrmBackendVersion2.Models.HeroSection;

namespace NexsolCrmBackendVersion2.Services.HeroSectionService
{
    public class HeroSectionLogicService
    {
        private string[] months = ["Янв", "Фев", "Мар", "Апр", "Май", "Июн", "Июл", "Авг", "Сен", "Окт", "Ноя", "Дек"];
        private string[] days = ["Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс"];
        private Dictionary<string, int> dayOrder = new Dictionary<string, int> { { "Пн", 0 }, { "Вт", 1 }, { "Ср", 2 }, { "Чт", 3 }, { "Пт", 4 }, { "Сб", 5 }, { "Вс", 6 } };

        public async Task<List<Visitor>> GetAllVisitorsService(IMongoCollection<Visitor> _visitors)
        {
            return await _visitors.Find(_ => true).ToListAsync();
        }

        public async Task<List<Lead>> GetAllLeadsService(IMongoCollection<Lead> _leads)
        {
            return await _leads.Find(_ => true).ToListAsync();
        }

        public async Task<List<User>> GetAllUsersService(IMongoCollection<User> _users)
        {
            return await _users.Find(_ => true).ToListAsync();
        }

        public async Task<VisitorsStatsDto?> GetVisitorsStatsService(IMongoCollection<Visitor> _visitors)
        {
            try
            {
                DateTime now = DateTime.Now;
                DateTime today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
                DateTime weekAgo = today.AddDays(-7);
                DateTime monthAgo = today.AddDays(-30);
                IAsyncCursor<string> uniqueToday = await _visitors.DistinctAsync<string>("VisitorId", Builders<Visitor>.Filter.Gte(v => v.Timestamp, today));
                IAsyncCursor<string> uniqueWeek = await _visitors.DistinctAsync<string>("VisitorId", Builders<Visitor>.Filter.Gte(v => v.Timestamp, weekAgo));
                IAsyncCursor<string> uniqueMonth = await _visitors.DistinctAsync<string>("VisitorId", Builders<Visitor>.Filter.Gte(v => v.Timestamp, monthAgo));
                long totalToday = await _visitors.CountDocumentsAsync(Builders<Visitor>.Filter.Gte(v => v.Timestamp, today));
                long totalWeek = await _visitors.CountDocumentsAsync(Builders<Visitor>.Filter.Gte(v => v.Timestamp, weekAgo));
                long totalMonth = await _visitors.CountDocumentsAsync(Builders<Visitor>.Filter.Gte(v => v.Timestamp, monthAgo));

                VisitorsStatsDto stats = new VisitorsStatsDto
                {
                    UniqueToday = uniqueToday.ToList().Count,
                    UniqueWeek = uniqueWeek.ToList().Count,
                    UniqueMonth = uniqueMonth.ToList().Count,
                    TotalToday = (int)totalToday,
                    TotalWeek = (int)totalWeek,
                    TotalMonth = (int)totalMonth
                };

               return stats;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<object?> GetChartDataService(string period, IMongoCollection<Visitor> _visitors)
        {
            try
            {
                DateTime now = DateTime.UtcNow;
                DateTime startDate;

                switch (period)
                {
                    case "week":
                        startDate = now.AddDays(-7);
                        break;
                    case "month":
                        startDate = now.AddMonths(-1);
                        break;
                    case "year":
                        startDate = now.AddYears(-1);
                        break;
                    default:
                        startDate = now.AddDays(-7);
                        break;
                }

                List<Visitor> visitors = await _visitors.Find(v => v.Timestamp >= startDate).SortBy(v => v.Timestamp).ToListAsync();

                Dictionary<string, (int Visitors, HashSet<string> UniqueIds)> groupedData = new Dictionary<string, (int Visitors, HashSet<string> UniqueIds)>();

                foreach (Visitor visitor in visitors)
                {
                    string dateKey;
                    if (period == "week")
                    {
                        dateKey = days[(int)visitor.Timestamp.DayOfWeek == 0 ? 6 : (int)visitor.Timestamp.DayOfWeek - 1];
                    }
                    else if (period == "month")
                    {
                        var weekNum = (visitor.Timestamp.Day - 1) / 7 + 1;
                        dateKey = $"{weekNum} нед";
                    }
                    else
                        dateKey = months[visitor.Timestamp.Month - 1];

                    if (!groupedData.ContainsKey(dateKey))
                        groupedData[dateKey] = (0, new HashSet<string>());

                    (int Visitors, HashSet<string> UniqueIds) entry = groupedData[dateKey];
                    entry.Visitors++;
                    if (!string.IsNullOrEmpty(visitor.VisitorId))
                        entry.UniqueIds.Add(visitor.VisitorId);
                    groupedData[dateKey] = entry;
                }

                List<ChartDataDto> formattedData = groupedData.Select(g => new ChartDataDto
                {
                    Date = g.Key,
                    Visitors = g.Value.Visitors,
                    Unique = g.Value.UniqueIds.Count
                }).ToList();

                if (period == "week")
                    formattedData = formattedData.OrderBy(d => dayOrder.ContainsKey(d.Date) ? dayOrder[d.Date] : 999).ToList();

                return new { data = formattedData };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<VisitorDetailsDto>?> GetRecentVisitorsService(IMongoCollection<Visitor> _visitors)
        {
            try
            {
                List<Visitor> visitors = await _visitors.Find(_ => true).SortByDescending(v => v.Timestamp).Limit(10).ToListAsync();

                List<VisitorDetailsDto> formattedVisitors = visitors.Select(v => new VisitorDetailsDto
                {
                    Id = !string.IsNullOrEmpty(v.VisitorId) && v.VisitorId.Length > 8
                        ? v.VisitorId.Substring(v.VisitorId.Length - 8)
                        : v.Id,
                    Page = v.Page,
                    Source = HeroSectionHelper.GetSource(v.Referrer),
                    Device = HeroSectionHelper.GetDevice(v.UserAgent),
                    Time = v.Timestamp.ToString("dd.MM.yyyy HH:mm")
                }).ToList();

                return formattedVisitors;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
