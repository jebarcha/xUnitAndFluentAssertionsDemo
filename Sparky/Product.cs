using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparky;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public double GetPrice(Customer customer) => customer.IsPlatinum ? Price * .8 : Price;
    public double GetPrice(ICustomer customer) => customer.IsPlatinum ? Price * .8 : Price;
}
