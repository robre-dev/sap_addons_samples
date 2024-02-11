# SAP B1 Technical Test Developer
[img1]: https://upload.wikimedia.org/wikipedia/commons/4/4d/SAP-Business-One-Logo.png
![img1]

## Introduction
### Overview
Develop an Add-on for SAP B1 that generates deliveries based on open sales orders. 

### Technical scopes
The Add-on must be developed on the following platforms:
- C# (.NET Framework 4.5 or higher)
- SQL/HANA
- SAP B1 9.0 or higher
### Features
- Create a menu called: Deliveries AddOn
   - a.Create a sub menu called: Create Deliveries
- Open a form when clicking the submenu to view all open sales orders displayed in a table. It must be able to filter by business partner.
Note: You only can view business partner of customer type.
- a.The table must contain the following fields:
   - i. CheckBox
   - ii. DocEntry (Link to document)
   - iii. DocDueDate
   - iv. DocTotal
- Based on the checked sales order, open a new form to show all the items of the sales orders. You can add more articles manually or delete lines.
     - a.The table must contain the following fields:
  - i. SO DocEntry (Link to document)
  - ii. ItemCode (Add Choose from List) 
   - iii. Quantity
   - iv. Price
- Finally, a delivery document must be created with the articles in the grid. Note: Delivery must be linked to sales orders.
Deliverable
 - 64 bits AddOn installer using the Extension manager



