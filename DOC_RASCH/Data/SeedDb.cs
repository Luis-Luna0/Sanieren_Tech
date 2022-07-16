using DOC_RASCH.Common.Enums;
using DOC_RASCH.Data.Entities;
using DOC_RASCH.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace DOC_RASCH.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }


        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckBusinessAsync();
            await CheckRolesAsycn();
            await CheckStatusAsync();
            await CheckUserAsync("Luis", "Luna", "__azphyxiia__@hotmail.es", "5528903575", "Gustavo Baz #230", UserType.Admin,1);
            await CheckUserAsync("Brayan", "Piña", "brayan@gmail.com", "5612345678", "Calle Luna Calle Sol", UserType.Admin,1);
            await CheckUserAsync("Cinthia", "Uriostegui", "cinthia@gmail.com", "5522889966", "Calle Luna Calle Sol", UserType.User,1);
            await CheckUserAsync("Alondra", "Rocha", "alondra@gmail.com", "5512345678", "Calle Luna Calle Sol", UserType.User,1);
            
        }

        private async Task CheckUserAsync(string firstName, string lastName, string email, string phoneNumber, string address, UserType userType, int active)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    Address = address,
                    Business = _context.Business.FirstOrDefault(x => x.Name == "RASCH"),
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = phoneNumber,
                    UserName = email,
                    UserType = userType,
                    Active = active
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }
        }

        private async Task CheckBusinessAsync()
        {
            if (!_context.Business.Any())
            {
                _context.Business.Add(new Business { Name = "RASCH", Active =1 });
                _context.Business.Add(new Business { Name = "Amazon", Active = 1 });
                _context.Business.Add(new Business { Name = "Telmex", Active = 1 });
                _context.Business.Add(new Business { Name = "Banco Azteca", Active = 1 });
                await _context.SaveChangesAsync();
            }
        }


        private async Task CheckRolesAsycn()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task CheckStatusAsync()
        {
            if (!_context.Status.Any())
            {
                _context.Status.Add(new Status { Name = "Apobado" });
                _context.Status.Add(new Status { Name = "Pendiente" });
                _context.Status.Add(new Status { Name = "Rechazado" });
                await _context.SaveChangesAsync();
            }
        }

    }
}