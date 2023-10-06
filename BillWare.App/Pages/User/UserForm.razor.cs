using BillWare.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace BillWare.App.Pages.User
{
    [Authorize("Administrator")]
    public partial class UserForm
    {
        [Parameter] public UserModel UserParameter { get; set; } = new UserModel();
        [Parameter] public Common.FormModeEnum FormMode { get; set; } = Common.FormModeEnum.ADD;

        public bool ShowPasswordInput { get; set; } = true;

        public RegistrationModel Registration { get; set; } = new RegistrationModel();

        public List<Role> Roles { get; set; } = new List<Role>
    {
        new Role { RoleValue = "Administrator" },
        new Role { RoleValue = "Operator" }
    };

        private async Task OnSubmit()
        {
            if (FormMode == Common.FormModeEnum.ADD)
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
            try
            {
                var response = await _authService.RegisterAsync(Registration);

                var closeReturn = response != null ? true : false;

                DialogService.Close(closeReturn);
            }
            catch (Exception ex)
            {
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
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

            try
            {
                var response = await _userService.UpdateUser(userToUpdate);

                var closeReturn = response != null ? true : false;

                await SweetAlertServices.ShowSuccessAlert("Usuario actualizado", "El usuario se actualizó correctamente");

                DialogService.Close(closeReturn);
            }
            catch (Exception ex)
            {
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
        }

        protected override void OnInitialized()
        {
            if (FormMode == Common.FormModeEnum.EDIT)
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
