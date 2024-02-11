// -----------------------------------------------------------------------
// <copyright file="DeliveryManager.cs" company="Jorge Robredo">
// Copyright (c) Developed by Jorge Robredo. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SBOAddonDeliveries.Core.Delivery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// It is the manager class for the creation of deliveries in SAP BO.
    /// </summary>
    public class DeliveryManager : IDeliveryManager
    {
        private readonly SAPbobsCOM.Company company;

        public DeliveryManager(SAPbobsCOM.Company company)
        {
            this.company = company;
        }

        /// <summary>
        /// Execute the delivery.
        /// </summary>
        /// <param name="listDeliveries">It is the list of deliveries that they want to process.</param>
        /// <param name="groupByCustomer">Defines whether to group by business partner.</param>
        /// <returns>Returns if the process was executed correctly and the error message.</returns>
        public ResultExecuteDelivery ExecuteDelivery(List<DeliveryModel> listDeliveries, bool groupByCustomer = false)
        {
            var deliveriesToProcess = new List<DeliveryToProcessModel>();

            if (groupByCustomer)
            {
                deliveriesToProcess = listDeliveries.GroupBy(l => l.CardCode)
                    .Select(l => new DeliveryToProcessModel
                    {
                        CardCode = l.Key,
                        DeliveryLinesToProcess = l.Select(x => new DeliveryLinesToProcessModel
                        {
                            ItemCode = x.ItemCode,
                            Price = x.Price,
                            Quantity = x.Quantity,
                            WhsCode = x.WhsCode,
                            DocEntrySO = x.DocEntry,
                            BaseLine = x.BaseLine,
                        }).ToList(),
                    }).ToList();
            }
            else
            {
                deliveriesToProcess = listDeliveries.GroupBy(l => new { l.DocEntry, l.CardCode })

                    .Select(l =>
                    new DeliveryToProcessModel
                    {
                        CardCode = l.Key.CardCode,
                        DeliveryLinesToProcess = l.Select(x => new DeliveryLinesToProcessModel
                        {
                            ItemCode = x.ItemCode,
                            Price = x.Price,
                            Quantity = x.Quantity,
                            WhsCode = x.WhsCode,
                            DocEntrySO = l.Key.DocEntry,
                            BaseLine = x.BaseLine,
                        }).ToList(),
                    }).ToList();
            }

            this.company.StartTransaction();
            int countErrors = 0;
            foreach (var delivery in deliveriesToProcess)
            {
                SAPbobsCOM.Documents deliveryDoc = (SAPbobsCOM.Documents)this.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes);
                deliveryDoc.CardCode = delivery.CardCode;
                deliveryDoc.DocDate = DateTime.Now;
                deliveryDoc.DocDueDate = DateTime.Now;

                foreach (var deliveryLines in delivery.DeliveryLinesToProcess)
                {
                    deliveryDoc.Lines.ItemCode = deliveryLines.ItemCode;
                    deliveryDoc.Lines.WarehouseCode = deliveryLines.WhsCode;
                    deliveryDoc.Lines.Price = deliveryLines.Price;
                    deliveryDoc.Lines.Quantity = deliveryLines.Quantity;
                    if (deliveryLines.DocEntrySO != 0)
                    {
                        deliveryDoc.Lines.BaseLine = deliveryLines.BaseLine;
                        deliveryDoc.Lines.BaseType = 17;
                        deliveryDoc.Lines.BaseEntry = deliveryLines.DocEntrySO;
                    }

                    deliveryDoc.Lines.Add();

                }

                if (deliveryDoc.Add() != 0)
                {
                    countErrors++;
                    var messageError = this.company.GetLastErrorDescription();
                }

            }

            if (countErrors == 0)
            {
                this.company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
            }

            return new ResultExecuteDelivery
            {
                Passed = countErrors == 0,
                Message = countErrors > 0 ? $"{countErrors} errors have occurred" : string.Empty,
            };
        }
    }
}
