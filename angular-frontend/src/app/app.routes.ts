import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth';
import { roleGuard } from './core/guards/role';


export const routes: Routes = [
    { path: '', loadComponent: () => import('./features/home/home').then(m => m.HomeComponent) },
    { path: 'menu', loadComponent: () => import('./features/menu/menu').then(m => m.MenuComponent) },
    {
        path: 'food/:id', loadComponent: () => import('./features/food-detail/food-detail').then(m =>
            m.FoodDetailComponent)
    },
    { path: 'auth/login', loadComponent: () => import('./features/auth/login/login').then(m => m.LoginComponent) },
    {
        path: 'auth/register', loadComponent: () => import('./features/auth/register/register').then(m =>
            m.RegisterComponent)
    },
    {
        path: 'auth/register-driver', loadComponent: () => import('./features/auth/register-driver/register-driver').then(m =>
            m.RegisterDriverComponent)
    },
    {
        path: 'cart', canActivate: [authGuard, roleGuard(['Customer'])],
        loadComponent: () => import('./features/cart/cart').then(m => m.CartComponent)
    },
    {
        path: 'checkout', canActivate: [authGuard, roleGuard(['Customer'])],
        loadComponent: () => import('./features/checkout/checkout').then(m => m.CheckoutComponent)
    },
    {
        path: 'addresses', canActivate: [authGuard, roleGuard(['Customer'])],
        loadComponent: () => import('./features/addresses/addresses').then(m => m.AddressesComponent)
    },
    {
        path: 'orders', canActivate: [authGuard],
        loadComponent: () => import('./features/orders/orders').then(m => m.OrdersComponent)
    },
    {
        path: 'orders/:id', canActivate: [authGuard],
        loadComponent: () => import('./features/order-detail/order-detail').then(m => m.OrderDetailComponent)
    },
    {
        path: 'profile', canActivate: [authGuard],
        loadComponent: () => import('./features/profile/profile').then(m => m.ProfileComponent)
    },
    {
        path: 'driver', canActivate: [authGuard, roleGuard(['Driver'])],
        loadComponent: () => import('./features/driver/driver-dashboard').then(m => m.DriverDashboardComponent)
    },
    {
        path: 'admin',
        canActivate: [authGuard, roleGuard(['Admin'])],
        loadComponent: () => import('./features/admin/admin-layout/admin-layout').then(m => m.AdminLayoutComponent),
        children: [
            { path: '', redirectTo: 'orders', pathMatch: 'full' },
            {
                path: 'orders', loadComponent: () => import('./features/admin/orders/admin-orders').then(m =>
                    m.AdminOrdersComponent)
            },
            {
                path: 'categories', loadComponent: () => import('./features/admin/categories/admin-categories').then(m =>
                    m.AdminCategoriesComponent)
            },
            {
                path: 'foods', loadComponent: () => import('./features/admin/foods/admin-foods').then(m =>
                    m.AdminFoodsComponent)
            },
            {
                path: 'payment-methods', loadComponent: () => import('./features/admin/payment-methods/admin-payment-methods').then(m =>
                    m.AdminPaymentMethodsComponent)
            },
            {
                path: 'drivers', loadComponent: () => import('./features/admin/drivers/admin-drivers').then(m =>
                    m.AdminDriversComponent)
            },
            {
                path: 'users', loadComponent: () => import('./features/admin/users/admin-users').then(m =>
                    m.AdminUsersComponent)
            },
        ]
    },
    { path: '**', redirectTo: '' }
];
