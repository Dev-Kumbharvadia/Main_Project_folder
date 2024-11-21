export const Constant = {
    API_METHOD: {
        PRODUCT: {
            GET_ALL: '/api/Product/GetAllProducts',
            GET_SORTED: '/api/Product/Sorted',
            GET_BY_ID: '/api/Product/GetProductById',
            ADD: '/api/Product/AddProduct',
            UPDATE: '/api/Product/UpdateProduct',
            DELETE: '/api/Product/DeleteProduct',
        },
        AUTH: {
            LOGOUT: '/api/Auth/Logout',
            REGISTER: '/api/Auth/Register',
            LOGIN: '/api/Auth/Login',
            REFRESH_TOKEN: '/api/Auth/Login',
        },
        CART: {
            PURCHASE: '',
        },
        AUDIT: {
            GET_ALL: '/api/Audit/GetAllAudits',
            GET_BY_ID: '/api/Audit/GetAuditsByUserID',
        },
        ROLE: {
            GET_ALL: '/api/Role/GetAllRoles',
            GET_BY_ID: '/api/Role/GetUserRolesByID',
            ADD: '/api/Role/AddRole',
            REMOVE: '/api/Role/RemoveRole',
            UPDATE: '/api/Role/UpdateRole',
        },
        USER: {
            ASSIGN_ROLE: '/api/User/AssignRole',
            REWRITE_ROLE: '/api/User/AssignRole',
            ADD: '/api/User/AssignRole',
            GET_ALL: '/api/User/GetAllUsers',
            GET_BY_ID: '/api/User/GetUserByID',
            UPDATE: '/api/User/UpdateUser',
            DELETE: '/api/User/UpdateUser',
        }
    }
}