export interface IProduct {
  productId: string; // Guid
  productName: string; // Required
  description?: string; // Optional
  imageContent: Uint8Array; // byte[]
  price: number; // Double
  stockQuantity: number; // Default 0
  sellerId: string; // Guid
  createdAt: Date; // Default value
  updatedAt: Date; // Default value
}

export interface IRole {
  roleId: string; // Guid
  roleName: string; // Required
}

export interface ITransactionHistory {
  transactionId: string; // Guid
  productId: string; // Guid
  buyerId: string; // Guid
  quantity: number; // Required
  totalAmount: number; // Required
  transactionDate: Date; // Default value
}

export interface IUser {
  userId: string; // Guid
  username: string; // Required
  email: string; // Required
  passwordHash: string; // Required
}

export interface IUserAudit {
  userAuditId: string; // Guid
  userId: string; // Guid
  loginTime: Date; // Required
  logoutTime?: Date; // Optional
}

export interface IRefreshToken {
  tokenId: string; // Guid
  userId: string; // Guid
  token: string; // Required
  expiresAt: Date; // Expiry date
}

export interface IFilter {
  Filters: string;
  Sorts: string;
  Page: number;
  PageSize: number;
}

export interface ICartItem {
  productId: string;
  productName: string;
  description?: string;
  imageContent: Uint8Array
  price: number;
  cartQuantity: number;
  sellerName: string;
  stockQuantity: number;
}

export interface ILoginModel {
  username: string;
  password: string;
}

export interface IRegisterModel {
  username: string;
  password: string;
  email: string;
}

export interface ILogoutModel {
  userId: string;
}

export interface IUserInfo {
  userId: string; // Guid will be converted to string in TypeScript
  username: string;
  email: string;
  lastLoginTime: string | null; // ISO date string or null
  lastLogoutTime: string | null; // ISO date string or null
}
