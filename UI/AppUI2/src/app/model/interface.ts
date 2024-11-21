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
