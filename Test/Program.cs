using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            MongoTest t = new MongoTest();
            t.f();

            Console.Read();
        }
    }

    public class father
    {
        public void fun()
        {
            Console.WriteLine("!!!");
        }


        protected void OnModelCreating(ModuleBuilder modelBuilder)
        {
            //modelBuilder.Entity(pc =>
            //{
            //    pc.ToTable("Blog").HasKey(k => k.Id);
            //    pc.Property(p => p.Name).IsRequired();
            //    pc.Property(p => p.Url).IsRequired();
            //    pc.Property(p => p.Count).IsRequired();
            //});
        }
    }
    public class child
    {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Url { get; set; }
            public int Count { get; set; }
        }
}
