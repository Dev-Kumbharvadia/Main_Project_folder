import { Routes } from '@angular/router';
import { LayoutComponent } from './component/layout/layout.component';
import { LoginComponent } from './component/login/login.component';
import { RegisterComponent } from './component/register/register.component';
import { HomeComponent } from './component/home/home.component';
import { AuditComponent } from './component/audit/audit.component';
import { CartComponent } from './component/cart/cart.component';
import { ManageUserComponent } from './component/manage-user/manage-user.component';
import { ProfileComponent } from './component/profile/profile.component';
import { AddProductComponent } from './component/add-product/add-product.component';
import { ProductListComponent } from './component/product-list/product-list.component';
import { UpdateProductComponent } from './component/update-product/update-product.component';
import { authGuard } from './guard/auth.guard';
import { RetailerStatsComponent } from './component/retailer-stats/retailer-stats.component';

export const routes: Routes = [
  {
    path: '',
    component: LoginComponent,
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'register',
    component: RegisterComponent,
  },
  {
    path: 'layout',
    component: LayoutComponent,
    canActivate: [authGuard],
    children: [
      {
        path: 'login',
        component: LoginComponent,
      },
      {
        path: 'register',
        component: RegisterComponent,
      },
      {
        path: 'home',
        component: HomeComponent,
      },
      {
        path: 'audit',
        component: AuditComponent,
      },
      {
        path: 'cart',
        component: CartComponent,
      },
      {
        path: 'manage-user',
        component: ManageUserComponent,
      },
      {
        path: 'cart',
        component: CartComponent,
      },
      {
        path: 'profile',
        component: ProfileComponent
      },
      {
        path: 'add-product',
        component: AddProductComponent
      },
      {
        path: 'product-list',
        component: ProductListComponent
      },
      {
        path: 'update-product',
        component: UpdateProductComponent
      },
      {
        path: 'stats',
        component: RetailerStatsComponent
      }
    ]
  },
];
