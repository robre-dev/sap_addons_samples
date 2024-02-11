// -----------------------------------------------------------------------
// <copyright file="DeliveryLinesToProcessModel.cs" company="Jorge Robredo">
// Copyright (c) Developed by Jorge Robredo. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SBOAddonDeliveries.Core.Delivery
{
    public class DeliveryLinesToProcessModel
    {
        public string ItemCode { get; set; }

        public string WhsCode { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public int DocEntrySO { get; set; }

        public int BaseLine { get; set; }


    }
}