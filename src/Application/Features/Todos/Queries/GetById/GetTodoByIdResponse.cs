﻿using System;

namespace BlazorHero.CleanArchitecture.Application.Features.Todos.Queries.GetById
{
    public class GetTodoByIdResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int Priority { get; set; }
        public bool IsCompleteted { get; set; }
    }
}
