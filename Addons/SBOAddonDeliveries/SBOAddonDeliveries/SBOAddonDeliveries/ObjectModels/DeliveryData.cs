// -----------------------------------------------------------------------
// <copyright file="DeliveryData.cs" company="Jorge Robredo">
// Copyright (c) Developed by Jorge Robredo. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SBOAddonDeliveries.ObjectModels
{
    public class DeliveryData
    {
        public int DocEntry { get; set; }

        public string CardCode { get; set; }

        public string ItemCode { get; set; }

        public string WhsCode { get; set; }

        public int BaseLine { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }
    }
}
