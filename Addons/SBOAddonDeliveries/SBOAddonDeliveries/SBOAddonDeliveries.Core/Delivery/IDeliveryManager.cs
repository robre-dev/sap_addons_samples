// -----------------------------------------------------------------------
// <copyright file="IDeliveryManager.cs" company="Jorge Robredo">
// Copyright (c) Developed by Jorge Robredo. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SBOAddonDeliveries.Core.Delivery
{
    using System.Collections.Generic;

    public interface IDeliveryManager
    {
        ResultExecuteDelivery ExecuteDelivery(List<DeliveryModel> listDeliveries, bool groupByCustomer = false);
    }
}