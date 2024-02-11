// -----------------------------------------------------------------------
// <copyright file="DeliveryModel.cs" company="Jorge Robredo">
// Copyright (c) Developed by Jorge Robredo. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SBOAddonDeliveries.Core.Delivery
{
    /// <summary>
    /// the exposed class to perform the creation of the releases.
    /// </summary>
    public class DeliveryModel
    {
        /// <summary>
        /// Gets or sets represents the DocEntry field of the ODLN table.
        /// </summary>
        public int DocEntry { get; set; }

        /// <summary>
        /// Gets or sets represents the CardCode field of the ODLN table.
        /// </summary>
        public string CardCode { get; set; }

        /// <summary>
        /// Gets or sets represents the ItemCode field of the DLN1 table.
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// Gets or sets represents the WhsCode field of the DLN1 table.
        /// </summary>
        public string WhsCode { get; set; }

        /// <summary>
        /// Gets or sets represents the Quantity field of the DLN1 table.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets represents the Price field of the DLN1 table.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets represents the LineNum field of the RDR1 table that will be associated with the ODLN table.
        /// </summary>
        public int BaseLine { get; set; }
    }
}
