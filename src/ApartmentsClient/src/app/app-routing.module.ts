import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { AuthGuard } from './auth/auth.guard';
import { SearchComponent } from './search/search.component';
import { SearchViewComponent } from './search/search-view/search-view.component';
import { ProfilePanelComponent } from './profile/profile-panel/profile-panel.component';
import { SearchApartmentDetailComponent } from './search/search-apartment-detail/search-apartment-detail.component';
import { OrderDetailComponent } from './user/order/order-detail/order-detail.component';
import { OwnOrdersComponent } from './user/order/own-orders/own-orders.component';

const routes: Routes = [
  { path: '', redirectTo: '/search', pathMatch: 'full' },
  { path: 'search', 
  component: SearchComponent,
  children: [
    {path: 'apartments', component: SearchViewComponent},
    {path: 'apartment/:id', component: SearchApartmentDetailComponent, canActivate: [AuthGuard]}
  ]},
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'profile', component: ProfilePanelComponent },
  { path: 'orders', component: OwnOrdersComponent },
  { path: 'order/:id', component: OrderDetailComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }