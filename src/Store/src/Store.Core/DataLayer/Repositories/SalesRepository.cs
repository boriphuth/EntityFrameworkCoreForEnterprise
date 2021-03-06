using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store.Core.DataLayer.Contracts;
using Store.Core.DataLayer.DataContracts;
using Store.Core.EntityLayer.Dbo;
using Store.Core.EntityLayer.HumanResources;
using Store.Core.EntityLayer.Sales;

namespace Store.Core.DataLayer.Repositories
{
    public class SalesRepository : Repository, ISalesRepository
    {
        public SalesRepository(IUserInfo userInfo, StoreDbContext dbContext)
            : base(userInfo, dbContext)
        {
        }

        public IQueryable<Customer> GetCustomers()
            => DbContext.Set<Customer>();

        public async Task<Customer> GetCustomerAsync(Customer entity)
            => await DbContext.Set<Customer>().FirstOrDefaultAsync(item => item.CustomerID == entity.CustomerID);

        public async Task<Int32> AddCustomerAsync(Customer entity)
        {
            Add(entity);

            return await CommitChangesAsync();
        }

        public async Task<Int32> UpdateCustomerAsync(Customer changes)
        {
            Update(changes);

            return await CommitChangesAsync();
        }

        public async Task<Int32> DeleteCustomerAsync(Customer entity)
        {
            Remove(entity);

            return await CommitChangesAsync();
        }

        public IQueryable<OrderInfo> GetOrders(Int16? currencyID = null, Int32? customerID = null, Int32? employeeID = null, Int16? orderStatusID = null, Guid? paymentMethodID = null, Int32? shipperID = null)
        {
            var query =
                from order in DbContext.Set<Order>()
                join currencyJoin in DbContext.Set<Currency>() on order.CurrencyID equals currencyJoin.CurrencyID into currencyTemp
                from currency in currencyTemp.Where(relation => relation.CurrencyID == order.CurrencyID).DefaultIfEmpty()
                join customer in DbContext.Set<Customer>() on order.CustomerID equals customer.CustomerID
                join employeeJoin in DbContext.Set<Employee>() on order.EmployeeID equals employeeJoin.EmployeeID into employeeTemp
                from employee in employeeTemp.Where(relation => relation.EmployeeID == order.EmployeeID).DefaultIfEmpty()
                join orderStatus in DbContext.Set<OrderStatus>() on order.OrderStatusID equals orderStatus.OrderStatusID
                join paymentMethodJoin in DbContext.Set<PaymentMethod>() on order.PaymentMethodID equals paymentMethodJoin.PaymentMethodID into paymentMethodTemp
                from paymentMethod in paymentMethodTemp.Where(relation => relation.PaymentMethodID == order.PaymentMethodID).DefaultIfEmpty()
                join shipperJoin in DbContext.Set<Shipper>() on order.ShipperID equals shipperJoin.ShipperID into shipperTemp
                from shipper in shipperTemp.Where(relation => relation.ShipperID == order.ShipperID).DefaultIfEmpty()
                select new OrderInfo
                {
                    OrderID = order.OrderID,
                    OrderStatusID = order.OrderStatusID,
                    CustomerID = order.CustomerID,
                    EmployeeID = order.EmployeeID,
                    ShipperID = order.ShipperID,
                    OrderDate = order.OrderDate,
                    Total = order.Total,
                    CurrencyID = order.CurrencyID,
                    PaymentMethodID = order.PaymentMethodID,
                    Comments = order.Comments,
                    CreationUser = order.CreationUser,
                    CreationDateTime = order.CreationDateTime,
                    LastUpdateUser = order.LastUpdateUser,
                    LastUpdateDateTime = order.LastUpdateDateTime,
                    Timestamp = order.Timestamp,
                    CurrencyCurrencyName = currency == null ? String.Empty : currency.CurrencyName,
                    CurrencyCurrencySymbol = currency == null ? String.Empty : currency.CurrencySymbol,
                    CustomerCompanyName = customer == null ? String.Empty : customer.CompanyName,
                    CustomerContactName = customer == null ? String.Empty : customer.ContactName,
                    EmployeeFirstName = employee.FirstName,
                    EmployeeMiddleName = employee == null ? String.Empty : employee.MiddleName,
                    EmployeeLastName = employee.LastName,
                    EmployeeBirthDate = employee.BirthDate,
                    OrderStatusDescription = orderStatus.Description,
                    PaymentMethodPaymentMethodName = paymentMethod == null ? String.Empty : paymentMethod.PaymentMethodName,
                    PaymentMethodPaymentMethodDescription = paymentMethod == null ? String.Empty : paymentMethod.PaymentMethodDescription,
                    ShipperCompanyName = shipper == null ? String.Empty : shipper.CompanyName,
                    ShipperContactName = shipper == null ? String.Empty : shipper.ContactName,
                };

            if (currencyID.HasValue)
            {
                query = query.Where(item => item.CurrencyID == currencyID);
            }

            if (customerID.HasValue)
            {
                query = query.Where(item => item.CustomerID == customerID);
            }

            if (employeeID.HasValue)
            {
                query = query.Where(item => item.EmployeeID == employeeID);
            }

            if (orderStatusID.HasValue)
            {
                query = query.Where(item => item.OrderStatusID == orderStatusID);
            }

            if (paymentMethodID.HasValue)
            {
                query = query.Where(item => item.PaymentMethodID == paymentMethodID);
            }

            if (shipperID.HasValue)
            {
                query = query.Where(item => item.ShipperID == shipperID);
            }

            return query;
        }

        public async Task<Order> GetOrderAsync(Order entity)
            => await DbContext.Set<Order>().Include(p => p.OrderDetails).FirstOrDefaultAsync(item => item.OrderID == entity.OrderID);

        public Task<Int32> AddOrderAsync(Order entity)
        {
            Add(entity);

            return CommitChangesAsync();
        }

        public async Task<Int32> UpdateOrderAsync(Order changes)
        {
            Update(changes);

            return await CommitChangesAsync();
        }

        public async Task<Int32> DeleteOrderAsync(Order entity)
        {
            Remove(entity);

            return await CommitChangesAsync();
        }

        public async Task<OrderDetail> GetOrderDetailAsync(OrderDetail entity)
            => await DbContext.Set<OrderDetail>().FirstOrDefaultAsync(item => item.OrderID == entity.OrderID && item.ProductID == entity.ProductID);

        public Task<Int32> AddOrderDetailAsync(OrderDetail entity)
        {
            Add(entity);

            return CommitChangesAsync();
        }

        public async Task<Int32> UpdateOrderDetailAsync(OrderDetail changes)
        {
            Update(changes);

            return await CommitChangesAsync();
        }

        public async Task<Int32> DeleteOrderDetailAsync(OrderDetail entity)
        {
            Remove(entity);

            return await CommitChangesAsync();
        }

        public IQueryable<Shipper> GetShippers()
            => DbContext.Set<Shipper>();

        public async Task<Shipper> GetShipperAsync(Shipper entity)
            => await DbContext.Set<Shipper>().FirstOrDefaultAsync(item => item.ShipperID == entity.ShipperID);

        public async Task<Int32> AddShipperAsync(Shipper entity)
        {
            Add(entity);

            return await CommitChangesAsync();
        }

        public async Task<Int32> UpdateShipperAsync(Shipper changes)
        {
            Update(changes);

            return await CommitChangesAsync();
        }

        public async Task<Int32> DeleteShipperAsync(Shipper entity)
        {
            Remove(entity);

            return await CommitChangesAsync();
        }

        public IQueryable<OrderStatus> GetOrderStatus()
            => DbContext.Set<OrderStatus>();

        public async Task<OrderStatus> GetOrderStatusAsync(OrderStatus entity)
            => await DbContext.Set<OrderStatus>().FirstOrDefaultAsync(item => item.OrderStatusID == entity.OrderStatusID);

        public async Task<Int32> AddOrderStatusAsync(OrderStatus entity)
        {
            Add(entity);

            return await CommitChangesAsync();
        }

        public async Task<Int32> UpdateOrderStatusAsync(OrderStatus changes)
        {
            Update(changes);

            return await CommitChangesAsync();
        }

        public async Task<Int32> RemoveOrderStatusAsync(OrderStatus entity)
        {
            Remove(entity);

            return await CommitChangesAsync();
        }

        public IQueryable<Currency> GetCurrencies()
            => DbContext.Set<Currency>();

        public IQueryable<PaymentMethod> GetPaymentMethods()
            => DbContext.Set<PaymentMethod>();
    }
}
