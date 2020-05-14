import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { AuthGuard } from './auth/auth.guard';
import { SearchComponent } from './search/search.component';
import { SearchViewComponent } from './search/search-view/search-view.component';
import { ProfilePanelComponent } from './profile/profile-panel/profile-panel.component';
import { SearchApartmentDetailComponent } from './search/search-apartment-detail/search-apartment-detail.component';
import { OwnOrderDetailComponent } from './user/order/own-order-detail/own-order-detail.component';
import { OwnOrdersComponent } from './user/order/own-orders/own-orders.component';
import { OwnCommentsComponent } from './user/comment/own-comments/own-comments.component';
import { OwnCommentDetailComponent } from './user/comment/own-comment-detail/own-comment-detail.component';
import { OwnApartmentsComponent } from './user/apartment/own-apartments/own-apartments.component';
import { OwnApartmentDetailComponent } from './user/apartment/own-apartment-detail/own-apartment-detail.component';
import { AddApartmentComponent } from './user/apartment/add-apartment/add-apartment.component';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { ProfileAdministrationDetailComponent } from './admin/profile-administration-detail/profile-administration-detail.component';

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
  { path: 'profile', component: ProfilePanelComponent, canActivate: [AuthGuard] },
  { path: 'orders', component: OwnOrdersComponent, canActivate: [AuthGuard] },
  { path: 'order/:id', component: OwnOrderDetailComponent, canActivate: [AuthGuard] },
  { path: 'comments', component: OwnCommentsComponent, canActivate: [AuthGuard] },
  { path: 'comment/:id', component: OwnCommentDetailComponent, canActivate: [AuthGuard] },
  { path: 'aparments', component: OwnApartmentsComponent, canActivate: [AuthGuard] },
  { path: 'apartment/:id', component: OwnApartmentDetailComponent, canActivate: [AuthGuard] },
  { path: 'addapartment', component: AddApartmentComponent, canActivate: [AuthGuard] },
  { path: 'admin', component: AdminPanelComponent, canActivate: [AuthGuard] },
  { path: 'user/:id', component: ProfileAdministrationDetailComponent, canActivate: [AuthGuard] }
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }