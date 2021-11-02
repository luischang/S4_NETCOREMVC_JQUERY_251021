using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using S4_NETCOREMVC_JQUERY.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace S4_NETCOREMVC_JQUERY.Web.Repository
{
    public class CustomerRepo
    {

        public static IEnumerable<Customer> GetCustomers()
        {
            using var data = new SalesDBContext();
            return data.Customer.ToList();
        }
        public static IEnumerable<Customer> GetCustomersSP()
        {
            using var data = new SalesDBContext();
            return data.Customer.FromSqlRaw("SP_GET_CUSTOMER");
        }

        public static async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            //using var data = new SalesDBContext();
            //return await data.Customer.Include(x=>x.Order).ToListAsync();

            using var httpClient = new HttpClient();
            using var response = await httpClient
                .GetAsync("http://localhost:24487/api/Customer/Customer");
            string apiResponse = await response.Content.ReadAsStringAsync();
            var customers = JsonConvert.DeserializeObject<IEnumerable<Customer>>(apiResponse);
            return customers;
        }

        public static async Task<Customer> GetCustomerById(int id)
        {
            //using var data = new SalesDBContext();
            //return await data.Customer.Where(x => x.Id == id).FirstOrDefaultAsync();

            using var httpClient = new HttpClient();
            using var response = await httpClient
                .GetAsync("http://localhost:24487/api/Customer/CustomerById?id=" + id);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var customer = JsonConvert.DeserializeObject<Customer>(apiResponse);
            return customer;

        }

        public static async Task<bool> Insert(Customer customer)
        {
            //bool exito = true;
            //try
            //{
            //    using var data = new SalesDBContext();
            //    await data.Customer.AddAsync(customer);
            //    await data.SaveChangesAsync();
            //}
            //catch (Exception)
            //{
            //    exito = false;
            //}

            var json = JsonConvert.SerializeObject(customer);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            using var response = await httpClient
                .PostAsync("http://localhost:24487/api/Customer/PostCustomer", data);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var customerResponse = JsonConvert.DeserializeObject<bool>(apiResponse);

            return customerResponse;
        }

        public static async Task<bool> Update(Customer customer)
        {
            //bool exito = true;
            //try
            //{
            //    using var data = new SalesDBContext();
            //    var customerNow = await data.Customer.Where(x => x.Id == customer.Id).FirstOrDefaultAsync();

            //    customerNow.FirstName = customer.FirstName;
            //    customerNow.LastName = customer.LastName;
            //    customerNow.City = customer.City;
            //    customerNow.Country = customer.Country;
            //    customerNow.Phone = customer.Phone;

            //    await data.SaveChangesAsync();
            //}
            //catch (Exception)
            //{
            //    exito = false;
            //}
            //return exito;

            var json = JsonConvert.SerializeObject(customer);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            using var response = await httpClient
                .PutAsync("http://localhost:24487/api/Customer/PutCustomer", data);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var customerResponse = JsonConvert.DeserializeObject<bool>(apiResponse);

            return customerResponse;

        }

        public static async Task<bool> Delete(int id)
        {
            bool exito = true;
            using var httpClient = new HttpClient();
            using var response = await httpClient
                .DeleteAsync("http://localhost:24487/api/Customer/DeleteCustomer?id=" + id);
            string apiResponse = await response.Content.ReadAsStringAsync();
            if ((int)response.StatusCode == 400)
                exito = false;


            return exito;
            //bool exito = true;

            //try
            //{
            //    using var data = new SalesDBContext();
            //    var customerNow = await data.Customer.Where(x => x.Id == id).FirstOrDefaultAsync();
            //    data.Customer.Remove(customerNow);
            //    await data.SaveChangesAsync();

            //}
            //catch (Exception)
            //{
            //    exito = false;
            //}

            //return exito;
        }


    }
}
