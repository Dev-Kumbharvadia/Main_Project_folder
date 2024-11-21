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
      email: ''
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
  export class User{
    userId: string = '';
    username: string = '';
    email: string = '';
    passwordHash: string = '';
  }
  export class UserAudit{
    userAuditId: string = '';
    userId: string = '';
    loginTime: Date = new Date();
    logoutTime?: Date;
  }
  
export class UserRole {
    userId: string = '';
    roleId: string = '';
  }