using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize(DatabaseContext context)
        {
            context.Database.EnsureCreated();

            if (context.Customers.Any())
            {
                return;
            }

            var customers = new Customer[]
            {
                new Customer{Name="Tuomo Laukkanen", Address="Sotilaspojankatu 2", BillingAddress="Sotilaspojankatu 2", Phone="0504041234", Email="tuomo.laukkanen@email.com", Password="tuomo123"}
            };
            foreach (Customer c in customers)
            {
                context.Customers.Add(c);
            }
            context.SaveChanges();

            var apartments = new Apartment[]
            {
                new Apartment{ApartmentType=ApartmentType.Kerrostalo, ApartmentArea=60.0, PropertyArea=0, CustomerID=1}
            };
            foreach (Apartment a in apartments)
            {
                context.Apartments.Add(a);
            }
            context.SaveChanges();

            var orders = new Order[]
            {
                new Order{State=State.TILATTU, Description="WC:n siivous", OrderDate=DateTime.Parse("2018-04-21"), StartDate=null, FinishDate=null, AcceptDate=null, CancelDate=null, Comment=null, HourCount=null, Cost=null, UsedAccessories=null, CustomerID=1},
                new Order{State=State.TILATTU, Description="Tuhkakupin tyhjennys", OrderDate=DateTime.Parse("2018-04-22"), StartDate=null, FinishDate=null, AcceptDate=null, CancelDate=null, Comment=null, HourCount=null, Cost=null, UsedAccessories=null, CustomerID=1},
                new Order{State=State.ALOITETTU, Description="Varaston järjestely", OrderDate=DateTime.Parse("2018-04-20"), StartDate=DateTime.Parse("2018-04-21"), FinishDate=null, AcceptDate=null, CancelDate=null, Comment=null, HourCount=null, Cost=null, UsedAccessories=null, CustomerID=1},
                new Order{State=State.VALMIS, Description="Kesärenkaiden vaihto", OrderDate=DateTime.Parse("2018-04-01"), StartDate=DateTime.Parse("2018-04-02"), FinishDate=DateTime.Parse("2018-04-02"), AcceptDate=null, CancelDate=null, Comment="Pinnapultti meni poikki..", HourCount=1.0, Cost=25.0, UsedAccessories="Uudet pyöränpultit", CustomerID=1},
                new Order{State=State.VALMIS, Description="Öljyjen vaihto", OrderDate=DateTime.Parse("2018-04-01"), StartDate=DateTime.Parse("2018-04-02"), FinishDate=DateTime.Parse("2018-04-02"), AcceptDate=null, CancelDate=null, Comment="Kannattas vaihtaa noi öljyt vähän useammin..", HourCount=1.0, Cost=35.0, UsedAccessories="3l öljyä + suodatin", CustomerID=1},
                new Order{State=State.HYVÄKSYTTY, Description="Auton pesu", OrderDate=DateTime.Parse("2018-03-01"), StartDate=DateTime.Parse("2018-03-02"), FinishDate=DateTime.Parse("2018-03-02"), AcceptDate=DateTime.Parse("2018-03-02"), CancelDate=null, Comment="Olipas paskainen auto..", HourCount=2.5, Cost=35.0, UsedAccessories="15l pesuainetta", CustomerID=1}
            };
            foreach (Order o in orders)
            {
                context.Orders.Add(o);
            }
            context.SaveChanges();

        }

    }
}
