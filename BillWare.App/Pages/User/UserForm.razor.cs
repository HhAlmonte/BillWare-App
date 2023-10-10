using BillWare.App.Enum;
using BillWare.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace BillWare.App.Pages.User
{
    [Authorize("Administrator")]
    public partial class UserForm
    {
        [Parameter] public UserModel UserParameter { get; set; } = new UserModel();
        [Parameter] public FormModeEnum FormMode { get; set; } = FormModeEnum.ADD;

        public bool ShowPasswordInput { get; set; } = true;

        public RegistrationModel Registration { get; set; } = new RegistrationModel();

        public List<Role> Roles { get; set; } = new List<Role>
    {
        new Role { RoleValue = "Administrator" },
        new Role { RoleValue = "Operator" }
    };

        private async Task OnSubmit()
        {
            if (FormMode == FormModeEnum.ADD)
            {
                await Add();
            }
            else
            {
                await Edit();
            }
        }

        public async Task Add()
        {
            var response = await _authService.RegisterAsync(Registration);

            if (!response.IsSuccessStatusCode)
            {
                return;
            }

            var closeReturn = response != null ? true : false;
            DialogService.Close(closeReturn);
        }

        public async Task Edit()
        {
            var userToUpdate = new UserModel
            {
                Id = UserParameter.Id,
                FirstName = Registration.FirstName,
                LastName = Registration.LastName,
                Email = Registration.Email,
                NumberId = Registration.NumberId,
                Address = Registration.Address,
                UserName = Registration.UserName,
                Role = Registration.Role
            };

            var response = await _userService.UpdateAsync(userToUpdate);

            if (!response.IsSuccessFull)
            {
                await SweetAlertServices.ShowErrorAlert(response.Message, response.Details!);
                return;
            }

            var closeReturn = response != null ? true : false;
            DialogService.Close(closeReturn);
        }

        protected override void OnInitialized()
        {
            if (FormMode == FormModeEnum.EDIT)
            {
                ShowPasswordInput = false;

                Registration = new RegistrationModel
                {
                    FirstName = UserParameter.FirstName,
                    LastName = UserParameter.LastName,
                    Email = UserParameter.Email,
                    NumberId = UserParameter.NumberId,
                    Address = UserParameter.Address,
                    UserName = UserParameter.UserName,
                    Role = UserParameter.Role
                };
            }
        }
    }

    public class Role
    {
        public string RoleValue { get; set; } = string.Empty;
    }
}
