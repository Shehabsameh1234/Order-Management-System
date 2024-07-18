using OrderSys.Core.Entities;


namespace OrderSys.Core.Specifications.CustomerSpecifications
{
    public class CustomerSpecifications :BaseSpecifications<Customer>
    {
        public CustomerSpecifications(int id) : base(c=>c.Id==id) 
        {
            Includes.Add(c => c.Orders);
            
        }
    }
}
