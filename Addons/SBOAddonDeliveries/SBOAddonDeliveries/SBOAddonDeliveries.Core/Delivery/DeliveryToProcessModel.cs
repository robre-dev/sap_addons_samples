// -----------------------------------------------------------------------
// <copyright file="DeliveryToProcessModel.cs" company="Jorge Robredo">
// Copyright (c) Developed by Jorge Robredo. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SBOAddonDeliveries.Core.Delivery
{
    using System.Collections.Generic;

    public class DeliveryToProcessModel
    {
        public string CardCode { get; set; }

        public List<DeliveryLinesToProcessModel> DeliveryLinesToProcess { get; set; }
    }
}
