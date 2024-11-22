export const Constant = {
    API_METHOD: {
        ADMIN: {
            REWRITE_ROLES: '/api/Admin/RewriteRoles',
        },
        AUDIT: {
            GET_ALL: '/api/Audit/GetAllAudits',
            GET_BY_ID: '/api/Audit/GetAuditsByUserID',
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
        },
        TRANSACTION: {
            GET_ALL: '/api/Transaction/getAllTransaction',
            GET_BY_ID: '/api/Transaction/getAllTransaction',
            MAKE_PURCHASE: '/api/Transaction/MakePurchase',
        },
        USER: {
            ASSIGN_ROLE: '/api/User/AssignRole',
            ASSIGN_ROLES: '/api/User/AssignRoles',
            ADD: '/api/User/AssignRole',
            GET_ALL: '/api/User/GetAllUsers',
            GET_BY_ID: '/api/User/GetUserByID',
            UPDATE: '/api/User/UpdateUser',
            DELETE: '/api/User/UpdateUser',
        }
    }
}