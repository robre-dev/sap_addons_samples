using AttachmentsOrderSales.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttachmentsOrderSales
{
    public class OrderSalesModel
    {

        public static Result Create(OrderSales orderSales)
        {

            SAPbobsCOM.Documents doc = (SAPbobsCOM.Documents)Program.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);

            doc.CardCode = orderSales.CardCode;
            doc.DocDate = orderSales.DocDate;



            foreach (var item in orderSales.OrderSalesDetails)
            {
                doc.Lines.Add();
                doc.Lines.ItemCode = item.ItemCode;
                doc.Lines.Quantity = item.Quantity;
                doc.Lines.Price = item.Price;
            }


            SAPbobsCOM.Attachments2 oAtt = (SAPbobsCOM.Attachments2)Program.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2);

            foreach (Atachments file in orderSales.Atachments)
            {

                oAtt.Lines.Add();
                oAtt.Lines.FileName = file.Name;
                oAtt.Lines.FileExtension = file.Format;
                oAtt.Lines.SourcePath = file.FilePath;
                oAtt.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES;
            }
            if (oAtt.Add() != 0)
            {

                throw new Exception(Program.Company.GetLastErrorDescription());
            }
            else
            {
                doc.AttachmentEntry = oAtt.AbsoluteEntry;

            }

            if (doc.Add() != 0)
            {
                return new Result
                {
                    Passed = false,
                    Error = Program.Company.GetLastErrorDescription()
                };
            }

            return new Result
            {
                Passed = true,
               
            };
        }

        public static Result Update(OrderSales orderSales)
        {

            SAPbobsCOM.Documents doc = (SAPbobsCOM.Documents)Program.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);


            if (doc.GetByKey(orderSales.DocEntry))
            {
                SAPbobsCOM.Attachments2 oAtt = (SAPbobsCOM.Attachments2)Program.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2);
                foreach (Atachments file in orderSales.Atachments)
                {

                    oAtt.Lines.Add();
                    oAtt.Lines.FileName = file.Name;
                    oAtt.Lines.FileExtension = file.Format;
                    oAtt.Lines.SourcePath = file.FilePath;
                    oAtt.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES;
                }

                if (oAtt.Add() != 0)
                {

                    throw new Exception(Program.Company.GetLastErrorDescription());
                }
                else
                {
                    doc.AttachmentEntry = oAtt.AbsoluteEntry;

                }

                if (doc.Add() != 0)
                {
                    return new Result
                    {
                        Passed = false,
                        Error = Program.Company.GetLastErrorDescription()
                    };
                }

                return new Result
                {
                    Passed = true,

                };
            }
            else
            {
                return new Result { Error = "El documento no existe.", Passed = false };
            }
            
        }
    }
}
