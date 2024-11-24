export const Constant = {
    API_METHOD: {
        ADMIN: {
            REWRITE_ROLES: '/api/Admin/RewriteRoles',
            DELETE: '/api/Admin/DeleteUser',
            GET_ALL: '/api/Admin/GetAllAudits',
            GET_BY_ID: '/api/Admin/GetAuditsByUserID',
        },
        AUTH: {
            LOGOUT: '/api/Auth/Logout',
            REGISTER: '/api/Auth/Register',
            LOGIN: '/api/Auth/Login',
            REFRESH_TOKEN: '/api/Auth/Login',
        },
        PRODUCT: {
            GET_ALL: '/api/Product/GetAllProducts',
            GET_SORTED: '/api/Product/Sorted',
            GET_BY_ID: '/api/Product/GetProductById',
            ADD: '/api/Product/AddProduct',
            UPDATE: '/api/Product/UpdateProduct',
            DELETE: '/api/Product/DeleteProduct',
        },
        ROLE: {
            GET_ALL: '/api/Role/GetAllRoles',
            GET_BY_ID: '/api/Role/GetUserRolesByID',
            ADD: '/api/Role/AddRole',
            REMOVE: '/api/Role/RemoveRole',
            UPDATE: '/api/Role/UpdateRole',
            DELETE: '/api/Role/DeleteRole',
        },
        TRANSACTION: {
            GET_ALL: '/api/Transaction/getAllTransaction',
            GET_BY_ID: '/api/Transaction/getAllTransactionById',
            MAKE_PURCHASE: '/api/Transaction/MakePurchase',
            GET_TRANSACTION_HISTORY: '/api/Transaction/getTransactionHistory',
            GET_TRANSACTION_HISTORY_BY_USER_ID: '/api/Transaction/getTransactionHistoryByUserId',
        },
        USER: {
            ASSIGN_ROLE: '/api/User/AssignRole',
            ASSIGN_ROLES: '/api/User/AssignRoles',
            ADD: '/api/User/AssignRole',
            GET_ALL: '/api/User/GetAllUsers',
            GET_ALL_INFO: '/api/User/GetAllUserInfo',
            GET_ALL_INFO_BY_ID: '/api/User/GetUserInfoById',
            GET_BY_ID: '/api/User/GetUserByID',
            UPDATE: '/api/User/UpdateUser',
        }
    }
}
