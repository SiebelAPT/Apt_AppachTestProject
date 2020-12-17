using Domain.Commands;
using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.ClientSideModels;
using Domain.DataModels;

namespace Core.Extensions.ModelConversion
{
    public static class ModelConversionExtensions
    {
        public static MenuItem[] ToMenuItems(this IEnumerable<MemberVm> models)
        {
            return models.Select(m => new MenuItem()
            {
                iconColor = m.Avatar,
                isActive = false,
                label = $"{m.LastName}, {m.FirstName}",
                referenceId = m.Id
            }).ToArray();
        }

        #region Create Public Commands

        public static CreateMemberCommand ToCreateMemberCommand(this MemberVm model)
        {
            var command = new CreateMemberCommand()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Roles = model.Roles,
                Avatar = model.Avatar,
                Email = model.Email
            };
            return command;
        }

        public static CreateTaskCommand ToCreateTaskCommand(this TaskVm model)
        {
            var command = new CreateTaskCommand()
            {
                Subject = model.Subject,
                AssignedToId = model.AssignedToId,
                IsComplete = model.IsComplete
            };
            return command;
        }

        public static UpdateMemberCommand ToUpdateMemberCommand(this MemberVm model)
        {
            var command = new UpdateMemberCommand()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Roles = model.Roles,
                Avatar = model.Avatar,
                Email = model.Email
            };
            return command;
        }

        public static UpdateTaskCommand ToUpdateTaskCommand(this TaskVm model)
        {
            var command = new UpdateTaskCommand()
            {
                Id = model.Id,
                Subject = model.Subject,
                AssignedToId = model.AssignedToId,
                IsComplete = model.IsComplete
            };
            return command;
        }

        public static AssignTaskCommand ToAssignTaskCommand(this TaskVm model)
        {
            var command = new AssignTaskCommand()
            {
                Id = model.Id,
                AssignedToId = model.AssignedToId                
            };
            return command;
        }

        public static CompleteTaskCommand ToCompleteTaskCommand(this TaskVm model)
        {
            var command = new CompleteTaskCommand()
            {
                Id = model.Id,
                IsComplete = model.IsComplete                
            };
            return command;
        }

        public static CompleteMemberTaskCommand ToCompleteMemberTaskCommand(this TaskVm model)
        {
            var command = new CompleteMemberTaskCommand()
            {
                Id = model.Id,
                IsComplete = model.IsComplete,
                AssignedToId = model.AssignedToId
            };
            return command;
        }

        #endregion
    }
}
