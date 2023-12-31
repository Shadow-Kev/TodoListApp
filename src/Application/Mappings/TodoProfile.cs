﻿using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Features.Todos.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Todos.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.Todos.Queries.GetById;
using BlazorHero.CleanArchitecture.Domain.Entities.TodoListApp;

namespace BlazorHero.CleanArchitecture.Application.Mappings
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            CreateMap<GetAllTodosResponse, Todo>().ReverseMap();
            CreateMap<AddEditTodoCommand, Todo>().ReverseMap();
            CreateMap<GetTodoByIdResponse, Todo>().ReverseMap();
        }
    }
}
