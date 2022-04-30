using DataAccess.Abstract;
using DataAccess.Concrete.DapperORM;
using Entities.Concrete;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            IEmployeeDal dal = new DpEmployeeDal();
            var lissrrer = dal.GetById(1);
            var list =dal.GetAll();
             //dal.Update(new Employee { Id = 1, Name = "Veli", Username = "Veliase" });
            Console.WriteLine("Hello World!");
        }
    }
}
