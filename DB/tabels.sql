CREATE TABLE
    Users (
        UserID GUID PRIMARY KEY IDENTITY (1, 1),
        Username VARCHAR(255) UNIQUE NOT NULL,
        Password VARCHAR(255) NOT NULL,
        Email VARCHAR(255) UNIQUE NOT NULL,
        Role ENUM ('Buyer', 'Seller', 'Admin') NOT NULL,
        CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
    );

CREATE TABLE
    Products (
        ProductID GUID PRIMARY KEY IDENTITY (1, 1),
        UserID INT, -- Refers to the seller
        ProductName VARCHAR(255) NOT NULL,
        Description TEXT,
        Price DECIMAL(10, 2) NOT NULL,
        Stock INT NOT NULL,
        CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
        ProductImage varbinary(MAX),
        FOREIGN KEY (UserID) REFERENCES Users (UserID)
    );

CREATE TABLE
    History (
        HistoryID GUID PRIMARY KEY IDENTITY (1, 1),
        UserID INT, -- Refers to the buyer
        ProductID INT,
        Quantity INT NOT NULL,
        PurchaseDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
        FOREIGN KEY (UserID) REFERENCES Users (UserID),
        FOREIGN KEY (ProductID) REFERENCES Products (ProductID)
    );

---------------------------------------------------------------Normalized---------------------------------------------------------------
-- Users Table
-- Stores common user information for all types of users (Buyers, Sellers, Admins)
CREATE TABLE
    Users (
        UserID GUID PRIMARY KEY IDENTITY (1, 1), -- Unique identifier for each user
        Username VARCHAR(255) UNIQUE NOT NULL, -- Unique username for each user
        Password VARCHAR(255) NOT NULL, -- Password for authentication
        Email VARCHAR(255) UNIQUE NOT NULL, -- Unique email for each user
        CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP -- Timestamp of account creation
    );

-- Buyers Table
-- Stores additional information specific to buyers
CREATE TABLE
    Buyers (
        BuyerID GUID PRIMARY KEY, -- Foreign key to Users
        FOREIGN KEY (BuyerID) REFERENCES Users (UserID) ON DELETE CASCADE, -- Ensures BuyerID exists in Users
        DeliveryAdd VARCHAR(255) -- Buyer-specific field (e.g., shipping address)
    );

-- Sellers Table
-- Stores additional information specific to sellers
CREATE TABLE
    Sellers (
        SellerID GUID PRIMARY KEY, -- Foreign key to Users
        FOREIGN KEY (SellerID) REFERENCES Users (UserID) ON DELETE CASCADE, -- Ensures SellerID exists in Users
        StoreName VARCHAR(255) NOT NULL, -- Name of the seller's store
        Rating DECIMAL(3, 2) -- Seller rating (e.g., 4.5)
    );

-- Products Table
-- Contains product information and seller details
CREATE TABLE
    Products (
        ProductID GUID PRIMARY KEY IDENTITY (1, 1), -- Unique identifier for each product
        SellerID GUID, -- Refers to the seller (UserID)
        FOREIGN KEY (SellerID) REFERENCES Sellers (SellerID) ON DELETE CASCADE, -- Links to Sellers table
        ProductName VARCHAR(255) NOT NULL, -- Name of the product
        Description TEXT, -- Detailed description of the product
        Price DECIMAL(10, 2) NOT NULL, -- Price of the product
        StockQuantity INT NOT NULL, -- Current stock level of the product
        ImageData VARBINARY(MAX) -- URL or binary data for the image
        CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP -- Timestamp of when the product was added
    );


-- Purchases Table
-- Records general information about purchases made by buyers
CREATE TABLE
    Purchases (
        PurchaseID GUID PRIMARY KEY IDENTITY (1, 1), -- Unique identifier for each purchase
        BuyerID GUID, -- Refers to the buyer (UserID)
        FOREIGN KEY (BuyerID) REFERENCES Buyers (BuyerID) ON DELETE CASCADE, -- Links to Buyers table
        ProductID GUID, -- Refers to the purchased product
        FOREIGN KEY (ProductID) REFERENCES Products (ProductID) ON DELETE CASCADE, -- Links to Products table
        Quantity INT NOT NULL, -- Quantity of the product purchased
        PurchaseDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP -- When the purchase was made
    );