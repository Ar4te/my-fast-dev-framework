using desktop.DataAccess.Repository;
using desktop.Models;
using desktop.Services.Base;
using desktop.Services.IServices;

namespace desktop.Services.Implements;

public class ContactService : BaseService<Contact>, IContactService
{
    public ContactService(IBaseRepository<Contact> contactRepo)
    {
        BaseRepo = contactRepo;
    }
}