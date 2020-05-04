import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ApartmentSearchComponent } from './apartment-search/apartment-search.component';
import { ProfileComponent } from './profile/profile.component';
import { LoginComponent } from './auth/login/login.component';
import { ApartmentDetailComponent } from './userCapabilities/apartment-detail/apartment-detail.component';

import { RegisterComponent } from './auth/register/register.component';
import { AuthGuard } from './auth/auth.guard';

import { ApartmentCreateComponent } from './userCapabilities/apartment-create/apartment-create.component';

const routes: Routes = [
  { path: '', redirectTo: '/search', pathMatch: 'full' },
  { path: 'search', component: ApartmentSearchComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard]},
  { path: 'apartment/:id', component: ApartmentDetailComponent, canActivate: [AuthGuard] },
  { path: 'addapartment', component: ApartmentCreateComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
