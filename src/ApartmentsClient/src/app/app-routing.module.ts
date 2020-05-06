import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ApartmentSearchComponent } from './apartment-search/apartment-search.component';
import { ProfileComponent } from './profile/profile.component';
import { LoginComponent } from './auth/login/login.component';
import { ApartmentDetailComponent } from './userCapabilities/apartment-detail/apartment-detail.component';

import { RegisterComponent } from './auth/register/register.component';
import { AuthGuard } from './auth/auth.guard';

import { ApartmentCreateComponent } from './userCapabilities/apartment-create/apartment-create.component';
import { CommentDetailComponent } from './userCapabilities/comment-detail/comment-detail.component';
import { OrderDetailComponent } from './userCapabilities/order-detail/order-detail.component';
import { AdminPanetComponent } from './admin-panet/admin-panet.component';
import { ProfileDetailComponent } from './admin-panet/profile-detail/profile-detail.component';


const routes: Routes = [
  { path: '', redirectTo: '/search', pathMatch: 'full' },
  { path: 'search', component: ApartmentSearchComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard]},
  { path: 'apartment/:id', component: ApartmentDetailComponent, canActivate: [AuthGuard] },
  { path: 'addapartment', component: ApartmentCreateComponent, canActivate: [AuthGuard] },
  { path: 'comment/:id', component: CommentDetailComponent, canActivate: [AuthGuard] },
  { path: 'order/:id', component: OrderDetailComponent, canActivate: [AuthGuard] },
  { path: 'admin', component: AdminPanetComponent, canActivate: [AuthGuard] },
  { path: 'user/:id', component: ProfileDetailComponent, canActivate: [AuthGuard] }




];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
