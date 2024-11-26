export class Product {
  productId: string = '';
  productName: string = '';
  description?: string;
  imageContent: Uint8Array = new Uint8Array();
  price: number = 0;
  stockQuantity: number = 0;
  createdAt: Date = new Date();
  updatedAt: Date = new Date();
  seller: {
    userId: string;
    username: string;
    email: string;
  } = {
    userId: '',
    username: '',
    email: '',
  };
}

export class Role {
  roleId: string = '';
  roleName: string = '';
}

export class TransactionHistory {
  transactionId: string = '';
  productId: string = '';
  buyerId: string = '';
  quantity: number = 0;
  totalAmount: number = 0;
  transactionDate: Date = new Date();
}
export class User {
  userId: string = '';
  username: string = '';
  email: string = '';
  passwordHash: string = '';
}
export class UserAudit {
  userAuditId: string = '';
  userId: string = '';
  username: string = '';
  loginTime?: Date;
  logoutTime?: Date;
}

export class UserRole {
  userId: string = '';
  roleId: string = '';
}

export class Filter {
  Filters: string = '';
  Sorts: string = '';
  Page: number = 1;
  PageSize: number = 15;
}

export class CartItem {
  productId: string = '';
  productName: string = '';
  description?: string;
  imageContent: Uint8Array = new Uint8Array();
  price: number = 0;
  cartQuantity: number = 0;
  sellerName: string = '';
  stockQuantity: number = 0 ;
}

export class LoginModel {
  username: string = '';
  password: string = '';
}

export class RegisterModel {
  username: string = '';
  password: string = '';
  email: string = '';

  constructor(
    username: string = '',
    password: string = '',
    email: string = ''
  ) {
    this.username = username;
    this.password = password;
    this.email = email;
  }
}

export class LogoutModel {
  userId: string = '';
}

export class UserInfo {
  userId: string = '';
  username: string = '';
  email: string = '';
  lastLoginTime: string | null = '';
  lastLogoutTime: string | null = '';
}

export class UserTransaction {
  productName: string = '';
  quantity: number = 0;
  transactionDate: string = '';
  totalAmount: number = 0;
}

export class MakePurchase {
  productId: string;
  buyerId: string;
  quantity: number;

  constructor(productId: string, buyerId: string, quantity: number) {
    this.productId = productId;
    this.buyerId = buyerId;
    this.quantity = quantity;
  }
}


export class SalesData {
  message: string = '';
  data: {
    SellerId: string;
    username: string;
    Email: string;
    totalAmountSold: number;
    totalProductsSold: number;
    ItemsSold: {
      ProductId: string;
      ProductName: string;
      TotalQuantitySold: number;
      TotalAmountSold: number;
    }[];
  } = {
    SellerId: '',
    username: '',
    Email: '',
    totalAmountSold: 0,
    totalProductsSold: 0,
    ItemsSold: []
  };
}
