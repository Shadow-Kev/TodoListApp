using BlazorHero.CleanArchitecture.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Domain.Entities.TodoListApp
{
    public class Todo : AuditableEntity<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int Priority { get; set; }
        public bool IsCompleteted { get; set; }
    }
}
