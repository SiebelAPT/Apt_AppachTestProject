using AutoMapper;
using Domain.Commands;
using Domain.DataModels;
using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.AutoMapper
{
    public class TaskTypes : Profile
    {
        public TaskTypes()
        {
            CreateMap<CreateTaskCommand, TaskDm>();
            CreateMap<UpdateTaskCommand, TaskDm>();
            CreateMap<TaskDm, TaskVm>();
        }
    }
}
