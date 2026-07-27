using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Revisio.Application.Course.Common.Models
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int TotalPage { get; set; }
        public int TotalCount { get; set; }
        public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalCount = count;
            TotalPage = (int)Math.Ceiling(count / (double)pageSize);
            Items = items;
        }

        public static async Task<PaginatedList<T>> CreateAsync (IQueryable<T>source,int pageNumber,int pageSize)
        {
            var count =await source.CountAsync();
            var items = await source
                .Skip((-1 + pageNumber) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<T>(items, count, pageNumber, pageSize);

        }
    }
}
