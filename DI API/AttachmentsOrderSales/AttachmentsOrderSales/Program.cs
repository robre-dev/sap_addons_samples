using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttachmentsOrderSales
{
    class Program
    {
        public static SAPbobsCOM.Company Company;
        static void Main(string[] args)
        {
            bool exit = false;
            Company = new SAPbobsCOM.Company();
            Company.SLDServer = "192.168.1.39:30013";
            Company.Server = "192.168.1.39";
            Company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB;
            Company.CompanyDB = "SBO_DEMOUS1";
            Company.Password = "Sap5erver";
            Company.UserName = "manager";
            Company.DbUserName = "B1ADMIN";
            Company.DbPassword = "Sap5erver";

            if (Company.Connect() != 0)
            {
                exit = true;
                Console.WriteLine("Error al conectar la compañia");
            }

            while (!exit)
            {
                Console.Clear();
                int option = 0;
                Console.WriteLine("Selecciones una opcion:");
                Console.WriteLine("1 Crear Orden:");
                Console.WriteLine("2 Actualizar Orden:");
                Console.WriteLine("0 Salir:");
                option = Convert.ToInt32(Console.ReadLine());
                Result doc = null;
                switch (option)
                {
                    case 1:
                        exit = false;
                       doc = OrderSalesModel.Create(new Entities.OrderSales
                        {
                            CardCode = "C20000",
                            DocDate = DateTime.Now,
                            Atachments = new List<Entities.Atachments>
                            {
                                new Entities.Atachments
                                {
                                    Format = "pdf",
                                    Name = "Ficha tecnica",
                                    FilePath = "C:\\Files\\Ficha tecnica.pdf"

                                }
                            },
                            OrderSalesDetails = new List<Entities.OrderSalesDetails>
                            {
                                new Entities.OrderSalesDetails
                                {
                                    ItemCode = "A000004",
                                    Price= 145,
                                    Quantity = 10
                                }
                            }
                        });
                        break;

                    case 2:
                        doc = OrderSalesModel.Update(new Entities.OrderSales
                        {
                            DocDate = DateTime.Now,
                            DocEntry = 121,
                            CardCode = "C20000",
                            Atachments = new List<Entities.Atachments>
                            {
                                new Entities.Atachments
                                {
                                    Format = "pdf",
                                    Name = "Ficha tecnica",
                                    FilePath = "C:\\Files\\Ficha tecnica.pdf"

                                }
                            },
                            OrderSalesDetails = new List<Entities.OrderSalesDetails>
                            {
                                new Entities.OrderSalesDetails
                                {
                                    ItemCode = "A000004",
                                    Price= 145,
                                    Quantity = 10
                                }
                            }
                        });
                        exit = false;
                        break;
                    case 0:
                        Company.Disconnect();
                        Company = null;
                        GC.WaitForPendingFinalizers();
                        GC.Collect();
                        exit = true;
                        break;

                    default:
                        exit = false;
                        break;
                }
                if (!doc.Passed)
                {
                    Console.WriteLine(doc.Error);
                    Console.ReadKey();
                }


            }
            Console.ReadKey();
           
        }
    }
}
