using System;
namespace ProjectManagementApplication.Models.Interfaces
{
	public interface IServiceRepository
	{


        void AddService(Service s);
        void AddPurchasedService(PurchasedServices service);
        int Count();
        void RemoveService(Service s);
        Service? RetrieveService(int id);
        List<Service> GetAllServices();
        void UpdateService(Service service);
    }
}

